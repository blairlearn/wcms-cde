using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE;
using NCI.Web;

namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSResultsControl : SnippetControl
    {
        private string _index = "clinicaltrials";
        private string _indexType = "trial";
        private string _clusterName = "SearchCluster";
        private string _templatePath = "~/VelocityTemplates/BasicCTSResults.vm";
        private string _resultsUrl = "/about-cancer/treatment/clinical-trials/basic/view";
        private int _defaultItemsPerPage = 10;
        private int _defaultZipProximity = 50;


        public BaseCTSSearchParam SearchParams { get; private set; }

        private string ParmAsStr(string param, string def)
        {
            string paramval = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramval))
                return def;
            else 
                return paramval;
        }

        private int ParmAsInt(string param, int def)
        {
            string paramval = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramval))
            {
                return def;
            }
            else
            {
                int tmpInt = 0;
                if (int.TryParse(paramval, out tmpInt))
                {
                    return tmpInt;
                } else {
                    return def;
                }                
            }
        }

        private void SetSearchParams()
        {
            //Parse Parameters
            int pageNum = this.ParmAsInt("pn", 1);
            int itemsPerPage = this.ParmAsInt("ni", _defaultItemsPerPage);
            string phrase = this.ParmAsStr("q", string.Empty);
            string zip = this.ParmAsStr("z", string.Empty);
            int zipProximity = this.ParmAsInt("zp", _defaultZipProximity); //In miles
            int age = this.ParmAsInt("a", 0);
            int gender = this.ParmAsInt("g", 0); //0 = decline, 1 = female, 2 = male, 
            string cancerType = this.ParmAsStr("t", string.Empty);

            BaseCTSSearchParam searchParams = null;

            #region Set Cancer Type or Phrase

            if (phrase != string.Empty)
            {
                searchParams = new PhraseSearchParam()
                {
                    Phrase = phrase
                };
            }
            else
            {
                searchParams = new CancerTypeSearchParam()
                {
                    //get cancer type.
                };
            }

            #endregion

            // Fill in common parameters
            //How to handle invalid zip?
            //Need to lookup zip

            #region Set Page and Items Per Page
            searchParams.Page = pageNum;
            searchParams.ItemsPerPage = itemsPerPage;
            #endregion

            #region Set Age

            //Handle Age
            if (age > 0)
            {
                searchParams.Age = age;
            }

            #endregion

            #region Set Gender

            //Handle Gender if specified
            switch (gender)
            {
                case 1: 
                    searchParams.Gender = BaseCTSSearchParam.GENDER_FEMALE;
                    break;
                case 2:
                    searchParams.Gender = BaseCTSSearchParam.GENDER_MALE;
                    break;
            }

            #endregion


            SearchParams = searchParams;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SetSearchParams();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            

            BasicCTSManager basicCTSManager = new BasicCTSManager(_index, _indexType, _clusterName);


            //Do the search
            var results = basicCTSManager.Search(SearchParams);

            // Show Results

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                _templatePath, 
                new
                {
                    Results = results,
                    Control = this
                }
            ));
            Controls.Add(ltl);
        }

        #region Velocity Helpers

        public string GetResultsUrl(string id)
        {
            NciUrl url = new NciUrl();
            url.SetUrl(_resultsUrl);

            url.QueryParameters.Add("id", id);
            //TODO: Add In Search Params

            return url.ToString();
        }

        public string GetPageUrl(int pageNum)
        {
            NciUrl url = this.PageInstruction.GetUrl("CurrentURL");
            url.QueryParameters.Add("pn", pageNum.ToString());

            return url.ToString();
        }

        public IEnumerable<object> GetPagerItems(int numLeft, int numRight, long totalResults)
        {
            int startPage = (SearchParams.Page - numLeft) >= 1 ? SearchParams.Page - numLeft : 1;
            int maxPage = (int)Math.Ceiling((double)(totalResults / SearchParams.ItemsPerPage));
            int endPage = (SearchParams.Page + numRight) <= maxPage ? SearchParams.Page + numRight : maxPage;

            List<object> items = new List<object>();

            if (SearchParams.Page != 1)
                items.Add(
                    new
                    {
                        Text = "&lt; Previous",
                        PageUrl = GetPageUrl(SearchParams.Page - 1)
                    });

            for (int i = startPage; i <= endPage; i++)
            {
                items.Add(
                    new {
                        Text = i.ToString(),
                        PageUrl = GetPageUrl(i)
                    }
                );
            }

            if (SearchParams.Page != maxPage)
                items.Add(
                    new
                    {
                        Text = "Next &gt;",
                        PageUrl = GetPageUrl(SearchParams.Page + 1)
                    });



            return items;
        }

        #endregion

    }
}
