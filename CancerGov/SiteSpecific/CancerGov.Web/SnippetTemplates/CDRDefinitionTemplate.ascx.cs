using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.UI.Configuration;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class CDRDefinitionTemplate : SnippetControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ltDefinitionText.Text = DefinitionText;
        }

        protected string DefinitionText
        {
            get 
            {
                string definitionText = string.Empty;
                definitionText = SnippetInfo.CDRDefinition + ":";
                // TODO:make the call to the CDR database here to obtain the text based on the 
                // CDRId

                return definitionText;
            }
        }

    }
}