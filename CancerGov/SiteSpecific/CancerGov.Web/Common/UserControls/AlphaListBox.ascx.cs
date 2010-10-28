namespace Www.Common.UserControls
{
    using System;
    using NCI.Web.CDE;

    ///<summary>
    ///HTML alphabetic index box<br/>
    ///<br/>
    ///<b>Author</b>:  Greg Andres<br/>
    ///<b>Date</b>:  9-15-2001<br/>
    ///<br/>
    ///<b>Revision History</b>:<br/>
    ///<br/>
    ///</summary>
    public partial class AlphaListBox : System.Web.UI.UserControl
    {
        private string alphaListItems = "";
        private string title = "Alphabetical List of Cancers";
        private string colSpan = "11";
        private string baseUrl = "";
        private string urlArgs = "";
        private bool numericItems = false;
        private bool textOnly = false;
        private bool showAll = false;
        private string[] boxItems;
        private string webAnalyticsFunction;
        private bool doWebAnalytics = false;

        #region Page properties

        /// <summary>
        /// Enables web form access to built HTML index links
        /// </summary>
        public string AlphaListItems
        {
            get { return alphaListItems; }
            set { alphaListItems = value; }
        }

        public string ExtraTd
        {
            get
            {
                string td = "<td style=\"background-image: none;\" valign=\"top\"><img src=\"/images/spacer.gif\"  border=\"0\"  width=\"1\" alt=\"\"></td>";
                if (showAll)
                    return "";
                else
                    return td;
            }
        }


        /// <summary>
        /// Sets base url for index links
        /// </summary>
        public string BaseUrl
        {
            get { return baseUrl; }
            set { baseUrl = value; }
        }

        /// <summary>
        /// Indicates index contains numeric items
        /// </summary>
        public bool NumericItems
        {
            get { return numericItems; }
            set { numericItems = value; }
        }

        public string ColSpan
        {
            get { return colSpan; }
            set { colSpan = value; }
        }

        /// <summary>
        /// Shows All at the end of the list
        /// </summary>
        public bool ShowAll
        {
            get { return showAll; }
            set { showAll = value; }
        }

        /// <summary>
        /// Sets title for box, enables web form access to title
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Sets standard url arguments for index links
        /// </summary>
        public string UrlArgs
        {
            get { return urlArgs; }
            set { urlArgs = "&" + value; }
        }

        /// <summary>
        /// Sets content version
        /// </summary>
        public bool TextOnly
        {
            get { return textOnly; }
            set { textOnly = value; }
        }

        /// <summary>
        /// Sets characters to display in box control
        /// </summary>
        public string[] BoxItems
        {
            get { return boxItems; }
            set { boxItems = value; }
        }

        /// <summary>
        /// Sets the Web Analytics onclick JavaScript function name 
        /// /// </summary>
        public string WebAnalyticsFunction
        {
            get { return webAnalyticsFunction; }
            set
            {
                webAnalyticsFunction = value;
                doWebAnalytics = true;
            }
        }

        #endregion

        /// <summary>
        /// Default user control class constructor
        /// </summary>
        public AlphaListBox()
        {
            this.Init += new System.EventHandler(Page_Init);
        }

        /// <summary>
        /// Builds HTML alphabet links from class properties
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            alphaListItems += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"98%\">\n";
            alphaListItems += "	<tbody><tr>\n";

            if (boxItems == null || boxItems.Length == 0)
            {
                if (numericItems)
                {
                    if (doWebAnalytics)
                        alphaListItems += "<td><a class=\"dictionary-alpha-list\" href=\"" + PageAssemblyContext.Current.requestedUrl + "?expand=" + Server.UrlEncode("#") + urlArgs + "\" onclick=" + webAnalyticsFunction + "(this,'#') >#</a></td>\n";
                    else
                        alphaListItems += "<td><a class=\"dictionary-alpha-list\" href=\"" + PageAssemblyContext.Current.requestedUrl + "?expand=" + Server.UrlEncode("#") + urlArgs + "\">#</a></td>\n";
                }

                for (int i = 65; i < 91; i++)
                {
                    if (doWebAnalytics)
                        alphaListItems += "<td><a class=\"dictionary-alpha-list\" href=\"" + PageAssemblyContext.Current.requestedUrl + "?expand=" + (char)i + urlArgs + "\" onclick=" + webAnalyticsFunction + "(this,'" + (char)i + "') >" + (char)i + "</a></td>\n";
                    else
                        alphaListItems += "<td><a class=\"dictionary-alpha-list\" href=\"" + PageAssemblyContext.Current.requestedUrl + "?expand=" + (char)i + urlArgs + "\">" + (char)i + "</a></td>\n";
                }
            }
            else
            {
                foreach (string item in boxItems)
                {
                    if (doWebAnalytics)
                        alphaListItems += "<td><a class=\"dictionary-alpha-list\" href=\"" + PageAssemblyContext.Current.requestedUrl + "?expand=" + Server.UrlEncode(item) + urlArgs + "\" onclick=" + webAnalyticsFunction + "(this,'" + item + "') >" + item + "</a></td>\n";
                    else
                        alphaListItems += "<td><a class=\"dictionary-alpha-list\" href=\"" + PageAssemblyContext.Current.requestedUrl + "?expand=" + Server.UrlEncode(item) + urlArgs + "\">" + item + "</a></td>\n";
                }
            }

            if (showAll)
            {
                if (doWebAnalytics)
                    alphaListItems += "<td><a class=\"dictionary-alpha-list\" href=\"" + PageAssemblyContext.Current.requestedUrl + "?expand=" + Server.UrlEncode("All") + urlArgs + "\" onclick=" + webAnalyticsFunction + "(this,'ALL') >" + "All" + "</a></td>\n";
                else
                    alphaListItems += "<td><a class=\"dictionary-alpha-list\" href=\"" + PageAssemblyContext.Current.requestedUrl + "?expand=" + Server.UrlEncode("All") + urlArgs + "\">" + "All" + "</a></td>\n";
            }

            //alphaListItems += "	<td width=\"1\"><img src=\"/images/spacer.gif\" alt=\"\" border=\"0\" width=\"1\" height=\"30\"></td>\n";
            alphaListItems += "	</tr></tbody>\n";
            alphaListItems += "</table>\n";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        #region Web Form Designer generated code
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}
