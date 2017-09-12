using System;
using System.Web.UI;
using NCI.Web;
using NCI.Web.CDE.Modules;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public partial class BasicCTSAdvSearchControl : BasicCTSBaseControl
    {
        /// <summary>
        /// Browser was sent to this page by way of a redirection from the results page.
        /// (i.e. A search query with a CDRID instead of a concept ID.)
        /// </summary>
        public bool Redirected { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Check for the presence of the REDIRECTED_FLAG. If not present, then this page load is not
            // the result of a redirection.
            if (Request.QueryString[REDIRECTED_FLAG] == null)
                Redirected = false;
            else
                Redirected = true;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                BasicCTSPageInfo.AdvSearchPageTemplatePath, new
                {
                    Control = this,
                    ResultsPagePrettyUrl = BasicCTSPageInfo.ResultsPagePrettyUrl
                }));
            Controls.Add(ltl);
        }

        /// <summary>
        /// Retrieve the working URL of this control from the page XML.
        /// </summary>
        protected override String WorkingUrl
        {
            get { return BasicCTSPageInfo.AdvSearchPagePrettyUrl; }
        }
    }
}
