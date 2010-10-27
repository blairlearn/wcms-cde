using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using CancerGov.Text;
using CancerGov.Exceptions;

namespace CancerGov.CDR.DataManager
{
	/// <summary>
	/// This gets protocol search info from the db
	/// </summary>
	public class XMLProtocolSearchParametersCollection
	{
		private int iProtocolSearchID = -1;
		private string strXML = "";
		private DataTable dtParams;

		//Params
		private string strCancerTypes = "";
		private string strCancerStages = "";
		private string strTreatmentType = "";
		private int iZipProximity = 20; //There can be only one
		private string strStates = ""; //Can be multiple, Don't Really need this param
		private string strInstitutions = ""; //There can be multiple
		private string strDrug = "";
		private string strSponsor = "";
		private string strSpecialCategory = "";
		private string strInvestigators = "";
		private string strLeadOrgs = "";
		private string strTrialPhase = "";
		private string strTrialType = "";
		private string strAlternateProtocolIDs = "";
		private string strZip = ""; //There can be only one
		private string strCity = ""; //There can be only one
		private string strCountry = ""; //There can be only one

		//These come in as a 0 or a 1.  Actually, they are probably nothing or 1, except trial status
		private bool bNIHOnly = false; //N to NIH Clinical Center
		private bool bNewOnly = false; //N to new trials
		private bool bTrialStatus = true; //Active trials or Y
		private bool bDrugFormula = false; //Or search formula


		#region Class Properties

		public bool NIHOnly {
			get {return bNIHOnly;}
		}

		public bool NewOnly {
			get {return bNewOnly;}
		}

		public bool TrialStatus {
			get {return bTrialStatus;}
		}

		public bool DrugFormula {
			get {return bDrugFormula;}
		}

		public string TrialTypes {
			get { return strTrialType; }
		}

		public string ProtocolIDs {
			get { return strAlternateProtocolIDs; }
		}

		public string ZipCode {
			get { return strZip; }
		}

		public string City {
			get { return strCity; }
		}

		public string Country {
			get { return strCountry; }
		}

		public string Interventions {
			get { return strTreatmentType;}
		}

		public string CancerTypes {
			get { return strCancerTypes; }
		}

		public string CancerStages {
			get { return strCancerStages; }
		}

		public string TreatmentTypes {
			get { return strTreatmentType; }
		}

		public string States {
			get { return strStates; }
		}

		public string Institutions {
			get { return strInstitutions; }
		}

		public string Drugs {
			get { return strDrug; }
		}

		public string Sponsors {
			get { return strSponsor; }
		}

		public string SpecialCategory 
		{
			get { return strSpecialCategory; }
		}

		public string Investigators 
		{
			get { return strInvestigators; }
		}

		public string LeadOrgs {
			get { return strLeadOrgs; }
		}

		public string Phases {
			get { return strTrialPhase; }
		}

		public string ZipProximity {
			get { return iZipProximity.ToString(); }
		}

		public int ProtocolSearchID {
			get { return iProtocolSearchID; }
		}

		#endregion

		public XMLProtocolSearchParametersCollection(int iProtocolSearchID)
		{

			this.iProtocolSearchID = iProtocolSearchID;

			this.dtParams = new DataTable("Parameters");
			SqlDataAdapter daParams = null;

			try {
				daParams = new SqlDataAdapter("usp_GetProtocolSearchParamsID",ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
				daParams.SelectCommand.CommandType = CommandType.StoredProcedure;
				daParams.SelectCommand.Parameters.Add("@ProtocolSearchID",iProtocolSearchID);

				daParams.Fill(dtParams);
			} catch (SqlException seSQL) {
				throw new SearchCriteriaFetchFailureException(seSQL.Message);
			} finally {
				if (daParams != null) {
					daParams.Dispose();
				}
			}

			//parse xml and set parameters
			if (dtParams != null && dtParams.Rows.Count > 0) {
				FillParamsFromXml();
			} else {
				throw new SearchCriteriaFetchFailureException("Empty Search Parameters");
			}

			//if XML cannot be parsed, then recreate the xml
		}

		private void FillParamsFromXml() {
			
			string strXML = "";

			strXML = Strings.Clean(dtParams.Rows[0]["ParameterDisplayHTML"].ToString());

			
			//throw new Exception(strXML.ToString());
			XmlDocument xdDoc = new XmlDocument();

			try {
				xdDoc.LoadXml(strXML);
			} catch (Exception e) {
				//invalid xml
				throw new SearchCriteriaInvalidXmlException(e.Message);
			}

			//Loop through doc
			XmlNode xnNode = xdDoc.FirstChild; //This should be <ProtocolSearch>"

			if (xnNode != null) {
				if (xnNode.HasChildNodes) {
					foreach (XmlNode xnTmp in xnNode.ChildNodes) {
						 
						switch (xnTmp.Name.Trim().ToUpper()) {
							case "NEWTRIALS" : {
								this.bNewOnly = true;
								break;
							}
							case "NIHCLINICALCENTER" : {
								this.bNIHOnly = true;
								break;
							}
							case "CLOSEDTRIALS" : {
								this.bTrialStatus = false;
								break;
							}

							case "CANCERTYPES" : {
								strCancerTypes = XmlChildrenToStringList(xnTmp);
								
								break;
							}
							case "CANCERSTAGES" : {
								strCancerStages = XmlChildrenToStringList(xnTmp);
								break;
							}
							case "INSTITUTIONS" : {
								strInstitutions = XmlChildrenToStringList(xnTmp);
								break;
							}
							case "INVESTIGATORS" : {
								strInvestigators = XmlChildrenToStringList(xnTmp);
								break;
							}
							case "LEADORGS" : {
								strLeadOrgs = XmlChildrenToStringList(xnTmp);
								break;
							}
							case "TRIALTYPES" : {
								strTrialType = XmlChildrenToStringList(xnTmp);
								break;
							}
							case "PROTOCOLIDS" : {
								this.strAlternateProtocolIDs = xnTmp.InnerText;
								break;
							}
							case "TRIALPHASES" : {
								strTrialPhase = XmlChildrenToStringList(xnTmp);
								break;
							}
							case "SPONSORS" : {
								strSponsor = XmlChildrenToStringList(xnTmp);
								break;
							}
							case "INTERVENTIONS" : {
								strTreatmentType = XmlChildrenToStringList(xnTmp);
								
								break;
							}
							case "DRUGINFO": {
								DecodeDrugInfo(xnTmp);
								break;
							}
							case "LOCATION": {
								DecodeLocation(xnTmp);
								break;
							}
							case "SPECIALCATEGORYS" : 
							{
								strSpecialCategory = XmlChildrenToStringList(xnTmp);
								break;
							}


						}
					}
				}
			}


		}

		private void DecodeDrugInfo(XmlNode xnNode) {
			//Get attributes
			foreach (XmlAttribute xaAttrib in xnNode.Attributes) {
				switch(xaAttrib.Name.Trim().ToUpper()) {
					case "DRUGFORMULA" : {
						if (xaAttrib.Value.Trim().ToUpper() == "AND") {
							this.bDrugFormula = true;
						}
						break;
					}
				}			
			}

			if (xnNode.HasChildNodes) {
				foreach (XmlNode xnTmp in xnNode.ChildNodes) {
					switch(xnTmp.Name.Trim().ToUpper()) {
						case "DRUGS" : {
							this.strDrug = XmlChildrenToStringList(xnTmp);
							break;
						}

					}
				}
			}
		}

		private void DecodeLocation(XmlNode xnNode) {
			if (xnNode.HasChildNodes) {
				foreach (XmlNode xnTmp in xnNode.ChildNodes) {
					switch(xnTmp.Name.Trim().ToUpper()) {
						case "CITY" : {
							this.strCity = xnTmp.InnerText;
							break;
						}

						case "STATES" : {
							this.strStates = XmlChildrenToStringList(xnTmp);
							break;
						}

						case "COUNTRY" : {
							this.strCountry = xnTmp.InnerText;
							break;
						}

						case "ZIPCODE" : {
							this.strZip = xnTmp.InnerText;

							foreach (XmlAttribute xaAttrib in xnTmp.Attributes) {
								switch(xaAttrib.Name.Trim().ToUpper()) {
									case "PROXIMITY" : {
										
										this.iZipProximity = Strings.ToInt(xaAttrib.Value.Trim());
										
										break;
									}
								}			
							}
							break;
						}


					}			
				}
			}
		}

		private string XmlChildrenToStringList(XmlNode xnNode) {
			StringBuilder sbContent = new StringBuilder();
			bool bIsFirst = true;

			if (xnNode.HasChildNodes) {
				foreach (XmlNode xnTmp in xnNode.ChildNodes) {
					if (bIsFirst) {
						bIsFirst = false;
					} else {
						sbContent.Append(", ");
					}
					sbContent.Append(xnTmp.InnerText);
				}
			}

			return sbContent.ToString();
		}

	}
}
