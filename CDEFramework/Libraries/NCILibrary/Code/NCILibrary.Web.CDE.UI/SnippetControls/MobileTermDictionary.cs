using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CancerGov.CDR.TermDictionary;

namespace NCI.Web.CDE.UI.SnippetControls
{
    
    public static class MobileTermDictionary
    {
        public const string ENGLISH = "english";
        public const string SPANISH = "spanish";


        static ArrayList azListLettersWithData = TermDictionaryManager.GetAZListLettersWithData("english");
        static ArrayList azListLettersWithDataSpanish = TermDictionaryManager.GetAZListLettersWithData("spanish");

        public static string AZBlock(string url,string language)
        {

            ArrayList azList;
            string languageAtribute = "";

            if (language.Trim().ToUpper() == "english")
                azList = azListLettersWithData;
            else
            {
                azList = azListLettersWithDataSpanish;
                languageAtribute = "&language=" + language;
            }


            string addOn = "";
            StringBuilder azBlock = new StringBuilder();
            azBlock.AppendLine("<table border=\"0\" cellpadding=\"5\" cellspacing=\"0\" width=\"100%\"> ");
            azBlock.AppendLine("<tbody> ");
            azBlock.AppendLine("<tr> ");

            //if (azListLettersWithData.IndexOf("#") > -1)
            if (azList.IndexOf("#") > -1)
                    azBlock.AppendLine("    <td><strong><a href=\"" + url + "?expand=%23" + languageAtribute + "\" >#</a></strong></td> ");
            else
                azBlock.AppendLine("    <td><strong>#</strong></td> ");
            
            char letter;
            for (int i = 65; i < 91; i++)
            {
                letter = System.Convert.ToChar(i);
                if (letter == 'H' || letter == 'Q')
                    addOn = "</tr><tr>";
                else
                    addOn = "";

                if (azList.IndexOf(letter.ToString().ToUpper()) > -1)
                    azBlock.AppendLine("    <td><strong><a href=\"" + url + "?expand=" + letter.ToString() +  languageAtribute + "\" >" + letter.ToString() + "</a></strong></td>" + addOn);
                else
                    azBlock.AppendLine("    <td><strong>" + letter.ToString() + "</strong></td>" + addOn);


            }   
            azBlock.AppendLine("</tr>");
            azBlock.AppendLine("</tbody>");
            azBlock.AppendLine("</table>");

            return azBlock.ToString();
        }

        public static string RawUrlClean(string rawUrl)
        {
            int paramStart = rawUrl.IndexOf("?");

            if (paramStart > -1)
                return rawUrl.Substring(0, paramStart);
            else
                return rawUrl;
        }


        public static string SearchBlock(string url, string searchString, string language, string heading, string buttonText, bool showAZlink)
        {
            string languageAtribute = "";
            if (language.Trim().ToLower() != "english")
                languageAtribute = "?language=" + language;

            StringBuilder searchBlock = new StringBuilder();
            searchBlock.AppendLine("<script type=\"text/javascript\">");
            searchBlock.AppendLine("function DoSearch()");
            searchBlock.AppendLine("{");
            searchBlock.AppendLine("    if($('#searchString').val() != \"\") {");
            searchBlock.AppendLine("       document.body.className = 'mtd_wait'");
            if (String.IsNullOrEmpty(language))
                searchBlock.AppendLine("       var url = $('#litPageUrl').text() + \"?search=\" + $('#searchString').val();");
            else
                if (language.Trim().ToLower() != "english")
                    searchBlock.AppendLine("       var url = $('#litPageUrl').text() + \"?search=\" + $('#searchString').val()+ \"&language=" + language + "\";");
                else
                    searchBlock.AppendLine("       var url = $('#litPageUrl').text() + \"?search=\" + $('#searchString').val();");
            searchBlock.AppendLine("       $(location).attr('href',url);");
            searchBlock.AppendLine("    }");
            searchBlock.AppendLine("}");
            searchBlock.AppendLine("</script>");
            searchBlock.AppendLine("<table width=\"100%\">");
            searchBlock.AppendLine("<tr>");
            searchBlock.AppendLine("<td>");
            searchBlock.AppendLine("<input class=\"searchString\" id=\"searchString\" maxlength=\"255\" name=\"searchString\" onblur=\"bSearchBoxBool=false;\" onfocus=\"bSearchBoxBool=true;\" onkeypress=\"if(event.keyCode==13) DoSearch();\" value=\"" + searchString + "\" /> ");
            searchBlock.AppendLine("</td>");
            searchBlock.AppendLine("<td width=\"40\">");
            searchBlock.AppendLine("<input alt=\"Search\" data-theme=\"a\" class=\"searchSubmit\" id=\"dctSearch\" onclick=\"DoSearch();\" type=\"submit\" value=\"Search\" />");
            searchBlock.AppendLine("</td>");
            searchBlock.AppendLine("</tr>");
            searchBlock.AppendLine("<tr>");
            if (showAZlink)
            {
                searchBlock.AppendLine("<td>");
                searchBlock.AppendLine("<a id=\"azLink\" class=\"mtd_az\" name=\"azLink\" visible=\"false\" href=\"" + url + languageAtribute + "\">A-Z</a>");
                searchBlock.AppendLine("</td>");
            }
            searchBlock.AppendLine("</tr>");
            searchBlock.AppendLine("</table>");
            return searchBlock.ToString();
        }


        public static void DetermineLanguage(string langParam, out string language, out string pageTitle, out string buttonText)
        {

            if (langParam == null)
                langParam = "";

            if (PageAssemblyContext.Current.PageAssemblyInstruction == null)
            {
                if (langParam.Trim().ToLower() == SPANISH)
                {
                    language = SPANISH;
                    pageTitle = "Diccionario de cáncer";
                    buttonText = "Buscar";
                }
                else
                {
                    language = ENGLISH;
                    pageTitle = "Dictionary of Cancer Terms";
                    buttonText = "Search";

                }
            }
            else
            {
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es" || langParam.Trim().ToLower() == SPANISH)
                {
                    language = SPANISH;
                    pageTitle = "Diccionario de cáncer";
                    buttonText = "Buscar";
                }
                else
                {
                    language = ENGLISH;
                    pageTitle = "Dictionary of Cancer Terms";
                    buttonText = "Search";

                }
            }
        }
    }
}
