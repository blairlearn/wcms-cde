using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.TCGA.Apps;

namespace TCGA.Web.SnippetTemplates
{
    public partial class ViewPublicationsByCancerType : AppsBaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Add the all option.

                // Load the dropdown box, with default all selected when the page is first 
                // displayed
            }
        }


    }
}