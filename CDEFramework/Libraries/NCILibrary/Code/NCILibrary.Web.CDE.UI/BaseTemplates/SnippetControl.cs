using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.CDE.UI
{
    //TODO: Document
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:BaseSnippetTemplate runat=server></{0}:BaseSnippetTemplate>")]
    public abstract class SnippetControl : UserControl
    {
        #region Public Properties
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public SnippetInfo SnippetInfo { get; set; }
        public IPageAssemblyInstruction PageInstruction
        {
            get
            {
                return PageAssemblyContext.Current.PageAssemblyInstruction;
            }
        }

        /// <summary>
        /// Dictionary holds all registered Field Filter Delegates
        /// </summary>
        private Dictionary<string, FieldFilterDelegate> _FieldFilterDelegates = new Dictionary<string, FieldFilterDelegate>();

        /// <summary>
        /// Gets the Field Filter Delegates
        /// </summary>
        /// <value>The Field Filter Deledates</value>
        private Dictionary<string, FieldFilterDelegate> FieldFilterDelegates
        {
            get
            {
                return _FieldFilterDelegates;
            }
        }


        /// <summary>
        /// Gets the value of the field referenced by "String" with all field filters applied
        /// </summary>
        /// <param name="fieldName"></param>
        /// <exception cref="ArgumentException">Thrown when the fieldName field is null or empty.</exception>
        /// <returns></returns>
        public string GetField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                // Throws exception if fieldName is null/empty
                throw new ArgumentException("The field name may not be null or empty.");

            string rtnValue = string.Empty;

            // Initialize delegate
            FieldFilterDelegate del = FieldFilterDelegates[fieldName.ToLower()];

            if (del != null)
            {
                // Initialize FieldFilterData to empty
                FieldFilterData data = new FieldFilterData();
                // Call delegate, to modify Field Data string of FieldFilterData object being passed in
                del(fieldName, data);
                // Set return value to processed value of FieldFilterData 
                rtnValue = data.Value;
            }
            return rtnValue;
        }

        /// <summary>
        /// Adds a field filter which modifies the value of the field referenced by "string" when
        /// GetField is called.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="filter"></param>
        /// Changed from public to private PageAssembly Instruction
        protected void AddFieldFilter(string fieldName, FieldFilterDelegate filter)
        {
            if (string.IsNullOrEmpty(fieldName))
                // Throws exception if fieldName is null/empty
                throw new ArgumentException("The fieldName parameter may not be null or empty.");

            string fieldNameKey = fieldName.ToLower();

            // If the delegates do not contain the passed filter key, add a new delegate
            if (FieldFilterDelegates.ContainsKey(fieldNameKey) == false)
            {
                FieldFilterDelegates.Add(fieldNameKey, filter);
            }
            // Add filter to dictionary associated with fieldNameKey
            // Because Delegates are immutable, an intermediate value cannot be used
            else
            {
                FieldFilterDelegates[fieldNameKey] += filter;
            }
        }
        
        #endregion
    }
}
