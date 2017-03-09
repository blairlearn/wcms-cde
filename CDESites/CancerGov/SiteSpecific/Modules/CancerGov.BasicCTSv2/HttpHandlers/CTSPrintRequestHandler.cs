using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;

namespace CancerGov.ClinicalTrials.Basic.v2.HttpHandlers
{
    public class CTSPrintRequestHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {

            var request = context.Request;
            var requestBody = new StreamReader(request.InputStream, request.ContentEncoding).ReadToEnd();
            var jsonSerializer = new JavaScriptSerializer();
            var data = jsonSerializer.Deserialize(requestBody, typeof(TrialCollection));

            //Set our output to be JSON
            context.Response.ContentType = "application/json";

            //Try and get the request.
            Request req = null;
            try
            { 
                req = GetRequestAndValidate(context);
            }
            catch (Exception ex)
            {
                SendErrorResponse(context, ex.Message, 400); //Anything here is just a bad request.
                return;
            }

            //By now our request is good and valid, so let's prep the email.
            StringBuilder body = new StringBuilder();

            Response response = new Response();
            context.Response.Write(JsonConvert.SerializeObject(response));
        }

        private static void QueryDatabase()
        {

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
        /// Parses the request JSON into a request object.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Request GetRequestAndValidate(HttpContext context)
        {
            Request rtnReq = null;

            try
            {
                context.Request.InputStream.Position = 0;
                using (var inputStream = new StreamReader(context.Request.InputStream))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    rtnReq = (Request)serializer.Deserialize(inputStream, typeof(Request));
                }
            }
            catch (Exception)
            {
                throw new Exception("Could not parse request.");
            }

            if (String.IsNullOrWhiteSpace(rtnReq.URL))
            {
                throw new Exception("URL is null or empty.");
            }
            else
            {
                string url = rtnReq.URL.Trim();
                if (url[0] != '/')
                    throw new Exception("URL must be relative to '/'.");
            }

            return rtnReq;
        }

        /// <summary>
        /// Defines the expected Request object of the FeedbackService
        /// </summary>
        public class Request
        {
            /// <summary>
            /// Gets the URL of the page the visitor is giving feedback on
            /// </summary>
            public string URL { get; set; }
            /// <summary>
            /// Gets the list of TrialIDs
            /// </summary>
            public string[] TrialIDs;
        }

        /// <summary>
        /// Defines a response from the FeedbackService
        /// </summary>
        public class Response
        {

        }

        /// <summary>
        /// Defines a response from the FeedbackService
        /// </summary>
        public class ErrorResponse
        {
            public int Status { get; set; }
            public string ErrorMessage { get; set; }
        }

        private class TrialCollection
        {
            public string[] TrialIDs;
        }

    }
}
