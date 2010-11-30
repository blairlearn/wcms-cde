using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CancerGov.CDR.ClinicalTrials;
using NCI.Util;
using System.Xml;
using System.Configuration;
using Argotic.Syndication;

namespace CancerGov.Handlers
{
    class NewProtocolRSSHandler : IHttpHandler
    {

        const int DEFAULT_MAXIMUM_RETURNCOUNT = 40;

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            List<ClinicalTrialInfo> trials = null;

            try
            {
                // Get the list of protocols and pass off to rendering.
                int NumItems = Strings.ToInt(context.Request.Params["NumItems"], DEFAULT_MAXIMUM_RETURNCOUNT);

                ClinicalTrialManager manager = new ClinicalTrialManager();
                trials = manager.LoadNewAndActiveProtocols(NumItems);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }

            // Write the Feed data
            RenderRSSFeed(context.Response, trials);
        }

        #endregion


        /// <summary>
        /// Helper method to create an RSS from a list of clinical trials (protocols).
        /// </summary>
        /// <param name="response">The HTTP response for writing the feed to.</param>
        /// <param name="triallist">List of clinical trials.</param>

        private void RenderRSSFeed(HttpResponse response, List<ClinicalTrialInfo> triallist)
        {
            RssFeed feed = new RssFeed();
            string siteURL = ConfigurationManager.AppSettings["RootUrl"].ToString();
            if (!string.IsNullOrEmpty(siteURL))
                siteURL = siteURL.Trim();
            if (!siteURL.EndsWith("/"))
                siteURL += "/";

            // RSS Feed header material.
            feed.Channel.Link = new Uri(siteURL);
            feed.Channel.Title = "NCI Clinical Trials";
            feed.Channel.Description = "NCI Clinical Trials";

            // Turn the individual trials into RSS items.
            if (triallist != null)
            {
                foreach (ClinicalTrialInfo trial in triallist)
                {
                    RssItem item = new RssItem();

                    item.Title = trial.HealthProfessionalTitle;
                    item.Description = trial.Description;
                    item.PublicationDate = trial.PublicationDate.ToUniversalTime();

                    // Build a link to the protocol's pretty URL.
                    if (!string.IsNullOrEmpty(siteURL) && !string.IsNullOrEmpty(trial.PrettyUrlID))
                    {
                        // After being retrieved, siteURL is already guaranteed to end with a / (see above).
                        string url = siteURL
                            + "clinicaltrials/"
                            + trial.PrettyUrlID;
                        item.Link = new Uri(url);
                    }

                    // Add categories to the RSS item.
                    trial.Categories.ForEach(category => item.Categories.Add(new RssCategory(category)));

                    feed.Channel.AddItem(item);
                }
            }

            // Write the RSS to the response output.
            XmlTextWriter writer = new XmlTextWriter(response.OutputStream, System.Text.Encoding.UTF8);
            feed.Save(writer);
            writer.Flush();
            writer.Close();

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "text/xml";
            response.Cache.SetCacheability(HttpCacheability.Public);

            response.End();
        }        

    }
}
