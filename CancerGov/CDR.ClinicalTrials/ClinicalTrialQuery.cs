using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using CancerGov.Common.ErrorHandling;
using NCI.Data;

namespace CancerGov.CDR.ClinicalTrials
{
    internal class ClinicalTrialQuery
    {
        #region Properties

        private string CdrDbConnectionString { get; set; }

        #endregion

        #region Constructors

        public ClinicalTrialQuery()
        {
            CdrDbConnectionString = ConfigurationManager.AppSettings["CDRDbConnectionString"];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads a list consisting of only new and active clinical trials.
        /// </summary>
        /// <param name="maxReturnCount">Controls the maximum number of results returned.</param>
        /// <returns>A DataTable containing the clinical trials, sorted with the newest first.</returns>
        public DataTable LoadNewAndActiveProtocols(int maxReturnCount)
        {
            DataTable results = null;

            SqlParameter[] parms = {
                                       new SqlParameter("@NumItems", SqlDbType.Int) { Value = maxReturnCount }
                                   };

            try
            {
                results = SqlHelper.ExecuteDatatable(
                    CdrDbConnectionString,
                    CommandType.StoredProcedure,
                    "usp_RSS_searchProtocol",
                    parms);

            }
            catch (Exception ex)
            {
                CancerGovError.LogError("ClinicalTrialQuery", 2, ex);
                throw ex;
            }

            return results;
        }

        #endregion

    }
}
