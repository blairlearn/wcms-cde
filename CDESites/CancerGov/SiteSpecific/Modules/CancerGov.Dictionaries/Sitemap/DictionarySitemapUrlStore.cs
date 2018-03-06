using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using NCI.Web;
using NCI.Web.Sitemap;
using NCI.Web.Dictionary;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.Configuration;
using NCI.Services.Dictionary;
using CancerGov.Dictionaries.Configuration;
using Common.Logging;

namespace CancerGov.Dictionaries.Sitemap
{
    public class DictionarySitemapUrlStore : SitemapUrlStoreBase
    {
        static ILog log = LogManager.GetLogger(typeof(FileSitemapUrlStore));

        private String _rootUrl = ConfigurationManager.AppSettings["RootUrl"];

        private DictionariesInfo _info = null;

        /// <summary>
        /// Create a collection of URL elements from a CSV file of dictionary entries
        /// </summary>
        /// <returns>SitemapUrlSet</returns>
        public override SitemapUrlSet GetSitemapUrls(string sitemapName)
        {
            List<SitemapUrl> sitemapUrls = new List<SitemapUrl>();

            try 
            {
                 _info = GetDictionariesInfo();
            }
            catch(Exception ex)
            {
                log.Error("Error deserializing DictionariesConfig.xml file.");
                throw ex;
            }

            string path = _info.SitemapStore;

            if(path != null)
            {
                String file = HttpContext.Current.Server.MapPath(path);

                List<DictionaryEntryMetadata> entries = new List<DictionaryEntryMetadata>();

                try
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        string currentLine;
                        // currentLine will be null when the StreamReader reaches the end of file
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            string[] values = currentLine.Split(',');

                            if (values.Length == 3)
                            {
                                try
                                {
                                    DictionaryEntryMetadata entry = new DictionaryEntryMetadata();

                                    int cdrid;
                                    bool validCDRID = int.TryParse(values[0], out cdrid);
                                    if (validCDRID)
                                    {
                                        entry.CDRID = cdrid;
                                        entry.Dictionary = (NCI.Services.Dictionary.DictionaryType)System.Enum.Parse(typeof(NCI.Services.Dictionary.DictionaryType), values[1]);
                                        entry.Language = (Language)System.Enum.Parse(typeof(Language), values[2]);
                                        entries.Add(entry);
                                    }
                                    else
                                    {
                                        log.ErrorFormat("Error in dictionary sitemap file for line {0} : invalid CDRID.", currentLine);
                                        continue;
                                    }

                                }
                                catch
                                {
                                    log.ErrorFormat("Error in dictionary sitemap file {0} for line {1} : could not create dictionary entry metadata.", file, currentLine);
                                    continue;
                                }
                            }
                            else
                            {
                                log.ErrorFormat("Error in dictionary sitemap file {0} for line {1} : invalid syntax.", file, currentLine);
                                continue;
                            }
                        }

                        // Check if these entries are valid in the DB. Returns a list of only the valid entries to be included in the sitemap.
                        DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();
                        entries = _dictionaryAppManager.DoDictionaryEntriesExist(entries);
                    }
                }
                catch (Exception e)
                {
                    log.ErrorFormat("Error in DictionarySitemapUrlStore: unable to read dictionary sitemap file located at {0}.", file);
                }

                foreach (DictionaryEntryMetadata entry in entries)
                {
                    string dic = entry.Dictionary.ToString();
                    string lang = entry.Language.ToString();

                    DictionaryInfo entryInfo = _info.DictionaryInfos.First(i => i.Name == entry.Dictionary.ToString() && i.Language == entry.Language.ToString());
                    string entryUrl = GetSitemapUrl(entryInfo, entry.CDRID.ToString());
                    double priority = 0.5;

                    sitemapUrls.Add(new SitemapUrl(entryUrl, sitemapChangeFreq.weekly, priority));
                }

                return new SitemapUrlSet(sitemapUrls);
            }
            else
            {
                log.ErrorFormat("Could not load dictionary provider file located at {0}.", path);
                return new SitemapUrlSet();
            }
        }

        /// <summary>
        /// Helper method to load configuration info based on DictionariesInfo appsetting path.
        /// </summary>
        /// <returns></returns>
        public static DictionariesInfo GetDictionariesInfo()
        {
            string configPath = ConfigurationManager.AppSettings["DictionariesConfigFilePath"];
            return (DictionariesInfo)ModuleObjectFactory<DictionariesInfo>.GetObjectFromFile(configPath);

        }

        /// <summary>
        /// Gets the friendly name of the given CDRID, if it exists.
        /// Otherwise, returns the CDRID.
        /// </summary>
        public string GetFriendlyName(DictionaryInfo info, string cdrId)
        {
            // Get CDRID to friendly name mappings
            string dictionaryMappingFilepath = null;

            dictionaryMappingFilepath = info.FriendlyNameMapping;

            if (!string.IsNullOrEmpty(dictionaryMappingFilepath))
            {
                TerminologyMapping map = TerminologyMapping.GetMappingForFile(dictionaryMappingFilepath);

                // If pretty name is in label mappings, set CDRID
                if (map.MappingContainsCDRID(cdrId))
                {
                    return map.GetFriendlyNameFromCDRID(cdrId);
                }
            }

            return cdrId;
        }

        /// <summary>
        /// Gets the URL for the given cdrID.
        /// Returns the URL with the friendly-name if found, otherwise with the CDRID.
        /// </summary>
        public string GetSitemapUrl(DictionaryInfo info, string cdrId)
        {
            NciUrl url = new NciUrl();
            url.SetUrl(_rootUrl);
            url.AppendPathSegment(info.DefinitionUrl);
            url.AppendPathSegment(GetFriendlyName(info, cdrId));

            return _rootUrl + url.ToString();
        }
    }
}
