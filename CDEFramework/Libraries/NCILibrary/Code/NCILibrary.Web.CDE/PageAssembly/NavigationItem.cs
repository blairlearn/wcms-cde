using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace NCI.Web.CDE
{
    /// <summary>
    /// This class represents the Navigation structure of the section to be displayed on 
    /// </summary>
    public class NavigationItem
    {
        private List<NavigationItem> _items = new List<NavigationItem>();

        public string Title { get; private set; }
        public string URL { get; private set; }
        public string SectionPath { get; private set; }

        /// <summary>
        /// Get the child items for this Navigation Item.
        /// Note: This is gauranteed to return an arry, which may be empty.
        /// </summary>
        public NavigationItem[] ChildItems
        {
            get { return _items.ToArray(); }
        }

        /// <summary>
        /// This  is the general constructor for the Navigation Item
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="sectionPath"></param>
        private NavigationItem(string title, string url, string sectionPath)
        {
            Title = title;
            URL = url;
            SectionPath = sectionPath;
        }
        /// <summary>
        /// This parses the xml that is passed in into the structure needed to produce the html for the site
        /// </summary>
        /// <param name="xml">xml passed in from snippet info</param>
        /// <returns>the parsed tree</returns>
        public static NavigationItem ParseTree(string xml)
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            return ParseTree(doc.DocumentElement); ;
        }

        /// <summary>
        /// parses the elements from the xml doc
        /// </summary>
        /// <param name="node">the element passed in</param>
        /// <returns>the Navigation Item</returns>
        internal static NavigationItem ParseTree(XmlNode node)
        {
            //gets sets the nodes to what they are in the xml document
            XmlNode titleNode = node.SelectSingleNode("./Title");
            XmlNode urlNode = node.SelectSingleNode("./URL");
            XmlNode sectionPathNode = node.SelectSingleNode("./SectionPath");

            if (titleNode == null || urlNode == null || sectionPathNode==null)
            {
                throw new Exception("Error Parsing Nodes: Title, URL and/or SectionPath are null");
            }

            //creates the navigation item based on the nodes above
            NavigationItem result = new NavigationItem(titleNode.InnerText, urlNode.InnerText, sectionPathNode.InnerText);

            //if there are children of a node it gets set here
            XmlNodeList Children = node.SelectNodes("./NavItems/NavItem");

            //check if null
            if (Children == null)
            {
                throw new Exception("Error: There are no child nodes");
            }
            else
            {
                //if children isn't null then recursively generates the nav items
                foreach (XmlNode noder in Children)
                {
                    result._items.Add(ParseTree(noder));
                }
            
            }
                return result;
        }   
        
        /// <summary>
        /// no longer used
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return ToStringInternal(0);
        }
        /// <summary>
        /// no longer used
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        private String ToStringInternal(int depth)
        {
            String s = "";
            for (int i = 0; i < depth; i++)
            {
                s += "\t";
            }
            s += this.Title + "(" + this.URL + ")\n";
            foreach (NavigationItem item in _items)
            {
                s += item.ToStringInternal(depth + 1);
            }
            return s;
        }
    }
}
