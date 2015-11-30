using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCI.Search.BestBets;
using NCI.Web.CDE.Configuration;

namespace CancerGov.Search.BestBets
{
    public class CancerGovBestBetsPathConfiguration : IBestBetPathConfiguration
    {
        #region IBestBetPathConfiguration Members

        public string BestBetsFilePath
        {
            get {
                //TODO: Put some checks here.
                return ContentDeliveryEngineConfig.PathInformation.BestBetsResultPath.Path;
            }
        }

        public string LuceneIndexPath
        {
            get {
                return System.IO.Path.Combine(System.IO.Path.GetTempPath(), "BestBetsIndex");
            }
        }

        #endregion
    }
}
