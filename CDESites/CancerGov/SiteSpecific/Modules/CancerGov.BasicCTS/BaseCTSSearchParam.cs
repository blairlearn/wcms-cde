using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace CancerGov.ClinicalTrials.Basic
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
        /// ZipCode to use for filtering.  NOTE: This may be some sort of LocationFilter object if we need more than Zip.
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Should be Male or Female
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets and Sets the Visitor's Age
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets and Sets the name of the ElasticSearch template to use for this search
        /// </summary>
        public string ESTemplateFile { get; set; }


        //We may need some sorting options here too...

        /// <summary>
        /// Gets the body for the ElasticSearch Search/SearchTemplate request.
        /// </summary>
        /// <returns>A JSON string to be used in the Search/SearchTemplate request</returns>
        public virtual SearchDescriptor<T> ModifySearchParams<T>(SearchDescriptor<T> descriptor) where T : class
        {          
        
            //From starts at 0
            int from = 0;

            if (Page > 1)
            {
                from = Page * ItemsPerPage;
            }

            //Figure out From and Size from PageNum
            return descriptor.
                From(from)
                .Size(ItemsPerPage);

            //Add age, gender and zip if needed
        }

        /// <summary>
        /// Gets the body for the ElasticSearch Search/SearchTemplate request.
        /// </summary>
        /// <returns>A JSON string to be used in the Search/SearchTemplate request</returns>
        public virtual SearchTemplateDescriptor<T> ModifySearchParams<T>(SearchTemplateDescriptor<T> descriptor) where T : class
        {


            //From starts at 0
            int from = 0;

            if (Page > 1)
            {
                from = Page * ItemsPerPage;
            }

            /**
             * 
             * age
             * gender
             * geopoint
             * size
             * from
             * 
             * 
             * searchstring
             * OR
             * cancertypeid
             */ 


            //Figure out how to conditionally add all the other parameters
            return descriptor.File(ESTemplateFile);

            //Add age, gender and zip if needed
        }

    }
}
