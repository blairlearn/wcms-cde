using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CancerGov.ClinicalTrials.Basic.v2.SnippetControls;
using CancerGov.ClinicalTrialsAPI;

using Xunit;
using Moq;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    public partial class TrialSiteLocationFilterExtension_Test
    {

        public static IEnumerable<object[]> AtNIHFilteringData 
        {
            get
            {
                // Array of tests
                return new[]
                {
                    //Empty Set
                    new object[] {
                        new ClinicalTrial() {
                            Sites = new List<ClinicalTrial.StudySite>()
                        },
                        new ClinicalTrial.StudySite[] { }
                    },
                    //
                    new object[] { 
                        new ClinicalTrial() {
                            Sites = new List<ClinicalTrial.StudySite>()
                        }, 
                        new ClinicalTrial.StudySite[] {

                        }
                    },
                };
            }
        }

        [Theory, MemberData("AtNIHFilteringData")]
        public void FilterByAtNIH(ClinicalTrial trial, IEnumerable<ClinicalTrial.StudySite> expectedSites)
        {

            CTSSearchParams searchParams = new CTSSearchParams() { Location = LocationType.AtNIH };
            IEnumerable<ClinicalTrial.StudySite> actual = trial.FilterSitesByLocation(searchParams);

        }

        private static IEnumerable<ClinicalTrial.StudySite> GetNIHSites() 
        {
            List<ClinicalTrial.StudySite> rtnSites = new List<ClinicalTrial.StudySite>();

            return rtnSites;
        }
        
    }
}
