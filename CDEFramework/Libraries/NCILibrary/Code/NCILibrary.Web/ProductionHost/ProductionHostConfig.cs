using System;
using System.Configuration;
using NCI.Logging;
using NCI.Web.ProductionHost.Configuration;

namespace NCI.Web.ProductionHost
{
    public static class ProductionHostConfig
    {
        private static string _hostname = String.Empty;

        public static bool IsProd
        {
            get { return false; }
        }

        public static string Hostname
        {
            get { return _hostname; }
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
                        Logger.LogError("NCI:ProductionHostConfig.cs:ProductionHostConfig",
                            "Found ProductionHostSection string element " + elem.Name,
                            NCIErrorLevel.Debug);

                        if (elem.Name == "hostname")
                        {
                            _hostname = elem.Value;
                        }
                    }
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
