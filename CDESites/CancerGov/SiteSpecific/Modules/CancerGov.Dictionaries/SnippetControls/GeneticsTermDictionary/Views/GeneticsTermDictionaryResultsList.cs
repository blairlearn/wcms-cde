using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using NCI.Util;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using Microsoft.Security.Application;

namespace CancerGov.Dictionaries.SnippetControls.GeneticsTermDictionary
{
    public class GenerticsTermDictionaryResultsList : SnippetControl
    {
        protected global::CancerGov.Dictionaries.SnippetControls.GeneticsTermDictionary.GeneticsTermDictionaryHome dictionarySearchBlock;

        protected global::System.Web.UI.WebControls.Panel numResDiv;

        protected global::System.Web.UI.WebControls.Label lblNumResults;

        protected global::System.Web.UI.WebControls.Label lblResultsFor;

        protected global::System.Web.UI.WebControls.Label lblWord;

        protected global::System.Web.UI.WebControls.ListView resultListView;

        public string SearchStr { get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public int NumResults { get; set; }

        public string DictionaryURL { get; set; }

        public String DictionaryLanguage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            //base.OnLoad(e);
            GetQueryParams();
            ValidateParams();

            //For Genetics dictionary language is always English
            DictionaryLanguage = "en";

            DictionarySearchResultCollection resultCollection = null;
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

            if (!string.IsNullOrEmpty(SrcGroup))
                BContains = Convert.ToBoolean(SrcGroup);

            if (!string.IsNullOrEmpty(Expand) && string.IsNullOrEmpty(SearchStr))
            {
                if (Expand.Trim() == "#")
                {
                    SearchStr = "[0-9]";
                }
                else
                {
                    if (!Expand.Trim().ToUpper().Equals("ALL"))
                        SearchStr = Expand.Trim().ToUpper();
                }
            }


            SearchType searchType = SearchType.Begins;
            if (BContains)
                searchType = SearchType.Contains;



            if (!String.IsNullOrEmpty(SearchStr)) // SearchString provided, do a term search
            {
                SearchStr = Sanitizer.GetSafeHtmlFragment(SearchStr);
                resultCollection = _dictionaryAppManager.Search(SearchStr, searchType, 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.genetic, DictionaryLanguage);

            }
            else if (!String.IsNullOrEmpty(Expand)) // A-Z expand provided - do an A-Z search
            {

                if (Expand.ToLower() == "all")
                    resultCollection = _dictionaryAppManager.Search("%", searchType, 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.genetic, DictionaryLanguage);
                else
                    resultCollection = _dictionaryAppManager.Expand(Expand, "", 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.genetic, DictionaryLanguage, "v1");
            }

            if (resultCollection != null && resultCollection.Count() > 0)
            {
                //if there is only 1 record - go directly to definition view
                //check if expand equals "A", as this is the default value
                if ((resultCollection.ResultsCount == 1) && (Expand == "A"))
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
                    if (!string.IsNullOrEmpty(SearchStr))
                        lblWord.Text = SearchStr.Replace("[[]", "[");
                    else if (!string.IsNullOrEmpty(Expand))
                        lblWord.Text = Expand;
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
            resultListView.DataSource = new DictionarySearchResultCollection(new DictionarySearchResult[0]);
            resultListView.DataBind();
            numResDiv.Visible = false;
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
            Expand = Strings.Clean(Request.Params["expand"], "A");
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            SearchStr = Sanitizer.GetSafeHtmlFragment(Request.Params["search"]);
            SearchStr = Strings.Clean(SearchStr);
            SrcGroup = Strings.Clean(Request.Params["contains"]);
        }


        protected string ResultListViewHrefOnclick(ListViewDataItem dataItemList)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.GeneticsDictionaryResults(this,'" + (dataItemList.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
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