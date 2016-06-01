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

namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSResultsControl : SnippetControl
    {
        private string _index = "clinicaltrials";
        private string _indexType = "trial";
        private string _clusterName = "cts";
        private string _templatePath = "~/VelocityTemplates/BasicCTSResults.vm";
        private string _resultsUrl = "/about-cancer/treatment/clinical-trials/basic/view";
        private int _defaultItemsPerPage = 10;
        private int _defaultZipProximity = 50;

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

        private BaseCTSSearchParam GetSearchParams()
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



            return searchParams;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BaseCTSSearchParam searchParams = GetSearchParams();
            

            BasicCTSManager basicCTSManager = new BasicCTSManager(_index, _indexType, _clusterName);


            //Do the search
            var results = basicCTSManager.Search(searchParams);

            // Show Results

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                _templatePath, 
                new
                {
                    Params = searchParams,
                    Results = results,
                    Control = this
                }
            ));
            Controls.Add(ltl);
        }

        #region Velocity Helpers

        public string GetResultsUrl(string id)
        {
            //TODO: Add In Search Params
            return _resultsUrl + "?id=" + id;
        }

        public string GetPageUrl(int pageNum)
        {
            return this.PageInstruction.GetUrl("CurrentURL").ToString();
        }

        #endregion

    }
}
