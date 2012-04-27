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
        protected string strPageUrl = "";
        protected string pvFirstPublished = String.Empty;
        protected string pvLastModified = String.Empty;

        public void Page_Load(object sender, EventArgs e)
        {
            strPageUrl = this.PageInstruction.GetUrl("PrettyUrl").ToString() + "?cdrid=" + Request.QueryString["cdrid"] + (Request.QueryString["protocolsearchid"] == null ? "" : "&protocolsearchid=" + Request.QueryString["protocolsearchid"]);
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            { pvFirstPublished = PageInstruction.GetField("pvFirstPublished"); }
            catch { }

            try
            { pvLastModified = PageInstruction.GetField("pvLastModified"); }
            catch { }

            setUrl();
            base.OnPreRender(e);
        }

        private string setUrl()
        {

            string sbContent = string.Empty;

            if (this.PageDisplayInformation.Version == DisplayVersions.Web)
            {
                if (PVersion == ProtocolVersions.Patient)
                {
                    strPageUrl += "&version=healthprofessional";
                }
                else
                {

                    strPageUrl += "&version=patient";
                }
            }
            else
            {
                if (PVersion == ProtocolVersions.Patient)
                {

                }
                else
                {
                }
            }

            return sbContent.ToString();
        }

        protected ProtocolVersions PVersion
        {
            get
            {
                return (ProtocolVersions)Enum.Parse(typeof(ProtocolVersions), Request.Params["version"], true);
            }
        }

        protected bool PatientVersion
        {
            get
            {
                return PVersion == ProtocolVersions.Patient;
            }
        }

        protected bool HPVersion
        {
            get
            {
                return PVersion == ProtocolVersions.HealthProfessional;
            }
        }
    }
}