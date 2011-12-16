using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace CancerGov.CDR.TermDictionary
{
    /// <summary>
    /// Business layer that contains items specific the the TermDictionary query.
    /// This class can only be instantiated by passing all known values in the
    /// constructor.
    [Serializable]
    [DataContract]
    public class TermDictionaryDataItem
    {
        [DataMember]
        public int GlossaryTermID { get { return termID; } set { termID = value; } }
        [DataMember]
        public string TermName { get { return termName; } set { termName = value; } }
        [DataMember]
        public string OLTermName { get { return olTermName; } set { olTermName = value; } }
        [DataMember]
        public string TermPronunciation { get { return termPronunciation; } set { termPronunciation = value; } }
        [DataMember]
        public string DefinitionHTML { get { return definitionHTML; } set { definitionHTML = value; } }
        [DataMember]
        public string MediaHTML { get { return mediaHTML; } set { mediaHTML = value; } }
        [DataMember]
        public string MediaCaption { get { return mediaCaption; } set { mediaCaption = value; } }
        [DataMember]
        public int MediaID { get { return mediaID; } set { mediaID = value; } }
        [DataMember]
        public string AudioMediaHTML { get { return audioMediaHTML; } set { audioMediaHTML = value; } } 
        [DataMember]
        public string RelatedInfoHTML { get { return relatedInfoHTML; } set { relatedInfoHTML = value; } }

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
            string mediaCaption,
            int mediaID,
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
            this.MediaCaption = mediaCaption;
            this.MediaID = mediaID;
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
        private string mediaCaption;
        private int mediaID;
        private string audioMediaHTML;
        private string relatedInfoHTML;
        private List<TermDictionaryDataItem> previousNeighbors;
        private List<TermDictionaryDataItem> nextNeighbors;

    }
}
