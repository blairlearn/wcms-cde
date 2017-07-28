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
                //Array of tests
                //TODO: put these in some kind of order
                return new[]
                {
                    //This array of objects maps to the parameters of the create method.
                    //URL at index 0, Expected object at index 1.

                    //TEST 1 - No parameters.
                    new object[] { "", new CTSSearchParams() },

                    //TEST 2 - Phrase/Keyword.
                    new object[] { "?q=chicken", new CTSSearchParams() {
                        Phrase = "chicken"
                    }},

                    //TEST 3 - Main Cancer Type
                    new object[] {"?t=C4872", new CTSSearchParams() {
                        MainType = new TerminologyFieldSearchParam() {
                            Codes = new string[] { "C4872" },
                            Label = "Breast Cancer"
                        }
                    }},

                    // TEST - Cancer subtype
                    new object[] {"?st=C7771", new CTSSearchParams() {
                        SubTypes = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C7771" },
                                Label = "Recurrent Breast Cancer"
                            }
                        }
                    }},

                    // TEST - Cancer Stage
                    new object[] {"?stg=C88375", new CTSSearchParams() {
                        SubTypes = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C88375" },
                                Label = "Stage I Breast Cancer"
                            }
                        }
                    }},

                    // TEST - Cancer subtype
                    new object[] {"?fin=C26696", new CTSSearchParams() {
                        SubTypes = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C26696" },
                                Label = "Anxiety"
                            }
                        }
                    }},

                    //TEST - Age
                    new object[] { "?a=35", new CTSSearchParams() {
                        Age = 35
                    }},

                    //TEST - Country
                    new object[] { "?lcnty=United+States", new CTSSearchParams() {
                        Country = "United States"
                    }},

                    //TEST - State
                    new object[] { "?lst=MD", new CTSSearchParams() {
                        State = new LabelledSearchParam() { 
                            Key = "MD",
                            Label = "Maryland"
                        }
                    }}, 

                    //TEST - City
                    new object[] { "?lcty=Baltimore", new CTSSearchParams() {
                        City = "Baltimore"
                    }},

                    //TEST - Hospital
                    new object[] { "?hos=M+D+Anderson+Cancer+Center", new CTSSearchParams() {
                        Hospital = "M D Anderson Cancer Center"
                    }},

                    //TEST - Principal investigator
                    new object[] { "?in=Sophia+Smith", new CTSSearchParams() {
                        Investigator = "Sophia Smith"
                    }},

                    //TEST - Lead Organization
                    new object[] { "?lo=Mayo+Clinic", new CTSSearchParams() {
                        LeadOrg = "Mayo Clinic"
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

            rtnMock.Setup(lookup => lookup.Get("MD"))
                .Returns("Maryland");

            //@Sarina and @Dion - Add other instances for GetTitleCase to support your unit tests.

            return rtnMock;
        }
    }
}
