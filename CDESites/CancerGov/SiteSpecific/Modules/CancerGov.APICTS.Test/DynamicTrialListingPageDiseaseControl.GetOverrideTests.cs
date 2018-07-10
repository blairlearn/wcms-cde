using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Moq;
using System.Reflection;
using System.IO;
using CancerGov.ClinicalTrials.Basic.v2.Lookups;
using CancerGov.ClinicalTrials.Basic.v2.Test.ConfigTest;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{
    public class DynamicTrialListingPageDiseaseControl_Tests
    {
        static readonly string AssemblyFileName;
        static readonly string AssemblyPath;

        static DynamicTrialListingPageDiseaseControl_Tests()
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
        /// <param name="withOverrides">Whether the mapping file loads the EVS mappings or the override mappings.</param>
        /// <returns>A mapping lookup service.</returns>
        public DynamicTrialListingFriendlyNameMapper GetFriendlyNameMappingService(string evsFilePath, string overrideFilePath, bool withOverrides)
        {
            return new DynamicTrialListingFriendlyNameMapper(LoadMappingPath(evsFilePath),
                                                         LoadMappingPath(overrideFilePath),
                                                         withOverrides);
        }

        /// <summary>
        /// Loads the lookup service given test mapping filepaths.  
        /// </summary>
        /// <param name="evsFilePath">The name of the EVS label mapping file to load.</param>
        /// <param name="overrideFilePath">The name of the EVS friendly name mapping file to load.</param>
        /// <param name="tokensFilePath">The name of the tokens mapping file to load.</param>
        /// <returns>A mapping lookup service.</returns>
        public DynamicTrialListingMapper GetLabelMappingService(string evsFilePath, string overrideFilePath, string tokensFilePath)
        {
            return new DynamicTrialListingMapper(LoadMappingPath(evsFilePath),
                                                 LoadMappingPath(overrideFilePath),
                                                 LoadMappingPath(tokensFilePath));
        }


        /// <summary>
        /// Replaces the Placeholder Codes (or text) with Override Labels
        /// </summary>
        /// <param name="codes"></param>
        /// <returns>A string with the override text</returns>
        private string GetOverride(DynamicTrialListingFriendlyNameMapper FriendlyNameMapping,
                                   DynamicTrialListingFriendlyNameMapper FriendlyNameWithOverridesMapping,
                                   DynamicTrialListingMapper LabelMapping,
                                   string valToOverride,
                                   bool needsTitleCase)
        {
            // Get friendly name to c-code mapping
            if (FriendlyNameWithOverridesMapping.MappingContainsFriendlyName(valToOverride.ToLower()))
            {
                valToOverride = FriendlyNameWithOverridesMapping.GetCodeFromFriendlyName(valToOverride);
            }
            else
            {
                if (FriendlyNameMapping.MappingContainsFriendlyName(valToOverride.ToLower()))
                {
                    valToOverride = FriendlyNameMapping.GetCodeFromFriendlyName(valToOverride);
                }
            }

            string overrideText = "";

            // Add check for whether override/EVS mapping has all of the codes.
            // If so, keep as is. If not, split and find the first match.

            // If combination of codes is in label mappings, set override
            if (LabelMapping.MappingContainsKey(valToOverride))
            {
                if (needsTitleCase)
                {
                    overrideText = LabelMapping.GetTitleCase(valToOverride);
                }
                else
                {
                    overrideText = LabelMapping.Get(valToOverride);
                }
            }
            // Raise 404 error if overrides aren't found
            else
            {
                throw new Exception(String.Format("" + "Invalid parameter in dynamic listing page: {0} does not have override", valToOverride));
            }

            return overrideText;
        }

        [Fact]
        public void GetOverride_EVSCodeTest()
        {
            DynamicTrialListingMapper labelMapper = GetLabelMappingService("DLPEVSRollupMapping.txt", "DLPEVSOverrideMapping.txt", "DLPTokensMapping.txt");
            DynamicTrialListingFriendlyNameMapper friendlyNameMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", false);
            DynamicTrialListingFriendlyNameMapper friendlyNameWithOverridesMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", true);

            Assert.Equal("Breast Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "c4872", true), new MappingsComparer());

            Assert.Equal("Stage II Breast Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "c7768", true), new MappingsComparer());

            Assert.Equal("Ependymoma", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "c9092,c3017", true), new MappingsComparer());

            Assert.Equal("Short Limb Dwarfism-Saddle Nose-Spinal Alterations-Metaphyseal Striation Syndrome", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "c92206", true), new MappingsComparer());
        }

        [Fact]
        public void GetOverride_EVSFriendlyNameTest()
        {
            DynamicTrialListingMapper labelMapper = GetLabelMappingService("DLPEVSRollupMapping.txt", "DLPEVSOverrideMapping.txt", "DLPTokensMapping.txt");
            DynamicTrialListingFriendlyNameMapper friendlyNameMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", false);
            DynamicTrialListingFriendlyNameMapper friendlyNameWithOverridesMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", true);

            Assert.Equal("Breast Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "breast-cancer", true), new MappingsComparer());

            Assert.Equal("Stage II Breast Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "stage-ii-breast-cancer", true), new MappingsComparer());
        }

        [Fact]
        public void GetOverride_OverrideCodeTest()
        {
            DynamicTrialListingMapper labelMapper = GetLabelMappingService("DLPEVSRollupMapping.txt", "DLPEVSOverrideMapping.txt", "DLPTokensMapping.txt");
            DynamicTrialListingFriendlyNameMapper friendlyNameMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", false);
            DynamicTrialListingFriendlyNameMapper friendlyNameWithOverridesMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", true);

            Assert.Equal("Thyroid Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "c118827,c4815", true), new MappingsComparer());

            Assert.Equal("Kidney Small Cell Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "c116317", true), new MappingsComparer());
        }

        [Fact]
        public void GetOverride_OverrideFriendlyNameTest()
        {
            DynamicTrialListingMapper labelMapper = GetLabelMappingService("DLPEVSRollupMapping.txt", "DLPEVSOverrideMapping.txt", "DLPTokensMapping.txt");
            DynamicTrialListingFriendlyNameMapper friendlyNameMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", false);
            DynamicTrialListingFriendlyNameMapper friendlyNameWithOverridesMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", true);

            Assert.Equal("Thyroid Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "thyroid-cancer", true), new MappingsComparer());

            Assert.Equal("Thyroid Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "thyroid-gland-cancer", true), new MappingsComparer());

            Assert.Equal("Kidney Small Cell Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "kidney-small-cell-cancer", true), new MappingsComparer());
        }

        public void GetOverride_EVSFriendlyNameWithOverrideLabelTest()
        {
            DynamicTrialListingMapper labelMapper = GetLabelMappingService("DLPEVSRollupMapping.txt", "DLPEVSOverrideMapping.txt", "DLPTokensMapping.txt");
            DynamicTrialListingFriendlyNameMapper friendlyNameMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", false);
            DynamicTrialListingFriendlyNameMapper friendlyNameWithOverridesMapper = GetFriendlyNameMappingService("DLPFriendlyNameMapping.txt", "DLPFriendlyNameOverrideMapping.txt", true);

            Assert.Equal("Thyroid Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "thyroid-gland-cancer", true), new MappingsComparer());

            Assert.Equal("Kidney Small Cell Cancer", GetOverride(friendlyNameMapper, friendlyNameWithOverridesMapper, labelMapper, "kidney-small-cell-carcinoma", true), new MappingsComparer());
        }
    }
}
