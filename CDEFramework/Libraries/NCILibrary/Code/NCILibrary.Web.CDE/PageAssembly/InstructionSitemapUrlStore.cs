
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

using NCI.Web.CDE.PageAssembly.Sitemap;
using NCI.Web.CDE.Configuration;
using NCI.Web.Sitemap;
using NCI.Logging;
using System.Threading.Tasks;

namespace NCI.Web.CDE.PageAssembly
{
    public class InstructionSitemapUrlStore : SitemapUrlStoreBase
    {
        static ILog log = LogManager.GetLogger(typeof(InstructionSitemapUrlStore));        

        /// <summary>
        /// Create a collection of URL elements from XML files
        /// </summary>
        /// <returns>SitemapUrlSet</returns>
        public override async Task<IEnumerable<SitemapUrl>> GetSitemapUrlsAsync()
        {
            throw new NotImplementedException();
            /*
            SitemapProviderConfiguration config = (SitemapProviderConfiguration)ConfigurationManager.GetSection("Sitemap");
            
            String hostName = ContentDeliveryEngineConfig.CanonicalHostName.CanonicalUrlHostName.CanonicalHostName;
            int maxErrorCount = config.ErrorCount.Max;

            SitemapLoaderAsyncImpl loader = new SitemapLoaderAsyncImpl(hostName);

            SitemapResults loaderResults = await loader.GetSitemapUrlsAsync();

            // The maximum number of allowable errors is set in the web config. 
            // If our error count is greater than that number, stop trying to build the sitemap and throw an exception.
            if (loaderResults.ErrorCount <= maxErrorCount)
            {
                if (loaderResults.ErrorCount > 0)
                {
                    String err = String.Join("\n", loaderResults.ErrorMessages);
                    log.Fatal(err);
                }
                return new SitemapUrlSet(loaderResults.SitemapUrls);
            }
            else
            {
                String err = "Error generating sitemap above threshold of " + maxErrorCount.ToString() + "\nCheck page and file instruction XML files. IntructionSitemapUrlStore:GetSitemapUrlsAsync()";
                log.Error(err);
                throw new Exception(err);
            }
             */
        }


        /// <summary>
        /// Create a collection of URL elements from XML files
        /// </summary>
        /// <returns>SitemapUrlSet</returns>
        
        public override IEnumerable<SitemapUrl> GetSitemapUrls()
        {
            SitemapProviderConfiguration config = (SitemapProviderConfiguration)ConfigurationManager.GetSection("Sitemap");

            String hostName = ContentDeliveryEngineConfig.CanonicalHostName.CanonicalUrlHostName.CanonicalHostName;
            int maxErrorCount = config.ErrorCount.Max;

            SitemapLoaderImpl loader = new SitemapLoaderImpl(hostName, maxErrorCount);

            //NOTE: this can throw if maxErrors occurred.  You cannot yield within a try/catch block
            //so the upstream handler will need to catch this.
            foreach (SitemapUrl url in loader.GetSitemapUrls())
            {
                //return as soon as a URL is encountered.
                yield return url;
            }

            //Log an error if errors were encountered, but did not hit the max errors threshold.
            if (loader.ErrorCount > 0)
            {
                String err = String.Join("\n", loader.ErrorMessages);
                log.Fatal(err);
            }
        }
    }
}
