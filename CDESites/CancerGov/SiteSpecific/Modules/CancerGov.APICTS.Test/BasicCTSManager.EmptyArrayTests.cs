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
        //Placeholder for testing CTSSearchParam -> FilterCriteria mapping with empty arrays.
        public static IEnumerable<object[]> EmptyArrayMappingData
        {
            get
            {
                // Array of tests
                return new[]
                {
                    // Empty array for Subtype
                    new object[] {
                        new CTSSearchParams() {
                            SubTypes = new TerminologyFieldSearchParam[] { }
                        },
                        new Dictionary<string, object> {
                            { "current_trial_status", CTSConstants.ActiveTrialStatuses }
                        }
                    },
                    // Empty array for Stage
                    new object[] {
                        new CTSSearchParams() {
                            Stages = new TerminologyFieldSearchParam[] { }
                        },
                        new Dictionary<string, object> {
                            { "current_trial_status", CTSConstants.ActiveTrialStatuses }
                        }
                    },
                    // Empty array for Findings
                    new object[] {
                        new CTSSearchParams() {
                            Findings = new TerminologyFieldSearchParam[] { }
                        },
                        new Dictionary<string, object> {
                            { "current_trial_status", CTSConstants.ActiveTrialStatuses }
                        }
                    },
                    // Empty array for Drugs
                    new object[] {
                        new CTSSearchParams() {
                            Drugs = new TerminologyFieldSearchParam[] { }
                        },
                        new Dictionary<string, object> {
                            { "current_trial_status", CTSConstants.ActiveTrialStatuses }
                        }
                    },
                    // Empty array for Other Treatments
                    new object[] {
                        new CTSSearchParams() {
                            OtherTreatments = new TerminologyFieldSearchParam[] { }
                        },
                        new Dictionary<string, object> {
                            { "current_trial_status", CTSConstants.ActiveTrialStatuses }
                        }
                    },
                    // Empty array for Trial Types
                    new object[] {
                        new CTSSearchParams() {
                            TrialTypes = new LabelledSearchParam[] { }
                        },
                        new Dictionary<string, object> {
                            { "current_trial_status", CTSConstants.ActiveTrialStatuses }
                        }
                    },
                    // Empty array for Trial Phases
                    new object[] {
                        new CTSSearchParams() {
                            TrialPhases = new LabelledSearchParam[] { }
                        },
                        new Dictionary<string, object> {
                            { "current_trial_status", CTSConstants.ActiveTrialStatuses }
                        }
                    },
                    // Empty array for Trial IDs
                    new object[] {
                        new CTSSearchParams() {
                            TrialIDs = new string[] { }
                        },
                        new Dictionary<string, object> {
                            { "current_trial_status", CTSConstants.ActiveTrialStatuses }
                        }
                    },
                    
                    //TODO: Handle combo?
                };
            }
        }


        /// <summary>
        /// CTSSearchParams -> filterCriterea mapping tests with empty arrays
        /// </summary>
        /// <param name="searchParams">An instance of a CTSSearchParams object</param>
        /// <param name="expectedCriteria">The expected criteria for the search</param>
        [Theory, MemberData("EmptyArrayMappingData")]
        public void MappingEmptyArrayFields(CTSSearchParams searchParams, Dictionary<string, object> expectedCriteria)
        {
            this.MappingTest(searchParams, expectedCriteria);
        }
    }
}
