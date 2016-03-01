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
    /// <summary>
    /// 
    /// </summary>
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
        /// <remarks>
        /// So we will handle backwards compatibility by using the Theme approach if TemplateThemeCollection
        /// has something in it, OR fallback to the old PageTemplateCollection way if there are no themes.
        ///
        /// This way the DOCs can use the old mechanism, and CGov can use the new.  Also the go live for
        /// CGov will not require the new PTC to be published before the CDE code is deployed.
        /// </remarks>
        /// </summary>
        /// <param name="themeName">The name of the theme that the template belongs to.</param>
        /// <param name="templateName">The logical name of the page template we are looking for</param>
        /// <param name="version">The current display version that is being used.  This is used to choose the correct page template. </param>
        /// <returns></returns>
        public static PageTemplateInfo GetPageTemplateInfo(string themeName, string templateName, DisplayVersions version)
        {
            //Error if the templateName is null
            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException("templateName", "The templateName parameter cannot be null or empty.");
            }

            //Error if we cannot get to the PTC
            if (PageTemplateConfiguration == null)
            {
                throw new Exception("PageTemplateConfiguration is null, Cannot determine page template");
            }

            //Check and see if we either have Template Themes or old school PageTemplateCollections and
            //if we do not have either, throw and exception
            if (!(
                (PageTemplateConfiguration.TemplateThemeCollection != null && PageTemplateConfiguration.TemplateThemeCollection.Length > 0) ||
                (PageTemplateConfiguration.PageTemplateCollections != null && PageTemplateConfiguration.PageTemplateCollections.Length > 0)
                )) 
            {
                throw new Exception("Neither Template Themes nor Page template collections exist in the PageTemplateConfiguration, cannot load page template.");
            }
           

            // Now we determine the way to get to the templateInfo.  Themes will take priority over old 
            // way.
            PageTemplateInfo rtnTempInfo = null;

            if (PageTemplateConfiguration.TemplateThemeCollection.Length > 0)
            {
                //Check Theme Name
                if (String.IsNullOrWhiteSpace(themeName))
                    throw new ArgumentNullException("themeName", "The themeName parameter cannot be null or empty.");

                //Handle New
                TemplateThemeInfo themeInfo = PageTemplateConfiguration.TemplateThemeCollection.FirstOrDefault(tti => tti.ThemeName == themeName);

                if (themeInfo == null)
                    throw new Exception("Cannot Find Theme named: " + themeName);

                rtnTempInfo = themeInfo.GetPageTemplateInfo(templateName, version);
            }
            else if (PageTemplateConfiguration.PageTemplateCollections.Length > 0)
            {
                PageTemplateCollection tempColl = PageTemplateConfiguration.PageTemplateCollections.FirstOrDefault(ptc => ptc.TemplateName == templateName);

                if (tempColl != null) {
                    rtnTempInfo = tempColl.GetPageTemplateInfo(version);
                }
            }

            return rtnTempInfo;
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
