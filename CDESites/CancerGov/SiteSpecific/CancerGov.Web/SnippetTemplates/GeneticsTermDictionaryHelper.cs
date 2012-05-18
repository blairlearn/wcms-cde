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

        public static string SearchBlock()
        {
            StringBuilder searchBlock = new StringBuilder();
            searchBlock.AppendLine("<div class='dictionary-box'>");
            searchBlock.AppendLine("<input id='AutoComplete1' class='dictionary' Name='AutoComplete1' type='text' />");
            searchBlock.AppendLine("</div>");


            //searchBlock.AppendLine("<div class='dictionary-box'>");
            //searchBlock.AppendLine("<div class='row1'>");
            //searchBlock.AppendLine("<form name='aspnetForm' method='post' action='/dictionary id='aspnetForm' onsubmit='NCIAnalytics.TermsDictionarySearch(this,false);'>");
            //searchBlock.AppendLine("<label for='AutoComplete1' class='search-label'>Search for</label>");
            //searchBlock.AppendLine("<input id='AutoComplete1' class='drug-dictionary' Name='AutoComplete1' autocomplete='off' type='text' />");
            //searchBlock.AppendLine("<input type='image' id='btnGo' title='Search' class='go-button' Name='btnGo' src='http://www.cancer.gov/images/red_go_button.gif' alt='Search' ");/>
            //searchBlock.AppendLine("<input id='ctl20_radioStarts' type='radio' name='StartsWith' value='radioStarts' checked='checked' onclick='toggleSearchMode(event, \'ctl20_AutoComplete1\', false);' class='starts-with-radio' />");
            //searchBlock.AppendLine("<label for='StartsWith' class='starts-with-label'>Starts with</label>");
            //searchBlock.AppendLine("<span title='Search item contains this' Name='radioContains' onmouseover='keepListBox(event, \'AutoComplete1\', true)' onmouseout='keepListBox(event, \'AutoComplete1\', false)">");
            //searchBlock.AppendLine("<input id="ctl20_radioContains" type="radio" name="Contains" value="radioContains" onclick="toggleSearchMode(event, 'ctl20_AutoComplete1', true);" class="contains-radio" />
            //searchBlock.AppendLine("<label for="Contains" class="contains-label">Contains</label></span>
            //searchBlock.AppendLine("</form>
            //searchBlock.AppendLine("</div>
            //searchBlock.AppendLine("<div class="row2">
            //searchBlock.AppendLine("<ul>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=%23" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'#') >#</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=A" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'A') >A</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=B" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'B') >B</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=C" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'C') >C</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=D" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'D') >D</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=E" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'E') >E</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=F" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'F') >F</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=G" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'G') >G</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=H" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'H') >H</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=I" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'I') >I</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=J" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'J') >J</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=K" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'K') >K</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=L" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'L') >L</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=M" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'M') >M</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=N" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'N') >N</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=O" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'O') >O</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=P" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'P') >P</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=Q" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'Q') >Q</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=R" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'R') >R</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=S" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'S') >S</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=T" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'T') >T</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=U" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'U') >U</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=V" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'V') >V</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=W" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'W') >W</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=X" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'X') >X</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=Y" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'Y') >Y</a></li>
            //searchBlock.AppendLine("<li><a class="dictionary-alpha-list" href="/dictionary?expand=Z" onclick=NCIAnalytics.TermsDictionarySearchAlphaList(this,'Z') >Z</a></li>
            //searchBlock.AppendLine("</ul>
            //searchBlock.AppendLine("</div>
            //searchBlock.AppendLine("</div>














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
                        NciUrl redirectTo = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl(PageAssemblyInstructionUrls.AltLanguage);
                        reDirect = redirectTo.UriStem.ToString();
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

