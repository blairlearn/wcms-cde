using System;
using System.Text;

using CancerGov.CDR.DataManager;
using NCI.Web.CDE;

namespace CancerGov.UI.CDR
{
    /// <summary>
    /// This class renders a ProtocolCollection for printable search results
    /// </summary>
    public class PrintSearchResultsRenderer : IRenderer
    {

        private ProtocolCollection pcProtocols;
        private bool bRenderPageBreaks = true;
        private DisplayInformation displayInfo;
        private ProtocolDisplayFormats displayFormat;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="basePage">A BasePage object</param>
        /// <param name="protocols">A collection of protocols</param>
        /// <param name="renderPageBreaks">True if you want page breaks displayed after each trial</param>
        public PrintSearchResultsRenderer(DisplayInformation displayInfo, ProtocolCollection protocols,
            ProtocolDisplayFormats displayFormat, bool renderPageBreaks)
        {
            this.bRenderPageBreaks = renderPageBreaks;
            this.displayInfo = displayInfo;
            this.pcProtocols = protocols;
            this.displayFormat = displayFormat;
        }

        /// <summary>
        /// Renders the protocol out to a string of html
        /// </summary>
        /// <returns>A string of HTML</returns>
        public string Render()
        {
            StringBuilder sbContent = new StringBuilder();
            bool IsFirstProtocol = true;

            sbContent.Append("<table width=\"100%\" cellspacing=\"8\" cellpadding=\"0\" border=\"0\">\n");

            int protocolCount = pcProtocols.Count;
            //foreach (Protocol pProto in this.pcProtocols)

            string pagebreakStyle = string.Empty;
            if (bRenderPageBreaks)
                pagebreakStyle = "style=\"page-break-after: always;\"";

            for (int i = 0; i < protocolCount; ++i)
            {
                if (!IsFirstProtocol)
                {
                    sbContent.AppendFormat("<tr {0}>\n", pagebreakStyle);
                    sbContent.Append("<td colspan=\"2\" valign=\"top\"><br /></td>\n");
                    sbContent.Append("</tr>\n");
                }
                else
                {
                    IsFirstProtocol = false;
                }

                sbContent.Append("<tr valign=\"top\">\n");

                sbContent.AppendFormat("<td valign=\"top\">{0}.</td>\n", i + 1);

                sbContent.Append("<td valign=\"top\" width=\"100%\">\n");
                sbContent.Append(new PrintProtocolRenderer(displayInfo, pcProtocols[i], displayFormat).Render());

                sbContent.Append("</td>\n");
                sbContent.Append("</tr>\n");
            }

            sbContent.Append("</table>\n");



            return sbContent.ToString();
        }

    }

}
