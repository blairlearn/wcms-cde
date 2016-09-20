using System;
using Common.Logging;

namespace NCI.Web.CDE.SimpleRedirector
{
    // Wrapper for the NCI Logging classes to provide a simple interface,
    // similar to log4net.
    internal class Log
    {
        private ILog log { get; set; }

        public Log(Type classType)
        {
            log = LogManager.GetLogger(classType);
        }

        public void trace(String message)
        {
            log.Trace(message);
        }

        public void trace(String message, Exception ex)
        {
            log.Trace(message, ex);
        }

        public void debug(String message)
        {
            log.Debug(message);
        }

        public void debug(String message, Exception ex)
        {
            log.Debug(message, ex);
        }

        public void warning(string message)
        {
            log.Warn(message);
        }

        public void warning(string message, Exception ex)
        {
            log.Warn(message, ex);
        }

        public void error(string message)
        {
            log.Error(message);
        }

        public void error(string message, Exception ex)
        {
            log.Error(message, ex);
        }

        public void fatal(string message)
        {
            log.Fatal(message);
        }

        public void fatal(string message, Exception ex)
        {
            log.Fatal(message, ex);
        }
    }
}
