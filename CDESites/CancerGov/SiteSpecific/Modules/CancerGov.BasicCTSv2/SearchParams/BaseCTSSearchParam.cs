using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Represents the shared parameters for any Basic CTS Search
    /// </summary>
    public abstract class BaseCTSSearchParam
    {
        public static readonly string GENDER_FEMALE = "Female";
        public static readonly string GENDER_MALE = "Male";

        int _pageNum = 1;
        int _itemsPerPage = 10;
        int _zipRadius = 100;

        /// <summary>
        /// Gets/Sets the Page Number
        /// </summary>
        public int Page
        {
            get { return _pageNum; }
            set { _pageNum = value; }
        }

        /// <summary>
        /// Gets/Sets the Page Number
        /// </summary>
        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set { _itemsPerPage = value; }
        }


        /// <summary>
        /// This contains both the ZipCode and Lat/Long for this search zip.
        /// </summary>
        public ZipLookup ZipLookup { get; set; }

        /// <summary>
        /// Gets and Sets Zip Code Search Radius
        /// </summary>
        public int ZipRadius
        {
            get { return _zipRadius; }
            set { _zipRadius = value; }
        }

        /// <summary>
        /// Should be Male or Female
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets and Sets the Visitor's Age
        /// </summary>
        public int? Age { get; set; }

    }
}
