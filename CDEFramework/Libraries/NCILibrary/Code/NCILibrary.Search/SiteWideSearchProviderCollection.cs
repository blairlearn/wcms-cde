using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search
{

    /// <summary>
    /// Represents a collection of SiteWide Search Provider objects that inherit from SiteWideSearchProviderBase
    /// </summary>
    public class SiteWideSearchProviderCollection : ProviderCollection
    {

        /// <summary>
        /// Adds a SiteWideSearch provider to the collection.
        /// </summary>
        /// <param name="provider">The provider to be added.</param>
        /// <exception cref="System.ArgumentNullException">The provider parameter cannot be null.</exception>
        /// <exception cref="System.ArgumentException">The provider parameter must be of type SiteWideSearchProvider.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        /// </PermissionSet>
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("The provider parameter cannot be null.");

            if (!(provider is SiteWideSearchProviderBase))
                throw new ArgumentException("The provider parameter must be of type SiteWideSearchProvider.");

            base.Add(provider);
        }

        /// <summary>
        /// Determines if this SiteWideSearchProviderCollection contains a configured provider
        /// </summary>
        /// <param name="name">The name of the provider</param>
        /// <returns>True if the provider is configured, false if not</returns>
        public bool Contains(string name)
        {
            foreach (ProviderBase provider in this)
            {
                if (provider.Name == name)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the SiteWide Search Provider with the configured name.
        /// </summary>
        /// <param name="name">The name of the configured provider</param>
        /// <returns>The sitewide search provider </returns>
        new public SiteWideSearchProviderBase this[string name]
        {
            get { return (SiteWideSearchProviderBase)base[name]; }
        }

        /// <summary>
        /// Copies the contents of the collection to the given array starting at the specified index
        /// </summary>
        /// <param name="array">The array to copy the elements of the collection to</param>
        /// <param name="index">The index of the collection item at which to start the copying process</param>
        public void CopyTo(SiteWideSearchProviderBase[] array, int index)
        {
            base.CopyTo(array, index);
        }

    }
}
