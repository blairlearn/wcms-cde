﻿using System;
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
    [ToolboxData("<{0}:DynamicListNoDescNoImgDate runat=server></{0}:DynamicListNoDescNoImgDate>")]
    public class DynamicListNoDescNoImgDate : BaseSearchSnippet
    {
        override protected SearchList SearchList
        {
            get
            {
                if (base.SearchList == null)
                {
                    DynamicListHelper helper = new DynamicListHelper();
                    base.SearchList = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);
                    base.SearchList.ResultsTemplate = base.SearchList.ResultsTemplate =
                    helper.languageStrings() +
                    @"<h3 class=""dynamic-list-title"">" + base.SearchList.SearchTitle + @"</h3>" +
                    helper.openList() +
                    helper.openListItem() +
                    helper.dateString() +
                    helper.closeListItem() +
                    helper.closeList();
                }
                return base.SearchList;
            }
        }
    }
}
