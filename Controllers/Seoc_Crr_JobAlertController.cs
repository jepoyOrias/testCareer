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
        public ActionResult CareerJobPosting_JobAlert()
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
                objLog.SalError("CareerJobPosting_JobAlert", ex);
            }
            return View("~/Views/JobAlert/LayoutScripts/Seol_Crr_JobAlert.cshtml");
        }

    }
}