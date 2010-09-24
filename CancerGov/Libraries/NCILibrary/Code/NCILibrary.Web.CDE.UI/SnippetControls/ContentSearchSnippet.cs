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

        /// <summary>
        /// Keyword is a search criteria used in searching
        /// </summary>
        override protected string KeyWords
        {
            get 
            {
                return string.IsNullOrEmpty(this.Page.Request.Params["keyword"]) ? String.Empty : this.Page.Request.Params["keyword"];
            }
        }

        /// <summary>
        /// Startdate is a search criteria used in searching.if 
        /// StartDate value is present then both StartDate and 
        /// EndDate value should exist.
        virtual protected DateTime StartDate
        {
            get
            {
                if (string.IsNullOrEmpty(this.Page.Request.Params["startdate"]))
                        return DateTime.MinValue;
                return DateTime.Parse(this.Page.Request.Params["startdate"]);
            }
        }

        /// <summary>
        /// Startdate is a search criteria used in searching.if 
        /// StartDate value is present then both StartDate and 
        /// EndDate value should exist.
        virtual protected DateTime EndDate
        {
            get
            {
                if (string.IsNullOrEmpty(this.Page.Request.Params["enddate"]))
                    return DateTime.MaxValue;
                return DateTime.Parse(this.Page.Request.Params["enddate"]);
            }
        }

    }
}
