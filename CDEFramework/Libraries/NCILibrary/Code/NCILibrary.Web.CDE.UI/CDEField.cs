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
        /// <summary>
        /// Automated getting and setting of CDEFieldScope
        /// </summary>
        public CDEFieldScope Scope { get; set; }

        /// <summary>
        /// Automated getting and setting of FieldName
        /// </summary>
        public string FieldName { get; set; }
        
        /// <summary>
        /// Renders contents for 
        /// </summary>
        /// <param name="writer"></param>
       
        protected override void RenderContents(HtmlTextWriter writer)
        {
            string DesiredField = null;

            try
            {
                if (this.Scope == CDEFieldScope.Page)
                    DesiredField = PageAssemblyContext.Current.PageAssemblyInstruction.GetField(this.FieldName);
            }
            catch (ArgumentException e)
            {
            }

            try
            {
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
            }
            catch (ArgumentException e)
            {
            }
            

            try
            {
            if (DesiredField != null)
            {
                writer.Write(DesiredField);
            }
            else
            {
                writer.Write("");
            }
            }
            catch
            {
            }

        }
    }

}
