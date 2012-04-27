using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.CDR.ClinicalTrials
{
    public class ClinicalTrialInfo
    {
        public string HealthProfessionalTitle { get; private set; }
        public string Description { get; private set; }
        public DateTime PublicationDate { get; private set; }
        public List<string> Categories { get; private set; }
        public string PrettyUrlID { get; private set; }

        public ClinicalTrialInfo(
            string healthProfessionalTitle,
            string description,
            DateTime publicationDate,
            List<string> categories,
            string prettyUrlID)
        {
            HealthProfessionalTitle = healthProfessionalTitle;
            Description = description;
            PublicationDate = publicationDate;
            Categories = categories;
            PrettyUrlID = prettyUrlID;
        }
    }
}
