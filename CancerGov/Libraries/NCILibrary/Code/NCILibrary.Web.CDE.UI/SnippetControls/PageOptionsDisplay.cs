using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using NCI.Web.UI.WebControls;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This Snippet Template is for displaying page options based on Alternate content versions.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:PageOptionsDisplay runat=server></{0}:PageOptionsDisplay>")]
    public class PageOptionsDisplay : SnippetControl
    {

        public void Page_Load(object sender, EventArgs e)
        {
            // In this case the snippet info data is not HTML(which is often the case)
            // but xml data which contains rendering properties for each page option item 
            processPageOptionsData(SnippetInfo.Data);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        /// <summary>
        /// Process the page options information in the xml and creates 
        /// the page options items.
        /// </summary>
        /// <param name="snippetXmlData">The xml fragment which contains pageoptions information.</param>
        private void processPageOptionsData(string snippetXmlData)
        {
            // If AlternateContentVersions information is not in the instructions then do not create 
            // the PageOptions box.
            string[] acvKeys = PageAssemblyContext.Current.PageAssemblyInstruction.AlternateContentVersionsKeys;

            if (acvKeys != null)
            {
                // TODO: use the factory to create the Module_PageOptionsBox objects
                XmlTextReader reader = new XmlTextReader(snippetXmlData.Trim(), XmlNodeType.Element, null);
                XmlSerializer serializer = new XmlSerializer(typeof(Module_PageOptionsBox), "cde");
                Module_PageOptionsBox mPBO = (Module_PageOptionsBox)serializer.Deserialize(reader);

                if (mPBO != null)
                {
                    // Create the Page Options box control.
                    PageOptionsBox pageOptionsBox = new PageOptionsBox();
                    pageOptionsBox.BoxTitle = mPBO.Title;
                    pageOptionsBox.CssClass = "po-box";

                    foreach (PageOption pgOptionItem in mPBO.PageOptions)
                    {
                        // Check if the Pageoptions are recognized in the Alternate Content Version keys
                        if (acvKeys.Contains<string>(pgOptionItem.Key))
                        {
                            NCI.Web.UI.WebControls.PageOption pgoBase = null;

                            if (String.Compare(pgOptionItem.OptionType, PageOptionType.Link.ToString()) == 0)
                            {
                                pgoBase = new LinkPageOption();
                                ((LinkPageOption)pgoBase).OnClick = pgOptionItem.WebAnalyticsFunction;
                            }

                            if (pgoBase != null)
                            {
                                pgoBase.CssClass = pgOptionItem.cssClass;
                                pgoBase.LinkText = pgOptionItem.LinkText;
                                pageOptionsBox.PageOptions.Add(pgoBase);
                            }
                        }
                    }

                    this.Controls.Add(pageOptionsBox);
                }
            }
        }
    }
}
