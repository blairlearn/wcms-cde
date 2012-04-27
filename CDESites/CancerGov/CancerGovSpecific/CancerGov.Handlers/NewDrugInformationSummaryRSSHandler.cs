using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CDR.Common;
using CancerGov.CDR.ClinicalTrials;
using NCI.Util;
using Argotic.Syndication;
using System.Configuration;
using System.Xml;
using CDR.DrugInformationSummary;

namespace CancerGov.Handlers
{
    public class NewDrugInformationSummaryRSSHandler : IHttpHandler
    {

        const int DEFAULT_MAXIMUM_RETURNCOUNT = 20;
        const NewOrUpdateStatus DEFAULT_NEW_OR_UPDATED = NewOrUpdateStatus.Both;

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            List<DrugInformationSummaryInfo> summaries = null;

            try
            {
                // Retrieve RSS parameters
                int NumItems = Strings.ToInt(context.Request.Params["NumItems"], DEFAULT_MAXIMUM_RETURNCOUNT);
                NewOrUpdateStatus newOrUpdated =
                    ConvertEnum<NewOrUpdateStatus>.Convert(Strings.Clean(context.Request.Params["NewOrUpdated"]), DEFAULT_NEW_OR_UPDATED);

                // Get the list of drug information summaries and pass off to rendering.
                DrugInformationSummaryManager manager = new DrugInformationSummaryManager();
                summaries = manager.LoadNewOrUpdatedDrugInformationSummaries(NumItems, newOrUpdated);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }

            // Write the Feed data
            RenderRSSFeed(context.Response, summaries);
        }

        #endregion

        /// <summary>
        /// Helper method to create an RSS from a list of summaries.
        /// </summary>
        /// <param name="response">The HTTP response for writing the feed to.</param>
        /// <param name="summaryList">List of summaries.</param>

        private void RenderRSSFeed(HttpResponse response, List<DrugInformationSummaryInfo> summaryList)
        {
            RssFeed feed = new RssFeed();
            string siteURL = ConfigurationManager.AppSettings["RootUrl"].ToString();
            if (!string.IsNullOrEmpty(siteURL))
                siteURL = siteURL.Trim();

            // RSS Feed header material.
            feed.Channel.Link = new Uri(siteURL);
            feed.Channel.Title = "NCI Drug Information Sumamries";
            feed.Channel.Description = "NCI Drug Information Sumamries";

            // Turn the individual summaries into RSS items.
            if (summaryList != null)
            {
                summaryList.ForEach(summary =>
                {
                    RssItem item = new RssItem();

                    // Pretty URL always starts with a /.
                    string url = siteURL + summary.PrettyUrl;

                    item.Title = summary.Title;
                    item.Description = summary.Description;
                    item.PublicationDate = summary.PublicationDate.ToUniversalTime();

                    if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                        item.Link = new Uri(url);

                    feed.Channel.AddItem(item);
                });
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
