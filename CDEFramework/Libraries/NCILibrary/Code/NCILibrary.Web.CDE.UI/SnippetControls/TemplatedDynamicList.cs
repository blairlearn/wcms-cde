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
    [ToolboxData("<{0}:TemplatedDynamicList runat=server></{0}:TemplatedDynamicList>")]
    public class TemplatedDynamicList : BaseSearchSnippet
    {
        override protected SearchList SearchList

        {
            get
            {
                if(base.SearchList == null)
                {
                    base.SearchList = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);
                }
                return base.SearchList;
            }
        }
    }
}
