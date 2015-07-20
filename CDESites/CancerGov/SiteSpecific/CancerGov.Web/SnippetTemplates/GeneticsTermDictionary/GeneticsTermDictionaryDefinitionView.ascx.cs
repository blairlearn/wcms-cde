using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE.UI.SnippetControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web;
using CancerGov.Text;
using CancerGov.Common;
using CancerGov.CDR.TermDictionary;
using CancerGov.Web.SnippetTemplates;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class GeneticsTermDictionaryDefintionView : SnippetControl
    {
        private TermDictionaryDataItem _di = null;
        private string _imageSmall = "";
        private string _imageLarge = "";
        private string _imageAlt = "";
        private string _imageCaption = "";
        private string _language = "";

        // Properties 
        public string AudioMediaHTML
        {
            get
            {
                if (_di != null)
                {
                    if (!String.IsNullOrEmpty(_di.AudioMediaHTML))
                        return _di.AudioMediaHTML.Replace("[_audioMediaLocation]", ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]);
                    else
                        return "";
                }
                else
                    return "";
            }
        }
        public string AudioPronounceLink
        {
            get
            {
                if (_di != null)
                {
                    if (!String.IsNullOrEmpty(AudioMediaHTML))
                    {
                        if (String.IsNullOrEmpty(TermPronunciation))
                            return AudioMediaHTML;
                        else
                            return AudioMediaHTML + "<span class=\"mtd_pronounce\">" + TermPronunciation + "</span>";
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(TermPronunciation))
                            return "";
                        else
                            return "<span class=\"mtd_pronounce\">" + TermPronunciation + "</span>";
                    }
                }
                else
                    return "";
            }
        }
        public string DefinitionHTML
        {
            get { return (_di == null ? "" : _di.DefinitionHTML); }
        }
        public string ImageAlt
        {
            get
            {
                if (!String.IsNullOrEmpty(_imageAlt))
                    return _imageAlt;
                else
                    return "";
            }
        }
        public string ImageCaption
        {
            get
            {
                if (!String.IsNullOrEmpty(_imageCaption))
                    return _imageCaption;
                else
                    return "";
            }
        }
        public string ImageLarge
        {
            get
            {
                if (!String.IsNullOrEmpty(_imageLarge))
                    return _imageLarge;
                else
                    return "";
            }
        }
        public string ImageLink
        {
            get
            {
                if (!String.IsNullOrEmpty(_imageSmall) && !String.IsNullOrEmpty(_imageSmall))
                    return "<a href=\"" + ImageLarge + "\"><img id=\"Img1\" border=\"0\" src=\"" + ImageSmall + "\" alt=\"" + ImageAlt + "\" /></a>";
                else
                    return "";
            }
        }
        public string ImageSmall
        {
            get
            {
                if (!String.IsNullOrEmpty(_imageSmall))
                    return _imageSmall;
                else
                    return "";
            }
        }
        public string MediaHTML
        {
            get
            {
                if (_di != null)
                {
                    if (!String.IsNullOrEmpty(_di.MediaHTML))
                        return _di.MediaHTML.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);

                    else
                        return "";
                }
                else
                    return "";
            }
        }
        public string RelatedInfoHTML
        {
            get { return (_di == null ? "" : _di.RelatedInfoHTML); }
        }
        public string TermName
        {
            get { return (_di == null ? "" : _di.TermName); }
        }
        public string TermPronunciation
        {
            get
            {
                if (_di != null)
                {
                    if (!String.IsNullOrEmpty(_di.TermPronunciation))
                    {
                        //if (Language == GeneticsTermDictionaryHelper.SPANISH)
                        //    return "";
                        //else
                            return _di.TermPronunciation;
                    }
                    else
                        return "";
                }
                else
                    return "";
            }
        }
        
        //public string Language
        //{
        //    get { return _language; }
        //    set { _language = value; }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            string startingUrl = "";

            try
            {
                startingUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentUrl").ToString();
                string cdrId = Strings.Clean(Request.QueryString["cdrid"]);
                string term = Strings.Clean(Request.QueryString["term"]);
                string id = Strings.Clean(Request.QueryString["id"]);
                //string languageParam = Strings.Clean(Request.QueryString["language"]);
                string languageParam = ""; //disable language selection by query parameter 

                if (String.IsNullOrEmpty(cdrId) && (!String.IsNullOrEmpty(id)))
                    cdrId = id;

                if (cdrId.Contains("CDR"))
                    cdrId = cdrId.Replace("CDR", "");

                // Determine langauge based PageAssemblyContext.Current.PageAssemblyInstruction.Language and 
                // looking at a language query parameter - currently language selection by query parameter
                // is turned off
                string pageTitle; // output parameter 
                string buttonText; // output parameter 
                string reDirect; // output parameter 
                
                GeneticsTermDictionaryHelper.DetermineLanguage(languageParam, out _language, out pageTitle, out buttonText, out reDirect);


                if (!String.IsNullOrEmpty(cdrId))
                {
                    _di = TermDictionaryManager.GetDefinitionByTermID(_language, cdrId, "Health professional", 1);
                    if (_di != null)
                    {
                        dissectMediaHTML(_di.MediaHTML);

                        // Setup Url Filters 
                        PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("CurrentUrl", (name, url) =>
                        {
                            url.QueryParameters.Add("cdrid", cdrId);
                        });

                        PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.AltLanguage, (name, url) =>
                        {
                            url.QueryParameters.Add("cdrid", cdrId);
                        });

                        // Add Drug Dictionary Term view event to analytics
                        PageAssemblyContext.Current.PageAssemblyInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.event12, wbField =>
                        {
                            wbField.Value = "";
                        });

                        litPageUrl.Text = startingUrl;
                        litSearchBlock.Text = GeneticsTermDictionaryHelper.SearchBlock(startingUrl, "", _language, pageTitle, buttonText, false);
                    }
                    else
                        Page.Response.Redirect(startingUrl); // if no data returned - redirect to base page
                }
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("GeneticsTermDictionaryDefinitionView", 2, ex);
                Page.Response.Redirect(startingUrl); // if error - redirect to base page
            }
        }

        private void dissectMediaHTML(string mediaHTML)
        {
            if (!String.IsNullOrEmpty(mediaHTML))
            {
                int imageLargeStart = mediaHTML.IndexOf("imageName=") + 10;
                int imageLargeEnd = mediaHTML.IndexOf("\'", imageLargeStart);
                int imageSmallStart = 0;
                int imageSmallEnd = 0;
                int imageAltStart = 0;
                int imageAltEnd = 0;
                string imageLarge = "";
                string imageSmall = "";
                string imageCaption = "";
                string imageAlt = "";

                if (imageLargeStart != -1 && imageLargeEnd != -1 && imageLargeStart < imageLargeEnd)
                {
                    imageLarge = mediaHTML.Substring(imageLargeStart, imageLargeEnd - imageLargeStart);
                    imageLarge = imageLarge.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);

                    imageAltStart = mediaHTML.IndexOf("alt=\"", imageLargeStart) + 5;
                    imageSmallStart = mediaHTML.IndexOf("src=\"", imageLargeStart) + 5;
                    imageSmallEnd = mediaHTML.IndexOf("\"", imageSmallStart);
                    if (imageSmallStart != -1 && imageSmallEnd != -1 && imageSmallStart < imageSmallEnd)
                    {
                        imageSmall = mediaHTML.Substring(imageSmallStart, imageSmallEnd - imageSmallStart);
                        imageSmall = imageSmall.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);
                        if (imageAltStart > -1)
                        {
                            imageAltEnd = mediaHTML.IndexOf("\"", imageAltStart);
                            if (imageAltEnd > -1 && imageAltEnd > imageAltStart)
                                imageAlt = mediaHTML.Substring(imageAltStart, imageAltEnd - imageAltStart);
                        }

                    }
                }

                if (imageLarge.IndexOf("caption=") > -1)
                {
                    int captionStart = imageLarge.IndexOf("&caption=");
                    imageCaption = imageLarge.Substring(captionStart + 9);
                    imageLarge = imageLarge.Substring(0, captionStart);
                }
                _imageLarge = imageLarge;
                _imageSmall = imageSmall;
                _imageCaption = imageCaption;
                _imageAlt = imageAlt;

            }
        }

    }
}