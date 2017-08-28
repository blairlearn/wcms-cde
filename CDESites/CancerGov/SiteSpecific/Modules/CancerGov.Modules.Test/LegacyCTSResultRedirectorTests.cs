using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xunit;

namespace CancerGov.Modules.Test
{
    public class LegacyCTSResultRedirectorTests
    {

        // TODO: fix references. For some reason, I'm not able to reach LegacyCTSResultsRedirector from here. 
        // Can't create a new LegacyCTSResultsRedirector object to test the methods there. Adding
        // references doesn't help. Copying and pasting the existing methods for now, but will need to figure this 
        // out later since this isn's really testing anything. 

        // Test one-item array
        [Fact]
        private void TestSplit1()
        {
            String[] vals = SplitArrayOnPipes("C4872");
            Assert.Equal(vals[0], "C4872");
            Assert.Equal(vals.Length, 1);
        }

        // Test two-item array
        [Fact]
        private void TestSplit2()
        {
            String[] vals = SplitArrayOnPipes("C4872|breast_carcinoma");
            Assert.Equal(vals[0], "C4872");
            Assert.Equal(vals[1], "breast_carcinoma");
            Assert.Equal(vals.Length, 2);
        }

        // Test three-item array
        [Fact]
        private void TestSplit3()
        {
            String[] vals = SplitArrayOnPipes("C89636|C4006|stage_iv_uterine_corpus_cancer");
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
            Assert.False(IsLegacyValue(val));
        }

        // Test legacy (non C-code) values
        [Theory]
        [InlineData("breast")]
        [InlineData("breast_carcinoma")]
        [InlineData("i")]
        private void TestIsLegacyData(string val)
        {
            Assert.True(IsLegacyValue(val));
        }

        /// <summary>
        /// Returns an array of values from a pipe-delimited string.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public String[] SplitArrayOnPipes(string val)
        {
            List<string> vals = new List<string>();
            string[] valArray = vals.ToArray();
            char[] delimiterChars = { '|' };

            // Check that the parameter has a value. 
            if (!string.IsNullOrWhiteSpace(val))
            {
                // Create an array of pipe-separated values.
                valArray = val.Split(delimiterChars);
            }
            return valArray;
        }

        /// <summary>
        /// Checks for a non-C-Code pattern in a given string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsLegacyValue(string str)
        {
            // Regex pattern: any string that contains an underscore or a letter other than "c".
            string pattern = @"[ab-zAB-Z_]$";
            Regex rgx = new Regex(pattern);
            return rgx.IsMatch(str);
        }

    }
}
