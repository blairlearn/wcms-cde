using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Xml;
//using System.Xml.Xsl;
//using CancerGov.Common;
//using CancerGov.Common.ErrorHandling;

using CancerGov.Text;
using CancerGov.Exceptions;

namespace CancerGov.CDR.DataManager
{
	/// <summary>
	///Defines the Protocol data access component, resolves Protocol documentId and exposes Protocol XML<br/>
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  5-30-2002<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	///<br/>
	///</summary>
	public class Protocol
	{
		private int cdrId = -1;
		private int resultNumber = 0;
		private int iProtocolSearchID = -1;
		private string protocolTitle = "";
		private string alternateTitle = "";
		private string trialType = "";
		private string currentStatus = "";
		private string ageRange = "";
		private string trialSponsor = "";
		private string primaryProtocolID = "";
		private string alternateProtocolIDs = "";
		private string phase = "";
		private string strSectionList = "";
		private ProtocolVersions pvVersion;
		private ProtocolSectionList pslSections;
		private StudySiteList sslStudySites;
		private DateTime dateFirstPublished;
		private DateTime dateLastModified;
		private ProtocolTypes documentTypeID = ProtocolTypes.Protocol;

		private bool bHasResultNumber = false;

		#region Class Properties

		/// <summary>
		/// Gets Protocol documentId
		/// </summary>
		public int CdrId
		{
			get {return cdrId;}
		}

		public string FullCdrId {
			get {
				string strCDRID = cdrId.ToString().PadLeft(10,'0'); 
				strCDRID = "CDR" + strCDRID;
				return strCDRID;
			}
		}

		//CDR0000063216

		public int ResultNumber {
			get {return resultNumber;}
		}

		/// <summary>
		/// Gets Protocol title
		/// </summary>
		public string ProtocolTitle
		{
			get {return protocolTitle;}
		}

		/// <summary>
		/// Gets Protocol title
		/// </summary>
		public string AlternateTitle
		{
			get {return alternateTitle;}
		}

        public string TrialType
		{
			get {return trialType;}
		}

		public string CurrentStatus
		{
			get {return currentStatus;}
		}

		public string AgeRange
		{
			get {return ageRange;}
		}

		public string TrialSponsor
		{
			get {return trialSponsor;}
		}

		public string PrimaryProtocolID
		{
			get {return primaryProtocolID;}
		}

		public string AlternateProtocolIDs
		{
			get {return alternateProtocolIDs;}
		}
        
		public string Phase {
			get {return phase;}
		}

		public string SectionList {
			get {return strSectionList;}
		}

		public ProtocolSectionList Sections {
			get {return pslSections;}
		}

		public StudySiteList Sites {
			get {return sslStudySites;}
		}

		public ProtocolVersions ProtocolVersion {
			get {return pvVersion;}
		}

		public int ProtocolSearchID {
			get {return iProtocolSearchID;}
		}

		public DateTime DateFirstPublished {
			get {return dateFirstPublished;}
		}

		public DateTime DateLastModified {
			get {return dateLastModified;}
		}

		public ProtocolTypes ProtocolType 
		{
			get {return documentTypeID;}
		}

		#endregion
		
		#region Basic Class Constructor

 		/// <summary>
		/// Basic web application protocol constructor
		/// </summary>
		public Protocol(int iProtocolSearchID, DataRow drProtocolInfo, DataView dvSections, DataView dvStudySites, ProtocolVersions pvVersion, string protocolSectionList) {

			
			this.iProtocolSearchID = iProtocolSearchID;

			//fill protocol
			FillProtocolInfo(drProtocolInfo);
			resultNumber = Strings.ToInt(drProtocolInfo["ResultNumber"].ToString());

			pslSections = new ProtocolSectionList(dvSections);
			sslStudySites = new StudySiteList(dvStudySites);
			this.pvVersion = pvVersion;
			this.bHasResultNumber = true;
			this.strSectionList = protocolSectionList;
		}


		//New for 2.42
		public Protocol(int iProtocolSearchID, DataRow drProtocolInfo, DataRow[] sections, DataView dvStudySites, ProtocolVersions pvVersion, string protocolSectionList) {

			
			this.iProtocolSearchID = iProtocolSearchID;

			//fill protocol
			FillProtocolInfo(drProtocolInfo);
			resultNumber = Strings.ToInt(drProtocolInfo["ResultNumber"].ToString());

			pslSections = new ProtocolSectionList(sections);
			sslStudySites = new StudySiteList(dvStudySites);
			this.pvVersion = pvVersion;
			this.bHasResultNumber = true;
			this.strSectionList = protocolSectionList;
		}

		//New for 2.42
		public Protocol(int iProtocolSearchID, DataRow drProtocolInfo, DataRow[] sections, ProtocolVersions pvVersion, string protocolSectionList) {
			
			this.iProtocolSearchID = iProtocolSearchID;

			//fill protocol
			FillProtocolInfo(drProtocolInfo);
			resultNumber = Strings.ToInt(drProtocolInfo["ResultNumber"].ToString());

			pslSections = new ProtocolSectionList(sections);
			
			this.pvVersion = pvVersion;
			this.bHasResultNumber = true;
			this.strSectionList = protocolSectionList;
		}

		public Protocol(int iProtocolSearchID, DataRow drProtocolInfo, DataView dvSections, ProtocolVersions pvVersion, string protocolSectionList) {

			this.iProtocolSearchID = iProtocolSearchID;

			//fill protocol
			FillProtocolInfo(drProtocolInfo);
			resultNumber = Strings.ToInt(drProtocolInfo["ResultNumber"].ToString());

			pslSections = new ProtocolSectionList(dvSections);

			this.pvVersion = pvVersion;
			this.bHasResultNumber = true;
			this.strSectionList = protocolSectionList;
		}

		public Protocol(int iProtocolSearchID, DataRow drProtocolInfo, ProtocolVersions pvVersion, string protocolSectionList) {

			this.iProtocolSearchID = iProtocolSearchID;

			//fill protocol
			FillProtocolInfo(drProtocolInfo);
			resultNumber = Strings.ToInt(drProtocolInfo["ResultNumber"].ToString());
			this.pvVersion = pvVersion;
			this.bHasResultNumber = true;
			this.strSectionList = protocolSectionList;
		}

		public Protocol(DataRow drProtocolInfo, ProtocolVersions pvVersion, string protocolSectionList) {

			this.iProtocolSearchID = -1;

			//fill protocol
			FillProtocolInfo(drProtocolInfo);
			resultNumber = -1;
			this.pvVersion = pvVersion;
			this.bHasResultNumber = false;
			this.strSectionList = protocolSectionList;
		}

		/// <summary>
		/// Basic web application protocol constructor
		/// </summary>
		public Protocol(int iProtocolID, string strSectionList, int iProtocolSearchID, ProtocolVersions pvVersion) {

			bool StudySitesExist = false;
			bool SectionsExist = false;
			bool ProtocolsExist = false;

			this.pvVersion = pvVersion;
			this.iProtocolSearchID = iProtocolSearchID;
			this.strSectionList = strSectionList;

			SqlDataAdapter daProtocol = null;
			DataSet dsProtocol = new DataSet();

			//Fill the data set
			try {
				//11-23-2004 BryanP: SCR1002 Changed the database object fetching from adhoc to stored proc. 
				daProtocol = new SqlDataAdapter("usp_GetProtocolByProtocolID", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
				daProtocol.SelectCommand.CommandType = CommandType.StoredProcedure;
				daProtocol.SelectCommand.CommandTimeout = Strings.ToInt(ConfigurationSettings.AppSettings["CTSearchTimeout"]);
				daProtocol.SelectCommand.Parameters.Add(new SqlParameter("@ProtocolID", iProtocolID));
				daProtocol.SelectCommand.Parameters.Add(new SqlParameter("@SectionList", strSectionList));
				daProtocol.SelectCommand.Parameters.Add(new SqlParameter("@Version", (int)pvVersion));
				daProtocol.SelectCommand.Parameters.Add(new SqlParameter("@ProtocolSearchID", iProtocolSearchID));

				daProtocol.Fill(dsProtocol);
			} catch (SqlException seSQL) {
				throw new ProtocolFetchFailureException(seSQL.Message);
			} finally {
				if (daProtocol != null) {
					daProtocol.Dispose();
				}
			}

			//Everything is here
			if (dsProtocol.Tables == null) {
				throw new ProtocolTableEmptyException("All tables are empty");
			}

			//Check to see what tables exist so we do not get nulls
			if (dsProtocol.Tables.Count == 3) {
				StudySitesExist = true;
				SectionsExist = true;
				ProtocolsExist = true;
			} else if (dsProtocol.Tables.Count == 2) {
				SectionsExist = true;
				ProtocolsExist = true;
			} else if (dsProtocol.Tables.Count == 1) {
				ProtocolsExist = true;
			} else {
				if (dsProtocol != null) {
					dsProtocol.Dispose();
				}

				throw new ProtocolTableMiscountException(dsProtocol.Tables.Count.ToString());
			}
			
			if (dsProtocol.Tables[0].Rows.Count == 1) {

			} else if (dsProtocol.Tables[0].Rows.Count > 1) {
				//throw too many rows error -- Um, I have no clue how this could happen though

				if (dsProtocol != null) {
					dsProtocol.Dispose();
				}
				throw new ProtocolTableMiscountException("Somehow the there are 2 protocols with the same id");

			} else {
				if (dsProtocol != null) {
					dsProtocol.Dispose();
				}

				throw new ProtocolTableEmptyException("Protocol table is empty");
			}

			FillProtocolInfo(dsProtocol.Tables[0].Rows[0]);

			if (SectionsExist) {

				//Should only be one protocol here
				DataView dvSections = new DataView(dsProtocol.Tables[1]);
				pslSections = new ProtocolSectionList(dvSections);
			}

			if (StudySitesExist) {
				//Should only be one protocol here
				DataView dvStudySites = new DataView(dsProtocol.Tables[2]);
				dvStudySites.Sort="Country, State, City, OrganizationName asc";
				sslStudySites = new StudySiteList(dvStudySites);
			}

			if (dsProtocol != null) {
				dsProtocol.Dispose();
			}

		}

		public Protocol(
			int protocolID, 
			string protocolTitle, 
			string alternateTitle, 
			string trialType, 
			string currentStatus, 
			string ageRange, 
			string trialSponsor,
			string primaryProtocolID,
			string alternateProtocolIDs,
			string phase,
			DateTime dateLastModified,
			DateTime dateFirstPublished,
			int documentTypeID,
			ProtocolVersions version
			) {

			this.cdrId = protocolID;
			this.protocolTitle = protocolTitle;
			this.trialType = trialType;
			this.currentStatus = currentStatus;
			this.ageRange = ageRange;
			this.trialSponsor = trialSponsor;
			this.primaryProtocolID = primaryProtocolID;
			this.alternateProtocolIDs = alternateProtocolIDs;
			this.phase = phase;
			this.dateLastModified = dateLastModified;
			this.dateFirstPublished = dateFirstPublished;
			this.documentTypeID = (ProtocolTypes)documentTypeID;
			this.pvVersion = pvVersion;

			//We could create sections here, but they may be null...

		}

		#endregion

		public string GetSectionByID (ProtocolSectionTypes pstType) {

			
			if (pslSections[(int)pstType] != null) {
				
				return ((ProtocolSection)pslSections[(int)pstType]).HTML;
			} else {
				return "";
			}

		}


		private void FillProtocolInfo(DataRow drProtocolInfo) {

			//fill protocol
			cdrId = Strings.ToInt(drProtocolInfo["ProtocolID"].ToString());
			protocolTitle = Strings.IfNull(Strings.Clean(drProtocolInfo["ProtocolTitle"].ToString()),"");
			alternateTitle = Strings.IfNull(Strings.Clean(drProtocolInfo["AlternateTitle"].ToString()),"");
			trialType = Strings.IfNull(Strings.Clean(drProtocolInfo["TypeOfTrial"].ToString()),"");
			currentStatus = Strings.IfNull(Strings.Clean(drProtocolInfo["CurrentStatus"].ToString()),"");
			ageRange = Strings.IfNull(Strings.Clean(drProtocolInfo["AgeRange"].ToString()),"");
			trialSponsor = Strings.IfNull(Strings.Clean(drProtocolInfo["SponsorOfTrial"].ToString()),"");
			primaryProtocolID = Strings.IfNull(Strings.Clean(drProtocolInfo["PrimaryProtocolID"].ToString()),"");
			alternateProtocolIDs = Strings.IfNull(Strings.Clean(drProtocolInfo["AlternateProtocolIDs"].ToString()),"");
			phase = Strings.IfNull(Strings.Clean(drProtocolInfo["Phase"].ToString()),"");
			dateLastModified = Strings.ToDateTime(Strings.Clean(drProtocolInfo["DateLastModified"].ToString()));
			dateFirstPublished = Strings.ToDateTime(Strings.Clean(drProtocolInfo["DateFirstPublished"].ToString()));
			if (Strings.Clean(drProtocolInfo["DocumentTypeID"].ToString()) == "28") 
			{
				documentTypeID = ProtocolTypes.CTGovProtocol;
			}
			if (bHasResultNumber) {
				resultNumber = Strings.ToInt(drProtocolInfo["ResultNumber"].ToString());
			}
		}

	}


}
