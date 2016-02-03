using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;

using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE
{
    public static class PageTemplateResolver
    {
        private const string PageTemplateConfigurationKey = "PageTemplateConfiguration";
        private const string PageTemplateConfigurationFilename = "PageTemplateConfiguration.xml";
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets an instance of the HttpServerUtility for the current request.
        /// </summary>
        private static HttpServerUtility Server {
            get { return HttpContext.Current.Server; }
        }

        private static string PageTemplateConfigurationPath
        {
            get
            {
                return Server.MapPath(ContentDeliveryEngineConfig.PathInformation.PageTemplateConfigurationPath.Path);
            }
        }
        
        private static PageTemplateConfiguration PageTemplateConfiguration
        {
            get 
            {
                if (HttpContext.Current.Cache[PageTemplateConfigurationKey] == null)
                {
                    //Make sure no one else tries to access this until we are done.
                    lock (_lock)
                    {
                        //Double check that another thread did not create the item before us.
                        if (HttpContext.Current.Cache[PageTemplateConfigurationKey] == null)
                        {
                            PageTemplateConfiguration config =
                                LoadPageTemplateConfiguration(PageTemplateConfigurationPath);

                            AddPageTemplateConfigurationToCache(config, PageTemplateConfigurationPath);
                        }
                    }
                }
                    
                return (PageTemplateConfiguration)HttpContext.Current.Cache[PageTemplateConfigurationKey];
            }
        }

        private static void AddPageTemplateConfigurationToCache(PageTemplateConfiguration config, string configurationPath)
        {
            HttpContext.Current.Cache.Add(
                PageTemplateConfigurationKey,
                config,
                new CacheDependency(configurationPath),
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                new CacheItemRemovedCallback(PTCCachedRemovalCallback));

        }

        private static void PTCCachedRemovalCallback(string key, object config, CacheItemRemovedReason reason)
        {
            //If the PageTemplateConfiguration was removed because of the dependency, then
            //reload the PageTemplateConfiguration
            if (
                key == PageTemplateConfigurationKey && 
                reason == CacheItemRemovedReason.DependencyChanged
                )
            {
                PageTemplateConfiguration reloadedConfig =
                    LoadPageTemplateConfiguration(PageTemplateConfigurationPath);

                AddPageTemplateConfigurationToCache(reloadedConfig, PageTemplateConfigurationPath);
            }
        }

        /// <summary>
        /// Checks to see if a given theme name is defined in the PageTemplateConfiguration
        /// </summary>
        /// <param name="themeName">The name of the theme to check</param>
        /// <returns>true if the theme is defined, false if not</returns>
        public static bool IsThemeDefined(string themeName)
        {
            return PageTemplateConfiguration.TemplateThemeCollection.Any(tti => tti.ThemeName == themeName);
        }

        /// <summary>
        /// Retrieves the LayoutTemplateInfo representing the named template.
        /// </summary>
        /// <param name="themeName">The name of the theme that the template belongs to.</param>
        /// <param name="templateName">The logical name of the page template we are looking for</param>
        /// <param name="version">The current display version that is being used.  This is used to choose the correct page template. </param>
        /// <returns></returns>
        public static PageTemplateInfo GetPageTemplateInfo(string themeName, string templateName, DisplayVersions version)
        {
            //Validate ThemeName
            if (string.IsNullOrEmpty(themeName))
            {
                throw new ArgumentNullException("themeName", "The themeName parameter cannot be null or empty.");
            }

            return GetPageTemplateInfo(
                templateName, //The templatename we are looking for
                version, //The version we are looking for
                tti => tti.ThemeName == themeName //Get the theme PTC.
            );
        }

        /// <summary>
        /// Gets the Page Template Info for a logical template for the default theme.
        /// (Where default theme is the first theme where IsDefault == true)
        /// </summary>
        /// <param name="templateName">The logical name of the page template we are looking for</param>
        /// <param name="version">The current display version that is being used.  This is used to choose the correct page template. </param>
        /// <returns></returns>
        public static PageTemplateInfo GetDefaultThemePageTemplateInfo(string templateName, DisplayVersions version)
        {
            return GetPageTemplateInfo(
                templateName, //The templatename we are looking for
                version, //The version we are looking for
                tti => tti.IsDefault //Get the theme PTC.
            );
        }

        /// <summary>
        /// Gets the Page Template Info for the given templateName within the first theme matching the predicate
        /// </summary>
        /// <param name="templateName">The logical name of the page template we are looking for</param>
        /// <param name="version">The current display version that is being used.  This is used to choose the correct page template. </param>
        /// <param name="predicate">A Func<TemplateThemeInfo,bool> that is used to find the first TemplateThemeInfo that matches.</param>
        /// <returns></returns>
        private static PageTemplateInfo GetPageTemplateInfo(string templateName, DisplayVersions version, Func<TemplateThemeInfo, bool> predicate) 
        {
            PageTemplateInfo rtnInfo = null;

            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException("templateName", "The templateName parameter cannot be null or empty.");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate", "The predicate function cannot be null");
            }

            if (PageTemplateConfiguration != null)
            {
                //First, we need to get the theme we are looking for
                TemplateThemeInfo info = PageTemplateConfiguration.TemplateThemeCollection.FirstOrDefault(predicate);

                if (info != null)
                {
                    //We found a theme.  Now get the template info.
                    rtnInfo = info.GetPageTemplateInfo(templateName, version);
                }
            }

            return rtnInfo;
        }

        private static PageTemplateConfiguration LoadPageTemplateConfiguration(string configurationPath)
        {
            PageTemplateConfiguration rtnConfig = null;

            XmlSerializer serializer = new XmlSerializer(typeof(PageTemplateConfiguration));

            bool pageAssemblyInstructionXmlValid = true;

            pageAssemblyInstructionXmlValid = PageAssemblyInstructionFactory.ValidateXml(PageTemplateConfigurationPath,
                                HttpContext.Current.Server.MapPath(ContentDeliveryEngineConfig.PageAssembly.PageAssemblyInfoTypes.XsdPath));

            try
            {
                using (FileStream xmlFile = File.Open(configurationPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {

                    rtnConfig = (PageTemplateConfiguration)serializer.Deserialize(XmlReader.Create(xmlFile));
                }
            }
            catch (Exception ex)
            {
                //TODO: Do something here
            }

            return rtnConfig;
        }
    }
}
