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
        private string browserTitle = "";

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

        /// <summary>
        /// Sets browser title based on language
        /// </summary>
        public string BrowserTitle
        {
            get { return browserTitle; }
            set { browserTitle = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            header = "popTextHeader.htm";
            footer = "popTextFooter.htm";
            browserTitle = "E-Mail This Page - National Cancer Institute";

            if (this.DisplayLanguage == DisplayLanguage.Spanish)
            {
                header = "popTextHeaderSpanish.htm";
                footer = "popTextFooterSpanish.htm";
                browserTitle = "Enviar esta p&aacute;gina por correo electr&oacute;nico - Instituto Nacional del C&amp;aacute;ncer";
            }

        }
    }
}
