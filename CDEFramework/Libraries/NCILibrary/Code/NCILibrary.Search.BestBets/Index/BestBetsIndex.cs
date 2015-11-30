using System;
using System.Collections.Generic;
using System.Configuration;
using IO = System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;

using NCI.Search.BestBets.Configuration;
using NCI.Search.BestBets.Lucene;


namespace NCI.Search.BestBets.Index
{
    /// <summary>
    /// This class is a singleton representing the Best Bets Index.
    /// </summary>
    public sealed class BestBetsIndex
    {

        #region Singleton things

        /// <summary>
        /// An object for locking for creation/reindexing
        /// </summary>
        private static readonly Object _indexLock = new object();

        /// <summary>
        /// The actual BestBetsIndex
        /// </summary>
        private static volatile BestBetsIndex _instance;

        /// <summary>
        /// Exception for initializing the index.
        /// </summary>
        private static Exception _indexingException = null;

        /// <summary>
        /// Gets the instance.
        /// <remarks>This will not log</remarks>
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static BestBetsIndex Instance
        {
            get
            {
                //So that we do not keep trying to index something that,
                //cannot be indexed, let's store the exception and just
                //throw it instead of trying to keep initializing...                
                //TODO: Maybe put a time limit so that we can try again after a period of time.
                if (_indexingException != null)
                {
                    throw _indexingException;
                }

                if (_instance == null)
                {
                    lock (_indexLock)
                    {
                        //If the lock was released, but the previous thread was not able to
                        //initialize, then we need to throw the exception they got.
                        if (_indexingException != null)
                        {
                            throw _indexingException;
                        }

                        if (_instance == null)
                        {
                            try
                            {
                                _instance = new BestBetsIndex();
                                _indexingException = null;
                            }
                            catch (Exception ex)
                            {                                
                                // We don't want to keep trying and failing and breaking and 
                                //stuff.
                                _indexingException = ex;

                                throw _indexingException;
                            }
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="BestBetsIndex"/> class from being created.
        /// </summary>
        private BestBetsIndex()
        {
            Initialize();
        }

        #endregion


        #region Instance Variables

        /// <summary>
        /// This is the instance's index.
        /// </summary>
        private Directory _luceneIndex = null;

        /// <summary>
        /// The folder path that will contain the lucene Index
        /// </summary>
        private string _luceneIndexPath = null;

        /// <summary>
        /// The path to the best bets files
        /// </summary>
        private string _bestBetsPath = null;
        
        #endregion

        /// <summary>
        /// Gets a searcher for handling search results
        /// </summary>
        /// <returns></returns>
        public Searcher GetSearcher()
        {
            return new IndexSearcher(_luceneIndex);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            //Get the configuration section
            BestBetsSection config = (BestBetsSection)ConfigurationManager.GetSection("nci/search/bestbets");

            if (config == null)            
                throw new ConfigurationErrorsException("BestBetsSection (nci/search/bestbets) is missing from configuration");            

            //Load the path config
            if (String.IsNullOrWhiteSpace(config.PathConfigurationClass))
                throw new ConfigurationErrorsException("pathConfigurationClass is required for BestBetsSection (nci/search/bestbets)");

            //We should be inside a block of code that has a lock, so no need to lock class.
            IBestBetPathConfiguration pathConfig = LoadPathConfigurationClass(config.PathConfigurationClass);            

            //Get configuration information
            _luceneIndexPath = pathConfig.LuceneIndexPath;

            _bestBetsPath = pathConfig.BestBetsFilePath;

            if (!IO.Directory.Exists(_luceneIndexPath))
                IO.Directory.CreateDirectory(_luceneIndexPath);

            //We are using an MMapDirectory as the default option (FSSimpleDirectory) does not handle
            //multi-threading well.
            _luceneIndex = MMapDirectory.Open(_luceneIndexPath);

            //Now let's actually index the content.
            BuildIndex();
        }

        /// <summary>
        /// Loads the path configuration class.
        /// </summary>
        /// <param name="classInfo">The class information.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">BestBetPathConfiguration must implement type  IBestBetPathConfiguration</exception>
        private IBestBetPathConfiguration LoadPathConfigurationClass(string classInfo)
        {
            IBestBetPathConfiguration rtnConfig = null;

            //Will throw exception if class cannot be found or bogus
            Type t = Type.GetType(classInfo, true, true);

            if (!typeof(IBestBetPathConfiguration).IsAssignableFrom(t))
                throw new ArgumentException("BestBetPathConfiguration must implement type " + typeof(IBestBetPathConfiguration).ToString());

            rtnConfig = (IBestBetPathConfiguration)Activator.CreateInstance(t);

            return rtnConfig;
        }

        /// <summary>
        /// Builds the index.
        /// </summary>
        private void BuildIndex()
        {
            //Let's assume that there is a lock in place for handling this.  Any calling program should handle locking/thread safety.

            //Define our custom Analyzer for analyzing text fields for our index.
            Analyzer analyzer = new BestBetsAnalyzer(global::Lucene.Net.Util.Version.LUCENE_30);

            //Create instance of an IndexWriter
            IndexWriter writer = new IndexWriter(_luceneIndex, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);

            foreach (string bbFilePath in System.IO.Directory.EnumerateFiles(_bestBetsPath, "*.xml"))
            {
                BestBetCategory bbcat = ModuleObjectFactory<BestBetCategory>.GetObjectFromFile(bbFilePath);

                //We will basically index each synonym as an idividual record, with the category information
                //"duplicated" on each record.

                PushToIndex(writer, bbcat.CategoryId.ToString(), bbcat.CategoryName, bbcat.CategoryName, false, bbcat.IsExactMatch, "en");

                foreach (BestBetSynonym syn in bbcat.IncludeSynonyms)
                {
                    PushToIndex(writer, bbcat.CategoryId.ToString(), bbcat.CategoryName, syn.Text, false, syn.IsExactMatch, bbcat.Language);
                }

                foreach (BestBetSynonym syn in bbcat.ExcludeSynonyms)
                {
                    PushToIndex(writer, bbcat.CategoryId.ToString(), bbcat.CategoryName, syn.Text, true, syn.IsExactMatch, bbcat.Language);
                }
            }

            //Commit the changes to the index.
            writer.Commit();
        }

        /// <summary>
        /// Pushes a best bet "Synonym" to the index.
        /// </summary>
        /// <param name="writer">An Index Writer</param>
        /// <param name="catID">The Best Bet Category ID</param>
        /// <param name="catName">Name of the Category.</param>
        /// <param name="term">The terms for searching</param>
        /// <param name="isExclude">Does a match on the term exclude this best bet category?</param>
        /// <param name="isExact">Must the term be an exact match?</param>
        /// <param name="twoLetterISOLanguageName">Name of the two letter iso language.</param>
        private static void PushToIndex(IndexWriter writer, string catID, string catName, string term, bool isExclude, bool isExact, string twoLetterISOLanguageName)
        {
            //We need to clean up the term as we used to do.  This is removing special characters.
            string cleanedTerm = BestBetUtils.CleanTerm(term);
            
            Document d = new Document();

            d.Add(new Field("cat_id", catID, Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            d.Add(new Field("cat_name", catName, Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            d.Add(new Field("language", twoLetterISOLanguageName, Field.Store.YES, Field.Index.NOT_ANALYZED));
            d.Add(new Field("is_exclude", isExclude.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            d.Add(new Field("is_exact", isExact.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED_NO_NORMS));
            d.Add(new Field("terms", cleanedTerm, Field.Store.YES, Field.Index.ANALYZED));            
            d.Add(new NumericField("term_count", 4, Field.Store.YES, true).SetIntValue(BestBetUtils.TokenizeStringStandard(cleanedTerm).Count));

            //Push the document into the index.
            writer.AddDocument(d, new BestBetsAnalyzer(global::Lucene.Net.Util.Version.LUCENE_30));
        }

        #region ModuleObjectFactory hack

        /// <summary>
        /// Factory for easily deserializing CDE XML
        /// <remarks>
        /// THIS IS A HACK!  Basically, until we have a BestBetsProvider that can be put in the CDE assemblies,
        /// the best bets indexing will have to know the best bets XML exists.  I don't want to reference the
        /// CDE.UI project since we want this to be able to live on its own at some point.  So I am making
        /// a copy of the factory and putting it in as a private class until this can all get worked out.
        /// </remarks>
        /// </summary>
        /// <typeparam name="ModuleObjectType"></typeparam>
        private class ModuleObjectFactory<ModuleObjectType>
        {
            private static System.Collections.Generic.Dictionary<string, XmlSerializer> serializers = new Dictionary<string, XmlSerializer>();

            public static ModuleObjectType GetModuleObject(string snippetXmlData)
            {
                try
                {
                    using (XmlTextReader reader = new XmlTextReader(snippetXmlData.Trim(), XmlNodeType.Element, null))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ModuleObjectType), "cde");
                        return (ModuleObjectType)serializer.Deserialize(reader);
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            public static ModuleObjectType GetObjectFromFile(string filePath)
            {
                try
                {
                    XmlSerializer serializer = null;
                    string typeName = typeof(ModuleObjectType).ToString().ToLower();

                    if (serializers.ContainsKey(typeName))
                        serializer = serializers[typeName];
                    else
                    {
                        serializer = new XmlSerializer(typeof(ModuleObjectType), "cde");
                        serializers.Add(typeName, serializer);
                    }

                    // Make an absolute path.
                    //filePath = HttpContext.Current.Server.MapPath(filePath);

                    using (IO.FileStream xmlFile = IO.File.Open(filePath, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite | IO.FileShare.Delete))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(xmlFile))
                        {
                            return (ModuleObjectType)serializer.Deserialize(xmlReader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = String.Format("Unable to load object from file \"{0}.\"  The file may not exist or the XML in the file may not be deserializable into a valid object.", filePath);

                    throw ex;
                }
            }
        }

        #endregion

    }
}
