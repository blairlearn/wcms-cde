using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class CTSPrintManager
    {
        public string GetPrintPageHtml(Guid printID)
        {
            // throw exception: no connection string 500
            // 404 if guid doesn't match anything in database
            return "hello";
        }

    }
}
