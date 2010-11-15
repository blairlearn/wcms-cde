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
using CancerGov.Common;
using NCI.Web.CDE;
using NCI.Util;
using CancerGov.Common.ErrorHandling;
using NCI.Web.CDE.WebAnalytics;
using CancerGov.CDR.DataManager;
using NCI.Web.CDE.UI;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ResultsGeneticsServices : SearchBaseUserControl
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}
               
        private string resultLabel;
        private string resultCount;
        private string searchSummary;
        private string pager = "";
        private int firstRec;
        private int lastRec;
        private int recordsPerPage = 10;

        #region Page properties

        /// <summary>
        /// Gets HTML phrase description search results
        /// </summary>
        public string ResultLabel
        {
            get { return resultLabel; }
            set { resultLabel = value; }
        }

        /// <summary>
        /// Gets the number of search results
        /// </summary>
        public string ResultCount
        {
            get { return resultCount; }
            set { resultCount = value; }
        }

        /// <summary>
        /// Gets the number of records displayed per page view
        /// </summary>
        public int RecordsPerPage
        {
            get { return recordsPerPage; }
            set { recordsPerPage = value; }
        }

        /// <summary>
        /// Gets the HTML source for the summary of search parameters
        /// </summary>
        public string SearchSummary
        {
            get { return searchSummary; }
            set { searchSummary = value; }
        }

        /// <summary>
        /// Gets the HTML source for the paging construct
        /// </summary>
        public string Pager
        {
            get { return pager; }
            set { pager = value; }
        }

        /// <summary>
        /// Gets the index of the first result displayed on the page view
        /// </summary>
        public int FirstRec
        {
            get { return firstRec; }
            set { firstRec = value; }
        }

        /// <summary>
        /// Gets the index of the last result displayed on the page view
        /// </summary>
        public int LastRec
        {
            get { return lastRec; }
            set { lastRec = value; }
        }

        #endregion

        /// <summary>
        /// Default web form class constructor
        /// </summary>       
        public ResultsGeneticsServices()
        {
           //Page.Unload += new System.EventHandler(Page_Unload);
        }

        /// <summary>
        /// Event method sets content version and templates and user control properties<br/>
        /// <br/>
        /// [1] Input parameters:<br/>
        ///			selCancerType {* name(string);value(integer), comma-delimited : genetic professional cancer type names and identifiers},<br/>
        ///			selCancerFamily {* string, comma-delimited: cancer family search parameters},<br/>
        ///			txtCity {string: city search parameter},<br/>
        ///			selState {* string, comma-delimited: postal abbreviations},<br/>
        ///			selCountry {string: country names},<br/>
        ///			txtLastName {string: last name search parameter},<br/>
        ///			selectedPage {integer: indicates page view to display}<br/>
        ///	[2] Uses usp_GetCancerGeneticProfessionals to get results table<br/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
			//Capture submitted data
			string cancerType = Request.Form["selCancerType"];
			string cancerFamily = Request.Form["selCancerFamily"];
			string city = Request.Form["txtCity"];
			string state = Request.Form["selState"];
			string country = Request.Form["selCountry"];
			string lastName = Request.Form["txtLastName"];

            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web)
            {
                this.textSubmit.Visible = false;
            }
            else
            {
                this.submit.Visible = false;
            }

            GeneticProfessional gp = new GeneticProfessional();
            DataTable dbTable;
            dbTable = gp.GetCancerGeneticProfessionals(cancerType, cancerFamily, city, state, country, lastName);

            resultCount = dbTable.Rows.Count.ToString();

            searchSummary = BuildSearchSummary(Functions.ParseNameValue(cancerType, 0).Replace(",", ", "), Functions.ParseNameValue(cancerFamily, 0).Replace(",", ", "), lastName, city, Functions.ParseNameValue(state, 0).Replace(",", ", "), Functions.ParseNameValue(country, 0), resultCount);

            if (resultCount == "0")
            {
                resultLabel = "Results: 0 - 0 of " + resultCount;
                this.submit.Visible = false;
                this.textSubmit.Visible = false;
                resultGrid.Visible = false;
            }
            else 
            {
                int selectedPage = Strings.ToInt(Request.Form["selectedPage"], 0);
                selectedPage = selectedPage <= 0 ? 1 : selectedPage;

                firstRec = selectedPage;
                lastRec = (selectedPage + (recordsPerPage - 1)) < Convert.ToInt32(resultCount) ? (selectedPage + (recordsPerPage - 1)) : Convert.ToInt32(resultCount);

                CancerGov.Common.ResultPager objPager = new CancerGov.Common.ResultPager();
                objPager.CurrentPage = (selectedPage / recordsPerPage) + ((selectedPage % recordsPerPage) > 0 ? 1 : 0);
                objPager.RecordCount = Convert.ToInt32(resultCount);
                objPager.RecordsPerPage = recordsPerPage;
                objPager.ShowPages = 2;
                pager = objPager.RenderPager();

                DataTable resultPage = dbTable.Clone();
                for (int i = firstRec - 1; i < lastRec; i++)
                {
                    resultPage.ImportRow(dbTable.Rows[i]);
                }

                resultLabel = "Results: " + firstRec.ToString() + " - " + lastRec.ToString() + " of " + resultCount;
                resultGrid.Visible = true;
                resultGrid.DataSource = resultPage;
                resultGrid.DataBind();

                if (resultPage != null)
                {
                    resultPage.Dispose();
                }
            }

            if (dbTable != null)
            {
                dbTable.Dispose();
            }

            if (WebAnalyticsOptions.IsEnabled)
            {

                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.PageName, wbField =>
                {
                    wbField.Value = ConfigurationSettings.AppSettings["HostName"] + SearchPageInfo.DetailedViewSearchResultPagePrettyUrl;
                });

            }
 
        }

        

        public void Page_Unload(object sender, System.EventArgs e)
        {

            if (resultGrid != null)
            {
                resultGrid.Dispose();
            }
        }

        #region BuildSearchSummary method

        /// <summary>
        /// Method builds HTML summary of initial search criteria
        /// </summary>
        /// <param name="cancerType">Types of cancer selected</param>
        /// <param name="cancerFamily">Families of cancer selected</param>
        /// <param name="lastName">Last name search criteria</param>
        /// <param name="city">City search criteria</param>
        /// <param name="stateName">States criteria selected</param>
        /// <param name="countryName">Country criteria selected</param>
        /// <returns>HTML table cells containing a summary of initial search criteria</returns>
        private string BuildSearchSummary(string cancerType, string cancerFamily, string lastName, string city, string stateName, string countryName, string resultCount)
        {
            string result = "";

            result += "<span class=\"page-title\">" + resultCount + " cancer genetics professionals found for:</span><p>\n";
            result += "<table cellspacing=\"0\" cellpadding=\"1\" border=\"0\" class=\"gray-border\" width=\"100%\"><tr><td>\n";
            result += "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" width=\"100%\" bgcolor=\"#ffffff\">\n";
            result += "	<tr>\n";
            result += "		<td valign=\"top\" width=\"50%\">\n";
            result += "			<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n";
            result += "			<tr>\n";
            result += "				<td nowrap align=\"right\" valign=\"top\">Type of cancer:</td>\n";
            result += "				<td>&nbsp;&nbsp;&nbsp;</td>\n";
            result += "				<td width=\"100%\"><b>" + cancerType + "</b></td>\n";
            result += "			</tr>\n";
            result += "			<tr>\n";
            result += "				<td align=\"right\" valign=\"top\" nowrap>Family Cancer Syndrome:</td>\n";
            result += "				<td>&nbsp;</td>\n";
            result += "				<td width=\"100%\"><b>" + cancerFamily + "</b></td>\n";
            result += "			</tr>\n";
            result += "			<tr>\n";
            result += "				<td align=\"right\" nowrap valign=\"top\">Last name:</td>\n";
            result += "				<td>&nbsp;</td>\n";
            result += "				<td width=\"100%\" nowrap><b>" + lastName + "</b></td>\n";
            result += "			</tr>\n";
            result += "			</table>\n";
            result += "		</td>\n";
            result += "		<td valign=\"top\" width=\"50%\">\n";
            result += "			<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\n";
            result += "			<tr>\n";
            result += "				<td align=\"right\" nowrap>City:</td>\n";
            result += "				<td>&nbsp;&nbsp;&nbsp;</td>\n";
            result += "				<td width=\"100%\" nowrap><b>" + city + "</b></td>\n";
            result += "			</tr>\n";
            result += "			<tr>\n";
            result += "				<td align=\"right\" valign=\"top\" nowrap>State:</td>\n";
            result += "				<td></td>\n";
            result += "				<td width=\"100%\"><b>" + stateName + "</b></td>\n";
            result += "			</tr>\n";
            result += "			<tr>\n";
            result += "				<td align=\"right\" valign=\"top\" nowrap>Country:</td>\n";
            result += "				<td></td>\n";
            result += "				<td width=\"100%\"><b>" + countryName + "</b></td>\n";
            result += "			</tr>\n";
            result += "			</table>\n";
            result += "		</td>\n";
            result += "</tr></table>\n";
            result += "</td></tr></table>\n";

            return result;
        }
        
        #endregion
        
    }
}