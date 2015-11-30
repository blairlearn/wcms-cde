using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.BestBets
{
    /// <summary>
    /// /// <remarks>This is a hack until we can get a BestBetProvider.</remarks>
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("BestBetsCategory", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class BestBetCategory
    {
        /// <summary>
        /// Gets the Category ID of this bestbet
        /// </summary>
        [System.Xml.Serialization.XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets the Category Name of this BestBetResult
        /// </summary>
        [System.Xml.Serialization.XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CategoryName { get; set; }

        /// <summary>
        /// Should the category name be an exact match?
        /// </summary>
        [System.Xml.Serialization.XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool IsExactMatch { get; set; }

        /// <summary>
        /// The 2 letter language code for this category
        /// </summary>
        [System.Xml.Serialization.XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <summary>
        /// Get the display HTML of the item
        /// </summary>
        [System.Xml.Serialization.XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CategoryDisplay { get; set; }

        /// <summary>
        /// Should this item Display or Not?
        /// </summary>
        [System.Xml.Serialization.XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Display { get; set; }

        /// <summary>
        /// Gets an array of synonyms that if matched would mean this best bet should display
        /// </summary>
        [System.Xml.Serialization.XmlArray("IncludeSynonyms", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItem("synonym", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BestBetSynonym[] IncludeSynonyms { get; set; }

        /// <summary>
        /// Gets an array of synonyms that if matched would mean this best bet should display
        /// </summary>
        [System.Xml.Serialization.XmlArray("ExcludeSynonyms", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItem("synonym", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BestBetSynonym[] ExcludeSynonyms { get; set; }



    }
}
