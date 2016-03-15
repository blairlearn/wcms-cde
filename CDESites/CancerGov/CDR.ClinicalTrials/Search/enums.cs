namespace CancerGov.CDR.ClinicalTrials.Search
{
    /// <summary>
    /// Denotes the different type of location searches.
    /// </summary>
    public enum LocationSearchType
    {
        None = 0,   // No search type was specified (legacy searches only)
        Zip = 1,    // Near Zip Code
        Institution = 2,
        City = 3,
        NIH = 4
    }

    /// <summary>
    /// Denotes the trial status to search for.
    /// OpenOnly -- search for Open/Active
    /// ClosedOnly -- search for Closed/Inactive
    /// </summary>
    public enum TrialStatusType
    {
        None = 0,
        OpenOnly = 1,
        ClosedOnly = 2
    }

    /// <summary>
    /// Denotes how the search was invoked.
    /// </summary>
    public enum SearchInvocationType
    {
        None = 0, // Undefined
        FromSearchForm = 1,
        FromSearchLink = 2
    }

    /// <summary>
    /// The intended audience for a protocol's abstract.
    /// 
    /// These values intentionally mimic those defined in CancerGov.UI.PDQVersion,
    /// but are intended to be passed to and within the business layer.  (i.e. The UI
    /// can use them, but they're not tied to the UI layer.)
    /// </summary>
    public enum AbstractAudience
    {
        Patient = 0,
        HealthProfessional
    }

    /// <summary>
    /// Used with to specify whether the search should filter for records
    /// beginning with the filter value or reconds containing the value.
    /// </summary>
    public enum FilterType
    {
        None = 0, // Undefined
        BeginsWith = 1,
        Contains = 2
    }

    /// <summary>
    /// Sort orders for clinical trial search results.
    /// </summary>
    public enum CTSSortFilters
    {
        TitleAsc = 1,
        TitleDesc = 2,
        PhaseAsc = 3,
        PhaseDesc = 4,
        ProtocolIDAsc = 5,
        ProtocolIDDesc = 6,
        TrialTypeAsc = 7,
        TrialTypeDesc = 8,
        StatusAsc = 9,
        StatusDesc = 10,
        AgeRangeAsc = 11,
        AgeRangeDesc = 12,
        Relevance = 15,
        RelevanceAsc = 16
    }

    public enum CTListTypes
    {
        Drugs,
        Treatment,
        Investigators,
        LeadOrganizations,
        Institutions  
    };
}