using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCGA.Web;
using System.Web.UI.HtmlControls;
using TCGA.UI;
using NCI.Web.CDE;

namespace TCGA.Web.Common.PopUps
{
    public partial class PopDefinition : PopUpPage
    {
        private IRenderer content;
        private string urlArgs = "";
        private string header = "popHeader.htm";
        private string footer = "popFooter.htm";

        #region Page properties

        /// <summary>
        /// Sets definition frame source document arguments
        /// </summary>
        public string UrlArgs
        {
            get { return urlArgs; }
            set { urlArgs = value; }
        }

        /// <summary>
        /// Sets header frame document source
        /// </summary>
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>
        /// Sets footer frame document source
        /// </summary>
        public string Footer
        {
            get { return footer; }
            set { footer = value; }
        }

        #endregion

        #region Page properties

        public IRenderer Content
        {
            get { return content; }
            set { content = value; }
        }

        #endregion

        /// <summary>
        /// Default web form class constructor
        /// </summary>
        public PopDefinition()
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {

                        base.OnLoad(e);
            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web)
            {
                header = "popTextHeader.htm";
                footer = "popTextFooter.htm";
                if (Request.QueryString["language"] != "English")
                {
                    footer = "popTextFooterSpanish.htm";
                    header = "popTextHeaderSpanish.htm";
                }
            }
            else
            {
                if (Request.QueryString["language"] != "English")
                {
                    footer = "popFooterSpanish.htm";
                    header = "popHeaderSpanish.htm";
                }
            }

            urlArgs = Request.Url.Query.Substring(1);
        }

                protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

                #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
