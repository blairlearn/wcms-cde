using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common.Logging;
using NCI.Core;
using NCI.Util;
using NCI.Web.CDE.WebAnalytics;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Abstract base class for both single page and multiple page classes to 
    /// encapsulate common members and functionality.
    /// </summary>
    abstract public class BasePageAssemblyInstruction
    {
        #region Private
        static ILog log = LogManager.GetLogger(typeof(BasePageAssemblyInstruction));

        /// <summary>
        /// Collection of FieldFilter delegates for Web analytics fields.
        /// </summary>
        private Dictionary<string, WebAnalyticsDataPointDelegate> _webAnalyticsFieldFilterDelegates = new Dictionary<string, WebAnalyticsDataPointDelegate>();
        private WebAnalyticsSettings webAnalyticsSettings = null;
        private Dictionary<string, UrlFilterDelegate> _TranslationFilterDelegates = new Dictionary<string, UrlFilterDelegate>();

        /// <summary>
        /// Gets the translation filter delegates.
        /// </summary>
        /// <value>The translation filter delegates.</value>
        private Dictionary<string, UrlFilterDelegate> TranslationFilterDelegates
        {
            get
            {
                return _TranslationFilterDelegates;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds an appropriate FieldFilter to the given PageAssemblyInstruction object, based on the Tag's
        /// current settings.
        /// </summary>
        /// <param name="pai">The IPageAssemblyInstruction object to receive the new FieldFilter.</param>
        private void RegisterSocialMetaTagFieldFilter(IPageAssemblyInstruction pai, SocialMetaTagData socialMetaTag)
        {
            // add a field filter for each tag
            pai.AddFieldFilter(socialMetaTag.Id, (name, data) =>
            {
                string content = String.Empty;
                switch (socialMetaTag.Source)
                {
                    case SocialMetaTagSources.field:
                        content = pai.GetField(socialMetaTag.Content);
                        break;
                    case SocialMetaTagSources.url:
                        content = pai.GetUrl(socialMetaTag.Content).ToString();
                        break;
                    // both literal types just use the content directly
                    default:
                        content = socialMetaTag.Content;
                        break;
                }

                data.Value = content;
            });
        }

        #endregion

        #region Protected Members
        /// <summary>
        /// This method intialize the state of the base object or peform tasks that are 
        /// applicable to all derived class object.
        /// </summary>
        public virtual void Initialize()
        {
            IPageAssemblyInstruction pgInst = ((IPageAssemblyInstruction)this);

            // field filters
            pgInst.AddFieldFilter("invokedFrom", (name, field) =>
            {
                field.Value = String.Empty;
            });

            pgInst.AddFieldFilter("language", (name, field) =>
            {
                string languageValue = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                field.Value = languageValue;
            });

            // URL filters 
            pgInst.AddUrlFilter("RootPrettyURL", (name, url) =>
            {
                url.SetUrl(pgInst.GetUrl("CurrentURL").ToString());
                // This is  hack to fix the RootPrettyURL. If  this content type is 
                // rx:pdqCancerInfoSummary then remove the 'patient' or 'healthprofessional' from
                // the pretty url
                string prettyUrl = pgInst.GetUrl("PrettyURL").ToString().ToLower();
                if (ContentItemInfo != null && ContentItemInfo.ContentItemType == "rx:pdqCancerInfoSummary")
                {
                    int verIndex = prettyUrl.LastIndexOf("/patient");
                    if (verIndex == -1)
                        verIndex = prettyUrl.LastIndexOf("/healthprofessional");
                    if (verIndex != -1)
                        prettyUrl = prettyUrl.Substring(0, verIndex);
                }
                url.SetUrl(prettyUrl);
            });

            pgInst.AddUrlFilter("BookMarkShareUrl", (name, url) =>
            {
                url.SetUrl(pgInst.GetUrl("CurrentURL").ToString());
            });

            pgInst.AddUrlFilter("EmailUrl", (name, url) =>
            {
                url.SetUrl(pgInst.GetUrl("CurrentURL").ToString());
            });

            pgInst.AddUrlFilter("PostBackURL", (name, url) =>
            {
                url.SetUrl(pgInst.GetUrl("CurrentURL") + "?" + HttpContext.Current.Request.QueryString);
            });

        }

        /// <summary>
        /// This protected method registers all delegates for web analytics.
        /// </summary>
        /// <param name="webAnalyticFieldName">The key or the name of the datapoint</param>
        /// <param name="filter">The actual delegate callback which will modify the FieldFilterData object</param>
        protected void SetWebAnalytics(string webAnalyticFieldName, WebAnalyticsDataPointDelegate filter)
        {
            if (string.IsNullOrEmpty(webAnalyticFieldName))
                throw new ArgumentException("The webAnalyticFieldName parameter may not be null or empty.");

            string fieldNameKey = webAnalyticFieldName;

            if (_webAnalyticsFieldFilterDelegates.ContainsKey(fieldNameKey) == false)
                _webAnalyticsFieldFilterDelegates.Add(fieldNameKey, filter);
            else
                _webAnalyticsFieldFilterDelegates[fieldNameKey] += filter;
        }

        protected WebAnalyticsSettings WebAnalyticsSettings
        {
            get
            {
                if (webAnalyticsSettings == null)
                    webAnalyticsSettings = new WebAnalyticsSettings();
                return webAnalyticsSettings;
            }
        }

        protected virtual string GetEmailUrl()
        {
            string popUpemailUrl = "";

            string title = ((IPageAssemblyInstruction)this).GetField("long_title");
            title = System.Web.HttpUtility.UrlEncode(Strings.StripHTMLTags(title.Replace("&#153;", "__tm;")));

            string emailUrl = ((IPageAssemblyInstruction)this).GetUrl("EmailUrl").ToString();
            string invokedFrom = ((IPageAssemblyInstruction)this).GetField("invokedFrom");

            if (!string.IsNullOrEmpty(invokedFrom))
                invokedFrom = "&invokedFrom=" + invokedFrom;

            if ((Strings.Clean(emailUrl) != null) && (Strings.Clean(emailUrl) != ""))
            {
                popUpemailUrl = "/common/popUps/PopEmail.aspx?title=" + title + invokedFrom + "&docurl=" + System.Web.HttpUtility.UrlEncode(emailUrl.Replace("&", "__amp;")) + "&language=" + PageAssemblyContext.Current.PageAssemblyInstruction.Language;
                popUpemailUrl = popUpemailUrl + HashMaster.SaltedHashURL(HttpUtility.UrlDecode(title) + emailUrl);
            }
            return popUpemailUrl;
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// This method returns the web analytics settings for Event, Props and eVars data points.
        /// </summary>
        public virtual WebAnalyticsSettings GetWebAnalytics()
        {
            // Enumerate _webAnalyticsFieldFilterDelegates , so each delagate can be executed.
            foreach (KeyValuePair<string, WebAnalyticsDataPointDelegate> kvDel in _webAnalyticsFieldFilterDelegates)
            {
                FieldFilterData fieldData = null;
                if (Enum.IsDefined(typeof(WebAnalyticsOptions.Events), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    // Execute the delegate so we can get the value of the field
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Events.Add((WebAnalyticsOptions.Events)Enum.Parse(typeof(WebAnalyticsOptions.Events), kvDel.Key), fieldData.Value);
                }
                else if (Enum.IsDefined(typeof(WebAnalyticsOptions.eVars), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Evars.Add((WebAnalyticsOptions.eVars)Enum.Parse(typeof(WebAnalyticsOptions.eVars), kvDel.Key), fieldData.Value);
                }
                else if (Enum.IsDefined(typeof(WebAnalyticsOptions.Props), kvDel.Key))
                {
                    fieldData = new FieldFilterData();
                    kvDel.Value(fieldData);
                    this.WebAnalyticsSettings.Props.Add((WebAnalyticsOptions.Props)Enum.Parse(typeof(WebAnalyticsOptions.Props), kvDel.Key), fieldData.Value);
                }
            }

            return this.WebAnalyticsSettings;
        }

        /// <summary>
        /// Register all site wide field filters for Web Analytics.
        /// </summary>
        protected virtual void RegisterWebAnalyticsFieldFilters()
        {
            IPageAssemblyInstruction pgInst = ((IPageAssemblyInstruction)this);

            // Add root pretty URL 
            SetWebAnalytics(WebAnalyticsOptions.Props.prop3.ToString(), wbField =>
            {
                wbField.Value = pgInst.GetUrl("RootPrettyURL").ToString();
            });
        }

        /// <summary>
        /// Creates field filters for the given SocialMetadata object.
        /// </summary>
        /// <param name="pai">An available PageAssemblyInstruction object to receive the field filters.</param>
        /// <param name="socialMetadata">The SocialMetadata object to provide the fields.</param>
        protected virtual void RegisterSocialMetadataFieldFilters(IPageAssemblyInstruction pai, SocialMetadata socialMetadata)
        {
            try
            {
                // check provided PageAssemblyInstruction
                if (pai == null)
                {
                    log.Warn("RegisterSocialMetadataFieldFilters(): null PageAssemblyInstruction provided.");

                    return;
                }

                // add commenting available field filter
                pai.AddFieldFilter("is_commenting_available", (name, data) =>
                {
                    data.Value = socialMetadata.IsCommentingAvailable.ToString();
                });

                // add field filters for any tags
                if (socialMetadata.Tags != null)
                {
                    foreach (SocialMetaTagData tag in socialMetadata.Tags)
                    {
                        RegisterSocialMetaTagFieldFilter(pai, tag);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("InitializeFieldFilters(): Exception encountered while initializing field filters.", e);
            }
        }

        /// <summary>
        /// Provides a list of all SocialMetaTag objects defined for the current assembly.
        /// </summary>
        /// <returns>A potentially-empty array of SocialMetaTag objects.</returns>
        protected SocialMetaTag[] GenerateSocialMetaTags(SocialMetaTagData[] SocialMetaTagData)
        {
            return (from datum in SocialMetaTagData
                    select new SocialMetaTag(datum)).ToArray();
        }

        #endregion

        #region Public
        public BasePageAssemblyInstruction()
        {
            Translations = new Translations();
        }

        public ContentDates ContentDates
        {
            get
            {
                ContentDates contentDates = null;
                if (this is SinglePageAssemblyInstruction)
                    contentDates = ((SinglePageAssemblyInstruction)this).ContentDates;
                else if (this is MultiPageAssemblyInstruction)
                    contentDates = ((MultiPageAssemblyInstruction)this).ContentDates;
                return contentDates;
            }
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public ContentItemInfo ContentItemInfo { get; set; }


        /// <summary>
        /// Gets or sets the page metadata.
        /// </summary>
        /// <value>The page metadata.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Translations Translations { get; set; }

        /// <summary>
        /// Gets the URL referenced by "String" with all translation URL filters.
        /// </summary>
        /// <param name="urlType"></param>
        /// <returns></returns>
        public NciUrl GetTranslationUrl(string urlType)
        {
            NciUrl nciTranslationUrl = new NciUrl();
            string linkTypeKey = urlType.ToLower();

            if (TranslationFilterDelegates.ContainsKey(linkTypeKey) == true)
            {
                UrlFilterDelegate translationFilterLinkDelegate = TranslationFilterDelegates[linkTypeKey];
                translationFilterLinkDelegate(linkTypeKey, nciTranslationUrl);
            }
            return nciTranslationUrl;
        }

        /// <summary>
        /// Adds a translation URL filter which modifies the URL referenced by "string" when GetTranslationUrl is called.
        /// </summary>
        /// <param name="translationType">Pretty translation URL</param>
        /// <param name="fieldFilter"></param>
        public void AddTranslationFilter(string translationType, UrlFilterDelegate fieldFilter)
        {
            if (string.IsNullOrEmpty(translationType))
                throw new ArgumentException("The urlType parameter may not be null or empty.");

            string linkTypeKey = translationType.ToLower();

            if (TranslationFilterDelegates.ContainsKey(linkTypeKey) == false)
            {
                TranslationFilterDelegates.Add(linkTypeKey, fieldFilter);
            }
            else
            {
                TranslationFilterDelegates[linkTypeKey] += fieldFilter;
            }
        }

		/// <summary>
        /// This property returns the keys which represent the available translations for this content item. 
        /// </summary>
        /// <value>A string array which are the keys to the translations.</value>
        public string[] TranslationKeys
        {
            get
            {
                ArrayList keysList = new ArrayList();
                // Enumerate the Files and set a URL filter.
                foreach (TranslationMetaTag tmt in Translations.Tags)
                {
                    keysList.Add(tmt.Locale.ToLower());
                    AddTranslationFilter(tmt.Locale.ToLower(), (name, url) =>
                    {
                        foreach (TranslationMetaTag tmtu in Translations.Tags)
                        {
                            if (tmtu.Locale.ToLower() == name.ToLower())
                                url.SetUrl(tmtu.Url);
                        }
                    });
                }
                return (string[])keysList.ToArray(typeof(string));
            }
        }

        /// <summary>
        /// Registers the translation filters.
        /// </summary>
        public virtual void RegisterTranslationFilters()
        {
            // Add translations using AddTranslationFilter() instead of AddUrlFilter()
            if (Translations.Tags != null)
            {
                AddTranslationFilter("TranslationUrls", (name, url) =>
                {
                    url.SetUrl("/", true);
                });

                foreach (TranslationMetaTag tmt in Translations.Tags)
                {
                    AddTranslationFilter(tmt.Locale.ToLower(), (name, url) =>
                    {
                        foreach (TranslationMetaTag tmtu in Translations.Tags)
                        {
                            if (tmtu.Locale.ToLower() == name.ToLower())
                                url.SetUrl(tmtu.Url);
                        }
                    });
                } // End foreach
            } // End if-statement for translations
        } // End register method
		#endregion
		
	}
}
