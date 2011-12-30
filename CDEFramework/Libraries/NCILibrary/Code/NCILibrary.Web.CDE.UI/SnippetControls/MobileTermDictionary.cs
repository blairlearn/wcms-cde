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
        static ArrayList azListLettersWithData = TermDictionaryManager.GetAZListLettersWithData("english");

        public static string AZBlock(string url)
        {
          
             string addOn = "";
            StringBuilder azBlock = new StringBuilder();
            azBlock.AppendLine("<table border=\"0\" cellpadding=\"5\" cellspacing=\"0\" width=\"100%\"> ");
            azBlock.AppendLine("<tbody> ");
            azBlock.AppendLine("<tr> ");

            if (azListLettersWithData.IndexOf("#") > -1)
                azBlock.AppendLine("    <td><strong><a href=\"" + url + "?expand=%23\" >#</a></strong></td> ");
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

                if (azListLettersWithData.IndexOf(letter.ToString().ToUpper()) > -1)
                    azBlock.AppendLine("    <td><strong><a href=\"" + url + "?expand=" + letter.ToString() + "\" >" + letter.ToString() + "</a></strong></td>" + addOn);
                else
                    azBlock.AppendLine("    <td><strong>" + letter.ToString() + "</strong></td>" + addOn);

            }   
            azBlock.AppendLine("</tr>");
            azBlock.AppendLine("</tbody>");
            azBlock.AppendLine("</table>");

            return azBlock.ToString();
        }

        public static string SearchBlock(string url, string searchString)
        {
            StringBuilder searchBlock = new StringBuilder();

            searchBlock.AppendLine("<script type=\"text/javascript\">");
            searchBlock.AppendLine("function DoSearch()");
            searchBlock.AppendLine("{");
            searchBlock.AppendLine("    document.body.className = 'wait'");
            searchBlock.AppendLine("    var url = $('#litPageUrl').text() + \"?search=\" + $('#searchString').val();");
            searchBlock.AppendLine("    $(location).attr('href',url);");
            searchBlock.AppendLine("}");
            searchBlock.AppendLine("</script>");
            searchBlock.AppendLine("<table border=\"0\" cellpadding=\"2\" cellspacing=\"0\" width=\"100%\">");
            searchBlock.AppendLine("<tbody>");
            searchBlock.AppendLine("<tr>");
            searchBlock.AppendLine("    <td></td>");
            searchBlock.AppendLine("    <td><span class=\"heading1\">Dictionary of Cancer Terms</span></td>");       
            searchBlock.AppendLine("</tr>");
            searchBlock.AppendLine("<tr>");
            searchBlock.AppendLine("    <td></td>");
            searchBlock.AppendLine("    <td width=\"90%\">");
            if (searchString != "")
                searchBlock.AppendLine("        <input name=\"searchString\" id=\"searchString\" type=\"text\" value=\"" + searchString + "\" runat=\"server\" onkeypress=\"if(event.keyCode==13) DoSearch();\" />");
            else
                searchBlock.AppendLine("        <input name=\"searchString\" id=\"searchString\" type=\"text\" runat=\"server\" onkeypress=\"if(event.keyCode==13) DoSearch();\" />");
            searchBlock.AppendLine("    </td>");
            searchBlock.AppendLine("    <td></td>");
            searchBlock.AppendLine("    <td width=\"5%\">");
            searchBlock.AppendLine("        <button class=\"ui-btn-hidden\" ");
            searchBlock.AppendLine("            data-theme=\"a\" ");
            searchBlock.AppendLine("            type=\"submit\" "); 
            searchBlock.AppendLine("            aria-disabled=\"false\" "); 
            searchBlock.AppendLine("            onclick=\"DoSearch();\">Search</button>");
            searchBlock.AppendLine("    </td>");
            if (url != "")
                searchBlock.AppendLine("    <td width=\"2px\"><a id=\"azLink\" class=\"az\" name=\"azLink\" visible=\"false\" href=\"" + url + "\">A-Z</a></td>");
            else    
                searchBlock.AppendLine("    <td width=\"2px\"></td>");
            
            
            searchBlock.AppendLine("    <td></td>");
            searchBlock.AppendLine("    <td></td>");
            searchBlock.AppendLine("</tr>");
            //searchBlock.AppendLine("<tr style=\"height:12px\"> ");
            //searchBlock.AppendLine("</tr>");
            searchBlock.AppendLine("</tbody>");
            searchBlock.AppendLine("</table>");

            return searchBlock.ToString();
        }
    }
}
