using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
                string path = ContentDeliveryEngineConfig.PathInformation.BestBetsResultPath.Path;

                path = path.Replace("{0}.xml", ""); // This replaces the file name formatter portion of the path

                path = HttpContext.Current.Server.MapPath(path); //Map this to a real file path

                return path;
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
