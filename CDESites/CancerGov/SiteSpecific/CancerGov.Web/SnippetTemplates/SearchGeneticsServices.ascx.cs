using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using CancerGov.Common;
using CancerGov.Common.ErrorHandling;
using CancerGov.CDR.DataManager;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.UI.WebControls.JSLibraries;   // In order to reference Prototype.
namespace CancerGov.Web.SnippetTemplates
{
    public partial class SearchGeneticsServices : SearchBaseUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (PageAssemblyContext.Current.DisplayVersion == DisplayVersions.Web)
            {
                this.textClear.Visible = false;
                this.textSubmit.Visible = false;
            }
            else
            {
                this.clearBtn.Visible = false;
                this.searchBtn.Visible = false;
            }
            GeneticProfessional gp = new GeneticProfessional();
            DataSet dbSet;
            dbSet = gp.GetSearchFormMasterData();

            selState.DataSource = dbSet.Tables["States"];
            selState.DataTextField = "Name";
            selState.DataValueField = "StateAbbr";
            selState.DataBind();
            selState.Items.Insert(0, new System.Web.UI.WebControls.ListItem("all states", "all states;default"));
            Functions.EncodeHtmlSelectValues(selState, e);

            selCancerFamily.DataSource = dbSet.Tables["CancerFamily"];
            selCancerFamily.DataTextField = "FamilyCancerSyndrome";
            selCancerFamily.DataValueField = "Value";
            selCancerFamily.DataBind();
            selCancerFamily.Items.Insert(0, new System.Web.UI.WebControls.ListItem("all", "all;default"));
            Functions.EncodeHtmlSelectValues(selCancerFamily, e);

            selCancerType.DataSource = dbSet.Tables["CancerType"];
            selCancerType.DataTextField = "CancerTypeSite";
            selCancerType.DataValueField = "Type";
            selCancerType.DataBind();
            selCancerType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("all types", "all types;default"));
            Functions.EncodeHtmlSelectValues(selCancerType, e);

            selCountry.DataSource = dbSet.Tables["Country"];
            selCountry.DataTextField = "Country";
            selCountry.DataValueField = "Country";
            selCountry.DataBind();
            selCountry.Items.Insert(0, new System.Web.UI.WebControls.ListItem("all countries", "all countries;default"));
            Functions.EncodeHtmlSelectValues(selCountry, e);

            if (dbSet != null)
            {
                dbSet.Dispose();
            }

            if (WebAnalyticsOptions.IsEnabled)
            {

                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.PageName, wbField =>
                {
                    wbField.Value = ConfigurationSettings.AppSettings["HostName"] + SearchPageInfo.SearchResultsPrettyUrl;
                });

            }
            else
            {
                litOnSubmitHandlerAdder.Visible = false;
            }

            NCI.Web.UI.WebControls.JSManager.AddExternalScript(this.Page, "/scripts/popEvents.js");
            PrototypeManager.Load(this.Page);
        }
    }
}