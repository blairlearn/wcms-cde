using System;
using System.Web;
using NCI.Text;
using CancerGov.UI.Pages;

namespace CancerGov.MarkupExtensions
{
    /// <summary>
    /// Derives from CancerGovBasePageHandler and adds an overloaded Process method that includes the ViewPage 
    /// for the current request.
    /// </summary>
    public abstract class CancerGovViewPageHandler : CancerGovBasePageHandler
    {
        /// <summary>
        /// Pulls the current ViewPage from the http context so cancer gov specific 
        /// Process methods in derived classes will have access to the base page.
        /// </summary>
        protected ViewPage ViewPage
        {
            get
            {
                if ((HttpContext.Current.Handler is ViewPage) == false)
                {
                    throw new MarkupExtensionException(this.GetType() + " requires that HttpContext.Current.Handler be compatible with " + typeof(ViewPage) + " but was of type " + HttpContext.Current.Handler.GetType());
                }

                return (ViewPage)HttpContext.Current.Handler;
            }
        }
    }
}
