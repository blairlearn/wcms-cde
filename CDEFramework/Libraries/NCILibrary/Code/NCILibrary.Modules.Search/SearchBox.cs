using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using System.Configuration;

namespace NCI.Web.CDE.UI.Modules
{
    [DefaultProperty("Title")]
    [ToolboxData("<{0}:SearchControl runat=server></{0}:SearchControl>")]
    public class SearchBox : SnippetControl
    {
        #region Public

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Title
        {
            get
            {
                String s = (String)ViewState["Title"];
                return ((s == null) ? "[" + this.ID + "]" : s);
            }

            set
            {
                ViewState["Title"] = value;
            }
        }

        [DefaultValue(false)]
        public string SearchType
        {
            get
            {
                if (ViewState["SearchType"] == null)
                    ViewState["SearchType"] = null;
                return (string)ViewState["SearchType"];
            }
            set
            {
                ViewState["SearchType"] = value;
            }
        }

        /// <summary>
        /// The url that will be set for the action attribute of the html form. The data is posted
        /// back to this url when go button is clicked.
        /// </summary>
        public string ActionURL
        {
            get
            {
                return (String)ViewState["ActionURL"];
            }

            set
            {
                ViewState["ActionURL"] = value;
            }
        }

        /// <summary>
        /// The client side Web Analytics function for OnSubmit event.
        /// </summary>
        public string WebAnalyticsFunction
        {
            get
            {
                return (String)ViewState["wbFunction"];
            }

            set
            {
                ViewState["wbFunction"] = value;
            }
        }


        #endregion

        public void Page_Load(object sender, EventArgs e)
        {
            SearchBoxSettings searchBoxSettings = ModuleObjectFactory<SearchBoxSettings>.GetModuleObject(SnippetInfo.Data);
            if (searchBoxSettings != null)
            {
                this.Title = searchBoxSettings.Title;
                this.ActionURL = searchBoxSettings.ActionUrl;
                this.WebAnalyticsFunction = searchBoxSettings.WebAnalyticsFunction;
                this.SearchType = searchBoxSettings.SearchType;
            }
        }

        public override void RenderControl(HtmlTextWriter output)
        {

            if (!string.IsNullOrEmpty(WebAnalyticsFunction))
                WebAnalyticsFunction = string.Format("onsubmit=\"{0}\"", WebAnalyticsFunction);
            else
                WebAnalyticsFunction = "";


            output.Write(string.Format("<div class=\"leftnav-default\"><h3>{2}</h3><form {0} method=\"post\" name=\"NCSearchBox\" action=\"{1}\">", WebAnalyticsFunction, ActionURL,  Title));

            

            if (SearchType.ToLower() == "keyword_with_date")
            {

                output.Write(@"Search For:<label class=""hidden"" for=""keyword"">keyword</label>
                            <input id=""keyword"" class=""find-news-release-keyword"" name=""keyword"">
                            <table class=""find-news-release-dates""><tr><th colspan=""2"" scope=""col"">Between these dates:</th></tr>
                            <tr><td>
                            <label class=""hidden"" for=""startMonth"">select start Month</label>
                            <select id=""startMonth"" name=""startMonth""> <option selected value=""1"">Jan.</option> <option value=""2"">Feb.</option> <option value=""3"">Mar.</option> <option value=""4"">Apr.</option> <option value=""5"">May</option> <option value=""6"">Jun.</option> <option value=""7"">Jul.</option> <option value=""8"">Aug.</option> <option value=""9"">Sept.</option> <option value=""10"">Oct.</option> <option value=""11"">Nov.</option> <option value=""12"">Dec.</option></select>
                            </td><td>
                            <label class=""hidden"" for=""startYear"">select start Year</label>
                            <select id=""startYear"" name=""startYear"">{0}</select>
                            </td></tr>
                            <tr><td>
                            <label class=""hidden"" for=""endMonth"">select end Month</label>
                            <select id=""endMonth"" name=""endMonth""> {1} </select></td><td>
                            <label class=""hidden"" for=""endYear"">select end Year</label>
                            <select id=""endYear"" name=""endYear"">{2}</select>
                            </td></tr></table>", GetYearListItems("startYear"), GetMonthListItems("endMonth"), GetYearListItems("endYear"));

                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                {
                    output.Write(@"<input alt=""Search"" src=""/images/red_buscar_button.gif"" type=""image"" class=""find-news-release-image""></form></div>");
                }
                else
                {
                    output.Write(@"<input alt=""Search"" src=""/images/red_go_button.gif"" type=""image"" class=""find-news-release-image""></form></div>");
                }

            }
            else
            {
                if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                {
                    output.Write(@"<label for=""keyword"" class=""palabra-title"">Palabra clave</label>
					<input type=""text"" id=""keyword"" name=""keyword"" class=""search-go-spanish"" />
					<input type=""image"" src=""/images/buscar-left-nav.gif"" alt=""Buscar"" class=""search-go-spanish-image"" />
				</form></div>");

                }
                else
                {

                    output.Write(@"<label class=""hidden"" for=""keyword"">keyword</label>
                    <input id=""keyword"" class=""search-go-english"" name=""keyword""><input
                    alt=""Search"" src=""/images/red_go_button.gif"" type=""image"" class=""search-go-english-image""></form></div>");
                }

            }

        }

        private string GetMonthListItems(string monthType)
        {
            string html = String.Empty;
            string[] monthNameLookup = { "Jan.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "Feb.", "Mar.", "Apr.", "May", "Jun.", "Jul.", "Aug.", "Sept.", "Oct.", "Nov.", "Dec." };
            for (int i = 1; i <= 12; i++)
                html += string.Format("<option {0} value=\"{1}\">{2}</option>", i == DateTime.Now.Month ? "selected" : String.Empty, i, monthNameLookup[i - 1]);
            return html;
        }

        private string GetYearListItems(string yearType)
        {
            string html = String.Empty;
            int startYear = 1998;
            int yearsAgo = DateTime.Now.Year - startYear;
            while (startYear <= DateTime.Now.Year)
            {
                if ((string.Compare("startYear", yearType, true) == 0 && startYear == DateTime.Now.Year - yearsAgo) ||
                     (string.Compare("endYear", yearType, true) == 0 && startYear == DateTime.Now.Year))
                    html += string.Format("<option selected value=\"{0}\">{0}</option>", startYear.ToString());
                else
                    html += string.Format("<option value=\"{0}\">{0}</option>", startYear.ToString());
                startYear++;
            }

            return html;
        }
    }
}
