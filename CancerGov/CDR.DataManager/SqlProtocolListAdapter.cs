using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CancerGov.CDR.DataManager
{
	/// <summary>
	/// This class interfaces with GetProtocolsByProtocolID
	/// </summary>
	public class SqlProtocolListAdapter : SqlProtocolAdapter, IDisposable
	{

		private string strProtocolIDList = "";
		private bool bIsDisposed = false;

		/// <summary>
		/// Constructor for SqlProtocolListAdapter
		/// </summary>
		/// <param name="protocolSearchID">A protocol search id</param>
		/// <param name="protocolIDList">A comma separated list of protocol ids</param>
		/// <param name="sectionList">A comma separated list of section ids</param>
		/// <param name="drawStudySites">True if you want study sites displayed</param>
		/// <param name="version">An enumeration of patient or health professional</param>
		public SqlProtocolListAdapter(int protocolSearchID, string protocolIDList, string sectionList, bool drawStudySites, ProtocolVersions version)
		{

			this.iProtocolSearchID = protocolSearchID;
			this.pvVersion = version;
			this.strSectionList = sectionList;
			this.strProtocolIDList = protocolIDList;

			scProtocols.CommandType = CommandType.StoredProcedure;
			scProtocols.Connection = new SqlConnection(ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
			scProtocols.CommandTimeout = Convert.ToInt32(ConfigurationSettings.AppSettings["CTSearchTimeout"]);

			scProtocols.Parameters.Add(new SqlParameter("@SectionList", sectionList));
			scProtocols.Parameters.Add(new SqlParameter("@ProtocolSearchID", protocolSearchID));
			scProtocols.Parameters.Add(new SqlParameter("@DrawStudySites", Convert.ToInt16(drawStudySites)));
			scProtocols.Parameters.Add(new SqlParameter("@Version", (int)version));

			scProtocols.CommandText = "usp_GetProtocolsByListOfProtocolIDs";
			scProtocols.Parameters.Add(new SqlParameter("@ProtocolList", protocolIDList));

		}

		/// <summary>
		/// Fills a ProtocolCollections with Protocols
		/// </summary>
		/// <param name="protocolCollection">The name of the ProtocolCollection to be filled.</param>
		public void Fill(ProtocolCollection protocolCollection) {

			//Populate the base dataset from our select command
			this.LoadDataSet();

			foreach (string strProtID in strProtocolIDList.Split(',')) {
				
				DataRow[] drProtocols = dsProtocols.Tables[0].Select("protocolid=" + strProtID);
				
				if ((drProtocols != null) && (drProtocols.Length > 0)) {
					Protocol pProtocol = this.GetProtocol(drProtocols[0]);

					if (pProtocol != null) {
						protocolCollection.Add(pProtocol);
					}	
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
