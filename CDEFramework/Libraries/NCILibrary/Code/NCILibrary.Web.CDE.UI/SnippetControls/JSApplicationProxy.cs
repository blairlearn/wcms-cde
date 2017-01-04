using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NCI.Text;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying blobs of HTML that Percussion has generated.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:JSApplicationProxy runat=server></{0}:PublicUseBanner>")]
    public class JSApplicationProxy : SnippetControl
    {
        private JObject _config = null;

        /// <summary>
        /// Gets or sets the PrettyUrl of the page this component lives on.
        /// </summary>
        private string PrettyUrl { get; set; }

        /// <summary>
        /// Gets or sets the Current AppPath, which is usually something like
        /// https://www.cancer.gov(PURL)(AppPath)
        /// (Where both PURL and AppPath start with /)
        /// </summary>
        private string CurrAppPath { get; set; }
        
        public void Page_Load(object sender, EventArgs e)
        {
            _config = JObject.Parse(SnippetInfo.Data);

            if (
                _config["base"] == null 
                || _config["base"].Type != JTokenType.String
                || String.IsNullOrWhiteSpace(_config["base"].Value<string>())
               )
            {
                throw new ConfigurationErrorsException("JSApplicationProxy requires a valid base URL.");
            }

            //We want to use the PURL for this item.
            NciUrl purl = this.PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl);

            if (purl == null || string.IsNullOrWhiteSpace(purl.ToString()))
                throw new Exception("JSApplicationProxy requires current PageAssemblyInstruction to provide its PrettyURL through GetURL.  PrettyURL is null or empty.");

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

            //Set the Current Application Path, e.g. if the URL is /foo/bar/results/chicken,
            //and the pretty URL is /foo/bar, then the CurrAppPath should be /results/chicken
            if (currURL.UriStem.Length == this.PrettyUrl.Length)
                this.CurrAppPath = "/";
            else
                this.CurrAppPath = currURL.UriStem.Substring(PrettyUrl.Length);
        }



        public override void RenderControl(HtmlTextWriter writer)
        {
            string baseUrl = _config["base"].Value<string>().TrimEnd('/');

            using (HttpClient client = new HttpClient())
            {
                //TODO: Handle headers/cookies

                //Determine URL.

                string urlToFetch = baseUrl + this.CurrAppPath;
                //TODO: Add Query Params.

                HttpResponseMessage response = client.GetAsync(urlToFetch).Result;

                if (response.IsSuccessStatusCode)
                {                    
                    //Handle content
                    try
                    {
                        writer.Write(ProcessContent(response.Content.ReadAsStringAsync().Result));
                    }
                    catch (Exception ex)
                    {
                        writer.Write("Error in parsing content");
                    }
                }
                else
                {
                    writer.Write("Error in fetching content");
                }
            }
        }

        private string ProcessContent(string res)
        {            
            Match contentMatch = Regex.Match(res, "<body>(?<appcontent>.*)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (!contentMatch.Success)
                throw new Exception("Body of Content could not be found");

            //This should now just be the body.
            string appcontent = contentMatch.Groups["appcontent"].Value;

            //Replace server relative URLS (e.g /foo)
            appcontent = Regex.Replace(
                appcontent, 
                "(href=\"([^\"]*)\")", 
                m => {
                    //This is the match evaluator.  Using a delegate as this may become more
                    //complicated.  E.g. replacing app_base with purl
                    string url = m.Groups[2].Value;

                    if (url.StartsWith("/"))
                    {
                        return "href=\"" + this.PrettyUrl + url + "\"";
                    }
                    else
                    {
                        return "href=\"" + url + "\"";
                    }
                    
                },                 
                RegexOptions.IgnoreCase
            );

            //Figure out what to do with src???

            return appcontent;
        }

    }
}
