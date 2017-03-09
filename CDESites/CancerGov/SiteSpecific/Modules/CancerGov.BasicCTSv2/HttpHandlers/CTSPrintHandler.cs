using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.HttpHandlers
{
    public class CTSPrintHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            //Set our output to be HTML
            context.Response.ContentType = "text/HTML";
            context.Response.ContentEncoding = Encoding.UTF8;

            // Validate if the 
            try
            {
                Guid printID = Guid.Parse(request.QueryString["printid"]);
            }
            catch (Exception ex)
            {
                SendErrorResponse(context, ex.Message, 404);
            }

            context.Response.Write("<html>Successfully check GUID</html>");
        }

        /// <summary>
        /// Send an error response back to the client
        /// </summary>
        /// <param name="context">the request context</param>
        /// <param name="message">the error message</param>
        /// <param name="status">the status</param>
        private static void SendErrorResponse(HttpContext context, string message, int status)
        {
            ErrorResponse res = new ErrorResponse() { ErrorMessage = message, Status = status };

            context.Response.Write(JsonConvert.SerializeObject(res));
            context.Response.StatusDescription = message;
            context.Response.StatusCode = status;
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.End();
        }

        /// <summary>
        /// Defines a response from the FeedbackService
        /// </summary>
        public class ErrorResponse
        {
            public int Status { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}

