using System;
using System.Web;
using Common.Logging;
using NCI.Web.CDE.CapabilitiesDetection;
using NCI.Web.ProductionHost;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Stores information useful for assembling the page for the current request.  This class 
    /// stores information on a per request basis (i.e. uses the current HttpContext).
    /// </summary>
    public class PageAssemblyContext
    {
        static ILog log = LogManager.GetLogger(typeof(PageAssemblyContext));

        private static object PAGE_ASSEMBLY_CONTEXT_KEY = new object();

        private static object PAGE_ASSEMBLY_DISPLAYVERSION_KEY = new object();
        private static object PAGE_ASSEMBLY_DISPLAYDEVICE_KEY = new object();
        public string requestedUrl { get; set; }

        /// <summary>
        /// Gets the IPageAssemblyInstruction derived object instance that can be used to build up the 
        /// page for the current request.
        /// </summary>
        public IPageAssemblyInstruction PageAssemblyInstruction { get; private set; }

        /// <summary>
        /// Gets or sets the display version web,print etc.
        /// </summary>
        /// <value>The display version.</value>
        public DisplayVersions DisplayVersion 
        {
            get
            {
                return PageAssemblyContext.CurrentDisplayVersion;
            }
            private set
            {
                PageAssemblyContext.CurrentDisplayVersion = value;
            } 
        }

        /// <summary>
        /// Gets or sets the page template info like the diplay version,page template path to be loaded and the stylesheet path to be applied.
        /// </summary>
        /// <value>The page template info.</value>
        public PageTemplateInfo PageTemplateInfo { get; private set; }

        /// <summary>
        /// Gets the production state of the current site.
        /// </summary>
        /// <value>The display version.</value>
        public bool IsProd
        {
            get
            {
                string prodHost = ProductionHostConfig.Hostname;
                string requestHost = HttpContext.Current.Request.Url.Host;
                log.DebugFormat("IsProd(): Prod hostname = {0}, request hostname = {1}", prodHost, requestHost);
                return !String.IsNullOrEmpty(prodHost) && String.Compare(prodHost, requestHost, StringComparison.OrdinalIgnoreCase) == 0;
            }
        }

        /// <summary>
        /// Private constructor to prevent external creation of PageAssemblyContext instances.
        /// </summary>
        private PageAssemblyContext() { }

        #region Public Properties
        public static DisplayVersions CurrentDisplayVersion
        {
            get 
            {
                if (HttpContext.Current.Items.Contains(PAGE_ASSEMBLY_DISPLAYVERSION_KEY))
                    return (DisplayVersions)HttpContext.Current.Items[PAGE_ASSEMBLY_DISPLAYVERSION_KEY];
                else
                    return DisplayVersions.Web;
            }
            set 
            {
                if (HttpContext.Current.Items.Contains(PAGE_ASSEMBLY_DISPLAYVERSION_KEY))
                    HttpContext.Current.Items[PAGE_ASSEMBLY_DISPLAYVERSION_KEY] = value;
                else
                    HttpContext.Current.Items.Add(PAGE_ASSEMBLY_DISPLAYVERSION_KEY, value);
            }
        }
        public static DisplayDevices DisplayDevice
        {
            get
            {
                if (HttpContext.Current.Items.Contains(PAGE_ASSEMBLY_DISPLAYDEVICE_KEY))
                    return (DisplayDevices)HttpContext.Current.Items[PAGE_ASSEMBLY_DISPLAYVERSION_KEY];
                else
                    HttpContext.Current.Items[PAGE_ASSEMBLY_DISPLAYVERSION_KEY] = DisplayDeviceDetector.DisplayDevice;
                return (DisplayDevices)HttpContext.Current.Items[PAGE_ASSEMBLY_DISPLAYVERSION_KEY];
            }
        }



        #endregion

        public static PageAssemblyContext Current
        {
            get
            {
                if (HttpContext.Current.Items.Contains(PAGE_ASSEMBLY_CONTEXT_KEY) == false)
                {
                    HttpContext.Current.Items.Add(PAGE_ASSEMBLY_CONTEXT_KEY, new PageAssemblyContext());
                }

                return (PageAssemblyContext)HttpContext.Current.Items[PAGE_ASSEMBLY_CONTEXT_KEY];
            }
        }


        /// <summary>
        /// Stores the current page assembly instruction object in the context so it can be made 
        /// available in one central location.
        /// </summary>
        /// <param name="info"></param>
        public void InitializePageAssemblyInfo(IPageAssemblyInstruction info, DisplayVersions displayVersion, PageTemplateInfo pageTemplateInfo,string requestedPath)
        {
            if (PageAssemblyInstruction != null)
            {
                throw new Exception("You cannot initialize the page assembly context with more than one IPageAssemblyInfo instance.");
            }
            else
            {
                PageAssemblyInstruction = info;                                
                PageAssemblyContext.Current.DisplayVersion = displayVersion;
                PageAssemblyContext.Current.PageTemplateInfo = pageTemplateInfo;
                PageAssemblyContext.Current.requestedUrl = requestedPath;

            }
        }


    }
}
