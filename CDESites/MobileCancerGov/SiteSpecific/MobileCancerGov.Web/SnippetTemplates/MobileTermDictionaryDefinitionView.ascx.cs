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
using CancerGov.Text;
using CancerGov.CDR.TermDictionary;
using CancerGov.Common.ErrorHandling;
using MobileCancerGov.Web.SnippetTemplates;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE.UI.SnippetControls;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileTermDictionaryDefinitionView : SnippetControl
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
                            return "<span class=\"CDR_audiofile\">" + AudioMediaHTML + "</span>";
                        else
                            return "<span class=\"CDR_audiofile\">" + AudioMediaHTML + "</span>&nbsp;&nbsp;<span class=\"mtd_pronounce\">" + TermPronunciation + "</span>";
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
        public string Language
        {
            get { return _language; }
            set { _language = value; }
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
                        if (Language == MobileTermDictionary.SPANISH)
                            return "";
                        else
                            return _di.TermPronunciation;
                    }
                    else
                        return "";
                }
                else
                    return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string startingUrl = "";

            try
            {
                startingUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentUrl").ToString();
                string cdrId = Strings.Clean(Request.QueryString["cdrid"]);
                string term = Strings.Clean(Request.QueryString["term"]);
                string id = Strings.Clean(Request.QueryString["id"]);
                string languageParam = Strings.Clean(Request.QueryString["language"]);

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
                MobileTermDictionary.DetermineLanguage(languageParam, out _language, out pageTitle, out buttonText, out reDirect);


                //if (!String.IsNullOrEmpty(reDirect))
                //{
                //    string reDirectUrl = "http://" + Page.Request.Url.Authority.ToString() + reDirect + "?cdrid=" + cdrId;
                //    Page.Response.Redirect(reDirectUrl);
                //}

                if (!String.IsNullOrEmpty(cdrId))
                {
                    _di = TermDictionaryManager.GetDefinitionByTermID(_language, cdrId, "", 1);
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

                        PageAssemblyContext.Current.PageAssemblyInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.DictionaryTermView, wbField =>
                        {
                            wbField.Value = "";
                        });

                        litPageUrl.Text = startingUrl;
                        litSearchBlock.Text = MobileTermDictionary.SearchBlock(startingUrl, "", _language, pageTitle, buttonText, true);
                    }
                    else
                        Page.Response.Redirect(startingUrl); // if no data returned - redirect to base page
                }
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("MobileTermDictionaryDefinitionView", 2, ex);
                Page.Response.Redirect(startingUrl); // if error - redirect to base page
            }
        }

        private void dissectMediaHTML(string mediaHTML)
        {
            if(!String.IsNullOrEmpty(mediaHTML))
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
                _imageLarge  = imageLarge;
                _imageSmall = imageSmall;
                _imageCaption = imageCaption;
                _imageAlt = imageAlt;

            }
        }
    }
}