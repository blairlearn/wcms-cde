using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using NCI.Util;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE.UI.WebControls;
using NCI.Web.CDE.Modules;
using NCI.Web.UI.WebControls;
using NCI.Logging;
using NCI.Search.Endeca;
using CancerGov.Modules.Search.Endeca;
using NCI.Web.CDE;
using NCI.Web.CancerGov.Apps;

namespace CancerGov.Web.SnippetTemplates.CancerBulletin
{
    public partial class CancerBulletinSearch : SearchBaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JSManager.AddExternalScript(this.Page, "/scripts/imgevents.js");
        }

        protected string SubmitScript
        {
            get
            {
                string onsubmitString = "return CBSearchSubmit();";
                // Web Analytics *************************************************
                if (WebAnalyticsOptions.IsEnabled)
                    onsubmitString = " return CBSearchSubmit() && NCIAnalytics.BulletinSearch(this, searchType);";
                // End Web Analytics *********************************************
                return onsubmitString;
            }
        }

        protected string GetMonthListItems(string monthType)
        {
            string html = String.Empty;
            string [] monthNameLookup = {"Jan.", "Feb.", "Mar.", "Apr.", "May", "Jun.", "Jul.", "Aug.", "Sept.", "Oct.", "Nov.", "Dec."};
            for (int i = 1; i <= 12; i++)
                html += string.Format("<option {0} value=\"{1}\">{2}</option>", i == DateTime.Now.Month ? "selected" : String.Empty, i, monthNameLookup[i-1]);
            return html;
        }

        protected string GetYearListItems(string yearType)
        {
            int yearsAgo = Int32.Parse(ConfigurationSettings.AppSettings["CBSearchYearsAgo"].ToString());
            string html = String.Empty;
            int startYear = DateTime.Now.Year - yearsAgo;

            while (startYear <= DateTime.Now.Year)
            {
                if ( (string.Compare("startYear", yearType, true) == 0 && startYear ==  DateTime.Now.Year - yearsAgo) ||
                     (string.Compare("endYear", yearType, true) == 0 && startYear == DateTime.Now.Year ))
                    html += string.Format("<option selected value=\"{0}\">{0}</option>", startYear.ToString());
                else
                    html += string.Format("<option value=\"{0}\">{0}</option>", startYear.ToString());
                startYear++;
            }

            return html;
        }

        protected string PostBackUrl
        {
            get 
            {
                return this.SearchPageInfo.SearchResultsPrettyUrl;                              
            }
        }
    }
}