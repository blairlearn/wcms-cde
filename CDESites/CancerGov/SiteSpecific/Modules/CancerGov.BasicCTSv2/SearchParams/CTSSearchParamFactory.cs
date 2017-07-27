using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCI.Web;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Class is a factory that when given a URL will return a populated CTSSearchParam object
    /// </summary>
    public class CTSSearchParamFactory
    {

        /// <summary>
        /// Gets an instance of a CTSSearchParams object based on params in URL.
        /// </summary>
        /// <param name="url">The URL to parse</param>
        /// <returns></returns>
        public CTSSearchParams Create(string url)
        {
            CTSSearchParams rtnParams = new CTSSearchParams();

            NciUrl reqUrl = new NciUrl();
            reqUrl.SetUrl(url);

            ParseKeyword(reqUrl, rtnParams);

            return rtnParams; 
        }

        //Parameter q
        private void ParseKeyword(NciUrl url, CTSSearchParams searchParams)
        {
            if (url.QueryParameters.ContainsKey("q"))
            {
                //TODO: Clean Param
                searchParams.Phrase = url.QueryParameters["q"];                
            }
        }
    }
}
