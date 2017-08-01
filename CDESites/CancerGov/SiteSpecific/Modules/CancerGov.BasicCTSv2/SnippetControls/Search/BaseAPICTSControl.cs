using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrialsAPI;
using System.Web.UI;
using System.Dynamic;
using System.Collections.Generic;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Search
{
    /// <summary>
    /// This is the base class for all API-Based Clinical Trial Search controls.
    /// This will also serve as the base for the Search controls as they do not need the API.
    /// <remarks>This replaces BasicCTSBaseControl</remarks>
    /// </summary>
    public abstract class BaseAPICTSControl: SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(BaseAPICTSControl));

        protected BasicCTSPageInfo _config = null;

        /// <summary>
        /// Create a new instance of a APICTS Control
        /// </summary>
        public BaseAPICTSControl()
        {

            //////////////////////////////
            // Load the configuration from the SnippetInfo data
            string spidata = this.SnippetInfo.Data;
            try
            {
                if (string.IsNullOrEmpty(spidata))
                    throw new Exception("BasicCTSPageInfo not present in xml, associate an application module item  with this page in percussion");

                spidata = spidata.Trim();
                if (string.IsNullOrEmpty(spidata))
                    throw new Exception("BasicCTSPageInfo not present in xml, associate an application module item  with this page in percussion");

                _config = ModuleObjectFactory<BasicCTSPageInfo>.GetModuleObject(spidata);
            }
            catch (Exception ex)
            {
                log.Error("could not load the BasicCTSPageInfo, check the config info of the application module in percussion", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the path to the template.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetTemplatePath();

        /// <summary>
        /// Gets the data that should be bound to the Velocity Control
        /// </summary>
        protected abstract object GetDataForTemplate();

        /// <summary>
        /// Overrides the OnPreRender event and 
        /// </summary>
        /// <param name="e"></param>
        protected override sealed void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string templatePath = this.GetTemplatePath();
            object templateData = this.GetDataForTemplate();

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                templatePath, 
                templateData
                )
            );
            Controls.Add(ltl);
        }
    }
}
