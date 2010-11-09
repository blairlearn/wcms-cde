using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NCI.Web.CancerGov.Apps;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using NCI.Util;
using CancerGov.UI.PageObjects;
using NCI.Data;
using CancerGov.DataAccessClasses.UI.Types;
using NCI.Logging;
using CancerGov.CDR.DataManager;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ViewGenericsProfessionals : AppsBaseUserControl
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

            //this.PageHeaders.Add(new TitleBlock("Cancer Genetics Professionals", null, this.PageDisplayInformation));
            //this.pageHtmlHead.Title = "View Cancer Genetics Professionals - National Cancer Institute";

            //if (this.PageDisplayInformation.Version != DisplayVersion.Print)
            //{
            //    //Updated the NavBar to use SelectedTabImg instead of SelectedSectionId for Spanish CancerGov.
            //    //This should be removed at some point. --Vadim 1/12/07 
            //    //this.pageBanner.NavigationBar.SelectedSectionId = new Guid("8FB5745E-98C4-43F7-9D7B-70BC21F617F3");
            //    this.pageBanner.NavigationBar.SelectedTabImg = "/cancertopics";
            //}
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

                        content += "<table border=\"0\" cellpadding=\"1\" cellspacing=\"0\" class=\"gray-border\" width=\"100%\"><tr><td>";
                        content += "<table border=\"0\" cellpadding=\"10\" cellspacing=\"0\" bgcolor=\"#ffffff\" width=\"100%\"><tr><td>";
                        content += geneticPro.GetHtml(Server.MapPath("/Stylesheets"));
                        content += "</td></tr></table>";
                        content += "</td></tr></table>";
                        content += "<p>";
                        content += new ReturnToTopAnchor(this.PageDisplayInformation).Render();
                        content += "<p>";
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

            //if (this.PageDisplayInformation.Version == DisplayVersion.Print)
            //{
            //    string strSendtoPrint = "Send to Printer";
            //    if (this.PageDisplayInformation.Language == DisplayLanguage.Spanish)
            //    {
            //        strSendtoPrint = "Imprima esta página";
            //    }

            //    this.PageHeaders.Add(new Spacer("10", "1"));
            //    this.PageHeaders.Add(new HtmlSegment("<table width=\"751\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td align=\"left\"><table width=\"650\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td><a href=\"javascript:window.print();\"  class=\"navigation-dark-red-link\" >" + strSendtoPrint + "</a></td></tr></table></td></tr></table>"));
            //    this.PageHeaders.Add(new Spacer("10", "1"));
            //}

            //this.PageLeftColumn = new LeftNavColumn(this, Strings.ToGuid(ConfigurationSettings.AppSettings["GeneticsSearchViewID"]));

            /*

             * Setup page options
             */
            //string pagePrintUrl = "/search/view_geneticspro.aspx?personid=" + args + "&print=1";

            ////These are the links that should show in the content version box.
            //KeyValuePair<PageOptionNames, string>[] links = new KeyValuePair<PageOptionNames, string>[] {
            //    new KeyValuePair<PageOptionNames, string>(PageOptionNames.PrintOption, pagePrintUrl),
            //    new KeyValuePair<PageOptionNames, string>(PageOptionNames.BookmarkShareOption, "View Cancer Genetics Professionals")
            //};

            //RandomAppContentVersionBox cvb = new RandomAppContentVersionBox(this.PageDisplayInformation.Language, links);
            //this.PageLeftColumn.Insert(0, cvb);

            ///*
            // * End page options
            // */

            //this.WebAnalyticsPageLoad.SetChannelFromSectionNameAndUrl("Cancerinfo", this.Request.Url.OriginalString.ToString());

            //litOmniturePageLoad.Text = this.WebAnalyticsPageLoad.Tag();

        }
		
    }
}