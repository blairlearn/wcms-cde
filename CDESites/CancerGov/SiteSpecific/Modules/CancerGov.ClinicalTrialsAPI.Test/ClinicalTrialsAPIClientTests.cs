using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace CancerGov.ClinicalTrialsAPI.Test
{
    public class ClinicalTrialsAPIClientTests
    {
        [Fact]
        public void TestGet()
        {
            //This is more of an integration test to get things worked out.  It would be nice to have some sort of adapter so that we can mock the service.
            //but for now we need to get this moving.

            ClinicalTrialsAPIClient client = new ClinicalTrialsAPIClient("clinicaltrialsapi.cancer.gov");

            ClinicalTrial trial = client.Get("NCT02194738");

            Assert.Equal("NCT02194738", trial.NCTID);
        }

        [Fact]
        public void TestListNoParams()
        {
            //This is more of an integration test to get things worked out.  It would be nice to have some sort of adapter so that we can mock the service.
            //but for now we need to get this moving.

            ClinicalTrialsAPIClient client = new ClinicalTrialsAPIClient("clinicaltrialsapi.cancer.gov");

            ClinicalTrialsCollection results = client.List();

            //This is a safe test range. 500-100,000 trials
            Assert.InRange<int>(3000, 500, 100000);
        }

        [Fact]
        public void TestListIncludeFields()
        {
            //This is more of an integration test to get things worked out.  It would be nice to have some sort of adapter so that we can mock the service.
            //but for now we need to get this moving.

            ClinicalTrialsAPIClient client = new ClinicalTrialsAPIClient("clinicaltrialsapi.cancer.gov");

            ClinicalTrialsCollection results = client.List(includeFields: new string[]{ "brief_title" } );

            //Every record should have one of these, so there should never be a case where these will fail if our listing worked.
            Assert.NotNull(results.Trials[0].BriefTitle);
            Assert.Null(results.Trials[0].NCIID); 
            
        }


    }
}
