using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CancerGov.Text;

namespace CancerGov.CDR.DataManager
{
	/// <summary>
	/// This is used to interface with GetProtocolsBySearchID
	/// </summary>
	public class SqlSearchResultsAdapter : SqlProtocolAdapter, IDisposable
	{

		private bool bIsDisposed = false;

		/// <summary>
		/// Creates a new SqlSearchResultsAdapter
		/// </summary>
		/// <param name="protocolSearchID">A protocol search id</param>
		/// <param name="sectionList">A comma separated list of section ids</param>
		/// <param name="drawStudySites">True if you want study sites displayed</param>
		/// <param name="sortFilter">An integer representation of the sort parameter</param>
		/// <param name="page">The current page number</param>
		/// <param name="resultsPerPage">The number of records to retrieve</param>
		/// <param name="version">An enumeration of patient or health professional</param>
		public SqlSearchResultsAdapter(int protocolSearchID, string sectionList, bool drawStudySites, int sortFilter, int page, int resultsPerPage,	ProtocolVersions version)
		{
			
			this.iProtocolSearchID = protocolSearchID;
			this.pvVersion = version;
			this.strSectionList = sectionList;

			scProtocols.CommandType = CommandType.StoredProcedure;
            scProtocols.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
			scProtocols.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CTSearchTimeout"]);

			scProtocols.CommandText = "usp_GetProtocolsBySearchID";

			scProtocols.Parameters.Add(new SqlParameter("@SectionList", sectionList));
			scProtocols.Parameters.Add(new SqlParameter("@ProtocolSearchID", protocolSearchID));
			scProtocols.Parameters.Add(new SqlParameter("@DrawStudySites", Convert.ToInt16(drawStudySites)));
			scProtocols.Parameters.Add(new SqlParameter("@Version", (int)version));
			scProtocols.Parameters.Add(new SqlParameter("@SortFilter", sortFilter));
			scProtocols.Parameters.Add(new SqlParameter("@Page", page));
			scProtocols.Parameters.Add(new SqlParameter("@ResultsPerPage", resultsPerPage));

			SqlParameter sqlParam = new SqlParameter("@TotalResults", SqlDbType.Int);
			sqlParam.Direction = ParameterDirection.Output;
			scProtocols.Parameters.Add(sqlParam);
	
		}

		/// <summary>
		/// Fills a ProtocolCollections with Protocols
		/// </summary>
		/// <param name="protocolCollection">The name of the ProtocolCollection to be filled.</param>
		public void Fill(ProtocolCollection protocolCollection) {
			
			//Populate the base dataset from our select command
			
			this.LoadDataSet();

			//Set Total Results
			protocolCollection.TotalResults = Strings.ToInt(scProtocols.Parameters["@TotalResults"].Value.ToString());

			foreach (DataRow drProtocol in dsProtocols.Tables[0].Rows) {

				Protocol pProtocol = this.GetProtocol(drProtocol);

				if (pProtocol != null) {
					protocolCollection.Add(pProtocol);
				}
			}
		}

		
		/// <summary>
		/// Internal Dispose method
		/// </summary>
		/// <param name="disposing">Dispose all managed and unmanaged resources</param>
		protected override void Dispose(bool disposing) {
			
			// Check to see if Dispose has already been called.
			if(!this.bIsDisposed) {
			
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if(disposing) {
					base.Dispose(disposing);
				}
             
				// Call the appropriate methods to clean up 
				// unmanaged resources here.
				// If disposing is false, 
				// only the following code is executed.

			}
			bIsDisposed = true;         
		}

	}
}
