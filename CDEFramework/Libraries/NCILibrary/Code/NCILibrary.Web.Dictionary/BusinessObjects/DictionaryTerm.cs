using System;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    public class DictionaryTerm
    {
        public DictionaryTerm()
        {
            // Guarantee that the arrays are never null.
            Aliases = new Alias[] { };
            Images = new ImageReference[] { };
        }

        /// <summary>
        ///  The Term's ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Term's name
        /// </summary>
        public String Term { get; set; }

        /// <summary>
        /// Data structure describing how a dictionary Term is pronounced.
        /// </summary>
        public Pronunciation Pronunciation { get; set; }

        /// <summary>
        /// Does the term have a pronuncation element?
        /// </summary>
        public Boolean HasPronunciation
        {
            get
            {
                return (Pronunciation != null)
                  && Pronunciation.HasPronunciation;
            }
        }

        /// <summary>
        /// The date the Term was first published
        /// </summary>
        public String DateFirstPublished { get; set; }

        /// <summary>
        /// The date the Term was last modified.
        /// </summary>
        public String DateLastModified { get; set; }

        /// <summary>
        /// Possibly empty array of strings containing the URLs of images associated with this definition.
        /// </summary>
        public ImageReference[] Images { get; set; }

        /// <summary>
        /// Data structure containing the Term's definition.
        /// </summary>
        public Definition Definition { get; set; }

        /// <summary>
        /// Possibly empty array of other names for the Term.  (Only populated for drug dictionary)
        /// </summary>
        public Alias[] Aliases { get; set; }

        /// <summary>
        /// related links. (Cancer Term and Genetics only)
        /// </summary>
        public RelatedItems Related { get; set; }
    }
}
