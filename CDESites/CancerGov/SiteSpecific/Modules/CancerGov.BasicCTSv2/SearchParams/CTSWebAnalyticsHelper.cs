using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NCI.Web;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Helper class to create page-load analytics values for CTS pages.
    /// </summary>
    class CTSWebAnalyticsHelper
    {
        // Delegate definition so we can more cleanly list the parsers we will call.
        private delegate void AnalyticsParamsDelegate(List<string> paramsList, CTSSearchParams searchParams);
        private delegate void AnalyticsCancerInfoFieldsDelegate(List<string> paramsList, CTSSearchParams searchParams);
        private delegate void AnalyticsLocFieldsDelegate(List<string> paramsList, CTSSearchParams searchParams);
        private delegate void AnalyticsTrialDrugTreatFieldsDelegate(List<string> paramsList, CTSSearchParams searchParams);
        private delegate void AnalyticsPhaseIDInvOrgFieldsDelegate(List<string> paramsList, CTSSearchParams searchParams);
        
        // Member variables 
        private static AnalyticsParamsDelegate _analyticsParams;
        private static AnalyticsCancerInfoFieldsDelegate _cancerInfoFields;
        private static AnalyticsLocFieldsDelegate _locationFields;
        private static AnalyticsTrialDrugTreatFieldsDelegate _trialDrugTreatFields;
        private static AnalyticsPhaseIDInvOrgFieldsDelegate _phaseIDInvOrgFields;

        /// <summary>
        /// Static constructor to initialize.
        /// </summary>
        static CTSWebAnalyticsHelper() {

            _analyticsParams = (AnalyticsParamsDelegate)AddAllUsedParams;

            _phaseIDInvOrgFields = (AnalyticsPhaseIDInvOrgFieldsDelegate)AddAnalyticsTrialPhases +  //First param needs the cast.
                AddAnalyticsTrialIDs +
                AddAnalyticsInvestigator +
                AddAnalyticsLeadOrg;

            _cancerInfoFields = (AnalyticsCancerInfoFieldsDelegate)AddAnalyticsCancerType +
                AddAnalyticsSubTypes +
                AddAnalyticsStages +
                AddAnalyticsFindings +
                AddAnalyticsAge +
                AddAnalyticsKeyword;

                //AddAnalyticsGender +
                //AddAnalyticsLocation +
                //AddAnalyticsTrialTypes +
                //AddAnalyticsDrugs +
                //AddAnalyticsOtherTreatments +

        }

        /// <summary>
        /// Get a list of search parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns>A list of search parameter values</returns>
        public static List<String> GetAnalyticsParamsList(CTSSearchParams searchParams)
        {
            List<string> waParamsList = new List<string>();

            // Call each of our delegate methods to build out the parameter list
            _analyticsParams(waParamsList, searchParams);

            // Return the assembled list of params
            return waParamsList;
        }

        /// <summary>
        /// Get a list of main Cancer Type, Subtype, Stages, Findings, Age, and Keyword values.
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns>A list of search parameter values</returns>
        public static List<String> GetAnalyticsCancerInfoList(CTSSearchParams searchParams)
        {
            List<string> waFieldsList = new List<string>();

            // Call each of our delegate methods to build out the parameter list
            _cancerInfoFields(waFieldsList, searchParams);

            // Return the assembled list of params
            return waFieldsList;
        }

        /// <summary>
        /// Get a list of Trial Phase, Trial ID, Investigator, and Lead Organization values.
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns>A list of search parameter values</returns>
        public static List<String> GetAnalyticsPhaseIDInvOrgList(CTSSearchParams searchParams)
        {
            List<string> waFieldsList = new List<string>();

            // Call each of our delegate methods to build out the parameter list
            _phaseIDInvOrgFields(waFieldsList, searchParams);

            // Return the assembled list of params
            return waFieldsList;
        }


        #region Big dumb param function
        /// <summary>
        /// Build the list of query abbreviations from completed search fields.
        /// </summary>
        /// <param name="waList"></param>
        /// <param name="searchParams"></param>
        private static void AddAllUsedParams(List<string> waList, CTSSearchParams searchParams)
        {
            //Parameter t (Main Cancer Type)
            if (searchParams.IsFieldSet(FormFields.MainType))
            {
                waList.Add("t");
            }

            //Parameter st (SubTypes)
            if (searchParams.IsFieldSet(FormFields.SubTypes))
            {
                waList.Add("st");
            }

            //Parameter stg (Stages)
            if (searchParams.IsFieldSet(FormFields.Stages))
            {
                waList.Add("stg");
            }

            //Parameter fin (Findings)
            if (searchParams.IsFieldSet(FormFields.Findings))
            {
                waList.Add("fin");
            }

            // Parameter a (Age)
            if (searchParams.IsFieldSet(FormFields.Age))
            {
                waList.Add("a");
            }

            //Parameter q (Keyword/Phrase)
            if (searchParams.IsFieldSet(FormFields.Phrase))
            {
                waList.Add("q");
            }

            // Parameter g (Gender)
            if (searchParams.IsFieldSet(FormFields.Gender))
            {
                waList.Add("g");
            }

            // Parameter loc (Location, and AtNIH if loc=nih)
            if (searchParams.IsFieldSet(FormFields.Location))
            {
                waList.Add("loc");
                switch (searchParams.Location)
                {
                    case LocationType.Zip:
                        {
                            ZipCodeLocationSearchParams locParams = (ZipCodeLocationSearchParams)searchParams.LocationParams;
                            if (locParams.IsFieldSet(FormFields.ZipCode))
                            {
                                waList.Add("z");
                            }
                            if (locParams.IsFieldSet(FormFields.ZipRadius))
                            {
                                waList.Add("zp");
                            }
                            break;
                        }
                    case LocationType.CountryCityState:
                        {
                            CountryCityStateLocationSearchParams locParams = (CountryCityStateLocationSearchParams)searchParams.LocationParams;
                            if (locParams.IsFieldSet(FormFields.Country))
                            {
                                waList.Add("lcnty");
                            }
                            if (locParams.IsFieldSet(FormFields.State))
                            {
                                waList.Add("lst");
                            }
                            if (locParams.IsFieldSet(FormFields.City))
                            {
                                waList.Add("lcty");
                            }
                            break;
                        }
                    case LocationType.Hospital:
                        {
                            HospitalLocationSearchParams locParams = (HospitalLocationSearchParams)searchParams.LocationParams;
                            if (locParams.IsFieldSet(FormFields.Hospital))
                            {
                                waList.Add("hos");
                            }
                            break;
                        }
                    case LocationType.None:
                        {
                            waList.Remove("loc");
                            break;
                        }
                } // End switch
            } // End if

            // Parameter tt (Trial Type)
            if (searchParams.IsFieldSet(FormFields.TrialTypes))
            {
                waList.Add("tt");
            }

            //Parameter d (Drugs)
            if (searchParams.IsFieldSet(FormFields.Drugs))
            {
                waList.Add("d");
            }

            //Parameter i (Other treatments / interventions)
            if (searchParams.IsFieldSet(FormFields.OtherTreatments))
            {
                waList.Add("i");
            }

            // Parameter tp (Trial Phase)
            if (searchParams.IsFieldSet(FormFields.TrialPhases))
            {
                waList.Add("tp");
            }

            // Parameter tid (Trial IDs)
            if (searchParams.IsFieldSet(FormFields.TrialIDs))
            {
                waList.Add("tid");
            }

            // Parameter in (Investigator)
            if (searchParams.IsFieldSet(FormFields.Investigator))
            {
                waList.Add("in");
            }

            // Parameter lo (Lead Org)
            if (searchParams.IsFieldSet(FormFields.LeadOrg))
            {
                waList.Add("lo");
            }
        }

        #endregion

        #region Analytics param adders

        //Parameter t (Main Cancer Type)
        private static void AddAnalyticsCancerType(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.MainType))
            {
                value = string.Join(",", searchParams.MainType.Codes);
            }
            waList.Add(value);
        }

        //Parameter st (SubTypes)
        private static void AddAnalyticsSubTypes(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.SubTypes))
            {
                value = AddAnalyticsMultiTermFieldCodes(searchParams.SubTypes);
            }
            waList.Add(value);
        }

        //Parameter stg (Stages)
        private static void AddAnalyticsStages(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Stages))
            {
                value = AddAnalyticsMultiTermFieldCodes(searchParams.Stages);
            }
            waList.Add(value);
        }

        //Parameter fin (Findings)
        private static void AddAnalyticsFindings(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Findings))
            {
                value = AddAnalyticsMultiTermFieldCodes(searchParams.Findings);
            }
            waList.Add(value);
        }

        // Parameter a (Age)
        private static void AddAnalyticsAge(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Age))
            {
                value = searchParams.Age.ToString();
            }
            waList.Add(value);
        }

        //Parameter q (Keyword/Phrase)
        private static void AddAnalyticsKeyword(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Phrase))
            {
                value = HttpUtility.UrlEncode(searchParams.Phrase);
            }
            waList.Add(value);
        }

        // Parameter g (Gender)
        private static void AddAnalyticsGender(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Gender))
            {
                value = HttpUtility.UrlEncode(searchParams.Gender);
            }
            waList.Add(value);
        }

        //// Parameter loc (Location, and AtNIH if loc=nih)
        //private static void AddAnalyticsLocation(List<string> waList, CTSSearchParams searchParams)
        //{
        //    string value = "none";
        //    if (searchParams.IsFieldSet(FormFields.Location))
        //    {
        //        switch (searchParams.Location)
        //        {
        //            case LocationType.Zip:
        //            {
        //                value = "zip";
        //                waList.Add(value);
        //                AddAnalyticsZip(waList, searchParams);
        //                break;
        //            }
        //            case LocationType.CountryCityState:
        //            {
        //                value = "country_city_state";
        //                waList.Add(value);
        //                AddAnalyticsCountryCityState(waList, searchParams);
        //                break;
        //            }
        //            case LocationType.Hospital:
        //            {
        //                value = "hospital";
        //                waList.Add(value);
        //                AddAnalyticsHospital(waList, searchParams);
        //                break;
        //            }
        //            case LocationType.AtNIH:
        //            {
        //                value = "at_nih";
        //                waList.Add(value);
        //                break;
        //            }
        //            default:
        //            {
        //                waList.Add(value);
        //                break;
        //            }
        //        }
        //    }
        //}

        //// Parameter z (Zipcode) and parameter zp (Zip proximity/radius)
        //private static void AddAnalyticsZip(List<string> waList, CTSSearchParams searchParams)
        //{
        //    ZipCodeLocationSearchParams locParams = (ZipCodeLocationSearchParams)searchParams.LocationParams;
        //    string valZip = "none";
        //    string valProx = "none";

        //    if (locParams.IsFieldSet(FormFields.ZipCode))
        //    {
        //        valZip = HttpUtility.UrlEncode(locParams.ZipCode);
        //    }
        //    if (locParams.IsFieldSet(FormFields.ZipRadius))
        //    {
        //        valProx = locParams.ZipRadius.ToString();
        //    }

        //    waList.Add(valZip);
        //    waList.Add(valProx);
        //}

        //////Parameter lst (State) && Parameter lcty (City) && Parameter lcnty (Country)
        //private static void AddAnalyticsCountryCityState(List<string> waList, CTSSearchParams searchParams)
        //{
        //    CountryCityStateLocationSearchParams locParams = (CountryCityStateLocationSearchParams)searchParams.LocationParams;
        //    string valCountry = "none";
        //    string valState = "none";
        //    string valCity = "none";

        //    if (locParams.IsFieldSet(FormFields.Country))
        //    {
        //        valCountry = HttpUtility.UrlEncode(locParams.Country);
        //    }
        //    if (locParams.IsFieldSet(FormFields.State))
        //    {
        //        valState = string.Join(",", locParams.State.Select(lst => lst.Key));
        //    }
        //    if (locParams.IsFieldSet(FormFields.City))
        //    {
        //        valCity = HttpUtility.UrlEncode(locParams.City);
        //    }

        //    waList.Add(valCountry);
        //    waList.Add(valState);
        //    waList.Add(valCity);
        //}

        ////Parameter hos (Hospital)
        //private static void AddAnalyticsHospital(List<string> waList, CTSSearchParams searchParams)
        //{
        //    string value = "none";
        //    HospitalLocationSearchParams locParams = (HospitalLocationSearchParams)searchParams.LocationParams;
        //    if (locParams.IsFieldSet(FormFields.Hospital))
        //    {
        //        value = HttpUtility.UrlEncode(locParams.Hospital);
        //    }
        //    waList.Add(value);
        //}

        //// Parameter tt (Trial Type)
        //private static void AddAnalyticsTrialTypes(List<string> waList, CTSSearchParams searchParams)
        //{
        //    string value = "none";
        //    if (searchParams.IsFieldSet(FormFields.TrialTypes))
        //    {
        //        value = string.Join(",", searchParams.TrialTypes.Select(tp => tp.Key));
        //    }
        //    waList.Add(value);
        //}

        ////Parameter d (Drugs)
        //private static void AddAnalyticsDrugs(List<string> waList, CTSSearchParams searchParams)
        //{
        //    string value = "none";
        //    if (searchParams.IsFieldSet(FormFields.Drugs))
        //    {
        //        value = AddAnalyticsMultiTermFields(searchParams.Drugs);
        //    }
        //    waList.Add(value);
        //}

        ////Parameter i (Other treatments / interventions)
        //private static void AddAnalyticsOtherTreatments(List<string> waList, CTSSearchParams searchParams)
        //{
        //    string value = "none";
        //    if (searchParams.IsFieldSet(FormFields.OtherTreatments))
        //    {
        //        value = AddAnalyticsMultiTermFields(searchParams.OtherTreatments);
        //    }
        //    waList.Add(value);
        //}

        // Parameter tp (Trial Phase)
        private static void AddAnalyticsTrialPhases(List<string> waList, CTSSearchParams searchParams)
        {
            //TODO: differentiate between 'all' and 'none' if possible
            string value = "all";
            if (searchParams.IsFieldSet(FormFields.TrialPhases))
            {
                value = string.Join(",", searchParams.TrialPhases.Select(tp => tp.Key));
            }
            waList.Add(value);
        }

        // Parameter tid (Trial IDs)
        private static void AddAnalyticsTrialIDs(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.TrialIDs))
            {
                if (searchParams.TrialIDs.Length == 1)
                {
                    value = "single:";
                }
                else
                {
                    value = "multiple:";
                }
                value += string.Join(",", searchParams.TrialIDs.Select(tid => HttpUtility.UrlEncode(tid)));
            }
            waList.Add(value);
        }

        // Parameter in (Investigator)
        private static void AddAnalyticsInvestigator(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Investigator))
            {
                value = HttpUtility.UrlEncode(searchParams.Investigator);
            }
            waList.Add(value);
        }

        // Parameter lo (Lead Org)
        private static void AddAnalyticsLeadOrg(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.LeadOrg))
            {
                value = HttpUtility.UrlEncode(searchParams.LeadOrg);
            }
            waList.Add(value);
        }

        #endregion


        #region Util methods

        /// <summary>
        /// Converts a TerminologyFieldSearchParam[] to a string of labels
        /// </summary>
        /// <param name="fieldValues">An array of TerminologyFieldSearchParam[]</param>
        /// <returns></returns>
        private static string AddAnalyticsMultiTermFieldLabels(TerminologyFieldSearchParam[] fieldValues)
        {
            List<string> labels = new List<string>();
            foreach (TerminologyFieldSearchParam termField in fieldValues)
            {
                labels.Add(string.Join("/", termField.Label));
            }
            return string.Join(",", labels.ToArray());
        }

        /// <summary>
        /// Converts a TerminologyFieldSearchParam[] to a string of codes
        /// </summary>
        /// <param name="fieldValues">An array of TerminologyFieldSearchParam[]</param>
        /// <returns></returns>
        private static string AddAnalyticsMultiTermFieldCodes(TerminologyFieldSearchParam[] fieldValues)
        {
            List<string> codes = new List<string>();

            if(fieldValues.Length > 5)
            {
                return "more than 5";
            }

            foreach (TerminologyFieldSearchParam termField in fieldValues)
            {
                codes.Add(string.Join(",", termField.Codes));
            }
            return string.Join(",", codes.ToArray());
        }

        #endregion
    }
}
