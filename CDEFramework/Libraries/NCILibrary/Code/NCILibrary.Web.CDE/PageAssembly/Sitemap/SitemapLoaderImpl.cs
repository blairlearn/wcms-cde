using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.XPath;

using Common.Logging;

using NCI.Web.CDE.Configuration;
using NCI.Web.Sitemap;
using NCI.Logging;

namespace NCI.Web.CDE.PageAssembly.Sitemap
{
    /// <summary>
    /// This is the class that does the real work.  This has been created because InstructionSitemapUrlStore
    /// is meant to be shared across multiple sitemap requests.  However, we have a number of state variables
    /// with regard to error counts and such that would be handled best as class variables so we can refactor
    /// the code in a more modular way.
    /// </summary>
    internal class SitemapLoaderImpl
    {
        static ILog log = LogManager.GetLogger(typeof(SitemapLoaderImpl));        

        private List<string> _errorMessages = new List<string>();
        private int _errorCount = 0;
        private readonly int _maxErrorCount;
        private string _hostName = string.Empty;

        /// <summary>
        /// Gets the messages of the errors encountered in processing these items.
        /// </summary>
        public IEnumerable<string> ErrorMessages
        {
            get
            {
                return _errorMessages;
            }
        }

        /// <summary>
        /// Gets the total errors encountered.
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return _errorCount;
            }
        }


        /// <summary>
        /// Creates a new instance of a Internal Sitemap Loader
        /// </summary>
        /// <param name="hostName">The hostname to pre-pend to URLs </param>
        /// <param name="maxErrorCount">The maximum number of errors to allow before quitting.</param>
        public SitemapLoaderImpl(string hostName, int maxErrorCount)
        {
            this._hostName = hostName;
            this._maxErrorCount = maxErrorCount;
        }

        /// <summary>
        /// This extracts the required info from both PageInstructions and FileInstructions in an sync way
        /// in order to create a SitemapUrl.  
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private SitemapUrl GetSitemapUrlFromXml(string filePath, sitemapChangeFreq sitemapChangeFreq)
        {
            double priority = 0.5;
            string sitemapUrl = string.Empty;

            using (StreamReader reader = new StreamReader(filePath))
            {
                using (XmlReader xmlreader = XmlReader.Create(reader, new XmlReaderSettings()))
                {
                    while (xmlreader.Read())
                    {
                        if (xmlreader.NodeType == XmlNodeType.Element)
                        {
                            switch (xmlreader.Name)
                            {
                                case "DoNotIndex":
                                    {
                                        //Set this up as a short circuit.  If we should not index, just return null.
                                        string tmpStr = xmlreader.ReadElementContentAsString();
                                        bool doNotIndex;

                                        bool.TryParse(tmpStr, out doNotIndex);

                                        if (doNotIndex)
                                        {
                                            return null;
                                        }

                                        break;
                                    }
                                case "ContentItemType":
                                    {
                                        //Try to determine priority.  Some types have a higher
                                        //priority than others.  This should be in a config though.
                                        //TODO: Move types to config.
                                        switch (xmlreader.ReadElementContentAsString())
                                        {
                                            case "rx:nciHome":
                                            case "rx:nciLandingPage":
                                            case "rx:cgvCancerTypeHome":
                                            case "rx:cgvCancerResearch":
                                            case "rx:nciAppModulePage":
                                            case "rx:pdqCancerInfoSummary":
                                            case "rx:pdqDrugInfoSummary":
                                            case "rx:cgvFactSheet":
                                            case "rx:cgvTopicPage":
                                                priority = 1.0;
                                                break;
                                            default:
                                                priority = 0.5;
                                                break;
                                        }

                                        break;
                                    }
                                case "PrettyUrl":
                                    {
                                        string purl = xmlreader.ReadElementContentAsString();

                                        if (!string.IsNullOrWhiteSpace(purl))
                                        {
                                            // If the URL is good, remove outer text and concatenate with base URL
                                            sitemapUrl = _hostName + purl;
                                        }
                                        //Else throw error???

                                        break;
                                    }
                            }
                        }
                    }
                }
            }

            //Return URL
            if (!string.IsNullOrWhiteSpace(sitemapUrl))
            {
                return new SitemapUrl(sitemapUrl, sitemapChangeFreq, priority);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Synchronously fetches sitemap URLs, yielding the resulting URL as soon as it is encountered.
        /// </summary>
        /// <returns>An InternalSitemapResults with the URLs, as well as errors.</returns>
        public IEnumerable<SitemapUrl> GetSitemapUrls()
        {

            String directory = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.PagePathFormat.Path, "/"));
            string fileDirectory = Path.GetDirectoryName(directory);

            // Find all Page Instruction files and add them to the list of URLs.  EnumerateFiles will start
            // returning filepaths as soon as the first is encountered.  This should help with long generation times
            // when asking for the sitemap after a long period between sitemap requests.
            foreach (string file in Directory.EnumerateFiles(fileDirectory, "*.xml", SearchOption.AllDirectories))
            {
                SitemapUrl url = null;

                try
                {
                    url = GetSitemapUrlFromXml(file, sitemapChangeFreq.weekly);
                }
                // If we hit missing or malformed XML, increment the error counter, send an error email, and move on to the next file without adding this to the sitemap
                catch (XmlException ex)
                {
                    this._errorCount++;
                    this._errorMessages.Add(string.Format(
                        "A PageInstruction XML file has failed parsing in SitemapLoaderImpl:GetSitemapUrls().\nFile: {0}\nEnvironment: {1}\nRequest Host: {2}\n{3}\n",
                        file,
                        System.Environment.MachineName,
                        HttpContext.Current.Request.Url.Host,
                        ex.ToString()
                    ));
                    continue;
                }

                // If we hit our fill of errors bail.
                if (_errorCount > _maxErrorCount)
                {
                    String err = "Error generating sitemap above threshold of " + _maxErrorCount.ToString() + "\nCheck page and file instruction XML files. SitemapLoaderImpl:GetSitemapUrls()";
                    log.Error(err);
                    throw new Exception(err);
                }

                if (url != null)
                {
                    yield return url;
                }
            }

            directory = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.FilePathFormat.Path, "/"));
            fileDirectory = Path.GetDirectoryName(directory);

            // Find all File Instruction files and add them to the list of URLs. EnumerateFiles will start
            // returning filepaths as soon as the first is encountered.  This should help with long generation times
            // when asking for the sitemap after a long period between sitemap requests.
            foreach (string file in Directory.EnumerateFiles(fileDirectory, "*.xml", SearchOption.AllDirectories))
            {
                SitemapUrl url = null;

                try
                {
                    url = GetSitemapUrlFromXml(file, sitemapChangeFreq.always);
                }

                // If we hit missing or malformed XML, increment the error counter, send an error email, and move on to the next file without adding this to the sitemap
                catch (XmlException ex)
                {
                    this._errorCount++;
                    this._errorMessages.Add(string.Format(
                        "A FileInstruction XML file has failed parsing in IntructionSitemapUrlStore:GetSitemapUrls().\nFile: {0}\nEnvironment: {1}\nRequest Host: {2}\n{3}\n",
                        file,
                        System.Environment.MachineName,
                        HttpContext.Current.Request.Url.Host,
                        ex.ToString()
                    ));
                    continue;
                }

                //If we hit our fill of errors bail
                if (_errorCount > _maxErrorCount)
                {
                    String err = "Error generating sitemap above threshold of " + _maxErrorCount.ToString() + "\nCheck page and file instruction XML files. SitemapLoaderImpl:GetSitemapUrls()";
                    log.Error(err);
                    throw new Exception(err);
                }

                if (url != null)
                {
                    yield return url;
                }
            }
        }
    }

}
