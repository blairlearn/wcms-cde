using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

using CancerGov.CDR.ClinicalTrials.Helpers;
using CancerGov.CDR.ClinicalTrials.Search;
using CancerGov.UI;

namespace NCI.Web.UI.WebControls.FormControls
{
    /// <summary>
    /// Renders Clinical Trial search criteria as HTML.
    /// </summary>
    [DefaultProperty("Criteria")]
    [ToolboxData("<{0}:CTSearchCriteriaDisplay runat=server></{0}:CTSearchCriteriaDisplay>")]
    public class CTSearchCriteriaDisplay : WebControl, IRenderer
    {
        #region Public properties.

        /// <summary>
        /// Gets and Sets the DisplayCriteria which to be rendered.
        /// </summary>
        [Bindable(false)]
        [Category("Data")]
        public DisplayCriteria Criteria
        {
            get
            {
                DisplayCriteria criteria = null;

                if (ViewState["DisplayCriteria"] != null)
                {
                    XmlSerializer serial = new XmlSerializer(typeof(DisplayCriteria));
                    TextReader reader = new StringReader((string)ViewState["DisplayCriteria"]);
                    criteria = (DisplayCriteria)serial.Deserialize(reader);
                }

                return criteria;
            }

            set
            {
                XmlSerializer serial = new XmlSerializer(typeof(DisplayCriteria));
                TextWriter writer = new StringWriter();
                serial.Serialize(writer, value);
                ViewState["DisplayCriteria"] = writer.ToString();
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Override the control's default outermost tag.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion

        protected override void RenderContents(HtmlTextWriter writer)
        {
            DisplayCriteria criteria = this.Criteria;

            if (criteria != null)
            {
                RenderCriterion(writer, "Cancer Type/Condition", criteria.CancerTypeNames);
                RenderCriterion(writer, "Stage/Subtype", criteria.CancerSubtypeNames);
                RenderLocation(writer, criteria);
                RenderCriterion(writer, "Trial Type", criteria.TrialTypeList);
                RenderDrugList(writer, criteria);
                RenderCriterion(writer, "Treatment/Intervention", criteria.InterventionList);
                RenderCriterion(writer, "Keywords/Phrases", criteria.Keywords);
                RenderCriterion(writer, "Trial Phase", criteria.TrialPhase);
                if (criteria.RestrictToRecent)
                    RenderCriterion(writer, "Added in last 30 days?", "Yes");
                RenderCriterion(writer, "Protocol ID", criteria.SpecificProtocolIDList);
                RenderCriterion(writer, "Trial Investigators", criteria.InvestigatorList);
                RenderCriterion(writer, "Lead Organization/Cooperative Group", criteria.LeadOrganizationList);
                RenderCriterion(writer, "Special Category", criteria.SpecialCategoryList);
            }
        }

        private void RenderLocation(HtmlTextWriter writer, DisplayCriteria criteria)
        {
            switch (criteria.LocationSearchType)
            {
                case LocationSearchType.Zip:
                    RenderCriterion(writer, "Near ZIP Code",
                        string.Format("within {0} miles of {1:D5}", criteria.LocationZipProximity,  int.Parse(criteria.LocationZipCode)));
                    break;
                case LocationSearchType.Institution:
                    RenderCriterion(writer, "At Hospital/Institution", criteria.LocationInstitutions);
                    break;
                case LocationSearchType.City:
                    if (!string.IsNullOrEmpty(criteria.LocationCity))
                        RenderCriterion(writer, "City", criteria.LocationCity);

                    // If Country is NOT U.S.A., and there is exactly one "state" name, then
                    // combine the values to appear as they do on the UI page.
                    if (!string.IsNullOrEmpty(criteria.LocationCountry) &&
                        criteria.LocationCountry.ToUpper() != "U.S.A.")
                    {
                        string country = criteria.LocationCountry;
                        if (criteria.LocationStateNameList != null &&
                            criteria.LocationStateNameList.Count == 1)
                        {
                            country += " - " + criteria.LocationStateNameList[0];
                        }
                        RenderCriterion(writer, "Country", country);
                    }
                    else
                    {
                        RenderCriterion(writer, "State", criteria.LocationStateNameList);
                        if (!string.IsNullOrEmpty(criteria.LocationCountry))
                            RenderCriterion(writer, "Country", criteria.LocationCountry);
                    }
                    break;
                case LocationSearchType.NIH:
                    RenderCriterion(writer, "At NIH", "Only show trials at the NIH Clinical Center (Bethesda, Md.)");
                    break;
                default:
                    break;
            }
        }

        private void RenderDrugList(HtmlTextWriter writer, DisplayCriteria criteria)
        {
            if (criteria.DrugList != null && criteria.DrugList.Count > 0)
            {
                string drugFormula = criteria.RequireAllDrugsMatch ? "All drugs shown" : "Any drugs shown";

                RenderCriterion(writer, "Drug", criteria.DrugList);
                RenderCriterion(writer, "Find trials that include", drugFormula);
            }
        }

        /// <summary>
        /// Helper method to render a single criterion.  Overloaded.
        /// </summary>
        /// <param name="writer">HTML stream for writing the criterion.</param>
        /// <param name="label">Label portion of the output item.</param>
        /// <param name="value">The value to be displayed.</param>
        private void RenderCriterion(HtmlTextWriter writer, string label, string value)
        {
            if (!string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(value))
            {
                //writer.AddAttribute(HtmlTextWriterAttribute.Class, "item-label");
                //change this to a strong tag for NVCG
                writer.RenderBeginTag(HtmlTextWriterTag.Strong);
                writer.Write("{0}:", label);    // Label comes from within this class.  We can trust it.
                writer.RenderEndTag();

                writer.Write("&nbsp;&nbsp;");
                writer.WriteEncodedText(value); // The value comes from the user.  Don't trust it.

                writer.WriteBreak();
            }
        }

        /// <summary>
        /// Helper method to render a single criterion.  Overloaded.
        /// </summary>
        /// <param name="writer">HTML stream for writing the criterion.</param>
        /// <param name="label">Label portion of the output item.</param>
        /// <param name="list">A list of string values to display.</param>
        private void RenderCriterion(HtmlTextWriter writer, string label, NameList list)
        {
            if (list != null && list.Count > 0)
            {
                RenderCriterion(writer, label, list.ToString());
            }
        }

        /// <summary>
        /// Helper method to render a single criterion.  Overloaded.
        /// </summary>
        /// <param name="writer">HTML stream for writing the criterion.</param>
        /// <param name="label">Label portion of the output item.</param>
        /// <param name="list">Collection of names to display, embedded
        /// in a collection of (name,value) pairs.</param>
        private void RenderCriterion(HtmlTextWriter writer, string label, IList<KeyValuePair<string, int>> list)
        {
            if (list != null)
            {
                NameList nameList = new NameList(list);
                RenderCriterion(writer, label, nameList);
            }
        }

        #region IRenderer Members

        /// <summary>
        /// Implementation of the IRenderer Render() method.  Renders the control as a string.
        /// </summary>
        /// <returns>A string containing an HTML representation of the search criteria.</returns>
        public string Render()
        {
            string renderedContent;

            using (StringWriter stringWriter = new StringWriter())
            {
                HtmlTextWriter writer = new HtmlTextWriter(stringWriter);

                RenderControl(writer);

                writer.Flush();
                writer.Close();
                writer.Dispose();

                renderedContent = stringWriter.ToString();
            }

            return renderedContent;
        }

        #endregion
    }
}
