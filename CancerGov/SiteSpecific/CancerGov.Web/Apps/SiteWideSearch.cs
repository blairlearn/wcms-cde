﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Globalization;
using NCI.Util;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE.UI.WebControls;
using NCI.Web.UI.WebControls;

namespace NCI.Web.CancerGov.Apps
{
    public class SiteWideSearch : AppsBaseUserControl
    {
        #region Control Members
        protected DropDownList ddlPageUnit;
        protected Label lblResultsForText;
        protected Label lblResultsForKeyword;
        protected Repeater rptBestBets;
        protected Label lblBBCatName;
        protected Repeater rptBBListItems;
        protected Panel phDYM;
        protected Literal litDidYouMeanText;
        protected Panel phError;
        protected Label lblTopResultsXofYKeyword;
        protected HyperLink lnkDym;
        protected Label lblTopResultsXofY;
        protected Label lblTopResultsXofYKeyword;
        protected Literal litError;
        protected Repeater rptResults;
        protected Label lblBottomResultsXofY;
        protected Label lblDDLPageUnitShowText;
        protected Label lblPageUnit;
        protected Label lblSearchWithinResultsFound;
        protected Label lblSearchWithinResultKeyword;
        protected Panel pnlSWR;
        protected RadioButtonList rblSWRSearchType;
        protected Label lblSWRKeywordLabel;
        protected TextBox txtSWRKeyword;
        protected ImageButton btnSWRImgSearch;
        protected Button btnSWRTxtSearch;
        protected Label lblDDLPageUnitResultsPPText;
        protected Button btnTextChangePageUnit;
        protected JavascriptProbeControl jsProbe;
        protected SimplePager spPager;
        #endregion
        private bool _allowedToShowDYM = false;

        private string _topResultsXofYFormatter = "Results {0}-{1} of {2} for:";
        private string _bottomResultsXofYFormatter = "Results {0}-{1} of {2}.";
        private string _swrResultsFor = "{0} results found for: ";

        private bool _hasPageUnitChanged = false;
        private bool _isIENoJSAndHitEnderInTheSearchBox = false;
        private int _resultOffset = 1;


        #region Properties

        /// <summary>
        /// This is the keyword.
        /// </summary>
        private string Keyword
        {
            get
            {
                return (string)ViewState["Keyword"] ?? string.Empty;
            }
            set
            {
                ViewState["Keyword"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the ItemsPerPage. (The pageunit parameter)
        /// </summary>
        private int ItemsPerPage
        {
            get
            {
                return Strings.ToInt(ddlPageUnit.SelectedValue, 10);
            }
            set
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlPageUnit.Items)
                {
                    if (li.Value == value.ToString())
                    {
                        ddlPageUnit.Text = value.ToString();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets the current page number
        /// </summary>
        private int CurrentPage
        {
            get
            {
                return (int)(ViewState["CurrentPage"] ?? 1);
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }

        private long TotalNumberOfResults
        {
            get
            {
                return (long)(ViewState["TotalNumberOfResults"] ?? 0);
            }
            set
            {
                ViewState["TotalNumberOfResults"] = value;
            }
        }

        /// <summary>
        /// Gets and sets the PreviousItemsPerPage.  
        /// </summary>
        /// <remarks>
        /// This is to support the moving to the
        /// correct page when the ItemsPerPage changes.  A DropDownList does not store the 
        /// previous selected value, even if it is set with an autopostback.  So we need to store
        /// it after we get the search results in order to figure out what the position of the first 
        /// item was before the user changed the number of items.  
        /// </remarks>
        private int PreviousItemsPerPage
        {
            get
            {
                return (int)(ViewState["PreviousItemsPerPage"] ?? ItemsPerPage);
            }
            set
            {
                ViewState["PreviousItemsPerPage"] = value;
            }
        }


        /// <summary>
        /// Gets the text for the search term.
        /// </summary>
        private string SearchTerm
        {
            get
            {
                string rtnText = "";
                if (OldKeywords.Count > 0)
                {
                    rtnText = string.Join(" ", OldKeywords.ToArray());
                    rtnText += " " + Keyword;
                }
                else
                {
                    rtnText = Keyword;
                }

                return rtnText;
            }
        }

        /// <summary>
        /// Gets the text for the different keyword labels.
        /// </summary>
        private string KeywordText
        {
            get
            {
                string rtnText = "";
                if (OldKeywords.Count > 0)
                {
                    rtnText = string.Join(" : ", OldKeywords.ToArray());
                    rtnText += " : " + Keyword;
                }
                else
                {
                    rtnText = Keyword;
                }

                return rtnText;
            }
        }


        /// <summary>
        /// Gets a list of OldKeywords.
        /// </summary>
        private List<string> OldKeywords
        {
            get
            {
                if (ViewState["OldKeywords"] == null)
                    ViewState["OldKeywords"] = new List<string>();

                return (List<string>)ViewState["OldKeywords"];
            }
        }

        /// <summary>
        /// Gets the old keywords suitable for query parameters.
        /// </summary>
        private string OldKeywordsForQuery
        {
            get
            {
                string rtnText = "";

                if (OldKeywords.Count > 0)
                    rtnText = string.Join("|", OldKeywords.ToArray());

                return rtnText;
            }
        }

        #endregion

        /// <summary>
        /// Event method sets content version and template and user control properties<br/>
        /// <br/>
        /// [1] Input form parameters:<br/>
        ///			keyword {string: search string},<br/>
        ///			first {integer: ordinal index of first result},<br/>
        ///			resultSearch {string: search within results search string},<br/>
        ///			chkCategories {comma-delimited set of strings: specific virtual paths to search},<br/>
        ///			selectedPage {integer: current result page number}<br/>
        /// [2] Builds Verity and BestBet query language and Verity query xml document<br/>
        /// [3] Parses Verity results and errors<br/>
        /// [4] Builds Verity and BestBet result HTML<br/>
        /// [5] Builds paging control for results browsing<br/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Setup the title, header, and left nav
            SetupCancerGovPageStuff();

            SetLabels();

            //Hide the Did You Mean Section
            phDYM.Visible = false;

            //Hide the error message
            phError.Visible = false;

            //Setup if we are allowed to show DYM
            _allowedToShowDYM = Strings.ToBoolean((string)ConfigurationSettings.AppSettings["EndecaDidYouMean"]) && PageDisplayInformation.Language == DisplayLanguage.English;

            if (PageDisplayInformation.Version == DisplayVersion.Image)
            {
                btnSWRImgSearch.Visible = true;
                btnSWRTxtSearch.Visible = false;
            }
            else
            {
                btnSWRImgSearch.Visible = false;
                btnSWRTxtSearch.Visible = true;
            }

            if (Page.Request.RequestType == "POST")
            {
                if (!IsPostBack)
                {
                    /*********************************************/
                    /*        Search using header search box     */
                    /*********************************************/

                    Keyword = Request.Params["swKeyword"];

                    if (Keyword == string.Empty)
                    {
                        ShowErrorMessage();
                    }
                    else
                    {
                        LoadResults();

                        //Log User Input for initial search.
                        LogUserInput("Keyword:" + Keyword);
                    }
                }
                else
                {
                    //This is a postback from a user changing the items per page or
                    //searching within results.  There is one problem though.
                    //By default, 
                    if (Strings.Clean(Request.Params["__EVENTTARGET"]) == null
                        && Strings.Clean(Request.Params["__EVENTARGUMENT"]) == null
                        && Strings.Clean(Request.Params[btnTextChangePageUnit.UniqueID]) == null
                        && Strings.Clean(Request.Params[btnSWRTxtSearch.UniqueID]) == null
                        && Strings.Clean(Request.Params[btnSWRImgSearch.UniqueID + ".x"]) == null)
                    {
                        //THIS IS FOR IE with no JS.  Oddly in most ways it is more broken than firefox, but
                        //is so broken that we can easily tell that the user hit enter on the only
                        //text box.
                        //Basically if all the buttons that can be clicked are null, and there
                        //is no event target or argument, then this is IE with Javascript turned
                        //off and the user clicked enter on the text box.  Also, image buttons do
                        //not come back ID, but ID.x and ID.y. (Hopefully screen readers send those
                        //things too.
                        //
                        //We need to handle this stuff after all the other postback events happen.
                        //so lets just set a variable for now.
                        _isIENoJSAndHitEnderInTheSearchBox = true;
                    }
                }
            }
            else
            {
                /*********************************************/
                /*    Either Paging or a DYM Click           */
                /*********************************************/

                //No Matter if this is a DYM or a Paging, swKeyword is the keyword. (There may be an old one, but that is for later)
                Keyword = Request.Params["swKeyword"];

                if (Keyword == string.Empty)
                {
                    ShowErrorMessage();
                }
                else
                {

                    //Lets us know that this was a DYM click.
                    bool isDym = Strings.ToBoolean(Request.Params["dym"]);

                    if (isDym)
                    {
                        //Log the keyword because this is a new search.  I figure it is better if we mark it as a DYM.
                        LogUserInput("DYM-Keyword:" + Keyword);
                    }
                    else
                    {
                        //These things only apply to paging

                        //Get Items per page.
                        ItemsPerPage = Strings.ToInt(Request.Params["pageunit"], 10);

                        //Get the current Page
                        CurrentPage = Strings.ToInt(Request.Params["page"], 1);

                        //"De-serialize" old keywords.
                        if (!string.IsNullOrEmpty(Request.Params["old_keywords"]))
                        {
                            string[] oldKeys = Request.Params["old_keywords"].Split(new char[] { '|' });
                            OldKeywords.AddRange(oldKeys);
                        }
                    }

                    LoadResults();
                }
            }

            // build the OnClineClick value for results search
            var siteResultSearchSubmitParameter = "";
            if (PageDisplayInformation.Language == DisplayLanguage.Spanish)
                siteResultSearchSubmitParameter = "true";
            var siteResultSearchSubmitCall = "return siteResultSearchSubmit(" + siteResultSearchSubmitParameter + ")";
            // Web Analytics *************************************************
            if (WebAnalyticsOptions.IsEnabled)
                siteResultSearchSubmitCall += "&& NCIAnalytics.SiteWideSearchResultsSearch(this,'" + txtSWRKeyword.ClientID + "','" + rblSWRSearchType.ClientID + "')";
            // End Web Analytics *********************************************
            siteResultSearchSubmitCall += ";";
            btnSWRImgSearch.OnClientClick = siteResultSearchSubmitCall;
            btnSWRTxtSearch.OnClientClick = siteResultSearchSubmitCall;

        }

        protected override void OnPreRender(EventArgs e)
        {
            //Handle the stupid IE No JS problem.  Really we just need to simulate the go
            //button click.
            if (_isIENoJSAndHitEnderInTheSearchBox)
                ChangePageUnitBtnClick(btnTextChangePageUnit, new EventArgs());


            base.OnPreRender(e);

            if (PageDisplayInformation.Version == DisplayVersion.Image)
                pnlSWR.DefaultButton = btnSWRImgSearch.ID;
            else
                pnlSWR.DefaultButton = btnSWRTxtSearch.ID;
        }

        protected string ResultsHyperlinkOnclick(RepeaterItem result, bool isBestBet)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "NCIAnalytics.SiteWideSearchResults(this," + (isBestBet ? "true" : "false") + ",'" + (result.ItemIndex + _resultOffset).ToString() + "');"; // Load results onclick script
            else
                return "";
        }


        #region Event Handlers

        /// <summary>
        /// This is the callback for the Go button next to the page unit dropdown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePageUnitBtnClick(object sender, EventArgs e)
        {
            //If we are here, then there is no Javascript.  This can happen because the user is in firefox and
            //hit enter in the textbox, or they are in either IE or Firefox and clicked the "Go" button.
            //Now if the textbox has text then we need to do a new search, if it is empty then the ChangePageUnit
            //handler has already done its work.  If the user changed the page size then the search function will
            //use the new page size.

            if (Strings.Clean(txtSWRKeyword.Text) != null)
            {
                //This will also take the new page unit size if the user changed it.
                DoSearchWithinResults();
            }
            else if (!_hasPageUnitChanged)
            {
                if (Keyword != string.Empty)
                {
                    //The button was clicked and nothing was changed.  So just rebind.
                    LoadResults();
                }
                else
                {
                    ShowErrorMessage();
                }
            }
        }

        /// <summary>
        /// This is the callback for the page size dropdown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePageUnit(object sender, EventArgs e)
        {
            //This handler fires whenever the page unit dropdown is changed.  With Javascript enabled, it
            //will always postback to the server.  So there is NO way that a user can change the dropdown 
            //and then submit the form with the search button.  So we throw away the text of the
            //txtSWRKeyword textbox since they did not click the search button.

            //With Javascript disabled, you can change the number of items and then click the search within
            //results button, click the go button, or etc... AND THIS WILL FIRE.  So without JS this handler is
            //ALWAYS called if the selected index changes.  Since pressing enter while in the search box in
            //firefox causes the go button to be clicked and not the search within results button to be 
            //clicked we decided that when JS is disabled and there is text in the search box, then whatever
            //caused the form to be posted back will do a new search with the new keyword.

            //The only real purpose of this handler is to change the current page so that the user
            //is not taken to page that still shows the records they were looking at before.  So
            //if the user were to input a new keyword to search for, they must go back to page 1.  In this 
            //case the code is meaningless and therefore we should do nothing.  We should instead let the
            //Go button's onclick handler to do the work.

            _hasPageUnitChanged = true; //This fired so it must have changed.

            if ((jsProbe.HasJavascript) || (!jsProbe.HasJavascript && Strings.Clean(txtSWRKeyword.Text) == null))
            {
                //Just to be consistant about things, if a user typed something in the search within results text box then they
                //changed the page size, then the text will still show, so lets clear it out.
                txtSWRKeyword.Text = "";

                if (Keyword == string.Empty)
                {
                    ShowErrorMessage();
                    return;
                }
                else
                {
                    ChangeItemsPerPageAndBind();
                }
            }
        }

        private void ChangeItemsPerPageAndBind()
        {
            //Get the last total number of results so if the user changes the ItemsPerPage(pageunit)
            //then we can move them to the closest page.  Say you are viewing 10 items per page and
            //you are on page 6. This shows 51-60.  If you change to 50 items per page you should put
            //the user on page 2 and not page 1.  That way they will see 51-100.

            //So we should know the current page number, it was part of the postback.
            //we do have to know what the old page unit was, that will give us the first record.
            int firstItem, lastItem;
            SimplePager.GetFirstItemLastItem(CurrentPage, PreviousItemsPerPage, (int)TotalNumberOfResults, out firstItem, out lastItem);

            //Set the current page.                
            CurrentPage = firstItem / ItemsPerPage + (firstItem % ItemsPerPage > 0 ? 1 : 0);

            //If the CurrentPage is 0, then Endeca will throw errors, so we need to set a curretnpage ==0
            //to 1.  This brings up a very good point, why do we show the change number of items per page
            //if we have 0 results.  One might also ask, why do we go to Endeca to bind the results when we 
            //know they will not change? The answer is partially due to lazyness and making the code simple.
            //LoadResults not only loads the results, but also makes sure that things like total results and
            //previousitemsperpage are set.  I would rather just call this and not add yet another code 
            //branch to this already complicated code.
            if (CurrentPage == 0)
                CurrentPage = 1;

            //Note LoadResults gets the page unit on its own.
            LoadResults();
        }

        /// <summary>
        /// This is the callback for the search within results button's onclick event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchWithinResults(object sender, EventArgs e)
        {
            //If JS is turned off, someone can say they want to see 50 items, and then do a search instead of hitting the go button to select the items
            //per page.  This would show as a bug in QA, although it might not be a bad things since you can do 2 things at once...
            if (PreviousItemsPerPage != ItemsPerPage)
                ItemsPerPage = PreviousItemsPerPage;

            DoSearchWithinResults();
        }

        #endregion

        /// <summary>
        /// Logs the search term a users searched with.
        /// </summary>
        /// <param name="clickItem"></param>
        private void LogUserInput(string clickItem)
        {
            //CancerGov.Common.Functions.LogUserInput(
            //    Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString(),
            //    Request.Url.AbsoluteUri,
            //    Request.UserHostAddress,
            //    clickItem,
            //    Page.Session.SessionID);
        }

        /// <summary>
        /// This sets up things like the left nav and title...
        /// </summary>
        private void SetupCancerGovPageStuff()
        {
            this.Page.Header.Title = GetResource("Title");
            //                this.PageHeaders.Add(new TitleBlock("Resultados ", null, this.PageDisplayInformation));
            //                this.PageLeftColumn = new LeftNavColumn(this, Strings.ToGuid(ConfigurationSettings.AppSettings["SpanishSearchViewID"]));
        }

        /// <summary>
        /// Sets up the spanish labels
        /// </summary>
        private void SetLabels()
        {
            //Set formatters
            this._topResultsXofYFormatter = GetResource("topResultsXofYFormatter"); // "Resultados {0}-{1} de {2} para:";
            this._bottomResultsXofYFormatter = GetResource("bottomResultsXofYFormatter"); // "Resultados {0}-{1} de {2}.";
            this._swrResultsFor = GetResource("ResultsFor");// "{0} resultados de b&uacute;squeda para: ";

            //Set controls.
            lblResultsForText.Text = GetResource("ResultsForText"); //"Resultados para:";
            litDidYouMeanText.Text = GetResource("DidYouMeanText"); //"Quiz&aacute;s quiso decir";
            btnTextChangePageUnit.Text = GetResource("TextChangePageUnit"); //"Sí";

            //Pager previous and next
            spPager.PagerStyleSettings.NextPageText = GetResource("NextPageText");// "Siguiente&nbsp;&gt;";
            spPager.PagerStyleSettings.PrevPageText = GetResource("PrevPageText");//"&lt;&nbsp;Anterior";

            //Search within results Radio button list.
            if (rblSWRSearchType.Items.Count == 2)
            {
                rblSWRSearchType.Items[0].Text = GetResource("SearchTypeText1"); //"Nueva b&uacute;squeda&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                rblSWRSearchType.Items[1].Text = GetResource("SearchTypeText1"); // "B&uacute;squeda dentro de los resultados";
            }

            //Search within results button
            btnSWRImgSearch.AlternateText = GetResource("AlternateText");// "Buscar";
            btnSWRImgSearch.ImageUrl = GetResource("imageURL"); //"/images/buscar-left-nav.gif";

            btnSWRTxtSearch.Text = GetResource("Search");  //"Buscar";

            //Error message
            litError.Text = GetResource("ErrorMessage"); //"Se necesita término de búsqueda.";

            //Page unit dropdown text and label
            lblDDLPageUnitShowText.Text = GetResource("PageUnit"); //"Mostrar&nbsp;";
            lblDDLPageUnitResultsPPText.Text = GetResource("PageUnitResults"); //"&nbsp;resultados por p&aacute;gina.";
        }

        private void DoSearchWithinResults()
        {
            if (Strings.Clean(txtSWRKeyword.Text) == null)
            {
                ShowErrorMessage();
            }
            else
            {
                if (rblSWRSearchType.SelectedValue == "2") //Search Within Results
                {
                    //Add the last keyword to the old keywords
                    OldKeywords.Add(Keyword);

                    //Set the current keyword
                    Keyword = Strings.Clean(txtSWRKeyword.Text);

                    LogUserInput("Refine-Keyword:" + (
                        OldKeywordsForQuery.Length > 0 ? OldKeywordsForQuery + "|" + Keyword : Keyword));
                }
                else //New Search
                {
                    //Log new search
                    OldKeywords.Clear();
                    Keyword = Strings.Clean(txtSWRKeyword.Text);

                    LogUserInput("New-Keyword:" + Keyword);
                }

                this.CurrentPage = 1;
                txtSWRKeyword.Text = ""; //Clear the text box.  .NET will leave in the text, which is what google does, but not what cancer.gov has done.

                LoadResults();
            }
        }

        /// <summary>
        /// This method is the one that sets up label text and binds the search results.
        /// </summary>
        private void LoadResults()
        {

            //Get Bestbets. Only show if we are on page one and we are not doing a search within
            //the results.
            if (CurrentPage == 1 && OldKeywords.Count == 0)
            {
                BestBetsResults bbResults = GetBestBetsResults(SearchTerm);

                if (bbResults.Count > 0)
                {
                    rptBestBets.DataSource = bbResults;
                    rptBestBets.DataBind();
                    rptBestBets.Visible = true;
                }
                else
                {
                    rptBestBets.Visible = false;
                }
            }
            else
            {
                rptBestBets.Visible = false;
            }

            //Get Results...            
            SiteWideSearchResults results = GetSearchResults(SearchTerm);

            //Setup the Did You Mean if needed.
            if (_allowedToShowDYM && !string.IsNullOrEmpty(results.DidYouMeanText) && CurrentPage == 1)
                ShowDYMText(results.DidYouMeanText);

            //Set the last total number of results so if the user changes the ItemsPerPage(pageunit)
            //then we can move them to the closest page.  Say you are viewing 10 items per page and
            //you are on page 6. This shows 51-60.  If you change to 50 items per page you should put
            //the user on page 2 and not page 1.  That way they will see 51-100.  
            TotalNumberOfResults = results.TotalNumResults;
            PreviousItemsPerPage = ItemsPerPage;

            int firstIndex, lastIndex;

            //Get first index and last index
            SimplePager.GetFirstItemLastItem(CurrentPage, ItemsPerPage, (int)results.TotalNumResults, out firstIndex, out lastIndex);
            _resultOffset = firstIndex;

            rptResults.DataSource = results;
            rptResults.DataBind();

            //Set Keywords in labels
            lblResultsForKeyword.Text = KeywordText;
            lblTopResultsXofYKeyword.Text = KeywordText;
            lblSearchWithinResultKeyword.Text = KeywordText;

            //Show labels for results X of Y
            ShowResultsXoYLabels(firstIndex, lastIndex, results.TotalNumResults);

            //Setup pager
            SetupPager();

            //Set the Search Within Results Radio button to new search
            rblSWRSearchType.SelectedIndex = 0;


            // Web Analytics *************************************************
            this.WebAnalyticsPageLoad.AddEvar(WebAnalyticsOptions.eVars.NumberOfSearchResults, TotalNumberOfResults.ToString()); // eVar10
            this.WebAnalyticsPageLoad.SetChannel("NCI Home");
            if (rptBestBets.Visible)
                this.WebAnalyticsPageLoad.AddEvent(WebAnalyticsOptions.Events.BestBets); //Best Bets are offered on search results (event10) 
            litOmniturePageLoad.Text = this.WebAnalyticsPageLoad.Tag();
            // End Web Analytics *********************************************

        }

        private void ShowErrorMessage()
        {
            //Clear out the fact that the user did a search before this.
            Keyword = "";
            OldKeywords.Clear();

            lblResultsForKeyword.Text = "";
            lblTopResultsXofYKeyword.Text = "";
            lblSearchWithinResultKeyword.Text = "";

            phError.Visible = true;
            ShowResultsXoYLabels(0, 0, 0);
            //For paging change.
            PreviousItemsPerPage = ItemsPerPage;

            //Set the Search Within Results Radio button to new search
            rblSWRSearchType.SelectedIndex = 0;
            spPager.RecordCount = 0;

            // Web Analytics *************************************************
            this.WebAnalyticsPageLoad.AddEvar(WebAnalyticsOptions.eVars.NumberOfSearchResults, "0"); // eVar10
            litOmniturePageLoad.Text = this.WebAnalyticsPageLoad.Tag();
            // End Web Analytics *********************************************


        }

        private void ShowResultsXoYLabels(int firstIndex, int lastIndex, long totalNumResults)
        {
            //This is the top Results X-Y of Z for: label
            lblTopResultsXofY.Text = String.Format(
                _topResultsXofYFormatter,
                firstIndex,
                lastIndex,
                totalNumResults);

            //This is the bottom Results X-Y of Z. label
            lblBottomResultsXofY.Text = String.Format(
                _bottomResultsXofYFormatter,
                firstIndex,
                lastIndex,
                totalNumResults);

            //This is the Z results found for: Label in the search within the results box.
            lblSearchWithinResultsFound.Text = String.Format(
                _swrResultsFor,
                totalNumResults);
        }

        /// <summary>
        /// Helper method to setup the pager
        /// </summary>
        private void SetupPager()
        {
            spPager.RecordCount = (int)this.TotalNumberOfResults;
            spPager.RecordsPerPage = ItemsPerPage;
            spPager.RecordsPerPageParamName = "pageunit";
            spPager.CurrentPage = CurrentPage;
            spPager.PageParamName = "page";
            spPager.ShowNumPages = 2;
            spPager.PagerStyleSettings.SelectedIndexCssClass = "pager-SelectedPage";

            //"Serialize" OldKeywords to string
            if (OldKeywords.Count > 0)
            {
                spPager.BaseUrl = string.Format(
                    "{0}?swKeyword={1}&old_keywords={2}",
                    Request.Url.AbsolutePath,
                    Server.UrlEncode(Keyword),
                    Server.UrlEncode(OldKeywordsForQuery));
            }
            else
            {
                spPager.BaseUrl = string.Format(
                    "{0}?swKeyword={1}",
                    Request.Url.AbsolutePath,
                    Server.UrlEncode(Keyword));
            }

            if (PageDisplayInformation.Language == DisplayLanguage.Spanish)
                spPager.BaseUrl += "&lang=spanish";
        }

        /// <summary>
        /// Helper method to setup the DYM stuff
        /// </summary>
        /// <param name="dymText"></param>
        private void ShowDYMText(string dymText)
        {
            phDYM.Visible = true;
            lnkDym.Text = dymText;
            lnkDym.NavigateUrl = string.Format(
                "{0}?swKeyword={1}&pageunit={2}&dym=1",
                Request.Url.AbsolutePath,
                Server.UrlEncode(dymText),
                ItemsPerPage);
        }

        /// <summary>
        /// Method to get best bets
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private BestBetsResults GetBestBetsResults(string searchTerm)
        {
            BestBetsResults results = null;

            try
            {
                results = BestBetsManager.GetBestBets(searchTerm, PageDisplayInformation.Language);
            }
            catch (Exception ex)
            {
                CancerGovError.LogError(Request.Url.AbsoluteUri, this.ToString(), ErrorType.EndecaError, ex);
                this.RaiseErrorPage();
            }

            return results;
        }

        /// <summary>
        /// Method to get search results.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private SiteWideSearchResults GetSearchResults(string searchTerm)
        {
            SiteWideSearchResults results = null;

            try
            {
                results = SiteWideSearchManager.GetSiteWideSearchResults(
                    searchTerm,
                    ItemsPerPage,
                    (CurrentPage - 1) * ItemsPerPage,
                    PageDisplayInformation.Language);
            }
            catch (Exception ex)
            {
                CancerGovError.LogError(Request.Url.AbsoluteUri, this.ToString(), ErrorType.EndecaError, (ex.Message + "\nEndeca Search ERROR\nQuery:\n\n" + searchTerm));
                this.RaiseErrorPage();
            }

            return results;
        }

    }
}
