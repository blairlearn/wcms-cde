using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using NCI.Util;

using CancerGov.CDR.ClinicalTrials.Helpers;

namespace CancerGov.CDR.ClinicalTrials.Search
{
    /// <summary>
    /// Helper class to transform a CTSearchDefinition into a structure which
    /// can be serialized to and from XML.
    /// </summary>
    [XmlRoot(ElementName = "ProtocolSearch")]
    public class DisplayCriteria : IXmlSerializable
    {
        #region Fields

        NameList _cancerTypeNames;
        List<int> _cancerTypeIDs;

        NameList _cancerSubtypeNames = null;
        List<int> _cancerSubtypeIDs = null;

        NameList _trialTypeList = null;

        List<KeyValuePair<string, int>> _interventionList = null;
        NameList _investigatorList = null;
        NameList _leadOrganizationList = null;

        // Drug information.
        bool _requireAllDrugsMatch = false;
        NameList _drugList = null;


        // Location-related fields.

        /// <summary>
        /// Legacy searches may contain criteria for multiple location types.
        /// The LocationSearchType value stored in _locationType specifies which
        /// one should be used.
        /// </summary>
        LocationSearchType _locationSearchType = LocationSearchType.None;

        // Location criteria to use when _locationType is LocationSearchType.Zip. 
        string _locationZipCode = null;
        string _locationZipProximity = null;

        // Location criteria to use when _locationType is LocationSearchType.NIH.
        bool _locationNihOnly = false;

        // Location criteria to use when _locationType is LocationSearchType.Institution.
        NameList _locationInstitutionNames = null;

        // Location criteria to use when _locationType is LocationSearchType.LocationCity.
        string _locationCountry = null;
        NameList _locationStateNameList = null;
        string _locationCity = null;

        string _keywords = null;

        NameList _trialPhase = null;

        bool _restrictToRecent = false;

        NameList _specificProtocolIDList = null;

        NameList _specialCategoryList = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor, used for serialization.
        /// </summary>
        public DisplayCriteria()
        {
        }

        /// <summary>
        /// Overloaded constructor for transforming a CTSearchDefinition.
        /// </summary>
        /// <param name="searchDef">A CTSearchDefinition which will be translated for serialization.</param>
        public DisplayCriteria(CTSearchDefinition searchDef)
        {
            // Cancer Type IDs
            if (searchDef.CancerType.Value != 0)
            {
                _cancerTypeIDs = new List<int>();
                _cancerTypeIDs.Add(searchDef.CancerType.Value);
            }

            // Cancer Type names
            if (!string.IsNullOrEmpty(searchDef.CancerType.Key))
            {
                _cancerTypeNames = new NameList();
                _cancerTypeNames.Add(searchDef.CancerType.Key);
            }

            // Cancer Subtypes
            if (searchDef.CancerSubtypeNameList.Count > 0)
            {
                // references for readability.
                List<string> names = searchDef.CancerSubtypeNameList;
                List<int> IDs = searchDef.CancerSubtypeIDList;
                bool lengthsMatch = (names.Count == IDs.Count);
                int listLength = names.Count;

                _cancerSubtypeNames = new NameList();
                _cancerSubtypeIDs = new List<int>();

                for (int i = 0; i < listLength; i++)
                {
                    _cancerSubtypeNames.Add(names[i]);
                    if (lengthsMatch)
                        _cancerSubtypeIDs.Add(IDs[i]);
                    else
                        _cancerSubtypeIDs.Add(-1);
                }
            }

            // Trial Type list
            if (searchDef.TrialTypeList.Count > 0)
                _trialTypeList = new NameList(searchDef.TrialTypeList);

            // Intervention list
            if (searchDef.InterventionList.Count > 0)
                _interventionList = new List<KeyValuePair<string, int>>(searchDef.InterventionList);

            // Investigator List
            if (searchDef.InvestigatorList.Count > 0)
                _investigatorList = new NameList(searchDef.InvestigatorList);

            // Lead Organizations
            if (searchDef.LeadOrganizationList.Count > 0)
                _leadOrganizationList = new NameList(searchDef.LeadOrganizationList);

            // Drug list
            if (searchDef.DrugList.Count > 0)
                _drugList = new NameList(searchDef.DrugList);
            _requireAllDrugsMatch = searchDef.RequireAllDrugsMatch;

            // Location, location, location.
            _locationSearchType = searchDef.LocationSearchType;
            switch (_locationSearchType)
            {
                case LocationSearchType.Zip:
                    if (searchDef.LocationZipCode > 0)
                        _locationZipCode = searchDef.LocationZipCode.ToString();
                    if (searchDef.LocationZipProximity > 0)
                        _locationZipProximity = searchDef.LocationZipProximity.ToString();
                    break;

                case LocationSearchType.City:
                    _locationCountry = searchDef.LocationCountryName;
                    _locationCity = searchDef.LocationCity;
                    _locationStateNameList = new NameList(searchDef.LocationStateNameList);
                    break;

                case LocationSearchType.Institution:
                    _locationInstitutionNames = new NameList(searchDef.LocationInstitutions);
                    break;

                case LocationSearchType.NIH:
                    _locationNihOnly = searchDef.LocationNihOnly;
                    break;

                default:
                    break;
            }

            // Keyword search text.
            _keywords = searchDef.Keywords;

            // Trial phase
            if (searchDef.TrialPhase.Count > 0)
                _trialPhase = new NameList(searchDef.TrialPhase);

            // Past 30 days only?
            _restrictToRecent = searchDef.RestrictToRecent;

            // Alternate Protocol ID list.
            if (searchDef.SpecificProtocolIDList.Count > 0)
                _specificProtocolIDList = new NameList(searchDef.SpecificProtocolIDList);

            // Special Category list.
            if (searchDef.SpecialCategoryList.Count > 0)
                _specialCategoryList = new NameList(searchDef.SpecialCategoryList);
        }

        #endregion

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads a DisplayCriteria object from an XmlReader and
        /// deserializes it back into memory.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                // Detect the end of the structure so we don't fall off the end.
                if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Name == "ProtocolSearch")
                {
                    break;
                }

                if (reader.IsStartElement() || reader.IsEmptyElement)
                {
                    // Detect the nodes which belong at this level.
                    // Separate handling of empty nodes from those which are supposed to have
                    // contents.  Aside from a very minor speed improvement, this also provides
                    // some safety against nodes which are unexpectedly empty.
                    if (!reader.IsEmptyElement)
                    {
                        if (reader.Name == "CancerTypes")
                        {
                            _cancerTypeNames = new NameList();
                            _cancerTypeIDs = new List<int>();
                            ReadElement(reader, "CancerTypes", "CancerType", "ID", _cancerTypeNames, _cancerTypeIDs);
                        }
                        else if (reader.Name == "CancerStages")
                        {
                            _cancerSubtypeNames = new NameList();
                            _cancerSubtypeIDs = new List<int>();
                            ReadElement(reader, "CancerStages", "CancerStage", "ID", _cancerSubtypeNames, _cancerSubtypeIDs);
                        }
                        else if (reader.Name == "Location")
                        {
                            // Handles ZIP and City/State/Country.  The other locations
                            // are nodes at this level.
                            ReadLocationElement(reader);
                        }
                        else if (reader.Name == "Institutions")
                        {
                            _locationSearchType = LocationSearchType.Institution;
                            _locationInstitutionNames = new NameList();
                            ReadElement(reader, "Institutions", "Institution", _locationInstitutionNames);
                        }
                        else if (reader.Name == "ProtocolIDs")
                        {
                            _specificProtocolIDList = new NameList();
                            ReadElement(reader, _specificProtocolIDList);
                        }
                        else if (reader.Name == "Investigators")
                        {
                            _investigatorList = new NameList();
                            ReadElement(reader, "Investigators", "Investigator", _investigatorList);
                        }
                        else if (reader.Name == "LeadOrgs")
                        {
                            _leadOrganizationList = new NameList();
                            ReadElement(reader, "LeadOrgs", "LeadOrg", _leadOrganizationList);
                        }
                        else if (reader.Name == "TrialTypes")
                        {
                            _trialTypeList = new NameList();
                            ReadElement(reader, "TrialTypes", "TrialType", _trialTypeList);
                        }
                        else if (reader.Name == "TrialPhases")
                        {
                            _trialPhase = new NameList();
                            ReadElement(reader, "TrialPhases", "TrialPhase", _trialPhase);
                        }
                        else if (reader.Name == "DrugInfo")
                        {
                            ReadDrugElement(reader);
                        }
                        else if (reader.Name == "Interventions")
                        {
                            _interventionList = new List<KeyValuePair<string, int>>();
                            ReadElement(reader, "Interventions", "Intervention", "ID", _interventionList);
                        }
                        else if (reader.Name == "SpecialCategorys")
                        {
                            _specialCategoryList = new NameList();
                            ReadElement(reader, "SpecialCategorys", "SpecialCategory", _specialCategoryList);
                        }
                        else if (reader.Name == "Keywords")
                        {
                            ReadElement(reader, ref _keywords);
                        }
                    }
                    else // Handling for nodes which are supposed to be empty.
                    {
                        if (reader.Name == "NewTrials")
                        {
                            // Empty element.
                            _restrictToRecent = true;
                        }
                        else if (reader.Name == "NIHClinicalCenter")
                        {
                            // Empty Node.  Only present for location type of NIH with a value.
                            _locationSearchType = LocationSearchType.NIH;
                            _locationNihOnly = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Serializes the DisplayCriteria object to an XmlWriter as part of the
        /// IXmlSerializable interface.
        /// </summary>
        /// <param name="writer">Stream for outtputting the serialized object.</param>
        public void WriteXml(XmlWriter writer)
        {
            // Outermost ProtocolSearch tag is controlled by the class' XmlRoot attribute.

            StringBuilder sb = new StringBuilder();
            string format;

            // Cancer type name(s) and ID(s)
            if ((_cancerTypeNames != null && _cancerTypeNames.Count > 0) ||
                (_cancerTypeIDs != null && _cancerTypeIDs.Count > 0))
            {
                // Cancer type name(s) and ID(s)
                format = string.Empty;
                if (_cancerTypeIDs != null)
                {
                    foreach (int id in _cancerTypeIDs)
                    {
                        // On first pass, arg 0 is empty.
                        sb.AppendFormat("{0}{1}", format, id);
                        format = ", ";
                    }
                }
                writer.WriteStartElement("CancerTypes");

                writer.WriteStartElement("CancerType");
                writer.WriteAttributeString("ID", sb.ToString());
                if (_cancerTypeNames != null)
                    writer.WriteString(_cancerTypeNames.ToString());
                writer.WriteEndElement();   // CancerType

                writer.WriteEndElement();   // CancerTypes
            }

            // Cancer stage name(s) and ID(s).
            // 
            if (_cancerSubtypeNames != null && _cancerSubtypeNames.Count>0 &&
                _cancerSubtypeIDs!=null && _cancerSubtypeIDs.Count>0)
            {
                writer.WriteStartElement("CancerStages");

                int length = _cancerSubtypeNames.Count;
                for (int i = 0; i < length; i++)
                {
                    WriteElement(writer, "CancerStage", _cancerSubtypeNames[i], "ID", _cancerSubtypeIDs[i].ToString("d"));
                }

                writer.WriteEndElement();   // CancerStages
            }

            // Past 30 days.
            if (_restrictToRecent)
                WriteElement(writer, "NewTrials");      // This node has no value.

            // Location flags)
            if (_locationSearchType != LocationSearchType.None)
            {
                switch (_locationSearchType)
                {
                    case LocationSearchType.Zip:
                        writer.WriteStartElement("Location");

                        writer.WriteStartElement("ZipCode");
                        writer.WriteAttributeString("Proximity", _locationZipProximity);
                        writer.WriteString(_locationZipCode);
                        writer.WriteEndElement();

                        writer.WriteEndElement();   // Location
                        break;
                    case LocationSearchType.Institution:
                        WriteElement(writer, "Institutions", "Institution", _locationInstitutionNames);
                        break;
                    case LocationSearchType.City:
                        writer.WriteStartElement("Location");

                        WriteElement(writer, "City", _locationCity);
                        WriteElement(writer, "States", "State", _locationStateNameList);
                        WriteElement(writer, "Country", _locationCountry);
                        
                        writer.WriteEndElement();   // Location
                        break;
                    case LocationSearchType.NIH:
                        WriteElement(writer, "NIHClinicalCenter");      // This node has no value.
                        break;

                    default:
                        break;
                }
            }

            // List of alternate protocol IDs.
            WriteElement(writer, "ProtocolIDs", _specificProtocolIDList);

            // Investigators
            WriteElement(writer, "Investigators", "Investigator", _investigatorList);

            // Lead Organizations
            WriteElement(writer, "LeadOrgs", "LeadOrg", _leadOrganizationList);

            // Trial Types
            WriteElement(writer, "TrialTypes", "TrialType", _trialTypeList);

            // Trial Phases
            WriteElement(writer, "TrialPhases", "TrialPhase", _trialPhase);

            // Drug list
            if (_drugList != null && _drugList.Count > 0)
            {
                writer.WriteStartElement("DrugInfo");
                writer.WriteAttributeString("DrugFormula", _requireAllDrugsMatch ? "and" : "or");
                WriteElement(writer, "Drugs", "Drug", _drugList);
                writer.WriteEndElement();   //DrugInfo
            }

            // Intervention list
            WriteElement(writer, "Interventions", "Intervention", _interventionList);

            // Special categorys
            WriteElement(writer, "SpecialCategorys", "SpecialCategory", _specialCategoryList);

            // Keyword text
            WriteElement(writer, "Keywords", _keywords);
        }

        #endregion

        #region IXmlSerializable Helper methods.

        private void ReadLocationElement(XmlReader reader)
        {
            // Walk through the substructure.
            while (reader.Read())
            {
                // Detect the end of the structure so we don't fall off the end.
                if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Name == "Location")
                {
                    break;
                }

                if ((reader.IsStartElement() && !reader.IsEmptyElement)) // Must be non-empty
                {
                    // Since any given element might be missing, we have to set the
                    // location type on every branch.
                    if (reader.Name == "City")
                    {
                        _locationSearchType = LocationSearchType.City;
                        ReadElement(reader, ref _locationCity);
                    }
                    else if (reader.Name == "Country")
                    {
                        _locationSearchType = LocationSearchType.City;
                        ReadElement(reader, ref _locationCountry);
                    }
                    else if (reader.Name == "States")
                    {
                        _locationSearchType = LocationSearchType.City;
                        _locationStateNameList = new NameList();
                        ReadElement(reader, "States", "State", _locationStateNameList);
                    }
                    if (reader.Name == "ZipCode")
                    {
                        _locationSearchType = LocationSearchType.Zip;
                        _locationZipProximity = reader.GetAttribute("Proximity");
                        ReadElement(reader, ref _locationZipCode);
                    }
                }
            }
        }

        private void ReadDrugElement(XmlReader reader)
        {
            string idAttribute = reader.GetAttribute("DrugFormula");
            _requireAllDrugsMatch = (idAttribute.ToLower() == "and");

            // Walk through the substructure.
            while (reader.Read())
            {
                // Detect the end of the structure so we don't fall off the end.
                if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Name == "DrugInfo")
                {
                    break;
                }

                // Look for inner elements
                if (reader.Name == "Drugs")
                {
                    _drugList = new NameList();
                    ReadElement(reader, "Drugs", "Drug", _drugList);
                }
            }
        }


        /// <summary>
        /// Reads the values contained in an XML structure.
        /// </summary>
        /// <param name="reader">Stream containing the XML structure which is being read.</param>
        /// <param name="outerElementName">Name of the structure's outermost element.</param>
        /// <param name="innerElementName">Name of the structur's inner tag.</param>
        /// <param name="idAttributeName">Name of the attribute containing ID values.</param>
        /// <param name="textValueList">Reference to a list for storing the structure's text values.</param>
        /// <param name="idValueList">Reference to a list for storing the structure's ID values.</param>
        private void ReadElement(XmlReader reader, string outerElementName, string innerElementName, string idAttributeName,
            List<String> textValueList, IList<int> idValueList)
        {
            // Walk through the substructure.
            while (reader.Read())
            {
                // Detect the end of the structure so we don't fall off the end.
                if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Name == outerElementName)
                {
                    break;
                }

                if ((reader.IsStartElement() &&         // Must be non-empty
                    !reader.IsEmptyElement) &&
                    reader.Name == innerElementName)
                {
                    // Get any ID values.
                    string idAttribute = reader.GetAttribute(idAttributeName);
                    string[] idValues;
                    if (!string.IsNullOrEmpty(idAttribute))
                    {
                        idValues = idAttribute.Split(',');
                        foreach (string value in idValues)
                        {
                            try
                            {
                                idValueList.Add(Convert.ToInt32(value));
                            }
                            catch (Exception ex) { }
                        }
                    }

                    // Retrieve the node contents.
                    textValueList.Add(reader.ReadString());
                }
            }
        }

        /// <summary>
        /// Reads the values contained in an XML structure.
        /// </summary>
        /// <param name="reader">Stream containing the XML structure which is being read.</param>
        /// <param name="outerElementName">Name of the structure's outermost element.</param>
        /// <param name="innerElementName">Name of the structur's inner tag.</param>
        /// <param name="idAttributeName">Name of the attribute containing ID values.</param>
        /// <param name="nameValueList">Reference to a list for storing the structure's text and ID values.</param>
        private void ReadElement(XmlReader reader, string outerElementName, string innerElementName, string attributeName,
            List<KeyValuePair<string, int>> nameValueList)
        {
            // Walk through the substructure.
            while (reader.Read())
            {
                // Detect the end of the structure so we don't fall off the end.
                if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Name == outerElementName)
                {
                    break;
                }

                if ((reader.IsStartElement() &&         // Must be non-empty
                    !reader.IsEmptyElement) &&
                    reader.Name == innerElementName)
                {
                    // Get the node attribute value and contained value
                    int attributeValue = Strings.ToInt(reader.GetAttribute(attributeName));
                    string containedValue = reader.ReadString();

                    nameValueList.Add(new KeyValuePair<string, int>(containedValue, attributeValue));
                }
            }
        }

        /// <summary>
        /// Reads the values contained in an XML structure.
        /// </summary>
        /// <param name="reader">Stream containing the XML structure which is being read.</param>
        /// <param name="outerElementName">Name of the structure's outermost element.</param>
        /// <param name="innerElementName">Name of the structur's inner tag.</param>
        /// <param name="textValueList">Reference to a list for storing the structure's text values.</param>
        private void ReadElement(XmlReader reader, string outerElementName, string innerElementName,
            List<String> textValueList)
        {
            // Walk through the substructure.
            while (reader.Read())
            {
                // Detect the end of the structure so we don't fall off the end.
                if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Name == outerElementName)
                {
                    break;
                }

                if ((reader.IsStartElement() &&         // Must be non-empty
                    !reader.IsEmptyElement) &&
                    reader.Name == innerElementName)
                {
                    // Retrieve the node contents.
                    textValueList.Add(reader.ReadString());
                }
            }
        }

        /// <summary>
        /// Reads the values contained in an XML structure.
        /// </summary>
        /// <param name="reader">Stream containing the XML structure which is being read.</param>
        /// <param name="textValueList">Reference to a list for storing the structure's text values.</param>
        private void ReadElement(XmlReader reader, List<String> textValueList)
        {
            // This implementation only reads from the current node and therefore doesn't
            // include a while loop.  Likewise, because it only deals with a single node,
            // it doesn't need to worry about element names (those are handled by the
            // caller.  It does however have to worry about avoiding attempts to read values
            // from an empty node.
               if (reader.IsStartElement() && !reader.IsEmptyElement)
            {
                string nodeValue = reader.ReadString();
                if (!string.IsNullOrEmpty(nodeValue))
                    textValueList.AddRange(nodeValue.Split(','));
            }
        }

        /// <summary>
        /// Reads the values contained in an XML structure.
        /// </summary>
        /// <param name="reader">Stream containing the XML structure which is being read.</param>
        /// <param name="textValueList">Reference to a list for storing the structure's text values.</param>
        private void ReadElement(XmlReader reader, ref string textValue)
        {
            // This implementation only reads from the current node and therefore doesn't
            // include a while loop.  Likewise, because it only deals with a single node,
            // it doesn't need to worry about element names (those are handled by the
            // caller.  It does however have to worry about avoiding attempts to read values
            // from an empty node.
            if (reader.IsStartElement() && !reader.IsEmptyElement)
            {
                textValue = reader.ReadString();
            }
        }

        /// <summary>
        /// Adds an empty named element to the XmlWriter.
        /// </summary>
        /// <param name="writer">XmlWriter to write the element to.</param>
        /// <param name="elementName">The element name.</param>
        private void WriteElement(XmlWriter writer, string elementName)
        {
                writer.WriteElementString(elementName, null);
        }

        /// <summary>
        /// Writes a single value to an XML node.
        /// </summary>
        /// <param name="writer">XmlWriter to write the element to.</param>
        /// <param name="elementName">The element name.</param>
        /// <param name="value">List of names to write to the XmlWriter.</param>
        private void WriteElement(XmlWriter writer, string elementName, string value)
        {
            if (value != null)
            {
                writer.WriteElementString(elementName, value);
            }
        }

        /// <summary>
        /// Writes a single element with an attribute.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="elementName"></param>
        /// <param name="value"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        private void WriteElement(XmlWriter writer, string elementName, string value, string attributeName, string attributeValue)
        {
            if (value != null)
            {
                writer.WriteStartElement(elementName);
                writer.WriteAttributeString(attributeName, attributeValue);
                writer.WriteString(value.ToString());
                writer.WriteEndElement();   //elementName
            }
        }

        /// <summary>
        /// Adds the values in a Namelist to a single XML node.
        /// </summary>
        /// <param name="writer">XmlWriter to write the element to.</param>
        /// <param name="elementName">The element name.</param>
        /// <param name="list">List of names to write to the XmlWriter.</param>
        private void WriteElement(XmlWriter writer, string elementName, NameList list)
        {
            if (list != null && list.Count > 0)
            {
                writer.WriteElementString(elementName, list.ToString());
            }
        }

        /// <summary>
        /// Adds a nested list of elements to the XmlWriter.
        /// </summary>
        /// <param name="writer">XmlWriter to write the element to.</param>
        /// <param name="outerElementName">Name of the list's outer XML element.</param>
        /// <param name="innerElementName">Name of the list's inner XML element.</param>
        /// <param name="list">List of values to write to the XmlWriter.</param>
        private void WriteElement(XmlWriter writer, string outerElementName, string innerElementName, IList<string> list)
        {
            if (list != null && list.Count > 0)
            {
                writer.WriteStartElement(outerElementName);

                foreach (string item in list)
                {
                    writer.WriteElementString(innerElementName, item);
                }

                writer.WriteEndElement();   // outerElementName
            }
        }

        /// <summary>
        /// Adds a list of name, values pairs to the XmlWriter
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="outerElementName"></param>
        /// <param name="innerElementName"></param>
        /// <param name="nameValueList"></param>
        private void WriteElement(XmlWriter writer, string outerElementName, string innerElementName, List<KeyValuePair<string, int>> nameValueList)
        {
            if (nameValueList != null)
            {
                writer.WriteStartElement(outerElementName);

                foreach (KeyValuePair<string, int> item in nameValueList)
                {
                    WriteElement(writer, innerElementName, item.Key, "ID", item.Value.ToString("d"));
                }
                
                writer.WriteEndElement();   // outerElementName
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of cancer type names.  This will usually be just one entry, but it's possible
        /// some older searches will have multiple.
        /// </summary>
        public NameList CancerTypeNames
        {
            get { return _cancerTypeNames; }
        }

        /// <summary>
        /// List of cancer type IDs.  This will usually be just one entry, but it's possible
        /// some older searches will have multiple.
        /// </summary>
        public IList<int> CancerTypeIDs
        {
            get { return _cancerTypeIDs; }
        }

        /// <summary>
        /// Gets a reference to the list of cancer subtype names selected for the search.
        /// </summary>
        public NameList CancerSubtypeNames
        {
            get { return _cancerSubtypeNames; }
        }

        /// <summary>
        /// Gets a reference to the list of cancer subtype IDs selected for the search.
        /// </summary>
        public List<int> CancerSubtypeIDs
        {
            get { return _cancerSubtypeIDs; }
        }

        /// <summary>
        /// Gets a reference to the list of trial type (Name, ID) pairs selected for the search.
        /// </summary>
        public NameList TrialTypeList
        {
            get { return _trialTypeList; }
        }

        /// <summary>
        /// Gets a reference to the list of drug Name, ID pairs selected for the search.
        /// </summary>
        public NameList DrugList
        {
            get { return _drugList; }
        }

        /// <summary>
        /// Gets a reference to the list of values of the treatments/intervention selected for the search.
        /// </summary>
        public IList<KeyValuePair<string, int>> InterventionList
        {
            get { return _interventionList; }
        }

        /// <summary>
        /// Gets a reference to the list of investigator Name, ID pairs selected for the search.
        /// </summary>
        public NameList InvestigatorList
        {
            get { return _investigatorList; }
        }

        /// <summary>
        /// Gets a reference to the list of lead organization Name, ID pairs selected for the search.
        /// </summary>
        public NameList LeadOrganizationList
        {
            get { return _leadOrganizationList; }
        }

        /// <summary>
        /// Restricts the search based on specific location types.
        /// 
        /// Meaningful values are:
        ///     LocationSearchType.Zip:         Distance from a specific ZIP code.
        ///                                     Only meaningful if LocationZipCode and LocationZipProximity are
        ///                                     also set.
        ///     
        ///     LocationSearchType.Institution:    Specific list of hospitals/institutions.
        ///                                     Only meaningful if LocationInstitutions has a value.
        ///     
        ///     LocationSearchType.City:        Specific City, State, and/or Country.
        ///                                     Only meaningful if LocationCountry, LocationStateIDList and/or
        ///                                     LocationStateNameList have one or more values.
        ///     
        ///     LocationSearchType.NIH:         Only trials taking place at the NIH clinical center.
        ///                                     Only meaningful if LocationNihOnly is set to TRUE.
        ///     
        /// </summary>
        public LocationSearchType LocationSearchType
        {
            get { return _locationSearchType; }
        }

        /// <summary>
        /// Gets a reference to the list of institution Name, ID pairs where trials should be taking place.
        /// 
        /// Only valid when LocationSearchType is set to LocationSearchType.Institution
        /// </summary>
        public NameList LocationInstitutions
        {
            get { return _locationInstitutionNames; }
        }

        /// <summary>
        /// Restricts search results to a specific ZIP code.  A distance from the ZIP code is specified
        /// via LocationZipProximity.
        /// 
        /// Only valid when LocationSearchType is set to LocationSearchType.Zip
        /// </summary>
        public string LocationZipCode
        {
            get { return _locationZipCode; }
        }

        /// <summary>
        /// Restricts search to a specific radius around the value specified for LocationZipCode.
        /// This value is meaningless unless LocationZipCode is also set.
        /// 
        /// Only valid when LocationSearchType is set to LocationSearchType.Zip
        /// </summary>
        public string LocationZipProximity
        {
            get { return _locationZipProximity; }
        }

        /// <summary>
        /// If set TRUE, restricts search results to only those trials taking place
        /// at the NIH Clinical Center.
        /// 
        /// Only valid when LocationSearchType is set to LocationSearchType.NIH
        /// </summary>
        public bool LocationNihOnly
        {
            get { return _locationNihOnly; }
        }

        /// <summary>
        /// Name of the country where search results are being sought.
        /// 
        /// Only valid when LocationSearchType is set to LocationSearchType.City
        /// </summary>
        public string LocationCountry
        {
            get { return _locationCountry; }
        }

        /// <summary>
        /// List of State names.
        /// </summary>
        public NameList LocationStateNameList
        {
            get { return _locationStateNameList; }
        }

        /// <summary>
        /// The name of the city where search results will be centered.
        /// </summary>
        public string LocationCity
        {
            get { return _locationCity; }
        }

        /// <summary>
        /// Specifies whether all entries in the drug list must match.
        /// </summary>
        public bool RequireAllDrugsMatch
        {
            get { return _requireAllDrugsMatch; }
        }

        /// <summary>
        /// The keyword text to search for.
        /// </summary>
        public string Keywords
        {
            get { return _keywords; }
        }

        /// <summary>
        /// List of trial phases.
        /// </summary>
        public NameList TrialPhase
        {
            get { return _trialPhase; }
        }

        /// <summary>
        /// Controls whether the search is restricted to 
        /// new clinical trials only.
        /// </summary>
        public bool RestrictToRecent
        {
            get { return _restrictToRecent; }
        }

        /// <summary>
        /// List of specific protocol IDs to search for
        /// </summary>
        public NameList SpecificProtocolIDList
        {
            get { return _specificProtocolIDList; }
        }

        /// <summary>
        /// List of special protocol category IDs.
        /// </summary>
        public NameList SpecialCategoryList
        {
            get { return _specialCategoryList; }
        }

        #endregion
    }
}
