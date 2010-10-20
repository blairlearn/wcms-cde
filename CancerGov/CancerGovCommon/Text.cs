using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Xml;
using CancerGov.Common;
using System.Text.RegularExpressions;

namespace CancerGov.Text
{
    public class Strings
    {
        private static Regex _entityRegex = new Regex(@"&[a-zA-Z0-9]{2,8};|&#\d+;", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);


        /// <summary>
        /// Converts string integer to Int32
        /// </summary>
        /// <param name="intString"></param>
        /// <returns>int, -1</returns>
        public static int ToInt(string intString)
        {
            int i = -1;

            try
            {
                i = Convert.ToInt32(intString.Trim());
            }
            catch (Exception)
            {
            }

            return i;
        }

        public static int ToInt(string intString, int defaultValue)
        {
            int i = defaultValue;

            try
            {
                i = Convert.ToInt32(intString.Trim());
            }
            catch (Exception)
            {
            }

            return i;
        }

        /// <summary>
        /// Converts string boolean to type bool
        /// </summary>
        /// <param name="boolString"></param>
        /// <returns>bool, false</returns>
        /// <summary>
        /// Converts string boolean to type bool
        /// </summary>
        /// <param name="boolString"></param>
        /// <returns>bool, false</returns>
        public static bool ToBoolean(string boolString)
        {
            bool b = false;

            if (boolString == null)
            {
                return false;
            }

            if (boolString.ToUpper() == "YES")
            {
                return true;
            }

            if (boolString.ToUpper() == "NO")
            {
                return false;
            }

            if (boolString.ToUpper() == "Y")
            {
                return true;
            }

            if (boolString.ToUpper() == "N")
            {
                return false;
            }

            if (boolString.ToUpper() == "1")
            {
                return true;
            }

            if (boolString.ToUpper() == "0")
            {
                return false;
            }


            try
            {

                b = Convert.ToBoolean(boolString.Trim());
            }
            catch (Exception)
            {
            }

            return b;
        }



        /// <summary>
        /// Converts string date to type DateTime
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns>DateTime, 00/00/0000</returns>
        public static DateTime ToDateTime(string dateTimeString)
        {
            DateTime dt = new DateTime(0);

            try
            {
                dt = Convert.ToDateTime(dateTimeString.Trim());
            }
            catch (Exception)
            {
            }

            return dt;
        }

        /// <summary>
        /// Converts string guid to type Guid
        /// </summary>
        /// <param name="guidString"></param>
        /// <returns>Guid, null</returns>
        public static Guid ToGuid(string guidString)
        {
            Guid g;

            try
            {
                g = new Guid(guidString.Trim());
            }
            catch (Exception)
            {
                g = System.Guid.Empty;
            }

            return g;
        }

        /// <summary>
        /// Trims string, returns null for both null and zero length strings
        /// </summary>
        /// <param name="val"></param>
        /// <returns>null or trimmed string</returns>
        public static string Clean(string val)
        {
            if (val == null || val.Trim() == String.Empty)
            {
                return null;
            }
            else
            {
                return val.Trim();
            }
        }

        /// <summary>
        /// Substitutes default string for a null string
        /// </summary>
        /// <param name="val">Test string</param>
        /// <param name="valDefault">Default string</param>
        /// <returns>Test string or default string</returns>
        public static string IfNull(string val, string valDefault)
        {
            if (val == null)
            {
                return valDefault;
            }
            else
            {
                return val;
            }
        }


        public static ArrayList ToArrayListOfInts(string val, char separator)
        {
            ArrayList alList = new ArrayList();

            if (val != null)
            {
                foreach (string strItem in val.Split(separator))
                {
                    alList.Add(Strings.ToInt(strItem));
                }
            }

            return alList;
        }

        public static ArrayList ToArrayListOfStrings(string val, char separator)
        {

            ArrayList alList = new ArrayList();

            if (val != null)
            {
                foreach (string strItem in val.Split(separator))
                {
                    alList.Add(strItem);
                }
            }

            return alList;
        }

        /// <summary>
        /// Converts a string to a DisplayDateMode type
        /// </summary>
        /// <param name="displayDateMode"></param>
        /// <returns>DisplayDateMode</returns>
        public static DisplayDateMode ToDisplayDateMode(string displayDateMode)
        {
            DisplayDateMode mode;

            //throw new Exception("--" + Strings.IfNull(Strings.Clean(displayDateMode.Trim()), "0").Trim() +"--");

            switch (Strings.IfNull(Strings.Clean(displayDateMode.Trim()), "0").Trim())
            {
                case "0":
                    {
                        mode = DisplayDateMode.None;
                        break;
                    }
                case "1":
                    {
                        mode = DisplayDateMode.Posted;
                        break;
                    }
                case "2":
                    {
                        mode = DisplayDateMode.Updated;
                        break;
                    }
                case "3":
                    {
                        mode = DisplayDateMode.PostedUpdated;
                        break;
                    }
                case "4":
                    {
                        mode = DisplayDateMode.Reviewed;
                        break;
                    }
                case "5":
                    {
                        mode = DisplayDateMode.PostedReviewed;
                        break;
                    }
                case "6":
                    {
                        mode = DisplayDateMode.UpdatedReviewed;
                        break;
                    }
                case "7":
                    {
                        mode = DisplayDateMode.All;
                        break;
                    }
                default:
                    {
                        mode = DisplayDateMode.None;
                        break;
                    }

                #region old date setting
                /*
			switch (Strings.IfNull(Strings.Clean(displayDateMode), "NONE").Trim().ToUpper()) 
			{
				case "UPDATED" : 
				{
					mode = DisplayDateMode.Updated;
					break;
				}
				case "POSTED" : 
				{
					mode = DisplayDateMode.Posted;
					break;
				}
				case "NONE" : 
				{
					mode = DisplayDateMode.None;
					break;
				}
				case "BOTH" : 
				{
					mode = DisplayDateMode.Both;
					break;
				}
				default : 
				{
					mode = DisplayDateMode.None;
					break;
				}
					*/
                #endregion
            }

            return mode;
        }

        public static string GetNumericSuffixFormat(string num)
        {
            int tens = 0;
            string suffix = "";

            try
            {
                tens = Convert.ToInt16(num.Substring(num.Length - 2, 1));
            }
            catch
            {
            }

            if (tens == 1)
            {
                suffix = "\\t\\h";
            }
            else
            {
                switch (num.Substring(num.Length - 1, 1))
                {
                    case "0":
                        suffix = "\\t\\h";
                        break;
                    case "1":
                        suffix = "\\s\\t";
                        break;
                    case "2":
                        suffix = "\\n\\d";
                        break;
                    case "3":
                        suffix = "\\r\\d";
                        break;
                    case "4":
                        suffix = "\\t\\h";
                        break;
                    case "5":
                        suffix = "\\t\\h";
                        break;
                    case "6":
                        suffix = "\\t\\h";
                        break;
                    case "7":
                        suffix = "\\t\\h";
                        break;
                    case "8":
                        suffix = "\\t\\h";
                        break;
                    case "9":
                        suffix = "\\t\\h";
                        break;
                }
            }

            return suffix;
        }

        public static string Wrap(string source, int charWidth, string wrapChar)
        {
            int index = charWidth;

            while (index < source.Length)
            {
                source = source.Insert(index, wrapChar);
                index += charWidth + wrapChar.Length;
            }

            return source;
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

        /// <summary>
        /// Processes input XHTML markup and returns plain text by removing all tags and converting 
        /// entities into the corresponding Unicode characters.  
        /// 
        /// This function is being created for the purpose of converting HTML in a description to 
        /// plain text suitable for use in a meta description tag attribute value.  There are two 
        /// things this method does that are specific to the current requirements: (1) it does not 
        /// convert the less than entity "&lt;" to "<" since that would break when we try to load 
        /// the XHTML into the XML document and we want to be able to let content authors include 
        /// the "<" character in their descriptions - they just have to use the entity to do so; 
        /// and (2) after all tags are removed and entities converted, it re-encodes double quotes 
        /// as an entity so that the output can be used inside an attribute that is itself defined 
        /// by starting and ending double quotes, i.e. we're outputting 
        /// <meta ... description="the value here" />
        /// so we have to ensure that any double quotes inside the description attribute value are 
        /// escaped so they don't close the attribute.
        /// 
        /// Therefore, *** BEFORE USING THIS FUNCTION *** for purposes other than the one it was 
        /// initially designed for, it should be considered whether or not the above two points are 
        /// of any concern.  Also, there are known issues below that should be considered before 
        /// using this function.
        /// 
        /// Known Issue 1: the approach used here will leave no space between sentences that were in 
        /// two adjacent tags, for example: "<p>Sentence one.</p><p>Sentence two.</p>" will result 
        /// in the output: "Sentence one.Sentence two."  This is a known issue but it was decided 
        /// that this is not a large concern right now since it will likely not be an issue and 
        /// in the rare case that it is we can just add a plain text Meta Description tag manually.
        /// 
        /// Known Issue 2: loading into the XML document will fail if the XML is not well formed or if 
        /// there is a < character in the text that is not part of a tag.  In that case we will 
        /// catch the exception and fall back to a more basic approach of using the StripHTMLTags 
        /// method to remove the tags.  That function has its own known issue that its regex could 
        /// capture more text than desired if there is a < in the text somewhere (not part of a tag).
        /// 
        /// Note: Ideally we'd put this function in a more general place outside Cancer.gov 
        /// such as in the NCILibrary but due to current constraints on our ability to version 
        /// that DLL properly we can't change it.  So, we're putting this method here for now 
        /// with the understanding that it should eventually move the NCI Library.
        /// </summary>
        /// <param name="xhtml">The XHTML to convert to plain text.</param>
        /// <returns>A plain text representation of the input XHTML.</returns>
        public static string GetPlainTextFromXhtml(string xhtml)
        {
            // We don't want to throw exceptions from this method so handle null param by short circuiting 
            // the method to return something reasonable for the null input case.
            if (xhtml == null)
            {
                return String.Empty;
            }

            // Decode entities (both named like &copy; and numbered like &#169;).  We're not decoding &lt; here 
            // since that would then break when loading into the XML document below.  We have to do the entity 
            // decoding in general here (before loading into the XML document) so we don't get an error due to 
            // unknown entities when load into the XML document.  &lt; is implicitly understood by the XML 
            // classes so that one is ok to leave in when we load the XML document.
            string text = _entityRegex.Replace(xhtml, new MatchEvaluator(DecodeEntityExceptLessThan));

            try
            {
                // Load the text up into an XML document (surrounding in single root element to make it valid 
                // XML in case it doesn't already have one) and get the text without any tags.
                XmlDocument doc = new XmlDocument();

                // The line below could fail if the document is not well formed, has undeclared entity references, 
                // or has invalid characters like < in the character data that are not entity encoded.
                doc.LoadXml("<d>" + text + "</d>");

                // Get the plain text version of the input XHTML by using the built logic of the InnerText property.
                text = doc.DocumentElement.InnerText;
            }
            catch (XmlException)
            {
                // In the case that this isn't a valid XML document, remove the tags manually with a regex (less 
                // safe than relying on "built in" code and has known issue with StripHTMLTags (see comments above).
                text = StripHTMLTags(text);
            }

            // Re-encode double quotes as an entity.  Note: order is important here - if you do the line below before 
            // loading into the XML doc above, quote entities will be converted back from the entity to the actual character.
            text = text.Replace(@"""", "&quot;");

            return text;
        }

        /// <summary>
        /// Converts one of the 252 HTML 4 entities to the corresponding unicode character, 
        /// e.g. &copy; is converted to ©.  Also converts the XHTML 1.0 entity &apos; to '
        /// Does NOT convert &lt; so can leave it in encoded form if need to load into XML document.
        /// </summary>
        /// <param name="match">A regular expression Match whose value is the entity to decode, for example, &copy;, &apos;, or &#169;.</param>
        /// <returns>The decoded entity, i.e. the unicode character represented by the entity.</returns>
        private static string DecodeEntityExceptLessThan(Match match)
        {
            #region Parameter Validation

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            if (match.Value == null)
            {
                throw new ArgumentException("match.Value cannot be null", "match");
            }

            #endregion

            if (match.Value.ToLower() == "&lt;")
            {
                return match.Value;
            }
            else
            {
                return DecodeEntity(match.Value);
            }
        }

        /// <summary>
        /// Converts one of the 252 HTML 4 entities to the corresponding unicode character, 
        /// e.g. &copy; is converted to ©.  Also converts the XHTML 1.0 entity &apos; to '
        /// </summary>
        /// <param name="entity">The entity to decode, for example, &copy;, &apos;, or &#169;.</param>
        /// <returns>The decoded entity, i.e. the unicode character represented by the entity.</returns>
        private static string DecodeEntity(string entity)
        {
            #region Parameter Validation

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            #endregion

            // Manually convert &apos; since .NET's list of entities used in HtmlDecode is only the 252 from 
            // HTML 4 (which doesn't include &apos;) then delegate to HtmlDecode to handle all other entities.
            if (entity.ToLower() == "&apos;")
            {
                return "'";
            }
            else
            {
                return HttpUtility.HtmlDecode(entity);
            }
        }
    }
}