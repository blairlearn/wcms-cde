using System;
using Common.Logging;

namespace NCI.Logging
{
    /// <summary>
    /// Wrapper for the NCI Logging classes to provide a simple interface,
    /// similar to log4net.
    /// </summary>
    public class Log
    {
        private ILog log;

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="classType">The class where the logger will be used.</param>
        public Log(Type classType)
        {
            log = LogManager.GetLogger(classType);
        }

        /// <summary>
        /// Create log entries at a Trace (INFO) recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void trace(String message)
        {
            log.Trace(message);
        }

        /// <summary>
        /// Create log entries at a Trace (INFO) recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void trace(String message, Exception ex)
        {
            log.Trace(message, ex);
        }

        /// <summary>
        /// Create log entries at a Debug recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void debug(String message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// Create log entries at a Debug recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void debug(String message, Exception ex)
        {
            log.Debug(message, ex);
        }

        /// <summary>
        /// Create log entries at a Warning recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void warning(string message)
        {
            log.Warn(message);
        }

        /// <summary>
        /// Create log entries at a Warning recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void warning(string message, Exception ex)
        {
            log.Warn(message, ex);
        }

        /// <summary>
        /// Create log entries at a Error recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void error(string message)
        {
            log.Error(message);
        }

        /// <summary>
        /// Create log entries at a Warning recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void error(string message, Exception ex)
        {
            log.Error(message, ex);
        }

        /// <summary>
        /// Create log entries at a Fatal (CRITCICAL) recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void fatal(string message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// Create log entries at a Fatal (CRITCICAL) recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void fatal(string message, Exception ex)
        {
            log.Fatal(message, ex);
        }
    }
}
