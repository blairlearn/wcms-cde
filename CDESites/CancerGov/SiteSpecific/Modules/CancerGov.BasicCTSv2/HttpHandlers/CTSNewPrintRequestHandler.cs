using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using CancerGov.ClinicalTrialsAPI;
using NCI.Web.CDE.Modules;
using CancerGov.ClinicalTrials.Basic.v2.DataManagers;
using NCI.Web.CDE.UI.Configuration;

namespace CancerGov.ClinicalTrials.Basic.v2.HttpHandlers
{
    public class CTSNewPrintRequestHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //var inputStream = new StreamReader(context.Request.InputStream);
            //var jsonSerializer = new JsonSerializer();
            //var data = (Request)jsonSerializer.Deserialize(inputStream, typeof(Request));

            //Set our output to be JSON
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = Encoding.UTF8;
            //context.Response.StatusCode = 404;

            //Guid.Parse("foo");

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

            // Retrieve the collections given the ID's
            BasicCTSManager manager = new BasicCTSManager("https://clinicaltrialsapi.cancer.gov");
            List<ClinicalTrial> results = manager.GetMultipleTrials(req.TrialIDs).ToList();

            // Send results to Velocity template
            var formattedResult = FormatResults(results);
             
            // Save result to cache table
            var test = CTSPrintResultsDataManager.SavePrintResult(formattedResult, results.ToString(), Settings.IsLive);

            // Return result from save to cache 
            // should be a URL or a GUID.

            context.Response.Write(JsonConvert.SerializeObject(formattedResult));
        }

        public string FormatResults(IEnumerable<ClinicalTrial> results)
        {
            // Show Results

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                @"~/PublishedContent/VelocityTemplates/BasicCTSPrintResultsv2.vm",
                 new
                 {
                     Results = results
                 }
            ));

            File.WriteAllText(@"C:\Development\misc\output.html", ltl.Text);
                   
            return (ltl.Text);
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

            return rtnReq;
        }

        /// <summary>
        /// Defines the expected Request object of the FeedbackService
        /// </summary>
        public class Request
        {
            /// <summary>
            /// Gets the list of TrialIDs
            /// </summary>
            public List<String> TrialIDs;
        }

        /// <summary>
        /// Defines a response from the FeedbackService
        /// </summary>
        public class Response
        {
            /// <summary>
            /// URL for Stored Results
            /// </summary>
            public String Url;
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
