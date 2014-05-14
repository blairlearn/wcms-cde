using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CancerGov.CDR.DataManager;
using NCI.Web.CDE;
using NCI.Web.CancerGov.Apps;

namespace www.Search.UserControls
{
    public partial class CTCustomSelection : AppsBaseUserControl
    {
        private static readonly object SelectionsChangedEvent = new object();

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler SelectionsChanged
        {
            add { this.Events.AddHandler(SelectionsChangedEvent, value); }
            remove { this.Events.RemoveHandler(SelectionsChangedEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectionsChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[SelectionsChangedEvent];
            if (handler != null)
                handler(this, e);
        }


        private List<ProtocolSectionTypes> _selectedIDs = new List<ProtocolSectionTypes>();

        public ProtocolSectionTypes[] SelectedIDs
        {
            get
            {
                // Allocate space for the selected sections plus the five "always on" sections.
                ProtocolSectionTypes[] selections;
                List<ProtocolSectionTypes> idList = (List<ProtocolSectionTypes>)ViewState["SelectedIDs"];
                int size = 5;
                if (idList != null)
                    size += idList.Count;

                selections = new ProtocolSectionTypes[size];

                // These sections are always included.
                // "91,95,19,93,"
                selections[0] = ProtocolSectionTypes.Title;
                selections[1] = ProtocolSectionTypes.AlternateTitle;
                selections[2] = ProtocolSectionTypes.InfoBox;
                selections[3] = ProtocolSectionTypes.SpecialCategory;

                // Append the selected sections (if any).
                if (idList != null)
                    idList.CopyTo(selections, 4);

                // This section is always included.
                // ",8"
                selections[size - 1] = ProtocolSectionTypes.HPDisclaimer;

                return selections;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PageDisplayInformation.Version != DisplayVersions.Text)
            {
                this.imgBtnSelect.Visible = true;
                this.btnSelect.Visible = false;
            }
            else
            {
                this.imgBtnSelect.Visible = false;
                this.btnSelect.Visible = true;
            }
        }

        private void GetSelection()
        {
            _selectedIDs.Clear();


            if (this.customSections1.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.Objectives);
                _selectedIDs.Add(ProtocolSectionTypes.CTGovBriefSummary);
                //_selectedIDs.Add("2,12");
            }
            if (this.customSections2.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.EntryCriteria);
                _selectedIDs.Add(ProtocolSectionTypes.CTGovEntryCriteria);
                //_selectedIDs.Add("3,17");
            }
            if (this.customSections3.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.ExpectedEnrollment);
                //_selectedIDs.Add("4");
            }
            if (this.customSections4.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.Outcomes);
                //_selectedIDs.Add("22");
            }
            if (this.customSections5.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.Outline);
                _selectedIDs.Add(ProtocolSectionTypes.CTGovDetailedDescription);
                //_selectedIDs.Add("5,16");
            }
            if (this.customSections6.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.PublishedResults);
                //_selectedIDs.Add("6");
            }
            if (this.customSections7.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.RelatedPublications);
                //_selectedIDs.Add("23");
            }
            if (this.customSections8.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.LeadOrgs);
                _selectedIDs.Add(ProtocolSectionTypes.CTGovLeadOrgs);
                //_selectedIDs.Add("9,14");
            }
            if (this.customSections9.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.StudySites);
                //_selectedIDs.Add("99");
            }
            if (this.customSections10.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.Terms);
                _selectedIDs.Add(ProtocolSectionTypes.CTGovTerms);
                //_selectedIDs.Add("7,18");
            }
            if (this.customSections11.Checked)
            {
                _selectedIDs.Add(ProtocolSectionTypes.RegistryInformation);
                //_selectedIDs.Add("24");
            }

            ViewState["SelectedIDs"] = _selectedIDs;
        }

        protected void imgBtnSelect_Click(object sender, ImageClickEventArgs e)
        {
            GetSelection();
            OnSelectionsChanged(EventArgs.Empty);
        }

        public void btnSelect_Click(object sender, EventArgs e)
        {
            GetSelection();
            OnSelectionsChanged(EventArgs.Empty);
        }
    }
}