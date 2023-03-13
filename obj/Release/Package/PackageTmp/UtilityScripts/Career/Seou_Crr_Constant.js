var CRR_REQUESTDATA_API = {
    GetJobsByKeyword: settings.baseUrl + 'Career/SalAjxGetJobsByKeyWord',
    GetLocationsByKeyword: settings.baseUrl + 'Career/SalAjxGetLocationsByKeyword',
    GetSkillsFilter: settings.baseUrl + 'Career/SalAjxGetSkillsFilter',
    GetJobByCareerJobPostingID: settings.baseUrl + 'Career/SalAjxGetJobByCareerJobPostingID'
};

var SEARCH_KEYWORD_TYPE = {
    JobTitleAndCompany: 0,
    JobTitle: 1,
    Company: 2,
    Skill: 3,
    CareerPath: 4
};

var SEARCH_KEYWORD_PlACEHolder = [
    'Search Job Title or Keywords',
    'e.g. Accountant',
    'e.g. Dollar General',
    'e.g. Estimating',
    'e.g. Assistant Manager'
];

var DATE_POSTED_TYPE = {
    All: 0,
    LastOneDay: 1,
    LastOneWeek: 2,
    LastOneMonth: 3
};

var EMPLOYMENT_TYPE = {
    None: 255,
    All: 0,
    FullTime: 1,
    PartTime: 2,
    Contract: 3,
    Internship: 4
};