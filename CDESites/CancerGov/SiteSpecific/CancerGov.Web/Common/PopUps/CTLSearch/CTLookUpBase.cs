using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace CancerGov.Web
{
    /// <summary>
    /// Summary description for Redesign.
    /// </summary>
    public partial class CTLookupBase : System.Web.UI.Page
    {
        protected string Title
        {
            get
            {
                string title = Request.Params["title"];
                if (title != null)
                {
                    return title.Replace(" ", "+");
                }
                else
                {
                    return "";
                }
            }
        }

        #region Validation Routines to avoid XSS
        public void ValidateInputData(string inputValue)
        {
            if (!string.IsNullOrEmpty(inputValue))
            {
                if (inputValue.LastIndexOfAny(new char[] { '\'', '(', ')', '<', '>', '"', '%', ';', '&', '+' }) != -1)
                    throw new Exception("Invalid Data");
            }
        }

        public void ValidateValidValues(string inputValue, ArrayList valuesList)
        {
            string fld1 = inputValue;
            if (!string.IsNullOrEmpty(fld1))
            {
                    fld1 = fld1.ToLower();
                if (!valuesList.Contains(fld1))
                    throw new Exception("Invalid Data");
            }
        }
        #endregion
    }
}