using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

using NCI.Logging;

namespace NCI.Services.Dictionary.Handler
{
    /// <summary>
    /// Base class for invoking service parameters.
    /// </summary>
    abstract class Invoker
    {
        static Log log = new Log(typeof(Invoker));

        protected NameValueCollection RawParams { get; private set; }

        protected Invoker(HttpRequest request)
        {
            RawParams = request.QueryString;
        }

        public static Invoker Create(ApiMethodType method, HttpRequest request)
        {
            Invoker invoker;

            switch (method)
            {
                case ApiMethodType.GetTerm:
                    invoker = new GetTermInvoker(request);
                    break;
                case ApiMethodType.Search:
                    invoker = new SearchInvoker(request);
                    break;
                case ApiMethodType.SearchSuggest:
                    invoker = new SearchSuggestInvoker(request);
                    break;
                case ApiMethodType.Expand:
                    invoker = new ExpandInvoker(request);
                    break;

                case ApiMethodType.Unknown:
                default:
                    throw new DictionaryException(method.ToString());
                    break;
            }

            return invoker;
        }
    }
}
