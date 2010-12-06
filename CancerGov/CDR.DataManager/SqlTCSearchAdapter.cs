using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using CancerGov.Text;

namespace CancerGov.CDR.DataManager
{
	/// <summary>
	/// This retrieves protocols for a TrialCheck Search
	/// </summary>
	public class SqlTCSearchAdapter : SqlProtocolAdapter
	{

		private bool bIsDisposed = false;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="listOfProtocolIDs">Comma separated list of Protocol IDs</param>
		/// <param name="sectionList">A comma separated list of section ids</param>
		/// <param name="version">An enumeration of patient or health professional</param>
		public SqlTCSearchAdapter(string listOfProtocolIDs, string sectionList, ProtocolVersions version)
		{
			this.iProtocolSearchID = -1;
			this.pvVersion = version;
			this.strSectionList = sectionList;

			scProtocols.CommandType = CommandType.StoredProcedure;
            scProtocols.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
			scProtocols.CommandTimeout = Convert.ToInt32(ConfigurationSettings.AppSettings["CTSearchTimeout"]);

			scProtocols.CommandText = "usp_GetProtocolsByTCResultsList";

			scProtocols.Parameters.Add(new SqlParameter("@ProtocolIdList", listOfProtocolIDs));
			scProtocols.Parameters.Add(new SqlParameter("@Version", (int)version));

		}

		/// <summary>
		/// Fills a ProtocolCollections with Protocols
		/// </summary>
		/// <param name="protocolCollection">The name of the ProtocolCollection to be filled.</param>
		public void Fill(ProtocolCollection protocolCollection) {
			
			//Populate the base dataset from our select command
			
			this.LoadDataSet();

			//Set Total Results
			//protocolCollection.TotalResults = Strings.ToInt(scProtocols.Parameters["@TotalResults"].Value.ToString());

			foreach (DataRow drProtocol in dsProtocols.Tables[0].Rows) {

				Protocol pProtocol = this.GetProtocol(drProtocol);

				if (pProtocol != null) {
					protocolCollection.Add(pProtocol);
				}
			}
		}

		/// <summary>
		/// Creates a protocol from a datarow representation of a protocol
		/// </summary>
		/// <param name="drProtocol">A Datarow from the protocol table</param>
		/// <returns>Protocol</returns>
		protected override Protocol GetProtocol(DataRow drProtocol) {
			
			return new Protocol(drProtocol,this.pvVersion, this.strSectionList); // trialcheck
			
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
