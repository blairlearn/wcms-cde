using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.BestBets
{
    /// <summary>
    /// Represents a Best Bet from Search
    /// </summary>
    public class BestBetResult
    {

        /// <summary>
        /// The category Id of this item
        /// </summary>
        public string CategoryID { get; set; }

        /// <summary>
        /// The name of this Best Bet Category
        /// </summary>
        public string CategoryName { get; set; }

        public override string ToString()
        {
            return "(" + this.CategoryID +") " + this.CategoryName; 
        }
    }
}
