using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCI.Services.Dictionary.BusinessObjects;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary
{
    [DataContract()]
    public class DictionaryEntryMetadata : IJsonizable
    {
        private int _cdrid = 0;
        private DictionaryType _dictionary = DictionaryType.Unknown;
        private Language _language = Language.Unknown;
        private AudienceType _audience = AudienceType.Unknown;

        // The CDRID of the requested term
        [DataMember(Name = "cdrid")]
        public int CDRID
        {
            get
            {
                return _cdrid;
            }
            set
            {
                _cdrid = value;
            }
        }

        // The type of dictionary of the requested term
        [DataMember(Name = "dictionary")]
        public DictionaryType Dictionary
        {
            get
            {
                return _dictionary;
            }
            set
            {
                _dictionary = value;
            }
        }

        // The language of the requested term
        [DataMember(Name = "language")]
        public Language Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        // The audience of the requested term
        [DataMember(Name = "audience")]
        public AudienceType Audience
        {
            get
            {
                return _audience;
            }
            set
            {
                _audience = value;
            }
        }

        /// <summary>
        /// Hook for storing data members by calling the various AddMember() overloads.
        /// </summary>
        /// <param name="builder">The Jsonizer instance to use for storing data members.</param>
        public void Jsonize(Jsonizer builder)
        {
            builder.AddMember("cdrid", CDRID.ToString(), true);
            builder.AddMember("dictionary", Dictionary.ToString(), true);
            builder.AddMember("language", Language.ToString(), true);
            builder.AddMember("audience", Audience.ToString(), true);
        }
    }
}
