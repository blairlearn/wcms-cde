using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CancerGov.CDR.TermDictionary;

//namespace CancerGov.Web.SnippetTemplates
namespace NCI.Web.CDE.UI.SnippetControls
{

    public static class GeneticsTermDictionaryHelper
    {
        public const string ENGLISH = "english";
        public const string SPANISH = "spanish";

        //string url, string searchString, string language, string heading, string buttonText, bool showAZlink)

        public static string SearchBlock(string url, string searchString, string language, string heading, string buttonText, bool contains)
        {

            StringBuilder searchBlock = new StringBuilder();

            searchBlock.AppendLine("<script type=\"text/javascript\">");
            searchBlock.AppendLine("function DoSearch()");
            searchBlock.AppendLine("{");
            searchBlock.AppendLine("   if($('#searchString').val() != \"\") {");
            searchBlock.AppendLine("      var localSearhString = htmlEscape($('#searchString').val());");
            searchBlock.AppendLine("      var isContains=false;");
            searchBlock.AppendLine("      if($(\"#radioContains\").attr(\"checked\")!= \"undefined\")");
            searchBlock.AppendLine("         if($(\"#radioContains\").attr(\"checked\"))");
            searchBlock.AppendLine("           isContains=true;");
            searchBlock.AppendLine("      if(isContains) {");
            searchBlock.AppendLine("         var url = $('#litPageUrl').text() + \"?search=\" + localSearhString + \"&contains=true\";");
            searchBlock.AppendLine("      } else {");
            searchBlock.AppendLine("         var url = $('#litPageUrl').text() + \"?search=\" + localSearhString;");
            searchBlock.AppendLine("      }");
            searchBlock.AppendLine("      $(location).attr('href',url);");
            searchBlock.AppendLine("   }");
            searchBlock.AppendLine("}");
            searchBlock.AppendLine("function htmlEscape(str) {");
            searchBlock.AppendLine("    return String(str)");
            searchBlock.AppendLine("    .replace(/&/g, '&amp;')");
            searchBlock.AppendLine("    .replace(/\"/g, '&quot;')");
            searchBlock.AppendLine("    .replace(/'/g, '&#39;')");
            searchBlock.AppendLine("    .replace(/[(]/g, '&#28;')");
            searchBlock.AppendLine("    .replace(/[)]/g, '&#29;')");
            searchBlock.AppendLine("    .replace(/[?]/g, '&#3f;')");
            searchBlock.AppendLine("    .replace(/</g, '&lt;')");
            searchBlock.AppendLine("    .replace(/>/g, '&gt;');");
            searchBlock.AppendLine("}");
            searchBlock.AppendLine("</script>");

            searchBlock.AppendLine("<div class='dictionary-box'>");
            searchBlock.AppendLine("   <div class='row1'>");
            searchBlock.AppendLine("      <div id='dictionary_jPlayer'></div>");
            searchBlock.AppendLine("      <input class=\"dictionary\" id=\"searchString\" maxlength=\"255\" name=\"searchString\" onblur=\"bSearchBoxBool=false;\" onfocus=\"bSearchBoxBool=true;\" onkeypress=\"if(event.keyCode==13) DoSearch();\" value=\"" + searchString + "\" /> ");
            searchBlock.AppendLine("      <input type='image' name='btnGo' id='btnGo' title='Search' class='go-button' Name='btnGo' src='/PublishedContent/Images/Images/red_search_button.gif' alt='Search' style='border-width:0px;' onclick='DoSearch();' />");
            if (contains)
            {
                searchBlock.AppendLine("      <span class='starts-with-radio' Name='radioStarts'><input id='radioStarts' name='radioGroup' type='radio' /></span>");
                searchBlock.AppendLine("      <label for='radioStarts' id='lblStartsWith' class='starts-with-label'>Starts with</label>");
                searchBlock.AppendLine("      <span class='contains-radio' Name='radioContains'><input id='radioContains' name='radioGroup' type='radio' checked='checked' /></span>");
                searchBlock.AppendLine("      <label for='radioContains' id='lblContains' class='contains-label'>Contains</label>");

            }
            else
            {
                searchBlock.AppendLine("      <span class='starts-with-radio' Name='radioStarts'><input id='radioStarts' name='radioGroup' type='radio' checked='checked' /></span>");
                searchBlock.AppendLine("      <label for='radioStarts' id='lblStartsWith' class='starts-with-label'>Starts with</label>");
                searchBlock.AppendLine("      <span class='contains-radio' Name='radioContains'><input id='radioContains' name='radioGroup' type='radio' /></span>");
                searchBlock.AppendLine("      <label for='radioContains' id='lblContains' class='contains-label'>Contains</label>");
            }


            searchBlock.AppendLine("   </div>");
            searchBlock.AppendLine("   <div class='row2'>");
            searchBlock.AppendLine("      <ul>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=%23'>#</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=A'>A</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=B'>B</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=C'>C</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=D'>D</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=E'>E</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=F'>F</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=G'>G</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=H'>H</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=I'>I</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=J'>J</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=K'>K</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=L'>L</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=M'>M</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=N'>N</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=O'>O</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=P'>P</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=Q'>Q</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=R'>R</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=S'>S</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=T'>T</a></li>");
            searchBlock.AppendLine("            <li><a class='dictionary-alpha-list' href='" + url + "?expand=U'>U</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=V'>V</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=W'>W</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=X'>X</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=Y'>Y</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=Z'>Z</a></li>");
            searchBlock.AppendLine("			<li><a class='dictionary-alpha-list' href='" + url + "?expand=All'>All</a></li>");
            searchBlock.AppendLine("		</ul>");
            searchBlock.AppendLine("	</div>");
            searchBlock.AppendLine("</div>");



 









            //searchBlock.AppendLine("<script src=\"/js/sw-mtd-autocomplete.js\" type=\"text/javascript\"></script>");
            //searchBlock.AppendLine("<script type=\"text/javascript\">");
            //searchBlock.AppendLine("function DoSearch()");
            //searchBlock.AppendLine("{");
            //searchBlock.AppendLine("    if($('#searchString').val() != \"\") {");
            //searchBlock.AppendLine("           var localSearhString = htmlEscape($('#searchString').val());");
            //if (language == MobileTermDictionary.SPANISH)
            //    searchBlock.AppendLine("       NCIAnalytics.TermsDictionarySearchMobile(this,localSearhString, true);");
            //else
            //    searchBlock.AppendLine("       NCIAnalytics.TermsDictionarySearchMobile(this,localSearhString, false);");
            //searchBlock.AppendLine("       var url = $('#litPageUrl').text() + \"?search=\" + localSearhString;");
            //searchBlock.AppendLine("       $(location).attr('href',url);");
            //searchBlock.AppendLine("    }");
            //searchBlock.AppendLine("}");
            //searchBlock.AppendLine("function htmlEscape(str) {");
            //searchBlock.AppendLine("    return String(str)");
            //searchBlock.AppendLine("    .replace(/&/g, '&amp;')");
            //searchBlock.AppendLine("    .replace(/\"/g, '&quot;')");
            //searchBlock.AppendLine("    .replace(/'/g, '&#39;')");
            //searchBlock.AppendLine("    .replace(/[(]/g, '&#28;')");
            //searchBlock.AppendLine("    .replace(/[)]/g, '&#29;')");
            //searchBlock.AppendLine("    .replace(/[?]/g, '&#3f;')");
            //searchBlock.AppendLine("    .replace(/</g, '&lt;')");
            //searchBlock.AppendLine("    .replace(/>/g, '&gt;');");
            //searchBlock.AppendLine("}");
            //searchBlock.AppendLine("</script>");
            //searchBlock.AppendLine("<table width=\"100%\">");
            //searchBlock.AppendLine("<tr>");
            //searchBlock.AppendLine("<td>");
            //searchBlock.AppendLine("<input class=\"searchString\" id=\"searchString\" maxlength=\"255\" name=\"searchString\" onblur=\"bSearchBoxBool=false;\" onfocus=\"bSearchBoxBool=true;\" onkeypress=\"if(event.keyCode==13) DoSearch();\" value=\"" + searchString + "\" /> ");
            //searchBlock.AppendLine("</td>");
            //searchBlock.AppendLine("<td width=\"40\">");


            //if (language == MobileTermDictionary.SPANISH)
            //    searchBlock.AppendLine("<input alt=\"Search\" data-theme=\"a\" class=\"searchSubmit\" id=\"dctSearch\" onclick=\"DoSearch();\" type=\"submit\" value=\"Buscar\" />");
            //else
            //    searchBlock.AppendLine("<input alt=\"Search\" data-theme=\"a\" class=\"searchSubmit\" id=\"dctSearch\" onclick=\"DoSearch();\" type=\"submit\" value=\"Search\" />");
            //searchBlock.AppendLine("</td>");
            //searchBlock.AppendLine("</tr>");
            //searchBlock.AppendLine("<tr>");
            //if (showAZlink)
            //{
            //    searchBlock.AppendLine("<td>");
            //    searchBlock.AppendLine("<a id=\"azLink\" class=\"mtd_az\" name=\"azLink\" visible=\"false\" href=\"" + url + "\">A-Z</a>");
            //    searchBlock.AppendLine("</td>");
            //}
            //searchBlock.AppendLine("</tr>");
            //searchBlock.AppendLine("</table>");
            return searchBlock.ToString();
        }

        public static void DetermineLanguage(string langParam, out string language, out string pageTitle, out string buttonText, out string reDirect)
        {

            if (langParam == null)
                langParam = "";

            reDirect = "";

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
                    if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                    {
                        //NciUrl redirectTo = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl(PageAssemblyInstructionUrls.AltLanguage);
                        //reDirect = redirectTo.UriStem.ToString();
                    }

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

