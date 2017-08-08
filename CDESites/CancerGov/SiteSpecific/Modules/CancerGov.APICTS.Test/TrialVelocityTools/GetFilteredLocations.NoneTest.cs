using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CancerGov.ClinicalTrials.Basic.v2.SnippetControls;
using CancerGov.ClinicalTrialsAPI;

using Xunit;
using Moq;

namespace CancerGov.ClinicalTrials.Basic.v2.Test.TrialVelocityTools
{
    public partial class GetFilteredLocations
    {
        public static IEnumerable<object[]> NoneFilteringData
        {
            get
            {
                // Array of tests
                return new[]
                {
                    //Empty Set
                    new object[] {
                        new ClinicalTrial(),
                        new CTSSearchParams() {
                            Location = LocationType.None
                        },
                        new ClinicalTrial.StudySite[] { }
                    }
                };
            }
        }


        [Theory, MemberData("NoneFilteringData")]
        public void FilterByNone(ClinicalTrial trial, CTSSearchParams searchParams, IEnumerable<ClinicalTrial.StudySite> expectedSites)
        {
            SnippetControls.TrialVelocityTools tvt = new SnippetControls.TrialVelocityTools();
            IEnumerable<ClinicalTrial.StudySite> actual = tvt.GetFilteredLocations(trial, searchParams);

            Assert.Equal(expectedSites, actual, new ClinicalTrialsAPI.Test.StudySiteComparer());
        }

    }
}
