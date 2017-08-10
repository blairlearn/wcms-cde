using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class TrialTermLookupService: ITerminologyLookupService
    {
        private TrialTermLookupConfig _config = null;
        private Dictionary<string, string> _mappingDict = new Dictionary<string, string>();

        public TrialTermLookupService(TrialTermLookupConfig config)
        {
            _config = config;
            LoadMappings();
        }

        /// <summary>
        /// Build Dictionaries Here.
        /// </summary>
        private void LoadMappings()
        {
            string codePattern = @"(?i)c\d{4}";

            foreach(string filePath in _config.MappingFiles)
            {
                if (File.Exists(filePath))
                {
                    // If file exists, use streamreader to load mappings into dictionary
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] entry = line.Split('|');

                            // Check that this line follows the pattern of: C1234,C5678|Label
                            if(entry.Length == 2)
                            {
                                // If there are multiple C-codes, split them apart 
                                if (entry[0].Contains(","))
                                {
                                    string[] parts = entry[0].Split(',');

                                    // For each individual code associated with a label (when multiple), add separately to the dictionary with the label
                                    // BIG ASSUMPTION: A C-code will only ever exist in one Label definition.  (e.g. C1234,C6789|Label)
                                    foreach (string part in parts)
                                    {
                                        if (Regex.IsMatch(part, codePattern))
                                        {
                                            if (!_mappingDict.ContainsKey(part.ToLower()))
                                                _mappingDict.Add(part.ToLower(), entry[1]);
                                        }
                                        else
                                        {
                                            if (!_mappingDict.ContainsKey(part))
                                            _mappingDict.Add(part, entry[1]);
                                        }
                                    }
                                }
                                // If there is only one code associated with the label, add to dictionary
                                else
                                {
                                    if (Regex.IsMatch(entry[0], codePattern))
                                    {
                                        if (!_mappingDict.ContainsKey(entry[0].ToLower()))
                                            _mappingDict.Add(entry[0].ToLower(), entry[1]);
                                    }
                                    else
                                    {
                                        if (!_mappingDict.ContainsKey(entry[0]))
                                            _mappingDict.Add(entry[0], entry[1]);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    
                }
            }
        }

        #region ITerminologyLookupService Members

        /// <summary>
        /// Gets the label mapping for the given C-code(s).
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A string with the code(s) associated label.</returns>
        public string GetTitleCase(string value)
        {
            string rtn = "";

            // Break up value on comma, as we are looking for the first match we find in the dictionary
            string[] lookup = value.Split(',');

            foreach (string key in lookup)
            {
                if (_mappingDict.ContainsKey(key))
                {
                    rtn = _mappingDict[key];
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets lower case name.
        /// <remarks>This does not lower case, but returns TitleCase</remarks>
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A string with the code(s) associated label.</returns>
        public string Get(string value)
        {
            return GetTitleCase(value);            
        }

        /// <summary>
        /// Checks the mapping for the key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A boolean.</returns>
        public bool MappingContainsKey(string key)
        {
            // Break up value on comma, as we are looking for the first match we find in the dictionary
            string[] lookup = key.Split(',');

            foreach (string val in lookup)
            {
                if (!_mappingDict.ContainsKey(val))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
