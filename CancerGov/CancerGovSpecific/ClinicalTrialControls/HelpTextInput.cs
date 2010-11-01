using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Web.UI.WebControls.JSLibraries;   // In order to reference Prototype.

/*
 * So, How does this thing work?
 * 
 * The HelpTextInput derives from TextBox and has all the functionality associated with a regular
 * TextBox control.
 * 
 * Functionally, when a HelpTextInput control is instantiated, in addition to creating a visible
 * UI element with the provided ID, an additional state field is also rendered (appending "state"
 * to the ID).  When the JavaScript component is initialized, the state field is set to either
 * "valid" or "invalid", depending on whether the input field has a non-helper text value.
 * 
 * During a postback, the state field has three possible values with corresponding meanings:
 * 
 *    Value      Meaning
 *  =========  ===========
 *   (null)     Javascript was not enabled on the client, any value in the text field must
 *              be considered to have been provided by the user.
 *              
 *   valid      The value contained in the input field was provided by the user.
 *   
 *  invalid     The input field's value is the helper text.  This text was not entered by
 *              the user and should be treated as an empty field.
*/
namespace NCI.Web.UI.WebControls.FormControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:HelpTextInput runat=server></{0}:HelpTextInput>")]
    public class HelpTextInput : TextBox
    {
        #region Fields

        private const string _stateFieldName = "state";

        private const string _validState = "valid";
        private const string _invalidState = "invalid";

        #endregion


        #region Properties

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string HelperText
        {
            get { return (string)ViewState["HelperText"]; }
            set { ViewState["HelperText"] = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string HelperTextColor
        {
            get { return (string)(ViewState["HelperTextColor"] ?? ""); }
            set { ViewState["HelperTextColor"] = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public override string Text
        {
            get
            {
                string text;
                string state = (string)ViewState[_stateFieldName];

                if (string.IsNullOrEmpty(state) || state.Equals(_validState))
                    text = base.Text;
                else
                    text = string.Empty;

                return text;
            }
            set
            {
                // If text is set externally, it *must* be valid.
                base.Text = value;
            }
        }

        #endregion

        protected override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            bool stateChanged;
            bool stateIsValid;

            string stateFieldID = BuildUniqueControlName(_stateFieldName);
            string state = postCollection[stateFieldID];

            // Blank state means no JS, the text field is meaningful.
            if (string.IsNullOrEmpty(state) || state.Equals(_validState))
                stateIsValid = true;
            else
                stateIsValid = false;

            // Is the state the same as what we last knew of?
            string oldState = (string)ViewState[_stateFieldName];
            if (state != oldState || !state.Equals(oldState))
            {
                stateChanged = true;
                ViewState[_stateFieldName] = state;

                if (!stateIsValid)
                {
                    Text = string.Empty;  // If the state isn't valid, clear the text.
                }
            }
            else
                stateChanged = false;

            bool textChanged = false;

            // If the state is valid, then process the text.
            // If the state is invalid, force the text to empty.
            string oldText = Text;
            if (stateIsValid)
            {
                string newText = postCollection[postDataKey];
                if (!ReadOnly && !oldText.Equals(newText))
                {
                    textChanged = true;
                    Text = newText;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    textChanged = true;
                    Text = string.Empty;
                }
            }

            // Flag the field as having changed if either the state has changed,
            // or if the underlaying value has changed and is valid.
            //(Invalid would mean that the base text contains the helper text, 
            // but the logic in the Text getter will override that, so we don't care
            // about changing to that value.)
            return stateChanged ||
                (stateIsValid && textChanged);
        }

        protected override void OnPreRender(EventArgs e)
        {
            /// Set up JavaScript resources. Order is important.  Because the control's script uses prototype, we need
            /// to register that one first.
            PrototypeManager.Load(this.Page);
            JSManager.AddResource(this.Page, typeof(HelpTextInput), "CancerGovUIControls.Resources.HelpTextInput.js");

            /// Register the control to be notified of postback events.
            Page.RegisterRequiresPostBack(this);

            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            // The state field is always rendered with no value.  If JavaScript
            // is present, the initializer will set a value.
            string stateFieldID = BuildUniqueControlName(_stateFieldName);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, stateFieldID);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, stateFieldID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            // Initialize JS code.
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.Write(string.Format("HelpTextInput.Create(\"{0}\", \"{1}\", \"{2}\", \"{3}\");",
                ClientID, stateFieldID, HelperText, HelperTextColor));
            writer.RenderEndTag();
        }

        #region Rendering Helper Methods

        /// <summary>
        /// Helper function to create a unique name for a UI item based on the
        /// unique Id of the overall control.
        /// </summary>
        /// <param name="controlId">String containing the name of the UI element.</param>
        /// <returns>String value containing a unique Identifier for the UI element.</returns>
        private string BuildUniqueControlName(string controlId)
        {
            return this.ClientID + this.ClientIDSeparator + controlId;
        }

        #endregion
    }
}
