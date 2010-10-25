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
using CancerGov.UI;
using NCI.Web.CDE;
namespace www.Common.UserControls
{
    public partial class LangSwitch : System.Web.UI.UserControl
    {
        private DisplayInformation _displayInfo;
        private bool _enableBothLinks;


        #region Propeties
        public DisplayInformation DisplayInfo
        {
            get { return _displayInfo; }
            set { _displayInfo = value; }
        }

        public DisplayLanguage Language
        {
            get { return _displayInfo.Language; }
        }

        public DisplayVersion Version
        {
            get { return _displayInfo.Version; }
        }

        public string EnglishUrl
        {
            get { return hlEnglish.NavigateUrl; }
            set { hlEnglish.NavigateUrl = value; }
        }

        public string SpanishUrl
        {
            get { return hlSpanish.NavigateUrl; }
            set { hlSpanish.NavigateUrl = value; }
        }

        public bool EnableBothLinks
        {
            get { return _enableBothLinks; }
            set { _enableBothLinks = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            string language = string.Empty;
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
            {
                language = "English";
            }
            else
            {
                language = "Spanish";
            }

            if (language == "Spanish")
                SpanishSetup();
            else
                EnglishSetup();

            CommonSetup();
        }

        private void CommonSetup()
        {
            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Print)
            {
                pnlLangSelect.Visible = false;
            }
            else if (Version == DisplayVersion.Text)
            {
                hlEnglish.Text = "In English";
                hlSpanish.Text = "En espa&ntilde;ol";
                imgEnglish.Visible = false;
                imgSpanish.Visible = false;
                imgLangTabDivider.Visible = false;
                litTextLangDivider.Text = "&nbsp;&nbsp;&nbsp;";
            }

            imgLangTabDivider.Attributes.Add("alt", "");
        }

        private void EnglishSetup()
        {
            //hlSpanish.Enabled = true;
            if (!EnableBothLinks)
                hlEnglish.Enabled = false;

            if (Version == DisplayVersion.Text && !EnableBothLinks)
            {
                hlEnglish.Visible = false;
                lblEnglish.Visible = true;
            }
        }

        private void SpanishSetup()
        {
            //hlEnglish.Enabled = true;
            if (!EnableBothLinks)
                hlSpanish.Enabled = false;

            if (Version == DisplayVersion.Text && !EnableBothLinks)
            {
                hlSpanish.Visible = false;
                lblSpanish.Visible = true;
            }


            imgEnglish.ImageUrl = "/images/in-english-gray.gif";
            imgSpanish.ImageUrl = "/images/en-espanol-white.gif";
            imgLangTabDivider.ImageUrl = "/images/tab-divider-white-right.gif";


        }


    }
}