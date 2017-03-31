using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Client
{
    public class BestBetResult
    {
        /// <summary>
        /// Gets or sets the ID of this Best Bet
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the catgory name of this Best Bet
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the HTML of this Best Bet
        /// </summary>
        public string HTML { get; set; }

        /// <summary>
        /// Gets or sets the weighting of this category
        /// </summary>
        public int Weight { get; set; }
    }
}
