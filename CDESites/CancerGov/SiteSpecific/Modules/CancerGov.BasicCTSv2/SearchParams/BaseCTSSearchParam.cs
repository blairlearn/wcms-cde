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
        /// Property to set/get cancer subtype.
        /// </summary>
        public string CancerSubtype { get; set; }

        /// <summary>
        /// Property to set/get cancer type stage.
        /// </summary>
        public string CancerStage { get; set; }

        /// <summary>
        /// Property to set/get cancer type side effects, biomarksers, attributes.
        /// </summary>
        public string CancerFindings { get; set; }

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
        /// 
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HospitalOrInstitution { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AtNIH { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AtNIH_IsSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TrialType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] TrialTypeArray { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] DrugIDs { get; set; }

        public string DrugIDsString {
            get {
                if (DrugIDs != null)
                    return String.Join(",", DrugIDs);
                else
                    return "";
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        public string DrugName { get; set; }
                
        /// <summary>
        /// 
        /// </summary>
        public string[] TreatmentInterventionCodes { get; set; }
        public string TreatmentInterventionCodeString
        {
            get
            {
                if (TreatmentInterventionCodes != null)
                    return String.Join(",", TreatmentInterventionCodes);
                else
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TreatmentInterventionTerm { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TrialPhase { get; set; }

        ///<summary>
        ///
        /// </summary>
        public string[] TrialPhaseArray { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NewTrialsOnly { get; set; }

        public bool NewTrialsOnly_IsSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] TrialIDs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PrincipalInvestigator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LeadOrganization { get; set; }

    }
}
