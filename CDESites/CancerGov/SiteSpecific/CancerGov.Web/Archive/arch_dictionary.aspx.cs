using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace www.Archive
{
	/// <summary>
	/// Summary description for arch_dictionary.
	/// </summary>
	public partial class arch_dictionary : System.Web.UI.Page
	{


        private string conString = ""; // CDR database connection string
        private SqlConnection conn;
		private char alpha = '#';
       

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// get database connection string
			conString = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;
			if (Request.Params["Alpha"] != null)
				alpha = Request.Params["Alpha"][0];

			// generate alphabet list
			if (!IsPostBack)
				BindAlphaList();

			// Show word list for selected alphabet
			BindWordList();
		}

		private void BindAlphaList()
		{
			string builtStr = "";	// to store alpha list

			// connect to database and get all dictionary words
            conn = new SqlConnection(conString);
            conn.Open();

			SqlCommand cmd = new SqlCommand("usp_GetAllGlossaryTerms", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataReader rows = cmd.ExecuteReader();
			char cur_letter = '#';
			char new_letter = '#';

			try 
			{
				while (rows.Read())
				{
					new_letter = rows["TermName"].ToString().ToLower()[0];

					// If new letter is gotten, write down link for current letter
					if (new_letter != cur_letter && (! Regex.IsMatch(new_letter.ToString(), "[0-9]", RegexOptions.Compiled)))
					{
						builtStr += "&nbsp;<a href=\"arch_dictionary.aspx?Alpha=" + Server.UrlEncode(cur_letter.ToString()) + "\">" + cur_letter + "</a>&nbsp;";
						cur_letter = new_letter;
					}
				}
			} 
			finally 
			{
				rows.Close();
				conn.Close();
			}

			// write down link for the last new letter
			if (new_letter != '#' && (! Regex.IsMatch(new_letter.ToString(), "[0-9]", RegexOptions.Compiled)))
			{
				builtStr += "&nbsp;<a href=\"arch_dictionary.aspx?Alpha=" + new_letter + "\">" + new_letter + "</a>";
			}

			// Display alpha links
			alphaList.Text = builtStr;
		}

		private void BindWordList()
		{
			string beginWith = "";

			// connect to database and get all dictionary words
            conn = new SqlConnection(conString);
            conn.Open();

			if (alpha == '#')
				beginWith = "[0-9]";
			else
				beginWith = alpha.ToString();

			SqlCommand cmd = new SqlCommand("usp_GetGlossaryTermList", conn);
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add("@BeginWith", SqlDbType.VarChar, 10);
			cmd.Parameters["@BeginWith"].Value = beginWith;

			SqlDataAdapter da = new SqlDataAdapter(cmd);
			DataSet ds = new DataSet();
			da.Fill(ds);
			da.Dispose();
            conn.Close();
			
			wordList.DataSource = ds.Tables[0];
			wordList.DataBind();
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
