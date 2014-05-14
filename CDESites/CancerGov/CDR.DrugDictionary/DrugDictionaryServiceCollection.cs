using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace CancerGov.CDR.DrugDictionary
{
    /// <summary>
    /// A Collection class needed to create the array response for WCF. the type in
    /// the list must be of the type that is being serialized. Using the interface
    /// class will cause the WCF not to work.
    /// </summary>
    [Serializable]
    [CollectionDataContract(ItemName = "DrugDictionaryItem")]
    public class DrugDictionaryServiceCollection : List<DrugDictionaryServiceItem>
    {
    }
}
