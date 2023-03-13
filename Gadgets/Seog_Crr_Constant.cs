using System.ComponentModel;

namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_Constant
    {
        public const string PopularLocationXmlFilePath = @"SalaryCompCool\Location\MajorCity.xml";
        public const string AllStatesXmlFilePath = @"SalaryCompCool\Location\AllState.xml";

        public const string AvailableCountofJobPostingXmlFilePath = @"AvailableCountofJobPosting.xml";
        public const string PopularSearchesXmlFilePath = @"PopularJobPostingTitle.xml";
        public const string PopularCompanyXmlFilePath = @"PopularCompany.xml";
        public const string PopularSkillsXmlFilePath = @"PopularSkills.xml";
        public const string CareerAdviceXmlFilePath = @"AllArticles.xml";
        public const string PopularHowToBecomeXmlFilePath = @"HowToBecome\AllArticles.xml";
        public const string AllJobPostingToCareerXmlFileDirPath = @"AllJobPostingToCareer";
        public const string SingleJobPostingXmlFileDirPath = @"SingleJobPosting";
        public const string SingleBenchmarkJobCareerPathXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleBenchmarkJobCareerPath";
        public const string AllCareerSkillXmlFilePath = "AllCareerSkill.xml";
        public const string AllIndustryFamilyXmlFilePath = @"SalaryCompCool\Other\IndustryFamily.xml";

        public const string UnitedStatesText = "United States";
        public const string AllJBDCompanyXmlFilePath = @"SalaryCompCool\JobConsumer\AllJBDCompany.xml";
        public const string SingleJBDCompanyXmlFilePath = @"SalaryCompCool\JobConsumer\SingleJBDCompany";
        public const string SingleJBDCompanyRelatedXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleJBDCompanyRelatedCompany";
        public const string AllHotJobPostingTitleToBenchmarkJobCodeXmlFilePath = @"AllHotJobPostingTitleToBenchmarkJobCode.xml";
        public const string SingleBenchmarkJobToCareerSkillXmlFilePath = @"SingleBenchmarkJobToCareerSkill";
        public const string FeaturedJobXmlFilePath = @"FeaturedJob.xml";
        public const string Conn_CompensationConsumer = "CompensationConsumer";
        public const string AllArticlesXmlFilePath = @"HowToBecome\AllArticles.xml";
        public const string AllArticleSkillDescXmlFilePath = @"HowToBecome\ArticleSkillDesc.xml";
        public const string SingleArticleHotJobXmlFilePath = @"HowToBecome\SingleArticle";
        public const string SingleArticleSectionContentXmlFilePath = @"HowToBecome\SingleArticleSectionContent";
        public const string SingleTXNYJobFamilyToUniversitySubjectXmlFilePath = @"HowToBecome\SingleTXNYJobFamilyToUniversitySubject";
        public const string SearchSimilarHotJobTitleXmlFilePath = @"HowToBecome\SearchSimilarHotJobTitle";
        public const string SingleArticleHotJobToBenchmarkJobXmlFilePath = @"HowToBecome\SingleArticleHotJobToBenchmarkJob";
        public const string SingleArticleSkillXmlFilePath = @"HowToBecome\SingleArticleSkill";
        public const string SingleBenchmarkJobXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleBenchmarkJob";
        public const string SingleBenchmarkJobSkillXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleBenchmarkJobSkill";
        public const string SingleAlternateJobXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleAlternateJob";
        public const string SingleCompanyJobXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleCompanyJob";
        public const string SinglePostingJobXmlFileDirPath = @"SalaryCompCool\JobConsumer\SinglePostingJob";
        public const string SingleNoLevelJobXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleNoLevelJob";
        public const string SingleFunctionJobXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleFunctionJob";
        public const string SingleCertificateJobXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleCertificateJob";
        public const string SingleListingJobXmlFileDirPath = @"SalaryCompCool\JobConsumer\SingleListingJob";
        public const string JobUpdateDateXmlFilePath = @"SalaryCompCool\Other\JobUpdateDate.xml";
        public const string AllCareerJobPostingInfoXmlFilePath = @"SingleJobPostingCareerInfo";

        public const string SingleStateMajorCityXmlFileDirPath = @"SalaryCompCool\Location\SingleStateMajorCity";

        public const string CareerSkillToJobOpeningsXmlFileDirPath = @"CareerSkillToJobOpenings";
    }

    public enum SearchKeywordType : byte
    {
     
        [Description("Title&Company")]
        JobTitleAndCompany = 0,

        [Description("Job Title")]
        JobTitle = 1,

        [Description("Company")]
        Company = 2,  // value is sec company name

        [Description("Skill")]
        Skill = 3,

        [Description("Career Path")]
        CareerPath = 4,

    }

    public enum DatePostedType:byte
    {
        [Description("All")]
        All = 0,

        [Description("Just posted")]
        JustPosted= 1,

        [Description("Within 1 Day")]
        LastOneDay = 2,

        [Description("Within 2-7 Days")]
        LastOneWeek = 3,

        [Description("Within 1 Month")]
        LastOneMonth = 4,

        [Description("Within 3 Months")]
        LastThreeMonth = 5,

        [Description("3 Months Ago")]
        OverThreeMonth = 6
    }

    public enum EmploymentType : byte
    {
        [Description("NA")]
        None = 255,

        [Description("All")]
        All = 0,

        [Description("Full Time")]
        Full_Time = 1,

        [Description("Part Time")]
        Part_Time = 2,

        [Description("Contractor")]
        Contractor = 3,

        [Description("Temporary")]
        Temporary = 4,

        [Description("Intern")]
        Intern = 5,

        [Description("Volunteer")]
        Volunteer = 6,

        [Description("Per Diem")]
        Per_Diem = 7,

        [Description("Other")]
        Other = 8
    }

    // don't change below job type order
    public enum JobType : byte
    {
        None = 0,
        BenchmarkJob,       // 1
        AlternateJob,       // 2
        NoLevelJob,         // 3
        FunctionJob,        // 4
        CertifiedJob,       // 5
        PostingJob,         // 6
        CompanyJob,         // 7
        SECCompanyJob,      // 8
        ListingJob,         // 9
        Skill,              // 10
        SkillPostingJob,    // 11
        SkillListingJob,    // 12
        RecruitingJob = 15,       // 15

        PositionJob = 17,        // 17
        HiringJob,               // 18
        OfferingJob,             // 19
        HotJob = 32

    }

    public enum PayType : byte
    {
        Annual = 0,
        Hourly
    }
}