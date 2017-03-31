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
    internal class SitemapLoaderAsyncImpl
    {
        private SitemapResults _results = new SitemapResults(true);
        private string _hostName = string.Empty;

        /// <summary>
        /// Creates a new instance of a Internal Sitemap Loader
        /// </summary>
        /// <param name="hostName">The hostname to pre-pend to URLs </param>
        public SitemapLoaderAsyncImpl(string hostName)
        {
            this._hostName = hostName;
        }

        /// <summary>
        /// This extracts the required info from both PageInstructions and FileInstructions in an async way
        /// in order to create a SitemapUrl.  
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task<SitemapUrl> GetSitemapUrlFromXmlAsync(string filePath, sitemapChangeFreq sitemapChangeFreq)
        {
            double priority = 0.5;
            string sitemapUrl = string.Empty;

            using (StreamReader reader = new StreamReader(filePath))
            {
                using (XmlReader xmlreader = XmlReader.Create(reader, new XmlReaderSettings() { Async = true }))
                {
                    while (await xmlreader.ReadAsync())
                    {
                        if (xmlreader.NodeType == XmlNodeType.Element)
                        {
                            switch (xmlreader.Name)
                            {
                                case "DoNotIndex":
                                    {
                                        //Set this up as a short circuit.  If we should not index, just return null.
                                        string tmpStr = await xmlreader.ReadElementContentAsStringAsync();
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
                                        switch (await xmlreader.ReadElementContentAsStringAsync())
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
                                        string purl = await xmlreader.ReadElementContentAsStringAsync();

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
        /// Helper function to asynchronusly loop over instruction directory.
        /// Found at http://writeasync.net/?p=2621
        /// </summary>
        /// <param name="rootdir"></param>
        /// <param name="doAsync"></param>
        /// <returns></returns>
        private static async Task ForEachFileAsync(string rootdir, Func<string, Task> doAsync)
        {
            // Avoid blocking the caller for the initial enumerate call.
            await Task.Yield();

            foreach (string file in Directory.EnumerateFiles(rootdir, "*.xml", SearchOption.AllDirectories))
            {
                await doAsync(file);
            }
        }

        /// <summary>
        /// Asynchronously fetches sitemap URLs
        /// </summary>
        /// <returns>An InternalSitemapResults with the URLs, as well as errors.</returns>
        public async Task<SitemapResults> GetSitemapUrlsAsync()
        {
            String path;
            String contentType;
            double priority;

            String directory = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.PagePathFormat.Path, "/"));
            string fileDirectory = Path.GetDirectoryName(directory);

            IEnumerable<string> pageInstructions = Directory.EnumerateFiles(fileDirectory, "*.xml", SearchOption.AllDirectories);
            IEnumerable<SitemapUrl> urls = await Task.WhenAll(
                pageInstructions.Select(
                    file => Task.Run(async () =>
                    {
                        return await GetSitemapUrlFromXmlAsync(file, sitemapChangeFreq.weekly);
                    })
                )
            );

            /*
            await ForEachFileAsync(fileDirectory, async file =>
            {
                try
                {
                    SitemapUrl url = await GetSitemapUrlFromXmlAsync(file, sitemapChangeFreq.weekly);
                    if (url != null)
                    {
                        _results.AddSitemapUrl(url);
                    }
                }
                // If we hit missing or malformed XML, increment the error counter, send an error email, and move on to the next file without adding this to the sitemap
                catch (XmlException ex)
                {
                    this._results.AddError(string.Format(
                        "A PageInstruction XML file has failed parsing in IntructionSitemapUrlStore:GetSitemapUrls().\nFile: {0}\nEnvironment: {1}\nRequest Host: {2}\n{3}\n",
                        file,
                        System.Environment.MachineName,
                        HttpContext.Current.Request.Url.Host,
                        ex.ToString()
                    ));
                }

            });
            */

            /*
            // Find all Page Instruction files and add them to the list of URLs
            foreach (string file in Directory.GetFiles(fileDirectory, "*.xml", SearchOption.AllDirectories))
            {

                try
                {
                    SitemapUrl url = await GetSitemapUrlFromXmlAsync(file, sitemapChangeFreq.weekly);
                    if (url != null)
                    {
                        _results.AddSitemapUrl(url);
                    }
                }
                // If we hit missing or malformed XML, increment the error counter, send an error email, and move on to the next file without adding this to the sitemap
                catch (XmlException ex)
                {
                    this._results.AddError(string.Format(
                        "A PageInstruction XML file has failed parsing in IntructionSitemapUrlStore:GetSitemapUrls().\nFile: {0}\nEnvironment: {1}\nRequest Host: {2}\n{3}\n",
                        file,
                        System.Environment.MachineName,
                        HttpContext.Current.Request.Url.Host,
                        ex.ToString()
                    ));
                    continue;
                }
            }
            */

            directory = HttpContext.Current.Server.MapPath(String.Format(ContentDeliveryEngineConfig.PathInformation.FilePathFormat.Path, "/"));
            fileDirectory = Path.GetDirectoryName(directory);

            await ForEachFileAsync(fileDirectory, async file =>
            {
                try
                {
                    SitemapUrl url = await GetSitemapUrlFromXmlAsync(file, sitemapChangeFreq.always);
                    if (url != null)
                    {
                        _results.AddSitemapUrl(url);
                    }
                }

                // If we hit missing or malformed XML, increment the error counter, send an error email, and move on to the next file without adding this to the sitemap
                catch (XmlException ex)
                {

                    _results.AddError(string.Format(
                        "A FileInstruction XML file has failed parsing in IntructionSitemapUrlStore:GetSitemapUrls().\nFile: {0}\nEnvironment: {1}\nRequest Host: {2}\n{3}\n",
                        file,
                        System.Environment.MachineName,
                        HttpContext.Current.Request.Url.Host,
                        ex.ToString()
                    ));
                }
            });

            /*
            // Find all File Instruction files and add them to the list of URLs
            foreach (string file in Directory.GetFiles(fileDirectory, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    SitemapUrl url = await GetSitemapUrlFromXmlAsync(file, sitemapChangeFreq.always);
                    if (url != null)
                    {
                        _results.AddSitemapUrl(url);
                    }
                }

                // If we hit missing or malformed XML, increment the error counter, send an error email, and move on to the next file without adding this to the sitemap
                catch (XmlException ex)
                {

                    _results.AddError(string.Format(
                        "A FileInstruction XML file has failed parsing in IntructionSitemapUrlStore:GetSitemapUrls().\nFile: {0}\nEnvironment: {1}\nRequest Host: {2}\n{3}\n",
                        file,
                        System.Environment.MachineName,
                        HttpContext.Current.Request.Url.Host,
                        ex.ToString()
                    ));
                    continue;
                }
            }
             */



            return _results;
        }
    }

}
