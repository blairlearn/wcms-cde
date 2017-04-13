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
using NCI.Web.CDE;
using NCI.Util;
using NCI.Core;

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
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            CTSPrintManager manager = new CTSPrintManager();
            if (request.HttpMethod == "POST")
            {
                // Get the Query parameters from the URL
                var cancerType = request.QueryString["t"];

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

                
                var searchTerms = new CTSSearchParams()
                {
                    CancerType = cancerType,
                    CancerTypePhrase = request.QueryString["ct"],
                    Phrase = request.QueryString["q"],
                    ZipCode = request.QueryString["z"],
                    AgeOfEligibility = request.QueryString["a"],
                    ZipRadius = !String.IsNullOrWhiteSpace(request.QueryString["zp"]) ? Int32.Parse(request.QueryString["zp"]) : 100,
                    Gender = request.QueryString["g"]

                };

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

                    ErrorPageDisplayer.RaisePageByCode(this.GetType().ToString(), 400); //Anything here is just a bad request.
                    return;
                }

                // Store the cached print content
                Guid printCacheID = manager.StorePrintContent(req.TrialIDs, DateTime.Now, searchTerms);

                // Format our return as JSON
                var resp = JsonConvert.SerializeObject(new
                {
                    printID = printCacheID
                });

                response.Write(resp);
                response.End();
            }
           
            if (request.HttpMethod == "GET")
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
                    ErrorPageDisplayer.RaisePageByCode(this.GetType().ToString(), 400);
                    throw new InvalidPrintIDException("Invalid PrintID parameter for CTS Print");
                }


                // If there is no error, send the printID to the manager to retrieve the cached print content
                string printContent = manager.GetPrintContent(printID);
                
                printContent = printContent.Replace("${generatePrintURL}", GetEmailUrl(printID));

                response.Write(printContent);
                response.End();
               
            }
        }

        protected virtual string GetEmailUrl(Guid printID)
        {
            string popUpemailUrl = "";

            string title = "Clinical Trials Results";
            title = System.Web.HttpUtility.UrlEncode(Strings.StripHTMLTags(title.Replace("&#153;", "__tm;")));

            string emailUrl = "/CTS.Print/Display?PrintID=" + printID.ToString();
            string invokedFrom = "&invokedFrom=" + EmailPopupInvokedBy.ClinicalTrialPrintableSearchResults.ToString("d");

            popUpemailUrl = "/common/popUps/PopEmail.aspx?title=" + title + invokedFrom + "&docurl=" + System.Web.HttpUtility.UrlEncode(emailUrl.Replace("&", "__amp;")) + "&language=en";
            popUpemailUrl = popUpemailUrl + HashMaster.SaltedHashURL(HttpUtility.UrlDecode(title) + emailUrl);
            
            return popUpemailUrl;
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

