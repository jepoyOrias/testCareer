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
        public ActionResult CrrIndex()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                hshPageParams["Location"] = null;

                ViewBag.PageParams = hshPageParams;

                SalFillCommonData();

                //TBD: adobe/ads for tracking data parameters here
                ViewBag.PrimaryCategoryForAdobeTracking = "homepage";
                ViewBag.PageNameForAdobeTracking = "homepage:landingpage";
            }
            catch (Exception ex)
            {
                objLog.SalError("CrrIndex", ex);
            }

            return View("~/Views/Index/LayoutScripts/Seol_Crr_Index.cshtml");
        }

        public ActionResult ErrorHandler(string strRequestActionName, Exception objException = null, string strRequestControllerName = "JobSalary", bool bolIsDebugMode = false)
        {
            ViewBag.RelativeUrl = HttpContext.Request.RawUrl;
            this.HttpContext.Response.StatusCode = 404;
            this.HttpContext.Response.TrySkipIisCustomErrors = true;

            //for adobe tracking
            ViewBag.titletype = "na";
            //TBD: Add more parameters here

            Hashtable hshPageParams = new Hashtable();
            ViewBag.IsDebugMode = bolIsDebugMode;

            SalFillCommonData();
            objLog.SalError($"Current Url: {ViewBag.RelativeUrl} An error cause, ActionName:{strRequestActionName};ControllerName:{strRequestControllerName} {objException?.StackTrace}");

            var model = new HandleErrorInfo(objException, strRequestControllerName, strRequestActionName ?? "ErrorHandler");
            return View("~/Views/Shared/Error.cshtml", model);
        }


    }
}