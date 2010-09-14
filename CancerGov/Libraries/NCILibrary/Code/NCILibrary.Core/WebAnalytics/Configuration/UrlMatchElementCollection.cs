using System;
using System.Configuration;


namespace NCI.Web.CDE.WebAnalytics.Configuration
{
    [ConfigurationCollection(typeof(UrlMatchElement),
         AddItemName = "urlMatch",
         CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class UrlMatchElementCollection : ConfigurationElementCollection
    {

        public UrlMatchElement GetMatchForURL(string url)
        {
            UrlMatchElement returnElement = null;

            if (url != null)
            {
                string localurl = url.ToLower();

                foreach (UrlMatchElement element in this)
                    if (localurl.IndexOf(element.PathMatch.ToLower()) >= 0)
                    {
                        returnElement = element;
                        break;
                    }
            }
            return returnElement;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlMatchElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlMatchElement)element).PathMatch;
        }
    }    
}
