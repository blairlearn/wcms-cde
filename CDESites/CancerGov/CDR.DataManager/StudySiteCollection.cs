using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace CancerGov.CDR.DataManager {
	/// <summary>
	/// Summary description for StudySiteList.
	/// </summary>
	// Hmm Tricky Tricky.  we need these things to be drawn in a certain order.
	// while we do not do presentation, we should make it so that we can get the
	// right info out.  We can actually order them later.
	public class StudySiteCollection  {
		protected DataView dvSiteList;
		bool bIsDisposed = false;

		public DataView SiteTable {
			get {return dvSiteList;}
		}

		public StudySiteCollection(DataView dvSiteList) {

			//I have no time to mess around.  If I figure out a fancier way, well then I will
			this.dvSiteList = dvSiteList;

		}

	}


//	public class CDRGeopoliticalContainer {
//
//		string strName = "";
//		CDRGeopoliticalContainerType cgctType;
//		IGeopoliticalContainer igcChildContainer;
//
//
//	}


//	public class StudySite {
//
//		int iProtocolContactInfoHtmlID;
//		int iProtocolID;
//		string strCity = "";
//		string strState = "";
//		string strCountry = "";
//		string strOrganizationName = "";
//		string strHTML = "";
//
//		public StudySite (DataRow drSite) {
//			
//
//		}
//
//	}

}
