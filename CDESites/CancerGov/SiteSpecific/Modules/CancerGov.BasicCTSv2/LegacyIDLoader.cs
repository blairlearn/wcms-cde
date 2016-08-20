using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using NCI.Logging;
using Newtonsoft.Json;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Class to load and do heavy lifting on loading lookup data.
    /// </summary>
    class LegacyIDLoader
    {
        /// <summary>
        /// Retrieves pipe 
        /// </summary>
        /// <returns>ZipCodeDictionary zipCodes</returns>
        public static LegacyCancerTypeDictionary LoadCancerTypeDictionary()
        {

            try
            {
                string[] mappingList = LoadMappingFile();
                
                // If no list is found, use an empty array to prevent an error.
                // Any  error loading the data file should be handled by LoadMappingFile().
                if(mappingList == null)
                    mappingList = new string[0];

                LegacyCancerTypeDictionary map = new LegacyCancerTypeDictionary();
                Array.ForEach(mappingList, (string entry) =>
                {
                    entry.Trim();

                        string[] parts = entry.Split('|');
                        string key, value;

                        if (parts.Length == 0)
                        {
                            return; // Blank line, go to next entry
                        }

                        // Check for insufficient pieces.
                        if (parts.Length == 1)
                        {
                            Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()",
                                String.Format("Partial entry '{0}'", entry), NCIErrorLevel.Error);
                            return; // Bad entry, go to next entry.
                        }

                        // Non-fatal, check for extra entries.
                        if (parts.Length > 2)
                        {
                            // Log, but keep going.
                            Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()",
                                String.Format("Entry has more than one mapping '{0}'", entry), NCIErrorLevel.Error);
                        }

                        // We know we have a key and value.
                        key = parts[0].Trim();
                        value = parts[1].Trim();

                        // Check for blanks.   
                        if(String.IsNullOrWhiteSpace(key) || String.IsNullOrWhiteSpace(value))
                        {
                            Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()",
                                String.Format("Blank key or value '{0}'", entry), NCIErrorLevel.Error);
                            return; // Bad entry, go to the next one.
                        }

                        // Check for duplicates
                        if (map.ContainsKey(key))
                        {
                            Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()",
                            String.Format("duplicate key '{0}'", entry), NCIErrorLevel.Error);
                            return; // Continue with next array entry.
                        }

                        // OK, we have something worth adding.
                        map.Add(key, value);
                });

                return map;
            }
            catch (Exception ex)
            {
                Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()", "Error loading cancer ID mapping file.", NCIErrorLevel.Error, ex);
                return null;
            }
        }

        private static string[] LoadMappingFile()
        {
            string cancerTypeFilePath = String.Empty;

            try
            {
                cancerTypeFilePath = ConfigurationManager.AppSettings["CancerIDMapping"].ToString();
                if (String.IsNullOrWhiteSpace(cancerTypeFilePath))
                {
                    Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()", "CancerIDMapping not set.", NCIErrorLevel.Error);
                    return null;
                }

                cancerTypeFilePath = HttpContext.Current.Server.MapPath(cancerTypeFilePath);
                string[] mappingList = File.ReadAllLines(cancerTypeFilePath);

                return mappingList;
            }
            catch (FileNotFoundException ex)
            {
                Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()", "Path " + cancerTypeFilePath + " not found.", NCIErrorLevel.Error, ex);
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError("CancerGov.ClinicalTrials.Basic.v2:LegacyIDLoader:LoadCancerTypeDictionary()", "Failed to read dictionary file on path " + cancerTypeFilePath, NCIErrorLevel.Error, ex);
                return null;
            }
        }
    }
}
