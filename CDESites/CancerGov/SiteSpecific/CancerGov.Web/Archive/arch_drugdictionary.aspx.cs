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

using CancerGov.Text;

namespace www.Archive
{
    public partial class arch_drugdictionary : System.Web.UI.Page
    {

        private string conString = "";  // CDR database connection string        
        private SqlConnection conn;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            // get database connection string
            conString = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;

            string selection = Strings.Clean(Request["expand"]);
            if (selection != null)
            {
                BindDrugList(selection);
            }
        }

        private void BindDrugList(string selection)
        {
            if (selection == null)
                throw new ArgumentNullException(selection);

            // Don't allow wildcards or other escapes.
            if (selection.Length != 1 || selection == "%")
                throw new ArgumentException("Illegal filter value", "selection");

            string beginWith = "";

            // connect to database and get all dictionary words
            conn = new SqlConnection(conString);
            conn.Open();
            
            
            if (selection == "#")
                beginWith = "[0-9]";
            else
                beginWith = selection.ToString();

            SqlCommand cmd = new SqlCommand("[usp_TermDD_GetMatchingTerms]", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Match", SqlDbType.NVarChar, 10);
            cmd.Parameters["@Match"].Value = beginWith;

            cmd.Parameters.Add("@PageSize", SqlDbType.Int, 10);
            cmd.Parameters["@PageSize"].Value = int.MaxValue;

            cmd.Parameters.Add("@CurPage", SqlDbType.Int, 10);
            cmd.Parameters["@CurPage"].Value = 1;

            cmd.Parameters.Add("@OtherNamesP", SqlDbType.Char, 10);
            cmd.Parameters["@OtherNamesP"].Value = 'N';

            cmd.Parameters.Add("@NumMatches", SqlDbType.Int, 10);
            cmd.Parameters["@NumMatches"].Direction = ParameterDirection.Output;

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
