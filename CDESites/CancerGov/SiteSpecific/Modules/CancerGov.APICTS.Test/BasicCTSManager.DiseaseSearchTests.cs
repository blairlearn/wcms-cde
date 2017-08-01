using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Moq;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    public partial class BasicCTSManager_Test
    {

        //Placeholder for testing CTSSearchParam -> FilterCriteria mapping.
        public static IEnumerable<object[]> DiseaseFieldsMappingData
        {
            get
            {
                // Array of tests
                return new[]
                {

                    // TEST 1 - Main Cancer Type
                    new object[] {
                        new CTSSearchParams() {
                            MainType = new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C4872" },
                                Label = "Breast Cancer"
                            }
                        },
                        new Dictionary<string, object>{
                            { "diseases.nci_thesaurus_concept_id", new string[] { "C4872" }},
                            { "current_trial_status", BasicCTSManager.ActiveTrialStatuses }
                        }
                    }//,
                    //TODO: Handle Cancer subtypes, stages and findings
                    // with multiple IDs too!
                    /*
                    // TEST 2 - Cancer subtype
                    new object[] {"?st=C7771", new CTSSearchParams() {
                        SubTypes = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C7771" },
                                Label = "Recurrent Breast Cancer"
                            }
                        }
                    }},

                    // TEST 3 - Cancer stage
                    new object[] {"?stg=C88375", new CTSSearchParams() {
                        Stages = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C88375" },
                                Label = "Stage I Breast Cancer"
                            }
                        }
                    }},

                    // TEST 4 - Cancer findings 
                    new object[] {"?fin=C26696", new CTSSearchParams() {
                        Findings = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C26696" },
                                Label = "Anxiety"
                            }
                        }
                    }},
                    */
                };
            }
        }


        /// <summary>
        /// CTSSearchParams -> filterCriterea mapping tests
        /// </summary>
        /// <param name="searchParams">An instance of a CTSSearchParams object</param>
        /// <param name="expectedCriteria">The expected criteria for the search</param>
        [Theory, MemberData("DiseaseFieldsMappingData")]
        public void MappingDiseaseFields(CTSSearchParams searchParams, Dictionary<string, object> expectedCriteria)
        {
            this.MappingTest(searchParams, expectedCriteria);
        }

    }
}
