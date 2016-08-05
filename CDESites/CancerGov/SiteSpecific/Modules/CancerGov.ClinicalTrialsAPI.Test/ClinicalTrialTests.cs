using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

using Newtonsoft.Json;

namespace CancerGov.ClinicalTrialsAPI.Test
{
    public class ClinicalTrialTests
    {
        static readonly string AssemblyFileName;
        static readonly string AssemblyPath;

        static ClinicalTrialTests()
        {
            AssemblyFileName = Assembly.GetExecutingAssembly().CodeBase;
            Uri fileNameURI = new Uri(AssemblyFileName);
            AssemblyPath = Path.GetDirectoryName(fileNameURI.LocalPath);
        }

        [Fact]
        public void TestDeserialize()
        {
            string path = Path.Combine(AssemblyPath, "TrialExamples", "NCT02194738.json");
            ClinicalTrial trial = JsonConvert.DeserializeObject<ClinicalTrial>(
                File.ReadAllText(path)
            );

            Assert.Equal("NCT02194738", trial.NCTID);
            //TODO: Check all the fields.
        }
    }
}
