using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    /// <summary>
    /// Tests for the CTSearchParamFactory
    /// </summary>
    public class CTSSearchParamFactory_Test
    {
        //Test cases for Create test method.
        public static IEnumerable<object[]> URLParsingData {
            get
            {
                //Array of tests
                return new[]
                {
                    //This array of objects maps to the parameters of the create method.
                    //URL at index 0, Expected object at index 1.
                    new object[] { "", new CTSSearchParams() }
                };
            }
        }

        /// <summary>
        /// Main test method.  Takes in a URL and an expected object and sees if Create(url) returns that object.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="expected"></param>
        [Theory, MemberData("URLParsingData")]
        public void Create(string url, CTSSearchParams expected)
        {
            //Create a new instance of the factory
            CTSSearchParamFactory factory = new CTSSearchParamFactory();

            //Get the results of parsing the URL
            CTSSearchParams actual = factory.Create(url);

            //Test the actual result to the expected.  NOTE: If you add fields to the CTSSearchParams, you need
            //to also modify the comparer
            Assert.Equal(expected, actual, new CTSSearchParamsComparer());
        }
    }
}
