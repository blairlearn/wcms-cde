using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EdgeJs;
using NCI.Web.CDE.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public class AngularModule : SnippetControl
    {
        private JObject _config = null;

        /// <summary>
        /// Gets or sets the PrettyUrl of the page this component lives on.
        /// </summary>
        private string PrettyUrl { get; set; }

        /// <summary>
        /// Gets or sets the current Url as an NciUrl object.
        /// </summary>
        private NciUrl CurrentUrl { get; set; }

        /// <summary>
        /// Gets or sets the Current AppPath, which is usually something like
        /// https://www.cancer.gov(PURL)(AppPath)
        /// (Where both PURL and AppPath start with /)
        /// </summary>
        private string CurrAppPath 
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

        public void Page_Load(object sender, EventArgs e)
        {
            _config = JObject.Parse(SnippetInfo.Data);

            /*
            //For now we do not care about the base URL.  We will need the path of the server
            //and client files.
            if (
                _config["base"] == null
                || _config["base"].Type != JTokenType.String
                || String.IsNullOrWhiteSpace(_config["base"].Value<string>())
               )
            {
                throw new ConfigurationErrorsException("JSApplicationProxy requires a valid base URL.");
            }
            */

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

            this.CurrentUrl = currURL;

        }


        public override void RenderControl(HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.H3);
            writer.Write("Angular Module");
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "angular-container");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            WriteNode(writer).Wait();


            writer.RenderEndTag();
        }

        public async Task WriteNode(HtmlTextWriter writer)
        {
            //When calling require, the rules to determine what is required are defined at
            //https://nodejs.org/api/modules.html#modules_all_together
            var func = Edge.Func(@"
                var theApp = require('/Development/CDE_Content/CancerGov/Live/PublishedContent/js/apps/myapp/server');
                                
                return function (data, callback) {     
                    // This is a workaround to handle hangs when using promises.
                    setImmediate(() => {});

                    //
                    //callback(null, 'URL is: ' + data.url);

                                                        
                    var result = theApp.default(data);
                    //    .then(html => callback(null,html))
                    //    .catch(callback);

                    result.then(callback);

                    //callback(null, result);

                    //.resolve('resolve').then((content) => {
                    //  callback(null, content);
                    //})
                    //.catch(callback);
                }
            ");            

            //This will write the data returned by the rendering.

            dynamic response = func(
                    new
                    {
                        //This should be the protocol, host and port, as in the same-origin policy meaning of origin.
                        //ignoring port for now.
                        origin = string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host),

                        //Url is req.originalUrl in express engine, which is the path and query.
                        url = this.CurrentUrl.ToString(),

                        //Dunno what this maps to...  Not used by express engine...
                        absoluteUrl = "",

                        //This should be what <base> would point to...  So, the PURL.
                        baseUrl = this.PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString()
                    }
            ).Result;

            string keys = "";
            foreach (string key in ((IDictionary<String, Object>)response).Keys)
            {
                keys += ", " + key;
            }
            writer.Write(keys);

        }


        //Interface passed from Edge.js to this module
        //export interface IParams {
        //    origin: string;
        //    url: string;
        //    absoluteUrl: string;
        //    base: string;
        //    data: {}; // custom user data sent from server (through asp-prerender-data="" attribute on index.cshtml)
        //}

    }
}
