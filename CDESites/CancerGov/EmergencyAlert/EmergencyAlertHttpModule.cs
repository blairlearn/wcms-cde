using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CancerGov.EmergencyAlert
{
    public class EmergencyAlertHttpModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext currContext = app.Context;

            //Since we only want to do this for .aspx files to cutdown on DB requests,
            //we need to check the extension.
            string currPath = currContext.Request.Path;
            if (!string.IsNullOrEmpty(currPath))
            {
                string ext = VirtualPathUtility.GetExtension(currPath); //.aspx
                ext = ext.ToLower();
                if (ext != ".aspx")
                    return;
            }

            //First see if there is an emergency and store it in the HttpContext. (So we do not have
            //to keep going back to the db.
            EmergencyContext context = EmergencyContext.CreateContext();

            if (context == null)
            {
                //Log error
            }
            else
            {
                //Secondly if we need to redirect ALL requests to the emergency page then we should 
                //redirect.

                if (context.RedirectAllPages)
                {
                    //check to see if this is the emergency url. Note since the emergency page is a flat
                    //aspx, it will not be a prettyURL and therefore not rewritten.
                    if (currContext.Request.RawUrl.ToLower() != context.EmergencyUrl.ToLower())
                        currContext.Response.Redirect(context.EmergencyUrl, true);
                }
            }
        }

        #endregion
    }
}
