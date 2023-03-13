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
using System.Linq;

namespace CareerCom.Cool.Controllers
{
    public partial class CareerController : Controller
    {

        [HttpPost]
        public ActionResult CareerJobPosting_JobDetailIndex()
        {
            string strSeoCompanyName = RouteData.GetRequiredString("strSeoFriendlyCompanyName");
            string strSeoJobTitle = RouteData.GetRequiredString("strSeoFriendlyJobTitle");
            string strLocation = RouteData.GetRequiredString("strLocation");
            //string strCareerJobPostingID = RouteData.GetRequiredString("strCareerJobPostingID");
            return CareerJobPosting_JobDetailIndex(strSeoCompanyName, strSeoJobTitle, strLocation);
        }

        [HttpGet]
        public ActionResult CareerJobPosting_JobDetailIndex(string strSeoFriendlyCompanyName, string strSeoFriendlyJobTitle, string strLocation)
        {
            string strCareerJobPostingID = Request["jid"];

            DataSet dstJobPostingData = null;
            Seom_Crr_CareerJob objJobPosting = new Seom_Crr_CareerJob();

            DataSet dstSkillByBenchmarkJobCode = null;
            DataTable dtbFiltedSkillData = null;
            try
            {
                Hashtable hshPageParams = new Hashtable();
                Hashtable hshAllHotJobToBenchmarkJobCode = null;
                DataSet dstCareerJobPostingData = null;
                string strJobTitle = null;
                string strSeoJobTitle = null;
                string strSeoCompanyName = null;
                string strBenchmarkJobCode = null;
                string strCleanJobPostingTitle = null;
                string strSkillCodesFromJobDesc = null;
                string[] arrCareerSkillCodes = null;

                string strJobPostingID = objJobPosting.SalGetJobPostingIDByCareerJobID(strCareerJobPostingID);
                string strDebugMode = Request.QueryString.Get("force_debug");
                if (strDebugMode == "1")
                {
                    ViewBag.IsDebugMode = "1";
                }
                if (string.IsNullOrEmpty(strJobPostingID) && strDebugMode == "1")
                {
                    return ErrorHandler(RouteData.Values["action"].ToString(), new ArgumentNullException($"{nameof(strCareerJobPostingID)}={strCareerJobPostingID},  {nameof(strJobPostingID)} = {strJobPostingID}"), RouteData.Values["controller"].ToString(), strDebugMode == "1" ? true : false);
                }
                dstJobPostingData = objJobPosting.SalGetJobPostingByJobPostingID(strJobPostingID);
                if (objData.SalIsEmptyDataSet(dstJobPostingData))
                {
                    return RedirectPermanent(Url.Content($"~/jobs?q={Uri.EscapeDataString(strSeoFriendlyJobTitle.Replace("-", " "))}"));
                }

                hshAllHotJobToBenchmarkJobCode = (Hashtable)HttpContext.Application["AllHotJobToBenchmarkJobCode"];
                if (hshAllHotJobToBenchmarkJobCode != null)
                {
                    if (!string.IsNullOrEmpty(strCareerJobPostingID))
                    {
                        dstCareerJobPostingData = objJobPosting.SalGetCareerJobPostingDataByCareerJobID(strJobPostingID);
                        if (!objData.SalIsEmptyDataSet(dstCareerJobPostingData))
                        {
                            DataRow drwCleanJobPosting = dstCareerJobPostingData.Tables[0].Select($"CareerJobPostingID='{strCareerJobPostingID}'").FirstOrDefault();
                            strCleanJobPostingTitle = drwCleanJobPosting["CleanJobPostingTitle"].ToString();
                            strSkillCodesFromJobDesc = drwCleanJobPosting["CareerSkillCodes"].ToString();
                            if (!string.IsNullOrEmpty(strCleanJobPostingTitle))
                            {
                                if (hshAllHotJobToBenchmarkJobCode.ContainsKey(strCleanJobPostingTitle.ToLower()))
                                {
                                    strBenchmarkJobCode = hshAllHotJobToBenchmarkJobCode[strCleanJobPostingTitle.ToLower()].ToString();
                                }
                            }
                        }
                    }
                }

                strJobTitle = dstJobPostingData.Tables[0].Rows[0]["Title"].ToString();
                strSeoJobTitle = dstJobPostingData.Tables[0].Rows[0]["SEOTitle"].ToString().ToLower();
                strSeoCompanyName = dstJobPostingData.Tables[0].Rows[0]["SEOCompanyName"].ToString().ToLower();
                if (string.Compare(strSeoFriendlyJobTitle, strSeoJobTitle, true) != 0)
                {
                    return ErrorHandler("ErrorHandler", new Exception("please ensure job title is valid in your url"));
                }
                if (string.Compare(strSeoCompanyName, strSeoFriendlyCompanyName, true) != 0)
                {
                    return ErrorHandler("ErrorHandler", new Exception("please ensure company name is valid in your url"));
                }
                if (!string.IsNullOrEmpty(strJobTitle))
                {
                    if (hshAllHotJobToBenchmarkJobCode.ContainsKey(strJobTitle.ToLower()))
                    {
                        strBenchmarkJobCode = hshAllHotJobToBenchmarkJobCode[strJobTitle.ToLower()].ToString();
                    }
                }



                if(!string.IsNullOrEmpty(strBenchmarkJobCode))
                {
                    dstSkillByBenchmarkJobCode = objJobPosting.SalGetSkillsByBenchmarkJobCode(strBenchmarkJobCode);
                    if (!objData.SalIsEmptyDataSet(dstSkillByBenchmarkJobCode))
                    {
                        if(!string.IsNullOrEmpty(strSkillCodesFromJobDesc))
                        {
                            arrCareerSkillCodes = strSkillCodesFromJobDesc.Split('|');
                            dtbFiltedSkillData = dstSkillByBenchmarkJobCode.Tables[0].Clone();
                            foreach (DataRow drw in dstSkillByBenchmarkJobCode.Tables[0].Rows)
                            {
                                for (int j = 0; j < arrCareerSkillCodes.Length; j++)
                                {
                                    if (drw["SkillCode"].ToString() == arrCareerSkillCodes[j].ToString())
                                    {
                                        dtbFiltedSkillData.Rows.Add(drw.ItemArray);
                                        break;
                                    }

                                }
                            }

                        }
                    }
                }
                hshPageParams["FiltedSkillData"] = dtbFiltedSkillData;



                    SalInitialPageParameters(hshPageParams);
                SalProcessFormParameterFromPostRequest(hshPageParams);
                if (Request.HttpMethod == "GET")
                {
                    hshPageParams["DatePostedType"] = (byte)DatePostedType.All; // ? t =
                    hshPageParams["EmploymentType"] = (byte)EmploymentType.All; // ? et =
                    hshPageParams["WorkForHome"] = "0"; // ? r =
                    hshPageParams["Keyword"] = strJobTitle;
                    hshPageParams["SearchKeywordType"] = (byte)SearchKeywordType.JobTitle;
                }

                hshPageParams["JobPostingData"] = dstJobPostingData.Tables[0];
                hshPageParams["BenchmarkJobCode"] = strBenchmarkJobCode;
                hshPageParams["CleanJobPostingTitle"] = strCleanJobPostingTitle;
                //hshPageParams["SkillCodesFromJobDesc"] = strSkillCodesFromJobDesc;
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();

                #region adobe or ads code
                string strPageName = "jboard:jobdetails";
                //for adobe tracking
                ViewBag.titletype = "na";
                ViewBag.PrimaryCategoryForAdobeTracking = "jboard";
                ViewBag.PageNameForAdobeTracking = strPageName;
                //TBD: Add more parameters here
                Hashtable hshAdParams = new Hashtable
                {
                    { "titletype", "jboard" },
                    { "keyword", strJobTitle },
                    { "jobtitle", strJobTitle },
                    { "pname", strPageName },
                };

                SalSetupPageAds("Crr_JobDetail", hshAdParams);
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
                objData.SalCleanDataSet(ref dstSkillByBenchmarkJobCode);
                objData.SalCleanDataTable(ref dtbFiltedSkillData);
            }

            return View("~/Views/JobDetail/LayoutScripts/Seol_Crr_JobDetail.cshtml");
        }


        private void SalInitialPageParameters(Hashtable hshPageParams)
        {
            // all url parameters value as follows:
            hshPageParams["DatePostedType"] = (byte)DatePostedType.All; // ? t =
            hshPageParams["EmploymentType"] = (byte)EmploymentType.All; // ? et =
            hshPageParams["CareerSkillCodes"] = null; // ? sk =
            hshPageParams["IndustryFamilyCode"] = null; // ? in = 
            hshPageParams["WorkForHome"] = null; // ? r =
            hshPageParams["CareerJobPostingID"] = null; // ?i=
            hshPageParams["Keyword"] = null;    // ? q=
            hshPageParams["SearchKeywordType"] = (byte)SearchKeywordType.JobTitle; // ? qt =
            hshPageParams["CompanyID"] = null;  // ? ci = 
            hshPageParams["Page"] = "1"; // default is 1
            hshPageParams["CityName"] = null;
            hshPageParams["StateCode"] = null;
            hshPageParams["StateName"] = null;
            hshPageParams["ZipCode"] = null;
            hshPageParams["Location"] = Seog_Crr_Constant.UnitedStatesText;   // ? l = 

            hshPageParams["CareerJobPostingIDFromFormSubmit"] = null;
            hshPageParams["KeywordFromFormSubmit"] = null;
        }

        private void SalProcessFormParameterFromPostRequest(Hashtable hshPageParams)
        {
            try
            {
                if (Request.HttpMethod == "GET")
                    return;

                string strBody = new System.IO.StreamReader(Request.InputStream).ReadToEnd();
                var col = HttpUtility.ParseQueryString(strBody);
                #region filter
                if (!string.IsNullOrEmpty(col["t"])) // date posted
                {
                    hshPageParams["DatePostedType"] = Convert.ToInt32(col["t"]);
                }
                if (!string.IsNullOrEmpty(col["et"])) // employement type
                {
                    hshPageParams["EmploymentType"] = Convert.ToInt32(col["et"]);
                }
                if (!string.IsNullOrEmpty(col["sk"])) // skills
                {
                    hshPageParams["CareerSkillCodes"] = col["sk"];
                }
                if (!string.IsNullOrEmpty(col["in"])) // industry
                {
                    hshPageParams["IndustryFamilyCode"] = col["in"];
                }
                if (!string.IsNullOrEmpty(col["r"])) // Remote only
                {
                    hshPageParams["WorkForHome"] = col["r"];
                }
                #endregion filter

                if (!string.IsNullOrEmpty(col["p"])) // load more
                {
                    hshPageParams["Page"] = col["p"];
                }

                if (!string.IsNullOrEmpty(col["i"])) // career job posting id
                {
                    // don't chage the following code:
                    //hshPageParams["CareerJobPostingID"] = hshPageParams["CareerJobPostingIDFromFormSubmit"] = col["i"];

                    hshPageParams["CareerJobPostingID"] = hshPageParams["CareerJobPostingIDFromFormSubmit"] = Request["jid"];
                }

                if (!string.IsNullOrEmpty(col["q"])) // global kewword search
                {
                    hshPageParams["Keyword"] = hshPageParams["KeywordFromFormSubmit"] = Uri.UnescapeDataString(col["q"]);
                }

                if (!string.IsNullOrEmpty(col["qt"]))  // global kewword search by: 
                {
                    hshPageParams["SearchKeywordType"] = Convert.ToInt32(col["qt"]);

                    if (!string.IsNullOrEmpty(col["ci"]))  // if qt=company
                    {
                        hshPageParams["CompanyID"] = col["ci"];
                    }
                }


                string strLocation = Seog_Crr_Constant.UnitedStatesText;
                if (!string.IsNullOrEmpty(col["l"]))  // global location
                {
                    strLocation = Uri.UnescapeDataString(col["l"] ?? "");
                }

                if (string.Compare(strLocation, Seog_Crr_Constant.UnitedStatesText, true) != 0)
                {
                    Hashtable hshStates = HttpContext.Application["States"] as Hashtable;
                    string[] arrLocation = strLocation.Split(',');
                    if (arrLocation.Length > 1)
                    {
                        string strCityName = arrLocation[arrLocation.Length - 2].Trim();
                        string strStateCode = arrLocation[arrLocation.Length - 1].Trim();
                        if (hshStates.ContainsKey(strStateCode))
                        {
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

                } // if


            }
            catch (Exception ex)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
        }
    }
}