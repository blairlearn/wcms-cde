using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Common.Logging.Factory;
using Common.Logging.Simple;
using NCI.Util;

namespace NCI.Logging
{
    /// <summary>
    /// Extension of AbstractSimpleLogger - logs all messages of a valid loglevel to the Windows Event Log.
    /// </summary>
    public class EventLogger : AbstractSimpleLogger
    {
        private const string _loggingFormat = "Facility: {0} \n\nError Level: {1} \n\nMessage:\n{2}\n\nException:\n{3}";

        /// <summary>
        /// The name of the Windows log source to which messages will be logged.
        /// </summary>
        public string LogSource { get; private set; }

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
        public EventLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat, string logSource)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        {
            LogSource = logSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            try
            {
                EventLog.WriteEntry(
                    LogSource,
                    String.Format(
                        _loggingFormat,
                        this.Name,
                        level.ToString(),
                        message,
                        exception.ToString()),
                    GetEventLogETFromLogLevel(level));
            }
            catch (Exception newex)
            {
                throw new NCILoggingException("NCI.Logging.Providers.EventLogProvider: Could not write error to Event Log ", newex);
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
