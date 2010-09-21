using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using NCI.Web.CDE.Modules;

namespace NCI.Web.CDE.UI.SnippetControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DynamicListSnippet runat=server></{0}:DynamicListSnippet>")]
    public class DynamicListSnippet : SnippetControl
    {
        public void Page_Load(object sender, EventArgs e)
        {
            // In this case the snippet info data is not HTML(which is often the case)
            // but xml data which are the dynamic search properties.

        }

        private void processData(string snippetXmlData)
        {
            try
            {
                DynamicList dynamicList = ModuleObjectFactory<DynamicList>.GetModuleObject(snippetXmlData);
                if (dynamicList != null)
                {
 
                }

            }
            catch (Exception ex)
            { 

            }
        }
    }
}
