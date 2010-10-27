using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CancerGov.CDR.DataManager;

namespace CancerGov.CDR.ClinicalTrials.Search
{
    /// <summary>
    /// Helper class to load the set of preferred names required for the
    /// ClinicalTrialsLink page.
    /// </summary>
    public class CTSearchLinkPreferredNames
    {
        string _preferredName;
        int _identifier;
        ClinicalTrialSearchLinkIdType _nameType;
        int _cancerTypeID;
        string _cancerTypeName;
        int _cancerSubtypeID;
        string _cancerSubtypeName;

        #region Properties

        /// <summary>
        /// The preferred name for a clinical trial search value.  The type of entity
        /// the name belongs to is contained in the value of the NameType property.
        /// </summary>
        public string PreferredName
        {
            get { return _preferredName; }
        }

        /// <summary>
        /// The Numeric ID of the item specified by the PreferredName property.
        /// The type of entity the ID corresponds to is specified in the
        /// value of the NameType property.
        /// </summary>
        public int Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// An enumerated value specifying the type of entity identified
        /// by the PreferredName and Identifier properties.
        /// </summary>
        public ClinicalTrialSearchLinkIdType NameType
        {
            get { return _nameType; }
        }

        /// <summary>
        /// Numeric identifier for a cancer type.
        /// </summary>
        public int CancerTypeID
        {
            get { return _cancerTypeID; }
        }

        /// <summary>
        /// Preferred name for a cancer type.
        /// </summary>
        public string CancerTypeName
        {
            get { return _cancerTypeName; }
        }

        /// <summary>
        /// Numeric identifer for a cancer subtype.
        /// </summary>
        public int CancerSubtypeID
        {
            get { return _cancerSubtypeID; }
        }

        /// <summary>
        /// Preferred name for a cancer subtype.
        /// </summary>
        public string CancerSubtypeName
        {
            get { return _cancerSubtypeName; }
        }

        #endregion

        public CTSearchLinkPreferredNames(string preferredName, int identifier, ClinicalTrialSearchLinkIdType nameType,
            int cancerTypeID, string cancerTypeName, int cancerSubtypeID, string cancerSubtypeName)
        {
            _preferredName = preferredName;
            _identifier = identifier;
            _nameType = nameType;
            _cancerTypeID = cancerTypeID;
            _cancerTypeName = cancerTypeName;
            _cancerSubtypeID = cancerSubtypeID;
            _cancerSubtypeName = cancerSubtypeName;
        }
    }
}
