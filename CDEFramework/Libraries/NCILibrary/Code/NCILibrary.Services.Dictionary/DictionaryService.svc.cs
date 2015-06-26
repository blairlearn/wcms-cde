using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    public class DictionaryService : IDictionaryService
    {
        public TermReturn GetTerm(String termId, String dictionary, String language, String version)
        {
            //retVal.key= String.Format("Foo!  termid = {0}  dictionary = {1}  language = {2}  API version = {3}",
            //    termId, dictionary, language, version);
            //retVal.audio = "the other thing.";

            DictionaryTerm termDetail = new DictionaryTerm();
            termDetail.id = termId;
            termDetail.term = "term";
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
            meta.Language = language;
            meta.Message = "K.O.";

            TermReturn ret = new TermReturn()
            {
                Meta = meta,
                Term = termDetail
            };

            return ret;
        }
    }
}
