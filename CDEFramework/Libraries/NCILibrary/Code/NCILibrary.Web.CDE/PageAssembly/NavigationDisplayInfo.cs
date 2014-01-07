using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using NCI.Logging;

namespace NCI.Web.CDE
{
    /// <summary>
    /// This is the class that will determine the css Class as well as getting the normal xml output
    /// </summary>
    public class NavigationDisplayInfo
    {
        public NavigationDisplayParams displayParams { get; private set; }
        public NavigationItem rootNavItem { get; private set; }

        /// <summary>
        /// Internal Constructor for NavigationDisplayInfo
        /// Use ParseTree to get the item
        /// </summary>
        /// <param name="item">Navigation Item</param>
        /// <param name="display">NavigationDisplayParams</param>
        private NavigationDisplayInfo(NavigationItem item, NavigationDisplayParams display)
        {
            displayParams = display;
            rootNavItem = item;
        }

        /// <summary>
        /// Inititates the parse to get the xml from the snippet infos
        /// </summary>
        /// <param name="xml">the xml passed</param>
        /// <returns>the Navigation Display info</returns>
        public static NavigationDisplayInfo ParseTree(string xml)
        {

            XmlDocument doc = new XmlDocument();
            bool docLoaded = false;

            try
            {
                doc.LoadXml(xml);
                docLoaded = true;   // Won't hit this line if there's an error loading.
            }
            catch (Exception ex)
            {
                // Swallow the exception. We still get an error recorded, and
                // the user still gets content, just without the navigation element.
                Logger.LogError("NCI.Web.CDE.NavigationDisplayInfo.NavigationDisplayInfo", "Unable to load XML document" + xml, NCIErrorLevel.Error, ex);
            }

            // If the XML string wasn't valid, don't try parsing the string.  Just return an empty
            // NavigationDisplayInfo structure.
            NavigationDisplayInfo tree = null;
            if(docLoaded )
                tree = ParseTree(doc.DocumentElement);
            else
                tree = new NavigationDisplayInfo(null, null);

            return tree;
        }

        /// <summary>
        /// This method gets the nodes from the xml nodes which are the fields needed for the NaviationDisplayInfo
        /// </summary>
        /// <param name="xml">xml node passed</param>
        /// <returns>the parsed navigation display info</returns>
        private static NavigationDisplayInfo ParseTree(XmlNode xml)
        {
            XmlNode navDisplayParamNode = xml.SelectSingleNode("./DisplayParams");
            XmlNode navItemNode = xml.SelectSingleNode("./NavItem");

            if (navDisplayParamNode == null)
            {
                Logger.LogError("NCI.Web.CDE.NavigationDisplayInfo.NavigationDisplayInfo", "DisplayParams is null.", NCIErrorLevel.Error);
                Logger.LogError("NCI.Web.CDE.NavigationDisplayInfo.NavigationDisplayInfo", xml.OuterXml, NCIErrorLevel.Error);
                throw new Exception("Error: DisplayParams is null");
            }

            // Load details (if any) of the Navigation Item.
            NavigationItem item = null;
            if (navItemNode != null)
                item = NavigationItem.ParseTree(navItemNode);

            NavigationDisplayParams display = NavigationDisplayParams.ParseElement(navDisplayParamNode);

            NavigationDisplayInfo result = new NavigationDisplayInfo(item, display);


            return result;
        }

    }
}
