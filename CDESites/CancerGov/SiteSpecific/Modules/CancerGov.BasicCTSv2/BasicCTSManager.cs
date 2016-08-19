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

        private static readonly string[] ActiveTrialStatuses = {
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
            /// "Withdrawn"
        };

        //ActiveRecruitmentStatuses (filter study sites) query for open or not based on trial statuses to lower
        private static readonly string[] ActiveRecruitmentStatuses = {
            // These statuses appear in results:
            "active",
            "approved", 
            "enrolling_by_invitation",
            "in_review",
            "temporarily_closed_to_accrual"
            // These statuses DO NOT appear in results:
            /// "closed_to_accrual",
            /// "completed",
            /// "administratively_complete",
            /// "closed_to_accrual_and_intervention",
            /// "withdrawn"
        };

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
            filterCriteria.Add("current_trial_status", ActiveTrialStatuses);

            if (searchParams.ZipLookup != null)
            {
                filterCriteria.Add("sites.org_coordinates_lat", searchParams.ZipLookup.GeoCode.Lat);
                filterCriteria.Add("sites.org_coordinates_lon", searchParams.ZipLookup.GeoCode.Lon);
                filterCriteria.Add("sites.org_coordinates_dist", searchParams.ZipRadius.ToString() + "mi");
                filterCriteria.Add("sites.recruitment_status", ActiveRecruitmentStatuses);
            }

            //Add Age Filter
            //<field>_gte, <field>_lte

            //Add Gender Filter
            if (!string.IsNullOrWhiteSpace(searchParams.Gender))
            {
                filterCriteria.Add("eligibility.structured.gender", searchParams.Gender);
            }

            //Add phrase if this is a phrase search
            if (searchParams is PhraseSearchParam)
            {
                filterCriteria.Add("_fulltext", ((PhraseSearchParam)searchParams).Phrase);
            }
            else if (searchParams is CancerTypeSearchParam)
            {
                //This is now an array of codes.
                filterCriteria.Add("diseases.nci_thesaurus_concept_id", ((CancerTypeSearchParam)searchParams).CancerTypeIDs);
            }

            //TODO: Actually handle search criteria
            ClinicalTrialsCollection rtnResults = Client.List(
                size: searchParams.ItemsPerPage,
                from: from,
                includeFields: new string[] {
                    "nct_id",
                    "brief_title",
                    "sites.org_name",
                    "sites.org_postal_code",
                    "eligibility.structured",
                    "current_trial_status",
                    "sites.org_country",
                    "sites.org_state_or_province",
                    "sites.org_city",
                    "sites.org_coordinates",
                    "sites.recruitment_status",
                    "diseases"
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
            return ActiveRecruitmentStatuses.Any(status => status.ToLower() == site.RecruitmentStatus.ToLower());
        }


        /// <summary>
        /// Gets the Geo Location for a ZipCode
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public ZipLookup GetZipLookupForZip(string zipCode)
        {
            // Setting the default latitude and longitude values for the middle of Antarctica.
            // This way, if a user enters a properly formatted, but invalid zip, we won't hit a 
            // null exception error when retrieving the coordinates.
            double latitude = -89.9999;
            double longitude = -89.9999;

            // Look up our ZipCodeGeoEntry object from the JSON reference file and pass in the 
            // coordinates if the mapping exists.
            ZipCodeGeoEntry zipEntry = ZipCodeGeoLookup.GetZipCodeGeoEntry(zipCode);
            if (zipEntry != null)
            { 
                if (!Double.IsNaN(zipEntry.Latitude) && !Double.IsNaN(zipEntry.Longitude))
                {
                    latitude = zipEntry.Latitude;
                    longitude = zipEntry.Longitude;
                }
            }

            return new ZipLookup()
            {
                PostalCode_ZIP = zipCode,
                GeoCode = new GeoLocation(latitude, longitude)
            };
        }

        /// <summary>
        /// Gets the cancer type display name the user selected
        /// </summary>
        /// <param name="cancertypeids">The IDs of the term</param>
        /// <param name="key">The term key of the unique menu name</param>
        /// <returns></returns>
        public string GetCancerTypeDisplayName(string[] cancertypeids, string key)
        {
            string displayName = string.Empty;

            if (!String.IsNullOrWhiteSpace(key))
            {
                Term term = Client.GetTerm(key);
                if (term == null)
                {
                    return GetCancerTypeDisplayName(cancertypeids, null);
                }
                else
                {
                    return term.DisplayText;
                }
            }
            else
            {
                //If we did not have a key, OR we did not find an entry
                //in either case we need to try and find it by ID.
                Dictionary<string, object> filterCriteria = new Dictionary<string, object>();

                //If we have a key, then look it up by key
                filterCriteria.Add("codes", cancertypeids);

                TermCollection rtnResults = Client.Terms(
                    size: 100,
                    from: 0,
                    searchParams: filterCriteria
                );

                if (rtnResults.TotalResults > 0)
                {
                    //This is hacky!!
                    //TODO: Clean this up and check for errors
                    //TODO: Some terms may have 2 IDs, this query will return anything with either term,
                    //not just the terms with both.  We should iterate through the results and find the first
                    //with both.
                    return rtnResults.Terms[0].DisplayText;
                }

            }

            return string.Empty; //Nothing found
        }


    }
}
