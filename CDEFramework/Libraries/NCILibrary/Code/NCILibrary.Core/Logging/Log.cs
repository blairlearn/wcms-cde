using System;

namespace NCI.Logging
{
    /// <summary>
    /// Wrapper for the NCI Logging classes to provide a simple interface,
    /// similar to log4net.
    /// </summary>
    public class Log
    {
        private String classname;

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="classType">The class where the logger will be used.</param>
        public Log(Type classType)
        {
            classname = classType.FullName;
        }

        /// <summary>
        /// Create log entries at a Trace (INFO) recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void trace(String message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Info);
        }

        /// <summary>
        /// Create log entries at a Trace (INFO) recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void trace(String message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Info, ex);
        }

        /// <summary>
        /// Create log entries at a Debug recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void debug(String message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Debug);
        }

        /// <summary>
        /// Create log entries at a Debug recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void debug(String message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Debug, ex);
        }

        /// <summary>
        /// Create log entries at a Warning recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void warning(string message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Warning);
        }

        /// <summary>
        /// Create log entries at a Warning recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void warning(string message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Warning, ex);
        }

        /// <summary>
        /// Create log entries at a Error recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void error(string message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Error);
        }

        /// <summary>
        /// Create log entries at a Warning recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void error(string message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Error, ex);
        }

        /// <summary>
        /// Create log entries at a Fatal (CRITCICAL) recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        public void fatal(string message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Critical);
        }

        /// <summary>
        /// Create log entries at a Fatal (CRITCICAL) recording threshold.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="ex">An exception to be recorded</param>
        public void fatal(string message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Critical, ex);
        }
    }
}
