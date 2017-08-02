using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Moq;
using NCI.Web;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    /// <summary>
    /// Tests for the CTSearchParamFactory
    /// </summary>
    public partial class CTSSearchParamFactory_Test
    {

        //Placeholder for testing CTSSearchParam -> FilterCriteria mapping.
        public static IEnumerable<object[]> SerializeParamsData
        {
            get
            {
                // Array of tests
                return new[]
                {
                    //This array of objects maps to the parameters of the create method.
                    //URL at index 0, Expected object at index 1.
                    //TODO: fill out the rest of these tests
                    //TODO: get the tests to actually work - still having the equals/equivalent
                    //      errors with array comparer 
                    
                    
                    // TEST 0 - No parameters.
                    new object[] { 
                        new CTSSearchParams(),
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                //Params HERE
                            }
                        }
                    },

                    // TEST 5 - Age
                    new object[] {
                        new CTSSearchParams() {
                            Age = 35
                        },                        
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                //Params HERE
                            }
                        }
                    },

                    
                    // TEST 6 - Gender
                    new object[] {
                        new CTSSearchParams() {
                            Gender = "male"
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                //Params HERE
                            }
                        }
                    },
                    

                    // TEST 7 - Phrase/keyword
                    new object[] {
                        new CTSSearchParams() {
                            Phrase = "chicken"
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                //Params HERE
                            }
                        }
                    }/*,
                    // TEST 16 - Trial type
                    new object[] {"?tt=basic_science", new CTSSearchParams() {
                        TrialTypes = new LabelledSearchParam[] { 
                            new LabelledSearchParam() {
                                Key = "basic_science",
                                Label = "Basic science"
                            }
                        }
                    }},

                    // TEST 17 - Drug
                    new object[] {"?d=C1647", new CTSSearchParams() {
                        Drugs = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C1647" },
                                Label = "Trastuzumab"
                            }
                        }
                    }},

                    // TEST 18 - Other treatments/interventions
                    new object[] {"?i=C131060", new CTSSearchParams() {
                        OtherTreatments = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C131060" },
                                Label = "Checkpoint Blockade Immunotherapy"
                            }
                        }
                    }},

                    // TEST 19 - Trial phase 
                    new object[] {"?tp=i", new CTSSearchParams() {
                        TrialPhases = new LabelledSearchParam[] { 
                            new LabelledSearchParam() {
                                Key = "i",
                                Label = "I"
                            }
                        }
                    }},

                    // TEST 20 - Trial ID 
                    new object[] {"tid=NCI-2014-01509", new CTSSearchParams() {
                        TrialIDs = new string[] {"NCI-2014-01509"}
                    }},

                    // TEST 21 - Principal investigator 
                    new object[] { "?in=Sophia+Smith", new CTSSearchParams() {
                        Investigator = "Sophia Smith"
                    }},

                    // TEST 22 - Lead organization
                    new object[] { "?lo=Mayo+Clinic", new CTSSearchParams() {
                        LeadOrg = "Mayo Clinic"
                    }},

                    // TEST 23 - Page number
                    new object[] { "?pn=3", new CTSSearchParams() {
                        Page = 3
                    }},

                    // TEST 24 - Items per page
                    new object[] { "?ni=25", new CTSSearchParams() {
                        ItemsPerPage = 25
                    }}
                    */
                };
            }
        }

        /// <summary>
        /// CTSSearchParams -> filterCriterea mapping tests
        /// </summary>
        /// <param name="searchParams">An instance of a CTSSearchParams object</param>
        /// <param name="expectedCriteria">The expected criteria for the search</param>
        [Theory, MemberData("SerializeParamsData")]
        public void SerializeParams(CTSSearchParams searchParams, NciUrl expected)
        {
            //Get the results of parsing the URL
            NciUrl actual = CTSSearchParamFactory.ConvertParamsToUrl(searchParams);

            //Test the actual result to the expected.  NOTE: If you add fields to the CTSSearchParams, you need
            //to also modify the comparer
            Assert.Equal(expected.QueryParameters, actual.QueryParameters);
        }

          


    }
}
