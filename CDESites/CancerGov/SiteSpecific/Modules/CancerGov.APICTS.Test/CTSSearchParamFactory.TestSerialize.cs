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
                            }
                        }
                    },

                    // TEST 1 - Main Cancer Type
                    new object[] { 
                        new CTSSearchParams() {
                            MainType = new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C4872" },
                                Label = "Breast Cancer"
                            },
                            ResultsLinkFlag = ResultsLinkType.Basic,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "t", "C4872"},
                                { "rl", "1" },
                                { "loc", "0" }
                            }
                        }
                    },

                    // TEST 2 - Main Cancer Type
                    new object[] { 
                        new CTSSearchParams() {
                            MainType = new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C3995", "C4872" },
                                Label = "Stage IV Breast Cancer"
                            },
                            ResultsLinkFlag = ResultsLinkType.Basic,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "t", "C3995|C4872"},
                                { "rl", "1" },
                                { "loc", "0" }
                            }
                        }
                    },
                    
                    // TEST 3 - Cancer SubType
                    new object[] { 
                        new CTSSearchParams() {
                            SubTypes = new TerminologyFieldSearchParam[] {
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C7771" },
                                    Label = "Recurrent Breast Cancer"
                                }
                            },
                            ResultsLinkFlag = ResultsLinkType.Advanced,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "st", "C7771"},
                                { "rl", "2" },
                                { "loc", "0" }
                            }
                        }
                    },
                    
                    // TEST 4 - Cancer SubType
                    new object[] { 
                        new CTSSearchParams() {
                            SubTypes = new TerminologyFieldSearchParam[] {
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C4001", "C7771" },
                                    Label = "Recurrent Inflammatory Breast Cancer"
                                }
                            },
                            ResultsLinkFlag = ResultsLinkType.Advanced,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "st", "C4001|C7771"},
                                { "rl", "2" },
                                { "loc", "0" }
                            }
                        }
                    },
                    
                    // TEST 5 - Cancer SubType
                    new object[] { 
                        new CTSSearchParams() {
                            SubTypes = new TerminologyFieldSearchParam[] {
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C4001", "C7771" },
                                    Label = "Recurrent Inflammatory Breast Cancer"
                                },
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C7771" },
                                    Label = "Recurrent Breast Cancer"
                                }
                            },
                            ResultsLinkFlag = ResultsLinkType.Advanced,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "st", "C4001|C7771,C7771"},
                                { "rl", "2" },
                                { "loc", "0" }
                            }
                        }
                    },

                    // TEST 6 - Cancer Stage
                    new object[] { 
                        new CTSSearchParams() {
                            Stages = new TerminologyFieldSearchParam[] {
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C7768" },
                                    Label = "Stage II Breast Cancer"
                                }
                            },
                            ResultsLinkFlag = ResultsLinkType.Advanced,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "stg", "C7768"},
                                { "rl", "2" },
                                { "loc", "0" }
                            }
                        }
                    },

                    // TEST 7 - Cancer Stages
                    new object[] { 
                        new CTSSearchParams() {
                            Stages = new TerminologyFieldSearchParam[] {
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C5454", "C7768" },
                                    Label = "Stage II Breast Cancer"
                                }
                            },
                            ResultsLinkFlag = ResultsLinkType.Advanced,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "stg", "C5454|C7768"},
                                { "rl", "2" },
                                { "loc", "0" }
                            }
                        }
                    },

                    // TEST 8 - Cancer Stages
                    new object[] { 
                        new CTSSearchParams() {
                            Stages = new TerminologyFieldSearchParam[] {
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C3641" },
                                    Label = "Stage 0 Breast Cancer"
                                },
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C5454", "C7768" },
                                    Label = "Stage II Breast Cancer"
                                },
                            },
                            ResultsLinkFlag = ResultsLinkType.Advanced,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "stg", "C3641,C5454|C7768"},
                                { "rl", "2" },
                                { "loc", "0" }
                            }
                        }
                    },

                    // TEST 9 - Findings
                    new object[] { 
                        new CTSSearchParams() {
                            Findings = new TerminologyFieldSearchParam[] {
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C26696" },
                                    Label = "Anxiety"
                                }
                            },
                            ResultsLinkFlag = ResultsLinkType.Advanced,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "fin", "C26696"},
                                { "rl", "2" },
                                { "loc", "0" }
                            }
                        }
                    },

                    // TEST 10 - Findings
                    new object[] { 
                        new CTSSearchParams() {
                            Findings = new TerminologyFieldSearchParam[] {
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C26696" },
                                    Label = "Anxiety"
                                },
                                new TerminologyFieldSearchParam() {
                                    Codes = new string[] { "C35014" },
                                    Label = "Separation Anxiety Disorder"
                                }
                            },
                            ResultsLinkFlag = ResultsLinkType.Advanced,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "fin", "C26696,C35014"},
                                { "rl", "2" },
                                { "loc", "0" }
                            }
                        }
                    },
                    
                    // TEST 11 - Age
                    new object[] {
                        new CTSSearchParams() {
                            Age = 35,
                            ResultsLinkFlag = ResultsLinkType.Basic,
                            Location = LocationType.None
                        },                        
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "a", "35" },
                                { "rl", "1" },
                                { "loc", "0" }
                            }
                        }
                    },
                    
                    // TEST 6 - Gender
                    new object[] {
                        new CTSSearchParams() {
                            Gender = "male",
                            ResultsLinkFlag = ResultsLinkType.Basic,
                            Location = LocationType.None
                        },
                        new NciUrl() {
                            QueryParameters = new Dictionary<string,string>() {
                                { "g", "male" },
                                { "rl", "1" },
                                { "loc", "0" }
                            }
                        }
                    },
                    /*
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
                    },
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
