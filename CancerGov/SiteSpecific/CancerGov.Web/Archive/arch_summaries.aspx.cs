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
using CancerGov.Text;

namespace www.Archive
{
	/// <summary>
	/// Summary description for arch_summaries.
	/// </summary>
	public partial class arch_summaries : System.Web.UI.Page
	{
		private string cdrDbConn;
		private string dbConn;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            cdrDbConn = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;
            dbConn = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

			// Get summary list
			BindSummaries();
		}

		private void BindSummaries()
		{
			// Get summary IDs from CDR database
			SqlConnection cdrDbh = new SqlConnection(cdrDbConn);
			SqlCommand cmd = new SqlCommand("usp_GetActiveSummaries", cdrDbh);
			SqlDataAdapter sumDa = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();
			sumDa.Fill(ds);

			// Create a temp table to show summary list
			DataTable sumTable = new DataTable();
			sumTable.Columns.Add(new DataColumn("SummaryId", System.Type.GetType("System.String")));
			sumTable.Columns.Add(new DataColumn("Type", System.Type.GetType("System.String")));
			sumTable.Columns.Add(new DataColumn("Audience", System.Type.GetType("System.String")));
			sumTable.Columns.Add(new DataColumn("Title", System.Type.GetType("System.String")));
			sumTable.Columns.Add(new DataColumn("Language", System.Type.GetType("System.String")));
            sumTable.Columns.Add(new DataColumn("PrettyURL", System.Type.GetType("System.String")));

			string docid, type, audience, title, language,prettyurl;
			SqlConnection dbh = new SqlConnection(dbConn);
			dbh.Open();

            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    docid = row["SummaryID"].ToString();
                    type = row["Type"].ToString();
                    audience = row["Audience"].ToString();
                    title = row["Title"].ToString();
                    language = row["Language"].ToString();
                    prettyurl = row["PrettyURL"].ToString();


                   

                                      
                    // insert the link to result table
                    DataRow dr = sumTable.NewRow();
                    dr["SummaryId"] = docid;
                    dr["Type"] = type;
                    dr["Audience"] = audience;
                    dr["Title"] = title;
                    dr["Language"] = language;
                    if (!string.IsNullOrEmpty(prettyurl))
                    {
                        Uri siteUri = new Uri(prettyurl);
                        dr["PrettyUrl"] = siteUri.LocalPath;
                    }
                    sumTable.Rows.Add(dr);
                }
            }

            
			finally 
			{
				sumDa.Dispose();
				cdrDbh.Close();
				dbh.Close();
			}

			// Bind datagrid to the result table
			summaryList.DataSource = sumTable;
			summaryList.DataBind();
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
