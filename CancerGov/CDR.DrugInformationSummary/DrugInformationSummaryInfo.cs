using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDR.DrugInformationSummary
{
    public class DrugInformationSummaryInfo
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime PublicationDate { get; private set; }
        public string PrettyUrl { get; private set; }

        public DrugInformationSummaryInfo(
            string title,
            string description,
            DateTime publicationDate,
            string prettyUrl)
        {
            Title = title;
            Description = description;
            PublicationDate = publicationDate;
            PrettyUrl = prettyUrl;
        }
    }
}
