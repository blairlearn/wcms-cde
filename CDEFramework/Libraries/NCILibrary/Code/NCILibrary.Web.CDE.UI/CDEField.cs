using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using NCI.Web.CDE.UI.Configuration;
using NCI.Logging;

namespace NCI.Web.CDE.UI
{
    /// <summary>
    /// This is an enumeration that defines the scope of the field we are looking for
    /// (Page or Snippet level)
    /// </summary>
    public enum CDEFieldScope
    {
        /// <summary>
        /// If user selects page, CDEFieldScope is set to the Page value and knows
        /// to look at Page level for the desired field
        /// </summary>
        Page,
        /// <summary>
        /// If user selects snippet, CDEFieldScope is set to the Snippet value and
        /// knows to look at Snippet level for the desired field
        /// </summary>
        Snippet
    } //Stores where the field we're looking for is defined (page or snipped level)
    

    
    public class CDEField : WebControl
    {

        private CDEFieldScope _scope = CDEFieldScope.Page;

        /// <summary>
        /// Automated getting and setting of CDEFieldScope
        /// </summary>
        public CDEFieldScope Scope 
        {
            get { return _scope; }
            set {this._scope = value; }
        }

        /// <summary>
        /// Automated getting and setting of FieldName
        /// </summary>
        public string FieldName { get; set; }
        
        /// <summary>
        /// Renders contents for DesiredField: the field the user wishes to find and use. If the current
        /// scope is Page level, the field is found using the current page assembly. If the current scope
        /// is snippet level, the parent SnippetControl is found, and the field is returned from there.
        /// </summary>
        /// <param name="writer"></param>
       
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (String.IsNullOrEmpty(this.FieldName))
                throw new ArgumentException("Property FieldName cannot be null or empty.");

            string DesiredField = null;

            if (this.Scope == CDEFieldScope.Page)
                    DesiredField = PageAssemblyContext.Current.PageAssemblyInstruction.GetField(this.FieldName);

            if (this.Scope == CDEFieldScope.Snippet)
            {
                Control parent = this.Parent;
                while (parent != null)
                {
                    if (parent is SnippetControl)
                    {
                        DesiredField = ((SnippetControl)parent).GetField(this.FieldName);
                        break;
                    }
                    else
                    {
                        parent = parent.Parent;
                    }
                }
            }
            
            if (DesiredField != null)
            {
                writer.Write(DesiredField);
            }
            else
            {
                writer.Write("");
            }
        }
    }
}
