using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CancerGov.Common.HashMaster;

namespace CancerGov.CDR.ClinicalTrials.Helpers
{
    public class CTLookupListItem
    {
        const string DELIMITER = "~";
        string _hashedCDRID;
        string _CDRIDonly;
        string _name;

        string _displayName;

        public CTLookupListItem(string cdrid, string name, string displayName)
        {

            _CDRIDonly = cdrid;
            _hashedCDRID = HashMaster.SaltedHashCompoundString(name, _CDRIDonly);
            _name = name;
            _displayName = displayName;

        }

        public string Name
        {
            get { return _name; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public string HashedCDRID
        {
            get { return _hashedCDRID; }
        }

        public string CDRIDonly
        {
            get { return _CDRIDonly; }
        }
    }
}
