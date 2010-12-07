using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace www.Archive
{
	/// <summary>
	/// Summary description for pdqProtocols.
	/// </summary>
	public partial class arch_protocols: System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.HyperLink lnkProtocol;
		protected System.Web.UI.WebControls.HyperLink lnkSummary;
		protected System.Web.UI.WebControls.HyperLink lnkDictionary;
		private string cdrDbConn;	// for cdr database
       	private string dbConn;	// for cancer gov database
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
            cdrDbConn = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;
            dbConn = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

			// Get protocol list
			BindProtocols();
		}

		private void BindProtocols()
        {
            #region //Sql not used 
            /*			string sql = "";

			sql = "SELECT P.ProtocolID, P.CurrentStatus, ISNULL(P.PatientTitle, P.HealthProfessionalTitle) As PatientTitle," +
					"P.HealthProfessionalTitle " +
				   "FROM Protocol P Inner Join Document D on P.ProtocolID = D.DocumentID " +
				   "WHERE D.IsActive = 'Y' ORDER BY P.ProtocolID";
*/
            #endregion
            SqlConnection cdrDbh = new SqlConnection(cdrDbConn);
			SqlCommand cmd = new SqlCommand("usp_GetActiveProtocols", cdrDbh);
			SqlDataAdapter proDa = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();
			proDa.Fill(ds);

			try 
			{
				protocolList.DataSource = ds;
				protocolList.DataBind();
			} 
			finally 
			{
				proDa.Dispose();
				cdrDbh.Close();
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
