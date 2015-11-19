using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCI.Search.Configuration;
using System.Configuration;
using System.Web.Configuration;
using System.Configuration.Provider;

namespace NCI.Search
{
    /// <summary>
    /// Provides a static interface from getting SiteWideSearch results from a configured search engine.
    /// </summary>
    public static class SiteWideSearch
    {
        //Initialization related variables and logic
        private static bool _isInitialized = false;
        private static bool _initializedDefaultProvider = false;
        private static Exception _initializationException = null;
        private static object _initializationLock = new object();

        private static SiteWideSearchProviderBase _defaultProvider;
        private static SiteWideSearchProviderCollection _providerCollection;

        /// <summary>
        /// Gets the default configured SiteWide search provider
        /// </summary>
        public static SiteWideSearchProviderBase Provider
        {
            get
            {
                Initialize();
                if (_defaultProvider == null)
                {
                    throw new InvalidOperationException("Default SiteWideSearchProvider could not be found");
                }
                return _defaultProvider;
            }
        }

        /// <summary>
        /// Gets a collection of the configured SiteWide Search Providers
        /// </summary>
        public static SiteWideSearchProviderCollection Providers
        {
            get
            {
                Initialize();
                return _providerCollection;
            }
        }

        /// <summary>
        /// Gets the search results from the default SiteWide search provider.
        /// </summary>
        /// <param name="searchCollection">The search collection.</param>
        /// <param name="term">The term.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>A SearchResultCollection representing the search results.</returns>
        public static ISiteWideSearchResultCollection GetSearchResults(string searchCollection, string term, int pageSize, int offset)
        {
            return Provider.GetSearchResults(searchCollection, term, pageSize, offset);
        }




        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private static void Initialize()
        {

            //Check if we have been initialized before.
            if (_isInitialized && _initializedDefaultProvider)
            {
                return;
            }

            //If we did initialize and there was an exception,
            //rethrow the exception
            if (_initializationException != null)
                throw _initializationException;

            lock (_initializationLock)
            {
                //Check if we have been initialized before.
                if (_isInitialized && _initializedDefaultProvider)
                {
                    return;
                }

                //If we did initialize and there was an exception,
                //rethrow the exception
                if (_initializationException != null)
                    throw _initializationException;




                //Now actually do the initialization
                try
                {
                    //Get the feature's configuration info
                    SiteWideSearchConfiguration searchProviderConfig =
                        (SiteWideSearchConfiguration)ConfigurationManager.GetSection("SiteWideSearch");

                    if (searchProviderConfig == null)
                        throw new ProviderException("SiteWideSearchProviders section cannot be found in configuration file.");

                    //Instantiate the providers
                    _providerCollection = new SiteWideSearchProviderCollection();

                    //"InitializeSettings"
                    ProvidersHelper.InstantiateProviders(
                        searchProviderConfig.Providers,
                        _providerCollection,
                        typeof(SiteWideSearchProviderBase)
                    );

                    //Don't Allow this to be written to anymore now that it is loaded
                    _providerCollection.SetReadOnly();

                    if (searchProviderConfig.DefaultProvider == null || _providerCollection.Count < 1)
                        throw new ProviderException("Default SiteWideSearchProvider could not be found");

                    _defaultProvider = _providerCollection[searchProviderConfig.DefaultProvider];

                    if (_defaultProvider == null)
                    {
                        throw new ConfigurationErrorsException(
                            "SiteWideSearch Providers: Default provider could not be found.",
                            searchProviderConfig.ElementInformation.Properties["defaultProvider"].Source,
                            searchProviderConfig.ElementInformation.Properties["defaultProvider"].LineNumber);
                    }

                }
                catch (Exception ex)
                {
                    _initializationException = ex;
                    throw;
                }

                //Set it as initialized
                _isInitialized = true;
                _initializedDefaultProvider = true;
            }
        }

    }
}
