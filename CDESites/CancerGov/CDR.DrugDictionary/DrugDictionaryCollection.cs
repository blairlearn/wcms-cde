using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.CDR.DrugDictionary
{
    /// <summary>
    /// Holds a collection of DrugDictionaryDataItem objects
    /// </summary>
    public class DrugDictionaryCollection : List<DrugDictionaryDataItem>
    {
        public int matchCount;
    }
}
