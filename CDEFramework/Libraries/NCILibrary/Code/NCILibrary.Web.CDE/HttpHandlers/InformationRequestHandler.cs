using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Web.CDE.InformationRequest;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE
{
    public class InformationRequestHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //After processing the Information Request and sending the result - shut down the HTTP context
//            context.Response.Flush();
//            context.Response.End();
        }

        #endregion
    }
}
