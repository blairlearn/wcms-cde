using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.TCGA.Apps;
using System.Xml.Linq;
using NCI.Web.CDE;

namespace TCGA.Web.SnippetTemplates
{
    public partial class ViewPublicationsByCancerType : AppsBaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind the dropdownlist control to display the cancer types

                // Add the all option.

                // Load the dropdown box, with default all selected when the page is first 
                // displayed.
            }

        }

        private XElement PublicationDataXml
        {
            get
            {
                XElement root = XElement.Load(PublicationsDataPath);
                IEnumerable<XElement> purchaseOrders =
                    from el in root.Elements("PurchaseOrder")
                    where
                        (from add in el.Elements("Address")
                         where
                             (string)add.Attribute("Type") == "Shipping" &&
                             (string)add.Element("State") == "NY"
                         select add)
                        .Any()
                    select el;
                foreach (XElement el in purchaseOrders)
                    Console.WriteLine((string)el.Attribute("PurchaseOrderNumber"));

                return root;
            }
        }

        private string PublicationsDataPath
        {
            get 
            {
                return "";
            }
        }
    }
}