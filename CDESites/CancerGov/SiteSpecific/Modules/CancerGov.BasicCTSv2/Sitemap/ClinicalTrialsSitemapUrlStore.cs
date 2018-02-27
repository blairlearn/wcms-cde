using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using NCI.Web;
using NCI.Web.Sitemap;
using NCI.Web.CDE.Configuration;
using CancerGov.ClinicalTrialsAPI;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;

namespace CancerGov.ClinicalTrials.Basic.v2.Sitemap
{
    public class ClinicalTrialsSitemapUrlStore : SitemapUrlStoreBase
    {
        static ILog log = LogManager.GetLogger(typeof(ClinicalTrialsSitemapUrlStore));

        private String _hostName = ContentDeliveryEngineConfig.CanonicalHostName.CanonicalUrlHostName.CanonicalHostName;

        private static BasicCTSPageInfo _config = null;

        /// <summary>
        /// Create a collection of URL elements from a CSV file of dictionary entries
        /// </summary>
        /// <returns>SitemapUrlSet</returns>
        public override SitemapUrlSet GetSitemapUrls(string sitemapName)
        {
            List<SitemapUrl> sitemapUrls = new List<SitemapUrl>();

            _config = BasicCTSPageInfo.GetConfig();

            if(_config == null)
            {
                log.Error("Unable to read CTSPageInfo config file.");
                throw new Exception();
            }

            string path = _config.SitemapStore;

            if (path != null)
            {
                String file = HttpContext.Current.Server.MapPath(path);

                List<string> trialIDs = new List<string>();
                IEnumerable<ClinicalTrial> validTrials = null;

                try
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        string currentLine;
                        // currentLine will be null when the StreamReader reaches the end of file
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            if(currentLine.Length > 0)
                            {
                                trialIDs.Add(currentLine);
                            }
                            else
                            {
                                log.ErrorFormat("Error in clinical trials sitemap file {0} for line {1} : invalid syntax.", file, currentLine);
                                continue;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    log.ErrorFormat("Error in ClinicalTrialsSitemapUrlStore: unable to read dictionary sitemap file located at {0}.", file);
                }

                try
                {
                    BasicCTSManager _basicCTSManager = new BasicCTSManager(APIClientHelper.GetV1ClientInstance());

                    validTrials = _basicCTSManager.GetMultipleTrials(trialIDs);
                }
                catch
                {
                    log.ErrorFormat("Error in clinical trials sitemap file {0} : invalid clinical trial IDs", file);
                }

                if(validTrials != null)
                {
                    foreach (ClinicalTrial trial in validTrials)
                    {
                        string entryUrl = GetSitemapUrl(trial.NCIID);
                        double priority = 0.5;

                        sitemapUrls.Add(new SitemapUrl(entryUrl, sitemapChangeFreq.weekly, priority));
                    }
                }

                return new SitemapUrlSet(sitemapUrls);
            }
            else
            {
                log.ErrorFormat("Could not load clinical trials provider file located at {0}.", path);
                return new SitemapUrlSet();
            }
        }

        /// <summary>
        /// Gets the friendly name of the given CDRID, if it exists.
        /// Otherwise, returns the CDRID.
        /// </summary>
        public string GetSitemapUrl(string nciid)
        {
            NciUrl url = new NciUrl();
            url.SetUrl(_hostName);
            url.AppendPathSegment(_config.DetailedViewPagePrettyUrl);
            url.QueryParameters.Add("id", nciid);

            return _hostName + url.ToString();
        }
    }
}
