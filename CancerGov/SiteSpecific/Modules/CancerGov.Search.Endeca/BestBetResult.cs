using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.Modules.Search.Endeca
{
    public class BestBetResult : List<BestBetListItem>
    {
        private string _catName;

        /// <summary>
        /// Gets the list items of this Best Bet Result.
        /// </summary>
        /// <remarks>
        /// This is to be used if this BestBetResult is being databound to a repeater item.  We can repeat
        /// the list items of this best bet by using this property.
        /// </remarks>
        public BestBetListItem[] ListItems
        {
            get
            {
                return this.ToArray();
            }
        }

        /// <summary>
        /// Gets the Category Name of this BestBetResult
        /// </summary>
        public string CategoryName
        {
            get { return _catName; }
        }

        public BestBetResult(string catName)
        {
            _catName = catName;
        }
    }
}
