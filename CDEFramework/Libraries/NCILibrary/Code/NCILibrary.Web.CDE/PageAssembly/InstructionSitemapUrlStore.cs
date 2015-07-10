using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCI.Web.Sitemap;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE.PageAssembly
{
    public class InstructionSitemapUrlStore : SitemapUrlStoreBase
    {
        public override SitemapUrlSet GetSitemapUrls()
        {
            List<SitemapUrl> sitemapUrls = new List<SitemapUrl>();
            String path;
            String contentType;
            double priority;
            String directory = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.PagePathFormat.Path, "/"));
            string fileDirectory = Path.GetDirectoryName(directory);

            // Find all Page Instruction files and add them to the list of URLs
            foreach (string file in Directory.GetFiles(fileDirectory, "*.xml", SearchOption.AllDirectories))
            {
                // Open new XPathDocument from file and create navigator
                XPathDocument doc = new XPathDocument(file);
                XPathNavigator nav = doc.CreateNavigator();

                // Add CDE namespace to parse through document
                XmlNamespaceManager manager = new XmlNamespaceManager(nav.NameTable);
                manager.AddNamespace("cde", "http://www.example.org/CDESchema");

                // Get pretty url from PrettyUrl node
                if (nav.SelectSingleNode("//cde:SinglePageAssemblyInstruction/PrettyUrl", manager) != null)
                {
                    path = nav.SelectSingleNode("//cde:SinglePageAssemblyInstruction/PrettyUrl", manager).Value;
                }
                else
                {
                    continue;
                }

                // Remove outer text and concatenate with base URL
                path.Replace("<![CDATA[", "");
                path.Replace("]]>", "");
                path = "http://www.cancer.gov" + path;

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

            directory = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.FilePathFormat.Path, "/"));
            fileDirectory = Path.GetDirectoryName(directory);

            // Find all File Instruction files and add them to the list of URLs
            foreach (string file in Directory.GetFiles(fileDirectory, "*.xml", SearchOption.AllDirectories))
            {
                // Open new XPathDocument from file and create navigator
                XPathDocument doc = new XPathDocument(file);
                XPathNavigator nav = doc.CreateNavigator();

                // Add CDE namespace to parse through document
                XmlNamespaceManager manager = new XmlNamespaceManager(nav.NameTable);
                manager.AddNamespace("cde", "http://www.example.org/CDESchema");

                // Get pretty url from PrettyUrl node
                if (nav.SelectSingleNode("//cde:GenericFileInstruction/PrettyUrl", manager) != null)
                {
                    path = nav.SelectSingleNode("//cde:GenericFileInstruction/PrettyUrl", manager).Value;
                }
                else
                {
                    continue;
                }

                // Remove outer text and concatenate with base URL
                path = "http://www.cancer.gov" + path;

                sitemapUrls.Add(new SitemapUrl(path, sitemapChangeFreq.always, 0.5));
            }

            return new SitemapUrlSet(sitemapUrls);
        }
    }
}
