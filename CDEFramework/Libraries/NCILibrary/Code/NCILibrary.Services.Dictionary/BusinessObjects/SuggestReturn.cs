using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    public class SuggestReturn
    {
        public SuggestReturn()
        {
            // Force collection to be non-null.
            Result = new DictionarySuggestion[] { };
        }

        public SuggestReturnMeta Meta { get; set; }
        public DictionarySuggestion[] Result { get; set; }
    }
}
