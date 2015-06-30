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
        String Param1 { get; set; }

        [DataMember]
        String Param2 { get; set; }
    }
}
