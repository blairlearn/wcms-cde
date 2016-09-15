using System;
using System.Net.Mail;
using Common.Logging;
using Common.Logging.Simple;
using NCI.Util;

namespace NCI.Logging
{
    /// <summary>
    /// Extension of AbstractSimpleLogger - logs all messages of a valid loglevel to the Windows Event Log.
    /// </summary>
    public class EmailLogger : AbstractSimpleLogger
    {
        /// <summary>
        /// The sender email address
        /// </summary>
        public string EmailAddressFrom { get; private set; }

        /// <summary>
        /// The Destination email addresses
        /// </summary>
        public string EmailAddressesTo { get; private set; }

        private const string _loggingFormat = "Facility: {0} \n\nError Level: {1} \n\nMessage:\n{2}\n\nException:\n{3}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="logLevel"></param>
        /// <param name="showLevel"></param>
        /// <param name="showDateTime"></param>
        /// <param name="showLogName"></param>
        /// <param name="dateTimeFormat"></param>
        /// <param name="emailAddressFrom"></param>
        /// <param name="emailAddressesTo"></param>
        public EmailLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName,
            string dateTimeFormat, string emailAddressFrom, string emailAddressesTo)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
            EmailAddressFrom = emailAddressFrom;
            EmailAddressesTo = emailAddressesTo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            string exString = (exception != null) ? exception.ToString() : "";

            SendEmail(
                Name,
                String.Format(
                        _loggingFormat,
                        Name,
                        level.ToString(),
                        message != null ? message : "",
                        exString));
        }

        /// <summary>
        /// Sends out the Email for the Error Message that the client needs to send.
        /// </summary>
        /// <param name="Subject">Subject of the Email.</param>
        /// <param name="Body">Body of the Email.</param>
        private void SendEmail(string subject, string body)
        {
            try
            {
                using (MailMessage message = new MailMessage(EmailAddressFrom, EmailAddressesTo, subject, body))
                {
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw new NCILoggingException("NCI.Logging.EmailLogger: Could not send email.", ex);
            }
        }
    }
}
