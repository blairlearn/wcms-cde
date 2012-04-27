using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NCI.Text;
using NCI.Web.CDE.UI;
using CancerGov.Text;
using NCI.Web.CDE;
using Www.Common.UserControls;
using NCI.Web.CDE.WebAnalytics;
using CancerGov.CDR.TermDictionary;
using CancerGov.UI;
using NCI.Web.UI.WebControls.FormControls;
using CancerGov.Web;
namespace Www.Common.PopUps
{
    ///<summary>
    ///Defines frame sources for definition popup<br/>
    ///<br/>
    ///<b>Author</b>:  Greg Andres<br/>
    ///<b>Date</b>:  10-15-2001<br/>
    ///<br/>
    ///<b>Revision History</b>:<br/>
    ///<br/>
    ///</summary>
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

        /// <summary>
        /// Event method sets frame content version and parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web)
            {
                header = "popTextHeader.htm";
                footer = "popTextFooter.htm";
                if (Request.QueryString["language"] == "Spanish")
                {
                    footer = "popTextFooterSpanish.htm";
                    header = "popTextHeaderSpanish.htm";
                }
            }
            else
            {
                if (Request.QueryString["language"] == "Spanish")
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
