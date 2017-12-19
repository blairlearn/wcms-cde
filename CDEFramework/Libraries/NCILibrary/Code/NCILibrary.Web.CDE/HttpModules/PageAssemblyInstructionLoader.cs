using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using Common.Logging;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE
{
    public class PageAssemblyInstructionLoader : IHttpModule
    {
        static ILog log = LogManager.GetLogger(typeof(PageAssemblyInstructionLoader));

        private const string PRINT_URL_ENDING = "/print";
        private const string VIEWALL_URL_ENDING = "/allpages";
        private static readonly object REQUEST_URL_KEY = new object();

        //HACK: This is the file regex to handle cache-busting js & css filenames
        private static Regex UniqueStaticFileCleaner = new Regex("\\.__v[0-9a-z]+\\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }
        /// <summary>
        /// Instantiated  events OnBeginRequest and OnLogRequest
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
            // Handle LogRequest event and provide custom logging implementation
            context.LogRequest += new EventHandler(OnLogRequest);
        }
        /// <summary>
        /// OnBeginRequest process the requested URL and rewrite the URL to load the correct template.
        /// </summary>        
        void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture));



            #region CSS/JS File Stamp Removal (Will Rewrite and Return if Conditions Met)
            //HACK: We need to generate unique filepaths for JS & CSS based on timestamp. So we will rewrite 
            //any URL that matches yyyy.v12353432.js or xxxx.v1233454.css to yyyy.js and xxxx.css respectively.
            //
            //This should move to another module when we have more time.
            if (url.ToLower().IndexOf(".css") != -1
                || url.IndexOf(".eot") != -1
                || url.IndexOf(".gif") != -1
                || url.IndexOf(".jpg") != -1
                || url.IndexOf(".js") != -1
                || url.IndexOf(".png") != -1
                || url.IndexOf(".svg") != -1
                || url.IndexOf(".ttf") != -1
                || url.IndexOf(".woff") != -1
                || url.IndexOf(".woff2") != -1)
            {
                //Only go through with the change if this path matches our string.  We experienced an issue in production
                //where rewriting the URL to the same path can mangle the response.  This also seems to be an "known" or
                //at least often complained about issue, where calling RewritePath on static assets confuses the static
                //compression stuff built into IIS.  So this should minimize our possible issues to only versioned URLs. 
                if (UniqueStaticFileCleaner.IsMatch(url))
                {

                    //This replaces "\.v[0-9]+\." with .  -- I don't like the "." portion below, but I want the
                    //regex to be static and compiled.
                    url = UniqueStaticFileCleaner.Replace(url, ".");

                    // Append original parameters in the request URL
                    if (!String.IsNullOrEmpty(context.Request.Url.Query))
                    {
                        //The query should contain a ?
                        url += context.Request.Url.Query;
                    }


                    //rewrite the URL.
                    try
                    {
                        //The general consensus is that rewritepath does not work with static assets well, so use TransferRequest as it
                        //will go back into the IIS pipeline as if a normal file.
                        //NOTE: definately do not call this on files that exist or a loop will occur.
                        context.Server.TransferRequest(url, false);
                        return; //Done rewriting, let's get out of here.
                    }

                    catch (HttpException ex)
                    {
                        string errMessage = "RewriteUrl(): Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nFailed to rewrite URL.";
                        log.Error(errMessage, ex);
                        RaiseErrorPage(errMessage, ex);
                    }
                }

            }
            #endregion

            if (url.ToLower().IndexOf(".ico") != -1
                || url.IndexOf(".css") != -1
                || url.IndexOf(".gif") != -1
                || url.IndexOf(".jpg") != -1
                || url.IndexOf(".js") != -1
                || url.IndexOf(".axd") != -1)
                return;

            //Check if the url has been rewritten yet.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return;

            //if (url == "/")
            //{
            //    //This may fix a bug with .net, and just use the virdir instead.
            //    //The issue is that instead of the clientdir being /, the clientfile
            //    //is / and the clientdir is null.  Internal things then start throwing
            //    //exceptions.
            //    return;
            //}

            RewriteUrl(context, url);
        }
        /// <summary>
        /// Identifies the display type print or web.loads the appropriate xml file and loads the IPageAssemblyInstruction
        /// Loads the page template info and provides the complete intructions/assemblyinfo to assemble a page for the requested URL.
        /// </summary>
        /// <param name="context">httpcontext</param>
        /// <param name="url">Requested URL</param>
        void RewriteUrl(HttpContext context, string url)
        {
            DisplayVersions displayVersion = DisplayVersions.Web;

            if (url.EndsWith("/")) //The key will never be just / because of the statement above
            {
                //strip the trailing /
                url = url.Substring(0, url.LastIndexOf('/'));
                if (string.IsNullOrEmpty(url))
                {
                    url = ContentDeliveryEngineConfig.DefaultHomePage.Homepage;
                }
            }

            //Store the url so it can be rewritten for logging.
            context.Items[REQUEST_URL_KEY] = url;

            bool isPrint = false;

            if (url.EndsWith(PRINT_URL_ENDING))
            {
                //Do not take a substring if someone is trying to print the homepage 
                //of the site
                if (url != PRINT_URL_ENDING)
                {
                    //Since the pretty url map knows nothing about urls
                    //that end with print we need to remove /print from
                    //the key
                    url = url.Substring(0, url.Length - PRINT_URL_ENDING.Length);
                }
                else
                {
                    //We know the key for the homepage
                    //This code does not seem to actually work
                    //At the very least it should rewrite the url to /?print=1 or whatever
                    url = "/";
                }

                //Set the URL to Default home page if the requested url is "/".
                if (url == "/")
                    url = ContentDeliveryEngineConfig.DefaultHomePage.Homepage;

                isPrint = true;
                displayVersion = DisplayVersions.Print;
            }

            //Now check to see if it is the view all. (For MultiPageAssemblyInstructions)
            if (url.EndsWith(VIEWALL_URL_ENDING))
            {
                //Do not take a substring if someone is trying to print the homepage 
                //of the site
                if (url != VIEWALL_URL_ENDING)
                {
                    //Since the pretty url map knows nothing about urls
                    //that end with print we need to remove /print from
                    //the key
                    url = url.Substring(0, url.Length - VIEWALL_URL_ENDING.Length);
                }
                else
                {
                    //We know the key for the homepage
                    //This code does not seem to actually work
                    //At the very least it should rewrite the url to /?print=1 or whatever
                    url = "/";
                }

                if (isPrint)
                    displayVersion = DisplayVersions.PrintAll;
                else
                    displayVersion = DisplayVersions.ViewAll;
            }

            //Handle for Dictionary Print pages AND foia summaries pages.
            if (context.Request.Url.Query.Contains("print") && context.Request.Url.Query.Contains("allpages"))
            {
                displayVersion = DisplayVersions.PrintAll;
            }
            else if (context.Request.Url.Query.Contains("print"))
            {
                displayVersion = DisplayVersions.Print;
            }


            // Set Display version before loading the assembly instructions so it can be accessed in the constructor
            PageAssemblyContext.CurrentDisplayVersion = displayVersion;

            //Now lookup the url..

            IPageAssemblyInstruction assemblyInfo = null;

            try
            {
                //Try to get the assemblyinfo following these rules.
                // When depth == 1, and we get an assemblyInfo, then we are looking for 
                //     the full URL that was requested
                // When depth == 2, and we get an assemblyInfo, then we are looking for
                //     either a MPAI, or a SPAI that implements PushState
                // When depth >= 3, and we get an assemblyInfo, then we are looking for
                //     only a SPAI that implements PushState

                string currUrlPath = url;
                int depth = 1;

                while (assemblyInfo == null && currUrlPath != string.Empty)
                {
                    //Load the assembly info from a path/file.
                    assemblyInfo = PageAssemblyInstructionFactory.GetPageAssemblyInfo(currUrlPath);

                    //Did not find anything, let's continue.
                    if (assemblyInfo == null)
                    {
                        //Chop off the current path and try again.
                        currUrlPath = currUrlPath.Substring(0, currUrlPath.LastIndexOf('/'));

                        // Increment the depth one more time as we continue diving.
                        depth++;

                        continue;
                    }

                    // We have an MPAI & depth is greater than 2 (e.g. MPAI is /foo and URL is /foo/bar/bazz)
                    if (assemblyInfo is IMultiPageAssemblyInstruction && depth != 2)
                    {
                        assemblyInfo = null;
                        break;
                    }

                    // We have a Single page that is 2 or more levels deep, and does not implement pushstate
                    // e.g. SPAI is /foo and URL is /foo/bar.
                    if (
                        assemblyInfo is SinglePageAssemblyInstruction
                        && depth > 1
                        && !assemblyInfo.ImplementsPushState
                       )
                    {
                        assemblyInfo = null;
                        break;
                    }

                    // Good so far, so let's check if it is a MPAI and it has the requested page.
                    if (assemblyInfo is MultiPageAssemblyInstruction)
                    {
                        int index = ((IMultiPageAssemblyInstruction)assemblyInfo).GetPageIndexOfUrl(url);
                        if (index >= 0)
                        {
                            //This url is a page, so set the current index so we can get the page template later.
                            ((IMultiPageAssemblyInstruction)assemblyInfo).SetCurrentPageIndex(index);
                            assemblyInfo.Initialize();
                        }
                        else
                        {
                            assemblyInfo = null;
                            break;
                        }
                    }

                    //By now we have sussed out if we have a PAI or not.
                    //If we do, then we need to initialize it.
                    if (assemblyInfo != null)
                    {
                        assemblyInfo.Initialize();
                    }

                    //Break out of loop.
                    break;
                }
            }
            catch (Exception ex)
            {
                string errMessage = "RewriteUrl(): Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nFailed to Create IPageAssemblyInstruction with the XML file Provided.";
                log.Error(errMessage, ex);
                RaiseErrorPage(errMessage, ex);
                return;
            }

            //Exiting because there is no page assembly instruction for the requested URL.
            if (assemblyInfo == null)
            {
                return;
            }

            //Load the page template info for the current request
            PageTemplateInfo pageTemplateInfo = null;
            try
            {
                pageTemplateInfo = PageTemplateResolver.GetPageTemplateInfo(assemblyInfo.TemplateTheme, assemblyInfo.PageTemplateName, displayVersion);
            }
            catch (Exception ex)
            {
                string errMessage = "RewriteUrl(): Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nCannot Load the pageTemplateInfo problem with the PageTemplateConfiguration XML file ";
                log.Error(errMessage, ex);
                RaiseErrorPage(errMessage, ex);
                return;
            }

            if (pageTemplateInfo == null)
            {
                string errMessage = "RewriteUrl(): Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nPage Template Not Found";
                log.Error(errMessage);
                RaiseErrorPage(errMessage, null);
                return;
            }

            //set the page assembly context with the assemblyInfo, dispayVersion and pageTemplateInfo
            PageAssemblyContext.Current.InitializePageAssemblyInfo(assemblyInfo, displayVersion, pageTemplateInfo, url);

            // Set culture for selected content.
            // The language and culture are formatted as xx-yy (eg. "en-us") when a locale is chosen in Percussion. 			
            // The hyphenated four-letter code is then trimmed to a 2-character neutral culture (eg. "en") by the Velocity
            // user macros file and is added to the XML file to be published in /PublishedContent/PageInstructions. The 
            // assemblyInfo object uses the neutral culture (found in the <Language> tag in the XML file) for page assembly.
            // The only exception to this is the Chinese language, in which the culture MUST be specified as either
            // "zh-hans" (Simplified Chinese) or "zh-hant" (Traditional Chinese). There is no support for a 2-character "zh" 
            // culture - see http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo(v=vs.90).aspx for
            // details. The logic below is a workaround to catch the "zh" and convert it to the full "zh-hans" culture.
            if (!string.IsNullOrEmpty(assemblyInfo.Language))
                if (assemblyInfo.Language == "zh")
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-hans");
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(assemblyInfo.Language);
                }
            string rewriteUrl = PageAssemblyContext.Current.PageTemplateInfo.PageTemplatePath;

            // Append original parameters in the request URL
            if (!String.IsNullOrEmpty(context.Request.Url.Query))
            {
                //The query should contain a ?
                rewriteUrl += context.Request.Url.Query;
            }

            //If we are showing the print version then append the print query
            //variable
            if (isPrint)
            {
                if (rewriteUrl.Contains("?"))
                    rewriteUrl += "&print=1";
                else
                    rewriteUrl += "?print=1";
            }

            // Do not reset the client path because it'll break form action url's.
            try
            {
                context.RewritePath(rewriteUrl, false);
            }

            catch (HttpException ex)
            {
                string errMessage = "RewriteUrl(): Requested URL: " + context.Items[REQUEST_URL_KEY] + "\nFailed to rewrite URL.";
                log.Error(errMessage, ex);
                RaiseErrorPage(errMessage, ex);
            }

        }

        private void RaiseErrorPage(string errMessage, Exception ex)
        {
            HttpContext.Current.Response.Write("There was an error processing the Request");
            if (DisplayErrorOnScreen)
            {
                HttpContext.Current.Response.Write("<br/><br/>Error Message: " + errMessage);
                if (ex != null)
                    HttpContext.Current.Response.Write("<br/><br/>Exception Message: " + ex.ToString());
            }
            HttpContext.Current.Response.End();
            return;
        }

        #endregion
        void OnLogRequest(object sender, EventArgs e)
        {
            //New comments for prototype.  So if there are multiple PrettyUrl modules, then this would
            //not be good to put here -- it should be in its own module.


            //IIS 7's logging is slightly different from IIS 6.  IIS 6 would log the prettyurl because
            //logging is not part of the asp.net pipeline and therefore the url of the GET header is what 
            //is logged.  
            //IIS 7's integrated mode logs the Request object's url.  Maybe the AbsoluteUrl, who knows,
            //but, since we do a RewritePath, the Request object's url is now the ugly url and that is
            //what is logged.  So what we can do at this stage is do a RewritePath back to the
            //orgininal requested url if the requested url was a pretty url.  Then the pretty url will
            //get logged and everyone is happy.  This should not affect anything since the page handler
            //has already run and rendered the content to the Response object.
            //SCR 30301.

            HttpContext context = ((HttpApplication)sender).Context;

            string url = string.Empty;

            if (context.Items.Contains(REQUEST_URL_KEY))
            {
                url = (string)context.Items[REQUEST_URL_KEY];

                if (context.Items.Contains(REQUEST_URL_KEY) && !string.IsNullOrEmpty((string)context.Items[REQUEST_URL_KEY]))
                    url += (string)context.Items[REQUEST_URL_KEY];
                else
                    url += "?";
            }

            if (!string.IsNullOrEmpty(url))
            {
                //context.RewritePath(url, false);
            }
        }

        #region Private Members
        private bool DisplayErrorOnScreen
        {
            get
            {
                string displayErrorOnScreen =
                    System.Configuration.ConfigurationManager.AppSettings["DisplayErrorOnScreen"];

                if (string.IsNullOrEmpty(displayErrorOnScreen))
                    return false;

                bool flag = false;
                if (bool.TryParse(displayErrorOnScreen, out flag))
                    return flag;
                else
                    return false;
            }
        }

        #endregion
    }
}
