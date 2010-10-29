using System;
using System.Collections;
using System.Text;

using CancerGov.CDR.DataManager;
using CancerGov.Text;
using NCI.Web.CDE;

namespace CancerGov.UI.CDR
{
	/// <summary>
	/// Summary description for AdvancedSearchResultRenderer.
	/// </summary>
	public class AdvancedSearchResultRenderer : IRenderer {

		private ProtocolCollection pcProtocols;
		private ArrayList alCDRIDs;
        private DisplayInformation displayInfo;
        private ProtocolDisplayFormats displayFormat;

        public AdvancedSearchResultRenderer(DisplayInformation displayInfo, ProtocolCollection protocols,
            int[] checkedCDRIDs, ProtocolDisplayFormats displayFormat)
        {

            this.pcProtocols = protocols;
            this.displayInfo = displayInfo;
            this.displayFormat = displayFormat;

            alCDRIDs = new ArrayList(checkedCDRIDs);
        }

        public string Render()
        {
            StringBuilder sbContent = new StringBuilder();
            bool isFirstProtocol = true;

            sbContent.Append("<table width=\"100%\" cellspacing=\"8\" cellpadding=\"0\" class=\"clinicaltrials-resultTable\">\n");

            foreach (Protocol pProto in this.pcProtocols)
            {
                if (!isFirstProtocol)
                {
                    sbContent.Append("<tr>\n");
                    sbContent.Append("<td colspan=\"2\" valign=\"top\"><hr></td>\n");
                    sbContent.Append("</tr>\n");
                }
                else
                {
                    isFirstProtocol = false;
                }

                sbContent.Append("<tr>\n");

                sbContent.AppendFormat("<td class=\"selectColumn\"><span>{0}.</span><br /><input type=\"checkbox\" name=\"cdrid\" class=\"cdridbox\" id=\"cdrid{1}\" value=\"{1}\" {2} ></td>\n",
                    pProto.ResultNumber, pProto.CdrId, alCDRIDs.Contains(pProto.CdrId) ? "checked" : "");

                sbContent.Append("<td width=\"100%\">\n");
                sbContent.Append(new AdvancedSearchProtocolRenderer(displayInfo, pProto, this.alCDRIDs, displayFormat).Render());
                sbContent.Append("</td>\n");
                sbContent.Append("</tr>\n");

                // Only allow the "Select Trial Above" checkbox for formats other than Short/Title-only. [FR9255-050]
                if (displayFormat != ProtocolDisplayFormats.Short)
                {
                    sbContent.AppendFormat("<tr id=\"cdrid_mirror_row{0}\" class=\"cdrid_mirror_row\" style=\"display:none;\"><td valign=\"top\" colspan=\"2\">", pProto.CdrId);
                    sbContent.AppendFormat("<input type=\"checkbox\" name=\"cdrid_mirror\"  id=\"cdrid_mirror{0}\" value=\"{0}\" {1} >\n",
                        pProto.CdrId, alCDRIDs.Contains(pProto.CdrId) ? "checked" : "");
                    sbContent.AppendFormat("<label for=\"cdrid_mirror{0}\"><strong>Select Trial Above</strong></label></td></tr>\n", pProto.CdrId);
                }
            }

            sbContent.Append("</table>\n");

            // Only allow the "Select Trial Above" checkbox for formats other than Short/Title-only. [FR9255-050]
            if (displayFormat != ProtocolDisplayFormats.Short)
            {

                // Create JavaScript code to reveal and initialize mirror checkboxes.
                StringBuilder sbCdrids = new StringBuilder();
                isFirstProtocol = true;

                sbContent.Append("<script type=\"text/javascript\">\ndocument.observe(\"dom:loaded\", function() {\n");
                
                // Reveal the mirror rows
                sbContent.Append("$$(\".cdrid_mirror_row\").invoke(\"show\");\n");

                // Join the checkboxes to their mirrors
                sbContent.Append("$$(\".cdridbox\").each(function(item){CreateCheckboxMirror(\"cdrid\" + item.value, \"cdrid_mirror\" + item.value);});\n");

                sbContent.Append("});\n</script>\n");
            }

            if (this.pcProtocols.Count > 0)
            {
                sbContent.Append("<script type=\"text/javascript\">document.observe(\"dom:loaded\", function() {");
                sbContent.Append("SetupTitleClickHandler();");
                sbContent.Append("});</script>\n");
            }

            return sbContent.ToString();
        }

	}

}

