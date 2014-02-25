using System.Configuration;
using NCI.Web.UI.WebControls.Disqus.Configuration;
using System;
using NCI.Logging;

namespace NCI.Web.UI.WebControls.Disqus
{
    public static class DisqusConfig
    {
        private static bool _isProd = false;

        public static bool IsProd
        {
            get { return _isProd; }
        }

        static DisqusConfig()
        {
            try
            {
                DisqusSection section = (DisqusSection)ConfigurationManager.GetSection("nci/disqus");
                if (section != null)
                {
                    Logger.LogError("NCI:DisqusConfig.cs:DisqusConfig",
                        "Found DisqusSection.",
                        NCIErrorLevel.Debug);

                    foreach (BooleanDisqusElement elem in section.BooleanConditions)
                    {
                        Logger.LogError("NCI:DisqusConfig.cs:DisqusConfig",
                            "Found DisqusSection element " + elem.Name,
                            NCIErrorLevel.Debug);

                        if (elem.Name == "isProd")
                        {
                            _isProd = elem.Value;
                        }
                    }
                }
                else
                {
                    Logger.LogError("NCI:DisqusConfig.cs:DisqusConfig",
                        "No DisqusSection found.",
                        NCIErrorLevel.Debug);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("NCI:DisqusConfig.cs:DisqusConfig", 
                    "Error encountered while loading configuration.", 
                    NCIErrorLevel.Error, e);
            }
        }
    }
}
