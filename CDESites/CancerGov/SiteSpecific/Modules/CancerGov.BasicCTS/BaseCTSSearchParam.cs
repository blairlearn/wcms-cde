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
        public SearchTemplateDescriptor<T> SetSearchParams<T>(SearchTemplateDescriptor<T> descriptor) where T : class
        {


            //From starts at 0
            int from = 0;

            if (Page > 1)
            {
                from = Page * ItemsPerPage;
            }

            /**
             * Parameters For Templates:
             * 
             * age
             * gender
             * geopoint, radius
             * size
             * from
             * 
             * Template Specific Options:
             * 
             * searchstring
             * OR
             * cancertypeid
             */

            //Set the template.
            descriptor = descriptor
                .File(ESTemplateFile)
                .Params(pd =>
                    {
                        //Add params that are always set.
                        pd
                            .Add("size", this.ItemsPerPage)
                            .Add("from", from)
                            .Add("fields", "_source");

                        //Add optional parameters
                        if (this.Age != null &&  this.Age > 0)
                            pd.Add("age", this.Age);

                        if (!String.IsNullOrWhiteSpace(this.Gender))
                            pd.Add("gender", this.Gender);

                        if (ZipLookup != null)
                        {
                            //I don't like the code below, the template should make this clean for us. :(
                            pd
                                .Add("radius", this.ZipRadius.ToString() + "mi")
                                .Add("geopoint", this.ZipLookup.GeoCode.Lat.ToString() + "," + this.ZipLookup.GeoCode.Lon.ToString());
                        }

                        AddTemplateParams(pd);
                        
                        return pd;
                });

            return ModifySearchParams<T>(descriptor);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        protected abstract SearchTemplateDescriptor<T> ModifySearchParams<T>(SearchTemplateDescriptor<T> descriptor) where T : class;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        protected abstract void AddTemplateParams(FluentDictionary<string, object> paramdict);
    }
}
