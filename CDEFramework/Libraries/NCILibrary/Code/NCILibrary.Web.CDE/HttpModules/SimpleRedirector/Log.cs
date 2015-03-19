using System;

using NCI.Logging;

namespace NCI.Web.CDE.SimpleRedirector
{
    // Wrapper for the NCI Logging classes to provide a simple interface,
    // similar to log4net.
    internal class Log
    {
        private String classname;

        public Log(Type classType)
        {
            classname = classType.FullName;
        }

        public void trace(String message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Info);
        }

        public void trace(String message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Info, ex);
        }

        public void debug(String message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Debug);
        }

        public void debug(String message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Debug, ex);
        }

        public void warning(string message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Warning);
        }

        public void warning(string message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Warning, ex);
        }

        public void error(string message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Error);
        }

        public void error(string message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Error, ex);
        }

        public void fatal(string message)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Critical);
        }

        public void fatal(string message, Exception ex)
        {
            Logger.LogError(classname, message, NCIErrorLevel.Critical, ex);
        }
    }
}
