using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
            try
            {
                doc.LoadXml(xml);
            }
            catch (Exception ex)
            {
                //log error
            }

            return ParseTree(doc.DocumentElement); ;
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

            if (navDisplayParamNode == null || navItemNode == null)
            {
                throw new Exception("Error: One or both DisplayParams or NavItem is null");
            }

            //I made the NavigationItem.ParseTree public so that I could just call straight into to the parse tree method
            //of with the node
            NavigationItem item = NavigationItem.ParseTree(navItemNode);
            NavigationDisplayParams display = NavigationDisplayParams.ParseElement(navDisplayParamNode);

            NavigationDisplayInfo result = new NavigationDisplayInfo(item, display);


            return result;
        }

    }
}
