using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCI.Search.BestBets.Lucene;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Tokenattributes;

namespace NCI.Search.BestBets
{
    public static class BestBetUtils
    {
        public static List<String> TokenizeStringStandard(String s)
        {
            //Analyzer analyzer = new Lucene.Net.Analysis.Snowball.SnowballAnalyzer(Lucene.Net.Util.Version.LUCENE_30, "Porter");
            Analyzer analyzer = new BestBetsAnalyzer(global::Lucene.Net.Util.Version.LUCENE_30);

            return TokenizeString(analyzer, s);
        }

        public static List<String> TokenizeString(Analyzer analyzer, String s)
        {
            List<String> result = new List<String>();
            try
            {
                TokenStream stream = analyzer.TokenStream(null, new StringReader(s));
                stream.Reset();
                var termAttr = stream.GetAttribute<global::Lucene.Net.Analysis.Tokenattributes.ITermAttribute>();
                while (stream.IncrementToken())
                {
                    result.Add(termAttr.Term);
                }
            }
            catch (IOException e)
            {
                // not thrown b/c we're using a string reader...
                throw;
            }
            return result;
        }

        public static string CleanTerm(string term)
        {
            return System.Text.RegularExpressions.Regex.Replace(term, "[-':;\"\\./]", "");
        }
    }
}
