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

using CancerGov.Text;

namespace CancerGov.Web
{
	/// <summary>
	/// Summary description for CTLookupSearch.
	/// </summary>
	public partial class CTLookupSearch : System.Web.UI.Page
	{
		private string title = "";
		private string inputKeyword;
		private string inputAlphaIndex;
		private string alphaIndexLinks;
		private string caption = "";
        private string textInputPrompt = string.Empty;

		#region Page Properties

		protected string Caption {
			get {return caption;}
			set {caption = value;}
		}

		public string Title {
			get {return title;}
			set {title = value;}
		}

		public string InputKeyword {
			get {return inputKeyword;}
			set {inputKeyword = value;}
		}

		public string InputAlphaIndex {
			get {return inputAlphaIndex;}
			set {inputAlphaIndex = value;}
		}

		public string AlphaIndexLinks {
			get {return alphaIndexLinks;}
			set {alphaIndexLinks = value;}
		}

        public string TextInputPrompt
        {
            get { return textInputPrompt; }
            set { textInputPrompt = value; }
        }

		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			title = Strings.IfNull(Strings.Clean(Request.Params["title"]), "");
			inputKeyword = Strings.IfNull(Strings.Clean(Request.Params["keyword"]), "");
			inputAlphaIndex = Strings.IfNull(Strings.Clean(Request.Params["alphaIndex"]), "");
			string fld = Strings.IfNull(Strings.Clean(Request.Params["fld"]), "");

			//Build AlphaIndexLinks
			
			switch(fld.Trim().ToLower()) {
				case "drug":
                    caption = "See a list of drugs to use in your clinical trials search by browsing the alphabetical list or by entering a drug name in the search box. You can select multiple drug names by checking the box next to each name and using the <b>Add Selected</b> button.";
                    textInputPrompt = "Enter Drug Name";
					break;
				case "institution":
                    caption = "See a list of hospitals and institutions that can be used in your clinical trials search by browsing the alphabetical list or by entering a hospital or institution name in the search box. You can select multiple hospitals and institutions by checking the box next to each name and using the <b>Add Selected</b> button.";
                    textInputPrompt = "Enter Hospital/Institution Name";
                    break;
				case "leadorg":
                    caption = "See a list of lead organizations and cooperative groups that can be used in your clinical trials search by browsing the alphabetical list or by entering a lead organization or cooperative group name in the search box. You can select multiple trial lead organizations and cooperative groups by checking the box next to each name and using the <b>Add Selected</b> button.";
                    textInputPrompt = "Enter Lead Organization/Cooperative Group Name";
                    break;
				case "investigator":
                    caption = "See a list of trial investigators that can be used in your clinical trials search by browsing the alphabetical list or by entering an investigator name in the search box. You can select multiple trial investigators by checking the box next to each name and using the <b>Add Selected</b> button.";
                    textInputPrompt = "Enter Trial Investigator Name";
                    break;
                case "intervention":
                    caption = "See a list of treatments and interventions that can be used in your clinical trials search by browsing the alphabetical list or by entering a treatment or intervention name in the search box. You can select multiple treatments and interventions by checking the box next to each name and using the <b>Add Selected</b> button.";
                    textInputPrompt = "Enter Treatment/Intervention Name";
                    break;
			}

            if ("[^a-zA-Z]" == inputAlphaIndex.Trim())
                {
                alphaIndexLinks += "<b>#</b>&nbsp;&nbsp;";
			}
			else {
                alphaIndexLinks += "<a href=\"javascript: document.forms[0].alphaIndex.value = '[^a-zA-Z]'; document.forms[0].keyword.value = ''; doSubmit();\">#</a>&nbsp;&nbsp;";
            }

			for(int i = Convert.ToInt16('A'); i <= Convert.ToInt16('Z'); i++) {
				if(((char)i).ToString() == inputAlphaIndex.Trim().ToUpper()) {
					alphaIndexLinks += "<b>" + (char)i + "</b>&nbsp;&nbsp;";
				}
				else {
					alphaIndexLinks += "<a href=\"javascript: document.forms[0].alphaIndex.value = '" + (char)i + "'; document.forms[0].keyword.value = ''; doSubmit();\">" + (char)i + "</a>&nbsp;&nbsp;";
				}
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
