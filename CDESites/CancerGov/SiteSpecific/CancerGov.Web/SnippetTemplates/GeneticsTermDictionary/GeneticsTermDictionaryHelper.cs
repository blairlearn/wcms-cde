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

            searchBlock.AppendLine("<div class='dictionary-box' id='genetics-terms-dictionary'>");
           
            searchBlock.AppendLine("      <div id='dictionary_jPlayer'></div>");
            searchBlock.AppendLine("     <form id=\"aspnetForm\" name=\"aspnetForm\" method=\"get\" action=\"/geneticsdictionary\" >");
            searchBlock.AppendLine("     <div class='row'>");
            if (contains)
            {
                searchBlock.AppendLine("      <div class=\"medium-2 columns\"><span class='radio' Name='radioStarts'><input id='radioStarts' name='contains'  type='radio' onchange='autoFunc();' />");
                searchBlock.AppendLine("      <label for='radioStarts' class=\"inline\" id='lblStartsWith'>Starts with</label></span></div>");
                searchBlock.AppendLine("      <div class=\"medium-2 columns left\"><span class='radio' Name='radioContains'><input id='radioContains' value=\"true\" name='contains' type='radio' checked='checked' onchange='autoFunc();'  />");
                searchBlock.AppendLine("      <label for='radioContains' class=\"inline\" id='lblContains'>Contains</label></span></div>");

            }
            else 
            {
                searchBlock.AppendLine("      <div class=\"medium-2 columns\"><span class='radio' Name='radioStarts'><input id='radioStarts' name='contains' type='radio'  checked='checked' onchange='autoFunc();' />");
                searchBlock.AppendLine("      <label for='radioStarts' class=\"inline\" id='lblStartsWith' >Starts with</label></span></div>");
                searchBlock.AppendLine("      <div class=\"medium-2 columns left\"><span class='radio' Name='radioContains'><input id='radioContains' name='contains' value=\"true\" type='radio' onchange='autoFunc();' />");
                searchBlock.AppendLine("      <label for='radioContains' class=\"inline\" id='lblContains'>Contains</label></span></div>"); 
           } 
            searchBlock.AppendLine("   </div>");
            searchBlock.AppendLine("      <div class='row'>");
            searchBlock.AppendLine("      <div class=\"medium-6 columns\"><input placeholder=\"Enter keywords or phrases\" autocomplete=\"off\" aria-label=\"Enter keywords or phrases\" aria-autocomplete=\"list\" type=\"text\" class=\"genetics-dictionary\" id=\"searchString\" maxlength=\"255\" name=\"search\" onblur=\"bSearchBoxBool=false;\" onfocus=\"bSearchBoxBool=true;\"  value=\"" + searchString + "\" /> </div>");
            searchBlock.AppendLine("      <div class=\"medium-2 columns left\"><input type='submit'  id='btnGo' title='Search' class='submit button postfix'  /></div>");
            searchBlock.AppendLine("      </div>");

            searchBlock.AppendLine("     </form>");

            
            searchBlock.AppendLine("   <div class='az-list'>");
            searchBlock.AppendLine("      <ul>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=%23\" " + insertWA("#") + " >#</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=A\" " + insertWA("A") + " >A</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=B\" " + insertWA("B") + " >B</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=C\" " + insertWA("C") + " >C</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=D\" " + insertWA("D") + " >D</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=E\" " + insertWA("E") + " >E</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=F\" " + insertWA("F") + " >F</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=G\" " + insertWA("G") + " >G</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=H\" " + insertWA("H") + " >H</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=I\" " + insertWA("I") + " >I</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=J\" " + insertWA("J") + " >J</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=K\" " + insertWA("K") + " >K</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=L\" " + insertWA("L") + " >L</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=M\" " + insertWA("M") + " >M</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=N\" " + insertWA("N") + " >N</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=O\" " + insertWA("O") + " >O</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=P\" " + insertWA("P") + " >P</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=Q\" " + insertWA("Q") + " >Q</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=R\" " + insertWA("R") + " >R</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=S\" " + insertWA("S") + " >S</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=T\" " + insertWA("T") + " >T</a></li>");
            searchBlock.AppendLine("            <li><a  href=\"" + url + "?expand=U\" " + insertWA("U") + " >U</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=V\" " + insertWA("V") + " >V</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=W\" " + insertWA("W") + " >W</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=X\" " + insertWA("X") + " >X</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=Y\" " + insertWA("Y") + " >Y</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=Z\" " + insertWA("Z") + " >Z</a></li>");
            searchBlock.AppendLine("			<li><a  href=\"" + url + "?expand=All\" " + insertWA("ALL") + " >All</a></li>");
            searchBlock.AppendLine("		</ul>");
            searchBlock.AppendLine("	</div>");
            searchBlock.AppendLine("</div>");

            return searchBlock.ToString();
        }

        private static string insertWA(string letter)
        {
            return "onclick=\"NCIAnalytics.GeneticsDictionarySearchAlphaList(this,'" + letter + "');\"";
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

