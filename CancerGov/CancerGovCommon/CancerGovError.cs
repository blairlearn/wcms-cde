using System;
using System.Web;
using System.Configuration;
using System.Diagnostics;
using NCI.Logging;
namespace CancerGov.Common.ErrorHandling
{
    ///<summary>
    ///Defines values for application error type constants<br/>
    ///<br/>
    ///<b>Author</b>:  Greg Andres<br/>
    ///<b>Date</b>:  8-8-2001<br/>
    ///<br/>
    ///<b>Revision History</b>:<br/>
    ///<br/>
    ///</summary>
    public class ErrorType
    {
        //Data type errors
        public static int InvalidGuid = 100;
        public static int InvalidArgument = 101;

        //Database errors
        public static int DbUnavailable = 200;
        public static int DbNoData = 201;

        //Include file errors
        public static int FileNotFound = 300;

        //Endeca errors
        public static int EndecaError = 400;


        //XML errors
        public static int XmlStringParseError = 500;
    }

    ///<summary>
    ///Defines values for application error type description constants<br/>
    ///<br/>
    ///<b>Author</b>:  Greg Andres<br/>
    ///<b>Date</b>:  8-8-2001<br/>
    ///<br/>
    ///<b>Revision History</b>:<br/>
    ///<br/>
    ///</summary>
    public class ErrorTypeDesc
    {
        //Data type errors
        public static string InvalidGuid = "Invalid Guid.";
        public static string InvalidArgument = "Invalid Argument.";

        //Database errors
        public static string DbUnavailable = "No database connection could be established, or syntax errors occurred.";
        public static string DbNoData = "No data was returned.";

        //Include file errors
        public static string FileNotFound = "File not found.";

        //Verity errors
        public static string VerityError = "Error occurred during Verity search.";

        //XML errors
        public static string XmlStringParseError = "String could not be parsed to XML Document.";
    }

    ///<summary>
    ///Defines error-logging overloads<br/>
    ///<br/>
    ///<b>Author</b>:  Greg Andres<br/>
    ///<b>Date</b>:  8-8-2001<br/>
    ///<br/>
    ///<b>Revision History</b>:<br/>
    ///<br/>
    ///</summary>
    public class CancerGovError
    {
        /// <summary>
        /// Get the name of the event log to write to
        /// </summary>
        /// <returns>the name of the event log</returns>
        private static string GetEventLogName()
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["EventLogName"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["EventLogName"];
            }
            catch (Exception e)
            {
            }
            return null;
        }

        /// <summary>
        /// Writes an entry to the event viewer
        /// </summary>
        /// <param name="errString">text for the body of the error</param>
        /// <param name="type">the type of error</param>
        /// <param name="eventID">the id for the error</param>
        private static void LogError(string errString, EventLogEntryType type, int eventID)
        {
            try
            {
                //string source = GetEventLogName();
                //if (!string.IsNullOrEmpty(source) && EventLog.SourceExists(source))
                //{
                //    EventLog.WriteEntry(source, errString, type, eventID);
                //}
                Exception ex = new Exception("Event ID" + eventID + "-Type" + type);                
                LoggingHelper helper = LoggingHelper.Instance;
                helper.LogError("CancerGovSpecific", errString, NCIErrorLevel.Error, ex);

            }
            catch { }
        }


        /// <summary>
        /// Overload method logging error by description with no referrer url
        /// </summary>
        /// <param name="url">Web page generating error</param>
        /// <param name="source">Class generating error</param>
        /// <param name="errEventId">Assigned error type code</param>
        /// <param name="errDescription">Assigned error type description</param>
        public static void LogError(string url, string source, int errEventId, string errDescription)
        {
            LogError(url + "\nSource: " + source + "\n\n" + errDescription + "\n\n", EventLogEntryType.Error, errEventId);
        }

        /// <summary>
        /// Overload method logging error by SQL Server exception with no referrer url
        /// </summary>
        /// <param name="url">Web page generating error</param>
        /// <param name="source">Class generating error</param>
        /// <param name="errEventId">Assigned error type code</param>
        /// <param name="rawException">SQL Server exception object</param>
        public static void LogError(string url, string source, int errEventId, Exception rawException)
        {
            LogError(url + "\nSource: " + source + "\n\n" + rawException.Message + "\n" + rawException.StackTrace, EventLogEntryType.Error, errEventId);
        }

        /// <summary>
        /// Overload method logging error by SQL Server exception with referrer url
        /// </summary>
        /// <param name="url">Web page generating error</param>
        /// <param name="referrer">Web page requesting page that generated error</param>
        /// <param name="source">Class generating error</param>
        /// <param name="errEventId">Assigned error type code</param>
        /// <param name="rawException">SQL Server exception object</param>
        public static void LogError(string url, string referrer, string source, int errEventId, Exception rawException)
        {
            LogError(url + "\nSource: " + source + "\n\nReferrer: " + referrer + "\n\n" + rawException.Message + "\n" + rawException.StackTrace, EventLogEntryType.Error, errEventId);
        }

        /// <summary>
        /// Overload method logging error by description with referrer url
        /// </summary>
        /// <param name="url">Web page generating error</param>
        /// <param name="referrer">Web page requesting page that generated error</param>
        /// <param name="source">Class generating error</param>
        /// <param name="errEventId">Assigned error type code</param>
        /// <param name="errDescription">Assigned error type description</param>
        public static void LogError(string url, string referrer, string source, int errEventId, string errDescription)
        {
            LogError(url + "\nSource: " + source + "\n\nReferrer: " + referrer + "\n\n" + errDescription + "\n", EventLogEntryType.Error, errEventId);
        }

        /// <summary>
        /// Overload method logging non-web error by description
        /// </summary>
        /// <param name="source">Class generating error</param>
        /// <param name="errEventId">Assigned error type code</param>
        /// <param name="errDescription">Assigned error type description</param>
        public static void LogError(string source, int errEventId, string errDescription)
        {
            LogError("Source: " + source + "\n\n" + errDescription, EventLogEntryType.Error, errEventId);
        }

        /// <summary>
        /// Overload method logging non-web error by SQL Server exception
        /// </summary>
        /// <param name="source">Class generating error</param>
        /// <param name="errEventId">Assigned error type code</param>
        /// <param name="rawException">SQL Server exception object</param>
        public static void LogError(string source, int errEventId, Exception rawException)
        {
            LogError("Source: " + source + "\n\n" + rawException.Message + "\n" + rawException.StackTrace, EventLogEntryType.Error, errEventId);
        }
    }
}
