using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace CancerGov.ClinicalTrials.Basic.v2.Test.SearchParams
{
    public class CountryCityStateLocationSearchParams_Test
    {
        [Fact]
        public void SetCityHasCityGetCity()
        {
            CountryCityStateLocationSearchParams locParams = new CountryCityStateLocationSearchParams();
            locParams.City = "Baltimore";

            Assert.True(locParams.IsFieldSet("City"));
            Assert.False(locParams.IsFieldSet("State"));
            Assert.False(locParams.IsFieldSet("Country"));
            Assert.Equal("Baltimore", locParams.GetFieldAsString("City"));
        }

        [Fact]
        public void SetCountryHasCountryGetCountry()
        {
            CountryCityStateLocationSearchParams locParams = new CountryCityStateLocationSearchParams();
            locParams.Country = "United States";

            Assert.True(locParams.IsFieldSet("Country"));
            Assert.False(locParams.IsFieldSet("City"));
            Assert.False(locParams.IsFieldSet("State"));
            Assert.Equal("United States", locParams.GetFieldAsString("Country"));
        }

        [Fact]
        public void SetStateHasStateGetState()
        {
            CountryCityStateLocationSearchParams locParams = new CountryCityStateLocationSearchParams();
            locParams.State = new LabelledSearchParam[] 
            {
                new LabelledSearchParam() {
                    Key = "MD",
                    Label = "Maryland"
                }
            };

            Assert.True(locParams.IsFieldSet("State"));
            Assert.False(locParams.IsFieldSet("City"));
            Assert.False(locParams.IsFieldSet("Country"));
            Assert.Equal("Maryland", locParams.GetFieldAsString("State"));
        }

        [Fact]
        public void SetStateHasStateGetStateX2()
        {
            CountryCityStateLocationSearchParams locParams = new CountryCityStateLocationSearchParams();
            locParams.State = new LabelledSearchParam[] 
            {
                new LabelledSearchParam() {
                    Key = "MD",
                    Label = "Maryland"
                },
                new LabelledSearchParam() {
                    Key = "DE",
                    Label = "Delaware"
                }
            };

            Assert.True(locParams.IsFieldSet("State"));
            Assert.False(locParams.IsFieldSet("City"));
            Assert.False(locParams.IsFieldSet("Country"));
            Assert.Equal("Delaware, Maryland", locParams.GetFieldAsString("State"));
        }

        [Fact]
        public void SetAllHasAll()
        {
            CountryCityStateLocationSearchParams locParams = new CountryCityStateLocationSearchParams();
            locParams.Country = "United States";
            locParams.City = "Baltimore";
            locParams.State = new LabelledSearchParam[] 
            {
                new LabelledSearchParam() {
                    Key = "MD",
                    Label = "Maryland"
                }
            };

            Assert.True(locParams.IsFieldSet("State"));
            Assert.True(locParams.IsFieldSet("City"));
            Assert.True(locParams.IsFieldSet("Country"));
        }


    }
}
