using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CancerGov.Text;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using CancerGov.Dictionaries.SnippetControls.Helpers;
using CancerGov.Dictionaries;
using CancerGov.Dictionaries.Configuration;

namespace CancerGov.Dictionaries.SnippetControls.DrugDictionary
{
    public class DrugDictionaryDefinitionView : BaseDictionaryControl
    {
        protected DrugDictionaryHome dictionarySearchBlock;

        protected Repeater drugDictionaryDefinitionView;

        static ILog log = LogManager.GetLogger(typeof(DrugDictionaryDefinitionView));

        public string CdrID { get; set; }

        public string FriendlyName { get; set; }

        public string DictionaryURLSpanish { get; set; }

        public string DictionaryURLEnglish { get; set; }

        public string DictionaryURL { get; set; }

        public String DictionaryLanguage { get; set; }

        public int RelatedTermCount { get; set; }

        /// <summary>
        /// Gets or sets the PrettyUrl of the page this component lives on.
        /// </summary>
        protected string PrettyUrl { get; set; }

        /// <summary>
        /// Gets or sets the current Url as an NciUrl object.
        /// </summary>
        protected NciUrl CurrentUrl { get; set; }

        /// <summary>
        /// Gets or sets the Current AppPath, which is usually something like
        /// https://www.cancer.gov(PURL)(AppPath)
        /// (Where both PURL and AppPath start with /)
        /// </summary>
        protected string CurrAppPath
        {
            get
            {
                //Get the Current Application Path, e.g. if the URL is /foo/bar/results/chicken,
                //and the pretty URL is /foo/bar, then the CurrAppPath should be /results/chicken
                if (this.CurrentUrl.UriStem.Length == this.PrettyUrl.Length)
                    return "/";
                else
                    return this.CurrentUrl.UriStem.Substring(PrettyUrl.Length);
            }
        }

        /// <summary>
        /// This sets up the Original Pretty URL, the current full URL and the app path
        /// </summary>
        private void SetupUrls()
        {
            //We want to use the PURL for this item.
            //NOTE: THIS 
            NciUrl purl = this.PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl);

            if (purl == null || string.IsNullOrWhiteSpace(purl.ToString()))
                throw new Exception("Dictionary requires current PageAssemblyInstruction to provide its PrettyURL through GetURL.  PrettyURL is null or empty.");

            //It is expected that this is pure and only the pretty URL of this page.
            //This means that any elements on the same page as this app should NOT overwrite the
            //PrettyURL URL Filter.
            this.PrettyUrl = purl.ToString();

            //Now, that we have the PrettyURL, let's figure out what the app paths are...
            NciUrl currURL = new NciUrl();
            currURL.SetUrl(HttpContext.Current.Request.RawUrl);
            currURL.SetUrl(currURL.UriStem);

            //Make sure this URL starts with the pretty url
            if (currURL.UriStem.ToLower().IndexOf(this.PrettyUrl.ToLower()) != 0)
                throw new Exception(String.Format("JSApplicationProxy: Cannot Determine App Path for Page, {0}, with PrettyURL {1}.", currURL.UriStem, PrettyUrl));

            this.CurrentUrl = currURL;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

            SetupUrls();
            SetupCanonicalUrl(DictionaryURL);
            GetDefinitionTerm();
            ValidateCDRID();

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

            //SetupPrintUrl();
        }

        //Add a filter for the Canonical URL.
        private void SetupCanonicalUrl(string englishDurl)
        {
            PageAssemblyContext.Current.PageAssemblyInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, SetupUrlFilter);
        }

        private void SetupUrlFilter(string name, NciUrl url)
        {
            url.SetUrl(url.ToString() + "/def/" + GetFriendlyName(CdrID));
        }

        private void ActivateDefinitionView(DictionaryTerm dataItem)
        {
            var myDataSource = new List<DictionaryTerm> { dataItem };

            drugDictionaryDefinitionView.Visible = true;
            drugDictionaryDefinitionView.DataSource = myDataSource;
            drugDictionaryDefinitionView.DataBind();

            string termName = dataItem.Term;

            CdrID = dataItem.ID.ToString();

            PageAssemblyContext.Current.PageAssemblyInstruction.AddFieldFilter("browser_title", (name, data) =>
            {
                data.Value = "Definition of " + termName + " - NCI Drug Dictionary";
            });

            DictionaryDefinitionHelper.SetMetaTagDescription(dataItem, DictionaryLanguage, PageInstruction);

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
        const string NAME_TYPE_IND_NUMBER = "IND number";
        const string NAME_TYPE_NSC_CODE = "NSC code";

        const string NAME_LABEL_SYNONYM = "Synonym";
        const string NAME_LABEL_BRAND_NAME = "US brand name";
        const string NAME_LABEL_FOREIGN_BRAND = "Foreign brand name";
        const string NAME_LABEL_ABBREV = "Abbreviation";
        const string NAME_LABEL_ACRONYM = "Acronym";
        const string NAME_LABEL_CODE_NAME = "Code name";
        const string NAME_LABEL_CHEMICAL_NAME = "Chemical structure";
        const string NAME_LABEL_IND_NUMBER = "IND number";
        const string NAME_LABEL_NSC_CODE = "NSC code";

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

                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_SYNONYM, NAME_LABEL_SYNONYM));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_BRAND_NAME, NAME_LABEL_BRAND_NAME));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_FOREIGN_BRAND, NAME_LABEL_FOREIGN_BRAND));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_ABBREV, NAME_LABEL_ABBREV));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_ACRONYM, NAME_LABEL_ACRONYM));
                    sb.Append(GenerateAliasRow(nameTypeMap, NAME_TYPE_CODE_NAME, NAME_LABEL_CODE_NAME));
                    sb.Append(GenerateAliasULRow(nameTypeMap, NAME_TYPE_CHEMICAL_NAME, NAME_LABEL_CHEMICAL_NAME));
                    sb.Append(GenerateAliasULRow(nameTypeMap, NAME_TYPE_IND_NUMBER, NAME_LABEL_IND_NUMBER));
                    sb.Append(GenerateAliasULRow(nameTypeMap, NAME_TYPE_NSC_CODE, NAME_LABEL_NSC_CODE));

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
            Array.Sort(arrTermAliases, (a, b) =>
            {
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

        protected void drugDictionaryDefinitionView_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //get the TermReturn object that is bound to the current row.
                DictionaryTerm termDetails = (DictionaryTerm)e.Item.DataItem;

                if (termDetails != null)
                {
                    PlaceHolder phAliasList = (PlaceHolder)e.Item.FindControl("phAliasList");
                    if (phAliasList != null && termDetails.HasAliases)
                        phAliasList.Visible = true;

                    SetupDrugInfoSummaryLink(e.Item, termDetails);
                }
            }
        }

        /// <summary>
        /// Set up the link to the definition's related drug information summary, if one exists.
        /// Sets up at most one related drug summary.
        /// </summary>
        /// <param name="dataItem">The DictionaryTerm item for the term being displayed.</param>
        private void SetupDrugInfoSummaryLink(Control parent, DictionaryTerm dataItem)
        {
            if (parent != null
                && dataItem != null
                && dataItem.HasRelatedItems
                && dataItem.Related.DrugSummary != null
                && dataItem.Related.DrugSummary.Length > 0)
            {
                // Only load data from the first item.
                RelatedDrugSummary drugSumamry = dataItem.Related.DrugSummary[0];

                // Find the dd tag containing the hyperlink.
                Control container = parent.FindControl("ddPatientInfo");

                // Find the actual hyperlink.
                HyperLink hlPatientInfo = (HyperLink)parent.FindControl("hlPatientInfo");
                if (container != null && hlPatientInfo != null)
                {
                    container.Visible = true;
                    hlPatientInfo.NavigateUrl = drugSumamry.url;
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
                            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImageRegular"]) || string.IsNullOrEmpty(ConfigurationManager.AppSettings["CDRImageEnlarge"]))
                            {
                                termImage.Src = imageDetails.Filename;

                                if (termEnlargeImage != null)
                                    termEnlargeImage.HRef = imageDetails.Filename;

                                //log a warning
                                log.WarnFormat("Web.Config file does not specify image sizes for term id: {0}. Display full image.", CdrID);
                            }
                            else
                            {
                                string[] regularTermImage = imageDetails.Filename.Split('.');
                                if (regularTermImage.Length == 2)
                                {
                                    //termImage image size is 571
                                    //example format CDR526538-571.jpg
                                    termImage.Src = regularTermImage[0] + "-" + ConfigurationManager.AppSettings["CDRImageRegular"] + "." + regularTermImage[1];

                                    //enlarge image size is 750
                                    //example format CDR526538-750.jpg
                                    if (termEnlargeImage != null)
                                        termEnlargeImage.HRef = regularTermImage[0] + "-" + ConfigurationManager.AppSettings["CDRImageEnlarge"] + "." + regularTermImage[1];
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ValidateCDRID()
        {
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

        private void GetDefinitionTerm()
        {
            List<string> path = this.CurrAppPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            if (path.Count > 0 && path[0].Equals("def"))
            {
                string param = Strings.Clean(path[1]);
                param = Server.UrlDecode(param);

                // Get friendly name to CDRID mappings
                string dictionaryMappingFilepath = null;

                dictionaryMappingFilepath = this.DictionaryConfiguration.Files.Single(a => a.Locale == PageAssemblyContext.Current.PageAssemblyInstruction.Language).Filepath;

                if(!string.IsNullOrEmpty(dictionaryMappingFilepath))
                {
                    TerminologyMapping map = TerminologyMapping.GetMappingForFile(dictionaryMappingFilepath);

                    // If pretty name is in label mappings, set CDRID
                    if (map.MappingContainsFriendlyName(param))
                    {
                        CdrID = map.GetCDRIDFromFriendlyName(param);
                    }
                    else
                    {
                        CdrID = param;
                    }
                }
                else
                {
                    CdrID = param;
                }

                if (path.Count > 2)
                {
                    // If path extends further than /search or /def/<term>, raise a 400 error
                    NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("Dictionary", 400, "Invalid parameters for dictionary");
                }
            }
        }
    }
}