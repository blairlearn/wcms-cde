using System;
using System.IO;
using System.Collections.Generic;
using NCI.Logging;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using System.Threading;
using System.Web;
using System.Globalization;

namespace NCI.Web.CDE.Modules
{
    public class VelocityTemplate
    {
        public static string MergeTemplateWithResults(string template, object obj)
        {
            try
            {
                Velocity.Init();
                VelocityContext context = new VelocityContext();
                context.Put("DynamicSearch", obj);
                context.Put("Tools", new VelocityTools());
                StringWriter writer = new StringWriter();
                Velocity.Evaluate(context, writer, "", template);
                return writer.GetStringBuilder().ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError("VelocityTemplate:MergeTemplateWithResults", "Failed to when evaluating results template and object." ,NCIErrorLevel.Error);
                throw (ex);
            }
        }

        public static string MergeTemplateWithResultsByFilepath(string filepath, object obj)
        {
            try
            {
                Velocity.Init();
                VelocityContext context = new VelocityContext();
                context.Put("SearchResults", obj);
                context.Put("CDEContext", new CDEContext());
                context.Put("PageContext", HttpContext.Current);
                context.Put("Tools", new VelocityTools());
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(filepath));
                string template = sr.ReadToEnd();
                sr.Close();
                //String template = File.ReadAllText(HttpContext.Current.Server.MapPath(filepath));
                StringWriter writer = new StringWriter();
                Velocity.Evaluate(context, writer, "", template);
                return writer.GetStringBuilder().ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError("VelocityTemplate:MergeTemplateWithResultsByFilepath", "Failed to when evaluating results template and object.", NCIErrorLevel.Error);
                throw (ex);
            }
        }

        class CDEContext
        {
            public string Language { get; set; }
            public CDEContext()
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "es")
                    //CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "es")
                {
                    Language = "es";
                }
                else
                {
                    Language = "en";
                }
            }
        }

        class VelocityTools
        {
            public bool IsNull(object obj)
            {
                return obj == null;
            }

            public bool IsNullOrWhitespace(string str)
            {
                return String.IsNullOrWhiteSpace(str);
            }
            public List<string> CreateEmptyStringList()
            {
                return new List<string>();
            }
        }
    }
}
