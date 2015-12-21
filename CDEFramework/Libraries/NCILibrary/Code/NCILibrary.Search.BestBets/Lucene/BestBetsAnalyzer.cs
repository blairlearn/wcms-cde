using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

using Version = Lucene.Net.Util.Version;
using Lucene.Net.Analysis.Miscellaneous;

namespace NCI.Search.BestBets.Lucene
{
    public class BestBetsAnalyzer: Analyzer
    {
        private ISet<string> stopSet;
        private readonly Version matchVersion;

        /// <summary>Builds the named analyzer with no stop words. </summary>
        public BestBetsAnalyzer(Version matchVersion)
        {
            SetOverridesTokenStreamMethod<BestBetsAnalyzer>();
            this.matchVersion = matchVersion;
        }

        /// <summary>
        /// Builds the named analyzer with the given stop words.
        /// </summary>
        public BestBetsAnalyzer(Version matchVersion, ISet<string> stopWords)
            : this(matchVersion)
        {
            stopSet = CharArraySet.UnmodifiableSet(CharArraySet.Copy(stopWords));
        }

        /// <summary>Constructs a <see cref="StandardTokenizer" /> filtered by a <see cref="StandardFilter" />
        ///, a <see cref="LowerCaseFilter" /> and a <see cref="StopFilter" />. 
        /// </summary>
        public override TokenStream TokenStream(System.String fieldName, System.IO.TextReader reader)
        {
            StandardTokenizer tokenStream = new StandardTokenizer(matchVersion, reader);            
            TokenStream result = new StandardFilter(tokenStream);
            result = new LowerCaseFilter(result);
            if (stopSet != null)
                result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(matchVersion), result, stopSet);
            
            //Now, our Stemming filter goes here
            result = new BestBetsWordFormsFilter(result);

            //This will remove duplicate keywords - bad for best bets/term count matching
            result = new RemoveDuplicatesTokenFilter(result);

            return result;
        }

        private class SavedStreams
        {
            internal Tokenizer source;
            internal TokenStream result;
        };

        /* Returns a (possibly reused) {@link StandardTokenizer} filtered by a 
   * {@link StandardFilter}, a {@link LowerCaseFilter}, 
   * a {@link StopFilter}, and a {@link SnowballFilter} */

        public override TokenStream ReusableTokenStream(String fieldName, TextReader reader)
        {
            if (overridesTokenStreamMethod)
            {
                // LUCENE-1678: force fallback to tokenStream() if we
                // have been subclassed and that subclass overrides
                // tokenStream but not reusableTokenStream
                return TokenStream(fieldName, reader);
            }

            SavedStreams streams = (SavedStreams)PreviousTokenStream;
            if (streams == null)
            {
                streams = new SavedStreams();
                streams.source = new StandardTokenizer(matchVersion, reader);
                streams.result = new StandardFilter(streams.source);
                streams.result = new LowerCaseFilter(streams.result);
                if (stopSet != null)
                    streams.result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(matchVersion),
                                                    streams.result, stopSet);
                streams.result = new BestBetsWordFormsFilter(streams.result);
                //This will remove duplicate keywords - bad for best bets/term count matching
                streams.result = new RemoveDuplicatesTokenFilter(streams.result);

                PreviousTokenStream = streams;
            }
            else
            {
                streams.source.Reset(reader);
            }
            return streams.result;
        }

    }
}
