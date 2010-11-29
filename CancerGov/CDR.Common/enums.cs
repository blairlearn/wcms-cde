using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDR.Common
{
    /// <summary>
    /// The intended audience for a summary.
    /// 
    /// These values intentionally mimic those defined in CancerGov.UI.PDQVersion,
    /// but are intended to be passed to and within the business layer.  (i.e. The UI
    /// may use them, but they're not tied to the UI layer.)
    /// </summary>
    // TODO: Replace AbstractAudience in CDR.ClinicalTrials.Search with this enum.
    public enum Audience
    {
        Unknown = -1,
        Patient = 0,
        HealthProfessional = 1
    }

    /// <summary>
    /// The type of information contained in a summary document.
    /// </summary>
    public enum SummaryType
    {
        All = 0,
        Treatment = 1,
        SupportiveCare = 2,
        CAM = 3,        // Complementary and Alternative Medicine
        Genetics = 4,
        Prevention = 5,
        Screening = 6
    }

    /// <summary>
    /// Bitmap flag for the new vs. updated status of items to search for.
    /// </summary>
    [Flags]
    public enum NewOrUpdated
    {
        New = 0x01,
        Update = 0x02,

        Both = New | Update
    }

    /// <summary>
    /// The language.
    /// </summary>
    public enum Language
    {
        English = 0,
        Spanish = 1
    }
}
