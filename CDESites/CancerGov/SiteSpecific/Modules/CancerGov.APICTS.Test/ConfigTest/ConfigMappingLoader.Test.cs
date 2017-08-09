using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CancerGov.ClinicalTrials.Basic.v2.SnippetControls;
using CancerGov.ClinicalTrialsAPI;

using Xunit;
using Moq;
using System.Reflection;
using System.IO;

namespace CancerGov.ClinicalTrials.Basic.v2.Test.ConfigTest
{
    public class ConfigMappingLoader
    {
        static readonly string AssemblyFileName;
        static readonly string AssemblyPath;

        static ConfigMappingLoader()
        {
            AssemblyFileName = Assembly.GetExecutingAssembly().CodeBase;
            Uri fileNameURI = new Uri(AssemblyFileName);
            AssemblyPath = Path.GetDirectoryName(fileNameURI.LocalPath);
        }

        /// <summary>
        /// Loads a test mapping file from MappingFiles folder.  
        /// </summary>
        /// <param name="trialName">The name of the file to load.</param>
        /// <returns></returns>
        public static string LoadMappingPath(string mappingName)
        {
            string path = Path.Combine(AssemblyPath, "MappingFiles", mappingName);
            return path;
        }

        public static TrialTermLookupService GetMappingService(string[] filePaths)
        {
            TrialTermLookupConfig config = new TrialTermLookupConfig();
            foreach(string path in filePaths)
            {
                config.MappingFiles.Add(LoadMappingPath(path));
            }
            return new TrialTermLookupService(config);
        }

        [Fact]
        public void LoadMapping()
        {
            TrialTermLookupService lookup = GetMappingService(new string[] { "EVSWithMultiple.txt", "Other.txt" });

            Assert.Equal("Breast Cancer", lookup.GetTitleCase("c4872"), new MappingsComparer());
            Assert.Equal("Stage I Breast Cancer", lookup.GetTitleCase("c88375"), new MappingsComparer());
            Assert.Equal("Stage I Breast Cancer", lookup.GetTitleCase("c85385"), new MappingsComparer());
            Assert.Equal("Stage I Breast Cancer", lookup.GetTitleCase("c85386"), new MappingsComparer());
            Assert.Equal("Virginia", lookup.Get("VA"), new MappingsComparer());
            Assert.Equal("Washington, D.C.", lookup.GetTitleCase("DC"), new MappingsComparer());
            Assert.Equal("Maryland", lookup.GetTitleCase("MD"), new MappingsComparer());
            Assert.Equal("Phase I", lookup.GetTitleCase("I"), new MappingsComparer());
        }

        /*
        [Fact]
        public void LoadMapping()
        {
            TrialTermLookupConfig config = new TrialTermLookupConfig();
            config.MappingFiles.Add(LoadMappingPath("foo.txt"));
            TrialTermLookupService lookup = new TrialTermLookupService(config);

            Assert.Equal("chicken", lookup.GetTitleCase("Cchicken"));
        }

        [Fact]
        public void LoadMapping()
        {
            TrialTermLookupConfig config = new TrialTermLookupConfig();
            config.MappingFiles.Add(LoadMappingPath("foo.txt"));
            TrialTermLookupService lookup = new TrialTermLookupService(config);

            Assert.Equal("chicken", lookup.GetTitleCase("Cchicken"));
        }*/

    }
}
