using System;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using NCI.Web.CDE.Conditional.Configuration;


namespace NCI.Web.CDE.Conditional
{
    public static class ConditionalConfig
    {
        private static bool _atColo = false;

        public static bool AtColo
        {
            get { return _atColo; }
        }

        static ConditionalConfig()
        {

            ConditionalSection section = (ConditionalSection)ConfigurationManager.GetSection("nci/web/conditional");
            foreach (BooleanConditionElement elem in section.BooleanConditions)
            {
                if (elem.Name == "atColo")
                {
                    _atColo = elem.Value;
                }
            }
        }
    }
}
