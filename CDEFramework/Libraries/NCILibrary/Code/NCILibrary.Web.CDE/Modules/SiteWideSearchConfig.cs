using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NCI.Web.CDE.Modules
{
    /// <summary>
    /// This class represents parameter fields used in search.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_SiteWideSearch", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class SiteWideSearchConfig
    {
        /// <summary>
        /// Search collection field -
        /// Possible values are - CancerGovEnglish, CancerGovSpanish
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchCollection { get; set; }

        /// <summary>
        /// Result Title Text - for all the titles returned from Elastic Search we need to remove the ' - National Cancer Institute' text
        /// e.g. This title 'Drugs Approved for Bladder Cancer - National Cancer Institute'
        /// should be 'Drugs Approved for Bladder Cancer'
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ResultTitleText { get; set; }

    }
}
