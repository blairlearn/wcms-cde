using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using NCI.Web.CDE.Modules;
using NCI.DataManager;
using NCI.Web.UI.WebControls;

namespace NCI.Web.CDE.UI.SnippetControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DeprecatedDynamicList runat=server></{0}:DeprecatedDynamicList>")]
    
    public class DeprecatedDynamicList : BaseSearchSnippet
    {
        override protected SearchList SearchList
        {
            get
            {
                if (base.SearchList == null)
                {
                    base.SearchList = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);
                    if(base.SearchList.ResultsTemplate.Contains("~/DynamicListTemplates"))
                    {
                        string filename = VirtualPathUtility.GetFileName(this.AppRelativeVirtualPath).Replace(".ascx", "");
                        if(filename == "BlogLandingDynamicList")
                        {
                            filename = "DynamicBlogDescImgDate";
                        }

                        base.SearchList.ResultsTemplate = "~/VelocityTemplates/" + filename + ".vm";
                    }
                }
                return base.SearchList;
            }
        }
    }
}
