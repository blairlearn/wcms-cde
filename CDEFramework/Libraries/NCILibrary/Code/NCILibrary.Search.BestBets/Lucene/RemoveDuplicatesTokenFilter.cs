using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucene.Net.Analysis.Miscellaneous
{
    /// <summary>
    /// A TokenFilter which filters out Tokens at the same position and Term text as the previous token in the stream.
    /// </summary>
    public sealed class RemoveDuplicatesTokenFilter : TokenFilter
    {
        //private readonly CharTermAttribute termAttribute = addAttribute(typeof(CharTermAttribute));
        //private readonly PositionIncrementAttribute posIncAttribute = addAttribute(typeof(PositionIncrementAttribute));

        // use a fixed version, as we don't care about case sensitivity.
        //private readonly CharArraySet previous = new CharArraySet(Lucene.Net.Util.Version.LUCENE_30, 8, false);

        /// <summary>
        /// Creates a new RemoveDuplicatesTokenFilter
        /// </summary>
        /// <param name="in"> TokenStream that will be filtered </param>
        public RemoveDuplicatesTokenFilter(TokenStream @in)
            : base(@in)
        {
            termAtt = AddAttribute<ITermAttribute>();
            posIncAtt = AddAttribute<IPositionIncrementAttribute>();
        }

        private readonly ITermAttribute termAtt;
        private readonly IPositionIncrementAttribute posIncAtt;

        //Store terms encountered
        private readonly List<string> previous = new List<string>();

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public override bool IncrementToken()
        {
            int skippedPositions = 0;

            while (input.IncrementToken())
            {
                string term = new string(termAtt.TermBuffer(), 0, termAtt.TermLength());

                if (!previous.Contains(term))
                {
                    previous.Add(term);
                    posIncAtt.PositionIncrement = posIncAtt.PositionIncrement + skippedPositions;
                    return true;
                }
                skippedPositions += posIncAtt.PositionIncrement;

            }
            return false;
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            previous.Clear();
        }
    }
}
