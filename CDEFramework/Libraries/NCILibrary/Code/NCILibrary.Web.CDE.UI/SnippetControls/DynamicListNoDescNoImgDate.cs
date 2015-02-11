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
    [ToolboxData("<{0}:DynamicListSnippet runat=server></{0}:DynamicListSnippet>")]
    public class DynamicListNoDescNoImgDate : BaseSearchSnippet
    {
        override protected SearchList SearchList
        {
            get
            {
                if (base.SearchList == null)
                {
                    base.SearchList = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);

                    //This is completely dirty and really a hack, but it gets this done.  This should be
                    //fixed in a future release. --BryanP 2/10/2015
                    base.SearchList.ResultsTemplate = @" 
                        #foreach($resultItem in $DynamicSearch.Results)
	                        $resultItem.RecNumber<br />
                        #end
                    ";
                }
                return base.SearchList;
            }
            // Do
        }

    }
}
