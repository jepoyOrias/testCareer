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
        public ActionResult CareerJobPosting_SearchCompanyiesByIndustry()
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
                objLog.SalError("CareerJobPosting_SearchCompanyiesByIndustry", ex);
            }
            return View("~/Views/Company/LayoutScripts/Seol_Crr_Company_ByIndustry.cshtml");
        }

        public ActionResult CareerJobPosting_BrowseCompanyDetail()
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
                objLog.SalError("CareerJobPosting_BrowseCompanyDetail", ex);
            }
            return View("~/Views/Company/LayoutScripts/Seol_Crr_Company_CompanyDetail.cshtml");
        }

        public ActionResult CareerJobPosting_SearchCompanyiesByIndustry_DrillIn()
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
                objLog.SalError("CareerJobPosting_SearchCompanyiesByIndustry_DrillIn", ex);
            }
            return View("~/Views/Company/LayoutScripts/Seol_Crr_Company_ByIndustry_DrillIn.cshtml");
        }


    }
}