using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using CancerGov.Common;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.Common.Extraction
{
	///<summary>
	///Exposes methods for extracting glossary terms from a large text field, 
	///changing the terms' links to anchor links, and building a glossary appendix.<br/>
	///
	///Design limitation (Chen): This program, because it keys off of the GlossaryTermID (CDRID)
	///assumes that a given document only links to at most one version of each Term.
	///i.e., summary XYZ cannot link to both the Patient and Health professional versions
	///or the English and Spanish versions of the same glossary term.
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  2-8-2002<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	///<br/>
	///</summary>
	public class GlossaryTermExtractor
	{		
		public ArrayList terms = new ArrayList();
		public ArrayList glossaryIds = new ArrayList();
		private ArrayList sourceIds = new ArrayList();
		public Hashtable glossaryHash = new Hashtable();
		public Hashtable glossaryIDHash = new Hashtable();
		public Hashtable glossaryTermHash = new Hashtable();
		private int termCount = 0;

		#region Properties

		/// <summary>
		/// Number of glossary terms extracted
		/// </summary>
		public int Count
		{
			get {return termCount;}
		}

		#endregion

		/// <summary>
		/// Default class constructor
		/// </summary>
		public GlossaryTermExtractor()
		{
		}
		
		/// <summary>
		/// Delegate method defines actions that will occur when a match is found,
		/// stores glossary term found as a match
		/// </summary>
		/// <param name="m">Regular expression match object</param>
		/// <returns>Match value with no alteration</returns>
		private string ChangeLink(Match m)
		{
			string type = m.Groups[3].Value;
			string lookup = m.Groups[4].Value;
			string language = "ENGLISH";
			string audience = "Patient";

			switch(type.ToLower())
			{
				case "defbyid":
					lookup = lookup.Replace("&amp;", "&");
					if (lookup.IndexOf("version") != -1) 
					{
						Match match = Regex.Match(lookup, "&version=(.+?)&language=(.+?)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
						if (match.Groups[1].Value.ToLower() == "patient") 
						{
							audience = "Patient";
						} 
						else 
						{
							audience = "Health professional";
						}
						language = match.Groups[2].Value.ToUpper();
					}
					if(lookup.StartsWith("CDR"))
					{
						lookup = Regex.Replace(lookup, "^CDR0+", "", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
						if (lookup.IndexOf("&") != -1) 
						{
							lookup = lookup.Substring(0, lookup.IndexOf("&"));
						}
						glossaryIds.Add(lookup);
					}
					else
					{
						if (lookup.IndexOf("&") != -1) 
						{
							lookup = lookup.Substring(0, lookup.IndexOf("&"));
						}
						sourceIds.Add(lookup);
					}
					if(!glossaryIDHash.ContainsKey(lookup))
					{		
						glossaryIDHash.Add(lookup, audience   + @"\" + language);
					}
					break;
				case "definition":
					terms.Add(lookup.Replace("\\'","'"));
					if(!glossaryTermHash.ContainsKey(lookup))
					{
						glossaryTermHash.Add(lookup, audience + @"\" + language);
					}
					break;
			}
			
			if(!glossaryHash.ContainsKey(lookup))
			{
				glossaryHash.Add(lookup, "value");
			}

			return m.Value;
		}

		public string ExtractGlossaryTerms(string text)
		{
			if (Regex.IsMatch(text, "/common/popups/popDefinition.aspx", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline) ||
				(Regex.IsMatch(text, "/dictionary", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline))) 
			{
                Regex exp = new Regex("href=\"(/common/popups/popDefinition.aspx.+?|/dictionary.+?)\"\\s+?onclick=\"(popWindow|javascript:popWindow)\\(\'(definition|defbyid)\',.*?\'(.+?)\'\\).+?\">", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                exp.Replace(text, new MatchEvaluator(ChangeLink));
                
                exp = new Regex("href=\"(" + ConfigurationSettings.AppSettings["RootUrl"] + "/common/popups/popDefinition.aspx.+?|/dictionary.+?)\"\\s+?onclick=\"(popWindow|javascript:popWindow)\\(\'(definition|defbyid)\',.*?\'(.+?)\'\\).+?\">", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
				return exp.Replace(text, new MatchEvaluator(ChangeLink));
			} 
			else 
			{
				return text;
			}
		}

		/// <summary>
		/// Method builds an HTML table of glossary terms and definitions for terms 
		/// found during the execution of the <code>ExtractGlossaryTerms</code> method
		/// </summary>
		/// <param name="title">title text that aptly describes the glossary table</param>
		/// <returns>HTML table of glossary terms and definitions</returns>
		public string BuildGlossaryTable(string title)
		{
			string result = ""; 
			string commandText = "";
			
			//Build query to get glossary terms
			if(terms.Count > 0 || glossaryIds.Count > 0 || sourceIds.Count > 0)
			{	
				if(terms.Count > 0)
				{
					if(commandText.Length > 0)
					{
						commandText += ",";
					}

					commandText += "@Terms='" + CollateParams(terms, false) + "'";
				}
				
				if(glossaryIds.Count > 0)
				{
					if(commandText.Length > 0)
					{
						commandText += ",";
					}

					commandText += "@GlossaryIDs='" + CollateParams(glossaryIds, true) + "'";
				}
				
				commandText = "usp_GetGlossaryTerms " + commandText;

                SqlDataAdapter dbAdapter = new SqlDataAdapter(commandText, ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
				DataTable dbTable = new DataTable();

				try
				{
					dbAdapter.Fill(dbTable);
				}
				catch(SqlException sqlE)
				{
					CancerGovError.LogError("GlossaryTermExtractor:  " + commandText, this.ToString(), ErrorType.InvalidArgument, sqlE);
				}
							
				result += "<table border=0 width=699 cellspacing=0 cellpadding=0><tr><td>\n";
				result += "<BR><BR><a name=\"" + title + "\"></a><h2>" + title + "</h2>\n";
				termCount = termCount + dbTable.Rows.Count;

				string intResult="";

				foreach(DataRow dbRow in dbTable.Rows)
				{
					bool include_p = false;
					string hashvalue = "";
					string audience = "";
					string language = "";

					if (glossaryIDHash.ContainsKey(dbRow["GlossaryID"].ToString())) 
					{
						hashvalue = glossaryIDHash[dbRow["GlossaryID"].ToString()].ToString();
						audience = hashvalue.Substring(0, hashvalue.IndexOf(@"\"));
						language = hashvalue.Substring(hashvalue.IndexOf(@"\") + 1);
						if (dbRow["audience"].ToString() == audience && dbRow["language"].ToString() == language) 
						{
							include_p = true;
						}
					} 
					else if (glossaryTermHash.ContainsKey(dbRow["name"])) 
					{
						hashvalue = glossaryTermHash[dbRow["name"]].ToString();
						audience = hashvalue.Substring(0, hashvalue.IndexOf(@"\"));
						language = hashvalue.Substring(hashvalue.IndexOf(@"\") + 1);
						if (dbRow["audience"].ToString() == audience && dbRow["language"].ToString() == language) 
						{
							include_p = true;
						}
					}

					if (include_p) 
					{
						intResult += "<B>" + dbRow["name"].ToString() + "</B> ";
						if(Functions.HasValue(dbRow["pronunciation"].ToString()))
						{
							intResult += dbRow["pronunciation"].ToString();
						}
                        intResult += "\n<blockquote>\n";
						intResult += dbRow["definition"].ToString() + "\n";
						intResult += "</blockquote>\n\n";
					} 
				}

				if (intResult.Trim().Length >0)
				{
					result += intResult + "</td></tr></table>\n";
				}
				else
					result ="";
				
				dbTable.Clear();
				dbTable.Dispose();
				dbAdapter.Dispose();
			}

			glossaryHash.Clear();
			glossaryIDHash.Clear();
			glossaryTermHash.Clear();
			
			return result;
		}


//Function Overload
        public string BuildGlossaryTable(string title, ArrayList glossaryid, ArrayList glossaryIDHash, ArrayList glossaryTermHash, ArrayList terms)
        {
            string result = "";
            string commandText = "";
            Hashtable glossaryIDHash1=new Hashtable();
            Hashtable glossaryTermHash1 = new Hashtable();

            foreach (DictionaryEntry de in glossaryIDHash)
            {
                glossaryIDHash1.Add(de.Key, de.Value);
            }

            foreach (DictionaryEntry de in glossaryTermHash)
            {
                glossaryTermHash1.Add(de.Key, de.Value);
            }

            //Build query to get glossary terms
            if (terms.Count > 0 || glossaryid.Count > 0 || sourceIds.Count > 0)
            {
                if (terms.Count > 0)
                {
                    if (commandText.Length > 0)
                    {
                        commandText += ",";
                    }

                    commandText += "@Terms='" + CollateParams(terms, false) + "'";
                }

                if (glossaryid.Count > 0)
                {
                    if (commandText.Length > 0)
                    {
                        commandText += ",";
                    }

                    commandText += "@GlossaryIDs='" + CollateParams(glossaryid, true) + "'";
                }

                commandText = "usp_GetGlossaryTerms " + commandText;

                SqlDataAdapter dbAdapter = new SqlDataAdapter(commandText, ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
                DataTable dbTable = new DataTable();

                try
                {
                    dbAdapter.Fill(dbTable);
                }
                catch (SqlException sqlE)
                {
                    CancerGovError.LogError("GlossaryTermExtractor:  " + commandText, this.ToString(), ErrorType.InvalidArgument, sqlE);
                }

                result += "<table border=0 width=699 cellspacing=0 cellpadding=0><tr><td>\n";
                result += "<BR><BR><a name=\"" + title + "\"></a><h2>" + title + "</h2>\n";
                termCount = termCount + dbTable.Rows.Count;

                string intResult = "";

                foreach (DataRow dbRow in dbTable.Rows)
                {
                    bool include_p = false;
                    string hashvalue = "";
                    string audience = "";
                    string language = "";

                    if (glossaryIDHash1.ContainsKey(dbRow["GlossaryID"].ToString()))
                    {
                        hashvalue = glossaryIDHash1[dbRow["GlossaryID"].ToString()].ToString();
                        audience = hashvalue.Substring(0, hashvalue.IndexOf(@"\"));
                        language = hashvalue.Substring(hashvalue.IndexOf(@"\") + 1);
                        if (dbRow["audience"].ToString() == audience && dbRow["language"].ToString() == language)
                        {
                            include_p = true;
                        }
                    }
                    else if (glossaryTermHash1.ContainsKey(dbRow["name"]))
                    {
                        hashvalue = glossaryTermHash1[dbRow["name"]].ToString();
                        audience = hashvalue.Substring(0, hashvalue.IndexOf(@"\"));
                        language = hashvalue.Substring(hashvalue.IndexOf(@"\") + 1);
                        if (dbRow["audience"].ToString() == audience && dbRow["language"].ToString() == language)
                        {
                            include_p = true;
                        }
                    }

                    if (include_p)
                    {
                        intResult += "<B>" + dbRow["name"].ToString() + "</B> ";
                        if (Functions.HasValue(dbRow["pronunciation"].ToString()))
                        {
                            intResult += dbRow["pronunciation"].ToString();
                        }
                        intResult += "\n<blockquote>\n";
                        intResult += dbRow["definition"].ToString() + "\n";
                        intResult += "</blockquote>\n\n";
                    }
                }

                if (intResult.Trim().Length > 0)
                {
                    result += intResult + "</td></tr></table>\n";
                }
                else
                    result = "";

                dbTable.Clear();
                dbTable.Dispose();
                dbAdapter.Dispose();
            }

            glossaryHash.Clear();
            glossaryIDHash.Clear();
            glossaryTermHash.Clear();

            return result;
        }

		private string CollateParams(ArrayList paramArray, bool numericParam)
		{
			string retval = "";

			foreach(string param in paramArray)
			{
				if(retval.Length > 0)
				{
					retval += ",";
				}

				if(numericParam)
				{
					retval += param;
				}
				else
				{
					retval += param.Replace("'","''");
				}
			}

			return retval;
		}
	}
}
