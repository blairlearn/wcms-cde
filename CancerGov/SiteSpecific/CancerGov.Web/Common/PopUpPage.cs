using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE;

namespace CancerGov.Web
{
    /// <summary>
    /// Summary description for PopUpPage.
    /// </summary>
    public class PopUpPage : System.Web.UI.Page
    {
        protected HtmlHead pageHtmlHead = new HtmlHead();

        #region Page properties

        public HtmlHead PageHtmlHead
        {
            get { return pageHtmlHead; }
            set { pageHtmlHead = value; }
        }

        #endregion

        public PopUpPage()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CssManager.AddStyleSheet(this, GetStylesheetUrl());
            JSManager.AddExternalScript(this, "/scripts/imgEvents.js");
            JSManager.AddExternalScript(this, "/scripts/popEvents.js");
        }

        private DisplayVersions DisplayVersion
        {
            get
            {
                string printParam = Request.Params["print"];
                if (printParam != null)
                    return DisplayVersions.Print;
                else
                    return DisplayVersions.Web;
            }
        }

        #region GetDisplayLanguage method

        /// <summary>
        /// Resolves DisplayLanguage from parameters
        /// </summary>
        /// <param name="langParam">lang Request param</param>
        /// <returns></returns>
        protected DisplayLanguage DisplayLanguage
        {
            get
            {
                string langParam = Request.Params["language"];
                switch (langParam == null ? "" : langParam.Trim().ToLower())
                {
                    case "spanish":
                        return DisplayLanguage.Spanish;
                    default:
                        return DisplayLanguage.English;
                }
            }
        }

        #endregion

        #region GetStylesheetUrl method

        /// <summary>
        /// Sets the stylesheet location
        /// </summary>
        /// <param name="version">Page display version</param>
        /// <returns>url for stylesheet</returns>
        private string GetStylesheetUrl()
        {
            switch (this.DisplayVersion)
            {
                case DisplayVersions.Print:
                    return "/stylesheets/print_nci.css";
                default:
                    return "/stylesheets/nci.css";
            }
        }

        #endregion
    }
}


