using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace CancerGov.CDR.DataManager
{
	/// <summary>
	/// Summary description for StudySiteList.
	/// </summary>
	// Hmm Tricky Tricky.  we need these things to be drawn in a certain order.
	// while we do not do presentation, we should make it so that we can get the
	// right info out.  We can actually order them later.
	public class StudySiteList
	{
		protected DataView dvSiteList;
		bool bIsDisposed = false;
		
		public DataView SiteTable {
			get {return dvSiteList;}
		}

		public StudySiteList(DataView dvSiteList)
		{

			//I have no time to mess around.  If I figure out a fancier way, well then I will
			this.dvSiteList = dvSiteList;

//			foreach (DataRowView drvSite in dvSiteList) {
//
//				int iPSID = Strings.ToInt(drvSite.Row["ProtocolSectionID"].ToString());
//				int iProtocolID = Strings.ToInt(drvSite.Row["ProtocolID"].ToString());
//				string strCity = Strings.Clean(drvSite.Row["HTML"].ToString());
//				string strState = Strings.Clean(drvSite.Row["HTML"].ToString());
//				string strCountry = Strings.Clean(drvSite.Row["HTML"].ToString());
//				string strOrganizationName = Strings.Clean(drvSite.Row["HTML"].ToString());
//				string strHTML = Strings.Clean(drvSite.Row["HTML"].ToString());
//
//
//				this.Add(new StudySite(drvSite.Row));
//			}
		}

//		public void Dispose() {
//			Dispose(true);
//
//			// This object will be cleaned up by the Dispose method.
//			// Therefore, you should call GC.SupressFinalize to
//			// take this object off the finalization queue 
//			// and prevent finalization code for this object
//			// from executing a second time.
//			GC.SuppressFinalize(this);
//		}
//
//		private void Dispose(bool disposing) {
//			
//			// Check to see if Dispose has already been called.
//			if(!this.bIsDisposed) {
//				
//				// If disposing equals true, dispose all managed 
//				// and unmanaged resources.
//				if(disposing) {
//					// Dispose managed resources.
//
//					if (dvSiteList != null) {
//						dvSiteList.Dispose();
//					}
//				}
//             
//				// Call the appropriate methods to clean up 
//				// unmanaged resources here.
//				// If disposing is false, 
//				// only the following code is executed.
//
//			}
//			bIsDisposed = true;         
//		}

	}



	public class StudySite {

		int iProtocolContactInfoHtmlID;
		int iProtocolID;
		string strCity = "";
		string strState = "";
		string strCountry = "";
		string strOrganizationName = "";
		string strHTML = "";

		public StudySite (DataRow drSite) {
			

		}

	}
}
