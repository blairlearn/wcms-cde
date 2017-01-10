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

using NCI.Web.CDE.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.NodeServices;


namespace NCI.Web.CDE.UI.SnippetControls
{
    public class AspnetCoreAngularModule : SnippetControl
    {
        /// <summary>
        /// Class definition for this modules JSON config -- prevents needing to use dynamic
        /// for nice clean things like _config.Server.Module
        /// </summary>
        private class Config
        {
            public class ServerConfig
            {
                public string Module { get; set; }
            }

            public class ClientConfig
            {
                public string[] Scripts { get; set; }
            }

            public ServerConfig Server { get; set; }
            public ClientConfig Client { get; set; }
        }


        private Config _config = null;

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


        private static readonly IServiceProvider _serviceProvider;
        private static readonly object SERVICE_PROV_LOCK = new object();

        static AspnetCoreAngularModule()
        {
            if (_serviceProvider == null)
            {
                lock (SERVICE_PROV_LOCK)
                {
                    if (_serviceProvider == null)
                    {
                        var services = new ServiceCollection();
                        services.AddNodeServices(options =>
                        {
                            options.ProjectPath = "/Development/CDE_Content/CancerGov/Live/PublishedContent/js";
                        });
                        _serviceProvider = services.BuildServiceProvider();
                    }
                }
            }


        }


        public void Page_Load(object sender, EventArgs e)
        {
            //_config = JObject.Parse(SnippetInfo.Data);
            _config = JsonConvert.DeserializeObject<Config>(SnippetInfo.Data);

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
            writer.Write("ASPNet Core Angular Module");
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "angular-container");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            WriteNode(writer).Wait();


            writer.RenderEndTag();
        }

        public async Task WriteNode(HtmlTextWriter writer)
        {            
            var nodeServices = _serviceProvider.GetRequiredService<INodeServices>();

            string nodeModulePath = "/Development/CDE_Content/CancerGov/Live/PublishedContent/js/apps/testnode/helloname";

                //Server.MapPath(_config.Server.Module);
            //nodeModulePath = System.IO.Path.GetPathRoot(nodeModulePath);


            try 
            {
                writer.Write(await nodeServices.InvokeAsync<string>(nodeModulePath, "this param"));
            }
            catch (Exception ex)
            {
                writer.Write(ex.ToString());
            }
            
            



            /*
            //in essence, we are mimicking https://github.com/aspnet/JavaScriptServices/blob/dev/src/Microsoft.AspNetCore.SpaServices/Prerendering/PrerenderTagHelper.cs
            //except we are not using MVC and can't really use the tag helpers.
            
            //TODO: The MS code talks about cleaning up the requested URL. (Line 96)
            //this is done some weird ViewContext.HttpContext.Features.Get<IHttpRequestFeature>
            //uuencocedPathAndQuery = requestFeature.RawTarget.

            var result = await Prerendering.Prerenderer.RenderToString(
                "",
                nodeServices,
                new Prerendering.JavaScriptModuleExport("")
                {
                    ExportName = "" //This is our module file.
                },
                uuencodedAbsoluteUrl,
                uuencodedPathAndQuery,
                customDataParameter,
                timeoutMillisecondsParameter,
                requestPathBase
            );

            if (!string.IsNullOrEmpty(result.RedirectUrl))
            {
                //This is a redirection.
                HttpContext.Current.Response.RedirectPermanent(result.RedirectUrl, true);
            }

            writer.Write(result.Html);
              
            // Also attach any specified globals to the 'window' object. This is useful for transferring
            // general state between server and client.
            if (result.Globals != null)
            {
                var stringBuilder = new StringBuilder();
                foreach (var property in result.Globals.Properties())
                {
                    stringBuilder.AppendFormat("window.{0} = {1};",
                        property.Name,
                        property.Value.ToString(Formatting.None));
                }
                if (stringBuilder.Length > 0)
                {
                    output.PostElement.SetHtmlContent($"<script>{stringBuilder}</script>");
                }
            }
              
            //TODO: Need other metadata, like title, description, etc. 
             
             */
            
            
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
