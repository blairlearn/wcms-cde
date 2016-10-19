using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using Common.Logging;
using Newtonsoft.Json;

namespace NCI.Web.CDE.HttpHandlers
{
    public class FeedbackService : IHttpHandler
    {
        static ILog log = LogManager.GetLogger(typeof(FeedbackService));

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
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

            //assume for now that the URL is /foo/bar and not http://blah.com/foo/bar.
            body.AppendLine("Feedback URL: " + ConfigurationManager.AppSettings["HostName"] + req.URL);
            body.AppendLine("Message:");
            body.AppendLine(req.Message);

            string subject = "CancerGov Feedback Submission";
            string toAddr = ConfigurationManager.AppSettings[req.MailRecipientKey];

            try
            {
                SendEmail(toAddr, subject, body.ToString());
            }
            catch
            {
                SendErrorResponse(context, "Could not send email", 500);
                return;
            }


            Response response = new Response();
            context.Response.Write(JsonConvert.SerializeObject(response));
        }

        private static void SendEmail(string toAddr, string subject, string body)
        {
            //TODO: Get better from address
            // 
            string fromAddr = ConfigurationManager.AppSettings["CTSFeedbackSender"];
            if (String.IsNullOrWhiteSpace(fromAddr))
                throw new Exception("CTSFeedbackSender not set");

            try
            {
                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(fromAddr, toAddr);

                mailMsg.Subject = subject;
                mailMsg.Body = body;
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                log.Error("Could not send mail", ex);
                throw new Exception("Error Sending Email");
            }

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

            if (String.IsNullOrWhiteSpace(rtnReq.Message))
            {
                throw new Exception("Message is null or empty");
            }

            if (String.IsNullOrWhiteSpace(rtnReq.MailRecipientKey))
            {
                throw new Exception("MailRecipientKey is null or empty");
            }
            else
            {
                //Test Key
                if (String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[rtnReq.MailRecipientKey]))
                {
                    throw new Exception("Mail Recipient Key is not configured: " + rtnReq.MailRecipientKey);
                }
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
            /// Gets the feedback message
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// Gets the recipient key to send the message to
            /// </summary>
            public string MailRecipientKey { get; set; }
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

    }
}
