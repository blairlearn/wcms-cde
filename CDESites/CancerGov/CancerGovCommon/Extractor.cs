using System;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;
using CancerGov.Common;
using System.IO;

namespace CancerGov.Common.Extraction
{
	///<summary>
	///Defines valid extraction types<br/>
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  3-26-2001<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	///<br/>
	///</summary>
	public class ExtractionTypes
	{
		public const int URL = 0;
	}

	///<summary>
	///Defines footnote extractor object<br/>
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  3-26-2001<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	///<br/>
	///</summary>
	public class FootnoteExtractor
	{
		private int footnoteCount = 0;
		private int extType = 0;
		private string extractGroup = "extractValue";
		private bool removeReturnToTopBar = true;
		private string[] excludeList;
		public Hashtable extractHash = new Hashtable();
        public ArrayList hashIndex = new ArrayList();

		#region Public Class Properties
        
		/// <summary>
		/// Extraction type
		/// </summary>
		public int ExtType
		{
			get {return extType;}
			set {extType = value;}
		}

		/// <summary>
		/// Array of strings defining extraction matches to exclude from footnotes
		/// </summary>
		public string[] ExcludeList
		{
			get {return excludeList;}
			set {excludeList = value;}
		}	

		/// <summary>
		/// Indicates whether to strip out return to top bar HTML
		/// </summary>
		public bool RemoveReturnToTopBar
		{
			get {return removeReturnToTopBar;}
			set {removeReturnToTopBar = value;}
		}

		/// <summary>
		/// Number of footnotes extracted
		/// </summary>
		public int Count
		{
			get {return footnoteCount;}
		}

		#endregion

		/// <summary>
		/// Default class constructor
		/// </summary>
		public FootnoteExtractor()
		{
		}

		/// <summary>
		/// Method extracts footnotes 
		/// </summary>
		/// <param name="expr">regular expression defining footnotes to extract</param>
		/// <param name="group">regular expression group defining relevant match</param>
		/// <param name="extractionType">type of footnote being extracted, <seealso cref="ExtractionTypes"/>ExtractionTypes</param>
		/// <param name="data">data from which to extract footnotes</param>
		/// <returns>data with alterations made to matches defined by MatchEvaluator</returns>
		public string Extract(Regex expr, string group, int extractionType, string data)
		{
           
			extType = extractionType;	
			extractGroup = group;

			if(removeReturnToTopBar)
			{
				data = StripReturnToTopBar(data);
			}

			data = Regex.Replace(data, "<a\\s+name", "<a_name", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
			data = expr.Replace(data, new MatchEvaluator(ExtractFootnote));
			data = Regex.Replace(data, "<a_name", "<a name", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
			return data;
		}

		/// <summary>
		/// Delegate method defines actions to take when a match is found,
		/// stores match in hash table
		/// </summary>
		/// <param name="mt">match object representing footnote</param>
		/// <returns>match with any alterations</returns>
		private string ExtractFootnote(Match mt) 
		{			
			bool include = true;
            excludeList = new string[] { "http://www.ncbi.nlm.nih.gov/entrez/query.fcgi?" };
			string extract = extract = mt.Groups[extractGroup].Value;
			int pos = 0;

			// these two are to remove any crap from the beginning of a url
			// these two (a newline in the original XML) and whitespace, should cover 80%
			// of all issues.
			extract = Regex.Replace(extract, "^&#xD;&#xA;", "", RegexOptions.Compiled | RegexOptions.Singleline);
            extract = Regex.Replace(extract, "&amp;#xD;&amp;#xA;", "", RegexOptions.Compiled | RegexOptions.Singleline);

			extract = Regex.Replace(extract, "^\\s+", "", RegexOptions.Compiled | RegexOptions.Singleline);
            
			switch(extType)
			{
				case ExtractionTypes.URL:		//URL extraction
					if (! (Regex.IsMatch(extract, "^(#|mailto|/common/popups/popdefinition.aspx|/dictionary|^\\s+$|^\"|javascript:)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
                        || Regex.IsMatch(extract, "^(#|mailto|" + ConfigurationManager.AppSettings["RootUrl"] + "/common/popups/popdefinition.aspx|/dictionary/db_alpha.aspx|^\\s+$|^\"|javascript:)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline)
						   || mt.Groups["linkText"].Value.StartsWith("http")
						   || mt.Groups["linkText"].Value.StartsWith("www.")) ) {

						// remove clickpassthrough link
						if (extract.IndexOf("/common/clickpassthrough.aspx") != -1) 
						{
							pos = extract.IndexOf("&redirectUrl=") + 13;
							extract = HttpUtility.UrlDecode(extract.Substring(pos, extract.IndexOf("&", pos) - pos));
						}

                        if (((extract.IndexOf("http://") == -1) || (extract.IndexOf("https://") == -1)) && extract.StartsWith("/"))
                        {
                            extract = ConfigurationManager.AppSettings["RootUrl"] + extract;
                            //throw new Exception("Got: '" + extract + "'");
                        }

						/*if (extract.StartsWith("mailto:")) 
						{
							extract = extract.Substring(7);
						}*/
						
						if (extract.EndsWith("/")) 
						{
							extract = extract.Substring(0, extract.Length - 1);
						}
					}
					else
					{
						include = false;
					}

					break;				
			}

			//Determine if match extract is in exclude list
			if(excludeList != null)
			{
				foreach(string exclude in excludeList)
				{
					if ((extract.ToLower().IndexOf(exclude.ToLower()) != -1) && (extract.ToLower().IndexOf("/images/documents/") == -1))
					{
						include = false;
						break;
					}
				}
			}
			
			if(include)
			{
				// change &sect to &amp;sect b/c even though it doesn't have a semicolon IE interprets it as §
				extract = extract.Replace("&sect", "&amp;sect");

				//store match in hashtable
				if(!extractHash.ContainsKey(extract))
				{
					extractHash.Add(extract, ++footnoteCount);
					hashIndex.Add(extract);
				}
			
				//return match with superscript link to footnote 
				return mt.Value + " <a href=\"#footnote" + extractHash[extract] + "\"><span class=\"blacktext\"><sup>" + extractHash[extract] + "</sup></span></a>";
			}
			else
			{
				return mt.Value;
			}
		}

		/// <summary>
		/// Method builds HTML table containing footnotes
		/// </summary>
		/// <param name="title">text title for footnote table</param>
		/// <returns>HTML table of footnotes</returns>
		public string GetFootnotes(string title)
		{
			string footnotes = "";

			if(extractHash.Keys.Count > 0)
			{
				footnotes = "<BR><BR><a name=\"" + title + "\"></a><h2>" + title + "</h2><P>";
				footnotes += "<table border=\"0\" cellspacing=\"2\" cellpadding=\"0\">\n";

				for(int i = 0; i < hashIndex.Count; i++)
				{
					footnotes += "<tr><td valign=\"top\"><a name=\"footnote" + extractHash[hashIndex[i]] + "\"></a><b><sup>" + extractHash[hashIndex[i]] + "</sup></b></td><td valign=\"top\">" + hashIndex[i] + "</td></tr>\n";							
				}
				footnotes += "</table>\n";
			}

			return footnotes;
		}

		/// <summary>
		/// Method builds HTML table containing footnotes
		/// </summary>
		/// <param name="title">text title for footnote table</param>
		/// <returns>HTML table of footnotes</returns>
		public string GetFootnotes(string title, int wrapWidth)
		{
			string footnotes = "";

			if(extractHash.Keys.Count > 0)
			{
				footnotes = "<BR><BR><a name=\"" + title + "\"></a><h2>" + title + "</h2><P>";
				footnotes += "<table border=\"0\" cellspacing=\"2\" cellpadding=\"0\">\n";

				for(int i = 0; i < hashIndex.Count; i++)
				{
					footnotes += "<tr><td valign=\"top\"><a name=\"footnote" + extractHash[hashIndex[i]] + "\"></a><b><sup>" + extractHash[hashIndex[i]] + "</sup></b></td><td valign=\"top\">" + Functions.TruncLine(hashIndex[i].ToString(), wrapWidth, true, true) + "</td></tr>\n";							
				}
				footnotes += "</table>\n";
			}

			return footnotes;
		}

		#region StripReturnToTopBar method

		/// <summary>
		/// Method defines regular expression for finding return to top bars, removes 
		/// return to top bars from source document
		/// </summary>
		/// <param name="htmlDoc">Source document</param>
		/// <returns>Source document with return to top bars removed</returns>
		public string StripReturnToTopBar(string htmlDoc)
		{
			int keywordIndex;
			int startIndex = 0;
			int endIndex = 0;

			Regex exp = new Regex("<a\\s+?href=\"(\\w+?)?#top\">.+?return.{1}to.{1}top.+?</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
			Regex expTable = new Regex("<table", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
			Regex expEndTable = new Regex("</table>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

			MatchCollection expMatchCol = exp.Matches(htmlDoc);
			MatchCollection tmpMatchCol;
			
			for(int i = expMatchCol.Count - 1; i >= 0; i--)
			{
				keywordIndex = expMatchCol[i].Index;
			
				//Get start of return to top bar
				tmpMatchCol = expTable.Matches(htmlDoc.Substring(0, keywordIndex), 0);

				if(tmpMatchCol.Count >= 2)
				{
					startIndex = tmpMatchCol[tmpMatchCol.Count - 2].Index;
				}
				else
				{
					startIndex = 0;
				}

				//Get end of return to top bar
				tmpMatchCol = expEndTable.Matches(htmlDoc, keywordIndex);

				endIndex = expEndTable.Match(htmlDoc, keywordIndex).Index + 8;
				endIndex = expEndTable.Match(htmlDoc, endIndex).Index;

				if(endIndex > startIndex && startIndex != -1 && endIndex != -1)
				{
					htmlDoc = htmlDoc.Remove(startIndex, endIndex + 8 - startIndex);
				}
				startIndex = 0;
				endIndex = 0;
				keywordIndex = 0;
			}

			htmlDoc = htmlDoc.Replace("<br clear=all>", "");
			return htmlDoc.Replace("<p><table cellpadding=0 cellspacing=0 border=0 width=100%><tr><td valign=top></td><td align=right valign=bottom><a href=\"#top\" style=\"color: #666699; font-size: 9pt;\">return to top<img src=\"/images/arrow_up.gif\" alt=\"\" border=0></a></td></tr><tr><td bgcolor=#86CCCE background=\"/images/line_bg.gif\"><img src=\"/images/line_left.gif\" alt=\"\" ></td><td align=right bgcolor=#86CCCE background=\"/images/line_bg.gif\"><img src=\"/images/line_right.gif\" alt=\"\" ></td></tr></table>", "");
		}

		#endregion
	}
}