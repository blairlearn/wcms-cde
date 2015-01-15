using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NCI.Web.CDE
{
    /// <summary>
    /// This method gets the node that contains the CSS class that is needed to load the css for the navigation
    /// </summary>
    public class NavigationDisplayParams
    {
        public string MobileNav {get; private set;}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mobileNav">string</param>
        private NavigationDisplayParams(string mobileNav)
        {
            MobileNav = mobileNav;
        }

        /// <summary>
        /// Gets the string for the css class node
        /// </summary>
        /// <param name="xml">the node passed from NavigationDisplayInfo</param>
        /// <returns>Navigation Display Params</returns>
        public static NavigationDisplayParams ParseElement(XmlNode xml)
        {
            XmlNode mobileNav = xml.SelectSingleNode("./MobileNav");

            if (mobileNav == null)
            {
                throw new Exception("Error: MobileNav is null");
            }

            NavigationDisplayParams result = new NavigationDisplayParams(mobileNav.InnerText);
            
            
            return result;
        }

    }
}
