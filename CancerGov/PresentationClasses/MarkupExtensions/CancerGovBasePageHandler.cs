using System;
using System.Web;
using NCI.Text;
using CancerGov.UI.Pages;

namespace CancerGov.MarkupExtensions
{
    /// <summary>
    /// Derives from MarkupExtensionHandler and adds an overloaded Process method that includes the BasePage 
    /// for the current request.
    /// </summary>
    public abstract class CancerGovBasePageHandler : MarkupExtensionHandler
    {
        /// <summary>
        /// Pulls the current BasePage from the http context so cancer gov specific 
        /// Process methods in derived classes will have access to the base page.
        /// </summary>
        protected BasePage BasePage
        {
            get
            {
                if ((HttpContext.Current.Handler is BasePage) == false)
                {
                    throw new MarkupExtensionException(this.GetType() + " requires that HttpContext.Current.Handler be compatible with " + typeof(BasePage) + " but was of type " + HttpContext.Current.Handler.GetType());
                }

                return (BasePage)HttpContext.Current.Handler;
            }
        }
    }
}
