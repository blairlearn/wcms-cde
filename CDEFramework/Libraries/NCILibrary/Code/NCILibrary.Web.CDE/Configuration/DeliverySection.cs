using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    /// <summary>
    /// Represents configuration information for the CDE.
    /// </summary>
    public class ContentDeliveryEngineSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the configuration for the PublishedContent Listing Handler, which is
        /// used to list select contents of the PublishedContent folder.
        /// </summary>
        [ConfigurationProperty("publishedContentListing")]
        public PublishedContentListingElement PublishedContentListing
        {
            get { return (PublishedContentListingElement)base["publishedContentListing"];  }
        }

        /// <summary>
        /// Gets the Page Assembly configuration used by the PageAssemblyInstructionLoader
        /// to determine what assemblers are available
        /// </summary>
        [ConfigurationProperty("pageAssembly")]
        public PageAssemblyElement PageAssembly
        {
            get { return (PageAssemblyElement)base["pageAssembly"]; }
        }

        /// <summary>
        /// Gets the File Assembly configuration used by the FileAssemblyInstructionLoader
        /// to determine what assemblers are available
        /// </summary>
        [ConfigurationProperty("fileInstruction")]
        public FileInstructionElement FileInstruction
        {
            get { return (FileInstructionElement)base["fileInstruction"]; }
        }

        /// <summary>
        /// Gets various information on paths configured within the system.  NOTE: these 
        /// paths have ended up being string formatters for getting a specific file path
        /// and less about the paths themselves.  (I.e. the BestBets info is not the BB folder,
        /// but template for getting a specific BB's FullPath.
        /// </summary>
        [ConfigurationProperty("pathInformation")]
        public PathInformationElement PathInformation
        {
            get { return (PathInformationElement)base["pathInformation"]; }
        }

        /// <summary>
        /// Gets the text that should be appended to all Browser Titles. 
        /// </summary>
        [ConfigurationProperty("pageTitle")]
        public PageTitleInformationElement PageTitle
        {
            get { return (PageTitleInformationElement)base["pageTitle"]; }
        }

        /// <summary>
        /// Gets information about what the root Canonical host name of this site is.
        /// </summary>
        [ConfigurationProperty("canonicalHostName")]
        public CanonicalHostNameInformationElement CanonicalHostName
        {
            get { return (CanonicalHostNameInformationElement)base["canonicalHostName"]; }
        }

        /// <summary>
        /// Configuration for the old Mobile Redirector that redirected users on mobile
        /// devices to m.cancer.gov.
        /// </summary>
        [ConfigurationProperty("mobileRedirector")]
        public MobileRedirectorInformationElement MobileRedirector
        {
            get { return (MobileRedirectorInformationElement)base["mobileRedirector"]; }
        }

        /// <summary>
        /// Pretty url of the published page that represents the home page.  (usually /defaultHomePage)
        /// </summary>
        [ConfigurationProperty("defaultHomePage")]
        public DefaultHomePageElement DefaultHomePage
        {
            get { return (DefaultHomePageElement)base["defaultHomePage"]; }
        }

    }
}
