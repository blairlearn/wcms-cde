using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using NCI.Util;


namespace NCI.Web.UI.WebControls
{
    public static class JSManager
    {
        public static void AddResource(Page p, Type type, string resourceName)
        {
            resourceName = Strings.Clean(resourceName);

            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (resourceName == null)
                throw new ArgumentNullException("The resource name passed in is null");

            if (type == null)
                throw new ArgumentNullException("The type passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            if (!HttpContext.Current.Items.Contains("LoadedJS_" + resourceName))
            {
                HtmlGenericControl script = new HtmlGenericControl("script");
                p.Header.Controls.Add(script);

                String srcUrl = p.ClientScript.GetWebResourceUrl(type, resourceName);

                script.Attributes.Add("src", srcUrl);

                script.Attributes.Add("type", "text/javascript");

                //Mark that the stylesheet has been added so no other 
                //context menus will add this...
                HttpContext.Current.Items.Add("LoadedJS_" + resourceName, true);
            }
        }

        public static void AddExternalScript(Page p, string scriptFile)
        {
            scriptFile = Strings.Clean(scriptFile);

            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (scriptFile == null)
                throw new ArgumentNullException("The script file name passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            if (!HttpContext.Current.Items.Contains("LoadedJS_" + scriptFile))
            {
                HtmlGenericControl script = new HtmlGenericControl("script");
                p.Header.Controls.Add(script);

                script.Attributes.Add("src", scriptFile);

                script.Attributes.Add("type", "text/javascript");

                //Mark that the javascript has been added so no other 
                //context menus will add this...
                HttpContext.Current.Items.Add("LoadedJS_" + scriptFile, true);
            }
        }

        public static void AddExternalScript(Page p, string scriptFile, bool async, bool defer)
        {
            scriptFile = Strings.Clean(scriptFile);

            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (scriptFile == null)
                throw new ArgumentNullException("The script file name passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            if (!HttpContext.Current.Items.Contains("LoadedJS_" + scriptFile))
            {
                HtmlGenericControl script = new HtmlGenericControl("script");
                p.Header.Controls.Add(script);

                script.Attributes.Add("src", scriptFile);

                if (async)
                {
                    script.Attributes.Add("async", "async");
                }

                if (defer)
                {
                    script.Attributes.Add("defer", "defer");
                }

                script.Attributes.Add("type", "text/javascript");

                //Mark that the javascript has been added so no other 
                //context menus will add this...
                HttpContext.Current.Items.Add("LoadedJS_" + scriptFile, true);
            }
        }

        public static void AddFooterScript(HtmlContainerControl body, WebControl analyticsControl, string scriptFile, bool async, bool defer)
        {
            scriptFile = Strings.Clean(scriptFile);

            //Check arguments
            if (body == null)
                throw new NullReferenceException("The body html element for the page requires the runat=server property.");

            if (scriptFile == null)
                throw new ArgumentNullException("The script file name passed in is null");

            if (!IsScriptOnPage(scriptFile))
            {
                HtmlGenericControl script = new HtmlGenericControl("script");

                if (analyticsControl != null)
                {
                    // If WebAnalyticsControl is on page, insert End Javascripts right before
                    int index = body.Controls.IndexOf(analyticsControl);
                    body.Controls.AddAt(index, script);
                }
                else
                {
                    // If WebAnalyticsControl is not on page, insert End Javascripts right before closing body tag
                    body.Controls.Add(script);
                }

                // Add attributes from PageTemplateConfigurations file
                script.Attributes.Add("src", scriptFile);
                
                if (async)
                {
                    script.Attributes.Add("async", "async");
                }

                if (defer)
                {
                    script.Attributes.Add("defer", "defer");
                }

                script.Attributes.Add("type", "text/javascript");

                //Mark that the javascript has been added so no other 
                //context menus will add this...
                HttpContext.Current.Items.Add("LoadedJS_" + scriptFile, true);
            }
        }

        public static bool IsScriptOnPage(string scriptFile)
        {
            if(HttpContext.Current.Items.Contains("LoadedJS_" + scriptFile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void AddScript(Page p, string scriptKey, string script)
        {
            AddScript(p, scriptKey, script, false);
        }

        public static void AddScript(Page p, string scriptKey, string script, bool addScriptTags)
        {
            scriptKey = Strings.Clean(scriptKey);
            script = Strings.Clean(script);

            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (scriptKey == null)
                throw new ArgumentNullException("The script key passed in is null");

            if (script == null)
                throw new ArgumentNullException("The script passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            if (!HttpContext.Current.Items.Contains("LoadedJS_" + scriptKey))
            {
                if (addScriptTags)
                {
                    HtmlGenericControl scriptCon = new HtmlGenericControl("script");
                    p.Header.Controls.Add(scriptCon);

                    scriptCon.InnerHtml = script;

                    scriptCon.Attributes.Add("type", "text/javascript");
                }
                else
                {
                    Literal lit = new Literal();
                    p.Header.Controls.Add(lit);

                    lit.Text = script;
                }
                //Mark that the stylesheet has been added so no other 
                //context menus will add this...
                HttpContext.Current.Items.Add("LoadedCss_" + scriptKey, true);
            }
        }

    }
}
