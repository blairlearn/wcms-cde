using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    /// <summary>
    /// Defines the overall data structure for returns from an individual
    /// Term lookup.
    /// </summary>
    public class TermReturn
    {
        public TermReturnMeta Meta { get; set; }

        public DictionaryTerm Term { get; set; }
    }
}
