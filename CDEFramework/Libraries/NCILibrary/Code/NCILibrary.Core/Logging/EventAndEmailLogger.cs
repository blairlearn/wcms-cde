using System;
using System.Diagnostics;
using System.Net.Mail;
using Common.Logging;
using Common.Logging.Simple;
using NCI.Util;

namespace NCI.Logging
{
    /// <summary>
    /// Extension of AbstractSimpleLogger - logs all messages of a valid loglevel to the Windows Event Log.
    /// </summary>
    public class EventAndEmailLogger : AbstractSimpleLogger
    {
        private const string _loggingFormat = "Facility: {0} \n\nError Level: {1} \n\nMessage:\n{2}\n\nException:\n{3}";

        public LogLevel EventLevel { get; private set; }
        public LogLevel EmailLevel { get; private set; }

        public string LogSource { get; private set; }

        /// <summary>
        /// The sender email address
        /// </summary>
        public string EmailAddressFrom { get; private set; }

        /// <summary>
        /// The Destination email addresses
        /// </summary>
        public string EmailAddressesTo { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logName"></param>
        /// <param name="logLevel"></param>
        /// <param name="showLevel"></param>
        /// <param name="showDateTime"></param>
        /// <param name="showLogName"></param>
        /// <param name="dateTimeFormat"></param>
        /// <param name="logSource"></param>
        public EventAndEmailLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime,
            bool showLogName, string dateTimeFormat, LogLevel eventLevel, LogLevel emailLevel, string logSource,
            string emailAddressFrom, string emailAddressesTo)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
            EventLevel = eventLevel;
            EmailLevel = emailLevel;
            LogSource = logSource;
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
            string fullMessage = String.Format(
                            _loggingFormat,
                            Name,
                            level,
                            message,
                            exception);

            WriteEvent(level, fullMessage);
            WriteEmail(level, fullMessage);
        }

        private void WriteEvent(LogLevel level, string fullMessage)
        {
            if (level >= EventLevel)
            {
                try
                {
                    EventLog.WriteEntry(
                        LogSource,
                        fullMessage,
                        GetEventLogETFromLogLevel(level));
                }
                catch (Exception newex)
                {
                    throw new NCILoggingException("NCI.Logging.EventAndEventLogger: Could not write error to Event Log.", newex);
                }
            }
        }

        private void WriteEmail(LogLevel level, string fullMessage)
        {
            if (level >= EmailLevel)
            {
                SendEmail(Name, fullMessage);
            }
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
                throw new NCILoggingException("NCI.Logging.EventAndEmailLogger: Could not send email.", ex);
            }
        }

        private EventLogEntryType GetEventLogETFromLogLevel(LogLevel level)
        {
            if (level >= LogLevel.Error)
                return EventLogEntryType.Error;
            else if (level > LogLevel.Info)
                return EventLogEntryType.Warning;
            else
                return EventLogEntryType.Information;
        }
    }
}
