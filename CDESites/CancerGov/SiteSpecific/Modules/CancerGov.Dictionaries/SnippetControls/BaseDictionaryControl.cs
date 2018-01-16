using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Globalization;

using NCI.Web.CDE;
using CancerGov.Dictionaries.Configuration;
using Microsoft.Security.Application;
using CancerGov.Text;

namespace CancerGov.Dictionaries.SnippetControls
{
    public class BaseDictionaryControl : UserControl
    {
        protected DictionarySearchBlock dictionarySearchBlock;
    
        public IPageAssemblyInstruction PageInstruction { get; set; }

        public DictionaryConfig DictionaryConfiguration { get; set; }

        public BaseDictionaryRouter DictionaryRouter { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.dictionarySearchBlock != null)
            {
                this.dictionarySearchBlock.listBoxBaseUrl = this.DictionaryRouter.GetBaseURL();
                this.dictionarySearchBlock.listBoxShowAll = false;
                this.dictionarySearchBlock.DisplayHelpLink = false;
                this.dictionarySearchBlock.FormAction = this.DictionaryRouter.GetSearchUrl();

                if(!String.Equals(this.DictionaryConfiguration.DictionaryType, "term"))
                {
                    this.dictionarySearchBlock.listBoxShowAll = true;
                }

                if(string.Equals(this.DictionaryConfiguration.DictionaryType, "drug"))
                {
                    this.dictionarySearchBlock.DisplayHelpLink = true;
                }
            }
        }

        /// <summary>
        /// Gets the definition param from the URL, using the DictionaryRouter's current app path.
        /// </summary>
        public string GetDefinitionParam()
        {
            List<string> path = this.DictionaryRouter.GetCurrAppPath().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            if (path.Count > 0 && path[0].Equals("def"))
            {
                string param = Sanitizer.GetSafeHtmlFragment(path[1]);
                param = Strings.Clean(path[1]);

                return param;
            }
            else if (path.Count > 2)
            {
                // If path extends further than /search or /def/<term>, raise a 400 error
                NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("Dictionary", 400, "Invalid parameters for dictionary");
                return null;
            }

            return null;
        }

        /// <summary>
        /// Gets the friendly name of the given CDRID param, if it exists.
        /// Otherwise, returns the CDRID param.
        /// </summary>
        public string GetFriendlyName(string cdrId)
        {
            // Get CDRID to friendly name mappings
            string dictionaryMappingFilepath = null;

            dictionaryMappingFilepath = this.DictionaryConfiguration.Files.Single(a => a.Locale == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName).Filepath;

            if (!string.IsNullOrEmpty(dictionaryMappingFilepath))
            {
                TerminologyMapping map = TerminologyMapping.GetMappingForFile(dictionaryMappingFilepath);

                // If pretty name is in label mappings, set CDRID
                if (map.MappingContainsCDRID(cdrId))
                {
                    return map.GetFriendlyNameFromCDRID(cdrId);
                }
            }

            return cdrId;
        }

        /// <summary>.
        /// Sets the DictionaryAnalyticsType for analytics
        /// </summary>
        public class DictionaryAnalyticsType
        {
            private DictionaryAnalyticsType(string name) { Name = name; }

            public string Name { get; set; }

            public static DictionaryAnalyticsType Term { get { return new DictionaryAnalyticsType("CancerTerms"); } }
            public static DictionaryAnalyticsType Genetics { get { return new DictionaryAnalyticsType("Genetics"); } }
            public static DictionaryAnalyticsType Drug { get { return new DictionaryAnalyticsType("Drug"); } }
        }
    }
}
