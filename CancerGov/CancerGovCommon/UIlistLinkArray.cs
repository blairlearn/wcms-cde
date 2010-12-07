using System;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using CancerGov.Common;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.Common
{
	///<summary>
	///Builds custom ArrayList of hyperlinks based on a list id and 
	///exposed list properties.<br/>
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  8-8-2001<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	///<br/>
	///</summary>
	public class ListLinkArray : System.Collections.ArrayList
	{
		private string listId;
		private bool showFeatured = true;
		private bool showDescription = false;
		private bool showMultiSourced = false;
		private int releaseDateStyle = -1;
		private string titleField = "ShortTitle";
				
		/// <summary>
		/// Constructor for creating a populated ArrayList object showing anchor and featured 
		/// formatting for list items.
		/// </summary>
		/// <param name="listId">List containing the list items to be formatted.</param>
		public ListLinkArray(string paramListId)
		{
			listId = paramListId;
			GetListLinkArray();
		}

		/// <summary>
		/// Constructor for empty object to allow customization of properties prior to 
		/// population of ArrayList.
		/// </summary>
		public ListLinkArray()
		{
			
		}

		#region ListLinkArray properties
		
		/// <summary>
		/// Indicates the list to be formatted
		/// </summary>
		public string ListId
		{
			get {return listId;}
			set {listId = value;}
		}

		/// <summary>
		/// Indicates featured list items receive special formatting
		/// </summary>
		public bool ShowFeatured
		{
			get {return showFeatured;}
			set {showFeatured = value;}
		}

		/// <summary>
		/// Indicates list item description is displayed after link
		/// </summary>
		public bool ShowDescription
		{
			get {return showDescription;}
			set {showDescription = value;}
		}

		/// <summary>
		/// For PDQ Summary list items only, indicates a link for each
		/// available version of the PDQ Summary be generated
		/// </summary>
		public bool ShowMultiSourced
		{
			get {return showMultiSourced;}
			set {showMultiSourced = value;}
		}

		/// <summary>
		/// Indicates the location of the list item date information 
		/// </summary>
		public int ReleaseDateStyle
		{
			get {return releaseDateStyle;}
			set {releaseDateStyle = value;}
		}

		/// <summary>
		/// Indicates the NCIView field that represents the list item title
		/// </summary>
		public string TitleField
		{
			get {return titleField;}
			set {titleField = value;}
		}									   

		#endregion

		#region Date methods

		/// <summary>
		/// Verifies that the argument is a valid date and later than 1980 
		/// (which was used as a default date for NCIView date fields).
		/// </summary>
		/// <param name="dbDate">Date from a NCIView date field</param>
		/// <returns></returns>		
		private bool ValidDate(string dbDate)
		{
			bool result = false;

			try
			{
				DateTime dt = Convert.ToDateTime(dbDate);
							
				if(dt.Year > 1980)
				{
					result = true;		
				}
			}
			catch(InvalidCastException)
			{
				//don't show date, but no error trap
			}

			return result;
		}

		/// <summary>
		/// Gets date information for list item from NCIView record using 
		/// DisplayDateMode field and PostedDate and/or ReleaseDate fields.
		/// </summary>
		/// <param name="dbRow">NCIView record containing date mode field and date fields</param>
		/// <param name="dateSuffix">String to be added after date string</param>
		/// <returns>Date string in parentheses followed by dateSuffix string</returns>
		private string GetDates(DataRow dbRow, string dateSuffix)
		{
			string result = "";

			switch(int.Parse(dbRow["DisplayDateMode"].ToString().Trim()))
			{
				case 0:
					break;
				case 1:
					if(ValidDate(dbRow["PostedDate"].ToString().Trim()))
					{
						result += "Posted:  " + dbRow["PostedDate"].ToString().Trim();
					}
					break;
				case 2:
					if(ValidDate(dbRow["ReleaseDate"].ToString().Trim()))
					{
						result += "Updated:  " + dbRow["ReleaseDate"].ToString().Trim();
					}
					break;
				case 3:
					if(ValidDate(dbRow["PostedDate"].ToString().Trim()))
					{
						result += "Posted:  " + dbRow["PostedDate"].ToString().Trim();
					}
					if(ValidDate(dbRow["ReleaseDate"].ToString().Trim()))
					{
						if(result.Trim().Length > 0)
						{
							result += ", ";
						}
						result += "Updated:  " + dbRow["ReleaseDate"].ToString().Trim();
					}
					break;
				case 4:
					if(ValidDate(dbRow["ReviewedDate"].ToString().Trim()))
					{
						result += "Reviewed:  " + dbRow["ReviewedDate"].ToString().Trim();
					}
					break;
				case 5:
					if(ValidDate(dbRow["PostedDate"].ToString().Trim()))
					{
						result += "Posted:  " + dbRow["PostedDate"].ToString().Trim();
					}
					if(ValidDate(dbRow["ReviewedDate"].ToString().Trim()))
					{
						if(result.Trim().Length > 0)
						{
							result += ", ";
						}
						result += "Reviewed:  " + dbRow["ReviewedDate"].ToString().Trim();
					}
					break;
				case 6:
					if(ValidDate(dbRow["ReleaseDate"].ToString().Trim()))
					{
						result += "Updated:  " + dbRow["ReleaseDate"].ToString().Trim();
					}
					if(ValidDate(dbRow["ReviewedDate"].ToString().Trim()))
					{
						if(result.Trim().Length > 0)
						{
							result += ", ";
						}
						result += "Reviewed:  " + dbRow["ReviewedDate"].ToString().Trim();
					}
					break;
				case 7:
					if(ValidDate(dbRow["PostedDate"].ToString().Trim()))
					{
						result += "Posted:  " + dbRow["PostedDate"].ToString().Trim();
					}
					if(ValidDate(dbRow["ReleaseDate"].ToString().Trim()))
					{
						if(result.Trim().Length > 0)
						{
							result += ", ";
						}
						result += "Updated:  " + dbRow["ReleaseDate"].ToString().Trim();
					}
					if(ValidDate(dbRow["ReviewedDate"].ToString().Trim()))
					{
						if(result.Trim().Length > 0)
						{
							result += ", ";
						}
						result += "Reviewed:  " + dbRow["ReviewedDate"].ToString().Trim();
					}
					break;

			}

			#region old date setting
			/*
			switch(dbRow["DisplayDateMode"].ToString().Trim())
			{
				case "none":
					break;
				case "posted":
					if(ValidDate(dbRow["PostedDate"].ToString().Trim()))
					{
						result += "Posted:  " + dbRow["PostedDate"].ToString().Trim();
					}
					break;
				case "updated":
					if(ValidDate(dbRow["ReleaseDate"].ToString().Trim()))
					{
						result += "Updated:  " + dbRow["ReleaseDate"].ToString().Trim();
					}
					break;
				case "both":
					if(ValidDate(dbRow["PostedDate"].ToString().Trim()))
					{
						result += "Posted:  " + dbRow["PostedDate"].ToString().Trim();
					}
					if(ValidDate(dbRow["ReleaseDate"].ToString().Trim()))
					{
						if(result.Trim().Length > 0)
						{
							result += ", ";
						}
						result += "Updated:  " + dbRow["ReleaseDate"].ToString().Trim();
					}
					break;
			}
			*/
			#endregion

			if(result.Trim().Length > 0)
			{
				result = "(" + result + ")" + dateSuffix;
			}

			return result;
		}
		
		#endregion

		/// <summary>
		/// Builds ArrayList based on class properties
		/// </summary>
		public void GetListLinkArray()
		{
			if(Functions.IsGuid(listId))
			{
				string temp = "";
				DataTable dbTable = new DataTable();

				try
				{
                    SqlDataAdapter dbAdapter = new SqlDataAdapter("usp_GetListItems @ListId='" + listId + "'", ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
					dbAdapter.Fill(dbTable);
				}
				catch(SqlException sqlE)
				{
					CancerGovError.LogError("", this.ToString(), ErrorType.DbUnavailable, sqlE);
				}

				string url;
				string urlAttributes;
				string urlText;
				string text;
                				
				foreach(DataRow dbRow in dbTable.Rows)
				{
					temp = "";
					url = "";
					urlAttributes = "";
					urlText = "";
					text = "";
				
					if(dbRow["IsMultiSourced"].ToString().Trim() == "2" && showMultiSourced == true)
					{
						temp += dbRow[titleField];
						if(releaseDateStyle == 1)
						{
							temp += " " + GetDates(dbRow, "");
						}
						
						temp += "<br>";
						
						temp += "<span class=\"graytext\">[&nbsp;</span>";
						if(Functions.HasValue(dbRow["PrettyUrl"].ToString()))
						{
							temp += "<a class=\"gray\" href=\"" + dbRow["PrettyUrl"] + "/patient/\">" + (Functions.IsPropertySet(dbRow["NCIViewID"].ToString(), "IsSpanishContent") ? "pacientes" : "patients") + "</a>";
							temp += "<span class=\"graytext\">&nbsp;] [&nbsp;</span>";
							temp += "<a class=\"gray\" href=\"" + dbRow["PrettyUrl"] + "/healthprofessional/\">" + (Functions.IsPropertySet(dbRow["NCIViewID"].ToString(), "IsSpanishContent") ? "profesionales" : "health professionals") + "</a>";
						}
						else
						{
							temp += "<a class=\"gray\" href=\"" + dbRow["Url"] + "?version=patient&viewid=" + HttpUtility.UrlEncode(dbRow["NCIViewId"].ToString()) + "\">" + (Functions.IsPropertySet(dbRow["NCIViewID"].ToString(), "IsSpanishContent") ? "pacientes" : "patients") + "</a>";
							temp += "<span class=\"graytext\">&nbsp;] [&nbsp;</span>";
							temp += "<a class=\"gray\" href=\"" + dbRow["Url"] + "?version=healthprofessional&viewid=" + HttpUtility.UrlEncode(dbRow["NCIViewId"].ToString()) + "\">" + (Functions.IsPropertySet(dbRow["NCIViewID"].ToString(), "IsSpanishContent") ? "profesionales" : "health professionals") + "</a>";
						}
						temp += "<span class=\"graytext\">&nbsp;]</span>";						
					}
					else
					{
						//Build link url
						if(Functions.HasValue(dbRow["PrettyUrl"].ToString()))
						{
							url = dbRow["PrettyUrl"].ToString().Trim();
						}
						else
						{
							url = dbRow["Url"].ToString();
							if(Functions.HasValue(url) && Functions.HasValue(dbRow["UrlArguments"].ToString()))
							{
								url += "?" + dbRow["UrlArguments"].ToString();
							}
						
							if((bool)dbRow["IsLinkExternal"])
							{
								url = "http://redir.nci.nih.gov/cgi-bin/redir.pl?section=" + dbRow["SectionName"].ToString() + "&destURI=" + url;
							}
						}

						if(showFeatured == false || (bool)dbRow["IsFeatured"] == false)
						{
							urlText = dbRow[titleField].ToString();
						}
						else
						{
							text = "<b>" + dbRow[titleField].ToString() + "</b><br>";
							urlText = dbRow["Description"].ToString();						
						}

						if(releaseDateStyle == 1)
						{
							urlText += " " + GetDates(dbRow, "");
						}

						if(Functions.HasValue(url))
						{
							//Check for clicklogging property
							if(Functions.IsPropertySet(dbRow["NCIViewID"].ToString(), "Clicklogging"))
							{
								temp = text + "<a href=\"/common/clickpassthrough.aspx?redirectUrl=" + HttpUtility.UrlEncode(url) + "&clickItem=listId:" + listId + ";viewId:" + dbRow["NCIViewID"].ToString() + "\" OnMouseOver=\"" + Functions.GetMouseOverStatusAttribute(url) + "\" OnMouseOut=\"" + Functions.GetMouseOutStatusAttribute() + "\">" + urlText + "</a>";
							}
							else
							{
								temp = text + "<a href=\"" + url + "\"" + urlAttributes + ">" + urlText + "</a>";
							}
						}
						else
						{
							temp = text + urlText;
						}
					}
				
					//Add Description
					if(Functions.HasValue(dbRow["Description"].ToString()) && showDescription == true)
					{
						if(releaseDateStyle == 0)
						{
							temp += "<br><span class=\"graytext\">" + GetDates(dbRow, " - ") + dbRow["Description"] + "</span>";	
						}
						else
						{
							temp += "<br><span class=\"graytext\">" + dbRow["Description"] + "</span>";	
						}
					}

					this.Add(temp);
				}
			
				dbTable.Clear();
			}
			else
			{
				CancerGovError.LogError("", this.ToString(), ErrorType.InvalidGuid, ErrorTypeDesc.InvalidGuid);
			}
		}
	}
}
