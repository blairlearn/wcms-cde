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
        public static IEnumerable<object[]> HospitalFilteringData
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
                            Location = LocationType.Hospital,
                            LocationParams = new HospitalLocationSearchParams() {
                                Hospital = "Mayo"
                            }
                        },
                        new ClinicalTrial.StudySite[] { }
                    }
                };
            }
        }


        [Theory, MemberData("HospitalFilteringData")]
        public void FilterByHospital(ClinicalTrial trial, CTSSearchParams searchParams, IEnumerable<ClinicalTrial.StudySite> expectedSites)
        {
            SnippetControls.TrialVelocityTools tvt = new SnippetControls.TrialVelocityTools();
            IEnumerable<ClinicalTrial.StudySite> actual = tvt.GetFilteredLocations(trial, searchParams);

            //TODO: Implement this.
            Assert.Equal(true, false);
            //Assert.Equal(expectedSites, actual, new ClinicalTrialsAPI.Test.StudySiteComparer());
        }

    }
}
