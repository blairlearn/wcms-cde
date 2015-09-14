using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using CancerGov.Common;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.CDR.DataManager
{
	/// <summary>
	/// <b>Data Access Class for CDR-source Genetic Professional</b><br/>
	/// 1.  Exposes Genetic Professional XML through class property, Xml,<br/>
	///     and Genetic Professional HTML though class accessor method, GetHtml(string stylesheetPath)<br/>
	///     <br/>
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  12-31-2002<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	/////11-23-2004 BryanP: SCR1002 Changed the database object fetching from adhoc to stored proc. 
	/// </summary>
	public class GeneticProfessional
	{
		private string documentId;

		/// <summary>
		/// Class constructor<br/>
		/// 1.  Utilizes usp_GetCDRDocumentXml(in database specified by CDRDbConnectionString appSetting)
		///  to populate xml class variable<br/>
		/// </summary>
		/// <param name="cdrId">CDRID for Genetic Professional Data Document, a.k.a. DocumentID</param>
		public GeneticProfessional(string cdrId)
		{
			documentId = cdrId;	
		}
        public GeneticProfessional()
        {
            
        }

		public string GetXML()
		{
			string docXML = "";
			DataTable dbTable = new DataTable();
			SqlDataAdapter dbAdapter = null;

			try {
				//11-23-2004 BryanP: SCR1002 Changed the database object fetching from adhoc to stored proc. 
                dbAdapter = new SqlDataAdapter("usp_GetCDRDocumentXml", ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
                dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dbAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DocumentId", documentId));
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException sqlE) {
				CancerGovError.LogError("Genetic Professional DAC", this.ToString(), ErrorType.DbUnavailable, sqlE); 
			} finally {
				if (dbAdapter != null) {
					dbAdapter.Dispose();
				}
			}
	
			if(dbTable.Rows.Count > 0)
			{
				docXML = dbTable.Rows[0]["XML"].ToString().Trim();
			}

			if (dbTable != null) {
				dbTable.Dispose();
			}

			return docXML;
		}
		
		/// <summary>
		/// Class method performs transform of DAC XML data to XHTML according to "GeneticsProfessional.xsl"
		/// </summary>
		/// <param name="stylesheetPath">Mapped path of the xsl container directory</param>
		/// <returns>Genetic professional display XHTML</returns>
		public string GetHtml(string stylesheetPath) {
			string html = "";
			string xml = "";
			DataTable dbTable = new DataTable();

			SqlDataAdapter sqlAdapter = null;
						
			try {
                sqlAdapter = new SqlDataAdapter("usp_GetCacheDocument", ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
				sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				sqlAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DocumentID", documentId));

				sqlAdapter.Fill(dbTable);
			}
			catch(SqlException sqlE) {
				CancerGovError.LogError("GeneticProfessional DAC GetHTML method:  Error accessing cache document:  " + documentId, this.ToString(), ErrorType.DbUnavailable, sqlE);
			} finally {
				if (sqlAdapter != null) {
					sqlAdapter.Dispose();
				}
			}
			
			if(dbTable.Rows.Count > 0) {
			 	//Retrieve cached document HTML
				html = dbTable.Rows[0]["DocumentHTML"].ToString();
			}
			else {
			 	//Retrieve document XML
				xml = GetXML();
				
				if(Functions.HasValue(xml)) {
				 	//Transform to HTML
					Transform transformer = new Transform();

					XmlDocument xmlGenProf = new XmlDocument();
					XmlDocument xslGenProf = new XmlDocument();
					
					//Load Genetics Professional XML
					xmlGenProf.LoadXml(xml);

					//Load Genetics Professional Display XSL
					try {
						xslGenProf.Load(stylesheetPath + "\\xsl\\GeneticsProfessional.xsl");
					}
					catch(DirectoryNotFoundException) {
						CancerGovError.LogError("Genetics Professional XSL Directory", this.ToString(), ErrorType.FileNotFound, ErrorTypeDesc.FileNotFound);
					}
					catch(FileNotFoundException) {
						CancerGovError.LogError("Genetics Professional XSL", this.ToString(), ErrorType.FileNotFound, ErrorTypeDesc.FileNotFound);
					}
					
					//Transform XML to HTML
					if(xmlGenProf.HasChildNodes && xslGenProf.HasChildNodes) {
						html += transformer.GetHtml(xmlGenProf, xslGenProf, null);
					}


					//This cannot be good for performance.  So it is commented out. -- BryanP 11/15/04
					//xmlGenProf.RemoveAll();
					//xslGenProf.RemoveAll();
					xmlGenProf = null;
					xslGenProf = null;
					
					if(Functions.HasValue(html)) {
					 	//Cache HTML
						int rowsAffected = 0;

						SqlCommand sqlCommand = null;
						
						try {
							//11-23-2004 BryanP: SCR1002 Changed the database object fetching from adhoc to stored proc. 
							sqlCommand = new SqlCommand("usp_InsertCacheDocument");
                            sqlCommand.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
							sqlCommand.CommandType = CommandType.StoredProcedure;

							SqlParameter sqlParam = null;

							sqlParam = new SqlParameter("@DocumentID", SqlDbType.Int);
							sqlParam.Value = documentId;
							sqlCommand.Parameters.Add(sqlParam);

							//Ok, this is one of those oddities.  Basically if you want to insert text you need 
							//to go the long way to create a param
							sqlParam = new SqlParameter("@DocumentHTML", SqlDbType.NText, html.Length + 1);
							sqlParam.Value = html;
							sqlCommand.Parameters.Add(sqlParam);

							sqlCommand.Connection.Open();
							rowsAffected = sqlCommand.ExecuteNonQuery();
							sqlCommand.Connection.Close();
						}
						catch(SqlException sqlE) {
							CancerGovError.LogError("GeneticProfessional DAC GetHTML method:  Error caching document:  " + documentId + ".", this.ToString(), ErrorType.DbUnavailable, sqlE);
						} finally {
							if (sqlCommand != null) {
								sqlCommand.Dispose();
							}
						}
					}
					else {
						CancerGovError.LogError("GeneticProfessional DAC GetHTML method:  Error transforming XML to HTML:  " + documentId + ".", this.ToString(), ErrorType.InvalidArgument, ErrorTypeDesc.InvalidArgument);
					}
				}
				else {
					CancerGovError.LogError("GeneticProfessional DAC GetHTML method:  Error accessing XML:  " + documentId + ".", this.ToString(), ErrorType.InvalidArgument, ErrorTypeDesc.InvalidArgument);
				}
			}

			if (dbTable != null) {
				dbTable.Dispose();
			}

			return html;
		}

        public DataSet GetSearchFormMasterData()
        {
            DataSet dbSet = new DataSet();
            SqlDataAdapter dbAdapter = null;
            try
            {
                dbAdapter = new SqlDataAdapter("Select ShortName + ' - ' + ISNULL(FullName, ShortName) AS Name, ShortName AS StateAbbr From PoliticalSubUnit WHERE ShortName IS NOT NULL AND CountryName = 'U.S.A.' AND ShortName <> 'AS' Order By ShortName", ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
                dbAdapter.Fill(dbSet, "States");
                dbAdapter.SelectCommand.CommandText = "Select FamilyCancerSyndrome, FamilyCancerSyndrome + ';' + CONVERT(varchar,FamilyCancerSyndromeListID) AS Value From GenProfFamilyCancerSyndromeList Order By FamilyCancerSyndrome";
                dbAdapter.Fill(dbSet, "CancerFamily");
                dbAdapter.SelectCommand.CommandText = "Select CancerTypeSite, CancerTypeSite + ';' + CONVERT(varchar,CancerTypeSiteID) As [Type] From GenProfCancerTypeSite Order By CancerTypeSite";
                dbAdapter.Fill(dbSet, "CancerType");
                dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dbAdapter.SelectCommand.CommandText = "usp_GetGenProfCountry";
                dbAdapter.Fill(dbSet, "Country");
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("GeneticProfessional GetSearchFormMasterData method:  Error Getting the Master data for the Search Form:  ","", this.ToString(), ErrorType.DbUnavailable, sqlE);                
            }
            finally
            {
                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                }
            }

            return dbSet;
        }


        public DataTable GetCancerGeneticProfessionals(string cancerType, string cancerFamily, string city, string state, string country, string lastName)
        {

            DataTable dbTable = new DataTable();

            //Execute search and show results
            string commandText = "usp_GetCancerGeneticProfessionals";
            commandText += " @CancerType=" + Functions.ParseNameValue(cancerType, 1);
            commandText += ", @CancerFamily=" + Functions.ParseNameValue(cancerFamily, 1);
            commandText += ", @City=" + Functions.EvalArg(city, true);
            commandText += ", @StateId=" + Functions.ParseNameValue(state, 1);
            commandText += ", @CountryId=" + Functions.ParseNameValue(country, 1);
            commandText += ", @LastName=" + Functions.EvalArg(lastName, true);


            SqlDataAdapter dbAdapter = null;
            try
            {
                dbAdapter = new SqlDataAdapter(commandText, ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
                dbAdapter.Fill(dbTable);
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("GeneticProfessional GetCancerGeneticProfessionals method:  Error Getting the cancer genetic professional data:  ", "", this.ToString(), ErrorType.DbUnavailable, sqlE);
            }
            finally
            {
                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                }
            }

            return dbTable;
        }
	}
}
