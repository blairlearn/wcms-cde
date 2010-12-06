using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NCI.Data;
using CancerGov.Common.ErrorHandling;

namespace CDR.Summary
{
    internal class SummaryQuery
    {
        #region Properties

        private string CdrDbConnectionString { get; set; }

        #endregion

        #region Constructors

        public SummaryQuery()
        {
            CdrDbConnectionString = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a list of new and/or updated Summary documents.
        /// </summary>
        /// <param name="maxReturnCount">Maximum number of items to return</param>
        /// <param name="audience">Patient or HealthProfessional</param>
        /// <param name="type">All, Treatment, SupportiveCare, CAM, Genetics,
        /// Prevention or Screening</param>
        /// <param name="newOrUpdateFlag">New, Update or Both</param>
        /// <param name="language">English or Spanish</param>
        /// <returns>DataTable containing summary document information.</returns>
        public DataTable LoadNewOrUpdatedSummaries(
            int maxReturnCount,
            string audience,
            string type,
            string newOrUpdateFlag,
            string language)
        {
            DataTable results = null;

            SqlParameter[] parms = {
                                       new SqlParameter("@Audience", SqlDbType.VarChar){ Value = audience },
                                       new SqlParameter("@NumItems", SqlDbType.Int){ Value = maxReturnCount },
                                       new SqlParameter("@SummaryType", SqlDbType.VarChar){ Value = type },
                                       new SqlParameter("@NewOrUpdated", SqlDbType.VarChar){ Value = newOrUpdateFlag },
                                       new SqlParameter("@Language", SqlDbType.VarChar){ Value = language }
                                   };

            try
            {
                results = SqlHelper.ExecuteDatatable(
                    CdrDbConnectionString,
                    CommandType.StoredProcedure,
                    "usp_RSS_searchSummary",
                    parms);

            }
            catch (Exception ex)
            {
                CancerGovError.LogError("SummaryQuery", 2, ex);
                throw ex;
            }

            return results;
        }

        #endregion

    }
}
