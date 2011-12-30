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
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE.UI.SnippetControls;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileTermDictionaryDefinitionView : SnippetControl
    {
        private TermDictionaryDataItem di = null;
        private string _imageSmall = "";
        private string _imageLarge = "";
        private string _imageAlt = "";
        private string _imageCaption = "";
        private const string AudioImage = "<object id='audioLink705730' width='16' height='16' type='application/x-shockwave-flash' data='/PublishedContent/files/global/flash/speaker.swf?r=1324585448557' style='visibility: visible;'> ";

        // Properties 
        public string TermName
        {
            get { return (di == null ? "" : di.TermName); }
        }
        public string AudioMediaHTML
        {
            get
            {
                if (di != null)
                {
                    if (!String.IsNullOrEmpty(di.AudioMediaHTML))
                        return AudioImage + di.AudioMediaHTML.Replace("[_audioMediaLocation]", ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]);
                    else
                        return "";
                }
                else
                    return "";
            }
        }
        public string TermPronunciation
        {
            get
            {
                if (di != null)
                {
                    if (!String.IsNullOrEmpty(di.TermPronunciation))
                        return di.TermPronunciation;
                    else
                        return "";
                }
                else
                    return "";
            }
        }
        public string MediaHTML
        {
            get
            {
                if (di != null)
                {
                    if (!String.IsNullOrEmpty(di.MediaHTML))
                        return di.MediaHTML.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);

                    else
                        return "";
                }
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
        public string DefinitionHTML
        {
            get { return (di == null ? "" : di.DefinitionHTML); }
        }
        public string AudioPronounceLink
        {
            get
            {
                if (di != null)
                {
                    if (!String.IsNullOrEmpty(di.AudioMediaHTML))
                        return "<span class=\"audio\">" + AudioMediaHTML + "&nbsp;&nbsp;" + TermPronunciation + "</span>";
                    else
                        return "<span class=\"audio\">" + TermPronunciation + "</span>";
                }
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

        protected void Page_Load(object sender, EventArgs e)
        {

            String cdrId = Strings.Clean(Request.QueryString["cdrid"]);
            String language = Strings.Clean(Request.QueryString["language"]);
            String lastSearch = Strings.Clean(Request.QueryString["lastSearch"]);

            litPageUrl.Text = Page.Request.Url.LocalPath;
            litSearchBlock.Text = MobileTermDictionary.SearchBlock(Page.Request.Url.LocalPath, lastSearch);
            
            if(String.IsNullOrEmpty(language))
                language = "english"; //default to English

            if (!String.IsNullOrEmpty(cdrId))
            {
                di = TermDictionaryManager.GetDefinitionByTermID(language, cdrId, "", 1);
                dissectMediaHTML(di.MediaHTML);
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
                string imageLarge = "";
                string imageSmall = "";
                string imageCaption = "";

                if (imageLargeStart != -1 && imageLargeEnd != -1 && imageLargeStart < imageLargeEnd)
                {
                    imageLarge = mediaHTML.Substring(imageLargeStart, imageLargeEnd - imageLargeStart);
                    imageLarge = imageLarge.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);

                    imageSmallStart = mediaHTML.IndexOf("src=\"", imageLargeStart) + 5;
                    imageSmallEnd = mediaHTML.IndexOf("\"", imageSmallStart);
                    if (imageSmallStart != -1 && imageSmallEnd != -1 && imageSmallStart < imageSmallEnd)
                    {
                        imageSmall = mediaHTML.Substring(imageSmallStart, imageSmallEnd - imageSmallStart);
                        imageSmall = imageSmall.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);
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

            }
        }
    }
}