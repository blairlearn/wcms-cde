using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    public class SearchReturn
    {
        public SearchReturn()
        {
            // Force collection to be non-null.
            Result = new DictionaryTerm[] { };
        }

        public SearchReturnMeta Meta { get; set; }
        public DictionaryTerm[] Result { get; set; }
    }
}
