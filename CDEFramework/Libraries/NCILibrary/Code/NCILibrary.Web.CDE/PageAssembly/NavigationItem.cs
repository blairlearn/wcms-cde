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


        private NavigationItem(string title, string url, string sectionPath)
        {
            Title = title;
            URL = url;
            SectionPath = sectionPath;
        }

        public static NavigationItem ParseTree(string xml)
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            return ParseTree(doc.DocumentElement); ;
        }

        private static NavigationItem ParseTree(XmlNode node)
        {

            XmlNode titleNode = node.SelectSingleNode("./Title");
            XmlNode urlNode = node.SelectSingleNode("./URL");
            XmlNode sectionPathNode = node.SelectSingleNode("./SectionPath");

            if (titleNode == null || urlNode == null || sectionPathNode==null)
            {
                throw new Exception("Error Parsing Nodes: Title, URL and/or SectionPath are null");
            }

            NavigationItem result = new NavigationItem(titleNode.InnerText, urlNode.InnerText, sectionPathNode.InnerText);

            XmlNodeList Children = node.SelectNodes("./NavItems/NavItem");

            if (Children == null)
            {
                throw new Exception("Error: There are no child nodes");
            }
            else
            {
                foreach (XmlNode noder in Children)
                {
                    result._items.Add(ParseTree(noder));
                }
            
            }
                return result;
        }   
        

        public override String ToString()
        {
            return ToStringInternal(0);
        }

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
