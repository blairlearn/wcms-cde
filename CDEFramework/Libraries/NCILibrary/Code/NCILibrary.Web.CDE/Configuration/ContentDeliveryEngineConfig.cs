using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    /// <summary>
    /// Helper class that wraps the fetching of CDE configuration information
    /// </summary>
    public static class ContentDeliveryEngineConfig
    {
        private static string _deliveryEnginePath = "nci/web/cde";
        private static ContentDeliveryEngineSection _deliveryEngine;

        /// <summary>
        /// Gets the configuration for the PublishedContent Listing Handler, which is
        /// used to list select contents of the PublishedContent folder.
        /// </summary>
        public static PublishedContentListingElement PublishedContentListing
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.PublishedContentListing;
            }
        }

        /// <summary>
        /// Gets various information on paths configured within the system.  NOTE: these 
        /// paths have ended up being string formatters for getting a specific file path
        /// and less about the paths themselves.  (I.e. the BestBets info is not the BB folder,
        /// but template for getting a specific BB's FullPath.
        /// </summary>
        public static PathInformationElement PathInformation
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.PathInformation;
            }
        }

        /// <summary>
        /// Gets the Page Assembly configuration used by the PageAssemblyInstructionLoader
        /// to determine what assemblers are available
        /// </summary>
        public static PageAssemblyElement PageAssembly
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.PageAssembly;
            }
        }

        /// <summary>
        /// Gets the File Assembly configuration used by the FileAssemblyInstructionLoader
        /// to determine what assemblers are available
        /// </summary>
        public static FileInstructionElement FileInstruction
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.FileInstruction;
            }
        }

        /// <summary>
        /// Gets the text that should be appended to all Browser Titles. 
        /// </summary>
        public static PageTitleInformationElement PageTitle
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.PageTitle;
            }
        }

        /// <summary>
        /// Gets information about what the root Canonical host name of this site is.
        /// </summary>
        public static CanonicalHostNameInformationElement CanonicalHostName
        {
            get
            {
                //ContentDeliveryEngine will throw ConfigurationErrorsException
                //So this does not have to...
                return ContentDeliveryEngine.CanonicalHostName;
            }
        }

        /// <summary>
        /// Configuration for the old Mobile Redirector that redirected users on mobile
        /// devices to m.cancer.gov.
        /// </summary>
        public static MobileRedirectorInformationElement MobileRedirector
        {
            get
            {
                return ContentDeliveryEngine.MobileRedirector;
            }
        }

        /// <summary>
        /// Pretty url of the published page that represents the home page.  (usually /defaultHomePage)
        /// </summary>
        public static DefaultHomePageElement DefaultHomePage
        {
            get
            {
                return ContentDeliveryEngine.DefaultHomePage;
            }
        }

        //Question: Should this still be public???
        public static ContentDeliveryEngineSection ContentDeliveryEngine
        {
            get
            {
                if (_deliveryEngine == null)
                {
                    _deliveryEngine = (ContentDeliveryEngineSection)ConfigurationManager.GetSection(_deliveryEnginePath);

                    // When the delivery sectionGroup isn't defined, _delivery comes back null.
                    if (_deliveryEngine == null)
                    {
                        throw new ConfigurationErrorsException(String.Format(
                            "Could not load delivery engine configuration from {0}.  Please ensure the nci/web/cde section group and cde element have been defined in the configuration file.", _deliveryEnginePath));
                    }
                }

                return _deliveryEngine;
            }
        }
    }
}
