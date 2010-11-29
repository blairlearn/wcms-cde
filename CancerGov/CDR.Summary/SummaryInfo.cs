using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDR.Summary
{
    public class SummaryInfo
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime PublicationDate { get; private set; }
        public string Category { get; private set; }
        public string PrettyUrl { get; private set; }

        public SummaryInfo(
            string title,
            string description,
            DateTime publicationDate,
            string category,
            string prettyUrl)
        {
            Title = title;
            Description = description;
            PublicationDate = publicationDate;
            Category = category;
            PrettyUrl = prettyUrl;
        }
    }
}
