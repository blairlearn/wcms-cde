using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Common.Logging;

namespace CancerGov.ClinicalTrials.Basic.v2.Lookups
{
    public class DynamicTrialListingMapper
    {
        private static string _evsMappingFile = null;
        private static string _overrideMappingFile = null;
        private static string _tokensMappingFile = null;
        private Dictionary<string, MappingItem> Mappings = new Dictionary<string, MappingItem>();
        private Dictionary<string, MappingItem> MappingsWithOverrides = new Dictionary<string, MappingItem>();
        private HashSet<string> Tokens = new HashSet<string>();

        public DynamicTrialListingMapper(string evsMappingFilepath, string overrideMappingFilepath, string tokensMappingFilepath)
        {
            _evsMappingFile = evsMappingFilepath;
            _overrideMappingFile = overrideMappingFilepath;
            _tokensMappingFile = tokensMappingFilepath;
            LoadMappings();
        }

        private void LoadMappings()
        {
           // Load and store EVS mappings
            Mappings = GetDictionary(_evsMappingFile, false);

            // Load and store EVS with Overrides mappings
            MappingsWithOverrides = GetDictionary(_overrideMappingFile, true);

            // Load and store tokens
            Tokens = GetTokens(_tokensMappingFile);
        }


        private static Dictionary<string, MappingItem> GetDictionary(string filePath, bool isOverride)
        {
            Dictionary<string, MappingItem> dict = new Dictionary<string, MappingItem>();
            try
            {

                // If file exists, use streamreader to load mappings into dictionary
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        // Lowercase c-codes (for comparison to codes from URL parameters later)
                        parts[0] = parts[0].ToLower();

                        // Sort c-codes alphabetically/numerically if there are multiple 
                        if (parts[0].Contains(","))
                        {
                            string[] split = parts[0].Split(',');
                            Array.Sort(split);
                            string newKey = string.Join(",", split);
                            parts[0] = newKey;
                        }

                        // Create MappingItem object for mapping
                        MappingItem item = new MappingItem();
                        item.Codes = parts[0].Split(',').ToList();
                        item.Text = parts[1];
                        item.IsOverride = isOverride;

                        // Add mapping to dictionary if it isn't already present
                        if (!dict.ContainsKey(parts[0]))
                        {
                            dict.Add(parts[0], item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(DynamicTrialListingMappingService)).ErrorFormat("Error while getting the mapping file.", ex);
            }

            return dict;
        }

        private static HashSet<string> GetTokens(string filePath)
        {
            HashSet<string> tokensSet = new HashSet<string>();
            try
            {
                if (File.Exists(filePath))
                {
                    // If file exists, use streamreader to load mappings into dictionary
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            tokensSet.Add(line);
                        }
                    }
                }
                else
                {
                    // Throw exception if file doesn't exist
                    LogManager.GetLogger(typeof(DynamicTrialListingMappingService)).ErrorFormat("Mapping file '{0}' not found.", filePath);
                    throw new FileNotFoundException(filePath);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(DynamicTrialListingMappingService)).ErrorFormat("Error while getting the mapping file.", ex);
            }

            return tokensSet;
        }

        /// <summary>
        /// Gets the title-cased term. (I.E. first letter of each word is upper case)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The display label</returns>
        public string GetTitleCase(string value)
        {
            if (MappingsWithOverrides.ContainsKey(value))
            {
                return MappingsWithOverrides[value].Text;
            }
            else if (Mappings.ContainsKey(value))
            {
                return Mappings[value].Text;
            }
            else
            {
                string[] splitIDs = value.Split(new char[] { ',' });

                return Mappings.FirstOrDefault(m => m.Key.Equals(splitIDs[0]) || m.Key.Split(new char[] { ',' }).Any(k => k == splitIDs[0])).Value.Text;
            }

        }

        /// <summary>
        /// Gets the non-title-cased term.  This accounts for special initials, proper nouns and roman numerals though.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The display label</returns>
        public string Get(string value)
        {
            string overrideText = "";

            if (MappingsWithOverrides.ContainsKey(value))
            {
                overrideText = MappingsWithOverrides[value].Text;
            }
            else if (Mappings.ContainsKey(value))
            {
                overrideText = Mappings[value].Text;
            }
            else
            {
                string[] splitIDs = value.Split(new char[] { ',' });

                overrideText = Mappings.FirstOrDefault(m => m.Key.Equals(splitIDs[0]) || m.Key.Split(new char[] { ',' }).Any(k => k == splitIDs[0])).Value.Text;
            }

            // Split apart string on known values (space and dash) for comparison to tokens
            string[] split = overrideText.Split(new char[] { ' ', '-' });

            // For use in formatter
            int i = 0;
            List<string> keepToken = new List<string>();

            foreach (string part in split)
            {
                if (Tokens.Contains(part))
                {
                    // If do-not-replace tokens contains this string, replace with value for formatter
                    // and add token to list for later replace
                    overrideText = overrideText.Replace(part, "{" + i.ToString() + "}");
                    keepToken.Add(part);
                    i++;
                }
            }

            overrideText = overrideText.ToLower();
            overrideText = String.Format(overrideText, keepToken.ToArray());

            return overrideText;
        }

        /// <summary>
        /// Checks to see if the lookup contains an entry for the ID(s)
        /// </summary>
        /// <param name="key">The ID(s) or labels to lookup</param>
        /// <param name="withOverrides">Whether or not to check the overrides mapping.</param>
        /// <returns>True or false based on the existance of the ID(s) in the lookup</returns>
        public bool MappingContainsKey(string code)
        {
            code = code.ToLower();

            if (MappingsWithOverrides.ContainsKey(code))
            {
                return MappingsWithOverrides.ContainsKey(code);
            }

            // Split the given codes apart, if there are multiple.
            string[] splitIDs = code.Split(new char[] { ',' });
            List<string> splitIDFriendlyNames = new List<string>();

            // Loop through all the codes.
            foreach (string ID in splitIDs)
            {
                if (!Mappings.Keys.Any(k => k.Contains(ID)))
                {
                    // If any code given is not contained in a key in the mapping, return false.
                    return false;
                }
                else
                {
                    if (!IsValidCCode(ID))
                    {
                        LogManager.GetLogger(typeof(DynamicTrialListingFriendlyNameMappingService)).ErrorFormat("Invalid parameter in dynamic listing page: {0} is not a valid c-code", ID);
                        NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("DynamicTrialListingFriendlyNameMappingService", 404, "Invalid parameter in dynamic listing page: value given is not a valid c-code");
                    }

                    string containsCode1 = "," + ID;
                    string containsCode2 = ID + ",";

                    // Add all of the friendly names for any entries whose key contains the current code to a list for later comparison.
                    splitIDFriendlyNames.AddRange(Mappings.Where(m => m.Key.Equals(ID) || m.Key.Split(new char[] { ',' }).Any(k => k == ID))
                                                            .GroupBy(g => g.Key)
                                                            .Select(kvp => kvp.First().Value.Text)
                                                            .ToList());

                    // Add all of the friendly names for any entries whose key contains the current code to a list for later comparison.
                    //splitIDFriendlyNames.AddRange(Mappings.Where(m => m.Key.Contains(ID)).Select(kvp => kvp.Value.Text).ToList());
                }
            }

            if (splitIDFriendlyNames.Any(f => f != splitIDFriendlyNames[0]))
            {
                // If the friendly names associated with keys that contain the given codes are not the same, return false.
                return false;
            }
            else
            {
                // If all of the friendly names associated with keys that contain the given codes match, return true.
                return true;
            }
        }

        public bool IsValidCCode(string code)
        {
            string pattern = @"[c][0-9]{4}";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

            return r.IsMatch(code);
        }
    }
}
