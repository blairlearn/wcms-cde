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
    [ToolboxData("<{0}:ContentSearchSnippet runat=server></{0}:ContentSearchSnippet>")]
    public class ContentSearchSnippet : BaseSearchSnippet
    {
        override protected SearchList SearchList
        {
            get
            {
                if (base.SearchList == null)
                    base.SearchList = ModuleObjectFactory<ContentSearchList>.GetModuleObject(SnippetInfo.Data);
                return base.SearchList;
            }
        }
    }
}
