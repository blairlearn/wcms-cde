using System;

namespace CancerGov.UI
{
    //public enum DisplayVersion
    //{
    //    Image = 1,
    //    [Obsolete("Text-only is dead.  Long live text-only!")]
    //    Text = 2,
    //    Print = 3
    //}

    //public enum DisplayLanguage
    //{
    //    English = 1,
    //    Spanish = 2
    //}

    //public struct DisplayInformation
    //{
    //    public DisplayVersion Version;
    //    public DisplayLanguage Language;
    //}

    /// <summary>
    /// Used with PopEmail.aspx to specify where the "Email this Page"
    /// pop up was invoked from.
    /// </summary>
    public enum EmailPopupInvokedBy
    {
        Unspecified = 0,
        ClinicalTrialSearchResults = 1,
        ClinicalTrialPrintableSearchResults = 2
    }
}

namespace CancerGov.UI
{
    public enum PDQVersion
    {
        Unknown = -1,
        Patient = 0,
        HealthProfessional = 1
    }

    public class PDQVersionResolver
    {
        public static PDQVersion GetPDQVersion(string version)
        {
            version = CancerGov.Text.Strings.IfNull(version, "");

            switch (version.Trim().ToLower())
            {
                case "0":
                    return PDQVersion.Patient;
                case "1":
                    return PDQVersion.HealthProfessional;
                case "patient":
                    return PDQVersion.Patient;
                case "healthprofessional":
                    return PDQVersion.HealthProfessional;
                case "provider":
                    return PDQVersion.HealthProfessional;
                default:
                    return PDQVersion.Patient;
            }
        }

        public static PDQVersion GetPDQVersion(CancerGov.DataAccessClasses.UI.Types.ObjectTypes type)
        {
            switch (type)
            {
                case CancerGov.DataAccessClasses.UI.Types.ObjectTypes.PatientSummary:
                    return PDQVersion.Patient;
                case CancerGov.DataAccessClasses.UI.Types.ObjectTypes.HealthProfessionalSummary:
                    return PDQVersion.HealthProfessional;
                default:
                    return PDQVersion.Patient;
            }
        }

        public static CancerGov.DataAccessClasses.UI.Types.ObjectTypes GetPDQObjectType(PDQVersion version)
        {
            switch (version)
            {
                case PDQVersion.HealthProfessional:
                    return CancerGov.DataAccessClasses.UI.Types.ObjectTypes.HealthProfessionalSummary;
                default:
                    return CancerGov.DataAccessClasses.UI.Types.ObjectTypes.PatientSummary;
            }
        }
    }

}

namespace CancerGov.UI.HTML
{
    public enum HtmlReferenceType
    {
        Unknown = -1,
        Image = 1,
        LongDateImage = 2,
        Date = 3,
        PrintVersionIcon = 4,
        RandomImage = 5
    }
}

namespace CancerGov.DataAccessClasses.UI
{

    public enum XmlSearchSourceType
    {
        Unknown = -1,
        CancerGovDB = 1,
        Verity = 2,
        CDR = 3  //This is for CDR if we ever use this to define cooked ctsearches
    }

    public enum XmlSearchType
    {
        Unknown = -1,
        NoParam = 1,
        Keyword = 2,
        KeywordWithDate = 3,
        Date = 4,
        ClinicalTrials = 5
    }

    public enum XmlSearchResultsType
    {
        Unknown = -1,
        List = 1,
        MultiMediaList = 2
        //List with numbers should just be a ShowItemNumber thing...
    }

    public enum ListItemType
    {
        Normal = 1,
        Featured = 2,
        MultiSourced = 3,
        XmlSearchList = 4
    }
}

namespace CancerGov.DataAccessClasses.UI.Types
{
    public enum NCITemplates
    {
        Unknown = -1,
        Include = 0,
        Doc = 1,
        DocList = 2,
        ContentNav = 3,
        ContentNav3Col = 4,
        ContentDC = 5,
        List = 6,
        MenuMulti = 7,
        DocOESI = 8,
        DocHeader = 9,
        DocImg = 10,
        DocWYNTK = 11,
        DocDigest = 12,
        DocPDQ = 13,
        SinglePageContent = 14,
        SinglePageNavigation = 15,
        MultiPageContent = 16,
        CancerTypeHomePage = 17,
        PowerPointPresentation = 18
    }

    public class NCITemplateResolver
    {
        public static NCITemplates GetNCITemplate(string templateId)
        {
            if (templateId != null && templateId.Trim().Length > 0)
            {
                switch (templateId.ToUpper())
                {
                    case "CF8A1FB5-8091-4193-BE9C-C82E7EAF2A22": //Powerpoint
                        return NCITemplates.PowerPointPresentation;
                    case "9B7A3F6C-FF23-4802-96AF-A03BA98F7B2C":	//include template
                        return NCITemplates.Include;
                    case "4E21A855-49D3-48EE-B4CB-4568D7C9421D":	//doc template
                        return NCITemplates.Doc;
                    case "CE699943-4970-411C-A1A0-A095C095336D":	//doc_list template
                        return NCITemplates.DocList;
                    case "A451FD26-6907-4421-B95E-2BF82AD0BACB":	//content_nav
                        return NCITemplates.ContentNav;
                    case "E888FA66-D424-4D65-A5F8-97633AAF588F":	//content_nav_3col
                        return NCITemplates.ContentNav3Col;
                    case "1DA7854E-6A97-46C5-A2FC-DD11319E998F":	//content_dc
                        return NCITemplates.ContentDC;
                    case "AC4D28B8-654E-4764-899F-32F1BFAA7313":	//list
                        return NCITemplates.List;
                    case "64895958-E443-4D76-86A0-2AFCB214465E":	//menu_multi
                        return NCITemplates.MenuMulti;
                    case "1524EF48-CE52-4346-A989-01885C76069D":	//doc_oesi
                        return NCITemplates.DocOESI;
                    case "E3212EFE-C56B-4BC3-A7F2-BE47CFBFFCED":	//doc_header
                        return NCITemplates.DocHeader;
                    case "967667FF-C4DE-42F0-BDE7-64317082A224":	//doc_img
                        return NCITemplates.DocImg;
                    case "BF0FB2A7-73FE-43F7-832F-769C860FD58F":	//doc_wyntk
                        return NCITemplates.DocWYNTK;
                    case "EC8B6CB6-9C31-4A46-9463-F964AE7AC06E":	//doc_digest
                        return NCITemplates.DocDigest;
                    case "FFF2D734-6A7A-46F0-AF38-DA69C8749AD0":	//doc_pdq
                        return NCITemplates.DocPDQ;
                    case "D9C8A380-6A06-4AFA-86E9-EA52E50E0493":	//SinglePageContent
                        return NCITemplates.SinglePageContent;
                    case "FE7A6A0B-BE0B-42F8-99D4-4A9D100C4985":	//SinglePageNavigation
                        return NCITemplates.SinglePageNavigation;
                    case "7EB26B3D-23AF-4298-B6FD-5C5027F9371C":	//MultiPageContent
                        return NCITemplates.MultiPageContent;
                    case "6984ED05-52BC-4F3D-89B9-2AB358E9943B":	//CancerTypeHomepage
                        return NCITemplates.CancerTypeHomePage;
                    default:
                        return NCITemplates.Unknown;
                }
            }
            else
            {
                return NCITemplates.Unknown;
            }
        }
    }


    //ViewObject "type" database field gets mapped to this enum.  See ViewObjects.cs.
    public enum ObjectTypes
    {
        Unknown = -1,
        Include = 1,
        TextInclude = 2,
        Header = 3,
        View = 4,
        Citation = 5,
        Protocol = 6,
        Document = 7,
        Audio = 8,
        Animation = 9,
        Photo = 10,
        TitleBlock = 11,
        Search = 12,
        VirtualSearch = 13,
        List = 14,
        Pdf = 15,
        Link = 16,
        PatientSummary = 17,
        TopicSearchDefinition = 18,
        HeaderList = 19,
        HealthProfessionalSummary = 20,
        HelpList = 21,
        TableOfContents = 22,
        Summary = 23,
        NavDoc = 24,
        DetailToc = 25,
        TipList = 26,
        DateSearch = 27,
        XmlInclude = 28,
        NavList = 29,
        Image = 30,
        HdrDoc = 31,
        InfoBox = 32,
        SimpleCTSearchForm = 33,    // Obsolete - no longer exists.
        LeftNavigationColumn = 34,
        SummarySection = 35,
        NewsCenterSearchBox = 36,
        FeaturedTrialsSearchBox = 37,
        RightColumn = 38,
        GrayBar = 39,
        Spacer = 40,
        BodyHeader = 41,
        BulletinSearch = 42,
        SpanishFeaturedTrialsSearchBox
    }
}

namespace CancerGov.DataAccessClasses.CDR
{

    public enum ProtocolSectionTypes
    {
        Nothing = 0,
        PatientAbstract = 1,
        Objectives = 2,
        EntryCriteria = 3,
        ExpectedEnrollment = 4,  //combines  Expected Enrollment and ProjectedAccrual 
        Outline = 5,
        PublishedResults = 6,
        Terms = 7,
        HPDisclaimer = 8,
        LeadOrgs = 9,
        PDisclaimer = 10,
        LastMod = 11,
        CTGovBriefSummary = 12,
        CTGovDisclaimer = 13,
        CTGovLeadOrgs = 14,
        CTGovFooter = 15,
        CTGovDetailedDescription = 16,
        CTGovEntryCriteria = 17,
        CTGovTerms = 18,
        SpecialCategory = 19,
        PatientRelatedInformation = 20,
        HPRelatedInformation = 21,
        Outcomes = 22,
        RelatedPublications = 23,
        RegistryInformation = 24,
        TableOfContents = 90,
        Title = 91,
        TitleUrl = 92,
        AlternateTitle = 93,
        InfoBox = 95,
        StudySites = 99 // Never making it to the db, so make this high.
    }

    public enum ClinicalTrialSearchLinkIdType
    {
        None = 0,
        Drug = 1,
        Institution = 2,
        LeadOrganization = 3,
        Investigator = 4,
        Intervention = 5
    }

    public enum ProtocolVersions
    {
        None = 0,
        Patient = 1,
        HealthProfessional = 2
    }

    public enum ProtocolPhases
    {
        All = 0,
        PhaseI = 1,
        PhaseII = 2,
        PhaseIII = 3,
        PhaseIV = 4
    }

    public enum ProtocolTrialTypes
    {
        All = 0,
        Treatment = 1,
        SupportiveCare = 2,
        Screening = 3,
        Prevention = 4,
        Genetics = 5,
        Diagnostic = 6
    }

    //I don't like this, but I have no time... well for right now
    public class ProtocolSearchLookupMethods
    {
        public const string AlphaNumIndex = "ALPHANUMINDEX";
        public const string Keyword = "KEYWORD";
    }

    public enum ProtocolTypes
    {
        Protocol = 5,
        CTGovProtocol = 28
    }

    public enum CDRGeopoliticalContainerType
    {
        Country = 1,
        State = 2,
        City = 3
    }

    public enum CDRMenuTypes
    {
        CancerTypes = 1
    }
}


namespace CancerGov.UI.CDR
{

    public enum ProtocolDisplayFormats
    {
        Short = 1,
        Medium = 2,
        Long = 3,
        Custom = 4,
        SingleProtocol = 5 //This is for view clinical trials, I would have named it all, but that would sound wrong.
    }

    //Updated 8-30-04 Removed for update of search stored procs to not use dynamic sql -- BryanP
    //	public enum SortFilters {
    //		Phase = 1,
    //		Title = 2,
    //		TrialType = 3,
    //		Status = 4,
    //		AgeRange = 5,
    //		Sponsor = 6,
    //		ProtocolID = 7
    //	}

    //Updated 8-30-04 Removed for update of search stored procs to not use dynamic sql -- BryanP
    //	public enum SimpleSortFilters {
    //		TitleAsc = 1,
    //		TitleDesc = 2,
    //		PhaseAsc = 3,
    //		PhaseDesc = 4,
    //		ProtocolIDAsc = 5,
    //		ProtocolIDDesc = 6
    //	}

}

namespace CancerGov.Common
{
    public enum ProtocolSectionTypes
    {
        PatientAbstract = 1,
        Objectives = 2,
        EntryCriteria = 3,
        ProjectedAccrual = 4,
        Outline = 5,
        PublishedResults = 6,
        Terms = 7,
        HPDisclaimer = 8,
        LeadOrgs = 9,
        PDisclaimer = 10,
        LastMod = 11,
        CTGovBriefSummary = 12,
        CTGovDisclaimer = 13,
        CTGovLeadOrgs = 14,
        CTGovFooter = 15,
        CTGovDetailedDescription = 16,
        CTGovEntryCriteria = 17,
        CTGovTerms = 18,
        TableOfContents = 90,
        Title = 91,
        TitleUrl = 92,
        InfoBox = 95,
        StudySites = 99 // Never making it to the db, so make this high.
    }

    public enum ProtocolDrawTypes
    {
        AdvancedSearch = 1,
        SimpleSearch = 2,
        ViewClinicalTrials = 3,
        PrintTrial = 4
    }
    /*
        public enum DisplayDateMode 
        {
            Posted = 1,
            Updated = 2,
            Both = 3,
            None = 4
        }
    */
    public enum DisplayDateMode
    {
        None = 0,
        Posted = 1,
        Updated = 2,
        PostedUpdated = 3,
        Reviewed = 4,
        PostedReviewed = 5,
        UpdatedReviewed = 6,
        All = 7
    }
}

namespace CancerGov.Common
{

    //*********************************************************************
    //TO BE DEPRECATED BY DisplayVersion and DisplayLanguage enums
    //*********************************************************************
    /// <summary>
    /// Defines valid versions of content
    /// </summary>
    public class ContentTypes
    {
        public const string Image = "IMAGE";
        public const string TextOnly = "TEXTONLY";
        public const string Print = "PRINT";
        public const string Spanish = "SPANISH";
    }


    //*********************************************************************
    //TO BE DEPRECATED BY CancerGov.DataAccessClasses.UI.Types.ObjectTypes enum
    //*********************************************************************
    /// <summary>
    /// Defines valid object types 
    /// </summary>
    public class ObjectTypes
    {
        public const string Include = "INCLUDE";
        public const string TextInclude = "TXTINCLUDE";
        public const string Header = "HEADER";
        public const string View = "VIEW";
        public const string Citation = "CITATION";
        public const string Protocol = "PROTOCOL";
        public const string Document = "DOCUMENT";
        public const string Audio = "AUDIO";
        public const string Animation = "ANIMATION";
        public const string Photo = "PHOTO";
        public const string TitleBlock = "TITLEBLOCK";
        public const string Search = "SEARCH";
        public const string VirSearch = "VIRSEARCH";
        public const string List = "LIST";
        public const string Pdf = "PDF";
        public const string Link = "LINK";
        public const string SummaryP = "SUMMARY_P";
        public const string TSTopic = "TSTOPIC";
        public const string HDRList = "HDRLIST";
        public const string SummaryHP = "SUMMARY_HP";
        public const string HelpList = "HTLPLIST";
        public const string Toc = "TOC";
        public const string Summary = "SUMMARY";
        public const string NavDoc = "NAVDOC";
        public const string DetailToc = "DETAILTOC";
        public const string TipList = "TIPLIST";
        public const string DateSearch = "DATESEARCH";
        public const string XmlInclude = "XMLINCLUDE";
        public const string NavList = "NAVLIST";
        public const string Image = "IMAGE";
        public const string HdrDoc = "HDRDOC";
    }

    public class PDQVersions
    {
        public const int Patient = 0;
        public const int HealthProfessional = 1;

        public static int GetPDQVersion(string version, int DefaultPDQVersion)
        {
            switch (version.Trim().ToLower())
            {
                case "patient":
                    return Patient;
                case "healthprofessional":
                    return HealthProfessional;
                case "provider":
                    return HealthProfessional;
                default:
                    return DefaultPDQVersion;
            }
        }
    }

    public class PDQIDTypes
    {
        public const int DocumentID = 0;
        public const int DocumentGUID = 1;
        public const int CDRKey = 2;
        public const int PDQKey = 3;
        public const int AlternateID = 4;
    }

    public class DisplayStyles
    {
        public const string NewsRelease = "NEWSRELEASE";
        public const string NewsSearchTitle = "NEWSSEARCH";
        public const string CTResult = "CTRESULT";
    }

    public class ProtocolSearchLookupMethods
    {
        public const string AlphaNumIndex = "ALPHANUMINDEX";
        public const string Keyword = "KEYWORD";
    }

    public class TransformTypes
    {
        public const int Document = 0;
        public const int Search = 1;
    }


}
