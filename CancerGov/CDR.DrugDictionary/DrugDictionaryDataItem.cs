using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.CDR.DrugDictionary
{
    /// <summary>
    /// Business layer that contains items specific the the DrugDictionary query.
    /// This class can only be instantiated by passing all known values in the
    /// constructor.
    /// </summary>
    public class DrugDictionaryDataItem
    {
        // This is a test - 04/26/09
        public int TermID { get { return termID; } }
        public string PreferredName { get { return preferredName; } }
        public string OtherName { get { return otherName; } }
        public string DefinitionHTML { get { return definitionHTML; } }
        public string PrettyURL { get { return prettyURL; } }
        public List<DrugDictionaryDataItem> PreviousNeighbors
        {
            get { return previousNeighbors; }
        }
        public List<DrugDictionaryDataItem> NextNeighbors
        {
            get { return nextNeighbors; }
        }
        public Dictionary<string, List<string>> DisplayNames
        {
            get { return displayNames; }
        }




        /// <summary>
        /// Constructor requires all data fields to be passed
        /// </summary>
        /// <param name="termID"></param>
        /// <param name="preferredName"></param>
        /// <param name="otherName"></param>
        /// <param name="definitionHTML"></param>
        public DrugDictionaryDataItem(int termID, string preferredName, string otherName, string definitionHTML, string prettyURL)
        {
            this.termID = termID;
            this.preferredName = preferredName;
            this.otherName = otherName;
            this.definitionHTML = definitionHTML;
            this.prettyURL = prettyURL;
            previousNeighbors = new List<DrugDictionaryDataItem>();
            nextNeighbors = new List<DrugDictionaryDataItem>();
            displayNames = new Dictionary<string, List<string>>();
        }

        private int termID;
        private string preferredName;
        private string otherName;
        private string definitionHTML;
        private string prettyURL;
        private List<DrugDictionaryDataItem> previousNeighbors;
        private List<DrugDictionaryDataItem> nextNeighbors;
        private Dictionary<string, List<string>> displayNames;
    }
}
