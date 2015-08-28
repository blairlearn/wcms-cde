using System;
using System.Web;

using NCI.Logging;
using NCI.Util;

using NCI.Services.Dictionary.BusinessObjects;
using NCI.Services.Dictionary.Handler;
using System.Text;

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
        static Log log = new Log(typeof(DictionaryHandler));
 
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
                HttpRequest request = context.Request;
                HttpResponse response = context.Response;

                try
                {
                    // Get the particular method being invoked.
                    ApiMethodType method = ParseApiMethod(request);

                    // Get object for invoking the specific dictionary method.
                    Invoker invoker = Invoker.Create(method, request);

                    // Get and invoke delegat that calls the particular web method.
                    IJsonizable result = invoker.Invoke();

                    // Put together the response.
                    Jsonizer json = new Jsonizer(result);

                    response.ContentType = "application/json";
                    response.Write(json.ToJsonString());
                }
                catch (HttpParseException ex)
                {
                    response.Status = ex.Message;
                    response.StatusCode = 400;
                }
                catch (Exception ex)
                {
                    log.error(String.Format("Error processing dictionary request. Query: {0}", request.RawUrl), ex);
                    response.StatusDescription = "Error processing dictionary request.";
                    response.StatusCode = 500;
                }
        }

        #endregion

        /// <summary>
        /// Parse the inovked "service" path to determine which method is meant to
        /// be invoked.
        /// </summary>
        /// <param name="request">The current HTTP Request object.</param>
        /// <returns>An ApiMethodType method denoting the invoked web method.</returns>
        /// <remarks>Throws HttpParseException if an invalid path is supplied.</remarks>
        private ApiMethodType ParseApiMethod(HttpRequest request)
        {
            ApiMethodType method = ApiMethodType.Unknown;

            // Get the particular method being invoked by parsing context.Request.PathInfo
            if(string.IsNullOrEmpty(request.PathInfo))
                throw new HttpParseException("Request.Pathinfo is empty.");

            String[] path = Strings.ToListOfTrimmedStrings(request.PathInfo, '/');

            // path[0] -- version
            // path[1] -- Method
            if (path.Length != 2) throw new HttpParseException("Unknown path format.");

            // Only version 1 is presently supported.
            if (!string.Equals(path[0], "v1", StringComparison.CurrentCultureIgnoreCase))
            {
                String msg = String.Format("Unknown version '{0}'.", path[0]);
                log.error(msg);
                throw new HttpParseException(msg);
            }

            // Attempt to retrieve the desired method.
            method = ConvertEnum <ApiMethodType>.Convert(path[1], ApiMethodType.Unknown);
            if (method == ApiMethodType.Unknown)
            {
                String msg = String.Format("Unknown method '{0}'.", path[1]);
                log.error(msg);
                throw new HttpParseException(msg);
            }

            return method;
        }
    }
}
