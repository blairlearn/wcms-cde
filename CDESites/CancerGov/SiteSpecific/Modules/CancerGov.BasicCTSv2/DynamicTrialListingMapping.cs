using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class DynamicTrialListingMapping
    {
        // .NET guarantees thread safety for static initialization
        private static DynamicTrialListingMapping instance;
        
        // Lock synchronization object
        private static object syncLock = new Object();

        private Dictionary<string, string> Mappings = new Dictionary<string, string>();
        private static readonly string OverrideMappingFile = "~/PublishedContent/Files/about-cancer/treatment/clinical-trials/LabelMapping.txt";
        private static readonly string EVSMappingFile = "~/PublishedContent/Files/about-cancer/treatment/clinical-trials/EVSMapping.txt";

        private DynamicTrialListingMapping() { }

        public static DynamicTrialListingMapping Instance
        {
            get
            {
                Initialize();
                return instance;
            }
        }

        private static void Initialize()
        {
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new DynamicTrialListingMapping();
                        Dictionary<string, string> dictEVS = GetDictionary(EVSMappingFile);
                        Dictionary<string, string> dictOverrides = GetDictionary(OverrideMappingFile);
                        

                        //TODO: Merge 
                        instance.Mappings = dictEVS;
                    }
                }
            }
        }

        private static Dictionary<string, string> GetDictionary(string filePath)
        {
            Dictionary<string, string> dict = new Dictionary<string,string>();
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(filePath)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    parts[0].ToLower();

                    if(parts[0].Contains(","))
                    {
                        string[] split = parts[0].Split(',');
                        Array.Sort(split);
                        string newKey = string.Join(",", split);
                        parts[0] = newKey;
                    }

                    if(!dict.ContainsKey(parts[0]))
                    {
                        dict.Add(parts[0], parts[1]);
                    }
                }
            }
            return dict;
        }

        public string this[string code]
        {
            get{
                return Mappings[code];
            }
        }

        public bool MappingContainsKey(string key)
        {
            return Mappings.ContainsKey(key);
        }
    }
}
