using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;

namespace CancerGov.Dictionaries.SnippetControls
{
    public class DictionaryHTMLSearchBlock : UserControl
    {
        public string FormAction { get; set; }

        protected Literal showHelpButton;

        public bool DisplayHelpLink
        {
            get
            {
                if(this.showHelpButton != null)
                {
                    return showHelpButton.Visible;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if(this.showHelpButton != null)
                {
                    showHelpButton.Visible = value;
                }
            }
        }

        protected AlphaListBox alphaListBox;

        public string listBoxBaseUrl
        {
            get
            {
                if(this.alphaListBox != null)
                {
                    return this.alphaListBox.BaseUrl;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                if(this.alphaListBox != null)
                {
                    alphaListBox.BaseUrl = value;
                }
            }
        }
    }
}
