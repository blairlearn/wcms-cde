using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using CDR.Common;

namespace CancerGov.Handlers
{
    class NewSummaryRSSHandler 
    {

        const int NumItems = 20;
        const SummaryType DEFAULT_SUMMARY_TYPE = SummaryType.All;
        const NewOrUpdated DEFAULT_NEW_OR_UPDATED = NewOrUpdated.Both;
        const Language DEFAULT_LANGUAGE = Language.English;

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }


        //public void ProcessRequest(HttpContext context)
        //{
        //    List<SummaryInfo> summaries = null;

        //    try
        //    {
        //        // Audience is a required parameter
        //        if (string.IsNullOrEmpty(context.Request.Params["Audience"]))
        //            throw new ArgumentNullException("Audience");

        //        Audience audience =
        //            ConvertEnum<Audience>.Convert(Strings.Clean(context.Request.Params["Audience"]));

        //        int NumItems = Strings.ToInt(context.Request.Params["NumItems"], NumItems);
        //        SummaryType type =
        //            ConvertEnum<SummaryType>.Convert(Strings.Clean(context.Request.Params["SummaryType"]), DEFAULT_SUMMARY_TYPE);
        //        NewOrUpdated newOrUpdated =
        //            ConvertEnum<NewOrUpdated>.Convert(Strings.Clean(context.Request.Params["NewOrUpdated"]), DEFAULT_NEW_OR_UPDATED);
        //        Language language =
        //            ConvertEnum<Language>.Convert(Strings.Clean(context.Request.Params["Language"]), DEFAULT_LANGUAGE);

        //        // Get the list of summaries and pass off to rendering.
        //        SummaryManager manager = new SummaryManager();
        //        summaries = manager.LoadNewOrUpdatedSummaries(NumItems, audience, type, newOrUpdated, language);
        //    }
        //    catch (Exception ex)
        //    {
        //        context.Response.Write(ex.Message);
        //    }

        //    RenderRSSFeed(context.Response, summaries);
        //}

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
