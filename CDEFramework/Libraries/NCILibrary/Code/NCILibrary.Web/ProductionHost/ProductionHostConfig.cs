using System;
using System.Configuration;
using Common.Logging;
using NCI.Web.ProductionHost.Configuration;

namespace NCI.Web.ProductionHost
{
    public static class ProductionHostConfig
    {
        static ILog log = LogManager.GetLogger(typeof(ProductionHostConfig));

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
                    log.Debug("ProductionHostConfig(): Found ProductionHostSection.");

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

                    log.DebugFormat("ProductionHostConfig(): members after searching StringConditions - _hostname = {0}, _sitename = {1}", _hostname, _sitename);
                }
                else
                {
                    log.Debug("ProductionHostConfig(): No ProductionHostSection found.");
                }
            }
            catch (Exception e)
            {
                log.Error("ProductionHostConfig(): Error encountered while loading configuration.", e);
            }
        }
    }
}
