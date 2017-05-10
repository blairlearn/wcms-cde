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
using Newtonsoft.Json.Linq;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// Serves as the base class for the different types of dynamic trial listing pages.
    /// </summary>
    public abstract class DynamicTrialListingPageControl : BaseTrialListingControl
    {
        /// <summary>
        /// Replaces the place holder text based on this Dynamic Trial Listing control type &
        /// user supplied URL parameters
        /// </summary>
        /// <param name="input">The string to replace</param>
        /// <returns>The string with placeholders replaced</returns>
        protected abstract string ReplacePlaceholderText(string input);

        /// <summary>
        /// Gets the Pattern Key for the Current URL parameters
        /// </summary>
        /// <returns>The keyname</returns>
        protected abstract string GetCurrentPatternKey();

        /// <summary>
        /// Gets the trial query parameters specific to the concrete implementations' type and
        /// user supplied URL parameters.
        /// </summary>
        /// <returns></returns>
        protected abstract JObject GetTypeSpecificQueryParameters();

        /// <summary>
        /// Implementation of base trial listing page's Trial Query
        /// </summary>
        /// <returns></returns>
        protected sealed override JObject GetTrialQuery()
        {
            JObject query = new JObject(this.GetTypeSpecificQueryParameters());

            //Add any common parameters between all of the items.

            return query;
        }

        protected override Type GetConfigType()
        {
            return typeof(DynamicTrialListingConfig);
        }

        /// <summary>
        /// Gets or sets the PrettyUrl of the page this component lives on.
        /// </summary>
        protected string PrettyUrl { get; set; }

        /// <summary>
        /// Gets or sets the current Url as an NciUrl object.
        /// </summary>
        protected NciUrl CurrentUrl { get; set; }

        /// <summary>
        /// Gets or sets the Current AppPath, which is usually something like
        /// https://www.cancer.gov(PURL)(AppPath)
        /// (Where both PURL and AppPath start with /)
        /// </summary>
        protected string CurrAppPath
        {
            get
            {
                //Get the Current Application Path, e.g. if the URL is /foo/bar/results/chicken,
                //and the pretty URL is /foo/bar, then the CurrAppPath should be /results/chicken
                if (this.CurrentUrl.UriStem.Length == this.PrettyUrl.Length)
                    return "/";
                else
                    return this.CurrentUrl.UriStem.Substring(PrettyUrl.Length);
            }
        }


        /**
         * 
         * This section is the pipeline of events in order to render the trials.
         * 
         **/

        /// <summary>
        /// Method called during URL Parsing phase of rendering results.
        /// </summary>
        protected abstract void ParseURL();

        /// <summary>
        /// This sets up the Original Pretty URL, the current full URL and the app path
        /// </summary>
        private void SetupUrls()
        {
            //We want to use the PURL for this item.
            //NOTE: THIS 
            NciUrl purl = this.PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl);

            if (purl == null || string.IsNullOrWhiteSpace(purl.ToString()))
                throw new Exception("DynamicTrialListingPageDiseaseControl requires current PageAssemblyInstruction to provide its PrettyURL through GetURL.  PrettyURL is null or empty.");

            //It is expected that this is pure and only the pretty URL of this page.
            //This means that any elements on the same page as this app should NOT overwrite the
            //PrettyURL URL Filter.
            this.PrettyUrl = purl.ToString();

            //Now, that we have the PrettyURL, let's figure out what the app paths are...
            NciUrl currURL = new NciUrl();
            currURL.SetUrl(HttpContext.Current.Request.RawUrl);

            //Make sure this URL starts with the pretty url
            if (currURL.UriStem.ToLower().IndexOf(this.PrettyUrl.ToLower()) != 0)
                throw new Exception(String.Format("JSApplicationProxy: Cannot Determine App Path for Page, {0}, with PrettyURL {1}.", currURL.UriStem, PrettyUrl));

            this.CurrentUrl = currURL;
        }

        /// <summary>
        /// Implement OnLoad Event to handle fetching of results.
        /// Prevents derrived classes from implementing.
        /// </summary>
        /// <param name="e"></param>
        sealed protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            

            //Step 1. Parse URL and pull out parameters
            this.SetupUrls();
            this.ParseURL();

            //Step 2. Get the Pattern for the placeholder text
            //string pattern = this.GetCurrentPatternKey();
            string pattern = "";
            //Step 3. Setup Page Metadata
            this.SetupPageMetadata(pattern);

            //Get Trials -> BaseTrialListingControl's OnPreRender method

            //Bind Trials and Intro Text/NotFound text to template
                        
        }

        private void SetupPageMetadata(string patternKey)
        {

            string browserTitle = this.ReplacePlaceholderText("I AM THE BROWSER TITLE");            
            this.PageInstruction.AddFieldFilter("browser_title", (name, data) =>
            {
                data.Value = browserTitle;
            });

            string pageTitle = this.ReplacePlaceholderText("I AM THE PAGE TITLE");            
            this.PageInstruction.AddFieldFilter("long_title", (name, data) =>
            {
                data.Value = pageTitle;
            });

            string metaDescription = this.ReplacePlaceholderText("I AM THE META DESCRIPTION");
            this.PageInstruction.AddFieldFilter("meta_description", (name, data) =>
            {
                data.Value = metaDescription;
            });

            //Setup the pretty url of this page, which feeds into the current URL and canonical url
            //TODO: When we build the magic Pretty URL mapping, we need to make sure the URL gets set here
            this.PageInstruction.AddUrlFilter(PageAssemblyInstructionUrls.PrettyUrl, (name, url) =>
            {
                url.SetUrl(this.CurrentUrl.ToString());
            });

            //Setup the addthis URL since it is forced to be the raw PrettyURL field on the page.
            //TODO: account for paging?
            this.PageInstruction.AddUrlFilter("add_this_url", (name, url) =>
            {
                url.SetUrl(this.CurrentUrl.ToString());
            });

        }
    }
}
 