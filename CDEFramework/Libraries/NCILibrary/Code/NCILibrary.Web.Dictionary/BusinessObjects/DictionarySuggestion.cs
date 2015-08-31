using System;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    public class DictionarySuggestion
    {
        /// <summary>
        ///  The Term's ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The Term's name
        /// </summary>
        public String Term { get; set; }
    }
}
