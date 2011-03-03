using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CancerGov.Web
{
    public partial class PopEmail : PopUpPage
    {
        private string urlArgs = "";
        private string header = "popHeader.htm";
        private string footer = "popFooter.htm";

        protected void Page_Load(object sender, EventArgs e)
        {

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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            header = "popTextHeader.htm";
            footer = "popTextFooter.htm";
            if (this.DisplayLanguage == DisplayLanguage.Spanish)
            {
                header = "popTextHeaderSpanish.htm";
                footer = "popTextFooterSpanish.htm";
            }

        }
    }
}
