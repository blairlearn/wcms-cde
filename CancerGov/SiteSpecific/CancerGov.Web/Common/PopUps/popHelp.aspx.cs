using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CancerGov.Web;
using CancerGov.UI;
using CancerGov.Text;
using NCI.Web.CDE;
namespace www.Common.PopUps
{
    /// <summary>
    /// Summary description for popHelp.
    /// </summary>
    public partial class popHelp : PopUpPage
    {
        private string header = "popHeader.htm";
        private string footer = "popFooter.htm";

        #region Page properties

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

        public popHelp()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.PageDisplayInformation.Version == DisplayVersions.Text)
            {
                header = "popTextHeader.htm";
                footer = "popTextFooter.htm";
            }
        }
    }
}
