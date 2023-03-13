using System;
using System.Web;
using System.Data;
using System.Text;
using System.Collections;
using System.Linq;
using CareerCom.Cool.Gadgets;

namespace CareerCom.Cool.Models
{
    public class Seom_Crr_CareerJob
    {
        Seog_Crr_Log objLog = new Seog_Crr_Log();
        Seog_Crr_Data objData = new Seog_Crr_Data();
        Seog_Crr_XmlHelper objXmlHelper = new Seog_Crr_XmlHelper();
        Seog_Crr_Utility objUtility = new Seog_Crr_Utility();
        public DataSet SalGetPopularSkill()
        {
            return objXmlHelper.SalGetPopularSkillFromXmlFile();
        }
        public DataSet SalGetPopularSearches()
        {
            return objXmlHelper.SalGetPopularSearchesFromXmlFile();
        }
        public DataSet SalGetPopularCompany()
        {
            return objXmlHelper.SalGetPopularCompanyFromXmlFile();
        }
        public DataSet SalGetPopularLocation()
        {
            return objXmlHelper.SalGetPopularLocationFromXmlFile();
        }
        public DataSet SalGetCareerAdvice()
        {
            return objXmlHelper.SalGetCareerAdviceFromXmlFile();
        }
        public DataSet SalGetPopularHowToBecome()
        {
            return objXmlHelper.SalGetPopularHowToBecomeFromXmlFile();
        }
        public DataSet SalGetJobPostingByJobPostingID(string strJobPostingID)
        {
            return objXmlHelper.SalGetSingleJobPostingFromXmlFile(strJobPostingID);
        }
        public DataSet SalGetCareerJobPostingDataByCareerJobID(string strJobPostingID)
        {
            return objXmlHelper.SalGetCareerPostingJobDataByCareerJobIDFromXmlFile(strJobPostingID);
        }
        public string SalGetJobPostingIDByCareerJobID(string strCareerJobPostingID)
        {
            DataSet dstCareerData = objXmlHelper.SalGetAllJobPostingToCareerFromXmlFile(strCareerJobPostingID);
            if (!dstCareerData.IsEmpty())
            {
                DataRow drw = dstCareerData.Tables[0].Select($"CareerJobPostingID='{strCareerJobPostingID}'").FirstOrDefault();
                if (drw != null)
                {
                    return drw["JobPostingID"].ToString();
                }
                objData.SalCleanDataSet(ref dstCareerData);
            }

            return null;
        }

        public string SalGenerateCareerJobPostingDetailUrl(string strSeoCompanyName, string strSeoJobTitle, string strCareerJobPostingID, string strCity, string strStateCode)
        {
            string strAppRootPath = VirtualPathUtility.ToAbsolute("~", HttpContext.Current.Request.ApplicationPath);
            return $"{strAppRootPath}company/{strSeoCompanyName}/job/{strSeoJobTitle}/-in-{objUtility.SalMakeSEOFriendlyName(strCity)},{objUtility.SalMakeSEOFriendlyName(strStateCode)}?jid={strCareerJobPostingID}".ToLower();
        }


        public string SalGenerateCareerJobSearchUrl(string strKeyword = null, DatePostedType datePostedType = DatePostedType.All, EmploymentType employmentType = EmploymentType.All, string strCareerSkillCodes = null, string strIndustryFamilyCode = "100", bool bolWFH = false, string strCompanyID = null, string strLocation = null, SearchKeywordType searchType = SearchKeywordType.JobTitle)
        {
            string strAppRootPath = VirtualPathUtility.ToAbsolute("~", HttpContext.Current.Request.ApplicationPath);
            StringBuilder sbUrl = new StringBuilder(strAppRootPath);
            sbUrl.Append("jobs?");
            var actionCreateUrl = new Action(() =>
            {
                if (searchType != SearchKeywordType.JobTitleAndCompany)
                {
                    sbUrl.AppendFormat("&qt={0}", (byte)searchType);
                }

                if (SearchKeywordType.Company == searchType && !string.IsNullOrEmpty(strCompanyID))
                {
                    sbUrl.AppendFormat("&ci={0}", strCompanyID);
                }
                if (DatePostedType.All != datePostedType)
                {
                    sbUrl.AppendFormat("&t={0}", (byte)datePostedType);
                }
                if (EmploymentType.All != employmentType)
                {
                    sbUrl.AppendFormat("&et={0}", (byte)employmentType);
                }
                if (!string.IsNullOrEmpty(strCareerSkillCodes))
                {
                    sbUrl.AppendFormat("&sk={0}", strCareerSkillCodes);
                }
                if (strIndustryFamilyCode != "100")
                {
                    sbUrl.AppendFormat("&in={0}", strIndustryFamilyCode);
                }
                if (bolWFH)
                {
                    sbUrl.AppendFormat("&r=1");
                }
            });

            if (string.IsNullOrEmpty(strKeyword))
            {
                if (!string.IsNullOrEmpty(strLocation))
                {
                    sbUrl.AppendFormat("l={0}", HttpUtility.UrlEncode(strLocation));
                }
                actionCreateUrl();
            }
            else
            {
                sbUrl.AppendFormat("q={0}", HttpUtility.UrlEncode(strKeyword));
                if (!string.IsNullOrEmpty(strLocation) && strLocation != Seog_Crr_Constant.UnitedStatesText)
                {
                    sbUrl.AppendFormat("&l={0}", HttpUtility.UrlEncode(strLocation));
                }
                actionCreateUrl();
            }

            return sbUrl.ToString().TrimEnd('?');
        }

        public DataSet SalGetJobsForCareerPathByBenchmarkJobCode(string strBenchmarkJobCode)
        {
            return objXmlHelper.SalGetJobsForCareerPathFromXmlFile(strBenchmarkJobCode);
        }


        public DataSet SalGetAllCareerSkill()
        {
            return objXmlHelper.SalGetAllCareerSkillFromXmlFile();
        }

        public DataSet SalGetAllIndustries()
        {
            DataSet dstAllIndustries = objXmlHelper.SalGetAllIndustryFamilyFromXmlFile();
            for (int i = 0; i < dstAllIndustries.Tables[0].Rows.Count; i++)
            {
                if (dstAllIndustries.Tables[0].Rows[i]["IndustryFamilyCode"].ToString() == "I00")
                {
                    dstAllIndustries.Tables[0].Rows[i].Delete();
                    dstAllIndustries.AcceptChanges();
                    break;
                }
            }
            return dstAllIndustries;
        }

        public DataSet SalGetCompanyInfoByCompanyID(string strCompanyID)
        {
            return objXmlHelper.SalGetCompanyInfoByCompanyIDFromXmlFile(strCompanyID);
        }

        public DataSet SalGetRelatedCompanyInfoByCompanyID(string strCompanyID)
        {
            return objXmlHelper.SalGetRelatedCompanyInfoByCompanyIDFromXmlFile(strCompanyID);
        }

        public DataSet SalGetSkillsByBenchmarkJobCode(string strBenchmarkJobCode)
        {
            return objXmlHelper.SalGetSkillsByBenchmarkJobCodeFromXmlFile(strBenchmarkJobCode);
        }

        public DataSet SalGetAvailableCountofJobPosting()
        {
            return objXmlHelper.SalGetAvailableCountofJobPostingFromXmlFile();
        }

        public DataSet SalGetFeaturedJob()
        {
            return objXmlHelper.SalGetFeaturedJobFromXmlFile();
        }

        public DataSet SalSearchJob(Hashtable hshPageParams, out int intCount)
        {
            Seog_Crr_LuceneSearch objSearch = new Seog_Crr_LuceneSearch();

            intCount = 0;
            string strKeyword = hshPageParams["Keyword"] as string;
            SearchKeywordType searchType = (SearchKeywordType)Convert.ToByte(hshPageParams["SearchKeywordType"].ToString());

            int intPage;
            int.TryParse(hshPageParams["Page"] as string, out intPage);

            string strStartDate, strEndDate;
            SalGetFormatDateTime((DatePostedType)Convert.ToByte(hshPageParams["DatePostedType"].ToString()), out strStartDate, out strEndDate);

            return objSearch.SalSearchJob(out intCount, searchType, strKeyword,
                strStartDate: strStartDate,
                strEndDate: strEndDate,
                strMetroCode: "",
                strCityName: hshPageParams["CityName"] as string,
                strStateCode: hshPageParams["StateCode"] as string,
                strCompanyID: hshPageParams["CompanyID"] as string,
                strIsWorkFromHome: hshPageParams["WorkForHome"] as string,
                strIndustryCode: hshPageParams["IndustryFamilyCode"] as string,
                strCareerSkillCodes: hshPageParams["CareerSkillCodes"] as string,
                strEmploymentType: ((EmploymentType)Convert.ToByte(hshPageParams["EmploymentType"].ToString())).ToString(),
                intPage: intPage);
        }

        public void SalGetFormatDateTime(DatePostedType postedType, out string strStartDate, out string strEndDate)
        {
            strStartDate = "";
            strEndDate = "";
            switch (postedType)
            {
                case DatePostedType.All:
                    strStartDate = "";
                    strEndDate = "";
                    break;
                case DatePostedType.JustPosted:
                    strEndDate = Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd");
                    strStartDate = Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd");
                    break;
                case DatePostedType.LastOneDay:
                    strEndDate = Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd");
                    strStartDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("D").ToString()).ToString("yyyy-MM-dd");
                    break;
                case DatePostedType.LastOneWeek:
                    strEndDate = Convert.ToDateTime(DateTime.Now.AddDays(-2).ToString("D").ToString()).ToString("yyyy-MM-dd");
                    strStartDate = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToString("D").ToString()).ToString("yyyy-MM-dd");
                    break;
                case DatePostedType.LastOneMonth:
                    strEndDate = Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd");
                    strStartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("D").ToString()).ToString("yyyy-MM-dd");
                    break;
                case DatePostedType.LastThreeMonth:
                    strEndDate = Convert.ToDateTime(DateTime.Now.ToString("D").ToString()).ToString("yyyy-MM-dd");
                    strStartDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToString("D").ToString()).ToString("yyyy-MM-dd");
                    break;
                case DatePostedType.OverThreeMonth:
                    strEndDate = Convert.ToDateTime(DateTime.Now.AddMonths(-3).ToString("D").ToString()).ToString("yyyy-MM-dd");
                    strStartDate = Convert.ToDateTime(DateTime.Now.AddYears(-100).ToString("D").ToString()).ToString("yyyy-MM-dd");
                    break;

            }
        }

        public DataSet SalGetSingleArticleSectionContent(string strBenchmarkJobCode)
        {
            return objXmlHelper.SalGetSingleArticleSectionContentFromXmlFile(strBenchmarkJobCode);
        }

        public DataSet SalGetAllHotTitles()
        {
            return objXmlHelper.SalGetAllHotTitlesFromXmlFile();
        }

        public DataSet SalGetSingleArticleHotJobByArticleHotJobCode(string strArticleHotJobCode)
        {
            return objXmlHelper.SalGetSingleArticleHotJobFromXmlFile(strArticleHotJobCode);
        }

        public DataSet SalGetSingleArticleHotJobToBenchmarkJob(string strArticleHotJobCode)
        {
            return objXmlHelper.SalGetSingleArticleHotJobToBenchmarkJobFromXmlFile(strArticleHotJobCode);
        }

        public DataSet SalGetSingleBenchmarkJobByBenchmarkJobCode(string strBenchmarkJobCode)
        {
            return objXmlHelper.SalGetSingleBenchmarkJobFromXmlFile(strBenchmarkJobCode);
        }

        public DataSet SalGetSingleTXNYJobFamilyToUniversitySubject(string strTXNYJobFamilyCode)
        {
            return objXmlHelper.SalGetSingleTXNYJobFamilyToUniversitySubjectFromXmlFile(strTXNYJobFamilyCode);
        }

        public DataSet SalGetJobData(JobType jobType, string strJobCode)
        {
            if (JobType.SECCompanyJob == jobType)
            {
                throw new ArgumentException($"The method not support job type {jobType.ToString()}, please invoke method: SalGetSingleSECCompanyJob");
            }

            DataSet dstJobData = null;
            switch (jobType)
            {
                case JobType.BenchmarkJob:
                    dstJobData = objXmlHelper.SalGetSingleBenchmarkJobFromXmlFile(strJobCode);
                    break;
                case JobType.AlternateJob:
                    dstJobData = objXmlHelper.SalGetSingleAlternateJobFromXmlFile(strJobCode);
                    break;
                case JobType.CompanyJob:
                    dstJobData = objXmlHelper.SalGetSingleCompanyJobFromXmlFile(strJobCode);
                    break;
                case JobType.PostingJob:
                    dstJobData = objXmlHelper.SalGetSinglePostingJobFromXmlFile(strJobCode);
                    break;
                case JobType.NoLevelJob:
                    dstJobData = objXmlHelper.SalGetSingleNoLevelJobFromXmlFile(strJobCode);
                    break;
                case JobType.FunctionJob:
                    dstJobData = objXmlHelper.SalGetSingleFunctionJobFromXmlFile(strJobCode);
                    break;
                case JobType.CertifiedJob:
                    dstJobData = objXmlHelper.SalGetSingleCertificateJobFromXmlFile(strJobCode);
                    break;
                case JobType.ListingJob:
                    dstJobData = objXmlHelper.SalGetSingleListingJobFromXmlFile(strJobCode);
                    break;
            }
            return dstJobData;
        }

        public DataSet SalGetCareerSkillToJobOpenings(string strSkillCode, string strStateCode = "USA")
        {
            return objXmlHelper.SalGetCareerSkillToJobOpenings(strSkillCode, strStateCode);
        }
    }

    public class SearchFilterCondition
    {
        public SearchKeywordType SearchKeywordType = SearchKeywordType.JobTitleAndCompany;
        public DatePostedType DatePostedType = DatePostedType.All;
        public EmploymentType EmploymentType = EmploymentType.All;
        public string CareerSkilCodes { get; set; }
        public string IndustryFamilyCode { get; set; }
        public bool WorkForHome { get; set; }
        public string Keyword { get; set; }
        public string CompanyID { get; set; }
        public string Location { get; set; }

    }
}