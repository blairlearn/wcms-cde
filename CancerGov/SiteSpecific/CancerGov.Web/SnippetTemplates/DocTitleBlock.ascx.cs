using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class DocTitleBlock : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Parse Data To Get Information
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.SnippetInfo.Data);

                XmlNode xnTitle = doc.SelectSingleNode("//Title");
                XmlNode titleDisplay = doc.SelectSingleNode("//TitleDisplay");
                XmlNode imageUrl = doc.SelectSingleNode("//ImageUrl");

                string title = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("");

                if (titleDisplay != null)
                {
                    switch (titleDisplay.Value)
                    {
                        case "DocTitleBlockTitle":
                            {
                            }
                            break;
                        default:
                            {

                            }
                            break;
                    }
                }
                
            }
            catch (Exception ex)
            {
            }
        }
    }
}