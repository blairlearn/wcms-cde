using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary
{
    /// <summary>
    /// Thrown when input validation fails for the dictionary services.
    /// </summary>
    [global::System.Serializable]
    public class DictionaryValidationException : Exception
    {
        public DictionaryValidationException() { }
        public DictionaryValidationException(string message) : base(message) { }
        public DictionaryValidationException(string message, Exception inner) : base(message, inner) { }
        protected DictionaryValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Thrown when a dictionary request specifies a language the dictionary doesn't support.
    /// </summary>
    public class UnsupportedLanguageException : DictionaryValidationException
    {
        public UnsupportedLanguageException() { }

        /// <summary>
        /// Overloaded constructor to supply the "unsupported language" message in a
        /// standard format.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="dictionary"></param>
        public UnsupportedLanguageException(Language language, DictionaryType dictionary) :
            this(String.Format("{0} is not a supported language for the {1} dictionary.",
                language, dictionary))
        {
        }

        public UnsupportedLanguageException(string message) : base(message) { }
        public UnsupportedLanguageException(string message, Exception inner) : base(message, inner) { }
        protected UnsupportedLanguageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}
