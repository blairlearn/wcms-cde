using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NCI.Web.CDE.Application;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class CTSPrintManager
    {
        public string GetPrintPageHtml(Guid printID)
        {
            // throw exception: no connection string 500
            // 404 if guid doesn't match anything in database
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            }
            catch (DbConnectionException ex)
            {
                ErrorPageDisplayer.RaisePageByCode(this.GetType().ToString(), 500);
            }

            return null;
        }

    }
}
