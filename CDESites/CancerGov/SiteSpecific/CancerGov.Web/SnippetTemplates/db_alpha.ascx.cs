using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using NCI.Text;
using NCI.Web.CDE.UI;
using CancerGov.Text;
using NCI.Web.CDE;
using Www.Common.UserControls;
using NCI.Web.CDE.WebAnalytics;
using CancerGov.CDR.TermDictionary;
using CancerGov.UI;
using NCI.Web.UI.WebControls.FormControls;
using NCI.Web.CDE.Modules;
namespace Www.Templates
{
    public partial class DbAlpha : SnippetControl
    {
        protected AlphaListBox alphaListBox;
        //private TitleBlock _titleBl;

        //These are the QueryString related variables
        private string _searchStr = string.Empty;
        private string _expand = string.Empty;
        private string _cdrid = string.Empty;
        private string _srcGroup = string.Empty;
        private bool _isSpanish = false;
        private string _queryStringLanguage = string.Empty;
        private string _pagePrintUrl = string.Empty;
        private string _pageOptionsBoxTitle = string.Empty;
        private string _prevText = string.Empty;
        private string _nextText = string.Empty;
        private bool _bContains = false;
        private int _numResults = 0;
        private string _dictionaryURL = string.Empty;
        private string _dictionaryURLSpanish = string.Empty;
        private string _dictionaryURLEnglish = string.Empty;
        private int _totalCount = 0;

        #region Page properties

        #region QueryString Props
        //QueryString Props
        public string SearchStr
        {
            get { return _searchStr; }
            set
            {
                if (value != null)
                    _searchStr = value.Trim();
            }
        }
        public string Expand
        {
            get { return _expand; }
            set { if (value != null) _expand = value; }
        }
        public string CdrID
        {
            get { return _cdrid; }
            set { if (value != null) _cdrid = value; }
        }
        public string SrcGroup
        {
            get { return _srcGroup; }
            set { if (value != null) _srcGroup = value; }
        }
        #endregion

        public string QueryStringLang
        {
            get { return _queryStringLanguage; }
            set { _queryStringLanguage = value; }
        }
        public string PagePrintUrl
        {
            get { return _pagePrintUrl; }
            set { _pagePrintUrl = value; }
        }
        public string PageOptionsBoxTitle
        {
            get { return _pageOptionsBoxTitle; }
            set { _pageOptionsBoxTitle = value; }
        }
        public string PrevText
        {
            get { return _prevText; }
            set { _prevText = value; }
        }
        public string NextText
        {
            get { return _nextText; }
            set { _nextText = value; }
        }
        public bool BContains
        {
            get { return _bContains; }
            set { _bContains = value; }
        }
        public int NumResults
        {
            get { return _numResults; }
            set { _numResults = value; }
        }

        public string DictionaryURL
        {
            get { return _dictionaryURL; }
            set { _dictionaryURL = value; }
        }
        public string DictionaryURLSpanish
        {
            get { return _dictionaryURLSpanish; }
            set { _dictionaryURLSpanish = value; }
        }
        public string DictionaryURLEnglish
        {
            get { return _dictionaryURLEnglish; }
            set { _dictionaryURLEnglish = value; }
        }
        public string TotalCount
        {
            get { return _totalCount.ToString("N0"); }
        }


        #endregion
              private void ResetControls()
        {
              
            radioContains.Checked = BContains;
            AutoComplete1.Text = (string.IsNullOrEmpty(Expand)) ? SearchStr.Replace("[[]", "[") : string.Empty;
            AutoComplete1.SearchCriteria = (BContains) ? AutoComplete.SearchCriteriaEnum.Contains : AutoComplete.SearchCriteriaEnum.BeginsWith;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ValidateParams();
            GetQueryParams();

            //DictionaryURLSpanish = PageAssemblyContext.Current.requestedUrl.ToString().ToLower().Replace("dictionary", "diccionario"); //ConfigurationSettings.AppSettings["DictionaryOfCancerTermsURLSpanish"];
            //DictionaryURLEnglish = PageAssemblyContext.Current.requestedUrl.ToString().ToLower().Replace("diccionario", "dictionary"); //ConfigurationSettings.AppSettings["DictionaryOfCancerTermsURLEnglish"];
            
            //Setup URLS
            string snippetXmlData = string.Empty;
            snippetXmlData = SnippetInfo.Data;
            snippetXmlData = snippetXmlData.Replace("]]ENDCDATA", "]]>");
            NCI.Web.CDE.Modules.DictionaryURL dUrl = ModuleObjectFactory<NCI.Web.CDE.Modules.DictionaryURL>.GetModuleObject(snippetXmlData);
            
            DictionaryURLSpanish = dUrl.DictionarySpanishURL;
            DictionaryURLEnglish = dUrl.DictionaryEnglishURL;


            DictionaryURL = DictionaryURLEnglish;

            if (Request.RawUrl.ToLower().Contains("dictionary") && Request.RawUrl.ToLower().Contains("spanish"))
            {
                Response.Redirect("/diccionario" + Request.Url.Query);
                
            }

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language != "en")
                DictionaryURL = DictionaryURLSpanish;

            //Set display props according to lang
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language != "en")
            {
                SetupSpanish();
            }
            else
            {
                SetupEnglish();
            }


            //Action must be set this way and not by adding the location to the action 
            //attribute of the form tag or it will generate Validation of viewstate MAC failed error
            //because we have virtual directories and don't know the location 

            if (Page.Request.Path.StartsWith("/templates/", StringComparison.OrdinalIgnoreCase))
            {

                Page.Form.Action = Page.Request.Path;

                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language != "en")
                    Page.Form.Action += "?lang=spanish";
            }
            else
            {
                Page.Form.Action = DictionaryURL;
            }

            alphaListBox.BaseUrl = DictionaryURL;

            //Set is IE property to determine if browser is IE 
            AutoComplete1.IsIE = (Request.Browser.Browser.ToUpper() == "IE" ? true : false);

            if (Page.Request.RequestType.Equals("POST")) //This is a POST(back)
            {
                
                SearchStr = Request.Form["AutoComplete1"];
                SearchStr = SearchStr.Replace("[", "[[]");
                CdrID = string.Empty;
                Expand = string.Empty;
                
                RadioButton rd = (RadioButton)FindControl("radioContains");

                if(rd.Checked==true)                
                    BContains = true;

                if (string.IsNullOrEmpty(SearchStr))
                {
                    ActivateDefaultView();
                }
                    
                else
                {
                    LoadData();
                }
            }
            else // This is a GET
            {
                if ((string.IsNullOrEmpty(CdrID)) && (string.IsNullOrEmpty(Expand)) && (string.IsNullOrEmpty(SearchStr)))
                {
                    ActivateDefaultView();
                }
                else
                { LoadData(); }
            }

            SetupPrintUrl();

            lblNumResults.Text = NumResults.ToString();
            lblWord.Text = SearchStr.Replace("[[]", "[");

            ResetControls();


            if (WebAnalyticsOptions.IsEnabled)
            {
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.PageName, wbField =>
                {
                string suffix = "";
                if (Expand != "")
                    suffix = " - AlphaNumericBrowse";
                else if(CdrID != "") 
                    suffix = " - Definition";
                    wbField.Value = ConfigurationSettings.AppSettings["HostName"] + PageAssemblyContext.Current.requestedUrl.ToString() + suffix;
                });

                Page.Form.Attributes.Add("onsubmit", "NCIAnalytics.TermsDictionarySearch(this," + _isSpanish.ToString().ToLower() + ");"); // Load from onsubit script
                if (_isSpanish)
                    alphaListBox.WebAnalyticsFunction = "NCIAnalytics.TermsDictionarySearchAlphaListSpanish"; // Load A-Z list onclick script
                else
                    alphaListBox.WebAnalyticsFunction = "NCIAnalytics.TermsDictionarySearchAlphaList"; // Load A-Z list onclick script




            }

            BackTopLink();


        }

        protected void BackTopLink()
        {
            //		RawUrl	"/drugdictionary?CdrID=42766"	string

            if (Request.RawUrl.Contains("?") == false && NumResults<1)
            {

                litBackToTop.Visible = false;
            } 
            else if (Request.RawUrl.ToLower().Contains("?cdrid") == true)
            {
                litBackToTop.Visible = false;
            }
            else
            {
                litBackToTop.Visible = true;
                litBackToTop.Text = "<a href=\"#top\" class=\"backtotop-link\"><img src=\"/images/backtotop_red.gif\" alt=\"Back to Top\" border=\"0\">Back to Top</a>";

            }
        }
        private void SetupPrintUrl()
        {
            //PagePrintUrl = "db_alpha.aspx?print=1";
            PagePrintUrl = "?print=1";

            //add expand
            if (!string.IsNullOrEmpty(Expand))
            {
                if (Expand.Trim() == "#")
                {
                    PagePrintUrl += "&expand=%23";
                }
                else
                {
                    PagePrintUrl += "&expand=" + Expand.Trim().ToUpper();
                }
            }

            //Language stuff
            PagePrintUrl += QueryStringLang;

            //add cdrid or searchstr
            if (!string.IsNullOrEmpty(CdrID))
            {
                PagePrintUrl += "&cdrid=" + CdrID;
            }
            else if (!string.IsNullOrEmpty(SearchStr))
            {
                PagePrintUrl += "&searchTxt=" + SearchStr;
                if (BContains)
                    PagePrintUrl += "&sgroup=Contains";
            }

            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("Print", (name, url) =>
            {
                url.SetUrl(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentURL").ToString() + "/" + PagePrintUrl);
            });

            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
            {
                if (CdrID != "")
                    url.SetUrl(url.ToString() + "?cdrid=" + CdrID);
                else if (Expand != "")
                    url.SetUrl(url.ToString() + "?expland=" + Expand);
                else 
                    url.SetUrl(url.ToString());
            });

            
        }


        #region Data-related
        /// <summary>
        /// Handles calls to the Management layer to get data
        /// </summary>
        private void LoadData()
        {
            string language = string.Empty;
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
            {
                language = "English";
            }
            else
            {
                language = "Spanish";
            }

            if (!string.IsNullOrEmpty(CdrID)) //this is a cdrid lookup for one term
            {
                TermDictionaryDataItem dataItem = TermDictionaryManager.GetDefinitionByTermID(language, CdrID, null, 5);
                if (dataItem != null)
                {
                    NumResults = 1;
                    ActivateDefinitionView(dataItem);

                    // Web Analytics *************************************************
                    if (WebAnalyticsOptions.IsEnabled)
                    {
                        this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.DictionaryTermView, wbField =>
                        {
                            wbField.Value = null;
                        });

                    }

                }
                else
                {
                    ActivateDefaultView();
                }
            }
            else //This is a search
            {
                TermDictionaryCollection dataCollection = TermDictionaryManager.Search(language, SearchStr, 0, BContains);
                if (dataCollection.Count == 1) //if there is only 1 record - go directly to definition view
                {
                    // If only one definition found, redirect so URL contains CdrID
                    if (QueryStringLang == string.Empty)
                        Page.Response.Redirect(DictionaryURL + "?CdrID=" + dataCollection[0].GlossaryTermID.ToString(), true);
                    else
                        Page.Response.Redirect(DictionaryURL + "?CdrID=" + dataCollection[0].GlossaryTermID.ToString() + QueryStringLang, true);

                }
                else
                {
                    resultListView.DataSource = dataCollection;
                    resultListView.DataBind();
                    NumResults = dataCollection.Count;
                    ActivateResultsListView();
                }
            }
        }
        #endregion

        private void ActivateDefaultView()
        {
            string language = "";

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                language = "English";
            else
                language = "Spanish";

            TermDictionaryCollection dataCollection = TermDictionaryManager.Search(language, "_", 0, false);
            _totalCount = dataCollection.Count;
 
            MultiView1.ActiveViewIndex = 0;

        }

        private void ActivateResultsListView()
        {
            MultiView1.ActiveViewIndex = 1;
            litBackToTop.Visible = (NumResults > 1);
            if (NumResults == 0)
            {
                RenderNoResults();
            }
        }

        private void RenderNoResults()
        {
            Control c = resultListView.Controls[0];
            Panel noDataEngPanel = (Panel)c.FindControl("pnlNoDataEnglish");
            Panel noDataSpanPanel = (Panel)c.FindControl("pnlNoDataSpanish");

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                noDataSpanPanel.Visible = true;
            else
                noDataEngPanel.Visible = true;
        }

        private void ValidateParams()
        {
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            if (!string.IsNullOrEmpty(CdrID.Trim()))
                try
                {
                    Int32.Parse(CdrID);
                }
                catch(Exception ex)
                {
                    throw new  Exception("Invalid CDRID" + CdrID);
                    
                }
        }

        /// <summary>
        /// Saves the quesry parameters to support old gets
        /// </summary>
        private void GetQueryParams()
        {
            Expand = Strings.Clean(Request.Params["expand"]);
            CdrID = Strings.Clean(Request.Params["cdrid"]);
            SearchStr = Strings.Clean(Request.Params["searchTxt"]);
            SrcGroup = Strings.Clean(Request.Params["sgroup"]);
        }

        /// <summary>
        /// Set up Spanish Properties
        /// </summary>
        private void SetupSpanish()
        {
            _isSpanish = true;
            lblAutoComplete1.Text = "buscar";
            //Controls
            //lblStrSearch.Text = String.Empty;
            //lblAccessSearch.Text = "Cuadro de búsqueda de texto";

            //lblStarts.Text = "Empieza con";
            //lblContains.Text = "Contiene";
            lblResultsFor.Text = "resultados de:";
            
            //radioStarts.Text = "Empieza con";
            //radioContains.Text = "Contiene";

            lblStartsWith.Text = "Empieza con";
            lblContains.Text = "Contiene";

            pnlIntroEnglish.Visible = false;
            pnlIntroSpanish.Visible = true;

            btnGo.ImageUrl = @"/images/red_buscar_button.gif";
            //btnGo.AlternateText = "Botón de búsqueda";
            //btnGo.ToolTip = "Botón de búsqueda";
            btnGo.AlternateText = "Buscar";
            btnGo.ToolTip = "Buscar";
            //btnGo.CssClass = "btnBuscar";

            AutoComplete1.CloseLinkText = "cerrar";
            //searchboxBtn.Attributes.Add("style", "width:64px;");
            //searchboxStarts.Attributes.Add("style", "margin-right:6px");
            //searchboxStarts.Attributes.Add("style", "width:95px;");

            //alphaListBox.UrlArgs = "lang=spanish";

            //Page Properties
            PageOptionsBoxTitle = "Opciones";
            PrevText = "Definiciones anteriores:";
            NextText = "Definiciones siguientes:";
            //QueryStringLang = "&lang=spanish";

            //// Get the image we need to display
            //CancerGov.UI.HTML.HtmlImage himage = null;
            //if (this.PageDisplayInformation.Version == DisplayVersion.Image)
            //    himage = new CancerGov.UI.HTML.HtmlImage("/images/title-default-spanish.jpg", "");

            ////Title block
            //this.PageHtmlHead.Title = "Diccionario de c&aacute;ncer - National Cancer Institute";
            ////_titleBl = new TitleBlock("Diccionario de c&aacute;ncer", new CancerGov.UI.HTML.HtmlImage("/images/title-default-spanish.jpg", "", "165", "58"), this.PageDisplayInformation);
            //_titleBl = new TitleBlock("Diccionario de c&aacute;ncer", himage, this.PageDisplayInformation);
            //this.PageHeaders.Add(_titleBl);
            //this.PageLeftColumn = new LeftNavColumn(this, Strings.ToGuid(ConfigurationSettings.AppSettings["SpanishDictionaryLeftViewID"]));

            ////common display features
            SetupCommon();
        }

        /// <summary>
        /// Set up English Properties
        /// </summary>
        private void SetupEnglish()
        {
            //Controls            
            lblAutoComplete1.Text = "Search for";
            lblResultsFor.Text = "results found for:";

            pnlIntroEnglish.Visible = true;
            pnlIntroSpanish.Visible = false;

            //Page Props
            PageOptionsBoxTitle = "Page Options";
            PrevText = "Previous Definitions:";
            NextText = "Next Definitions:";

            //common display features
            SetupCommon();
        }
        /// <summary>
        /// Setup shared by English and Spanish versions
        /// </summary>
        private void SetupCommon()
        {
            string language = string.Empty;
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
            {
                language = "English";
            }
            else
            {
                language = "Spanish";
            }
            // This sets the url and link text for close
            AutoComplete1.SearchURL = "/TermDictionary.svc/SearchJSON/" + language + "?searchTerm=";

            radioStarts.Attributes["onclick"] = "toggleSearchMode(event, '" + AutoComplete1.ClientID + "', false)";
            radioContains.Attributes["onclick"] = "toggleSearchMode(event, '" + AutoComplete1.ClientID + "', true)";

            radioStarts.Attributes["onmouseover"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', true)";
            radioStarts.Attributes["onmouseout"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', false)";

            radioContains.Attributes["onmouseover"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', true)";
            radioContains.Attributes["onmouseout"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', false)";

            //lblStarts.Attributes["onmouseover"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', true)";
            //lblStarts.Attributes["onmouseout"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', false)";

            //lblContains.Attributes["onmouseover"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', true)";
            //lblContains.Attributes["onmouseout"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', false)";

            if (!string.IsNullOrEmpty(SrcGroup))
                BContains = SrcGroup.Equals("Contains");

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
            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web)
                RenderGutter();

            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Print)
            {
                pnlTermSearch.Visible = false;

            }
            else
            {
                alphaListBox.TextOnly = (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web) ? true : false;
                alphaListBox.Title = string.Empty;
            }
        }
        private void RenderGutter()
        {

            //gutterLangSwitch.DisplayInfo = this.PageDisplayInformation;
            gutterLangSwitch.EnglishUrl = "/dictionary/";
            gutterLangSwitch.SpanishUrl = "/diccionario/";
            gutterLangSwitch.Visible = true;
        }

        private void ActivateDefinitionView(TermDictionaryDataItem dataItem)
        {
            string language = string.Empty;
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
            {
                language = "English";
            }
            else
            {
                language = "Spanish";
            }

            MultiView1.ActiveViewIndex = 2;
            pnlPrevNext.Visible = true;

            string termName = string.Empty;
            string termPronun = string.Empty;
            string defHtml = string.Empty;
            string imageHtml = string.Empty;
            string audioMediaHtml = string.Empty;
            string relatedLinkInfo = String.Empty;

            CdrID = dataItem.GlossaryTermID.ToString();
            termName = dataItem.TermName;
            termPronun = dataItem.TermPronunciation;
            defHtml = dataItem.DefinitionHTML;
            imageHtml = (dataItem.MediaHTML == null) ? string.Empty : dataItem.MediaHTML;
            imageHtml = imageHtml.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);

            audioMediaHtml = (dataItem.AudioMediaHTML == null) ? string.Empty : dataItem.AudioMediaHTML;
            audioMediaHtml = audioMediaHtml.Replace("[_audioMediaLocation]", ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]);
            audioMediaHtml = audioMediaHtml.Replace("[_flashMediaLocation]", ConfigurationSettings.AppSettings["FlashMediaLocation"]);

            relatedLinkInfo = (dataItem.RelatedInfoHTML == null) ? string.Empty : dataItem.RelatedInfoHTML;

            if (language == "Spanish")
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("short_title", (name, data) =>
                        {
                            data.Value = "Definici&oacute;n de " + termName + " - Diccionario de c&aacute;ncer";
                        });

                this.Page.Header.Title=PageAssemblyContext.Current.PageAssemblyInstruction.GetField("short_title");
            }
            else
            {

                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("short_title", (name, data) =>
                        {
                            data.Value = "Definition of " + termName + " - NCI Dictionary of Cancer Terms";
                        });


                this.Page.Header.Title=PageAssemblyContext.Current.PageAssemblyInstruction.GetField("short_title");
                lblTermPronun.Text = termPronun;
            }

            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_description", (name, data) =>
            {
                data.Value = "Definition of " + termName;
            });


            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_keywords", (name, data) =>
            {
                data.Value = termName + ", definition";

            });

           

            if (imageHtml != string.Empty)
                imageHtml = "<p>" + imageHtml;

            lblTermName.Text = termName;
            litDefHtml.Text = defHtml;
            litImageHtml.Text = imageHtml;
            litAudioMediaHtml.Text = audioMediaHtml;

            if (!string.IsNullOrEmpty(relatedLinkInfo))
            {
                pnlRelatedInfo.Visible = true;
                litRelatedLinkInfo.Text = relatedLinkInfo;
            }
            else
                pnlRelatedInfo.Visible = false;

            RenderLangButtons();
        }

        private void RenderLangButtons()
        {
            langSwitch.EnglishUrl = DictionaryURLEnglish + "?CdrID=" + CdrID;
            langSwitch.SpanishUrl = DictionaryURLSpanish + "?CdrID=" + CdrID;
        }

        protected string AudioMediaHTML(object objData)
        {
            string audioMediaHTML = String.Empty;
            if (objData != null )
            {
                audioMediaHTML = objData.ToString();
                audioMediaHTML = audioMediaHTML.Replace("[_audioMediaLocation]", ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]);
            }

            return audioMediaHTML;
        }

        protected string ResultListViewHrefOnclick(ListViewDataItem dataItem)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "onclick=\"NCIAnalytics.TermsDictionaryResults(this,'" + (dataItem.DataItemIndex + 1).ToString() + "');\""; // Load results onclick script
            else
                return "";
        }

    }
}
