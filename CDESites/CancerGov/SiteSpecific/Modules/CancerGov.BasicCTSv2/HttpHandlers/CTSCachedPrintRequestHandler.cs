using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;
using System.Collections.Specialized;
using NCI.Web.CDE.Modules;
using CancerGov.ClinicalTrials.Basic.v2.DataManagers;
using System.Threading.Tasks;
using NCI.Web.CDE.Application;
using NCI.Web.CDE.UI.Configuration;
using CancerGov.ClinicalTrialsAPI;
using CancerGov.ClinicalTrials.Basic.v2.SnippetControls;

namespace CancerGov.ClinicalTrials.Basic.v2.HttpHandlers
{
    public class CTSCachedPrintRequestHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // Get the Query parameters from the URL
            NameValueCollection queryParams = HttpUtility.ParseQueryString(context.Request.Url.Query);
            var cancerType = queryParams.Get("t");
    
            if (cancerType != null)
            {
                String[] ctarr = (cancerType.Contains("|") ? cancerType.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { cancerType });

                //split up the disease ids
                string[] diseaseIDs = ctarr[0].Split(',');

                // Determine cancer type display name from CIDs and key (if there is no match with the key,
                // then first term with matching ids is used)
                string termKey = ctarr.Length > 1 ? ctarr[1] : null;
                var _basicCTSManager = new BasicCTSManager("https://clinicaltrialsapi.cancer.gov");

                cancerType = _basicCTSManager.GetCancerTypeDisplayName(diseaseIDs, termKey);
            }
            

            var searchTerms = new SearchTerms()
            {
                CancerType = cancerType,
                ZipCode = queryParams.Get("z"),
                Age = queryParams.Get("a"),
            };
            


            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            CTSPrintManager manager = new CTSPrintManager();
            bool isError = false;

            if (request.RequestType.ToLower() == "get")
            {
                //Set our output to be HTML
                response.ContentType = "text/HTML";
                response.ContentEncoding = Encoding.UTF8;

                Guid printID = new Guid();

                // Validate if the printID passed in through the URL is a valid Guid
                try
                {
                    printID = Guid.Parse(request.QueryString["printid"]);
                }
                catch
                {
                    // Incorrect parameter for printid (not guid)
                    isError = true;
                    ErrorPageDisplayer.RaisePageByCode(this.GetType().ToString(), 400);
                    throw new InvalidPrintIDException("Invalid PrintID parameter for CTS Print");
                }

                if (!isError)
                {
                    // If there is no error, send the printID to the manager to retrieve the cached print content
                    string printContent = manager.GetPrintContent(printID);
                    response.Write(printContent);
                    response.End();
                }
            }

            else if (request.RequestType.ToLower() == "post")
            {
                //Set our output to be JSON
                response.ContentType = "application/json";
                response.ContentEncoding = Encoding.UTF8;

                //Try and get the request.
                Request req = null;
                try
                {
                    req = GetRequestAndValidate(context);
                }
                catch (Exception ex)
                {
                    isError = true;
                    ErrorPageDisplayer.RaisePageByCode(this.GetType().ToString(), 400); //Anything here is just a bad request.
                    return;
                }

                //BasicCTSManager APImanager = new BasicCTSManager("https://clinicaltrialsapi.cancer.gov");
                //var formattedResult = manager.FormatPrintResults(APImanager.GetMultipleTrials(req.TrialIDs).ToList(), DateTime.Now, searchTerms);
                // Return result from save to cache 
                // should be a URL or a GUID.
                //Guid printCacheID = manager.StorePrintContent(formattedResult, searchTerms);

                if (!isError)
                {
                    // If there is no error, send our trialIDs, timestamp, and search terms to the manager
                    // to store the cached print content
                    Guid printCacheID = manager.StorePrintContent(req.TrialIDs, DateTime.Now, searchTerms);

                    // Format our return as JSON
                    var resp = JsonConvert.SerializeObject(new
                    {
                        printID = printCacheID
                    });

                    response.Write(resp);
                    response.End();
                }
            }
        }

        /*private string FormatResults(IEnumerable<ClinicalTrial> results, DateTime searchDate, SearchTerms searchTerms)
        {
            // convert description to pretty description
            foreach (var trial in results)
            {
                var desc = trial.DetailedDescription;
                trial.DetailedDescription = new TrialVelocityTools().GetPrettyDescription(trial);
            }
            // Show Results
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                @"~/PublishedContent/VelocityTemplates/BasicCTSPrintResultsv2.vm",
                 new
                 {
                     Results = results,
                     SearchDate = searchDate.ToString("d/MM/yyyy"),
                     SearchTerms = searchTerms,
                     TrialTools = new TrialVelocityTools()
                 }
            ));

            File.WriteAllText(@"C:\Development\misc\output.html", ltl.Text);

            return (ltl.Text);
        }*/

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
        /// Send an error response back to the client
        /// </summary>
        /// <param name="context">the request context</param>
        /// <param name="status">the status</param>
        private static void SendErrorResponse(HttpContext context, int status)
        {
            ErrorResponse res = new ErrorResponse() { Status = status };

            //context.Response.Write(JsonConvert.SerializeObject(res));
            //context.Response.StatusDescription = message;
            context.Response.StatusCode = status;
            //context.Response.TrySkipIisCustomErrors = true;
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
        /*public class Response
        {
            /// <summary>
            /// URL for Stored Results
            /// </summary>
            public String Url;
        }*/
    }
}

