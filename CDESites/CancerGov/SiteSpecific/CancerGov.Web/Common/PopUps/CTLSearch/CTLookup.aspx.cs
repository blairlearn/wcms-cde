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

namespace CancerGov.Web
{
	/// <summary>
    /// Summary description for Redesign.
	/// </summary>
    public partial class CTLookup : CTLookupBase
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            ValidateInputData(Request.Params["fld"]);
            ValidateInputData(Request.Params["type"]);
            ValidateInputData(Request.Params["title"]);

            ValidateValidValues(Request.Params["fld"], new ArrayList { "institution", "drug", "intervention", "investigator", "leadorg" });
            ValidateValidValues(Title, new ArrayList { "find+hospitals/institutions", 
                "find+drug", "treatment/intervention", "find+trial+investigators", "find+lead+organizations" });

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
