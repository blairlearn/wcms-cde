using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CancerGov.CDR.DataManager;
using CancerGov.UI.CDR;

namespace CancerGov.CDR.ClinicalTrials.Search
{
    public class CTSearchResultOptions
    {
        ProtocolVersions _audience;
        CTSSortFilters _sortOrder;
        ProtocolDisplayFormats _displayFormat;
        bool _includeStudySites;
        ProtocolSectionTypes[] _protocolSections=null;

        #region Properties

        /// <summary>
        /// Patient versus Health Professional
        /// </summary>
        public ProtocolVersions Audience
        {
            get { return _audience; }
        }

        /// <summary>
        /// Which criteria should the sort be based one?
        /// </summary>
        public CTSSortFilters SortOrder
        {
            get { return _sortOrder; }
        }

        public ProtocolDisplayFormats DisplayFormat
        {
            get { return _displayFormat; }
        }

        public bool IncludeStudySites
        {
            get { return _includeStudySites; }
        }

        public ProtocolSectionTypes[] ProtocolSections
        {
            get { return _protocolSections; }
        }

        #endregion

        public CTSearchResultOptions(ProtocolVersions audience, CTSSortFilters sortOrder,
            ProtocolDisplayFormats displayFormat, ProtocolSectionTypes[] protocolSections, bool includeStudySites)
        {
            _audience = audience;
            _sortOrder = sortOrder;
            _displayFormat = displayFormat;
            _includeStudySites = includeStudySites;

            if (protocolSections != null && protocolSections.Length > 0)
            {
                _protocolSections = (ProtocolSectionTypes[])protocolSections.Clone();
            }
        }
    }
}
