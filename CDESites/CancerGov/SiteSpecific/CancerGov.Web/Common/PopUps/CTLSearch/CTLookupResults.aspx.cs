using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using CancerGov.CDR.ClinicalTrials.Helpers;
using CancerGov.CDR.DataManager;
using CancerGov.Text;
using CancerGov.Common.HashMaster;

namespace CancerGov.Web
{

 	/// <summary>
	/// Summary description for CTLookupResults.
	/// </summary>
	public partial class CTLookupResults : CTLookupBase
	{

		private string caption = "";

		protected string Caption {
			get {return caption;}
			set {
                caption = value;
                resultsCaption.Visible = !String.IsNullOrEmpty(caption);
            }
		}

        protected void Page_Load(object sender, System.EventArgs e)
        {

            ValidateInputData(Request.Params["fld"]);
            ValidateInputData(Request.Params["type"]);
            ValidateInputData(Request.Params["title"]);

            ValidateValidValues(Request.Params["fld"], new ArrayList { "institution", "drug", "intervention", "investigator", "leadorg" });
            ValidateValidValues(Title, new ArrayList { "find+hospitals/institutions", 
                "find+drug", "treatment/intervention", "find+trial+investigators", "find+lead+organizations" });

            resultsForm.EnableViewState = false;

            //Local variables			
            DataTable dbTable = new DataTable();
            string parameter = "";
            string lookupMethod = "";
            string qString = "";

            //Capture submitted parameters
            string keyword = Strings.IfNull(Strings.Clean(Request.Params["keyword"]), "");
            string alphaIndex = Strings.IfNull(Strings.Clean(Request.Params["alphaIndex"]), "");
            string fld = Strings.IfNull(Strings.Clean(Request.Params["fld"]), "");
            string page = Strings.IfNull(Strings.Clean(Request.Params["page"]), "");

            //Build querystring for result pager
            if (fld != "")
            {
                qString += "fld=" + Server.UrlEncode(fld);
            }

            if (keyword != "")
            {
                if (qString.Length > 0)
                {
                    qString += "&";
                }
                keyword = keyword.Replace("[", "[[]");
                qString += "keyword=" + Server.UrlEncode(keyword);
            }

            if (alphaIndex != "")
            {
                if (qString.Length > 0)
                {
                    qString += "&";
                }
                qString += "alphaIndex=" + Server.UrlEncode(alphaIndex);
            }

            //Build parameter string
            if (alphaIndex != "")
            {
                parameter = alphaIndex;
                lookupMethod = ProtocolSearchLookupMethods.AlphaNumIndex;
            }
            else if (keyword != "")
            {
                parameter = keyword;
                lookupMethod = ProtocolSearchLookupMethods.Keyword;
            }

            if (parameter != "")
            {
                switch (fld.Trim().ToLower())
                {
                    case "drug":
                        dbTable = ProtocolSearchFormData.GetProtocolDrugs(parameter, lookupMethod);

                        //Format result display
                        if (dbTable.Rows.Count > 0)
                        {
                            //results.DataSource = dbTable;

                            // Create hash code for Names and add hash codes to CDRID separator by delimiter 
                            //   use this list as DataSource for DataGrid control -LH
                            CTLookupList list = new CTLookupList(dbTable);
                            results.DataSource = list;

                            results.Visible = true;
                            results.DataBind();
                        }

                        break;
                    case "institution":
                        dbTable = ProtocolSearchFormData.GetProtocolInstitutions(parameter, lookupMethod);

                        //Format result display
                        if (dbTable.Rows.Count > 0)
                        {
                            //results.DataSource = dbTable;

                            // Create hash code for Names and add hash codes to CDRID separator by delimiter 
                            //   use this list as DataSource for DataGrid control -LH
                            CTLookupList list = new CTLookupList(dbTable);
                            results.DataSource = list;
                            
                            results.Visible = true;
                            results.DataBind();
                        }
                        break;
                    case "leadorg":
                        dbTable = ProtocolSearchFormData.GetProtocolLeadOrganizations(parameter, lookupMethod);

                        //Format result display
                        if (dbTable.Rows.Count > 0)
                        {
                            //results.DataSource = dbTable;

                            // Create hash code for Names and add hash codes to CDRID separator by delimiter 
                            //   use this list as DataSource for DataGrid control -LH
                            CTLookupList list = new CTLookupList(dbTable);
                            results.DataSource = list;
                            
                            results.Visible = true;
                            results.DataBind();
                        }
                        break;
                    case "investigator":
                        dbTable = ProtocolSearchFormData.GetProtocolInvestigators(parameter, lookupMethod);

                        //Format result display
                        if (dbTable.Rows.Count > 0)
                        {
                            //results.DataSource = dbTable;

                            // Create hash code for Names and add hash codes to CDRID separator by delimiter 
                            //   use this list as DataSource for DataGrid control -LH
                            CTLookupList list = new CTLookupList(dbTable);
                            results.DataSource = list;

                            results.Visible = true;
                            results.DataBind();
                        }
                        break;
                    case "intervention":
                        dbTable = ProtocolSearchFormData.GetProtocolInterventions(parameter, lookupMethod);

                        //Format result display
                        if (dbTable.Rows.Count > 0)
                        {
                            //results.DataSource = dbTable;

                            // Create hash code for Names and add hash codes to CDRID separator by delimiter 
                            //   use this list as DataSource for DataGrid control -LH
                            CTLookupList list = new CTLookupList(dbTable);
                            results.DataSource = list;

                            results.Visible = true;
                            results.DataBind();
                        }
                        break;
                }

                if (results.Items.Count == 0)
                {
                    Caption = "No records matched your search criteria.";
                }
            }
            else
            {
                Caption = "";
            }

        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
