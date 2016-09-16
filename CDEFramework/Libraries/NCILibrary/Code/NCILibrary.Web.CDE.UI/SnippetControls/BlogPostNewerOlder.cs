using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using Common.Logging;
using NCI.DataManager;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI.Configuration;

namespace NCI.Web.CDE.UI.SnippetControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:BlogPostNewerOlder runat=server></{0}:BlogPostNewerOlder>")]
    public class BlogPostNewerOlder : SnippetControl
    {
        #region Private Members
        static ILog log = LogManager.GetLogger(typeof(BlogPostNewerOlder));

         BlogSearchList _blogSearchList = null;

        /// <summary>
        /// The current page that is being used.
        /// </summary>
        private int CurrentPage
        {
            get
            {
                if (string.IsNullOrEmpty(this.Page.Request.Params["page"]))
                    return 1;
                return Int32.Parse(this.Page.Request.Params["page"]);
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// SitePath is a search criteria used in searching
        /// </summary>

        protected string SiteName
        {
            get
            {
                if (this.BlogSearchList.SearchParameters == null)
                    return string.Empty;
                return this.BlogSearchList.SearchParameters.SiteName;
            }
        }

        protected virtual BlogSearchList BlogSearchList
        { get; set; }

        #endregion

        #region Public
        public void Page_Load(object sender, EventArgs e)
        {
            if (this.BlogSearchList == null)
                this.BlogSearchList = ModuleObjectFactory<BlogSearchList>.GetModuleObject(SnippetInfo.Data);
            
                   
            processData();
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    processData();
        //}

        #endregion

        #region Private Methods
        private void processData()
        {
          
            try
            {
                if (this.BlogSearchList != null)
                {
                    // Validate();

                  
                    string siteName = SiteName;
                 
                    

                    // Call the  datamanger to perform the search
                    ICollection<SeriesPrevNextResult> searchResults =
                                BlogSearchDataManager.Execute(this.BlogSearchList.Filter, this.BlogSearchList.ContentID,
                                this.BlogSearchList.Language, Settings.IsLive, siteName);

                    BlogSearchResult blogSearch = new BlogSearchResult();
                    blogSearch.Results = searchResults;
                   
                    blogSearch.SiteName = siteName;

                   
                    LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResults(this.BlogSearchList.ResultsTemplate, blogSearch));
                    Controls.Add(ltl);
           
                }
            }
            catch (Exception ex)
            {
                log.Error("this.SearchListSnippet:processData", ex);
            }
        }

       
        
        /// <summary>
        /// Validates the data received from the xml, throws an exception if the required 
        /// fields are null or empty.
        /// </summary>
        /// <param name="this.SearchList">The object whose properties are being validated.</param>
        private void Validate()
        {
            if (string.IsNullOrEmpty(this.BlogSearchList.Filter) ||
                string.IsNullOrEmpty(this.BlogSearchList.ResultsTemplate) ||
                string.IsNullOrEmpty(this.BlogSearchList.ContentID))
                throw new Exception("One or more of these fields SearchFilter,ResultsTemplate,SearchType cannot be empty, correct the xml data.");

        }
        #endregion

    }
}

