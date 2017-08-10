using System.Xml.Schema;
using System.Xml.Serialization;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// This class defines the properties of search result. Like the prettyUrl of the 
    /// search results page. This information should be made avaliable in the instruction 
    /// that defines the search page.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_BasicCTSPageInfo", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class BasicCTSPageInfo
    {
        /*private string _index = "clinicaltrials";
        private string _indexType = "trial";
        private string _clusterName = "cts";
        private string _templatePath = "~/VelocityTemplates/BasicCTSResults.vm";
        private string _searchUrl = "/about-cancer/treatment/clinical-trials/basic";
        private string _resultsUrl = "/about-cancer/treatment/clinical-trials/basic/view";
        private int _defaultItemsPerPage = 10;
        private int _defaultZipProximity = 50;*/


        /// <summary>
        /// The search index to use.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchIndex { get; set; }

        /// <summary>
        /// The type of the trial document.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string TrialIndexType { get; set; }

        /// <summary>
        /// The type of the GeoLocation document.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string GeoLocIndexType { get; set; }

        /// <summary>
        /// The type of the MenuTerm document.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string MenuTermIndexType { get; set; }

        /// <summary>
        /// The search cluster name.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchClusterName { get; set; }

        /// <summary>
        /// The path to the template to use for basic search.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string BasicSearchPageTemplatePath { get; set; }

        /// <summary>
        /// The path to the template to use for advanced search.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string AdvSearchPageTemplatePath { get; set; }


        /// <summary>
        /// The pretty url of the basic search page.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string BasicSearchPagePrettyUrl { get; set; }

        /// <summary>
        /// The pretty url of the advanced search page.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string AdvSearchPagePrettyUrl { get; set; }

        /// <summary>
        /// The path to the template to use.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ResultsPageTemplatePath { get; set; }

        /// <summary>
        /// The pretty url of the results page.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ResultsPagePrettyUrl { get; set; }

        /// <summary>
        /// The path to the template to use.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DetailedViewPageTemplatePath { get; set; }

        /// <summary>
        /// The pretty url of the detailed view page
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DetailedViewPagePrettyUrl { get; set; }

        /// <summary>
        /// The path to the print template to use.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PrintPageTemplatePath { get; set; }

        /// <summary>
        /// The default number of search result items per page.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int DefaultItemsPerPage { get; set; }

        /// <summary>
        /// The default zip code proximity.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int DefaultZipProximity { get; set; }

        /// <summary>
        /// The Elastic Search template to use for full text.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ESTemplateFullText { get; set; }

        /// <summary>
        /// The Elastic Search template to use for cancer types.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ESTemplateCancerType { get; set; }

        /// <summary>
        /// Flag to redirect if a selected trial is not active.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool RedirectIfInactive { get; set; }

        /// <summary>
        /// Filepaths for mapping files needed.
        /// </summary>
        [XmlArray(ElementName = "MappingFiles", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("MappingFile", typeof(string), Form = XmlSchemaForm.Unqualified)]
        public string[] MappingFiles { get; set; }
    }
}
