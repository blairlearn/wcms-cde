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
    public class GeneticsTermDictionaryResultsList : BaseDictionaryControl
    {
        protected GeneticsTermDictionaryHome dictionarySearchBlock;

        protected Panel numResDiv;

        protected Label lblNumResults;

        protected Label lblResultsFor;

        protected Label lblWord;

        protected ListView resultListView;

        public string SearchStr { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public int NumResults { get; set; }

        public string DictionaryURL { get; set; }

        public String DictionaryLanguage { get; set; }

        public string DictionaryPrettyURL { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();
            DictionaryPrettyURL = this.PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString();

            GetQueryParams();
            SetDoNotIndex();

            //For Genetics dictionary language is always English
            DictionaryLanguage = "en";

            DictionarySearchResultCollection resultCollection = null;
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

            if (!string.IsNullOrEmpty(SrcGroup))
                BContains = Convert.ToBoolean(SrcGroup);

            SearchType searchType = SearchType.Begins;
            if (BContains)
                searchType = SearchType.Contains;

            if (!String.IsNullOrEmpty(SearchStr)) // SearchString provided, do a term search
            {
                SearchStr = Sanitizer.GetSafeHtmlFragment(SearchStr);
                resultCollection = _dictionaryAppManager.Search(SearchStr, searchType, 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.genetic, DictionaryLanguage);

            }

            if (resultCollection != null && resultCollection.Count() > 0)
            {
                //if there is only 1 record - go directly to definition view
                if (resultCollection.ResultsCount == 1)
                {
                    // Get the first (only) item so we can redirect to it specifically
                    IEnumerator<DictionarySearchResult> itemPtr = resultCollection.GetEnumerator();
                    itemPtr.MoveNext();

                    string urlItem = GetFriendlyName(itemPtr.Current.ID);

                    string itemDefinitionUrl = DictionaryPrettyURL + "/def/" + urlItem;
                    Page.Response.Redirect(itemDefinitionUrl);
                }
                else
                {
                    resultListView.DataSource = resultCollection;
                    resultListView.DataBind();
                    NumResults = resultCollection.ResultsCount;
                    if (!string.IsNullOrEmpty(SearchStr))
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
            resultListView.DataSource = new DictionarySearchResultCollection(new DictionarySearchResult[0]);
            resultListView.DataBind();
            numResDiv.Visible = false;
        }

        private void SetDoNotIndex()
        {
            PageInstruction.AddFieldFilter("meta_robots", (name, data) =>
            {
                data.Value = "noindex, nofollow";
            });
        }

        /// <summary>
        /// Saves the quesry parameters to support old gets
        /// </summary>
        private void GetQueryParams()
        {
            SearchStr = Sanitizer.GetSafeHtmlFragment(Request.Params["q"]);
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