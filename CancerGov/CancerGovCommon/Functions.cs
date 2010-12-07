using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;
using CancerGov.Common;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.Common {
	///<summary>
	///Defines a set of common static functions<br/>
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  8-8-2001<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	///   Mike Brady 2-14-03   Added support for logging sessionID in the LogUserInput function
	///<br/>
	///</summary>
	public class Functions {
		/// <summary>
		/// Default class constructor
		/// </summary>
		public Functions() {
		}

		#region StripHTMLTags method

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string StripHTMLTags(string text)
		{
			Regex tagExpr = new Regex("<.+?>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
			
			return tagExpr.Replace(text, "");
		}

		#endregion

		#region LogUserInput method

		public static void LogUserInput(string eventSrc, string destUrl, string clientIP, string inputValue, string sessionID) 
		{
			try 
			{
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_LogClick", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@EventSrc", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@ClickValue", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@ClickItem", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@ClientIP", SqlDbType.VarChar));
                        cmd.Parameters.Add(new SqlParameter("@UserSessionID", SqlDbType.VarChar));

                        cmd.Parameters["@EventSrc"].Value = eventSrc;
                        cmd.Parameters["@ClickValue"].Value = destUrl;
                        cmd.Parameters["@ClickItem"].Value = inputValue;
                        cmd.Parameters["@ClientIP"].Value = eventSrc;
                        cmd.Parameters["@UserSessionID"].Value = sessionID;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                }
			}
			catch(Exception e) {
				CancerGovError.LogError("", "Functions.LogUserInput(string inputValue)", ErrorType.DbUnavailable, e);
			}
		}

		#endregion

		#region HasValue method

		/// <summary>
		/// Method safely determines if database object field or request parameter field
		/// is not null or zero-length
		/// </summary>
		/// <param name="param">database object field value or request parameter or other string</param>
		/// <returns></returns>
		public static bool HasValue(string param) {
			if(param == null || param.Trim().Length == 0) {
				return false;
			}
			else {
				return true;
			}
		}

		#endregion		

		#region IsPropertySet method

		/// <summary>
		/// Method determines if a particular view property is set for a view
		/// </summary>
		/// <param name="viewId">NCIViewId</param>
		/// <param name="property">Valid ViewProperty.PropertyName value</param>
		/// <returns></returns>
		public static bool IsPropertySet(string viewId, string property) {
			bool isSet = false;

			if(Functions.IsGuid(viewId)) {
				DataTable dbTable = new DataTable();
                SqlDataAdapter dbAdapter = new SqlDataAdapter("usp_GetViewProperties @ViewID='" + viewId + "', @PropertyName='" + property + "'", ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
			
				try {
					dbAdapter.Fill(dbTable);
				}
				catch(SqlException) {
					CancerGovError.LogError("IsPropertySet:  viewId=\"" + viewId + "\"", "CancerGov.Common.Utility.Functions.IsPropertySet", ErrorType.DbUnavailable, ErrorTypeDesc.DbUnavailable);
				}

				if(dbTable.Rows.Count > 0) {
					if(HasValue(dbTable.Rows[0]["PropertyValue"].ToString()) && dbTable.Rows[0]["PropertyValue"].ToString().Trim().ToUpper() == "YES") {
						isSet = true;
					}
				}

				dbTable.Clear();
			}

			return isSet;
		}	
	
		#endregion

		#region GetViewProperty method

		/// <summary>
		/// Method gets view property value from cancergov database
		/// </summary>
		/// <param name="viewId">NCIViewId owning property</param>
		/// <param name="property">Name of property to get</param>
		/// <returns>Value of view property</returns>
		public static string GetViewProperty(string viewId, string property) {
			string propertyValue = "";
			DataTable dbTable = new DataTable();
            SqlDataAdapter dbAdapter = new SqlDataAdapter("usp_GetViewProperties @ViewID='" + viewId + "', @PropertyName='" + property + "'", ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
			
			try {
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException) {
				CancerGovError.LogError("GetViewProperty:  viewId=\"" + viewId + "\"", "CancerGov.Common.Utility.Functions.GetViewProperty", ErrorType.DbUnavailable, ErrorTypeDesc.DbUnavailable);
			}

			if(dbTable.Rows.Count > 0) {
				if(HasValue(dbTable.Rows[0]["PropertyValue"].ToString().Trim())) {
					propertyValue = dbTable.Rows[0]["PropertyValue"].ToString().Trim();
				}
			}

			dbTable.Clear();
			
			return propertyValue;
		}	
	
		public static Hashtable GetViewProperty(string viewId) 
		{
			Hashtable properties = new Hashtable();
			DataTable dbTable = new DataTable();
						
			try 
			{
                SqlDataAdapter dbAdapter = new SqlDataAdapter("usp_GetViewProperties  @ViewID='" + viewId + "'", ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException sqlE) 
			{
				CancerGovError.LogError("CancerGov.Common.Functions.GetViewProperty", ErrorType.DbUnavailable, sqlE);
			}

			foreach(DataRow dbRow in dbTable.Rows)
			{
				properties.Add(dbRow["PropertyName"].ToString().Trim().ToLower(), Functions.IfNull(dbRow["PropertyValue"].ToString(), ""));
			}

			dbTable.Clear();
			dbTable.Dispose();
			
			return properties;
		}	

		#endregion

		#region GetViewObjectProperty method

		/// <summary>
		/// Method gets view object property value from cancergov database
		/// </summary>
		/// <param name="viewobjectId">NCIViewId owning property</param>
		/// <param name="property">Name of property to get</param>
		/// <returns>Value of view object property</returns>
		public static string GetViewObjectProperty(string viewObjectId, string property) 
		{
			string propertyValue = "";
			DataTable dbTable = new DataTable();
            SqlDataAdapter dbAdapter = new SqlDataAdapter("usp_GetViewObjectProperties  @ViewObjectID='" + viewObjectId + "', @PropertyName='" + property + "'", ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
			
			try 
			{
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException) 
			{
				CancerGovError.LogError("GetViewObjectProperty:  viewObjectId=\"" + viewObjectId + "\"", "CancerGov.Common.Utility.Functions.GetViewObjectProperty", ErrorType.DbUnavailable, ErrorTypeDesc.DbUnavailable);
			}

			if(dbTable.Rows.Count > 0) 
			{
				if(HasValue(dbTable.Rows[0]["PropertyValue"].ToString().Trim())) 
				{
					propertyValue = dbTable.Rows[0]["PropertyValue"].ToString().Trim();
				}
			}

			dbTable.Clear();
			
			return propertyValue;
		}	
	
		public static Hashtable GetViewObjectProperty(string viewObjectId) 
		{
			Hashtable properties = new Hashtable();
			DataTable dbTable = new DataTable();
						
			try 
			{
                SqlDataAdapter dbAdapter = new SqlDataAdapter("usp_GetViewObjectProperties  @ViewObjectID='" + viewObjectId + "'", ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException sqlE) 
			{
				CancerGovError.LogError("CancerGov.Common.Functions.GetViewObjectProperty", ErrorType.DbUnavailable, sqlE);
			}

			foreach(DataRow dbRow in dbTable.Rows)
			{
				properties.Add(dbRow["PropertyName"].ToString().Trim().ToLower(), Functions.IfNull(dbRow["PropertyValue"], ""));
			}

			dbTable.Clear();
			dbTable.Dispose();
			
			return properties;
		}	

		#endregion

		#region GetMouseOverStatusAttribute method

		public static string GetMouseOverStatusAttribute(string url) {
			if(Functions.HasValue(url)) {
				if(url.IndexOf("://") == -1) {
					return "window.status=window.location.protocol + '//' + window.location.host + '" + url + "'; return true;";
				}
				else {
					return "window.status='" + url + "'; return true;";
				}
			}
			else {
				return "";
			}
		}

		#endregion

		#region GetMouseOutStatusAttribute

		public static string GetMouseOutStatusAttribute() {
			return "window.status=''; return true;";
		}

		#endregion

		#region GetReferrer method

		/// <summary>
		/// Method safely gets the request referrer url.  It also makes sure that the
		/// host for cancer.gov looks the same.
		/// </summary>
		/// <param name="req">Page Request</param>
		/// <returns>AbsoluteUri of request referrer or ""</returns>
		public static string GetReferrer(System.Web.HttpRequest req) {
			if(req.UrlReferrer != null) {
				string host = req.UrlReferrer.Host;
				string path = req.UrlReferrer.LocalPath;
				string query = req.UrlReferrer.Query;
                string hostip = "0.0.0.0";
				System.Net.IPHostEntry ipheHost = null;
				bool SkipCheck = false;

				//Remove any trailing periods.  Trust me it happens...  A lot... no I am serious.
				host=Regex.Replace(host,"\\.$","");

				try {
					ipheHost=System.Net.Dns.GetHostByName(host);
				}catch (System.Net.Sockets.SocketException) {
                   SkipCheck = true;
				}
				
				if (! SkipCheck) {
					if ((ipheHost != null) && (ipheHost.AddressList != null) && (ipheHost.AddressList.Length > 0) && (ipheHost.AddressList[0] != null))  {
						hostip=ipheHost.AddressList[0].ToString(); 
					}

					if ((hostip == ConfigurationSettings.AppSettings["CancerGovIPAddress"].ToString()) && (!Regex.IsMatch(host,"preview")))  {
						host="cancer.gov";
					}
				}

				//Double /s and the great /./ which is just /
				path=Regex.Replace(path,"\\/+","/");
				path=Regex.Replace(path,"\\/\\.\\/","/");

				if ((query != null) && (query != "")) {
					return "http://" + host + path + query;
				} else {
					return "http://" + host + path;
				}

			}
			else {
				return "";
			}
		}

		#endregion

		#region TruncLine method

		/// <summary>
		/// Method forces a fixed width paragraph
		/// </summary>
		/// <param name="line">Input string</param>
		/// <param name="lineSize">Paragraph width</param>
		/// <param name="forceWrap">Indicates wrapping at paragraph width instead of at first space or new line character before paragraph width</param>
		/// <param name="htmlLineBreak">Indicates using an HTML break tag to wrap a line instead of a new line character</param>
		/// <returns>fixed width paragraph</returns>		
		public static string TruncLine(string line, int lineSize, bool forceWrap, bool htmlLineBreak) {
			string result = "";
			
			string lineBreak = "\n";
			int wrapIndex = 0;

			if(htmlLineBreak) {
				lineBreak = "<BR>";
			}

			if(line != null && line.Trim().Length > 0) {
				while(line.Length > lineSize) {
					//Find last incidence of a new line character within the specified line width 
					wrapIndex = line.LastIndexOf("\n", lineSize);
					//add other line end characters

					if(wrapIndex == -1) {
						//Get last incidence of a space character within the specified line width
						wrapIndex = line.LastIndexOf(" ", lineSize);
						if(wrapIndex == -1) {
							//Set wrap index to linesize, if indicator to force a line wrap is set
							if(forceWrap) {
								wrapIndex = lineSize - 1;
							}
							else {
								//Get first space or line break
								wrapIndex = line.IndexOf("\n");
								if(wrapIndex == -1) {
									wrapIndex = line.IndexOf(" ");
									if(wrapIndex == -1) {
										wrapIndex = line.Length;
									}
								}								
							}
						}						
					}
					
					//build return value and truncate line
					result += line.Substring(0, wrapIndex) + lineBreak;
					line = line.Remove(0, wrapIndex).Trim();
				}
					
				result += line;
			}

			return result;
		}

		#endregion		

		#region IsGuid method

		/// <summary>
		/// Method determines if parameter is a valid SQL Server Guid
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static bool IsGuid(string guid) {
			SqlGuid testGuid;
			
			try {
				testGuid = System.Data.SqlTypes.SqlGuid.Parse(guid);
				return true;	
			}
			catch(FormatException) {
				return false;
			}
			catch(ArgumentNullException) {
				return false;
			}
		}

		#endregion

		#region Date methods

		/// <summary>
		/// Method indicates if parameter is a date more recent than 1980
		/// </summary>
		/// <param name="dbDate">Date argument</param>
		/// <returns></returns>
		public static bool ValidDate(string dbDate) {
			bool result = false;

			try {
				DateTime dt = Convert.ToDateTime(dbDate);
							
				if(dt.Year > 1980) {
					result = true;		
				}
			}
			catch(InvalidCastException) {
				//don't show date, but no error trap
			}

			return result;
		}

		/// <summary>
		/// Method builds document posted and updated date strings based on
		/// date values and display mode defined in data row
		/// </summary>
		/// <param name="dbRow">usp_GetViewDocument record</param>
		/// <returns>Document posted/updated dates, for the given view, not any individual document</returns>
		public static string GetDates(DataRow dbRow) {
			string result = "";
	
			switch(dbRow["DisplayDateMode"].ToString().Trim()) {
				case "none":
					break;
				case "posted":
					if(ValidDate(dbRow["PostedDate"].ToString().Trim())) {
						result += "Posted:  " + dbRow["PostedDate"].ToString().Trim();
					}
					break;
				case "updated":
					if(ValidDate(dbRow["ReleaseDate"].ToString().Trim())) {
						result += "Updated:  " + dbRow["ReleaseDate"].ToString().Trim();
					}
					break;
				case "both":
					if(ValidDate(dbRow["PostedDate"].ToString().Trim())) {
						result += "Posted:  " + dbRow["PostedDate"].ToString().Trim();
					}
					if(ValidDate(dbRow["ReleaseDate"].ToString().Trim())) {
						if(result.Trim().Length > 0) {
							result += ", ";
						}
						result += "Updated:  " + dbRow["ReleaseDate"].ToString().Trim();
					}
					break;
			}

			return result;
		}
		

		/// <summary>
		/// Method builds document posted and updated date strings based on
		/// date values and display mode defined in data row
		/// </summary>
		/// <param name="dbRow">usp_GetViewDocument record</param>
		/// <returns>Document posted/updated dates for a given document, not the view</returns>
		public static string GetDocDates(DataRow dbRow) 
		{
			string result = "";
	
			switch(dbRow["DocDisplayDateMode"].ToString().Trim()) 
			{
				case "none":
					break;
				case "posted":
					if(ValidDate(dbRow["DocPostedDate"].ToString().Trim())) 
					{
						result += "Posted:  " + dbRow["DocPostedDate"].ToString().Trim();
					}
					break;
				case "updated":
					if(ValidDate(dbRow["DocReleaseDate"].ToString().Trim())) 
					{
						result += "Updated:  " + dbRow["DocReleaseDate"].ToString().Trim();
					}
					break;
				case "both":
					if(ValidDate(dbRow["DocPostedDate"].ToString().Trim())) 
					{
						result += "Posted:  " + dbRow["DocPostedDate"].ToString().Trim();
					}
					if(ValidDate(dbRow["DocReleaseDate"].ToString().Trim())) 
					{
						if(result.Trim().Length > 0) 
						{
							result += ", ";
						}
						result += "Updated:  " + dbRow["DocReleaseDate"].ToString().Trim();
					}
					break;
			}

			return result;
		}
		
		#endregion
		
		#region EvalArg method

		/// <summary>
		/// Method evaluates a character string argument to null or 'value' for use in a
		/// database query or to "" or value for data validation
		/// </summary>
		/// <param name="arg">Character string argument</param>
		/// <param name="dbEncode">Indicates if return value should be formatted for db query</param>
		/// <returns>"null" or "'arg'"</returns>
		public static string EvalArg(string arg, bool dbEncode) {
			if(dbEncode) {
				if(!Functions.HasValue(arg)) {
					return "null";
				}
				else {
					return "'" + arg.Replace("'","''") + "'";
				}
			}
			else {
				if(!Functions.HasValue(arg)) {
					return "";
				}
				else {
					return arg;
				}
			}
		}

		#endregion
		
		#region ParseNameValue method

		/// <summary>
		/// Method parses one or more name-value request parameters
		/// </summary>
		/// <param name="arg">Name-value request parameter(s)</param>
		/// <param name="returnType">Indicates what gets returned, name{0} or value{1}</param>
		/// <param name="parseMultiple">Indicates if arg is a comma-delimited set of name-value pairs to return a comma-delimited set of return type values</param>
		/// <returns>One or more(comma-delimited) parsed names or values</returns>
		public static string ParseNameValue(string arg, int returnType) {
			string tmp;
			string result = "";
			
			if(Functions.HasValue(arg)) {
				string[] parsedArgs = arg.Split(',');

				for(int i = 0; i < parsedArgs.Length; i++) {
					tmp = HttpUtility.UrlDecode(parsedArgs[i]);

					if(tmp.IndexOf(";") > -1) {
						switch(returnType) {
							case 0:
								tmp = tmp.Substring(0, tmp.IndexOf(";"));
								break;
							case 1:
								tmp = tmp.Substring(tmp.IndexOf(";") + 1);
								break;
						}					
					}
					else {
						if(returnType == 1  && tmp.IndexOf("any") != -1 && tmp.IndexOf("all") != -1) {
							tmp = "default";
						}
					}
					
					if(tmp != "default") 
					{
						result += (result.Length != 0 ? "," : "") + tmp;
					}
					else
					{
						result = String.Empty;
						break;
					}
				}
				
				if(returnType == 1 && result.Trim().Length > 0) {
					result = "'" + result.Replace("'","''").Replace("|",",") + "'";
				}
			}

			if(result.Trim().Length == 0) {
				switch(returnType) {
					case 0:
						result = "any";
						break;
					case 1:
						result = "null";
						break;
				}
			}

			return result;
		}

		#endregion

		#region IfNull method
		/// <summary>
		/// Method tests a value argument for null and returns the second argument if it tests true
		/// else it returns test value
		/// </summary>
		/// <param name="testValue">Value to test for null</param>
		/// <param name="ifNullValue">Value to return if testValue is null</param>
		/// <returns>TestValue if not null, ifNullValue if null</returns>
		public static string IfNull(string testValue, string ifNullValue) {
			if(testValue == null) {
				return ifNullValue;
			}
			else {
				return testValue;
			}
		}

		public static string IfNull(object testValue, string ifNullValue) 
		{			
			if(testValue == null) 
			{
				return ifNullValue;
			}
			else 
			{
				return testValue.ToString();
			}
		}

		

		#endregion

		#region NullIf method
		/// <summary>
		/// Method tests a value argument for null and returns the second argument if it tests true
		/// else it returns test value
		/// </summary>
		/// <param name="testValue">Value to test for null</param>
		/// <param name="ifNullValue">Value to return if testValue is null</param>
		/// <returns>TestValue if not null, ifNullValue if null</returns>
		public static string NullIf(string testValue, string ifNullValue) {
			if(testValue == null) {
				return ifNullValue;
			}
			else {
				return testValue;
			}
		}

		#endregion

		#region IfBlank method
		/// <summary>
		/// Method tests a value argument for null or zero-length string and 
		/// returns the second argument if it tests true
		/// else it returns test value
		/// </summary>
		/// <param name="testValue">Value to test for null</param>
		/// <param name="ifNullValue">Value to return if testValue is null</param>
		/// <returns>TestValue if not null, ifNullValue if null</returns>
		public static string IfBlank(string testValue, string ifBlankValue) 
		{
			if(testValue == null || testValue.Trim().Length == 0) 
			{
				return ifBlankValue;
			}
			else 
			{
				return testValue;
			}
		}

		#endregion

		#region BlankIf method
		/// <summary>
		/// Method tests a value argument for null and returns the second argument if it tests true
		/// else it returns test value
		/// </summary>
		/// <param name="testValue">Value to test for null</param>
		/// <param name="ifNullValue">Value to return if testValue is null</param>
		/// <returns>TestValue if not null, ifNullValue if null</returns>
		public static string BlankIf(string testValue, string ifBlankValue) {
			if(testValue == null || testValue.Trim().Length == 0) {
				return ifBlankValue;
			}
			else {
				return testValue;
			}
		}

		#endregion

		#region GetHashtable method and overloads

		public static Hashtable GetHashtable(string[] keys) {
			Hashtable retHash = new Hashtable(keys.Length);
			
			foreach(string key in keys) {
				retHash.Add(key, "");
			}

			return retHash;
		}

		#endregion

		#region List control encoding methods

		public static void EncodeListBoxValues(object sender, EventArgs e) {
			ListBox lBox = (ListBox)sender;
	
			foreach(ListItem lItem in lBox.Items) {
				lItem.Value = HttpUtility.UrlEncode(lItem.Value);
			}            			
		}

		public static void EncodeHtmlSelectValues(object sender, EventArgs e) {
			HtmlSelect hSelect= (HtmlSelect)sender;
			 
			foreach(ListItem lItem in hSelect.Items) {
				lItem.Value = HttpUtility.UrlEncode(lItem.Value);
			}            			
		}

		public static void EncodeDropDownListValues(object sender, EventArgs e) {
			DropDownList ddList = (DropDownList)sender;
			
			foreach(ListItem lItem in ddList.Items) {
				lItem.Value = HttpUtility.UrlEncode(lItem.Value);
			}            			
		}

		#endregion

		#region BuildReturnToTopBar method

		public static string BuildReturnToTopBar(bool textOnly) {
			string result = "";

			//Return to top
			result += "<table border=\"0\" cellpadding=\"1\" cellspacing=\"0\" width=\"100%\" bgcolor=\"#333366\">\n";
			result += "	<tr>\n";
			result += "		<td>\n";
			result += "			<table border=\"0\" cellpadding=\"1\" cellspacing=\"0\" width=\"100%\" bgcolor=\"#f1f1e7\">\n";
			result += "				<tr>\n";
			result += "					<td>\n";
			if(!textOnly) {
				result += "						<div align=\"right\"><a href=\"#top\"><img src=\"/images/return_to_top.gif\" width=\"69\" height=\"12\" border=\"0\" alt=\"return to top\"></a></div>\n";
			}
			else {
				result += "						<div align=\"right\"><a href=\"#top\">return to top</a></div>\n";
			}
			result += "					</td>\n";
			result += "				</tr>\n";
			result += "			</table>\n";
			result += "		</td>\n";
			result += "	</tr>\n";
			result += "</table>\n";
			result += "<P>\n";

			return result;
		}

		#endregion

		#region RedirectURL method

		/// <summary>
		/// Redirects a viewId to a page defined as the view property RedirectUrl
		/// </summary>
		/// <param name="viewId">View for this page</param>
		/// <param name="Page">The Control.Page for the current page</param>
		public static void RedirectURL(string viewId, System.Web.UI.Page Page) 
		{
			string strRedirectURL = Functions.GetViewProperty(viewId, "RedirectUrl");

			//We need to add the host and stuff if it does not already exist.
			if (!Regex.IsMatch(strRedirectURL,"^http:"))
			{
				strRedirectURL="http://" + Page.Request.Url.Host + strRedirectURL;
			}
			
			Page.Response.Clear();
			Page.Response.Write("<html><head><meta http-equiv=\"Refresh\" content=\"0; URL=" + strRedirectURL + "\"></head><body></body></html>\n\n");
			Page.Response.End();
		}

		#endregion
	}
}
