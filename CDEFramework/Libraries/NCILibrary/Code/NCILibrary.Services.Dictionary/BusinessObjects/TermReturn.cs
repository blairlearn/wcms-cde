using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    public class TermReturn
    {
        public TermReturnMeta Meta { get; set; }
        public DictionaryTerm Term { get; set; }
    }
}
