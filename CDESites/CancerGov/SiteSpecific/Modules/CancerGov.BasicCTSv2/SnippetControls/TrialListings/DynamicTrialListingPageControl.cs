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
        /// Set logging for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(DynamicTrialListingPageControl));

        /// <summary>
        /// Creates a DynamicTrialListingConfig for use in implementation methods
        /// </summary>
        protected DynamicTrialListingConfig Config
        {
            get
            {
                return (DynamicTrialListingConfig)this.BaseConfig;
            }
        }

        /// <summary>
        /// Implementation of base trial listing page's GetConfigType()
        /// </summary>
        /// <returns>The type of the current configuration</returns>
        protected override Type GetConfigType()
        {
            return typeof(DynamicTrialListingConfig);
        }

        /// <summary>
        /// Implementation of base trial listing page's Trial Query
        /// </summary>
        /// <returns>A JObject with all of the query parameters for use </returns>
        protected sealed override JObject GetTrialQuery()
        {
            JObject query = new JObject(this.GetTypeSpecificQueryParameters());

            //Add any common parameters between all of the items.

            return query;
        }

        /// <summary>
        /// Implementation of base trial listing page's InternalGetNoTrialsHtml
        /// </summary>
        /// <returns>A string with the NoTrialsHtml from the configuration</returns>
        protected override String InternalGetNoTrialsHtml()
        {
            DynamicTrialListingConfigPattern pattern = this.Config.DynamicListingPatterns[this.GetCurrentPatternKey()];

            if (!string.IsNullOrWhiteSpace(pattern.NoTrialsHtml))
            {
                return this.ReplacePlaceholderText(pattern.NoTrialsHtml);
            }
            else
            {
                return string.Empty;
            }
        }

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
            currURL.SetUrl(currURL.UriStem);

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
            string pattern = this.GetCurrentPatternKey();
            
            //Step 3. Setup Page Metadata
            this.SetupPageMetadata(pattern);


            //if(Session["redirect_to_notrials"] != null)
            //{
            //    Session["redirect_to_notrials"] = null;
            //}

        }

     /// <summary>
     ///    This method is called when no results are returned by the query. In this case the function checks that the current URL does not
     ///    have the word notrials in it. If it is the case, the function will redirect the user to a page with the following URL:
     ///    PRETTYURL/NOTRIALS?p1=a&p2=b
     /// </summary>
        protected override void OnEmptyResults()
        {
            try
            {
                string pageUrl = this.PrettyUrl;

                if(pageUrl.ToLower().Trim().Contains("notrials") == false)
                {
                    int parametersCount = 0;
                    string[] parameters = this.CurrAppPath.Split(new char[] { '/' });
                    string noTrialsPageUrl = pageUrl + "/notrials?";

                    



                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if(parameters[i].Length > 0)
                        {
                            parametersCount = parametersCount + 1;
                            noTrialsPageUrl = noTrialsPageUrl + "p" + (parametersCount) + "=" + parameters[i];

                            if (i < parameters.Length - 1)
                            {
                                noTrialsPageUrl = noTrialsPageUrl + "&";
                            }
                        }
                        

                       
                    }

                    if (noTrialsPageUrl.Length > 0 && noTrialsPageUrl.Contains("p1") == true && CurrAppPath.ToLower().Trim().Contains("/notrials") == false)
                    {
                        Response.Redirect(noTrialsPageUrl);
                    }

                }
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                string stackTrace = ex.StackTrace;
            }
        }

        /// <summary>
        /// Sets up the page metadata based on the given pattern key using overrides
        /// </summary>
        /// <param name="patternKey"></param>
        private void SetupPageMetadata(string patternKey)
        {
            DynamicTrialListingConfigPattern pattern = this.Config.DynamicListingPatterns[patternKey];

            string browserTitle = this.ReplacePlaceholderText(pattern.BrowserTitle);            
            this.PageInstruction.AddFieldFilter("browser_title", (name, data) =>
            {
                data.Value = browserTitle;
            });

            string pageTitle = this.ReplacePlaceholderText(pattern.PageTitle);            
            this.PageInstruction.AddFieldFilter("long_title", (name, data) =>
            {
                data.Value = pageTitle;
            });

            string metaDescription = this.ReplacePlaceholderText(pattern.MetaDescription);
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

        /// <summary>
        /// Gets the IntroText for the dynamic trial listing page
        /// </summary>
        /// <returns>A string with the NoTrialsHtml from the configuration</returns>
        public string GetIntroText()
        {
            DynamicTrialListingConfigPattern pattern = this.Config.DynamicListingPatterns[this.GetCurrentPatternKey()];

            if (!string.IsNullOrWhiteSpace(pattern.IntroText))
            {
                return this.ReplacePlaceholderText(pattern.IntroText);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
 