using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Moq;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    /// <summary>
    /// Tests for the CTSearchParamFactory
    /// </summary>
    public class CTSSearchParamFactory_Test
    {
        //Test cases for Create test method. 
        //@Sarina and @Dion - You will want to create your expected objects in another file at some point.
        public static IEnumerable<object[]> URLParsingData {
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

                    // TEST 7 - Phrase/keyword
                    new object[] { "?q=chicken", new CTSSearchParams() {
                        Phrase = "chicken"
                    }},

                    // TEST 8 - Location
                    new object[] { "?loc=all", new CTSSearchParams() {
                        Location = "all"
                    }},

                    // TEST 9 - Zip code
                    new object[] { "?z=20850", new CTSSearchParams() {
                        ZipCode = "20850"
                    }},

                    // TEST 10 - Zip radius
                    new object[] { "?zp=500", new CTSSearchParams() {
                        ZipRadius = 500
                    }},


                    // TEST 11 - Country
                    new object[] { "?lcnty=United+States", new CTSSearchParams() {
                        Country = "United States"
                    }},

                    // TEST 12 - State 
                    new object[] { "?lst=MD", new CTSSearchParams() {
                        State = new LabelledSearchParam() { 
                            Key = "MD",
                            Label = "Maryland"
                        }
                    }}, 

                    // TEST 13 - City 
                    new object[] { "?lcty=Baltimore", new CTSSearchParams() {
                        City = "Baltimore"
                    }},

                    // TEST 14 - Hospital 
                    new object[] { "?hos=M+D+Anderson+Cancer+Center", new CTSSearchParams() {
                        Hospital = "M D Anderson Cancer Center"
                    }},

                    // TEST 15 - Is location NIH?  
                    new object[] { "?nih=", new CTSSearchParams() {
                        AtNIH = true
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
                };
            }
        }

        /// <summary>
        /// Main test method.  Takes in a URL and an expected object and sees if Create(url) returns that object.
        /// </summary>
        /// <param name="url">The "URL" with query parameters</param>
        /// <param name="expected">An instance of a CTSSearchParams object that represents the expected result after parsing the URL.</param>
        [Theory, MemberData("URLParsingData")]
        public void Create(string url, CTSSearchParams expected)
        {
            var mockLookupSvc = GetLookupMock();

            //Create a new instance of the factory, passing in the Mock's version of an implementation
            //of our ITerminologyLookupService interface.
            CTSSearchParamFactory factory = new CTSSearchParamFactory(mockLookupSvc.Object);

            //Get the results of parsing the URL
            CTSSearchParams actual = factory.Create(url);

            //Test the actual result to the expected.  NOTE: If you add fields to the CTSSearchParams, you need
            //to also modify the comparer
            Assert.Equal(expected, actual, new CTSSearchParamsComparer());
        }

        /// <summary>
        /// Gets a mock that can be used for a ITerminologyLookupService
        /// See https://github.com/moq/moq4 for more details on the mock library.
        /// (You can do cool thinks like make sure a method was called a certain number of times too...)
        /// (You can pretend to throw an exception if this are not right...)
        /// </summary>s
        /// <returns>A mock to be used as the service.</returns>
        private Mock<ITerminologyLookupService> GetLookupMock()
        {
            Mock<ITerminologyLookupService> rtnMock = new Mock<ITerminologyLookupService>();

            //Handle the case when a string of C4872 is passed in to GetTitleCase and return the label "Breast Cancer"
            //This makes it so that we do not have to create a fake class that returns fake data.
            rtnMock.Setup(lookup => lookup.GetTitleCase("C4872"))
                .Returns("Breast Cancer");

            rtnMock.Setup(lookup => lookup.GetTitleCase("C7771"))
                .Returns("Recurrent Breast Cancer");

            rtnMock.Setup(lookup => lookup.GetTitleCase("C88375"))
                .Returns("Stage I Breast Cancer");

            rtnMock.Setup(lookup => lookup.GetTitleCase("C26696"))
                .Returns("Anxiety");

            rtnMock.Setup(lookup => lookup.GetTitleCase("C1647"))
                .Returns("Trastuzumab");

            rtnMock.Setup(lookup => lookup.GetTitleCase("C131060"))
                .Returns("Checkpoint Blockade Immunotherapy");

            rtnMock.Setup(lookup => lookup.Get("MD"))
                .Returns("Maryland");

            //@Sarina and @Dion - Add other instances for GetTitleCase to support your unit tests.

            return rtnMock;
        }
    }
}
