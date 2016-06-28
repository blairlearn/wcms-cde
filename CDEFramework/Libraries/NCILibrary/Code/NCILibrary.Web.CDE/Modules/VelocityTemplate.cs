using System;
using System.IO;
using System.Collections.Generic;
using NCI.Logging;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Globalization;

namespace NCI.Web.CDE.Modules
{
    public class VelocityTemplate
    {
        private static VelocityEngineManager _engineManager = new VelocityEngineManager();

        private static FileSystemWatcher templateDirectoryWatcher;

        [Obsolete("Use MergeTemplateWithResultsByFilepath() instead.")]
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
                Logger.LogError("VelocityTemplate:MergeTemplateWithResults", "Failed when evaluating results template and object.", NCIErrorLevel.Error, ex);
                throw (ex);
            }
        }

        public static string MergeTemplateWithResultsByFilepath(string filepath, object obj)
        {
            try
            {
                VelocityEngine velocity = _engineManager.Engine;
                WatchTemplateDirectory(filepath);

                velocity.Init();
                VelocityContext context = new VelocityContext();
                context.Put("SearchResults", obj);
                context.Put("CDEContext", new CDEContext());
                context.Put("PageContext", HttpContext.Current);
                context.Put("Tools", new VelocityTools());
                StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(filepath));
                string template = sr.ReadToEnd();
                sr.Close();
                StringWriter writer = new StringWriter();
                velocity.Evaluate(context, writer, "", template);
                return writer.GetStringBuilder().ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError("VelocityTemplate:MergeTemplateWithResultsByFilepath", "Failed when evaluating results template and object.", NCIErrorLevel.Error, ex);
                throw;
            }
        }

        private static void WatchTemplateDirectory(string filepath)
        {
            if (templateDirectoryWatcher == null)
            {
                lock (typeof(VelocityTemplate))
                {
                    if(templateDirectoryWatcher == null)
                    {
                        filepath = HttpContext.Current.Server.MapPath(filepath);

                        templateDirectoryWatcher = new FileSystemWatcher((Path.GetDirectoryName(filepath)));
                        templateDirectoryWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.LastAccess | NotifyFilters.Attributes;
                        templateDirectoryWatcher.EnableRaisingEvents = true;

                        templateDirectoryWatcher.Changed += new FileSystemEventHandler(TemplatesChanged);
                        templateDirectoryWatcher.Created += new FileSystemEventHandler(TemplatesChanged);
                        templateDirectoryWatcher.Deleted += new FileSystemEventHandler(TemplatesChanged);
                        templateDirectoryWatcher.Renamed += new RenamedEventHandler(TemplatesChanged);
                    }
                }
            }
        }

        private static void TemplatesChanged(object src, FileSystemEventArgs e)
        {
            _engineManager.ResetVelocityEngine();
        }

        private static void TemplatesRenamed(object src, RenamedEventArgs e)
        {
            _engineManager.ResetVelocityEngine();
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

            public string Replace(string str, string pattern1, string pattern2)
            {
                string rtn = str.Replace(pattern1, pattern2);
                return rtn;
            }
        }

        /// <summary>
        /// Helper class to manage use of a single instance of the Velocity Engine.
        /// </summary>
        private class VelocityEngineManager
        {
            /// <summary>
            /// The instance. Not available for access outside this helper class.
            /// </summary>
            private static VelocityEngine _velocityEngine = null;

            /// <summary>
            /// Read-only property for accessing the managed instance of VelocityEngine.
            /// By design, use of this property enforces thread-safe access to the engine.
            /// All callers will have their own reference to the managed instance, which
            /// remains valid after a call to ResetVelocityEngine().
            /// </summary>
            public VelocityEngine Engine
            {
                get
                {
                    lock (this)
                    {
                        if (_velocityEngine == null)
                            _velocityEngine = new VelocityEngine();

                        return _velocityEngine;
                    }
                }
            }

            /// <summary>
            /// Thread-safe mechanism for removing the managed VelocityEngine instance.
            /// </summary>
            public void ResetVelocityEngine()
            {
                lock (this)
                {
                    _velocityEngine = null;
                }
            }
        }
    }
}
