using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using CDR.Common;
using CDR.Summary;
using NCI.Util;
using System.Xml;

namespace CancerGov.Handlers
{
    class NewSummaryRSSHandler : IHttpHandler
    {

        const int DEFAULT_MAXIMUM_RETURNCOUNT = 20;
        const SummaryType DEFAULT_SUMMARY_TYPE = SummaryType.All;
        const NewOrUpdateStatus DEFAULT_NEW_OR_UPDATED = NewOrUpdateStatus.Both;
        const CDRLanguage DEFAULT_LANGUAGE = CDRLanguage.English;

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }


        public void ProcessRequest(HttpContext context)
        {
            List<SummaryInfo> summaries = null;

            try
            {
                // Audience is a required parameter
                if (string.IsNullOrEmpty(context.Request.Params["Audience"]))
                    throw new ArgumentNullException("Audience");

                TargetAudience audience =
                    ConvertEnum<TargetAudience>.Convert(Strings.Clean(context.Request.Params["Audience"]));

                int NumItems = Strings.ToInt(context.Request.Params["NumItems"], DEFAULT_MAXIMUM_RETURNCOUNT);
                SummaryType type =
                    ConvertEnum<SummaryType>.Convert(Strings.Clean(context.Request.Params["SummaryType"]), DEFAULT_SUMMARY_TYPE);
                NewOrUpdateStatus newOrUpdated =
                    ConvertEnum<NewOrUpdateStatus>.Convert(Strings.Clean(context.Request.Params["NewOrUpdated"]), DEFAULT_NEW_OR_UPDATED);
                CDRLanguage language =
                    ConvertEnum<CDRLanguage>.Convert(Strings.Clean(context.Request.Params["Language"]), DEFAULT_LANGUAGE);

                // Get the list of summaries and pass off to rendering.
                SummaryManager manager = new SummaryManager();
                summaries = manager.LoadNewOrUpdatedSummaries(NumItems, audience, type, newOrUpdated, language);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }

            //RenderRSSFeed(context.Response, summaries);
        }

        #endregion



        //private void RenderRSSFeed(HttpResponse response, List<SummaryInfo> summaryList)
        //{
        //    RssFeed feed = new RssFeed();
        //    string siteURL = ConfigurationManager.AppSettings["RootUrl"].ToString();
        //    if (!string.IsNullOrEmpty(siteURL))
        //        siteURL = siteURL.Trim();

        //    // RSS Feed header material.
        //    feed.Channel.Link = new Uri(siteURL);
        //    feed.Channel.Title = "NCI Cancer Information Sumamries";
        //    feed.Channel.Description = "NCI Cancer Information Sumamries";

        //    // Turn the individual summaries into RSS items.
        //    if (summaryList != null)
        //    {
        //        foreach (SummaryInfo summary in summaryList)
        //        {
        //            RssItem item = new RssItem();

        //            // Pretty URL always starts with a /.
        //            string url = siteURL + summary.PrettyUrl;

        //            item.Title = summary.Title;
        //            item.Description = summary.Description;
        //            item.PublicationDate = summary.PublicationDate.ToUniversalTime();
        //            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
        //                item.Link = new Uri(url);
        //            item.Categories.Add(new RssCategory(summary.Category));

        //            feed.Channel.AddItem(item);
        //        }
        //    }

        //    // Write the RSS to the response output.
        //    XmlTextWriter writer = new XmlTextWriter(response.OutputStream, System.Text.Encoding.UTF8);
        //    feed.Save(writer);
        //    writer.Flush();
        //    writer.Close();

        //    response.ContentEncoding = System.Text.Encoding.UTF8;
        //    response.ContentType = "text/xml";
        //    response.Cache.SetCacheability(HttpCacheability.Public);

        //    response.End();
        //}

    }
}
