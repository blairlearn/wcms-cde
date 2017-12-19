using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using NCI.Util;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using Microsoft.Security.Application;

namespace CancerGov.Dictionaries.SnippetControls.TermDictionary
{
    public class TermDictionaryExpandList : BaseDictionaryControl
    {
        protected TermDictionaryHome dictionarySearchBlock;

        protected Panel numResDiv;

        protected Label lblNumResults;

        protected Label lblResultsFor;

        protected Label lblWord;

        protected ListView resultListView;

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public bool isSearch { get; set; }

        public int NumResults { get; set; }

        public string DictionaryURL { get; set; }

        public string DictionaryPrettyURL { get; set; }

        public string DictionaryURLSpanish { get; set; }

        public string DictionaryURLEnglish { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            DictionaryPrettyURL = this.PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString();

            GetQueryParams();

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

            DictionaryURLSpanish = DictionaryURL;
            DictionaryURLEnglish = DictionaryURL;
            SetupCanonicalUrls(DictionaryURLEnglish, DictionaryURLSpanish);
        }

        private void LoadData()
        {
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

            SearchType searchType = SearchType.Begins;
            if (BContains)
                searchType = SearchType.Contains;

            DictionarySearchResultCollection resultCollection = null;

            if (!String.IsNullOrEmpty(Expand)) // A-Z expand provided - do an A-Z search
            {

                if (Expand.ToLower() == "all")
                    resultCollection = _dictionaryAppManager.Expand("%", "", 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.term, PageAssemblyContext.Current.PageAssemblyInstruction.Language, "v1");
                else
                    resultCollection = _dictionaryAppManager.Expand(Expand, "", 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.term, PageAssemblyContext.Current.PageAssemblyInstruction.Language, "v1");
            }

            if (resultCollection != null && resultCollection.Count() > 0)
            {
                resultListView.DataSource = resultCollection;
                resultListView.DataBind();
                NumResults = resultCollection.ResultsCount;
                lblWord.Text = Expand.Replace("[[]", "[");
                lblNumResults.Text = NumResults.ToString();
                if (NumResults == 0)
                {
                    RenderNoResults();
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
            resultListView.DataSource = new DictionarySearchResultCollection(new DictionarySearchResult[0]);
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
                    Expand = "[0-9]";
                }
                else
                {
                    Expand = Expand.Trim().ToUpper();
                }
            }
        }

        //Add a filter for the Canonical URL.
        private void SetupCanonicalUrls(string englishDurl, string spanishDurl)
        {
            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, SetupUrlFilter);

            foreach (var lang in PageAssemblyContext.Current.PageAssemblyInstruction.TranslationKeys)
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddTranslationFilter(lang, SetupUrlFilter);
            }
        }

        private void SetupUrlFilter(string name, NciUrl url)
        {
            url.SetUrl(url.ToString());

            url.QueryParameters.Add("expand", Expand);
        }

        /// <summary>
        /// Saves the query parameters
        /// </summary>
        private void GetQueryParams()
        {
            Expand = Strings.Clean(Request.Params["expand"], "A");
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
                            pronunciationLink.HRef = dictionaryResult.Term.Pronunciation.Audio;
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