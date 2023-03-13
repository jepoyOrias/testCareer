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

namespace CareerCom.Cool.Controllers
{
    public partial class CareerController : Controller
    {
        public ActionResult CareerJobPosting_SavedJobs()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SavedJobs", ex);
            }
            return View("~/Views/SavedJob/LayoutScripts/Seol_Crr_SavedJob.cshtml");
        }
        public ActionResult CareerJobPosting_SearchJobsByIndustry()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsByIndustry", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_ByIndustry.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsByIndustry_DrillIn()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsByIndustry", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_ByIndustry_DrillIn.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsByState()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsByIndustry", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_ByState.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsByState_DrillIn()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsByIndustry", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_ByState_DrillIn.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsByCity()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsByCity", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_ByCity.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsByCity_DrillIn()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsByCity_DrillIn", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_ByCity_DrillIn.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsByCategory()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsByCategory", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_ByCategory.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsByCategory_DrillIn()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsByCategory_DrillIn", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_ByCategory_DrillIn.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsBySkill()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsBySkill", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_BySkill.cshtml");
        }

        public ActionResult CareerJobPosting_SearchJobsBySkill_DrillIn()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "";
                ViewBag.PageNameForAdobeTracking = "";
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_SearchJobsBySkill_DrillIn", ex);
            }
            return View("~/Views/Jobs/LayoutScripts/Seol_Crr_Jobs_BySkill_DrillIn.cshtml");
        }

    }
}