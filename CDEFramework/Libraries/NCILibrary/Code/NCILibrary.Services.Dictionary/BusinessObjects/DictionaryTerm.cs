using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    public class DictionaryTerm
    {
        public DictionaryTerm()
        {
            // Guarantee that the arrays are never null.
            Aliases = new Alias[] { };
            images = new String[] { };
        }

        /// <summary>
        ///  The term's ID
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// The term's name
        /// </summary>
        public String term { get; set; }

        /// <summary>
        /// Data structure describing how a dictionary term is pronounced.
        /// </summary>
        public Pronunciation pronunciation { get; set; }

        /// <summary>
        /// The date the term was first published
        /// </summary>
        public String dateFirstPublished { get; set; }

        /// <summary>
        /// The date the term was last modified.
        /// </summary>
        public String dateLastModified { get; set; }

        /// <summary>
        /// Possibly empty array of strings containing the URLs of images associated with this definition.
        /// </summary>
        public String[] images { get; set; }

        /// <summary>
        /// Data structure containing the term's definition.
        /// </summary>
        public Definition Definition { get; set; }

        /// <summary>
        /// Possibly empty array of other names for the term.  (Only populated for drug dictionary)
        /// </summary>
        public Alias[] Aliases { get; set; }

        /// <summary>
        /// related links. (Cancer Term and Genetics only)
        /// </summary>
        public RelatedItems Related { get; set; }
    }
}
