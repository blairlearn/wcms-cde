using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.Index;


namespace NCI.Search.BestBets
{
    /// <summary>
    /// Handles the retrieval of search results from the index.
    /// </summary>
    public static class BestBetsManager
    {
        /// <summary>
        /// Gets the Best Bets Categories for a given term
        /// </summary>
        /// <param name="term">The term to search</param>
        public static BestBetResult[] Search(string term)
        {
            List<BestBetResult> includedResults = new List<BestBetResult>();


            string cleanTerm = BestBetUtils.CleanTerm(term);
            List<string> tokenizedString = BestBetUtils.TokenizeStringStandard(cleanTerm);

            Searcher searcher = Index.BestBetsIndex.Instance.GetSearcher();

            Query q = BuildQuery(tokenizedString);

            TopDocs docs = searcher.Search(q, null, searcher.MaxDoc);


            List<string> excludedIDs = new List<string>();

            // Loop through results and process each result.
            foreach (var res in docs.ScoreDocs)
            {
                //Fetch the doc from the index.
                var doc = searcher.Doc(res.Doc);

                //int docID = res.Doc;
                string categoryID = doc.Get("cat_id");
                string categoryName = doc.Get("cat_name");
                bool isExactMatch = bool.Parse(doc.Get("is_exact"));                
                bool isExclusion = bool.Parse(doc.Get("is_exclude"));
                string terms = doc.Get("terms");
                int numTerms = int.Parse(doc.Get("term_count"));
                //Score = res.Score

                //If we do not match exactly, don't add this
                if (isExactMatch && (numTerms != tokenizedString.Count))
                    continue;

                if (isExclusion)
                {
                    if (!excludedIDs.Contains(categoryID))
                    {
                        excludedIDs.Add(categoryID);
                    }

                    includedResults.RemoveAll(item => item.CategoryID == categoryID);
                }
                else
                {
                    if (!includedResults.Exists(item => item.CategoryID == categoryID) && !excludedIDs.Contains(categoryID))
                    {
                        includedResults.Add(new BestBetResult()
                        {
                            CategoryID = categoryID,
                            CategoryName = categoryName
                        });
                    }
                }
            }


            return includedResults.ToArray();
        }



        #region Permutations Expansion

        private static string[][] BuildNList(int num, params string[] terms)
        {
            List<string[]> rtnItems = new List<string[]>();

            while (terms.Length >= num)
            {
                //Build it with N
                rtnItems.AddRange(internalBuildNList(terms.Take(num - 1).ToArray(), terms.Skip(num - 1).ToArray()));

                terms = terms.Skip(1).ToArray();
            }

            return rtnItems.ToArray();
        }

        private static string[][] internalBuildNList(string[] firstTerms, params string[] remainingTerms)
        {
            List<string[]> rtnStrings = new List<string[]>();

            foreach (string additionalTerm in remainingTerms)
            {
                List<string> thisArr = new List<string>(firstTerms);
                thisArr.Add(additionalTerm);
                rtnStrings.Add(thisArr.ToArray());
            }

            return rtnStrings.ToArray();
        }

        #endregion

        private static Query BuildQuery(List<string> tokenizedString)
        {

            //This is the overall Boolean query
            BooleanQuery q = new BooleanQuery();

            //This handles each individual word
            // A
            // B
            // C
            foreach (string term in tokenizedString)
            {
                q.Add(GetAndedTerms(term), Occur.SHOULD);
            }

            //this handles All the words
            if (tokenizedString.Count >= 2)
            {
                for (int i = 2; i <= tokenizedString.Count; i++)
                {
                    string[][] terms = BuildNList(i, tokenizedString.ToArray());
                    foreach (string[] termset in terms)
                    {
                        q.Add(GetAndedTerms(termset), Occur.SHOULD);
                    }
                }
            }

            return q;
        }

        private static BooleanQuery GetAndedTerms(params string[] terms)
        {
            BooleanQuery termsQuery = new BooleanQuery();

            //Make each term of terms AND
            foreach (string term in terms)
            {
                TermQuery tq = new TermQuery(new Term("terms", term));
                termsQuery.Add(tq, Occur.MUST);
            }

            //We must match all the terms within that query
            termsQuery.Add(NumericRangeQuery.NewIntRange("term_count", terms.Length, terms.Length, true, true), Occur.MUST);

            return termsQuery;
        }


    }
}
