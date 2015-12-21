using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.BestBets
{
    /// <summary>
    /// Interface for configuring Best Bets paths
    /// <remarks>
    /// Basically, BestBets should not know about the CDE, BUT the CDE is where the path information
    /// is already configured.  So instead of duplicating paths to great peril, let's make an interface
    /// where we can tell the BestBetsIndex where to find the files.  We can then have a configuration
    /// item (BestBetsSection) point to the concrete class that implements this interface.
    /// 
    /// //TODO: Replace this with a best bets provider/kind of like sitemap
    /// it would have a method such as:
    /// public IBestBetIndexingItem GetBestBetsForIndexing()
    /// 
    /// An IBestBetIndexingItem would have
    /// CategoryID
    /// CatName
    /// List&lt;IBestBetSynonym&gt; includeSynonyms
    /// /// List&lt;IBestBetSynonym&gt; excludeSynonyms
    /// </remarks>
    /// </summary>
    public interface IBestBetPathConfiguration
    {
        /// <summary>
        /// Gets the path to the folder that contains the Best Bets XML.
        /// </summary>
        /// <value>
        /// The best bets file path.
        /// </value>
        string BestBetsFilePath { get; }

        /// <summary>
        /// Gets the folder path where the lucene index should be placed.
        /// </summary>
        /// <value>
        /// The lucene index path.
        /// </value>
        string LuceneIndexPath { get; }
    }
}
