using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NCI.Collections
{
    [Serializable()]
    [XmlRoot("SynonymList")]
    public class SynonymList : List<string>, IXmlSerializable
    {
        const string synonymTagName = "synonym";

        int _id;

        public SynonymList()
        {
        }

        public SynonymList(int id)
        {
            _id = id;
        }

        [XmlAttribute("ID")]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            // At entry, reader is pointing to the root node.
            if (reader.HasAttributes)
            {
                string tempID = reader.GetAttribute("id");
                _id = int.Parse(tempID);
            }

            while(reader.Read())
            {
                if (reader.IsStartElement() &&
                    reader.Name.Equals(synonymTagName))
                {
                    // Skip empty elements
                    if (reader.IsEmptyElement)
                    {
                        Add(string.Empty);
                    }
                    else
                    {
                        Add(reader.ReadString());
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("id", _id.ToString());

            foreach (string entry in this)
            {
                writer.WriteElementString(synonymTagName, entry);
            }
        }

        #endregion
    }
}
