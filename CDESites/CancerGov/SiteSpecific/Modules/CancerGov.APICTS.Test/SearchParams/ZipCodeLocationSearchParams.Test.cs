using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace CancerGov.ClinicalTrials.Basic.v2.Test.SearchParams
{
    
    public class ZipCodeLocationSearchParams_Test
    {
        [Fact]
        public void GetAndSetZipCode()
        {
            ZipCodeLocationSearchParams locParam = new ZipCodeLocationSearchParams();

            locParam.ZipCode = "20852";
            Assert.True(locParam.IsFieldSet("ZipCode"));
            Assert.Equal("20852", locParam.GetFieldAsString("ZipCode"));
        }

        [Fact]
        public void GetAndSetZipRadius()
        {
            ZipCodeLocationSearchParams locParam = new ZipCodeLocationSearchParams();

            locParam.ZipRadius = 100;
            Assert.True(locParam.IsFieldSet("ZipRadius"));
            Assert.Equal("100", locParam.GetFieldAsString("ZipRadius"));
        }

        [Fact]
        public void NoFieldIsSet()
        {
            ZipCodeLocationSearchParams locParam = new ZipCodeLocationSearchParams();
            Assert.False(locParam.IsFieldSet("ZipCode"));
            Assert.False(locParam.IsFieldSet("ZipRadius"));
        }

        [Fact]
        public void BogusField()
        {
            ZipCodeLocationSearchParams locParam = new ZipCodeLocationSearchParams();
            Assert.Equal("Error Retrieving Field", locParam.GetFieldAsString("None"));
        }
    }
}
