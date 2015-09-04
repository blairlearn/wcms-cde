using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using CancerGov.CDR.TermDictionary;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class TermDictionaryResultsList : SnippetControl
    {
        public string SearchStr{ get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public int NumResults { get; set; }

        public string DictionaryURL { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = DictionaryType.Term;
            dictionarySearchBlock.DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            //base.OnLoad(e);
            GetQueryParams();
            ValidateParams();
            
            //Set display props according to lang
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                lblResultsFor.Text = "resultados de:";
            }
            else
            {
                lblResultsFor.Text = "results found for:";
            }

            SetupCommon();

            LoadData();

        }

        private void LoadData()
        {
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

            SearchType searchType = SearchType.Begins;
            if (BContains)
                searchType = SearchType.Contains;

            DictionarySearchResultCollection resultCollection = null;

            if (!String.IsNullOrEmpty(SearchStr)) // SearchString provided, do a term search
            {

                resultCollection = _dictionaryAppManager.Search(SearchStr, searchType, 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.term, PageAssemblyContext.Current.PageAssemblyInstruction.Language);

            }
            else if (!String.IsNullOrEmpty(Expand)) // A-Z expand provided - do an A-Z search
            {

                if (Expand.ToLower() == "all")
                    resultCollection = _dictionaryAppManager.Expand("%", "", 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.term, PageAssemblyContext.Current.PageAssemblyInstruction.Language, "v1");
                else
                    resultCollection = _dictionaryAppManager.Expand(Expand, "", 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.term, PageAssemblyContext.Current.PageAssemblyInstruction.Language, "v1");
            }

            if (resultCollection != null && resultCollection.Count() > 0)
            {
                //if there is only 1 record - go directly to definition view
                if ((resultCollection.ResultsCount == 1) && string.IsNullOrEmpty(Expand))
                {
                    // Get the first (only) item so we can redirect to it specifically
                    IEnumerator<DictionarySearchResult> itemPtr = resultCollection.GetEnumerator();
                    itemPtr.MoveNext();

                    string itemDefinitionUrl = DictionaryURL + "?cdrid=" + itemPtr.Current.ID;
                    Page.Response.Redirect(itemDefinitionUrl);
                }
                else
                {
                    resultListView.DataSource = resultCollection;
                    resultListView.DataBind();
                    NumResults = resultCollection.ResultsCount;
                    lblWord.Text = SearchStr.Replace("[[]", "[");
                    lblNumResults.Text = NumResults.ToString();
                    if (NumResults == 0)
                    {
                        RenderNoResults();
                    }

                }
            }
            else 
            {
                RenderNoResults();
            }
        }

        private void RenderNoResults()
        {
            //to display EmptyDataTemplate the ListView datasource needs to be set to null
            resultListView.DataSource = null;
            resultListView.DataBind();
            numResDiv.Visible = false;
        }

        //update the EmptyDataTemplate label text based on the Dictionary Language
        protected void GetNoDataMessage(object sender, EventArgs e)
        {
            Label lblNoDataMessage = sender as Label;

            if (lblNoDataMessage != null)
            {
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                    lblNoDataMessage.Text = "No se encontraron resultados para lo que usted busca. Revise si escribi&oacute; correctamente e inténtelo de nuevo. También puede escribir las primeras letras de la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la lista de términos que empiezan con esa letra.";
                else
                    lblNoDataMessage.Text = "No matches were found for the word or phrase you entered. Please check your spelling, and try searching again. You can also type the first few letters of your word or phrase, or click a letter in the alphabet and browse through the list of terms that begin with that letter.";
            }
        }
                
        /// <summary>
        /// Setup shared by English and Spanish versions
        /// </summary>
        private void SetupCommon()
        {           
            if (!string.IsNullOrEmpty(SrcGroup))
                BContains = Convert.ToBoolean(SrcGroup);

            if (!string.IsNullOrEmpty(Expand))
            {
                if (Expand.Trim() == "#")
                {
                    SearchStr = "[0-9]";
                }
                else
                {
                    SearchStr = Expand.Trim().ToUpper();
                }
            }
            
        }

        private void ValidateParams()
        {
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            if (!string.IsNullOrEmpty(CdrID))
            {
                try
                {
                    Int32.Parse(CdrID);
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid CDRID" + CdrID);

                }
            }
        }

        /// <summary>
        /// Saves the quesry parameters to support old gets
        /// </summary>
        private void GetQueryParams()
        {
            Expand = Strings.Clean(Request.Params["expand"]);
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            SearchStr = Strings.Clean(Request.Params["search"]);
            SrcGroup = Strings.Clean(Request.Params["contains"]);
        }

        protected string ResultListViewHrefOnclick(ListViewDataItem dataItem)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.TermsDictionaryResults(this,'" + (dataItem.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }

        protected void resultListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;

            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                DictionarySearchResult dictionaryResult = (DictionarySearchResult)dataItem.DataItem;

                if (dictionaryResult != null)
                {
                    PlaceHolder phPronunciation = (PlaceHolder)dataItem.FindControl("phPronunciation");
                    if (dictionaryResult.Term.HasPronunciation && phPronunciation != null)
                    {
                        phPronunciation.Visible = true;
                        HtmlAnchor pronunciationLink = (HtmlAnchor)dataItem.FindControl("pronunciationLink");
                        if (pronunciationLink != null && dictionaryResult.Term.Pronunciation.HasAudio)
                        {
                            pronunciationLink.Visible = true;
                            pronunciationLink.HRef = ConfigurationSettings.AppSettings["CDRAudioMediaLocation"] + "/" + dictionaryResult.Term.Pronunciation.Audio;
                        }
                        else
                            pronunciationLink.Visible = false;

                        Literal pronunciationKey = (Literal)dataItem.FindControl("pronunciationKey");

                        if (pronunciationKey != null && dictionaryResult.Term.Pronunciation.HasKey)
                            pronunciationKey.Text = " " + dictionaryResult.Term.Pronunciation.Key;

                    }
                    else
                        phPronunciation.Visible = false;
                }

            }

        }
        
    }

    
}