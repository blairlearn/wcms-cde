using System;
using System.Configuration;
using NCI.Logging;
using NCI.Web.ProductionHost.Configuration;

namespace NCI.Web.ProductionHost
{
    public static class ProductionHostConfig
    {
        private static string _hostname = String.Empty;
        private static string _sitename = String.Empty;

        public static string Hostname
        {
            get { return _hostname; }
        }

        public static string Sitename
        {
            get { return _sitename; }
        }

        static ProductionHostConfig()
        {
            try
            {
                ProductionHostSection section = (ProductionHostSection)ConfigurationManager.GetSection("nci/web/productionHost");
                if (section != null)
                {
                    Logger.LogError("NCI:ProductionHostConfig.cs:ProductionHostConfig",
                        "Found ProductionHostSection.",
                        NCIErrorLevel.Debug);

                    foreach (StringProductionHostElement elem in section.StringConditions)
                    {
                        if (elem.Name == "hostname")
                        {
                            _hostname = elem.Value;
                        }

                        if (elem.Name == "sitename")
                        {
                            _sitename = elem.Value;
                        }
                    }

                    Logger.LogError("NCI:ProductionHostConfig.cs:ProductionHostConfig",
                            "members after searching StringConditions - _hostname = " + _hostname + 
                            ", _sitename = " + _sitename,
                            NCIErrorLevel.Debug);
                }
                else
                {
                    Logger.LogError("NCI:ProductionHostConfig.cs:ProductionHostConfig",
                        "No ProductionHostSection found.",
                        NCIErrorLevel.Debug);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("NCI:ProductionHostConfig.cs:ProductionHostConfig",
                    "Error encountered while loading configuration.",
                    NCIErrorLevel.Error, e);
            }
        }
    }
}
