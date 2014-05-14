using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE;

namespace CancerGov.Web.UserControls
{
    public partial class TopLevelNavigationMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptTopLevelNavigation.DataSource = GetMenuData();
            rptTopLevelNavigation.DataBind();
        }

        /// <summary>
        /// Highlight the correct section menu item. The menu item is highlighted by 
        /// setting the correct image.
        /// </summary>
        /// <param "objRepeaterItem">Name of the current section for which information is being displayed.</param>
        /// <returns>A correct image which highlight the section menu.</returns>
        protected string isInSection(RepeaterItem objRepeaterItem)
        {
            DataRowView drv = ((DataRowView)(objRepeaterItem.DataItem));
            string currSectionPath = PageAssemblyContext.Current.PageAssemblyInstruction.SectionPath.ToLower();
            if (drv["sectionPath"].ToString().ToLower().CompareTo(currSectionPath) == 0)
                return drv["sectionId"] + "_on.gif";
            return drv["sectionId"] + "_off.gif";
        }

        /// <summary>
        /// A method which returns a datasource which contains information for 
        /// each menu item. 
        /// </summary>
        /// <returns>A bindable datasource object like DataTable.</returns>
        private DataTable GetMenuData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("sectionPath");
            dt.Columns.Add("altText");
            dt.Columns.Add("sectionID");

            DataRow drMenuItem = dt.NewRow();
            drMenuItem[0] = "/"; drMenuItem[1] = "NCI Home"; drMenuItem[2] = "nav_home";
            dt.Rows.Add(drMenuItem);

            drMenuItem = dt.NewRow();
            drMenuItem[0] = "/cancertopics"; drMenuItem[1] = "Cancer Topics"; drMenuItem[2] = "nav_cancer_topics";
            dt.Rows.Add(drMenuItem);

            drMenuItem = dt.NewRow();
            drMenuItem[0] = "/clinicaltrials"; drMenuItem[1] = "Clinical Trials"; drMenuItem[2] = "nav_clinical_trials";
            dt.Rows.Add(drMenuItem);

            drMenuItem = dt.NewRow();
            drMenuItem[0] = "/statistics"; drMenuItem[1] = "Cancer Statistics"; drMenuItem[2] = "nav_cancer_statistics";
            dt.Rows.Add(drMenuItem);

            drMenuItem = dt.NewRow();
            drMenuItem[0] = "/researchandfunding"; drMenuItem[1] = "Research &amp; Funding"; drMenuItem[2] = "nav_research_funding";
            dt.Rows.Add(drMenuItem);

            drMenuItem = dt.NewRow();
            drMenuItem[0] = "/newscenter"; drMenuItem[1] = "News"; drMenuItem[2] = "nav_news";
            dt.Rows.Add(drMenuItem);

            drMenuItem = dt.NewRow();
            drMenuItem[0] = "/aboutnci"; drMenuItem[1] = "About NCI"; drMenuItem[2] = "nav_about";
            dt.Rows.Add(drMenuItem);
            return dt;
        }
    }
}