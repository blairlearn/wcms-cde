using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using CancerGov.CDR.DataManager;
using CancerGov.Web;
using NCI.Web.CDE;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ClinicalTrialsViewHeader : NCI.Web.CancerGov.Apps.AppsBaseUserControl
    {
        private string strDates = "";
        private string strPageUrl = "";

        public void Page_Load(object sender, EventArgs e)
        {
            string data = SnippetInfo.Data;
            contentHeader.Text = data;
            strPageUrl = this.PageInstruction.GetUrl("PrettyUrl").ToString() + "?cdrid=" + Request.QueryString["cdrid"] + (Request.QueryString["protocolsearchid"] == null ? "" : "&protocolsearchid=" + Request.QueryString["protocolsearchid"]);
        }

        protected override void OnPreRender(EventArgs e)
        {
            string pvFirstPublished = String.Empty;
            string pvLastModified = String.Empty;

            try
            { pvFirstPublished = PageInstruction.GetField("pvFirstPublished"); }
            catch { }

            try
            { pvLastModified = PageInstruction.GetField("pvLastModified"); }
            catch { }

            if (pvLastModified != String.Empty || pvFirstPublished != String.Empty)
            {
                StringBuilder sbDate = new StringBuilder();

                if (pvLastModified != String.Empty)
                {
                    sbDate.Append("<span class=\"protocol-date-label\">Last Modified: </span>");
                    sbDate.Append("<span class=\"protocol-dates\">");
                    sbDate.Append(pvLastModified);
                    sbDate.Append("</span>");
                    sbDate.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp");
                }
                if (pvFirstPublished != String.Empty)
                {
                    sbDate.Append("<span class=\"protocol-date-label\">First Published: </span>");
                    sbDate.Append("<span class=\"protocol-dates\">");
                    sbDate.Append(pvFirstPublished);
                    sbDate.Append("</span>");
                    sbDate.Append("&nbsp;&nbsp;");
                }
                strDates = sbDate.ToString();
            }
            cdrVersionBar.Text = RenderCDRVesionrBar();
            base.OnPreRender(e);
        }

        private string RenderCDRVesionrBar()
        {

            StringBuilder sbContent = new StringBuilder();



            sbContent.Append("<table width=\"771\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\n");
            sbContent.Append("<tr>\n");
            //sbContent.Append("<td valign=\"top\"><img src=\"/images/spacer.gif\" width=\"10\" height=\"1\" alt=\"\" border=\"0\"></td>\n");

            sbContent.Append("<td align=\"left\" valign=\"top\">");
            if (this.PageDisplayInformation.Version == DisplayVersions.Web)
            {
                if (PVersion == ProtocolVersions.Patient)
                {

                    sbContent.Append("<img src=\"/images/tabs-beginning.gif\" width=\"1\" height=\"21\" alt=\"\" border=\"0\"><img src=\"/images/patient-version-on.gif\" alt=\"Patient Version\" border=\"0\"><img src=\"/images/tabs-transition-white-gray.gif\" alt=\"\" border=\"0\">");
                    sbContent.Append("<a href=\"");
                    sbContent.Append(strPageUrl + "&version=healthprofessional");
                    sbContent.Append("\">");
                    sbContent.Append("<img src=\"/images/health-professional-off.gif\" alt=\"Health Professional Version\" border=\"0\"></a><img src=\"/images/tabs-end-gray.gif\" alt=\"\" border=\"0\">");
                }
                else
                {

                    sbContent.Append("<img src=\"/images/tabs-beginning.gif\" width=\"1\" height=\"21\" alt=\"\" border=\"0\">");
                    sbContent.Append("<a href=\"");
                    sbContent.Append(strPageUrl + "&version=patient");
                    sbContent.Append("\">");
                    sbContent.Append("<img src=\"/images/patient-version-off.gif\" alt=\"Patient Version\" border=\"0\">");
                    sbContent.Append("</a>");
                    sbContent.Append("<img src=\"/images/tabs-transition-gray-white.gif\" alt=\"\" border=\"0\"><img src=\"/images/health-professional-on.gif\" alt=\"Health Professional Version\" border=\"0\"><img src=\"/images/tabs-end-white.gif\" alt=\"\" border=\"0\">");
                }
            }
            else
            {
                if (PVersion == ProtocolVersions.Patient)
                {
                    sbContent.Append("Patient Version");
                    sbContent.Append("&nbsp;&nbsp;&nbsp;");
                    sbContent.Append("<a href=\"");
                    sbContent.Append(strPageUrl + "&version=healthprofessional");
                    sbContent.Append("\">Health Professional Version</a>");

                }
                else
                {
                    sbContent.Append("<a href=\"");
                    sbContent.Append(strPageUrl + "&version=patient");
                    sbContent.Append("\">Patient Version</a>");
                    sbContent.Append("&nbsp;&nbsp;&nbsp;");

                    sbContent.Append("Health Professional Version");
                }
            }
            sbContent.Append("</td>");

            sbContent.Append("<td valign=\"top\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" border=\"0\"></td>\n");
            sbContent.Append("<td valign=\"top\"><img src=\"/images/spacer.gif\" width=\"55\" height=\"18\" alt=\"\" border=\"0\"></td>\n");
            sbContent.Append("<td valign=\"top\"><img src=\"/images/spacer.gif\" width=\"10\" height=\"30\" alt=\"\" border=\"0\"></td>\n");
            sbContent.Append("<td valign=\"top\" width=\"363\" align=\"right\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"4\" alt=\"\" border=\"0\"><br>\n");
            sbContent.Append(strDates);
            sbContent.Append("</td>\n");
            sbContent.Append("<td valign=\"top\"><img src=\"/images/spacer.gif\" width=\"10\" height=\"1\" alt=\"\" border=\"0\"></td>\n");
            sbContent.Append("</tr>\n");
            sbContent.Append("</table>\n");

            return sbContent.ToString();
        }

        private ProtocolVersions PVersion
        {
            get
            {
                return (ProtocolVersions)Enum.Parse(typeof(ProtocolVersions), Request.Params["version"], true);
            }
        }
    }
}