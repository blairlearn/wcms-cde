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
                    if (!String.IsNullOrEmpty(_di.AudioMediaHTML))
                        return "<span class=\"mtd_audio\">" + AudioMediaHTML + "</span>&nbsp;&nbsp;<span class=\"mtd_pronounce\">" + TermPronunciation + "</span>";
                    else
                        return "<span class=\"mtd_pronounce\">" + TermPronunciation + "</span>";
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
                    return "<span class=\"imageCaption\">" + _imageCaption + "</span>";
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
                        return _di.TermPronunciation;
                    else
                        return "";
                }
                else
                    return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String cdrId = Strings.Clean(Request.QueryString["cdrid"]);
            String languageParam = Strings.Clean(Request.QueryString["language"]);
            String lastSearch = Strings.Clean(Request.QueryString["lastSearch"]);

            string pageTitle;
            string buttonText;
            string language;
            MobileTermDictionary.DetermineLanguage(languageParam, out language, out pageTitle, out buttonText);
            litSearchBlock.Text = MobileTermDictionary.SearchBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), lastSearch, language, pageTitle, buttonText);
            litPageUrl.Text = MobileTermDictionary.RawUrlClean(Page.Request.RawUrl);


            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
            {
                url.SetUrl(Page.Request.RawUrl);
            }); 

            if (!String.IsNullOrEmpty(cdrId))
            {
                _di = TermDictionaryManager.GetDefinitionByTermID(language, cdrId, "", 1);
                dissectMediaHTML(_di.MediaHTML);
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