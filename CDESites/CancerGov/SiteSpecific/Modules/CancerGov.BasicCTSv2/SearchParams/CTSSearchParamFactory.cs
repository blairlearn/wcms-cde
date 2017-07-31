using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCI.Web;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Class is a factory that when given a URL will return a populated CTSSearchParam object
    /// </summary>
    public class CTSSearchParamFactory
    {
        ///Delegate definition so we can more cleanly list the parsers we will call.
        private delegate void ParameterParserDelegate(NciUrl url, CTSSearchParams searchParams);


        private ITerminologyLookupService _lookupSvc;
        private ParameterParserDelegate _parsers;

        /// <summary>
        /// Creates new instance of a search param factory
        /// </summary>
        /// <param name="lookupSvc">An instance of a ITerminologyLookupService </param>
        public CTSSearchParamFactory(ITerminologyLookupService lookupSvc)
        {
            this._lookupSvc = lookupSvc;

            //Add parser methods here
            this._parsers =
                (ParameterParserDelegate)ParseKeyword + //First param needs the cast.
                ParseCancerType +
                ParseSubTypes +
                ParseStages +
                ParseFindings +
                ParseAge +
                ParseGender +
                ParseLocation +
                ParseZipCode +
                ParseZipRadius +
                ParseState +
                ParseDrugs +
                ParseOtherTreatments +
                ParseCity +
                ParsePageNum +
                ParseItemsPerPage +
                ParseResultsLinkFlag;
        }

        /// <summary>
        /// Gets an instance of a CTSSearchParams object based on params in URL.
        /// </summary>
        /// <param name="url">The URL to parse</param>
        /// <returns></returns>
        public CTSSearchParams Create(string url)
        {
            CTSSearchParams rtnParams = new CTSSearchParams();

            NciUrl reqUrl = new NciUrl();
            reqUrl.SetUrl(url);

            _parsers(reqUrl, rtnParams); //This calls each of the parsers, one chained after another.
            

            return rtnParams; 
        }

        #region Parameter Parsers 

        //Parameter q (Keyword/Phrase)
        private void ParseKeyword(NciUrl url, CTSSearchParams searchParams)
        {
            //TODO: Handle lowercase
            if (url.QueryParameters.ContainsKey("q"))
            {
                string phrase = ParamAsStr(url.QueryParameters["q"]);
                if(!string.IsNullOrWhiteSpace(phrase))
                {
                    searchParams.Phrase = phrase;
                }
                else
                {
                    LogParseError("Keyword/Phrase", "Please enter a valid keyword parameter.", searchParams);
                }
            }
        }
        
        //Parameter t (Main Cancer Type)
        private void ParseCancerType(NciUrl url, CTSSearchParams searchParams)
        {
            //TODO: Extra credit, refactor the term extraction logic so it does not get repeated for each type
            //TODO: Handle Lowercase
            if (url.QueryParameters.ContainsKey("t"))
            {
                TerminologyFieldSearchParam[] terms = GetTermFieldFromParam(url.QueryParameters["t"]);
                if (terms.Length == 1)
                {
                    searchParams.MainType = terms[0];
                }
                else if (terms.Length > 1)
                {
                    LogParseError("MainType", "Please include only one main cancer type in your search.", searchParams);

                }
            }
        }

        //Parameter st (SubTypes)
        private void ParseSubTypes(NciUrl url, CTSSearchParams searchParms)
        {
            //TODO: Handle Lowercase
            if (url.QueryParameters.ContainsKey("st"))
            {
                searchParms.SubTypes = GetTermFieldFromParam(url.QueryParameters["st"]);
            }
        }

        //Parameter stg (Stages)
        private void ParseStages(NciUrl url, CTSSearchParams searchParms)
        {
            //TODO: Handle Lowercase
            if (url.QueryParameters.ContainsKey("stg"))
            {
                searchParms.Stages = GetTermFieldFromParam(url.QueryParameters["stg"]);
            }
        }

        //Parameter fin (Findings)
        private void ParseFindings(NciUrl url, CTSSearchParams searchParms)
        {
            //TODO: Handle Lowercase
            if (url.QueryParameters.ContainsKey("fin"))
            {
                searchParms.Findings = GetTermFieldFromParam(url.QueryParameters["fin"]);
            }
        }

        // Parameter a (Age)
        private void ParseAge(NciUrl url, CTSSearchParams searchParams)
        {
            if(url.QueryParameters.ContainsKey("a"))
            {
                int age = this.ParamAsInt(url.QueryParameters["a"], 0);
                if(age == 0)
                {
                    LogParseError("Age", "Please enter a valid age parameter.", searchParams);
                }
                else
                {
                    searchParams.Age = age;
                }
            }
        }

        // Parameter g (Gender)
        private void ParseGender(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("g"))
            {
                string gender = ParamAsStr(url.QueryParameters["g"]);
                if (!string.IsNullOrWhiteSpace(gender))
                {
                    searchParams.Gender = gender;
                }
                else
                {
                    LogParseError("Gender", "Please enter a valid gender.", searchParams);
                }
            }
        }

        // Parameter loc (Location)
        private void ParseLocation(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("loc"))
            {
                string location = ParamAsStr(url.QueryParameters["loc"]);
                if (!string.IsNullOrWhiteSpace(location))
                {
                    searchParams.Location = location;
                }
                else
                {
                    LogParseError("Location", "Please enter a valid location type.", searchParams);
                }
            }
        }

        // Parameter z (Zip Code)
        private void ParseZipCode(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("z"))
            {
                string zipCode = ParamAsStr(url.QueryParameters["z"]);
                if (string.IsNullOrWhiteSpace(zipCode) || (zipCode.Length < 5))
                // TODO: add regex to check for chars other than numbers or "-"
                {
                    LogParseError("ZipCode", "Please enter a valid zip code value.", searchParams);
                }
                else
                {
                    searchParams.ZipCode = zipCode;
                }
            }
        }

        // Parameter zp (Zip Radius)
        private void ParseZipRadius(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("zp"))
            {
                int zipRadius = ParamAsInt(url.QueryParameters["zp"], 100);
                if (zipRadius < 1 || zipRadius > 12451)
                // TODO: check for type mismatch
                {
                    LogParseError("ZipRadius", "Please enter a valid zip radius value.", searchParams);
                }
                else
                {
                    searchParams.ZipRadius = zipRadius;
                }
            }
        }

        //Parameter lst (State)
        private void ParseState(NciUrl url, CTSSearchParams searchParams)
        {
            //TODO: Handle label conversion
            if (url.QueryParameters.ContainsKey("lst"))
            {
                LabelledSearchParam[] states = GetLabelledFieldFromParam(url.QueryParameters["lst"]);
                if (states.Length == 1)
                {
                    searchParams.State = states[0];
                }
                else
                {
                    LogParseError("State", "Please include only one state in your search.", searchParams);
                }
            }
        }

        //Parameter lcty (City)
        private void ParseCity(NciUrl url, CTSSearchParams searchParams)
        {
            //TODO: Handle lowercase
            if (url.QueryParameters.ContainsKey("lcty"))
            {
                string city = ParamAsStr(url.QueryParameters["lcty"]);
                if(!string.IsNullOrWhiteSpace(city))
                {
                    searchParams.City = city;
                }
                else
                {
                    LogParseError("City", "Please enter a valid city parameter.", searchParams);
                }
            }
        }

        //Parameter d (Drugs)
        private void ParseDrugs(NciUrl url, CTSSearchParams searchParms)
        {
            //TODO: Handle Lowercase
            if (url.QueryParameters.ContainsKey("d"))
            {
                searchParms.Drugs = GetTermFieldFromParam(url.QueryParameters["d"]);
            }
        }

        //Parameter i (Other treatments / interventions)
        private void ParseOtherTreatments(NciUrl url, CTSSearchParams searchParms)
        {
            //TODO: Handle Lowercase
            if (url.QueryParameters.ContainsKey("i"))
            {
                searchParms.OtherTreatments = GetTermFieldFromParam(url.QueryParameters["i"]);
            }
        }

        // Parameter in (Investigator)
        private void ParseInvestigator(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("in"))
            {
                string investigator = ParamAsStr(url.QueryParameters["in"]);
                if (!string.IsNullOrWhiteSpace(investigator))
                {
                    searchParams.City = investigator;
                }
                else
                {
                    LogParseError("Investigator", "Please enter a valid trial investigator parameter.", searchParams);
                }
            }
        }

        // Parameter lo (Lead Org)
        private void ParseLeadOrg(NciUrl url, CTSSearchParams searchParams)
        {
            if(url.QueryParameters.ContainsKey("lo"))
            {
                string leadOrg = ParamAsStr(url.QueryParameters["lo"]);
                if (!string.IsNullOrWhiteSpace(leadOrg))
                {
                    searchParams.City = leadOrg;
                }
                else
                {
                    LogParseError("LeadOrg", "Please enter a valid lead organization parameter.", searchParams);
                }
            }
        }

        // Parameter pn (Page Number)
        private void ParsePageNum(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("pn"))
            {
                int pageNum = ParamAsInt(url.QueryParameters["pn"], 10);
                if (pageNum < 1)
                {
                    LogParseError("Page", "Please enter a valid page number.", searchParams);
                }
                else
                {
                    searchParams.Page = pageNum;
                }
            }
        }

        // Parameter ni (Items Per Page)
        private void ParseItemsPerPage(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("ni"))
            {
                int itemsPerPage = ParamAsInt(url.QueryParameters["ni"], 10);
                if (itemsPerPage < 1)
                {
                    LogParseError("ItemsPerPage", "Please enter a valid number of items to display per page.", searchParams);
                }
                else
                {
                    searchParams.ItemsPerPage = itemsPerPage;
                }
            }
        }

        // Parameter rl (Results Link Flag)
        private void ParseResultsLinkFlag(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("rl"))
            {
                int resLinkFlag = ParamAsInt(url.QueryParameters["rl"], 0);
                if (resLinkFlag == 0)
                {
                    LogParseError("ResultsLinkFlag", "Please enter a valid results link flag value (1 or 2).", searchParams);
                }
                else
                {
                    searchParams.ResultsLinkFlag = resLinkFlag;
                }
            }
        }

        #endregion

        #region Utility methods 

        /// <summary>
        /// Extract property values for 'Labelled' Field search params (e.g. state, phase, trial type)
        /// </summary>
        /// <param name="paramData"></param>
        /// <returns>Array of param objects</returns>
        private LabelledSearchParam[] GetLabelledFieldFromParam(string paramData)
        {
            List<LabelledSearchParam> rtnParams = new List<LabelledSearchParam>();
            LabelledSearchParam type = new LabelledSearchParam();

            //TODO: Update mapping to handle different cases (like state abbreviation vs primary purpose label)
            type.Key = paramData;
            type.Label = this._lookupSvc.Get(type.Key);

            rtnParams.Add(type);

            return rtnParams.ToArray();
        }

        /// <summary>
        /// Extract property values for Terminology Field search params (e.g. cancer type, subtype, drug)
        /// </summary>
        /// <param name="paramData"></param>
        /// <returns>Array of param objects</returns>
        private TerminologyFieldSearchParam[] GetTermFieldFromParam(string paramData)
        {
            List<TerminologyFieldSearchParam> rtnParams = new List<TerminologyFieldSearchParam>();

            TerminologyFieldSearchParam type = new TerminologyFieldSearchParam();

            //TODO: Handle validating codes, handling multiple codes, etc.
            type.Codes = new string[] { paramData };
            type.Label = this._lookupSvc.GetTitleCase(String.Join(",", type.Codes));

            rtnParams.Add(type);

            return rtnParams.ToArray();
        }

        /// <summary>
        /// Gets a query parameter as a string
        /// </summary>
        protected string ParamAsStr(string paramVal)
        {
            if (string.IsNullOrWhiteSpace(paramVal))
                return String.Empty;
            else
                return paramVal.Trim();
        }

        /// <summary>
        /// Converts a query param to an int; returns 0 if unable to parse
        /// </summary>
        private int ParamAsInt(string paramVal, int def)
        {
            if (string.IsNullOrWhiteSpace(paramVal))
            {
                return def;
            }
            else
            {
                int tmpInt = 0;
                if (int.TryParse(paramVal.Trim(), out tmpInt))
                {
                    return tmpInt;
                }
                else
                {
                    return def;
                }
            }
        }

        /// <summary>
        /// Logs an error in the ParseErrors array on the Search Params
        /// </summary>
        private void LogParseError(string param, string errorMessage, CTSSearchParams searchParams)
        {
            CTSSearchParamError error = new CTSSearchParamError();
            error.Param = param;
            error.ErrorMessage = errorMessage;
            searchParams.ParseErrors.Add(error);
        }
        #endregion

    }
}
