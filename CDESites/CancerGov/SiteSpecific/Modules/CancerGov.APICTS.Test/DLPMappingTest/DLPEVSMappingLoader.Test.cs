using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CancerGov.ClinicalTrials.Basic.v2.Lookups;
using CancerGov.ClinicalTrials.Basic.v2.Test.ConfigTest;
using Xunit;
using Moq;
using System.Reflection;
using System.IO;

namespace CancerGov.ClinicalTrials.Basic.v2.Test.DLPMappingTest
{
    public class DLPEVSMappingLoader
    {
        static readonly string AssemblyFileName;
        static readonly string AssemblyPath;

        static DLPEVSMappingLoader()
        {
            AssemblyFileName = Assembly.GetExecutingAssembly().CodeBase;
            Uri fileNameURI = new Uri(AssemblyFileName);
            AssemblyPath = Path.GetDirectoryName(fileNameURI.LocalPath);
        }

        /// <summary>
        /// Loads the paths for the test mapping file(s) from MappingFiles folder.  
        /// </summary>
        /// <param name="mappingName">The name of the file to load.</param>
        /// <returns>The filepath(s) for the mapping files.</returns>
        public static string LoadMappingPath(string mappingName)
        {
            string path = Path.Combine(AssemblyPath, "MappingFiles", mappingName);
            return path;
        }

        /// <summary>
        /// Loads the lookup service given test mapping filepaths.  
        /// </summary>
        /// <param name="evsFilePath">The name of the EVS label mapping file to load.</param>
        /// <param name="overrideFilePath">The name of the EVS friendly name mapping file to load.</param>
        /// <param name="tokensFilePath">The name of the tokens mapping file to load.</param>
        /// <returns>A mapping lookup service.</returns>
        public DynamicTrialListingMapper GetMappingService(string evsFilePath, string overrideFilePath, string tokensFilePath)
        {
            return new DynamicTrialListingMapper(LoadMappingPath(evsFilePath),
                                                 LoadMappingPath(overrideFilePath),
                                                 LoadMappingPath(tokensFilePath));
        }

        [Fact]
        public void LoadMapping()
        {
            DynamicTrialListingMapper mapper = GetMappingService("DLPEVSRollupMapping.txt", "DLPEVSOverrideMapping.txt", "DLPTokensMapping.txt");

            Assert.Equal("Breast Cancer", mapper.GetTitleCase("c4872"), new MappingsComparer());
            Assert.Equal("stage II breast cancer", mapper.Get("c7768,c139538,c139569"), new MappingsComparer());
            Assert.Equal("Thyroid Cancer", mapper.GetTitleCase("c118827,c4815"), new MappingsComparer());
            Assert.Equal("kidney small cell cancer", mapper.Get("c116317"), new MappingsComparer());
            Assert.Equal("Ependymoma", mapper.GetTitleCase("c8578"), new MappingsComparer());
        }

        [Fact]
        public void MappingContainsKeys()
        {
            DynamicTrialListingMapper mapper = GetMappingService("DLPEVSRollupMapping.txt", "DLPEVSOverrideMapping.txt", "DLPTokensMapping.txt");
            Assert.Equal(true, mapper.MappingContainsKey("c4872"));
            Assert.Equal(true, mapper.MappingContainsKey("c118827,c4815"));
        }
    }
}
