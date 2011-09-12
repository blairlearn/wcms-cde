using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using CancerGov.CDR.DataManager;
using CancerGov.DataAccessClasses.UI.Types;
using CancerGov.UI.PageObjects;
using NCI.Data;
using NCI.Logging;
using NCI.Util;
using NCI.Web.CancerGov.Apps;
using NCI.Web.CDE;
using NCI.Web.CDE.WebAnalytics;
namespace CancerGov.Web.SnippetTemplates
{
    public partial class ViewGenericsProfessionals : SearchBaseUserControl
    {
        private string content;

        #region Page properties

        /// <summary>
        /// Gets cancer genetics professional summary
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        #endregion


        /// <summary>
        /// Event method sets content version and template and user control properties<br/>
        /// <br/>
        /// [1] Uses input parameter, personid (comma-delimited intIds or recnum;intId pairs), to <br/>
        ///     identify instance of template<br/>
        /// [2] Uses usp_GetGeneticProfessional to pull professional summary data<br/> 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string args = Strings.Clean(Request.Params["personid"]);

            if (args != null)
            {
                string[] personids = args.Split(',');
                GeneticProfessional geneticPro;

                foreach (string id in personids)
                {
                    if (Strings.Clean(id) != null)
                    {
                        geneticPro = new GeneticProfessional(id);

                        content += "<table border=\"0\" cellpadding=\"1\" cellspacing=\"0\" style=\"border: 1px solid #bdbdbd;\" width=\"100%\"><tr><td>";
                        content += "<table border=\"0\" cellpadding=\"10\" cellspacing=\"0\" bgcolor=\"#ffffff\" width=\"100%\"><tr><td>";
                        content += geneticPro.GetHtml(Server.MapPath("/Stylesheets"));
                        content += "</td></tr></table>";
                        content += "</td></tr></table>";
                        content += "<p>";
                        content += new ReturnToTopAnchor(this.PageDisplayInformation).Render();
                        content += "<p>";
                        content = content.Replace("/search/search_geneticsservices.aspx", SearchPageInfo.SearchPagePrettyUrl);
                    }
                }

                if (Strings.Clean(content) == null)
                {
                    content = "The cancer genetic professional(s) you selected was not found.";
                }
            }
            else
            {
                content = "No genetic professional(s) were selected.";
            }

            string pagePrintUrl = PageAssemblyContext.Current.requestedUrl + "?personid=" + args + "&print=1";
            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("Print", (name, url) =>
            {
                url.SetUrl(pagePrintUrl);
            });
            
            //Web Analytics
            if (WebAnalyticsOptions.IsEnabled)
            {

                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.PageName, wbField =>
                {
                    wbField.Value = ConfigurationSettings.AppSettings["HostName"] + SearchPageInfo.SearchResultsPrettyUrl;
                });
                //End of Web Anlytics
            }

            this.PageInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
            {
                string localUrl = url.ToString();

                if (args != "")
                    localUrl += "?personid=" + args;

                url.SetUrl(localUrl);
            });

        }
		
    }
}