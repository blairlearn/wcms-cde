using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace NCI.Services.Dictionary
{
    [DataContract]
    public class SearchInputs
    {
        [DataMember]
        public String searchText { get; set; }

        [DataMember]
        public String searchType { get; set; }

        [DataMember]
        public int offset { get; set; }

        [DataMember]
        public int maxResults { get; set; }
    }
}
