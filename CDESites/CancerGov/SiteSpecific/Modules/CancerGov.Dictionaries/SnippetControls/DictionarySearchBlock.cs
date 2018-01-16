using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CancerGov.CDR.TermDictionary;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.Dictionary;
using NCI.Web.Dictionary.BusinessObjects;
using Microsoft.Security.Application;
using NCI.Web;

namespace CancerGov.Dictionaries.SnippetControls
{
    public class DictionarySearchBlock : UserControl
    {
        protected DictionaryHTMLSearchBlock searchBlock;

        public string FormAction
        {
            get
            {
                return this.searchBlock.FormAction;
            }
            set
            {
                this.searchBlock.FormAction = value;
            }
        }

        public bool DisplayHelpLink
        {
            get
            {
                return searchBlock.DisplayHelpLink;
            }
            set
            {
                searchBlock.DisplayHelpLink = value;
            }
        }

        public string listBoxBaseUrl
        {
            get
            {
                return this.searchBlock.listBoxBaseUrl;
            }
            set
            {
                this.searchBlock.listBoxBaseUrl = value;
            }
        }

        public bool listBoxShowAll
        {
            get
            {
                return this.searchBlock.listBoxShowAll;
            }
            set
            {
                this.searchBlock.listBoxShowAll = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                searchBlock = (DictionaryHTMLSearchBlock)Page.LoadControl("~/SnippetTemplates/TermDictionary/SpanishDictionarySearchBlock.ascx");
            }
            else
            {
                searchBlock = (DictionaryHTMLSearchBlock)Page.LoadControl("~/SnippetTemplates/TermDictionary/EnglishDictionarySearchBlock.ascx");
            }

            this.Controls.Add(searchBlock);
        }
    }
}