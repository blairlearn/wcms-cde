using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// Serves as the base class for the different types of trial listing pages.
    /// </summary>
    public abstract class BaseTrialListingControl : SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(BaseTrialListingControl));

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

            // Read the basic CTS page information JSON
            string sidata = this.SnippetInfo.Data;

            try
            {
                if (string.IsNullOrWhiteSpace(sidata))
                {
                    throw new Exception("TrialListingConfig not present in JSON, associate an Application Module item with this page in Percussion");
                }
                sidata = sidata.Trim();

                // Get our TrialListingPageInfo object this is JSON data that includes template and URL paths and result count parameters.
                // It also includes a nested, JSON-formatted string "RequestFilters", which represents the JSON passed in with the API body request - this is
                // deserialized in TrialListingPageControl.
                // TODO: handle all deserialization in once place, if possible. This will avoid having to go through the process twice
                this.Config = (BaseTrialListingConfig)JsonConvert.DeserializeObject(sidata, configType);

            }
            catch (Exception ex)
            {
                log.Error("Could not load the TrialListingPageInfo; check the config info of the Application Module item in Percussion", ex);
                throw ex;
            }
            //this.Config = Deserialize JSON in this.Data
        }
    }
}
