using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// Serves as the base class for the different types of trial listing pages.
    /// </summary>
    public abstract class BaseTrialListingControl : SnippetControl
    {
        /// <summary>
        /// Gets the configuration type of the derrieved class
        /// </summary>
        /// <returns></returns>
        protected abstract Type GetConfigType();

        protected BaseTrialListingConfig Config { get; private set; }

        protected sealed override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Type configType = this.GetConfigType();
            //this.Config = Deserialize JSON in this.Data
        }
    }
}
