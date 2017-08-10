using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrialsAPI;
using System.Web.UI;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// This is the base class for all API-Based Clinical Trial Search controls.
    /// This will also serve as the base for the Search controls as they do not need the API.
    /// <remarks>This replaces BasicCTSBaseControl</remarks>
    /// </summary>
    public abstract class BaseAPICTSControl: SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(BaseAPICTSControl));

        /// <summary>
        /// Gets the Snippet Controls Config.
        /// </summary>
        protected BasicCTSPageInfo Config { get; private set; }

        /// <summary>
        /// Gets the path to the template.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetTemplatePath();

        /// <summary>
        /// Gets the data that should be bound to the Velocity Control
        /// </summary>
        protected abstract object GetDataForTemplate();

        /// <summary>
        /// Provides a method to be called on initialization.  If overridden you must call base!
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// Overrides the OnPreRender event and forces derrived classes to handle events here.
        /// </summary>
        /// <param name="e"></param>
        protected override sealed void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string templatePath = this.GetTemplatePath();
            object templateData = this.GetDataForTemplate();

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                templatePath, 
                templateData
                )
            );
            Controls.Add(ltl);
        }



        /// <summary>
        /// DO NOT IMPLEMENT ANYTHING HERE OR IN DERRIVED CLASSES.
        /// </summary>
        /// <param name="e"></param>
        protected override sealed void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Force config to be loaded here and not rely on others to call base from init. :)

            //////////////////////////////
            // Load the configuration XML from the App Settings
            string configPath = ConfigurationManager.AppSettings["CTSConfigFilePath"];
            Config = ModuleObjectFactory<BasicCTSPageInfo>.GetObjectFromFile(configPath);

            /*
            try
            {
                if (string.IsNullOrEmpty(configPath))
                    throw new Exception("CTSConfigFilePath XML file name cannot be null.");

                try
                {
                    using (FileStream xmlFile = File.Open(configPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(xmlFile))
                        {
                            // Get the serializer for the BasicCTSPageInfo configuration.
                            XmlSerializer serializer = new XmlSerializer(typeof(BasicCTSPageInfo), "cde");

                            // Deserialize the XML into an object.
                            Config = (BasicCTSPageInfo)serializer.Deserialize(xmlReader);
                        }
                    }
                }
                catch
                {
                    throw new Exception(String.Format("Unable to create BasicCTSPageInfo Config for file \"{0}.\"", configPath));
                }

                //Config = ModuleObjectFactory<BasicCTSPageInfo>.GetModuleObject(spidata);
            }
            catch (Exception ex)
            {
                log.Error("could not load the BasicCTSPageInfo, check the configuration file in Percussion", ex);
                throw ex;
            }
            */

            Init();
        }

        /// <summary>
        /// DO NOT IMPLEMENT ANYTHING HERE OR IN DERRIVED CLASSES.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}
