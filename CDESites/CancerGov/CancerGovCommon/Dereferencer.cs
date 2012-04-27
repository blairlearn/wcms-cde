using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.Common
{
	/// <summary>
	/// Summary description for Dereferencer.
	/// </summary>
	public class Dereferencer
	{
		private string text;
		private Hashtable refHash;
		private string contentType;
		private string viewId;
	
		#region Class Properties
	
		public string Text 
		{
			set {text = value;}
		}

		public string ViewId 
		{
			set {viewId = value;}
		}

		#endregion

		/// <summary>
		/// Class constructor specifying
		/// </summary>
		/// <param name="nCIViewId"></param>
		/// <param name="version"></param>
		public Dereferencer(string version)
		{
			contentType = version;
			refHash = new Hashtable();
		}

		/// <summary>
		/// Default class constructor
		/// </summary>
		public Dereferencer()
		{
			refHash = new Hashtable();
		}

		public Dereferencer(string version, string viewId) 
		{
			this.contentType = version;
			this.viewId = viewId;
			refHash = new Hashtable();
		}

		/// <summary>
		/// Method defines reference tag expression and replaces matches with resolved HTML
		/// </summary>
		/// <param name="text">Input containing reference tags</param>
		/// <returns>Input with reference tags resolved to HTML</returns>
		public string Deref(string text)
		{
			Regex expRefTag = new Regex("<ref\\s(?<attributes>.+?)/>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
			
			//Top-down resolution of reference tags
			while(expRefTag.IsMatch(text))
			{
				text = expRefTag.Replace(text, new MatchEvaluator(ResolveRef));
			}

			return text;
		}

		/// <summary>
		/// Delegate method handling reference tag matches, parses tag, builds appropriate
		/// HTML by type of reference tag
		/// </summary>
		/// <param name="mRef">reference tag match</param>
		/// <returns>HTML resolution of reference tag</returns>
		private string ResolveRef(Match mRef)
		{
			string result = "";
			string passThroughAttr = "";

			Regex nameValueExp = new Regex("(?<name>[^\\s].+?)=\"(?<value>.+?)\"");
			MatchCollection nameValuePairs = nameValueExp.Matches(mRef.Groups["attributes"].Value.Trim());
			
			foreach(Match nameValue in nameValuePairs)
			{
				refHash.Add(nameValue.Groups["name"].Value, nameValue.Groups["value"].Value);
			}
			
			//resolve reference tag
			if(refHash.ContainsKey("type"))
			{
				DataTable objTable = new DataTable();
						
				switch(refHash["type"].ToString().ToLower())
				{
					case "image":
						//Get object data from database

						SqlDataAdapter imgAdapter = null;
						try {
                            imgAdapter = new SqlDataAdapter("usp_GetImageViewObject", ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
							imgAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
							imgAdapter.SelectCommand.Parameters.Add("@ImageName",refHash["imagename"].ToString());
							imgAdapter.Fill(objTable);
						}
						catch(SqlException sqlE) {
							CancerGovError.LogError("", this.ToString(), ErrorType.DbUnavailable, sqlE);
						} finally {
							if (imgAdapter != null) {
								imgAdapter.Dispose();
							}
						}

						//Get custom passthrough attributes, ignoring doctagging specific attributes
						refHash.Remove("type");
						refHash.Remove("imagename");

						//At some point someone should make this a foreach statement.
						//I just do not want to have to test it in this release.
						//actually, someone should just rewrite all of this.
						IEnumerator refHashEnum = refHash.Keys.GetEnumerator();
						while(refHashEnum.MoveNext())
						{
							passThroughAttr += " " + refHashEnum.Current.ToString() + "=\"" + refHash[refHashEnum.Current.ToString()] + "\"";
						}

						refHash.Clear();


						//Format HTML output
						if(objTable.Rows.Count > 0)
						{
							DataRow dbRow = objTable.Rows[0];
							
							switch(contentType)
							{
								case ContentTypes.Image:
									result = "<img src=\"" + dbRow["ImageSource"].ToString().Trim() + "\" alt=\"" + (Functions.HasValue(dbRow["ImageAltText"].ToString()) ? dbRow["ImageAltText"].ToString().Trim() : "") + "\" border=\"" + (Functions.HasValue(dbRow["Border"].ToString()) ? dbRow["Border"].ToString().Trim() : "0") + "\"" + (Functions.HasValue(dbRow["Width"].ToString()) ? " width=\"" + dbRow["Width"].ToString() + "\"" : "") + (Functions.HasValue(dbRow["Height"].ToString()) ? " height=\"" + dbRow["Height"].ToString() + "\"" : "") + passThroughAttr + ">";
									break;
								case ContentTypes.TextOnly:
									result = dbRow["TextSource"].ToString();
									break;
							}
						
							string url = dbRow["Url"].ToString();

							if(Functions.HasValue(url))
							{
								if(dbRow["ClickloggingEnabled"] != null && Functions.HasValue(dbRow["ClickloggingEnabled"].ToString()) && dbRow["ClickLoggingEnabled"].ToString().Trim().ToUpper() == "YES")
								{
									result = "<a href=\"/common/clickpassthrough.aspx?redirectUrl=" + HttpUtility.UrlEncode(url) + "&clickItem=viewId:" + HttpUtility.UrlEncode(dbRow["NCIViewId"].ToString()) + ";imageSrc" + HttpUtility.UrlEncode(dbRow["ImageSource"].ToString()) + "\" style=\"text-decoration:none;\" OnMouseOver=\"" + Functions.GetMouseOverStatusAttribute(url) + "\" OnMouseOut=\"" + Functions.GetMouseOutStatusAttribute() + " \">" + result + "</a>";
								}
								else
								{
									result = "<a href=\"" + url + "\" style=\"text-decoration:none;\">" + result + "</a>";
								}
							}							
						}

						objTable.Clear();

						break;
					case "longdateimage":
						string baseSrc = "";

						if(refHash.ContainsKey("basesrc"))
						{
							baseSrc = refHash["basesrc"].ToString();
						}

						if(contentType == ContentTypes.Image)
						{
							result = "<img src=\"/images/dates/" + baseSrc + DateTime.Today.ToString("MMMM") + ".gif\" alt=\"" + DateTime.Today.ToString("MMMM") + "\" border=\"0\">";
							result += "<img src=\"/images/dates/" + baseSrc + DateTime.Today.Day.ToString() + ".gif\" alt=\"" + DateTime.Today.Day.ToString() + "\" border=\"0\">";
							result += "<img src=\"/images/dates/" + baseSrc + DateTime.Today.Year.ToString() + ".gif\" alt=\"" + DateTime.Today.Year.ToString() + "\" border=\"0\">";
						}
						else
						{
							result = DateTime.Today.ToString("MMMM dd, yyyy");								
						}						
						break;

						//This allows for a DB date to show up in a content header
						//Or today's date
					case "date" : 
					{
						string strDateType = "";
						string strDateString = "";

						int iDay = 0;
						int iMonth = 0;
						int iYear = 0;
						int iDayOfWeek = 0;

						string strDayNum1 = "";
						string strDayNum2 = "";
						string strYearNum4 = "";
						string strYearNum2 = "";
						string strMonthNum1 = "";
						string strMonthNum2 = "";
						string strMonthWord3 = "";
						string strMonthFull = "";
						string strDayOfWeek = "";

						if(refHash.ContainsKey("datetype")) 
						{
							strDateType = refHash["datetype"].ToString();
						}

						if(refHash.ContainsKey("datestring")) 
						{
							strDateString = refHash["datestring"].ToString();
						}

						//Check to make sure the guid is valid and that the datetype is set
						if ((Functions.IsGuid(viewId)) && (strDateType != "")) 
						{

							if (strDateType.ToUpper() != "TODAY") 
							{
								SqlDataAdapter daDates = null;
								//Grab all types of dates out of the db
								try {
                                    daDates = new SqlDataAdapter("usp_GetViewDates", ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);

									daDates.SelectCommand.CommandType = CommandType.StoredProcedure;
									daDates.SelectCommand.Parameters.Add("@ViewID", viewId);
									daDates.Fill(objTable);
								}
								catch(SqlException sqlE) {
									CancerGovError.LogError("", this.ToString(), ErrorType.DbUnavailable, sqlE);
								} finally {
									if (daDates != null) {
										daDates.Dispose();
									}
								}
							
							
								//Set the date depending on what date is chosen
								if ((objTable != null) && (objTable.Rows.Count > 0)) 
								{
									switch (strDateType.ToUpper()) 
									{
										case "POSTED" : 
										{
											try 
											{
												iDay = Convert.ToInt32(objTable.Rows[0]["PostedDateDay"].ToString());
											} 
											catch 
											{
												iDay = -1;
											}

											try 
											{
												iMonth = Convert.ToInt32(objTable.Rows[0]["PostedDateMonth"].ToString());
											} 
											catch 
											{
												iMonth = -1;
											}
										
											try 
											{
												iYear = Convert.ToInt32(objTable.Rows[0]["PostedDateYear"].ToString());
											} 
											catch 
											{
												iYear = -1; 
											}

											try 
											{
												iDayOfWeek = Convert.ToInt32(objTable.Rows[0]["PostedDateDayOfWeek"].ToString());
											} 
											catch 
											{
												iDayOfWeek = -1;
											}

											break;
										}
										case "UPDATE" : 
										{
											try 
											{
												iDay = Convert.ToInt32(objTable.Rows[0]["ReleaseDateDay"].ToString());
											} 
											catch 
											{
												iDay = -1;
											}

											try 
											{
												iMonth = Convert.ToInt32(objTable.Rows[0]["ReleaseDateMonth"].ToString());
											} 
											catch 
											{
												iMonth = -1;
											}
										
											try 
											{
												iYear = Convert.ToInt32(objTable.Rows[0]["ReleaseDateYear"].ToString());
											} 
											catch 
											{
												iYear = -1; 
											}

											try 
											{
												iDayOfWeek = Convert.ToInt32(objTable.Rows[0]["ReleaseDateDayOfWeek"].ToString());
											} 
											catch 
											{
												iDayOfWeek = -1;
											}

											break;
										}
										case "EXPIRATION" : 
										{

											try 
											{
												iDay = Convert.ToInt32(objTable.Rows[0]["ExpirationDateDay"].ToString());
											} 
											catch 
											{
												iDay = -1;
											}

											try 
											{
												iMonth = Convert.ToInt32(objTable.Rows[0]["ExpirationDateMonth"].ToString());
											} 
											catch 
											{
												iMonth = -1;
											}
										
											try 
											{
												iYear = Convert.ToInt32(objTable.Rows[0]["ExpirationDateYear"].ToString());
											} 
											catch 
											{
												iYear = -1; 
											}

											try 
											{
												iDayOfWeek = Convert.ToInt32(objTable.Rows[0]["ExpirationDateDayOfWeek"].ToString());
											} 
											catch 
											{
												iDayOfWeek = -1;
											}

											break;
										}

									}
								}
							} 
							else 
							{
								iDay = DateTime.Now.Day;
								iMonth = DateTime.Now.Month;
								iYear = DateTime.Now.Year;
								iDayOfWeek = 2;

							}
							//Now translate this to strings for replacing
							if (iDay != -1) 
							{

								strDayNum1 = iDay.ToString();
								if (iDay < 10) 
								{
									strDayNum2 = "0" + iDay.ToString();
								} 
								else 
								{
									strDayNum2 = iDay.ToString();
								}
							}

							if (iMonth != -1) 
							{
								strMonthNum1 = iMonth.ToString();
								
								if (iMonth < 10) 
								{
									strMonthNum2 = "0" + iMonth.ToString();
								} 
								else 
								{
									strMonthNum2 = iMonth.ToString();
								}

								//Maybe 2 hash tables??
								switch (iMonth) 
								{
									case 1 : 
										strMonthWord3 = "Jan"; 
										strMonthFull = "January";
										break;
									case 2 : 
										strMonthWord3 = "Feb"; 
										strMonthFull = "February";
										break;
									case 3 : 
										strMonthWord3 = "Mar"; 
										strMonthFull = "March";
										break;
									case 4 :
										strMonthWord3 = "Apr"; 
										strMonthFull = "April";
										break;
									case 5 :
										strMonthWord3 = "May"; 
										strMonthFull = "May";
										break;
									case 6 :
										strMonthWord3 = "Jun"; 
										strMonthFull = "June";
										break;
									case 7 :
										strMonthWord3 = "Jul"; 
										strMonthFull = "July";
										break;
									case 8 :
										strMonthWord3 = "Aug"; 
										strMonthFull = "August";
										break;
									case 9 :
										strMonthWord3 = "Sep"; 
										strMonthFull = "September";
										break;
									case 10 :
										strMonthWord3 = "Oct"; 
										strMonthFull = "October";
										break;
									case 11 :
										strMonthWord3 = "Nov"; 
										strMonthFull = "November";
										break;
									case 12 :
										strMonthWord3 = "Dec"; 
										strMonthFull = "December";
										break;
								}
							}

							if (iYear != -1) 
							{
								strYearNum4 = iYear.ToString();
								strYearNum2 = iYear.ToString().Substring(2,2);
							}

							if (iDayOfWeek != -1) 
							{
								switch(iDayOfWeek) 
								{
									case 1 : 
										strDayOfWeek = "Sunday";
										break;
									case 2 : 
										strDayOfWeek = "Monday";
										break;
									case 3 : 
										strDayOfWeek = "Tuesday";
										break;
									case 4 : 
										strDayOfWeek = "Wednesday";
										break;
									case 5 : 
										strDayOfWeek = "Thursday";
										break;
									case 6 : 
										strDayOfWeek = "Friday";
										break;
									case 7 : 
										strDayOfWeek = "Saturday";
										break;

								}
							}

							//Now replace the placeholders
							strDateString = Regex.Replace(strDateString,"DayNum1",strDayNum1,RegexOptions.IgnoreCase);
							strDateString = Regex.Replace(strDateString,"DayNum2",strDayNum2,RegexOptions.IgnoreCase);
							strDateString = Regex.Replace(strDateString,"MonthNum1",strMonthNum1,RegexOptions.IgnoreCase);
							strDateString = Regex.Replace(strDateString,"MonthNum2",strMonthNum2,RegexOptions.IgnoreCase);
							strDateString = Regex.Replace(strDateString,"MonthWord3",strMonthWord3,RegexOptions.IgnoreCase);
							strDateString = Regex.Replace(strDateString,"MonthWordFull",strMonthFull,RegexOptions.IgnoreCase);
							strDateString = Regex.Replace(strDateString,"YearNum2",strYearNum2,RegexOptions.IgnoreCase);
							strDateString = Regex.Replace(strDateString,"YearNum4",strYearNum4,RegexOptions.IgnoreCase);
							strDateString = Regex.Replace(strDateString,"DayOfWeek",strDayOfWeek,RegexOptions.IgnoreCase);
							
							result = strDateString;
						} 
						else 
						{
							//Error message (not valid guid or no dates type)
							if (strDateType == "") 
							{
								CancerGovError.LogError("","Dereferencer.cs",2,"date: datetype not defined");
							} 
							else 
							{
								CancerGovError.LogError("","Dereferencer.cs",2,"date: invalid guid");
							}
						}

						break;
					}

					//The problem is determining the print template... grr...
					//Actually this needs to be rewritten.  I guess we have not used this one since the
					//redesign.  I am not going to touch this one for SCR 1002. --BryanP
					case "printversionicon" : 
					{
						 
						if (Functions.IsGuid(viewId)) 
						{

							string strIsPrintAvailable = "no";

							strIsPrintAvailable = Functions.IfBlank(Functions.GetViewProperty(viewId,"IsPrintAvailable"),"no");
							 
							if (strIsPrintAvailable.ToUpper() == "YES") 
							{
								
								//Get the name of the print template

                                SqlConnection scnPrint = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
								SqlCommand scPrint = new SqlCommand("usp_GetPrintTemplate @viewid='" + viewId + "'",scnPrint);
								
								string strPrintTemplate = "";

								try 
								{
									scPrint.Connection.Open();
									strPrintTemplate = (string) scPrint.ExecuteScalar();
									scPrint.Connection.Close();
								} 
								catch (SqlException eSql) 
								{
									CancerGovError.LogError("", this.ToString(), ErrorType.DbUnavailable, eSql);
								}

								if (contentType == ContentTypes.Image) 
								{
									if (refHash.ContainsKey("imagefile")) 
									{
										result = "<a href=\"/templates/" + strPrintTemplate + "?viewid=" + viewId + "\"><img src=\"" + refHash["imagefile"].ToString() + "\" border=\"0\" alt=\"Printable Version\"></a>";
									} 
									else 
									{
										CancerGovError.LogError("","Dereferencer.cs",2,"printversionicon: no imagefile defined");
									}
								} 
								else 
								{ //text
									result = "<a href=\"/templates/" + strPrintTemplate + "?viewid=" + viewId + "\">Printable Version</a>";
								}

							}

						} 
						else 
						{
							CancerGovError.LogError("","Dereferencer.cs",2,"printversionicon: Invalid Guid");
						}
						break;
					}

						//This is for random rotating images
					case "randomimage" : 
					{

						string strImageList = "";

						if (refHash.ContainsKey("imagelist")) 
						{
							strImageList = refHash["imagelist"].ToString();
						}
												
						if (strImageList != "") 
						{
							string[] strarrImages = strImageList.Split(',');

							Random rdmNum = new Random();

							int iIndex = Convert.ToInt32(Math.Round(rdmNum.NextDouble() * (strarrImages.Length - 1)));
							

							result = "<img src=\"" + strarrImages[iIndex] + "\"  alt=\"\">";

						}

						break;
					}
					case "displayviewproperty" : {
						
						if (Functions.IsGuid(viewId)) {
							if (refHash.Contains("propertyname")) {
								result = Functions.GetViewProperty(viewId,refHash["propertyname"].ToString());
							}
						}

						break;
					}

					case "headtitle":
					{

						if (Functions.IsGuid(viewId)) 
						{
                            string strConnString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

							System.Data.SqlClient.SqlConnection scnComment = new System.Data.SqlClient.SqlConnection(strConnString);
							System.Data.SqlClient.SqlCommand scComment = new System.Data.SqlClient.SqlCommand();
							scComment.Connection = scnComment;
                  
                  
							scComment.Connection.Open();
							scComment.CommandText = "select title from nciview where nciviewid = '" + viewId + "'";
					
							//object title = null;
							try 
							{
								result = scComment.ExecuteScalar().ToString();
							}
							catch
							{
								CancerGovError.LogError("","Dereferencer.cs",2,"headtitle: Title not returned");
								result = "";
							}


							/*if (title != null) 
							{
								result = title.ToString();
							} */

							//result = tempView.Title;
						}
						else 
						{
							CancerGovError.LogError("","Dereferencer.cs",2,"headtitle: Invalid Guid");
						}
						break;
					}

					case "unknown type":
						//log it
						break;
				}
			}
			else
			{
				//invalid reference tag.  log it.

			}

			refHash.Clear();
			return result;
		}
	}
}
