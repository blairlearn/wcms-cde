using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// Extensions for filtering Locations
    /// </summary>
    public static class TrialSiteLocationFilterExtension
    {

        /// <summary>
        /// Gets a list of the trial sites filtered by the location in the supplied CTSSearchParams
        /// </summary>
        /// <param name="trial"></param>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        public static IEnumerable<ClinicalTrial.StudySite> FilterSitesByLocation(this ClinicalTrial trial, CTSSearchParams searchParams)
        {
            IEnumerable<ClinicalTrial.StudySite> rtnSites = trial.Sites;

            switch (searchParams.Location)
            {
                case LocationType.AtNIH:
                    {
                        rtnSites = rtnSites.Where(s => s.PostalCode == "20892");
                        break;
                    }
                case LocationType.CountryCityState: 
                    {

                        CountryCityStateLocationSearchParams locParams = (CountryCityStateLocationSearchParams)searchParams.LocationParams;
                        
                        if (locParams.IsFieldSet(FormFields.Country))
                        {
                            rtnSites = rtnSites.Where(s => StringComparer.CurrentCultureIgnoreCase.Equals(s.Country, locParams.Country));
                        }

                        if (locParams.IsFieldSet(FormFields.City))
                        {
                            rtnSites = rtnSites.Where(s => StringComparer.CurrentCultureIgnoreCase.Equals(s.City, locParams.City));
                        }

                        if (locParams.IsFieldSet(FormFields.State))
                        {
                            var states = locParams.State.Select(s => s.Key); //Get Abbreviations
                            rtnSites = rtnSites.Where(s => states.Contains(s.StateOrProvinceAbbreviation));
                        }

                        break;
                    }
                case LocationType.Zip:
                    {
                        ZipCodeLocationSearchParams locParams = (ZipCodeLocationSearchParams)searchParams.LocationParams;

                        rtnSites = rtnSites.Where(site =>
                                    site.Coordinates != null &&
                                    locParams.GeoLocation.DistanceBetween(new GeoLocation(site.Coordinates.Latitude, site.Coordinates.Longitude)) <= locParams.ZipRadius &&
                                    site.Country == "United States"
                        );

                        break;
                    }
                default:
                    {
                        //Basically we can't/shouldn't filter.
                        break;
                    }
            }

            //Now that we have the sites filtered, now we need to sort.
            return rtnSites.SortAlpha();
        }


        /// <summary>
        /// Sorts a list of study sites alphabetically based on Country, state, city, name, USA, then Canada, then the world.
        /// </summary>
        /// <param name="sites">List of sites to sort.</param>
        /// <returns></returns>
        private static IEnumerable<ClinicalTrial.StudySite> SortAlpha(this IEnumerable<ClinicalTrial.StudySite> sites)
        {
            var usaSites = sites.Where(s => s.Country == "United States").OrderBy(s => s.StateOrProvince).ThenBy(s => s.City).ThenBy(s => s.Name);
            var canadaSites = sites.Where(s => s.Country == "Canada").OrderBy(s => s.StateOrProvince).ThenBy(s => s.City).ThenBy(s => s.Name);
            var otherSites = sites.Where(s => s.Country != "United States" && s.Country != "Canada").OrderBy(s => s.City).ThenBy(s => s.Name);

            return usaSites.Union(canadaSites).Union(otherSites);            
        }
        
    }
}
