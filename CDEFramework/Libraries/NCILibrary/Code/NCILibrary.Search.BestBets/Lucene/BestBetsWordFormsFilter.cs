using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Analysis;

namespace NCI.Search.BestBets.Lucene
{
    public class BestBetsWordFormsFilter: TokenFilter
    {
		public BestBetsWordFormsFilter(TokenStream @in)
			: base(@in)
		{
            termAtt = AddAttribute<ITermAttribute>();
		}
		
		private readonly ITermAttribute termAtt;

        /// <summary>
        /// Basically is called on each token
        /// </summary>
        /// <returns></returns>
		public override bool IncrementToken()
		{
			if (input.IncrementToken())
			{

                string term = new String(termAtt.TermBuffer(), 0, termAtt.TermLength());
                string replaceterm = BestBetsWordForms.ResourceManager.GetString(term);

                if (!String.IsNullOrEmpty(replaceterm))
                {
                    //Replace Term was a match, so we need to replace this item.
                    termAtt.SetTermBuffer(replaceterm);
                }
                
	
				char[] buffer = termAtt.TermBuffer();
                
                /*
				int length = termAtt.TermLength();
				for (int i = 0; i < length; i++)
					buffer[i] = System.Char.ToLower(buffer[i]);
				*/

				return true;
			}
			return false;
		}
    }
}
