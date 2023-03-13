using System;
using System.IO;
using System.Net;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using System.Configuration;
using CareerCom.Cool.Gadgets;
using CareerCom.Cool.Models;
using System.Threading.Tasks;

namespace CareerCom.Cool.Controllers
{
    public partial class CareerController : Controller
    {

        private readonly Seog_Crr_Data objData = new Seog_Crr_Data();
        private readonly Seog_Crr_Log objLog = new Seog_Crr_Log();
        private readonly Seog_Crr_Json objJson = new Seog_Crr_Json();
        private readonly Seog_Crr_Utility objUtility = new Seog_Crr_Utility();
        private readonly Seog_Crr_LuceneSearch objLuceneSearch = new Seog_Crr_LuceneSearch();

        private readonly Seog_Crr_XmlHelper objXmlHelper = new Seog_Crr_XmlHelper();

        [HttpPost]
        public ActionResult SalAjxGetJobsByKeyWord(string strKeyWords, SearchKeywordType searchType)
        {
            Hashtable hshParams = new Hashtable();
            int intCount = 0, intCompanyCount = 0, intSkillCount = 0;
            DataSet dstJob = null, dstCompany = null, dstSkill = null;

            switch (searchType)
            {
                case SearchKeywordType.Company:
                    dstCompany = objLuceneSearch.SalSearchCompanyName(strKeyWords, 1, out intCount);
                    break;
                case SearchKeywordType.Skill:
                    dstSkill = objLuceneSearch.SalSearchSkill(strKeyWords, 1, out intCount);
                    break;
                case SearchKeywordType.JobTitle:
                case SearchKeywordType.CareerPath:
                    dstJob = objLuceneSearch.SalSearchHotJob(strKeyWords, 1, out intCount);
                    break;
            }

            hshParams.Add("JobTitleData", dstJob);
            hshParams.Add("CompanyData", dstCompany);
            hshParams.Add("SkillData", dstSkill);
            hshParams.Add("CareerPathData", dstJob);

            hshParams.Add("JobTitleCount", intCount);
            hshParams.Add("CompanyCount", intCompanyCount);
            hshParams.Add("SkillCount", intSkillCount);
            hshParams.Add("CareerPathCount", intCount);
            hshParams.Add("Keyword", strKeyWords);
            hshParams.Add("SearchType", searchType);

            ViewBag.DestinationHost = ConfigurationManager.AppSettings["DestinationHost"];
            ViewBag.PageParams = hshParams;
            return PartialView("~/Views/Shared/ElementScripts/Seoe_Crr_JobsSearchResult.cshtml");
        }

        [HttpGet]
        public string SalAjxGetLocationsByKeyword(string strLocationKeyword, bool bolIsSearchCountry = false)
        {
            Hashtable hshDbParams = new Hashtable();

            hshDbParams["keyword"] = strLocationKeyword;
            hshDbParams["IsSearchCountry"] = bolIsSearchCountry ? 1 : 0;
            DataSet dstLocation = objData.SalGetDataSet(hshDbParams, "usp_SWZNETV2_GetLocationByKeyword", Seog_Crr_Constant.Conn_CompensationConsumer);
            if (!objData.SalIsEmptyDataSet(dstLocation))
            {
                return objJson.SalSerializeDataTableToJson(dstLocation.Tables[0]);
            }

            return null;
        }

        [HttpPost]
        public ActionResult SalAjxGetSkillsFilter(string strSelectedSkills)
        {
            Hashtable hshParams = new Hashtable();
            hshParams.Add("CareerSkillCodes", strSelectedSkills);

            ViewBag.PageParams = hshParams;
            return PartialView("~/Views/Shared/ElementScripts/Seoe_Crr_Filter_SkillList.cshtml");
        }

        [HttpPost]
        public ActionResult SalAjxGetJobByCareerJobPostingID(string strJobPostingID, string strLocation, string strBenchmarkJobCode, string strSkillCodesFromJobDesc, string strCleanJobPostingTitle)
        {
            Hashtable hshPageParams = new Hashtable();

            Seom_Crr_CareerJob objJob = new Seom_Crr_CareerJob();
            DataSet dstJobPostingData = objJob.SalGetJobPostingByJobPostingID(strJobPostingID);
            if (!dstJobPostingData.IsEmpty())
            {
                hshPageParams["JobPostingData"] = dstJobPostingData.Tables[0];
            }

            hshPageParams["Location"] = strLocation;
            hshPageParams["BenchmarkJobCode"] = strBenchmarkJobCode;
            hshPageParams["SkillCodesFromJobDesc"] = strSkillCodesFromJobDesc;
            hshPageParams["CleanJobPostingTitle"] = strCleanJobPostingTitle;

            ViewBag.PageParams = hshPageParams;
            return PartialView("~/Views/Search/ElementScripts/Seoe_Crr_Search_RightColumn.cshtml");
        }
    }
}