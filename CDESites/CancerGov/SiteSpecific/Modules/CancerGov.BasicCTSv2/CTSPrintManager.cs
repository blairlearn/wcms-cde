using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NCI.Web.CDE.Application;
using CancerGov.ClinicalTrials.Basic.v2.DataManagers;
using NCI.Text;
using NCI.Web.CDE.UI;
using NCI.Web.CDE;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class CTSPrintManager
    {
        public string GetPrintContent(Guid printID)
        {
            string printContent = CTSPrintResultsDataManager.RetrieveResult(printID, NCI.Web.CDE.UI.Configuration.Settings.IsLive);

            return printContent;
        }

    }
}
