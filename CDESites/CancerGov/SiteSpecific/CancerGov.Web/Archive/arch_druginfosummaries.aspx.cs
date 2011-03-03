using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace www.Archive
{
    public partial class arch_druginfosummaries : System.Web.UI.Page
    {
        private string conString = "";// CDR database connection string
        private SqlConnection conn;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            conString = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;
            BindDrugList();
        }

        private void BindDrugList()
        {
            // connect to database and get all dictionary words
            conn = new SqlConnection(conString);
            conn.Open();
            
            SqlCommand cmd = new SqlCommand("[usp_GetDrugInfo_List]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            cmd.Dispose();
            da.Dispose();
            conn.Close();
           
            drugDictionaryList.DataSource = ds;
            drugDictionaryList.DataBind();
        }
    }
}
