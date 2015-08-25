using System;
using System.Web;

namespace NCI.Services.Dictionary
{
    /// <summary>
    /// Cheesy HTTP handler to make up for WCF not giving us an easy way to remove the quotation marks from
    /// around the string containing the term details.
    /// 
    /// To configure this handler, go to the web.config and in the system.webServer / handlers section, add the new line:
    /// 
    ///     <add name="DictionaryServiceHandler" verb="GET" path="Dictionary.svc" type ="NCI.Services.Dictionary.DictionaryHandler, NCILibrary.Services.Dictionary"/>
    /// 
    /// In the Risk Assessment Tools, in the same section, be sure to add a matching
    /// 
    ///     <remove name="DictionaryServiceHandler" />
    /// 
    /// </summary>
    public class DictionaryHandler : IHttpHandler
    {
        /// <summary>
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // Get the Function by parsing context.Request.PathInfo
            // Get the paramters from context.Request.QueryString
            context.Response.Write("Blah, blah, blah.  Whatever.");
        }

        #endregion
    }
}
