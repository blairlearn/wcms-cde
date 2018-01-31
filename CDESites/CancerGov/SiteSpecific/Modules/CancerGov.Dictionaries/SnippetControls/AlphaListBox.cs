namespace CancerGov.Dictionaries.SnippetControls
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
    public class AlphaListBox : System.Web.UI.UserControl
    {
        protected System.Web.UI.WebControls.PlaceHolder phBrowseEnglish;

        protected System.Web.UI.WebControls.PlaceHolder phBrowseSpanish;

        private string alphaListItems = "";
        //private string title = "Alphabetical List of Cancers";
        //private string colSpan = "11";
        private string baseUrl = "";
        //private string urlArgs = "";
        //private bool numericItems = false;
        //private bool textOnly = false;
        private bool showAll = false;
        //private string[] boxItems;
        //private string webAnalyticsFunction;
        //private bool doWebAnalytics = false;

        #region Page properties

        /// <summary>
        /// Enables web form access to built HTML index links
        /// </summary>
        public string AlphaListItems
        {
            get { return alphaListItems; }
            set { alphaListItems = value; }
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
        /*public bool NumericItems
        {
            get { return numericItems; }
            set { numericItems = value; }
        }*/

        /*public string ColSpan
        {
            get { return colSpan; }
            set { colSpan = value; }
        }*/

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
        /*public string Title
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
        }*/

        /// <summary>
        /// Sets content version
        /// </summary>
        /*public bool TextOnly
        {
            get { return textOnly; }
            set { textOnly = value; }
        }*/

        /// <summary>
        /// Sets characters to display in box control
        /// </summary>
        /*public string[] BoxItems
        {
            get { return boxItems; }
            set { boxItems = value; }
        }*/

        /// <summary>
        /// Sets the Web Analytics onclick JavaScript function name 
        /// /// </summary>
        /*public string WebAnalyticsFunction
        {
            get { return webAnalyticsFunction; }
            set
            {
                webAnalyticsFunction = value;
                doWebAnalytics = true;
            }
        }*/

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
            alphaListItems = "<ul>";

            for (int i = 65; i < 91; i++)
            {
                alphaListItems += "<li><a href=\"" + BaseUrl + "?expand=" + (char)i + "\">" + (char)i + "</a></li>\n";
            }

            alphaListItems += "<li><a href=\"" + BaseUrl + "?expand=" + Server.UrlEncode("#") + "\">#</a></li>\n";

            if (showAll)
            {
                alphaListItems += "<li><a href=\"" + BaseUrl + "?expand=" + Server.UrlEncode("All") + "\">All</a></li>\n";
            }

            alphaListItems += "</ul>\n";

            //set visibilty for the English versus Spanish text
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                phBrowseEnglish.Visible = false;
                phBrowseSpanish.Visible = true;
            }
            else
            {
                phBrowseEnglish.Visible = true;
                phBrowseSpanish.Visible = false;
            }
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
