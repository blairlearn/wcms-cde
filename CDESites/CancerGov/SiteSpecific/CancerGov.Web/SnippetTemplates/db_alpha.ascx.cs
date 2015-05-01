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
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ValidateParams();
            GetQueryParams();

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

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                DictionaryURL = DictionaryURLSpanish;

            //Set display props according to lang
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
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

                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                    Page.Form.Action += "?lang=spanish";
            }
            else
            {
                Page.Form.Action = DictionaryURL;
            }

            alphaListBox.BaseUrl = DictionaryURL;

            if (Page.Request.RequestType.Equals("POST")) //This is a POST(back)
            {
                SearchStr = Request.Form[AutoComplete1.UniqueID];
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
        }

        private void SetupPrintUrl()
        {
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
                    url.SetUrl(url.ToString() + "?expand=" + Expand);
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
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                language = "Spanish";
            }
            else
            {
                language = "English";
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

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                language = "Spanish";
            else
                language = "English";

            TermDictionaryCollection dataCollection = TermDictionaryManager.Search(language, "_", 0, false);
            _totalCount = dataCollection.Count;
 
            MultiView1.ActiveViewIndex = 0;
            numResDiv.Visible = (NumResults > 0);
        }

        private void ActivateResultsListView()
        {
            ActivateDefaultView();
            MultiView2.ActiveViewIndex = 0;
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

            //Controls
            AutoComplete1.Attributes.Add("aria-label", "Escriba frase o palabra clave");
            AutoComplete1.Attributes.Add("placeholder", "Escriba frase o palabra clave");
            
            lblResultsFor.Text = "resultados de:";
            lblStartsWith.Text = "Empieza con";
            lblContains.Text = "Contiene";

            pnlIntroEnglish.Visible = false;
            pnlIntroSpanish.Visible = true;

            btnGo.Text = "Buscar";
            btnGo.ToolTip = "Buscar";

            //Page Properties
            PageOptionsBoxTitle = "Opciones";
            PrevText = "Definiciones anteriores:";
            NextText = "Definiciones siguientes:";
            
            ////common display features
            SetupCommon();
        }

        /// <summary>
        /// Set up English Properties
        /// </summary>
        private void SetupEnglish()
        {
            //Controls            
            AutoComplete1.Attributes.Add("aria-label", "Enter keywords or phrases");
            AutoComplete1.Attributes.Add("placeholder", "Enter keywords or phrases");

            lblResultsFor.Text = "results found for:";
            btnGo.Text = "Search";

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
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                language = "Spanish";
            }
            else
            {
                language = "English";
            }

            //radioStarts.InputAttributes["onclick"] = "toggleSearchMode(event, '" + AutoComplete1.ClientID + "', false)";
            //radioContains.InputAttributes["onclick"] = "toggleSearchMode(event, '" + AutoComplete1.ClientID + "', true)";

            //radioStarts.InputAttributes["onmouseover"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', true)";
            //radioStarts.InputAttributes["onmouseout"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', false)";

            //radioContains.InputAttributes["onmouseover"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', true)";
            //radioContains.InputAttributes["onmouseout"] = "keepListBox(event, '" + AutoComplete1.ClientID + "', false)";

            radioStarts.InputAttributes.Add("onchange", "autoFunc();");
            radioContains.InputAttributes.Add("onchange", "autoFunc();");

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
            gutterLangSwitch.EnglishUrl = "/dictionary/";
            gutterLangSwitch.SpanishUrl = "/diccionario/";
            gutterLangSwitch.Visible = true;
        }

        private void ActivateDefinitionView(TermDictionaryDataItem dataItem)
        {
            string language = string.Empty;
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                language = "Spanish";
            }
            else
            {
                language = "English";
            }

            ActivateDefaultView();
            MultiView2.ActiveViewIndex = 1;

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
