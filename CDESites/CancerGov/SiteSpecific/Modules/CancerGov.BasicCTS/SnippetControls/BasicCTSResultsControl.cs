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
    public class BasicCTSResultsControl : BasicCTSBaseControl
    {
        /*
        private string _index = "clinicaltrials";
        private string _indexType = "trial";
        private string _clusterName = "SearchCluster";
        private string _templatePath = "~/VelocityTemplates/BasicCTSResults.vm";
        private string _resultsUrl = "/about-cancer/treatment/clinical-trials/basic/view";
        private string _ESTemplateFullText = "clinicaltrials_CTfulltextTemplate";
        private string _ESTemplateCancerType = "clinicaltrials_CTCancerTypeIDTemplate";
        private int _defaultItemsPerPage = 10;
        private int _defaultZipProximity = 100;
        */

        public BaseCTSSearchParam SearchParams { get; private set; }

        private BasicCTSManager _basicCTSManager = null;


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
            int itemsPerPage = this.ParmAsInt("ni", BasicCTSPageInfo.DefaultItemsPerPage);
            string phrase = this.ParmAsStr("q", string.Empty);
            string zip = this.ParmAsStr("z", string.Empty);
            int zipProximity = this.ParmAsInt("zp", BasicCTSPageInfo.DefaultZipProximity); //In miles
            int age = this.ParmAsInt("a", 0);
            int gender = this.ParmAsInt("g", 0); //0 = decline, 1 = female, 2 = male, 
            string cancerType = this.ParmAsStr("t", string.Empty);

            BaseCTSSearchParam searchParams = null;

            #region Set Cancer Type or Phrase

            if (cancerType != string.Empty)
            {
                searchParams = new CancerTypeSearchParam()
                {
                    //get cancer type.
                    CancerTypeID = cancerType,
                    ESTemplateFile = BasicCTSPageInfo.ESTemplateCancerType
                };
            }
            else
            {
                searchParams = new PhraseSearchParam()
                {
                    Phrase = phrase,
                    ESTemplateFile = BasicCTSPageInfo.ESTemplateFullText
                };
            }

            #endregion

            // Fill in common parameters

            #region Set Zip Code + GeoLocation
            if (!string.IsNullOrWhiteSpace(zip))
            {
                searchParams.ZipLookup = _basicCTSManager.GetZipLookupForZip(zip);
            }

            #endregion

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

            _basicCTSManager = new BasicCTSManager(
                BasicCTSPageInfo.SearchIndex,
                BasicCTSPageInfo.TrialIndexType,
                BasicCTSPageInfo.MenuTermIndexType,
                BasicCTSPageInfo.GeoLocIndexType,
                BasicCTSPageInfo.SearchClusterName
            );

            SetSearchParams();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            



            //Do the search
            var results = _basicCTSManager.SearchTemplate(SearchParams);

            // Show Results

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                BasicCTSPageInfo.ResultsPageTemplatePath, 
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
            url.SetUrl(BasicCTSPageInfo.DetailedViewPagePrettyUrl);

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
