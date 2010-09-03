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

            if(!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "popup"))
                this.Page.ClientScript.RegisterClientScriptBlock( this.GetType(), "popup", "<script language=\"JavaScript\" src=\"/scripts/popEvents.js\"></script>");
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
            IPageAssemblyInstruction pgInstruction = PageAssemblyContext.Current.PageAssemblyInstruction;
            // If AlternateContentVersions information is not in the instructions then do not create 
            // the PageOptions box.
            string[] acvKeys = pgInstruction.AlternateContentVersionsKeys;

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
                        try
                        {
                            // Check if the Pageoptions are recognized in the Alternate Content Version keys
                            if (acvKeys.Contains<string>(pgOptionItem.Key))
                            {
                                NCI.Web.UI.WebControls.PageOption pgoBase = null;

                                if (String.Compare(pgOptionItem.OptionType, PageOptionType.Link.ToString()) == 0)
                                {
                                    pgoBase = new LinkPageOption();
                                    ((LinkPageOption)pgoBase).OnClick = pgOptionItem.WebAnalyticsFunction;
                                    ((LinkPageOption)pgoBase).Href = pgInstruction.GetUrl(pgOptionItem.Key).ToString();
                                }
                                else if (String.Compare(pgOptionItem.OptionType, PageOptionType.Email.ToString()) == 0)
                                {
                                    pgoBase = new LinkPageOption();
                                    ((LinkPageOption)pgoBase).Href = pgInstruction.GetUrl("Email").ToString();
                                    ((LinkPageOption)pgoBase).OnClick = pgOptionItem.WebAnalyticsFunction;
                                    ((LinkPageOption)pgoBase).OnClick += " " + "dynPopWindow('" + ((LinkPageOption)pgoBase).Href.Replace("'", "%27").Replace("(", "%28").Replace(")", "%29") + "', 'emailPopUp', 'height=365,width=525'); return false;";
                                }
                                else if (String.Compare(pgOptionItem.OptionType, PageOptionType.BookMarkShare.ToString()) == 0)
                                {
                                    pgoBase = new AddThisPageOption();
                                    ((AddThisPageOption)pgoBase).Settings.Language = "en-us";
                                    ((AddThisPageOption)pgoBase).PageTitle = pgInstruction.GetUrl("PrettyUrl").ToString();
                                }

                                if (pgoBase != null)
                                {
                                    pgoBase.CssClass = pgOptionItem.cssClass;
                                    pgoBase.LinkText = pgOptionItem.LinkText;
                                    pageOptionsBox.PageOptions.Add(pgoBase);
                                }
                            }
                        }
                        catch
                        { 
                            //TODO, log exception
                        }
                    }

                    this.Controls.Add(pageOptionsBox);
                }
            }
        }
    }
}
