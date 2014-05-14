using System;
using System.Data;
using CancerGov.Collections;
using CancerGov.Text;

namespace CancerGov.CDR.DataManager
{

	public class ProtocolSectionList : IntHashList {

		public ProtocolSectionList (DataView dvSections) {
			
			foreach (DataRowView drvSection in dvSections) {
				
				int iPSID = -99;
				int iPID = -99;
				int iSTID = 0;
				string html = "";

				iPSID = Strings.ToInt(drvSection.Row["ProtocolSectionID"].ToString());
				iPID = Strings.ToInt(drvSection.Row["ProtocolID"].ToString());
				iSTID = Strings.ToInt(drvSection.Row["SectionTypeID"].ToString());
				html = Strings.Clean(drvSection.Row["HTML"].ToString());

				if (this.Contains(iSTID)) {
					
				} else {
					if (html != null) {
						this.Add(iSTID,new ProtocolSection(iPSID,iPID,(ProtocolSectionTypes)iSTID,html));
					}
				}
		
			}

		}

		public ProtocolSectionList (DataRow[] sections) {

			foreach (DataRow drSection in sections) {
				
				int iPSID = -99;
				int iPID = -99;
				int iSTID = 0;
				string html = "";

				iPSID = Strings.ToInt(drSection["ProtocolSectionID"].ToString());
				iPID = Strings.ToInt(drSection["ProtocolID"].ToString());
				iSTID = Strings.ToInt(drSection["SectionTypeID"].ToString());
				html = Strings.Clean(drSection["HTML"].ToString());

				if (this.Contains(iSTID)) {
					
				} else {
					if (html != null) {
						this.Add(iSTID,new ProtocolSection(iPSID,iPID,(ProtocolSectionTypes)iSTID,html));
					}
				}
		
			}

		}

		public new ProtocolSection this[int key] {
			get {
				if(base[key] != null) {
					return ((ProtocolSection)base[key]);
				}
				else {
					return null;
				}
			}
		}
	}

	/// <summary>
	/// Summary description for ProtocolSection.
	/// </summary>
	public class ProtocolSection {

		private int iProtocolSectionID = -1;
		private int iProtocolID = -1;
		private ProtocolSectionTypes pstSectionType = ProtocolSectionTypes.Nothing; 
		private string strHTML = "";

		public string HTML {
			get {return strHTML;}
		}

		public ProtocolSection(int iProtocolSectionID, int iProtocolID, ProtocolSectionTypes pstst, string strHTML) {
			this.iProtocolSectionID = iProtocolSectionID;
			this.iProtocolID = iProtocolID;
			this.pstSectionType = pstSectionType;
			this.strHTML = strHTML;
		}

	}

}


