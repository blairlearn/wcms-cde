using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class BasicCTSManager
    {
        /// <summary>
        /// Gets the API client this instance of a BasicCTSManager will use
        /// </summary>
        private ClinicalTrialsAPIClient Client { get; set; }

        /// <summary>
        /// Creates a new instance of a BasicCTSManager
        /// </summary>
        /// <param name="host">The hostname of the API</param>        
        public BasicCTSManager(string host)
        {
            //TODO: This should take in an interface for when we change this next time. 
            //that interface is what this wraps. (maybe)
            this.Client = new ClinicalTrialsAPIClient(host);
        }


        /// <summary>
        /// Returns a Clinical Trial
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ClinicalTrial Get(string id)
        {
            return Client.Get(id);
        }

        public ClinicalTrialsCollection Search(BaseCTSSearchParam searchParams) {
            throw new NotImplementedException();
        }

        public ClinicalTrialsCollection SearchByDisease(string diseaseCode)
        {
            throw new NotImplementedException();
        }

        public ClinicalTrialsCollection SearchByPhrase(string keyword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the Geo Location for a ZipCode
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public ZipLookup GetZipLookupForZip(string zipCode)
        {
            //TODO: Fix This
            return new ZipLookup()
            {
                PostalCode_ZIP = "20874",
                GeoCode = new GeoLocation(39.1349, -77.2922)
            };            
        }

        /// <summary>
        /// Gets the cancer type display name the user selected
        /// </summary>
        /// <param name="cancertypeid">The ID of the term</param>
        /// <param name="hashid">The hash id of the unique menu name</param>
        /// <returns></returns>
        public string GetCancerTypeDisplayName(string cancertypeid, string hashid)
        {
            //This may not be needed in phase 2
            return "NOT IMPLEMENTED";
        }


    }
}
