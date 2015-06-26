using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary
{
    internal class DictionaryManager
    {
        public TermReturn GetTerm(String termId, DictionaryType dictionary, Language language, String version)
        {
            TermReturn term;

            switch (dictionary)
            {
                case DictionaryType.term:
                    term = GetCancerTerm(termId, language, version);
                    break;
                case DictionaryType.drug:
                    term = GetDrugTerm(termId, language, version);
                    break;
                case DictionaryType.genetic:
                    term = GetGeneticTerm(termId, language, version);
                    break;
                default:
                    term = null;
                    break;
            }

            return term;
        }

        private TermReturn GetCancerTerm(String termId, Language language, String version)
        {
            DictionaryTerm termDetail = new DictionaryTerm();
            termDetail.id = termId;
            termDetail.term = "Cancer Term";
            termDetail.pronunciation = new Pronunciation();
            termDetail.pronunciation.audio = "audio";
            termDetail.pronunciation.key = "key";
            termDetail.dateFirstPublished = DateTime.MinValue.ToString("MM-dd-yyyy");
            termDetail.dateLastModified = DateTime.MaxValue.ToString("MM-dd-yyyy");
            termDetail.images = new String[] { "http://www.cancer.gov/PublishedContent/Images/images/patient-focused/work/hands-on-laptop-wide.jpg", "http://www.cancer.gov/PublishedContent/Images/images/causes/substances/coal-fired-power-plant-wide.jpg" };
            termDetail.Definition = new Definition();
            termDetail.Definition.Text = "Text";
            termDetail.Definition.Html = "<p>Html</p>";
            termDetail.Aliases = new Alias[] {
                new Alias(){name="Term", type="preferred"},
                new Alias(){name="Word", type="synonym"},
                new Alias(){name="Dictionary Term", type="synonym"},
                new Alias(){name="Cancer Term", type="brand name"}
            };
            termDetail.Related = new RelatedItems();

            TermReturnMeta meta = new TermReturnMeta();
            meta.Audience = "Determined by dictionary for now.";
            meta.Language = language.ToString();
            meta.Messages = new string[] { "K.O." };

            TermReturn ret = new TermReturn()
            {
                Meta = meta,
                Term = termDetail
            };

            return ret;
        }

        private TermReturn GetGeneticTerm(String termId, Language language, String version)
        {
            DictionaryTerm termDetail = new DictionaryTerm();
            termDetail.id = termId;
            termDetail.term = "Genetic Term";
            termDetail.pronunciation = new Pronunciation();
            termDetail.pronunciation.audio = "audio";
            termDetail.pronunciation.key = "key";
            termDetail.dateFirstPublished = DateTime.MinValue.ToString("MM-dd-yyyy");
            termDetail.dateLastModified = DateTime.MaxValue.ToString("MM-dd-yyyy");
            termDetail.images = new String[] { "http://www.cancer.gov/PublishedContent/Images/images/patient-focused/work/hands-on-laptop-wide.jpg", "http://www.cancer.gov/PublishedContent/Images/images/causes/substances/coal-fired-power-plant-wide.jpg" };
            termDetail.Definition = new Definition();
            termDetail.Definition.Text = "Text";
            termDetail.Definition.Html = "<p>Html</p>";
            termDetail.Aliases = new Alias[] {
                new Alias(){name="Term", type="preferred"},
                new Alias(){name="Word", type="synonym"},
                new Alias(){name="Dictionary Term", type="synonym"},
                new Alias(){name="Cancer Term", type="brand name"}
            };
            termDetail.Related = new RelatedItems();

            TermReturnMeta meta = new TermReturnMeta();
            meta.Audience = "Determined by dictionary for now.";
            meta.Language = language.ToString();
            meta.Messages = new string[] { "K.O." };

            TermReturn ret = new TermReturn()
            {
                Meta = meta,
                Term = termDetail
            };

            return ret;
        }

        private TermReturn GetDrugTerm(String termId, Language language, String version)
        {
            DictionaryTerm termDetail = new DictionaryTerm();
            termDetail.id = termId;
            termDetail.term = "Drug Term";
            termDetail.pronunciation = new Pronunciation();
            termDetail.pronunciation.audio = "audio";
            termDetail.pronunciation.key = "key";
            termDetail.dateFirstPublished = DateTime.MinValue.ToString("MM-dd-yyyy");
            termDetail.dateLastModified = DateTime.MaxValue.ToString("MM-dd-yyyy");
            termDetail.images = new String[] { "http://www.cancer.gov/PublishedContent/Images/images/patient-focused/work/hands-on-laptop-wide.jpg", "http://www.cancer.gov/PublishedContent/Images/images/causes/substances/coal-fired-power-plant-wide.jpg" };
            termDetail.Definition = new Definition();
            termDetail.Definition.Text = "Text";
            termDetail.Definition.Html = "<p>Html</p>";
            termDetail.Aliases = new Alias[] {
                new Alias(){name="Term", type="preferred"},
                new Alias(){name="Word", type="synonym"},
                new Alias(){name="Dictionary Term", type="synonym"},
                new Alias(){name="Cancer Term", type="brand name"}
            };
            termDetail.Related = new RelatedItems();

            TermReturnMeta meta = new TermReturnMeta();
            meta.Audience = "Determined by dictionary for now.";
            meta.Language = language.ToString();
            meta.Messages = new string[] { "K.O." };

            TermReturn ret = new TermReturn()
            {
                Meta = meta,
                Term = termDetail
            };

            return ret;
        }
    }
}
