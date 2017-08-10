using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CancerGov.ClinicalTrials.Basic.v2.SnippetControls;
using CancerGov.ClinicalTrialsAPI;

using Xunit;
using Moq;
using System.Reflection;
using System.IO;

namespace CancerGov.ClinicalTrials.Basic.v2.Test.ConfigTest
{
    public class ConfigMappingLoader
    {
        static readonly string AssemblyFileName;
        static readonly string AssemblyPath;

        static ConfigMappingLoader()
        {
            AssemblyFileName = Assembly.GetExecutingAssembly().CodeBase;
            Uri fileNameURI = new Uri(AssemblyFileName);
            AssemblyPath = Path.GetDirectoryName(fileNameURI.LocalPath);
        }

        /// <summary>
        /// Loads the paths for the test mapping file(s) from MappingFiles folder.  
        /// </summary>
        /// <param name="trialName">The name of the file to load.</param>
        /// <returns>The filepath(s) for the mapping files.</returns>
        public static string LoadMappingPath(string mappingName)
        {
            string path = Path.Combine(AssemblyPath, "MappingFiles", mappingName);
            return path;
        }

        /// <summary>
        /// Loads the lookup service given test mapping filepath(s).  
        /// </summary>
        /// <param name="trialName">The name of the file to load.</param>
        /// <returns>A mapping lookup service.</returns>
        public static TrialTermLookupService GetMappingService(string[] filePaths)
        {
            TrialTermLookupConfig config = new TrialTermLookupConfig();
            foreach(string path in filePaths)
            {
                config.MappingFiles.Add(LoadMappingPath(path));
            }
            return new TrialTermLookupService(config);
        }

        /// <summary>
        /// Gets a mock that can be used for a ITerminologyLookupService
        /// See https://github.com/moq/moq4 for more details on the mock library.
        /// </summary>
        /// <returns>A mock to be used as the zip lookup service.</returns>
        private Mock<IZipCodeGeoLookupService> GetZipLookupMock()
        {
            Mock<IZipCodeGeoLookupService> rtnMock = new Mock<IZipCodeGeoLookupService>();
            rtnMock.Setup(lookup => lookup.GetZipCodeGeoEntry("20850"))
                .Returns(new GeoLocation(39.0897, -77.1798));

            return rtnMock;
        }

        public static IEnumerable<object[]> URLParseLabels
        {
            get
            {
                // Array of tests
                return new[]
                {
                    /*// TEST 0 - No parameters
                    new object[] { "", new CTSSearchParams() {
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},
                    

                    // TEST 1 - Single item (one code) for lookup
                    new object[] {"?t=C4872", new CTSSearchParams() {
                        MainType = new TerminologyFieldSearchParam() {
                            Codes = new string[] { "C4872" },
                            Label = "Breast Cancer"
                        },
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 2 - Single item (multiple codes) for lookup
                    new object[] {"?t=C7899|C4995", new CTSSearchParams() {
                        MainType = new TerminologyFieldSearchParam() {
                            Codes = new string[] { "C4995", "C7899" },
                            Label = "Recurrent Bladder Cancer"
                        },
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},
                    
                    // TEST 3 - Single item (invalid) for lookup
                    new object[] {"?t=chicken", new CTSSearchParams() {
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 4 - Single item (multiple codes, one invalid) for lookup
                    new object[] {"?t=C4872|chicken", new CTSSearchParams() {
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 5 - Multiple items (multiple codes) for lookup
                    new object[] {"?st=C4872,C7771", new CTSSearchParams() {
                        SubTypes = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C4872" },
                                Label = "Breast Cancer"
                            },
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C7771" },
                                Label = "Recurrent Breast Cancer"
                            }
                        },
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 6 - Multiple items (multiple codes) for lookup; all have same label
                    new object[] {"?st=C4001,C7771", new CTSSearchParams() {
                        SubTypes = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C4001", "C7771" },
                                Label = "Recurrent Breast Cancer"
                            },
                        },
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 7 - Multiple items (one invalid) for lookup
                    new object[] {"?st=C4872,chicken", new CTSSearchParams() {
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 7 - Multiple item (one with multiple codes, one invalid) for lookup
                    new object[] {"?st=C4872,C7771|chicken", new CTSSearchParams() {
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 8 - Multiple item (one with multiple codes) for lookup; all have same label
                    new object[] {"?stg=C88375|C85385,C85386", new CTSSearchParams() {
                        Stages = new TerminologyFieldSearchParam[] { 
                            new TerminologyFieldSearchParam() {
                                Codes = new string[] { "C88375", "C85385", "C85386" },
                                Label = "Stage I Breast Cancer"
                            },
                        },
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},*/
                    
                    // TEST 9 - Single item (non-code) for lookup 
                    new object[] {"?tp=I", new CTSSearchParams() {
                        TrialPhases = new LabelledSearchParam[] { 
                            new LabelledSearchParam() {
                                Key = "I",
                                Label = "Phase I"
                            }
                        },
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 10 - Single item (non-code, invalid) for lookup 
                    new object[] {"?tp=chicken", new CTSSearchParams() {
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},

                    // TEST 11 - Multiple items (non-code) for lookup
                    new object[] {"?rl=2&loc=2&lst=VA,MD", new CTSSearchParams() {
                        Location = LocationType.CountryCityState,
                        LocationParams = new CountryCityStateLocationSearchParams() {
                            State = new LabelledSearchParam[] {
                                new LabelledSearchParam() {
                                    Key = "VA",
                                    Label = "Virginia"
                                },
                                new LabelledSearchParam() {
                                    Key = "MD",
                                    Label = "Maryland"
                                }
                            }
                        },
                        ResultsLinkFlag = ResultsLinkType.Advanced
                    }},

                    // TEST 12 - Multiple items (non-code, one invalid) for lookup
                    new object[] {"?tt=tt_treatment,tt_supportive_care,chicken", new CTSSearchParams() {
                        ResultsLinkFlag = ResultsLinkType.Basic,
                        Location = LocationType.None
                    }},
                };
            }
        }

        [Fact]
        public void LoadMapping()
        {
            TrialTermLookupService lookup = GetMappingService(new string[] { "EVSWithMultiple.txt", "Other.txt" });

            Assert.Equal("Breast Cancer", lookup.GetTitleCase("c4872"), new MappingsComparer());
            Assert.Equal("Stage I Breast Cancer", lookup.GetTitleCase("c88375"), new MappingsComparer());
            Assert.Equal("Stage I Breast Cancer", lookup.GetTitleCase("c85385"), new MappingsComparer());
            Assert.Equal("Stage I Breast Cancer", lookup.GetTitleCase("c85386"), new MappingsComparer());
            Assert.Equal("Virginia", lookup.Get("VA"), new MappingsComparer());
            Assert.Equal("Washington, D.C.", lookup.GetTitleCase("DC"), new MappingsComparer());
            Assert.Equal("Maryland", lookup.GetTitleCase("MD"), new MappingsComparer());
            Assert.Equal("Phase I", lookup.GetTitleCase("I"), new MappingsComparer());
            Assert.Equal("Phase II", lookup.GetTitleCase("II"), new MappingsComparer());
            Assert.Equal("Treatment", lookup.GetTitleCase("tt_treatment"), new MappingsComparer());
            Assert.Equal("Supportive Care", lookup.GetTitleCase("tt_supportive_care"), new MappingsComparer());
        }

        
        [Fact]
        public void SingleLookup()
        {
            TrialTermLookupService lookup = GetMappingService(new string[] { "EVSWithMultiple.txt", "Other.txt" });
            Assert.Equal(true, lookup.MappingContainsKey("c4872"));
            Assert.Equal("Breast Cancer", lookup.GetTitleCase("c4872"), new MappingsComparer());
        }

        [Fact]
        public void SingleLookupWithError()
        {
            TrialTermLookupService lookup = GetMappingService(new string[] { "EVSWithMultiple.txt", "Other.txt" });
            Assert.Equal(false, lookup.MappingContainsKey("c4872,chicken"));
        }

        [Fact]
        public void MultipleLookup()
        {
            TrialTermLookupService lookup = GetMappingService(new string[] { "EVSWithMultiple.txt", "Other.txt" });
            Assert.Equal("Stage I Breast Cancer", lookup.GetTitleCase("c88375,c88375"), new MappingsComparer());
        }

        [Fact]
        public void MultipleLookupWithError()
        {
            TrialTermLookupService lookup = GetMappingService(new string[] { "EVSWithMultiple.txt", "Other.txt" });
            Assert.Equal(false, lookup.MappingContainsKey("c88375,chicken,c88375"));
        }
        
        [Theory, MemberData("URLParseLabels")]
        public void Create(string url, CTSSearchParams expected)
        {
            TrialTermLookupService lookupSvc = GetMappingService(new string[] { "EVSWithMultiple.txt", "Other.txt" });
            var mockZipLookupSvc = GetZipLookupMock();

            // Create a new instance of the factory, passing in the loaded lookup service and
            // the mock zip lookup service.
            CTSSearchParamFactory factory = new CTSSearchParamFactory(lookupSvc, mockZipLookupSvc.Object);

            //Get the results of parsing the URL.
            CTSSearchParams actual = factory.Create(url);

            //Test the actual result to the expected.
            Assert.Equal(expected, actual, new CTSSearchParamsComparer());
        }
    }
}
