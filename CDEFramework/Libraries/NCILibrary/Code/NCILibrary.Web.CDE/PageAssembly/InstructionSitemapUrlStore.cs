
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using Common.Logging;

using NCI.Web.CDE.Configuration;
using NCI.Web.Sitemap;
using NCI.Logging;

namespace NCI.Web.CDE.PageAssembly
{
    public class InstructionSitemapUrlStore : SitemapUrlStoreBase
    {
        static ILog log = LogManager.GetLogger(typeof(InstructionSitemapUrlStore));

        private String hostName = ContentDeliveryEngineConfig.CanonicalHostName.CanonicalUrlHostName.CanonicalHostName;

        /// <summary>
        /// Selects out the DoNotIndex property
        /// </summary>
        /// <param name="nav">The XPathNavigator representing the Instruction</param>
        /// <param name="manager">The namespace manager for the Instruction</param>
        /// <param name="instructionType">The type of the Instruction (SinglePageAssemblyInstruction or GenericFileInstruction)</param>
        /// <returns>The value of the DoNotIndex node, or false if it does not exist</returns>
        private bool DoNotIndex(XPathNavigator nav, XmlNamespaceManager manager, string instructionType)
        {
            string nodePath = String.Format("//cde:{0}/SearchMetadata/DoNotIndex", instructionType);

            try
            {
                //Try and select it as a boolean returning the actual value as a boolean
                return nav.SelectSingleNode(nodePath, manager).ValueAsBoolean;
            }
            catch { } //If it could not convert or be found, then continue.                        

            return false; //Default is DoNotIndex = false;  This is in case the node is not found or not boolean text
        }

        /// <summary>
        /// Gets the URL for this item
        /// </summary>
        /// <param name="nav">The XPathNavigator representing the Instruction</param>
        /// <param name="manager">The namespace manager for the Instruction</param>
        /// <param name="instructionType">The type of the Instruction (SinglePageAssemblyInstruction or GenericFileInstruction)</param>
        /// <returns>The value of the PrettyUrl node with the base URL, or null if it does not exist</returns>
        private string GetURL(XPathNavigator nav, XmlNamespaceManager manager, string instructionType)
        {
            string nodePath = String.Format("//cde:{0}/PrettyUrl", instructionType);
            string url = null;

            try
            {
                //Get out URL from node
                url = nav.SelectSingleNode(nodePath, manager).Value;

                if (url != null && url.Trim() != "")
                {
                    // If the URL is good, remove outer text and concatenate with base URL
                    url = hostName + url;
                }
                else
                {
                    // Otherwise, set the URL back to null
                    url = null;
                }
            }
            catch { }

            return url;
        }

        /// <summary>
        /// Create a collection of URL elements from XML files
        /// </summary>
        /// <returns>SitemapUrlSet</returns>
        public override SitemapUrlSet GetSitemapUrls()
        {
            List<SitemapUrl> sitemapUrls = new List<SitemapUrl>();
            String path;
            String contentType;
            double priority;
            String directory = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.PagePathFormat.Path, "/"));
            string fileDirectory = Path.GetDirectoryName(directory);
            SitemapProviderConfiguration config = (SitemapProviderConfiguration)ConfigurationManager.GetSection("Sitemap");
            int maxErrorCount = config.ErrorCount.Max;
            int errorCount = 0;
            List<String> errorMessages = new List<String>();

            // Find all Page Instruction files and add them to the list of URLs
            foreach (string file in Directory.GetFiles(fileDirectory, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    // Open new XPathDocument from file and create navigator
                    XPathDocument doc = new XPathDocument(file);
                    XPathNavigator nav = doc.CreateNavigator();

                    // Add CDE namespace to parse through document
                    XmlNamespaceManager manager = new XmlNamespaceManager(nav.NameTable);
                    manager.AddNamespace("cde", "http://www.example.org/CDESchema");

                    //If this item is marked as DoNotIndex, then skip it.
                    if (DoNotIndex(nav, manager, "SinglePageAssemblyInstruction"))
                        continue;

                    // Get pretty url from PrettyUrl node
                    path = GetURL(nav, manager, "SinglePageAssemblyInstruction");
                    if (path == null)
                        continue;

                    // Get content type and set priority accordingly
                    contentType = nav.SelectSingleNode("//cde:SinglePageAssemblyInstruction/ContentItemInfo/ContentItemType", manager).Value;
                    if (contentType == "rx:nciHome" || contentType == "rx:nciLandingPage" || contentType == "rx:cgvCancerTypeHome" ||
                        contentType == "rx:cgvCancerResearch" || contentType == "rx:nciAppModulePage" || contentType == "rx:pdqCancerInfoSummary" ||
                        contentType == "rx:pdqDrugInfoSummary" || contentType == "rx:cgvFactSheet" || contentType == "rx:cgvTopicPage")
                        priority = 1.0;
                    else
                    {
                        priority = 0.5;
                    }

                    sitemapUrls.Add(new SitemapUrl(path, sitemapChangeFreq.weekly, priority));
                }

                // If we hit missing or malformed XML, increment the error counter, send an error email, and move on to the next file without adding this to the sitemap
                catch (XmlException ex)
                {
                    ++errorCount;
                    errorMessages.Add("A PageInstruction XML file has failed parsing in IntructionSitemapUrlStore:GetSitemapUrls().\nFile: " + file + "\nEnvironment: " + System.Environment.MachineName + "\nRequest Host: " + HttpContext.Current.Request.Url.Host + "\n" + ex.ToString());
                    continue;
                }
            }

            directory = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.FilePathFormat.Path, "/"));
            fileDirectory = Path.GetDirectoryName(directory);

            // Find all File Instruction files and add them to the list of URLs
            foreach (string file in Directory.GetFiles(fileDirectory, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    // Open new XPathDocument from file and create navigator
                    XPathDocument doc = new XPathDocument(file);
                    XPathNavigator nav = doc.CreateNavigator();

                    // Add CDE namespace to parse through document
                    XmlNamespaceManager manager = new XmlNamespaceManager(nav.NameTable);
                    manager.AddNamespace("cde", "http://www.example.org/CDESchema");

                    //If this item is marked as DoNotIndex, then skip it.
                    if (DoNotIndex(nav, manager, "GenericFileInstruction"))
                        continue;

                    // Get pretty url from PrettyUrl node
                    path = GetURL(nav, manager, "GenericFileInstruction");
                    if (path == null)
                        continue;

                    sitemapUrls.Add(new SitemapUrl(path, sitemapChangeFreq.always, 0.5));
                }

                // If we hit missing or malformed XML, increment the error counter, send an error email, and move on to the next file without adding this to the sitemap
                catch (XmlException ex)
                {
                    ++errorCount;
                    errorMessages.Add("A FileInstruction XML file has failed parsing in IntructionSitemapUrlStore:GetSitemapUrls().\nFile: " + file + "\nEnvironment: " + System.Environment.MachineName + "\nRequest Host: " + HttpContext.Current.Request.Url.Host + "\n" + ex.ToString());
                    continue;
                }
            }

            // The maximum number of allowable errors is set in the web config. 
            // If our error count is greater than that number, stop trying to build the sitemap and throw an exception.
            if (errorCount <= maxErrorCount)
            {
                if(errorCount > 0)
                {
                    String err = String.Join("\n", errorMessages.ToArray());
                    log.Fatal(err);
                }
                return new SitemapUrlSet(sitemapUrls);
            }
            else
            {
                String err = "Error generating sitemap above threshold of " + maxErrorCount.ToString() + "Check page and file instruction XML files. IntructionSitemapUrlStore:GetSitemapUrls()";
                log.Error(err);
                throw new Exception(err);
            }
        }
    }
}
