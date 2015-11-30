using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.BestBets
{
    /// <summary>
    /// Defines a Best Bet Synonym
    /// <remarks>This is a hack until we can get a BestBetProvider.</remarks>
    /// </summary>
    public class BestBetSynonym
    {
        /// <summary>
        /// Should this synonym be treated as exact match or not
        /// </summary>
        [System.Xml.Serialization.XmlAttribute]
        public bool IsExactMatch { get; set; }

        /// <summary>
        /// The synonym text
        /// </summary>
        [System.Xml.Serialization.XmlText]
        public string Text { get; set; }
    }
}
