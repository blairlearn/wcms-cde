using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.CDR.TermDictionary
{
    /// <summary>
    /// Business layer that contains items specific the the TermDictionary query.
    /// This class can only be instantiated by passing all known values in the
    /// constructor.
    public class TermDictionaryDataItem
    {
        public int GlossaryTermID { get { return termID; } }
        public string TermName { get { return termName; } }
        public string OLTermName { get { return olTermName; } }
        public string TermPronunciation { get { return termPronunciation; } }
        public string DefinitionHTML { get { return definitionHTML; } }
        public string MediaHTML { get { return mediaHTML; } }
        public string AudioMediaHTML { get { return audioMediaHTML; } }
        public string RelatedInfoHTML { get { return relatedInfoHTML; } }

        public List<TermDictionaryDataItem> PreviousNeighbors
        {
            get { return previousNeighbors; }
        }
        public List<TermDictionaryDataItem> NextNeighbors
        {
            get { return nextNeighbors; }
        }

        /// <summary>
        /// Constructor requires all data fields to be passed
        /// </summary>
        /// <param name="termID"></param>
        /// <param name="termName"></param>
        /// <param name="spanishTermName"></param>
        /// <param name="termPronunciation"></param>
        /// <param name="definitionHTML"></param>
        /// <param name="mediaHTML"></param>
        public TermDictionaryDataItem(
            int termID,
            string termName,
            string olTermName,
            string termPronunciation,
            string definitionHTML,
            string mediaHTML,
            string audioMediaHTML,
            string relatedInfoHTML
        )
        {
            this.termID = termID;
            this.termName = termName;
            this.olTermName = olTermName;
            this.termPronunciation = termPronunciation;
            this.definitionHTML = definitionHTML;
            this.mediaHTML = mediaHTML;
            this.audioMediaHTML = audioMediaHTML;
            this.relatedInfoHTML = relatedInfoHTML;
            previousNeighbors = new List<TermDictionaryDataItem>();
            nextNeighbors = new List<TermDictionaryDataItem>();
        }

        private int termID;
        private string termName;
        private string olTermName;
        private string termPronunciation;
        private string definitionHTML;
        private string mediaHTML;
        private string audioMediaHTML;
        private string relatedInfoHTML;
        private List<TermDictionaryDataItem> previousNeighbors;
        private List<TermDictionaryDataItem> nextNeighbors;

    }
}
