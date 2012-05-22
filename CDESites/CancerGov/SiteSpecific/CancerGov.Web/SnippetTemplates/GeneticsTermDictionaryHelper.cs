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

            searchBlock.AppendLine("<div class='dictionary-box'>");
            searchBlock.AppendLine("   <div class='row1'>");
            searchBlock.AppendLine("      <div id='dictionary_jPlayer'></div>");
            searchBlock.AppendLine("      <input class=\"dictionary\" id=\"searchString\" maxlength=\"255\" name=\"searchString\" onblur=\"bSearchBoxBool=false;\" onfocus=\"bSearchBoxBool=true;\" onkeypress=\"if(event.keyCode==13) DoSearch();\" value=\"" + searchString + "\" /> ");
            searchBlock.AppendLine("      <input type='image' name='btnGo' id='btnGo' title='Search' class='go-button' Name='btnGo' src='/PublishedContent/Images/Images/red_search_button.gif' alt='Search' style='border-width:0px;' onclick='DoSearch();' />");
            if (contains)
            {
                searchBlock.AppendLine("      <span class='starts-with-radio' Name='radioStarts'><input id='radioStarts' name='radioGroup' type='radio' onchange='autoFunc();' /></span>");
                searchBlock.AppendLine("      <label for='radioStarts' id='lblStartsWith' class='starts-with-label'>Starts with</label>");
                searchBlock.AppendLine("      <span class='contains-radio' Name='radioContains'><input id='radioContains' name='radioGroup' type='radio' checked='checked' onchange='autoFunc();'  /></span>");
                searchBlock.AppendLine("      <label for='radioContains' id='lblContains' class='contains-label'>Contains</label>");

            }
            else
            {
                searchBlock.AppendLine("      <span class='starts-with-radio' Name='radioStarts'><input id='radioStarts' name='radioGroup' type='radio' checked='checked' onchange='autoFunc();' /></span>");
                searchBlock.AppendLine("      <label for='radioStarts' id='lblStartsWith' class='starts-with-label'>Starts with</label>");
                searchBlock.AppendLine("      <span class='contains-radio' Name='radioContains'><input id='radioContains' name='radioGroup' type='radio' onchange='autoFunc();' /></span>");
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

            return searchBlock.ToString();
        }
      
        public static void DetermineLanguage(string langParam, out string language, out string pageTitle, out string buttonText, out string reDirect)
        {
            //Currently the Genetics Term Dictionary is only in English 

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

