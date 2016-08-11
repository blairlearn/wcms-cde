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
            ClinicalTrial trial = Client.Get(id);
            RemoveNonRecruitingSites(trial);
            return trial;
        }

        /// <summary>
        /// Performs a search against the Clinical Trials API
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public ClinicalTrialsCollection Search(BaseCTSSearchParam searchParams) {
            
            //Set page
            //Set size
            //Get only the fields we want
            //Get only "active" statuses
            //Then get additional filters based on type of search.

            //From starts at 0
            int from = 0;

            if (searchParams.Page > 1)
            {
                from = searchParams.Page * searchParams.ItemsPerPage;
            }

            Dictionary<string, object> filterCriteria = new Dictionary<string, object>();

            //This is for only searching open trials.
            filterCriteria.Add("current_trial_status", new string[] {
                // These CTRP statuses appear in results:
                "Active",
                "Approved", 
                "Enrolling by Invitation",
                "In Review",
                "Temporarily Closed to Accrual",
                "Temporarily Closed to Accrual and Intervention"
                // These CTRP statuses DO NOT appear in results:
                /// "Administratively Complete",
                /// "Closed to Accrual",
                /// "Closed to Accrual and Intervention",
                /// "Complete",
                /// "Withdrawn",
            });

            if (searchParams.ZipLookup != null)
            {
                filterCriteria.Add("sites.org.coordinates_lat", searchParams.ZipLookup.GeoCode.Lat);
                filterCriteria.Add("sites.org.coordinates_lon", searchParams.ZipLookup.GeoCode.Lon);
                filterCriteria.Add("sites.org.coordinates_dist", "100mi");
                filterCriteria.Add("sites.recruitment_status", new string[] {
                    // TODO: make this a constant and update IsActivelyRecruiting() to check all valid values
                    // These CTRP statuses appear in results:
                    "Active",
                    "Approved", 
                    "Enrolling by Invitation",
                    "In Review",
                    "Temporarily Closed to Accrual",
                    "Temporarily Closed to Accrual and Intervention"
                    // These CTRP statuses DO NOT appear in results:
                    /// "Administratively Complete",
                    /// "Closed to Accrual",
                    /// "Closed to Accrual and Intervention",
                    /// "Complete",
                    /// "Withdrawn",
                });
            }


            //TODO: Actually handle search criteria
            ClinicalTrialsCollection rtnResults = Client.List(
                size: searchParams.ItemsPerPage,
                from: from,
                includeFields: new string[] {
                    "nct_id",
                    "brief_title",
                    "sites.org.name",
                    "sites.org.postal_code",
                    "eligibility.structured",
                    "current_trial_status",
                    "sites.recruitment_status"
                },
                searchParams: filterCriteria
            );

            foreach(ClinicalTrial trial in rtnResults.Trials)
            {
                RemoveNonRecruitingSites(trial);
            }

            return rtnResults;

        }

        private static void RemoveNonRecruitingSites(ClinicalTrial trial)
        {
            trial.Sites = new List<ClinicalTrial.StudySite>(trial.Sites.Where(site => IsActivelyRecruiting(site)));
        }

        private static bool IsActivelyRecruiting(ClinicalTrial.StudySite site)
        {
            return site.RecruitmentStatus == "Active";
        }


        /// <summary>
        /// Gets the Geo Location for a ZipCode
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public ZipLookup GetZipLookupForZip(string zipCode)
        {
            // Get the zip code dictionary entry object
			// TODO: test check for null values
            ZipCodeGeoEntry zipEntry = ZipCodeGeoLookup.GetZipCodeGeoEntry(zipCode);
            return new ZipLookup()
            {
                PostalCode_ZIP = zipCode,
                GeoCode = new GeoLocation(zipEntry.Latitude, zipEntry.Longitude)
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
