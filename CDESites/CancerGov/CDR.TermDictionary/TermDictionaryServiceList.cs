using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace CancerGov.CDR.TermDictionary
{
    /// <summary> 
    /// This class holds the TermDictionaryServiceCollection and the totalNuber of records.
    /// Basically this class is the response to the GetTermDictionaryList request
    /// </summary>
    [Serializable]
    [DataContract] 
    public class TermDictionaryServiceList
    {
        [DataMember(Name="TermDictionaryItems")]
        public TermDictionaryServiceCollection TermDictionaryServiceCollection
        {
            get;
            set;
        }

        [DataMember]
        public int TotalRecordCount
        {
            get;
            set;
        }
    }
}
