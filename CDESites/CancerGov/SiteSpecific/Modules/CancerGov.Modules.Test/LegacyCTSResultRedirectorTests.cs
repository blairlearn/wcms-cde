using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using CancerGov.HttpModules;
using Xunit;

namespace CancerGov.Modules.Test
{
    public class LegacyCTSResultRedirectorTests
    {
        // Create new redirector object for testing
        LegacyCTSResultRedirector fakeRedirector = new LegacyCTSResultRedirector();

        // Test one-item array
        [Fact]
        private void TestSplit1()
        {
            String[] vals = fakeRedirector.SplitArrayOnPipes("C4872");
            Assert.Equal(vals[0], "C4872");
            Assert.Equal(vals.Length, 1);
        }

        // Test two-item array
        [Fact]
        private void TestSplit2()
        {
            String[] vals = fakeRedirector.SplitArrayOnPipes("C4872|breast_carcinoma");
            Assert.Equal(vals[0], "C4872");
            Assert.Equal(vals[1], "breast_carcinoma");
            Assert.Equal(vals.Length, 2);
        }

        // Test three-item array
        [Fact]
        private void TestSplit3()
        {
            String[] vals = fakeRedirector.SplitArrayOnPipes("C89636|C4006|stage_iv_uterine_corpus_cancer");
            Assert.Equal(vals[0], "C89636");
            Assert.Equal(vals[1], "C4006");
            Assert.Equal(vals[2], "stage_iv_uterine_corpus_cancer");
            Assert.Equal(vals.Length, 3);
        }

        // Test valid C-code data
        [Theory]
        [InlineData("C4872")]
        [InlineData("C89636,C4006")]
        [InlineData("C89636%7CC4006")]
        private void TestIsNotLegacyData(string val)
        {
            Assert.False(fakeRedirector.IsLegacyValue(val));
        }

        // Test legacy (non C-code) values
        [Theory]
        [InlineData("breast")]
        [InlineData("breast_carcinoma")]
        [InlineData("i")]
        private void TestIsLegacyData(string val)
        {
            Assert.True(fakeRedirector.IsLegacyValue(val));
        }

    }
}
