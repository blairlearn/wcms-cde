using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

using CancerGov.Text;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using System.Text;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class DrugDictionaryDefinitionView : SnippetControl
    {
        public string SearchStr { get; set; }

        public string Expand { get; set; }

        public string CdrID { get; set; }

        public string SrcGroup { get; set; }

        public bool BContains { get; set; }

        public string DictionaryURLSpanish { get; set; }

        public string DictionaryURLEnglish { get; set; }

        public string DictionaryURL { get; set; }

        public String DictionaryLanguage { get; set; }

        public string QueryStringLang { get; set; }

        public string PagePrintUrl { get; set; }

        public int RelatedTermCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {       
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            GetQueryParams();
            ValidateParams();
            
            DictionaryURLSpanish = DictionaryURL;
            DictionaryURLEnglish = DictionaryURL;
                       
            if (Request.RawUrl.ToLower().Contains("dictionary") && Request.RawUrl.ToLower().Contains("spanish"))
            {
                Response.Redirect("/diccionario" + Request.Url.Query);
            }

            DictionaryLanguage = PageAssemblyContext.Current.PageAssemblyInstruction.Language;
                       
            
            if (!Page.IsPostBack)
            {
                DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

                DictionaryTerm dataItem = _dictionaryAppManager.GetTerm(Convert.ToInt32(CdrID), NCI.Web.Dictionary.DictionaryType.drug, DictionaryLanguage, "v1");
                if (dataItem != null && dataItem.Term != null)
                {
                    ActivateDefinitionView(dataItem);
                    // Web Analytics *************************************************
                    if (WebAnalyticsOptions.IsEnabled)
                    {
                        // Add dictionary term view event to analytics
                        this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.event11, wbField =>
                        {
                            wbField.Value = null;
                        });
                    }
                }
                else
                {
                    drugDictionaryDefinitionView.Visible = false;
                }
            }

            SetupPrintUrl();
                                   
        }

        private void ActivateDefinitionView(DictionaryTerm dataItem)
        {

            var myDataSource = new List<DictionaryTerm> { dataItem };

            drugDictionaryDefinitionView.Visible = true;
            drugDictionaryDefinitionView.DataSource = myDataSource;
            drugDictionaryDefinitionView.DataBind();

            string termName = dataItem.Term;

            CdrID = dataItem.ID.ToString();

            if (DictionaryLanguage == "es")
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("browser_title", (name, data) =>
                {
                    data.Value = "Definici&oacute;n de " + termName + " - Diccionario de c&aacute;ncer";
                });
            }
            else
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("browser_title", (name, data) =>
                {
                    data.Value = "Definition of " + termName + " - NCI Dictionary of Cancer Terms";
                });
            }

            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_description", (name, data) =>
            {
                data.Value = "Definition of " + termName;
            });


            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("meta_keywords", (name, data) =>
            {
                data.Value = termName + ", definition";
            });



        }

        // Type names from the TermOtherNameType table, sorted by
        // priority order.
        const string NAME_TYPE_SYNONYM = "synonym";
        const string NAME_TYPE_BRAND_NAME = "us brand name";
        const string NAME_TYPE_FOREIGN_BRAND = "foreign brand name";
        const string NAME_TYPE_ABBREV = "abbreviation";
        const string NAME_TYPE_ACRONYM = "acronym";
        const string NAME_TYPE_CODE_NAME = "code name";
        const string NAME_TYPE_CHEMICAL_NAME = "chemical structure name";

        const string NAME_LABEL_SYNONYM = "Synonym";
        const string NAME_LABEL_BRAND_NAME = "US brand name";
        const string NAME_LABEL_FOREIGN_BRAND = "Foreign brand name";
        const string NAME_LABEL_ABBREV = "Abbreviation";
        const string NAME_LABEL_ACRONYM = "Acronym";
        const string NAME_LABEL_CODE_NAME = "Code name";
        const string NAME_LABEL_CHEMICAL_NAME = "Chemical structure";



        /// <summary>
        /// Stinky code to generate a drug definition's list of aliases.
        /// </summary>
        /// <param name="termSearchResult"></param>
        /// <returns></returns>
        public string GenerateTermAliasList(object termSearchResult)
        {
            string aliasDisplay = String.Empty;

            DictionaryTerm termInfo = termSearchResult as DictionaryTerm;
            if (termInfo != null && termInfo.HasAliases)
            {
                Dictionary<string, List<string>> nameTypeMap = RollupNameTypeLists(termInfo.Aliases);
                StringBuilder sb = new StringBuilder();

                // Display the list of names.
                if (nameTypeMap.Count > 0)
                {
                    sb.Append("<p><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");

                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_SYNONYM,      NAME_LABEL_SYNONYM));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_BRAND_NAME,   NAME_LABEL_BRAND_NAME));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_FOREIGN_BRAND,NAME_LABEL_FOREIGN_BRAND));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_ABBREV,       NAME_LABEL_ABBREV));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_ACRONYM,      NAME_LABEL_ACRONYM));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_CODE_NAME,    NAME_LABEL_CODE_NAME));
                    sb.Append(GenerateAliasULRow(nameTypeMap, NAME_TYPE_CHEMICAL_NAME, NAME_LABEL_CHEMICAL_NAME));

                    sb.Append("</table><p>");
                }

                aliasDisplay = sb.ToString();
            }

            return aliasDisplay;
        }

        private string GenerateAliasRow(Dictionary<string, List<string>> nameTypeMap, string nameTypeKey, string nameLabel)
        {
            string row = String.Empty;

            if (nameTypeMap.ContainsKey(nameTypeKey))
            {
                List<string> aliasList = nameTypeMap[nameTypeKey];
                string nameList = String.Empty;
                if (aliasList.Count > 1)
                {
                    // Append all the names to one another, separating with a <br />.  Arguably, this might be done better
                    // with a StringBuilder, but this should be such a small list that the StringBuilder overhead is worse than
                    // the multiple small objects.
                    aliasList.ForEach(name => { nameList += name + "<br />"; });
                }
                else
                    nameList = aliasList[0];

                row = String.Format("<tr><td valign=\"top\" width=\"28%\"><b>{0}:</b></td><td valign=\"top\" width=\"68%\">{1}</td>",
                    nameLabel, nameList);
            }

            return row;
        }

        /// <summary>
        /// Special handling for aliases (e.g. Chemical Name) where multiple values are to be output in an unordered list
        /// instead of the usual list using br tags.
        /// </summary>
        /// <param name="nameTypeMap"></param>
        /// <param name="nameTypeKey"></param>
        /// <param name="nameLabel"></param>
        /// <returns></returns>
        private string GenerateAliasULRow(Dictionary<string, List<string>> nameTypeMap, string nameTypeKey, string nameLabel)
        {
            string row = String.Empty;

            if (nameTypeMap.ContainsKey(nameTypeKey))
            {
                List<string> aliasList = nameTypeMap[nameTypeKey];
                string nameList = String.Empty;
                if (aliasList.Count > 1)
                {
                    
                    // Append all the names to one another, separating with a <br />.  Arguably, this might be done better
                    // with a StringBuilder, but this should be such a small list that the StringBuilder overhead is worse than
                    // the multiple small objects.
                    aliasList.ForEach(name => { nameList += "<li>" + name; });
                    nameList = String.Format("<ul>{0}</ul>", nameList);
                }
                else
                    nameList = aliasList[0];

                row = String.Format("<tr><td valign=\"top\" width=\"28%\"><b>{0}:</b></td><td valign=\"top\" width=\"68%\">{1}</td>",
                    nameLabel, nameList);
            }

            return row;
        }


        /// <summary>
        /// Replicates legacy logic for rolling up alias names into a collection of named lists.
        /// </summary>
        /// <param name="arrTermAliases"></param>
        /// <returns>A non-empty Dictionary structure with name types as keys and sorted lists of 
        /// aliases as the values.</returns>
        private Dictionary<string, List<string>> RollupNameTypeLists(Alias[] arrTermAliases)
        {
            Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();

            // Put the list into alphabetic order, based on the alias name.
            Array.Sort(arrTermAliases, (a, b) => { 
                return a.Name.CompareTo(b.Name); 
            });

            // Add individual aliases to the list associated with the alias type.
            Array.ForEach(arrTermAliases, alias =>
            {
                string key = alias.Type.ToLower();
                List<string> nameList;

                // If there's no list for a name, create one.
                if (!map.ContainsKey(key))
                    map.Add(key, new List<string>());

                nameList = map[key];
                nameList.Add(alias.Name);
            });


            return map;
        }

        /**
         * Add URL filter for old print page implementation
         * @deprecated
         */
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
                PagePrintUrl += "&search=" + SearchStr;
                if (BContains)
                    PagePrintUrl += "&contains=true";
            }

            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter("Print", (name, url) =>
            {
                url.SetUrl(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentURL").ToString() + "/" + PagePrintUrl);
            });
        }

        protected void drugDictionaryDefinitionView_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //get the TermReturn object that is bound to the current row.
                DictionaryTerm termDetails = (DictionaryTerm)e.Item.DataItem;

                if (termDetails != null)
                {
                    PlaceHolder phPronunciation = (PlaceHolder) e.Item.FindControl("phPronunciation");
                    if (termDetails.HasPronunciation && phPronunciation != null)
                    {
                        phPronunciation.Visible = true;
                        System.Web.UI.HtmlControls.HtmlAnchor pronunciationLink = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("pronunciationLink");
                        if (pronunciationLink != null && termDetails.Pronunciation.HasAudio)
                        {
                            pronunciationLink.Visible = true;
                            pronunciationLink.HRef = termDetails.Pronunciation.Audio;
                        }
                        else
                            pronunciationLink.Visible = false;

                        Literal pronunciationKey = (Literal)e.Item.FindControl("pronunciationKey");
                        if (pronunciationKey != null && termDetails.Pronunciation.HasKey)
                            pronunciationKey.Text = " " + termDetails.Pronunciation.Key;

                    }
                    else
                        phPronunciation.Visible = false;

                                    
                    PlaceHolder phAliasList = (PlaceHolder) e.Item.FindControl("phAliasList");
                    if (phAliasList != null && termDetails.HasAliases)
                        phAliasList.Visible = true;
                }
            } 
        }

        protected void relatedTerms_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //get the RelatedTerm object that is bound to the current row.
                RelatedTerm relatedTerm = (RelatedTerm)e.Item.DataItem;
                if (relatedTerm != null)
                {
                    HyperLink relatedTermLink = (HyperLink)e.Item.FindControl("relatedTermLink");
                    if (relatedTermLink != null)
                    {
                        relatedTermLink.NavigateUrl = DictionaryURL + "?cdrid=" + relatedTerm.Termid;
                        relatedTermLink.Text = relatedTerm.Text;

                        //make sure the comma is only displayed when there is more than one related term
                        Literal relatedTermSeparator = (Literal)e.Item.FindControl("relatedTermSeparator");
                        if (relatedTermSeparator != null)
                        {
                            if (e.Item.ItemIndex >= 0 && e.Item.ItemIndex < RelatedTermCount - 1)
                                relatedTermSeparator.Visible = true;
                        }

                    }
                }
            }
        }

        protected void relatedImages_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //get the ImageReference object that is bound to the current row.
                ImageReference imageDetails = (ImageReference)e.Item.DataItem;

                if (imageDetails != null)
                {                    
                    System.Web.UI.HtmlControls.HtmlImage termImage = (System.Web.UI.HtmlControls.HtmlImage)e.Item.FindControl("termImage");
                    if (termImage != null)
                    {                       
                        termImage.Alt = imageDetails.AltText;
                       
                        if (!string.IsNullOrEmpty(imageDetails.Filename))
                        {
                            System.Web.UI.HtmlControls.HtmlAnchor termEnlargeImage = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("termEnlargeImage");

                            //if either the regular image size or the enlarge image size is not in the config file
                            //default to the full image in the database
                            if (string.IsNullOrEmpty(ConfigurationSettings.AppSettings["CDRImageRegular"]) || string.IsNullOrEmpty(ConfigurationSettings.AppSettings["CDRImageEnlarge"]))
                            {
                                termImage.Src = imageDetails.Filename;

                                if (termEnlargeImage != null)
                                    termEnlargeImage.HRef = imageDetails.Filename;

                                //log a warning
                                NCI.Logging.Logger.LogError("DrugDictionaryDefinitionView.ascx", "Web.Config file does not specify image sizes for term id: " + CdrID + ". Display full image.", NCI.Logging.NCIErrorLevel.Warning);
                            }
                            else
                            {
                                string[] regularTermImage = imageDetails.Filename.Split('.');
                                if (regularTermImage.Length == 2)
                                {
                                    //termImage image size is 571
                                    //example format CDR526538-571.jpg
                                    termImage.Src = regularTermImage[0] + "-" + ConfigurationSettings.AppSettings["CDRImageRegular"] + "." + regularTermImage[1];

                                    //enlarge image size is 750
                                    //example format CDR526538-750.jpg
                                    if (termEnlargeImage != null)
                                        termEnlargeImage.HRef = regularTermImage[0] + "-" + ConfigurationSettings.AppSettings["CDRImageEnlarge"] + "." + regularTermImage[1];

                                }
                            }
                                                                                    
                        }
                    }
                    
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
                catch (Exception)
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
    }
}