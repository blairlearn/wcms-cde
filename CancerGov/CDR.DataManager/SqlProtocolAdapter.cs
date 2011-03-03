using System;
using System.Data;
using System.Data.SqlClient;

using CancerGov.Exceptions;


namespace CancerGov.CDR.DataManager
{
	/// <summary>
	/// This is a class that combines common elements for adapters that retrieve protocols
	/// </summary>
	public class SqlProtocolAdapter : IDisposable
	{

		/// <summary>
		/// The SqlCommand used to fill dsProtocols.
		/// </summary>
		protected SqlCommand scProtocols;

		/// <summary>
		/// The Dataset that is used by inheriting classes
		/// </summary>
		protected DataSet dsProtocols;

		//These are used by the inheriting classes
		protected bool bStudySitesExist = false;
		protected bool bSectionsExist = false;
		protected bool bProtocolsExist = false;

		protected int iProtocolSearchID = -1;
		protected ProtocolVersions pvVersion = ProtocolVersions.HealthProfessional;
		protected string strSectionList = "";

		private bool bIsDisposed = false;

		/// <summary>
		/// Create a new SqlProtocolAdapter
		/// </summary>
		public SqlProtocolAdapter() {
			scProtocols = new SqlCommand();
			dsProtocols = new DataSet();
		}

		/// <summary>
		/// Fills a protected dataset and checks to make sure it contains the right number of items.
		/// </summary>
		protected void LoadDataSet() {

			SqlDataAdapter daProtocols = new SqlDataAdapter(scProtocols);

			try {
				daProtocols.Fill(dsProtocols);
			} catch (SqlException seSQL) {
				throw new ProtocolFetchFailureException(seSQL.Message.ToString());
			}

			//DsProtocol has tables.  In reality this will never be null unless the sql above catches the error
			if (dsProtocols.Tables == null) {
				throw new ProtocolTableEmptyException("No Tables Returned");
			}

			//Check to see what tables exist so we do not get nulls
			//while we are checking on tables, lets setup some relationships...
			switch (dsProtocols.Tables.Count) {
				case 3: {
					bStudySitesExist = true;
					bSectionsExist = true;
					bProtocolsExist = true;
					
					DataRelation drProtocolToSections = new DataRelation("ProtocolToSections",dsProtocols.Tables[0].Columns["ProtocolID"],dsProtocols.Tables[1].Columns["ProtocolID"]);
					dsProtocols.Relations.Add(drProtocolToSections);
					
					//DataRelation drProtocolToStudySites = new DataRelation("ProtocolToStudySites",dsProtocols.Tables[0].Columns["ProtocolID"],dsProtocols.Tables[2].Columns["ProtocolID"]);
					//dsProtocols.Relations.Add(drProtocolToStudySites);

					break;
				}
				case 2 : {
					bSectionsExist = true;
					bProtocolsExist = true;

					DataRelation drProtocolToSections = new DataRelation("ProtocolToSections",dsProtocols.Tables[0].Columns["ProtocolID"],dsProtocols.Tables[1].Columns["ProtocolID"]);
					dsProtocols.Relations.Add(drProtocolToSections);

					break;
				}
				case 1: {
					bProtocolsExist = true;
					break;
				}
				default : {
					throw new ProtocolTableMiscountException(dsProtocols.Tables.Count.ToString());
				}
			}

			if (daProtocols != null) {
				daProtocols.Dispose(); 
			}
		}

		/// <summary>
		/// Creates a protocol from a datarow representation of a protocol
		/// </summary>
		/// <param name="drProtocol">A Datarow from the protocol table</param>
		/// <returns>Protocol</returns>
		protected virtual Protocol GetProtocol(DataRow drProtocol) {

			DataRow[] drarrSections = null;
			DataView dvStudySites = null;

			//DataRow[] drarrStudySites;

			if (bSectionsExist) {
				//dvSections = new DataView(dsProtocol.Tables[1]);
				//dvSections.RowFilter = "ProtocolID = " + drProtocol["ProtocolID"].ToString();
					
				drarrSections = drProtocol.GetChildRows("ProtocolToSections");
			}

			if (bStudySitesExist) {
				//Setup StudySites dataview
				dvStudySites = new DataView(dsProtocols.Tables[2]);
				dvStudySites.RowFilter = "ProtocolID = " + drProtocol["ProtocolID"].ToString();
				dvStudySites.Sort="Country, State, City, OrganizationName asc";

				//I would like this, but not until we get a StudySitesList object that does not just hold a dataview
				//Maybe I will fix that however if there is time...
				//drarrStudySites = drProtocol.GetChildRows("ProtocolToStudySites");
					
			}

			Protocol pProtocol = null;

			//Fix this
			if (iProtocolSearchID > 0) {
				if (bSectionsExist && bStudySitesExist) {
					pProtocol = new Protocol(iProtocolSearchID,drProtocol,drarrSections,dvStudySites,pvVersion,strSectionList);
				} else if (bSectionsExist) {
					pProtocol = new Protocol(iProtocolSearchID,drProtocol,drarrSections,pvVersion, strSectionList);
				} else {
					pProtocol = new Protocol(iProtocolSearchID,drProtocol,pvVersion, strSectionList);
				}
			}

			return pProtocol;

		}


		/// <summary>
		/// Disposes this object
		/// </summary>
		public void Dispose() {
			Dispose(true);

			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue 
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Internal Dispose method
		/// </summary>
		/// <param name="disposing">Dispose all managed and unmanaged resources</param>
		protected virtual void Dispose(bool disposing) {
			
			// Check to see if Dispose has already been called.
			if(!this.bIsDisposed) {
				
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if(disposing) {
					// Dispose managed resources.
					if (dsProtocols != null) {
						dsProtocols.Dispose();
					}

					if (scProtocols != null) {
						scProtocols.Dispose();
					}

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
