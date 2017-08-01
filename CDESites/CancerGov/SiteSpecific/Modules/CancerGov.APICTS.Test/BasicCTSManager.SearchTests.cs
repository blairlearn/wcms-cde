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
        public static IEnumerable<object[]> ParamMappingData
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
                    
                    /*
                    // TEST 0 - No parameters.
                    new object[] { "", new CTSSearchParams() },

                    // TEST 1 - Main Cancer Type
                    new object[] {"?t=C4872", new CTSSearchParams() {
                        MainType = new TerminologyFieldSearchParam() {
                            Codes = new string[] { "C4872" },
                            Label = "Breast Cancer"
                        }
                    }},

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

                    // TEST 5 - Age
                    new object[] { "?a=35", new CTSSearchParams() {
                        Age = 35
                    }},

                    // TEST 6 - Gender
                    new object[] { "?g=male", new CTSSearchParams() {
                        Gender = "male"
                    }},
                    */

                    // TEST 7 - Phrase/keyword
                    new object[] {
                        new CTSSearchParams() {
                            Phrase = "chicken"
                        },
                        new Dictionary<string, object>() {
                            { "_fulltext", "chicken" },
                            { "current_trial_status", BasicCTSManager.ActiveTrialStatuses }
                        }
                    }//,

                    /*
                    // TEST 8 - Location
                    new object[] { "?loc=0", new CTSSearchParams() {
                        Location = LocationType.None
                    }},

                    // TEST 9 - Zip code
                    new object[] { "?loc=1&z=20850", new CTSSearchParams() {
                        Location = LocationType.Zip,
                        LocationParams = new ZipCodeLocationSearchParams() {
                            ZipCode = "20850"
                        }
                    }},

                    // TEST 10 - Zip radius
                    new object[] { "?loc=1&z=20850&zp=500", new CTSSearchParams() {
                        Location = LocationType.Zip,
                        LocationParams = new ZipCodeLocationSearchParams() {
                            ZipCode = "20850",
                            ZipRadius = 500
                        }
                    }},


                    // TEST 11 - Country
                    new object[] { "?loc=2&lcnty=United+States", new CTSSearchParams() {
                        Location = LocationType.CountryCityState,
                        LocationParams = new CountryCityStateLocationSearchParams() {
                            Country = "United States"
                        }
                    }},

                    // TEST 12 - State 
                    new object[] { "?loc=2&lst=MD", new CTSSearchParams() {
                        Location = LocationType.CountryCityState,
                        LocationParams = new CountryCityStateLocationSearchParams() {
                            State = new LabelledSearchParam() { 
                                Key = "MD",
                                Label = "Maryland"
                            }
                        }
                    }}, 

                    // TEST 13 - City 
                    new object[] { "?loc=2&lcty=Baltimore", new CTSSearchParams() {
                        Location = LocationType.CountryCityState,
                        LocationParams = new CountryCityStateLocationSearchParams() {
                            City = "Baltimore"
                        }
                    }},
                    
                    // TEST 14 - Hospital 
                    new object[] { "?loc=3&hos=M+D+Anderson+Cancer+Center", new CTSSearchParams() {
                        Location = LocationType.Hospital,
                        LocationParams = new HospitalLocationSearchParams() {
                            Hospital = "M D Anderson Cancer Center"
                        }
                    }},
                    
                    // TEST 15 - Is location NIH?  
                    new object[] { "?loc=4", new CTSSearchParams() {
                        Location = LocationType.AtNIH,
                        LocationParams = new AtNIHLocationSearchParams()
                    }},

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
        [Theory, MemberData("ParamMappingData")]
        public void MappingTest(CTSSearchParams searchParams, Dictionary<string,object> expectedCriteria)
        {
            Dictionary<string, object> actualCriteria = null;

            //When search gets called trap the criteria and set the actualCriteria
            var mockClient = GetClientMock(
                (size, from, include, exclude, filterCriteria) => actualCriteria = filterCriteria,
                new ClinicalTrialsCollection() { TotalResults = 0, Trials = new ClinicalTrial[] { } }
            );

            //Create a new instance of the factory, passing in the Mock's version of an implementation
            //of our IClinicalTrialsAPIClient interface.
            BasicCTSManager manager = new BasicCTSManager(mockClient.Object);

            //Get the results of parsing the URL
            ClinicalTrialsCollection returnedCol = manager.Search(searchParams);

            //Test the actual result to the expected.  NOTE: If you add fields to the CTSSearchParams, you need
            //to also modify the comparer
            Assert.Equal(expectedCriteria, actualCriteria);
        }

        /// <summary>
        /// Gets a mock that can be used for a IClinicalTrialsAPIClient
        /// See https://github.com/moq/moq4 for more details on the mock library.
        /// (You can do cool thinks like make sure a method was called a certain number of times too...)
        /// (You can pretend to throw an exception if this are not right...)
        /// </summary>
        /// 
        /// <returns>A mock to be used as the service.</returns>
        private Mock<IClinicalTrialsAPIClient> GetClientMock(
            Action<int, int, string[], string[], Dictionary<string,object>> criteriaIntercept, ClinicalTrialsCollection rtnCollection)
        {
            Mock<IClinicalTrialsAPIClient> rtnMock = new Mock<IClinicalTrialsAPIClient>();

            //Handle the case when a string of C4872 is passed in to GetTitleCase and return the label "Breast Cancer"
            //This makes it so that we do not have to create a fake class that returns fake data.
            rtnMock.Setup(client => client.List(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string[]>(),
                It.IsAny<string[]>(),
                It.IsAny < Dictionary<string, object>>()))
                .Callback(criteriaIntercept) //This should be fleshed out to accept more params
                .Returns(rtnCollection);

            return rtnMock;
        }

    }
}
