using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.HttpHeaders;
using NCI.Util;
using NCI.Logging;
using NCI.Web.Extensions;
using NCI.Web.CDE;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.Modules;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public class SubLayoutControl : SnippetControl
    {
        #region Private Members
        /// <summary>
        /// Loads a collection of all template slots that are declaratively 
        /// defined  in the page template. The slots that are specified as being blocked
        /// in the assembly instructions are not processed.
        /// </summary>
        private void loadTemplateSlots()
        {
            foreach (TemplateSlot slot in this.FindControlByType<TemplateSlot>())
            {
                //Add to slot collection only if it is not marked as being blocked.
                TemplateSlots.Add(slot);
            }
        }

        /// <summary>
        /// Adds the snippet info from the PageAssembly instruction for each 
        /// page template slot. Use the ID of the template slot control to match 
        /// the snippet info slotname property.
        /// </summary>
        private void loadSnippetsIntoTemplateSlots()
        {
            if (TemplateSlots.Count > 0)
            {
                List<SnippetControl> supportingSnippets = new List<SnippetControl>();
                foreach (SnippetInfo snippet in this.SnippetInfo.Snippets)
                {
                    if (!String.IsNullOrEmpty(snippet.SlotName))
                    {
                        string slotName = snippet.SlotName.Trim();
                        if (TemplateSlots.ContainsSlotName(slotName))
                        {
                            try
                            {
                                SnippetControl snippetControl = (SnippetControl)Page.LoadControl(snippet.SnippetTemplatePath.Trim());

                                // Note this has to come before adding the template control to the control tree.  
                                // This way, we can be sure any event in the control lifecycle like OnInit() 
                                // already has the snippet info data.
                                snippetControl.SnippetInfo = snippet;

                                TemplateSlots[slotName].AddSnippet(snippetControl);

                                // Some snippetcontrol implement the ISupportingSnippet interface. 
                                // The controls which implement ISupportingSnippet will provide additional 
                                // snippet control that are required for the complete functionality.
                                // Add them to a collection that will be processed later.
                                if (snippetControl is ISupportingSnippet)
                                {
                                    SnippetControl[] supportingcontrols = ((ISupportingSnippet)snippetControl).GetSupportingSnippets();
                                    if (supportingcontrols != null)
                                        supportingSnippets.AddRange(supportingcontrols);
                                }
                            }
                            catch (Exception ex)
                            {
                                //Failed to load the slot template control. Log this error.
                                Logger.LogError("CDE:SubLayoutControl.cs:SubLayoutControl", "Failed to load snippet control-" + snippet.SnippetTemplatePath, NCIErrorLevel.Error, ex);
                            }
                        }
                    }
                }

                foreach (SnippetControl supportSnippet in supportingSnippets)
                {
                    try
                    {
                        if (TemplateSlots.ContainsSlotName(supportSnippet.SnippetInfo.SlotName))
                            TemplateSlots[supportSnippet.SnippetInfo.SlotName].AddSnippet(supportSnippet);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("CDE:SubLayoutControl.cs:SubLayoutControl", "Failed to add supporting snippet control", NCIErrorLevel.Error, ex);
                    }
                }

            }
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Constructor. Initializes the object state. 
        /// </summary>
        public SubLayoutControl()
            : base()
        {
            TemplateSlots = new TemplateSlotCollection();
        }

        /// <summary>
        /// Gets the collection of Template Slots used by this page assembler.
        /// </summary>
        /// <value></value>
        /// <remarks>Since controls and apps might need to shove stuff into slots,
        /// this information should be available to controls.
        /// </remarks>
        public TemplateSlotCollection TemplateSlots { get; private set; }
        public CDEField CdeField { get; private set; }
        private SublayoutInfo _subLayoutInfo;
        protected string Title
        {
            get
            {
                if (_subLayoutInfo == null || _subLayoutInfo.Title == null)
                {
                    return string.Empty;
                }
                else
                {
                    return _subLayoutInfo.Title;
                }
            }
        }
              
        #endregion

        #region Protected Members
        /// <summary>
        /// This property returns the current page header control. 
        /// Use this in lieu of the Page.Header property when the Page.Header property is null. 
        /// </summary>
        protected HtmlHead CurrentPageHead
        {
            get
            {
                // Find the html head control on the template page
                HtmlHead currentPageHead = null;
                foreach (HtmlHead head in this.FindControlByType<HtmlHead>())
                {
                    currentPageHead = head;
                    break;
                }
                return currentPageHead;
            }
        }

        /// <summary>
        /// Sets the title of the page. Uses the GetField method of the Page instructions interface
        /// to get  the value.
        /// </summary>
        protected virtual void SetTitle()
        {
            //string title = PageInstruction.GetField(CdeField.FieldName);
            //PageInstruction.GetField("CDEField");
            if (CurrentPageHead != null)
            {
                string title = this.GetField(CdeField.FieldName);
                CurrentPageHead.Title = title;
            }
        }
        #endregion

        #region Page Overrides
        /// <summary>
        /// Identify the Templates on the Page. For each template add the Snippet info objects.
        /// </summary>
        /// <param name="e">Not Used</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            loadTemplateSlots();
            loadSnippetsIntoTemplateSlots();

            string snippetXmlData = this.SnippetInfo.Data;
            snippetXmlData = snippetXmlData.Replace("]]ENDCDATA", "]]>");
            _subLayoutInfo = ModuleObjectFactory<SublayoutInfo>.GetModuleObject(snippetXmlData);
            this.AddFieldFilter("sublayout_title", (name, data) =>
            {
                data.Value = this.Title;
            });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        #endregion
        
    }
}
