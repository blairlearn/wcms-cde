using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using CancerGov.Common.ErrorHandling;
using NCI.Data;

namespace CDR.DrugInformationSummary
{
    internal class DrugInformationSummaryQuery
    {

        #region Properties

        private string CdrDbConnectionString { get; set; }

        #endregion

        #region Constructors

        public DrugInformationSummaryQuery()
        {
            CdrDbConnectionString = ConfigurationManager.AppSettings["CDRDbConnectionString"];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a list of new and/or updated DrugInformationSummary documents.
        /// </summary>
        /// <param name="maxReturnCount">Maximum number of items to return</param>
        /// <param name="newOrUpdateFlag">New, Update or Both</param>
        /// <returns>DataTable containing DrugInformationSummary document information.</returns>
        public DataTable LoadNewOrUpdatedDrugInformationSummaries(
            int maxReturnCount,
            string newOrUpdateFlag)
        {
            DataTable results = null;

            SqlParameter[] parms = {
                                       new SqlParameter("@NumItems", SqlDbType.Int){Value = maxReturnCount},
                                       new SqlParameter("@NewOrUpdated", SqlDbType.VarChar){Value = newOrUpdateFlag}
                                   };

            try
            {
                results = SqlHelper.ExecuteDatatable(
                    CdrDbConnectionString,
                    CommandType.StoredProcedure,
                    "usp_RSS_searchDrugInfoSummary",
                    parms);

            }
            catch (Exception ex)
            {
                CancerGovError.LogError("DrugInformationSummaryQuery", 2, ex);
                throw ex;
            }

            return results;
        }

        #endregion

    }
}
