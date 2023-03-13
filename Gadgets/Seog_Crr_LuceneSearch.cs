using System;
using System.IO;
using System.Data;
using System.Configuration;
using CareerCom.Consumer.CareerJobBoardLuceneSearcher;

namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_LuceneSearch
    {
        private readonly Seog_Crr_Log objLog = new Seog_Crr_Log();
        private readonly Seog_Crr_Data objData = new Seog_Crr_Data();

        public DataSet SalSearchHotJob(string strKeyword, int intPage, out int intCount)
        {
            DataSet dstSearchResult = null;

            string strLuceneIndexFileDir = ConfigurationManager.AppSettings["CareerJobBoardLuceneIndexPathDir"];
            if (!Directory.Exists(strLuceneIndexFileDir))
                throw new DirectoryNotFoundException($@"can't find file {strLuceneIndexFileDir}");

            var objSearch = HotJobTitleSearchService.GetInstance(Path.Combine(strLuceneIndexFileDir, "HotJobTitleLuceneIndexStore", "index.config"));
            int intPageSize = (ConfigurationManager.AppSettings["pagesize"] ?? "10").ChangeType<int>();
            try
            {
                dstSearchResult = objSearch.SalSearchHotJobTitleByName(strKeyword, intPage, intPageSize, out intCount);
                return dstSearchResult;
            }
            catch (Exception e)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", e);
                throw;
            }
            finally
            {
                objData.SalCleanDataSet(ref dstSearchResult);
            }
        }

        public DataSet SalSearchSkill(string strKeyword, int intPage, out int intCount)
        {
            DataSet dstSearchResult = null;

            string strLuceneIndexFileDir = ConfigurationManager.AppSettings["CareerJobBoardLuceneIndexPathDir"];
            if (!Directory.Exists(strLuceneIndexFileDir))
                throw new DirectoryNotFoundException($@"can't find file {strLuceneIndexFileDir}");

            var objSearch = SkillSearchService.GetInstance(Path.Combine(strLuceneIndexFileDir, "SkillLuceneIndexStore", "index.config"));
            int intPageSize = (ConfigurationManager.AppSettings["pagesize"] ?? "10").ChangeType<int>();
            try
            {
                dstSearchResult = objSearch.SalSearchCareerSkillCodesByName(strKeyword, intPage, intPageSize, out intCount);
                return dstSearchResult;

            }
            catch (Exception e)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", e);
                throw;
            }
            finally
            {
                objData.SalCleanDataSet(ref dstSearchResult);
            }
        }

        public DataSet SalSearchCompanyName(string strKeyword, int intPage, out int intCount)
        {
            intCount = 0;
            DataSet dstSearchResult = null;

            if (intPage < 1) intPage = 1;

            string strLuceneIndexFileDir = ConfigurationManager.AppSettings["CareerJobBoardLuceneIndexPathDir"];
            if (!Directory.Exists(strLuceneIndexFileDir))
                throw new DirectoryNotFoundException($@"can't find file {strLuceneIndexFileDir}");

            var objSearch = CompanyNameSearchService.GetInstance(Path.Combine(strLuceneIndexFileDir, "CompanyLuceneIndexStore", "index.config"));
            int intPageSize = (ConfigurationManager.AppSettings["pagesize"] ?? "10").ChangeType<int>();
            try
            {
                dstSearchResult = objSearch.SalSearchCompanyByName(strKeyword, intPage, intPageSize, out intCount);
                return dstSearchResult;
            }
            catch (Exception e)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", e);
                throw;
            }
            finally
            {
                objData.SalCleanDataSet(ref dstSearchResult);
            }
        }

        public DataSet SalSearchJob(out int intCount, SearchKeywordType searchType, string strKeyword = null, string strStartDate = null, string strEndDate = null, string strMetroCode = null, string strCityName = null, string strStateCode = null, string strCompanyID = null, string strIsWorkFromHome = null, string strIndustryCode = null, string strCareerSkillCodes = null, string strEmploymentType = null, int intPage = 1)
        {
            intCount = 0;
            DataSet dstSearchResult = null;

            if (intPage < 1) intPage = 1;

            string strLuceneIndexFileDir = ConfigurationManager.AppSettings["CareerJobBoardLuceneIndexPathDir"];
            if (!Directory.Exists(strLuceneIndexFileDir))
                throw new DirectoryNotFoundException($@"can't find file {strLuceneIndexFileDir}");

            var objSearch = JobBoardLuceneSearchService.GetInstance(Path.Combine(strLuceneIndexFileDir, "JobLuceneIndexStore", "index.config"));
            int intPageSize = (ConfigurationManager.AppSettings["pagesize"] ?? "10").ChangeType<int>();
            if (string.Compare(strEmploymentType, "All", true) == 0)
            {
                strEmploymentType = "";
            }
            try
            {
                switch (searchType)
                {
                    case SearchKeywordType.JobTitleAndCompany:
                        dstSearchResult = objSearch.SalSearchJobByDefault(strKeyword, strStartDate, strEndDate, strMetroCode, strCityName, strStateCode, strCompanyID, strIsWorkFromHome, strIndustryCode, strCareerSkillCodes, strEmploymentType, intPage, intPageSize, out intCount);
                        break;
                    case SearchKeywordType.JobTitle:
                        if (string.IsNullOrEmpty(strKeyword))
                        {
                            strKeyword = "Accountant";
                        }
                        dstSearchResult = objSearch.SalSearchJobByJobTitle(strKeyword, strStartDate, strEndDate, strMetroCode, strCityName, strStateCode, strCompanyID, strIsWorkFromHome, strIndustryCode, strCareerSkillCodes, strEmploymentType, intPage, intPageSize, out intCount);
                        break;
                    case SearchKeywordType.Company:
                        if (string.IsNullOrEmpty(strKeyword))
                        {
                            strKeyword = "Dollar General";
                        }
                        dstSearchResult = objSearch.SalSearchJobByCompanyName(strKeyword, strStartDate, strEndDate, strMetroCode, strCityName, strStateCode, strCompanyID, strIsWorkFromHome, strIndustryCode, strCareerSkillCodes, strEmploymentType, intPage, intPageSize, out intCount);
                        break;
                    case SearchKeywordType.Skill:
                        if (string.IsNullOrEmpty(strKeyword))
                        {
                            strKeyword = "Estimating";
                        }
                        dstSearchResult = objSearch.SalSearchJobBySkills(strKeyword, strStartDate, strEndDate, strMetroCode, strCityName, strStateCode, strCompanyID, strIsWorkFromHome, strIndustryCode, strCareerSkillCodes, strEmploymentType, intPage, intPageSize, out intCount);
                        break;
                    case SearchKeywordType.CareerPath:
                        if (string.IsNullOrEmpty(strKeyword))
                        {
                            strKeyword = "Assistant Manager";
                        }
                        dstSearchResult = objSearch.SalSearchJobByCareerPath(strKeyword, strStartDate, strEndDate, strMetroCode, strCityName, strStateCode, strCompanyID, strIsWorkFromHome, strIndustryCode, strCareerSkillCodes, strEmploymentType, intPage, intPageSize, out intCount);
                        break;
                }

                return dstSearchResult;
            }
            catch (Exception e)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", e);
                throw;
            }
            finally
            {
                objData.SalCleanDataSet(ref dstSearchResult);
            }
        }
    }
}