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
    public class TrialListingPageInfo
    {

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
        /// The default number of search result items per page.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int DefaultItemsPerPage { get; set; }

        /// <summary>
        /// The JSON body used for a request. 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string JSONBodyRequest { get; set; }

        /// <summary>
        /// Minimum number of results to return.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ListingMinResults { get; set; }

        /// <summary>
        /// Maximum number of results to return.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int ListingMaxResults { get; set; }

        /// <summary>
        /// HTML block for page with no trials
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string NoTrialsHTML { get; set; }


    }
}
