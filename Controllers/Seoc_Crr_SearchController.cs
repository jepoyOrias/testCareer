using System;
using System.IO;
using System.Net;
using System.Web;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using System.Configuration;
using CareerCom.Cool.Gadgets;
using CareerCom.Cool.Models;

namespace CareerCom.Cool.Controllers
{
    public partial class CareerController : Controller
    {
        private readonly Seog_Crr_ApplicationCache objAppache = new Seog_Crr_ApplicationCache();

        public ActionResult CareerJobPosting_SearchIndex_Old()
        {
            return Redirect("/jobs");
        }

        public ActionResult CareerJobPosting_GenericSearchIndex()
        {
            DataSet dstJobPostingData = null;
            DataSet dstSearchResult = null;
            Seom_Crr_CareerJob objJob = new Seom_Crr_CareerJob();
            Seog_Crr_LuceneSearch objSearch = new Seog_Crr_LuceneSearch();
            try
            {
                Hashtable hshPageParams = new Hashtable();
                string strJobPostingID = null;
       
                int intCount = 0;
                string strDebugMode = Request.QueryString.Get("force_debug");
                string pid = Request.QueryString.Get("pid");

                SalProcessUrlParameter(hshPageParams);
     
                dstSearchResult = objJob.SalSearchJob(hshPageParams, out intCount);

                if (objData.SalIsEmptyDataSet(dstSearchResult) && strDebugMode == "1")
                {
                    return ErrorHandler(RouteData.Values["action"].ToString()
                                        , new Exception("The first page job title returned by Lucene has been deleted from the disk")
                                        , RouteData.Values["controller"].ToString(), strDebugMode == "1" ? true : false);
                }

                ViewBag.IsDebugMode = strDebugMode;

                if (!objData.SalIsEmptyDataSet(dstSearchResult))
                {
                    DataRow[] rows = dstSearchResult.Tables[0].Select($"JobPostingID='{pid}'");
                    foreach (DataRow row in rows)
                    {
                        strJobPostingID = row["JobPostingID"].ToString();
                        dstJobPostingData = objJob.SalGetJobPostingByJobPostingID(strJobPostingID);
                        if (!objData.SalIsEmptyDataSet(dstJobPostingData))
                        {
                            hshPageParams["JobPostingData"] = dstJobPostingData.Tables[0];

                            hshPageParams["SkillCodesFromJobDesc"] = row["CareerSkillCodesFromJobDesc"];
                            hshPageParams["CleanJobPostingTitle"] = row["CleanJobPostingTitle"];
                            hshPageParams["BenchmarkJobCode"] = row["BenchmarkJobCode"];
                            hshPageParams["CareerJobPostingID"] = row["CareerJobPostingID"];
                            break;
                        }
                    }
                    if (objData.SalIsEmptyDataSet(dstJobPostingData))
                    {
                        rows = dstSearchResult.Tables[0].Select($"JobPostingID<>'{pid}'");
                        foreach (DataRow row in rows)
                        {
                            strJobPostingID = row["JobPostingID"].ToString();
                            dstJobPostingData = objJob.SalGetJobPostingByJobPostingID(strJobPostingID);
                            if (!objData.SalIsEmptyDataSet(dstJobPostingData))
                            {
                                hshPageParams["JobPostingData"] = dstJobPostingData.Tables[0];

                                hshPageParams["SkillCodesFromJobDesc"] = row["CareerSkillCodesFromJobDesc"];
                                hshPageParams["CleanJobPostingTitle"] = row["CleanJobPostingTitle"];
                                hshPageParams["BenchmarkJobCode"] = row["BenchmarkJobCode"];
                                hshPageParams["CareerJobPostingID"] = row["CareerJobPostingID"];
                                break;
                            }
                        }
                    }

                    hshPageParams["SearchResultData"] = dstSearchResult.Tables[0];
                }

                hshPageParams["SearchResultCount"] = intCount;
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();

                #region adobe or ads code
                string strPageName = "seo:jobsearch";
                //for adobe tracking
                ViewBag.TitleTypeForAdobeTracking = "carrerjboard";
                ViewBag.PrimaryCategoryForAdobeTracking = "seo";
                ViewBag.PageNameForAdobeTracking = strPageName;
                //TBD: Add more parameters here
                Hashtable hshAdParams = new Hashtable
                {
                    { "titletype", "carrerjboard" },
                    { "keyword", hshPageParams["Keyword"] },
                    { "jobtitle", hshPageParams["Keyword"] },
                    { "state", hshPageParams["StateCode"] },
                    { "city", hshPageParams["StateName"] },
                    { "pname", "jboard_searchresults" },
                };

                SalSetupPageAds("Crr_Search", hshAdParams);
                #endregion adobe or ads code
            }
            catch (Exception ex)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
                if (ViewBag.IsDebugMode == "1")
                {
                    return ErrorHandler(RouteData.Values["action"].ToString(), ex, RouteData.Values["controller"].ToString(), true);
                }
            }
            finally
            {
                objData.SalCleanDataSet(ref dstJobPostingData);
                objData.SalCleanDataSet(ref dstSearchResult);
            }

            return View("~/Views/Search/LayoutScripts/Seol_Crr_Search.cshtml");
        }

        public ActionResult CareerJobPosting_SearchIndex()
        {
            DataSet dstJobPostingData = null;
            DataSet dstSearchResult = null;
            Seom_Crr_CareerJob objJob = new Seom_Crr_CareerJob();
            Seog_Crr_LuceneSearch objSearch = new Seog_Crr_LuceneSearch();
            try
            {
                Hashtable hshPageParams = new Hashtable();
                //string strJobTitle = null;
                //string strSeoJobTitle = null;
                //string strCompanyName = null;
                //string strSeoCompanyName = null;
                //string strCompanyID = null;
                //string strCareerJobPostingID = null;
                string strJobPostingID = null;
                //string strKeyword = null;
                //string strBenchmarkJobCode = null;
                //string strCleanJobPostingTitle = null;
                //string strSkillCodesFromJobDesc = null;
                int intCount = 0;
                string strDebugMode = Request.QueryString.Get("force_debug");
                string pid = Request.QueryString.Get("pid");

                SalProcessUrlParameter(hshPageParams);
                //SearchKeywordType searchType = (SearchKeywordType)Convert.ToInt32(hshPageParams["SearchKeywordType"].ToString());
                //strKeyword = hshPageParams["Keyword"].ToString();
                dstSearchResult = objJob.SalSearchJob(hshPageParams, out intCount);

                if (objData.SalIsEmptyDataSet(dstSearchResult) && strDebugMode == "1")
                {
                    return ErrorHandler(RouteData.Values["action"].ToString()
                                        , new Exception("The first page job title returned by Lucene has been deleted from the disk")
                                        , RouteData.Values["controller"].ToString(), strDebugMode == "1" ? true : false);
                }

                ViewBag.IsDebugMode = strDebugMode;

                if (!objData.SalIsEmptyDataSet(dstSearchResult))
                {
                    //var location = dstCareerSkillToJobOpenings.Tables[0].AsEnumerable()
                    //    .Where(q => q["City"].Equals(strJobPostingCity) && q["StateCode"].Equals(strJobPostingStateCode))
                    //    .OrderBy(c => Convert.ToDouble(c["DisSeq"]));

                    DataRow[] rows = dstSearchResult.Tables[0].Select($"JobPostingID='{pid}'");
                    foreach (DataRow row in rows)
                    {
                        strJobPostingID = row["JobPostingID"].ToString();
                        dstJobPostingData = objJob.SalGetJobPostingByJobPostingID(strJobPostingID);
                        if (!objData.SalIsEmptyDataSet(dstJobPostingData))
                        {
                            hshPageParams["JobPostingData"] = dstJobPostingData.Tables[0];

                            hshPageParams["SkillCodesFromJobDesc"] = row["CareerSkillCodesFromJobDesc"];
                            hshPageParams["CleanJobPostingTitle"] = row["CleanJobPostingTitle"];
                            hshPageParams["BenchmarkJobCode"] = row["BenchmarkJobCode"];
                            hshPageParams["CareerJobPostingID"] = row["CareerJobPostingID"];
                            break;
                        }
                    }
                    if (objData.SalIsEmptyDataSet(dstJobPostingData))
                    {
                        rows = dstSearchResult.Tables[0].Select($"JobPostingID<>'{pid}'");
                        foreach (DataRow row in rows)
                        {
                            strJobPostingID = row["JobPostingID"].ToString();
                            dstJobPostingData = objJob.SalGetJobPostingByJobPostingID(strJobPostingID);
                            if (!objData.SalIsEmptyDataSet(dstJobPostingData))
                            {
                                hshPageParams["JobPostingData"] = dstJobPostingData.Tables[0];

                                hshPageParams["SkillCodesFromJobDesc"] = row["CareerSkillCodesFromJobDesc"];
                                hshPageParams["CleanJobPostingTitle"] = row["CleanJobPostingTitle"];
                                hshPageParams["BenchmarkJobCode"] = row["BenchmarkJobCode"];
                                hshPageParams["CareerJobPostingID"] = row["CareerJobPostingID"];
                                break;
                            }
                        }
                    }

                    hshPageParams["SearchResultData"] = dstSearchResult.Tables[0];
                }

                hshPageParams["SearchResultCount"] = intCount;
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();

                #region adobe or ads code
                string strPageName = "seo:jobsearch";
                //for adobe tracking
                ViewBag.TitleTypeForAdobeTracking = "carrerjboard";
                ViewBag.PrimaryCategoryForAdobeTracking = "seo";
                ViewBag.PageNameForAdobeTracking = strPageName;
                //TBD: Add more parameters here
                Hashtable hshAdParams = new Hashtable
                {
                    { "titletype", "carrerjboard" },
                    { "keyword", hshPageParams["Keyword"] },
                    { "jobtitle", hshPageParams["Keyword"] },
                    { "state", hshPageParams["StateCode"] },
                    { "city", hshPageParams["StateName"] },
                    { "pname", "jboard_searchresults" },
                };

                SalSetupPageAds("Crr_Search", hshAdParams);
                #endregion adobe or ads code
            }
            catch (Exception ex)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
                if (ViewBag.IsDebugMode == "1")
                {
                    return ErrorHandler(RouteData.Values["action"].ToString(), ex, RouteData.Values["controller"].ToString(), true);
                }
            }
            finally
            {
                objData.SalCleanDataSet(ref dstJobPostingData);
                objData.SalCleanDataSet(ref dstSearchResult);
            }

            return View("~/Views/Search/LayoutScripts/Seol_Crr_Search.cshtml");
        }

        public void SalProcessUrlParameter(Hashtable hshPageParams)
        {
            try
            {
                // all url parameters value as follows:
                hshPageParams["DatePostedType"] = (byte)DatePostedType.All; // ? t =
                hshPageParams["EmploymentType"] = (byte)EmploymentType.All; // ? et =
                hshPageParams["CareerSkillCodes"] = null; // ? sk =
                hshPageParams["IndustryFamilyCode"] = null; // ? in = 
                hshPageParams["WorkForHome"] = null; // ? r =
                hshPageParams["SearchKeywordType"] = (byte)SearchKeywordType.JobTitle; // ? qt =
                hshPageParams["CompanyID"] = null;  // ? ci = 
                hshPageParams["Page"] = 1;
                hshPageParams["CityName"] = null;
                hshPageParams["StateCode"] = null;
                hshPageParams["StateName"] = null;
                hshPageParams["ZipCode"] = null;
                hshPageParams["Location"] = Seog_Crr_Constant.UnitedStatesText;   // ? l = 
                hshPageParams["Keyword"] = string.Empty;

                #region filter
                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "t"))) // date posted
                {
                    hshPageParams["DatePostedType"] = Uri.UnescapeDataString(Request.QueryString["t"]);
                }

                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "et"))) // employement type
                {
                    hshPageParams["EmploymentType"] = Uri.UnescapeDataString(Request.QueryString["et"]);
                }
                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "sk"))) // skills
                {
                    hshPageParams["CareerSkillCodes"] = Uri.UnescapeDataString(Request.QueryString["sk"]);
                }
                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "p"))) // Page
                {
                    hshPageParams["Page"] = Request.QueryString["p"];
                }
                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "in"))) // industry
                {
                    hshPageParams["IndustryFamilyCode"] = Uri.UnescapeDataString(Request.QueryString["in"]);
                }
                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "r"))) // Remote only
                {
                    hshPageParams["WorkForHome"] = Uri.UnescapeDataString(Request.QueryString["r"]);
                }
                #endregion filter

                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "q"))) // global kewword search
                {
                    hshPageParams["Keyword"] = Uri.UnescapeDataString(Request.QueryString["q"] ?? "");
                }

                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "qt"))) // global kewword search by: 
                {
                    hshPageParams["SearchKeywordType"] = Uri.UnescapeDataString(Request.QueryString["qt"] ?? "");

                    if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "ci"))) // if qt=company
                    {
                        hshPageParams["CompanyID"] = Uri.UnescapeDataString(Request.QueryString["ci"] ?? "");
                    }
                }

                string strLocation = Seog_Crr_Constant.UnitedStatesText;
                if (!String.IsNullOrEmpty(objUtility.SalGetQueryStringValue(Request, "l"))) // global location
                {
                    strLocation = Uri.UnescapeDataString(Request.QueryString["l"] ?? "");
                }

                if (string.Compare(strLocation, Seog_Crr_Constant.UnitedStatesText, true) != 0)
                {
                    Hashtable hshStates = HttpContext.Application["States"] as Hashtable;
                    string[] arrLocation = strLocation.Split(',');
                    if (arrLocation.Length > 1)
                    {
                        string strStateCode = arrLocation[arrLocation.Length - 1].Trim();
                        if (hshStates.ContainsKey(strStateCode))
                        {
                            string strCityName = arrLocation[arrLocation.Length - 2].Trim();
                            hshPageParams["CityName"] = strCityName;
                            hshPageParams["StateCode"] = strStateCode;
                            hshPageParams["StateName"] = objAppache.SalGetStateNameByStateCode(strStateCode);
                            if (arrLocation.Length == 3)
                                hshPageParams["ZipCode"] = arrLocation[0];
                        }
                        else
                        {
                            strLocation = Seog_Crr_Constant.UnitedStatesText;
                        }
                    }
                    else
                    {
                        if (hshStates.ContainsKey(strLocation.Trim()))
                        {
                            hshPageParams["StateCode"] = strLocation;
                            hshPageParams["StateName"] = objAppache.SalGetStateNameByStateCode(strLocation);
                        }
                        else if (hshStates.Values.Cast<string>().Any(q => q.Equals(strLocation.Trim(), StringComparison.OrdinalIgnoreCase)))
                        {
                            foreach (var key in hshStates.Keys)
                            {
                                if (hshStates[key].ToString().Equals(strLocation, StringComparison.OrdinalIgnoreCase))
                                {
                                    hshPageParams["StateCode"] = key;
                                    break;
                                }
                            }

                            hshPageParams["StateName"] = strLocation;
                        }
                        else
                        {
                            strLocation = Seog_Crr_Constant.UnitedStatesText;
                        }
                    }

                    if (hshPageParams["Keyword"] == null)
                    {
                        SearchKeywordType searchType = (SearchKeywordType)Convert.ToByte(hshPageParams["SearchKeywordType"].ToString());
                        switch (searchType)
                        {
                            case SearchKeywordType.JobTitle:
                                hshPageParams["Keyword"] = "Accountant";
                                break;
                            case SearchKeywordType.Company:
                                hshPageParams["Keyword"] = "Dollar General";
                                break;
                            case SearchKeywordType.Skill:
                                hshPageParams["Keyword"] = "Estimating";
                                break;
                            case SearchKeywordType.CareerPath:
                                hshPageParams["Keyword"] = "Assistant Manager";
                                break;
                        }
                    }

                    hshPageParams["Location"] = strLocation;

                } // if location is not equal  united states

            }
            catch (Exception ex)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }


    }
}