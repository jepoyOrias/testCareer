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
        public ActionResult CareerJobPosting_LegalIndex()
        {
            Hashtable hshPageParams = new Hashtable();
            try
            {
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_LegalIndex", ex);
            }
            return View("~/Views/Legal/Index/LayoutScripts/Seol_Crr_Legal_Index.cshtml");
        }

        public ActionResult CareerJobPosting_Legal_DSA()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_Legal_DSA", ex);
            }
            return View("~/Views/Legal/DSA/LayoutScripts/Seol_Crr_Legal_DSA.cshtml");
        }

        public ActionResult CareerJobPosting_Legal_PC()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_Legal_PC", ex);
            }
            return View("~/Views/Legal/PP/LayoutScripts/Seol_Crr_Legal_PC.cshtml");
        }

        public ActionResult CareerJobPosting_Legal_DPA()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_Legal_DPA", ex);
            }
            return View("~/Views/Legal/DPA/LayoutScripts/Seol_Crr_Legal_DPA.cshtml");
        }

        public ActionResult CareerJobPosting_Legal_PP()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_Legal_PP", ex);
            }
            return View("~/Views/Legal/PP/LayoutScripts/Seol_Crr_Legal_PP.cshtml");
        }

        public ActionResult CareerJobPosting_Legal_Accessibility()
        {
            Hashtable hshPageParams = new Hashtable();

            try
            {
                ViewBag.PageParams = hshPageParams;
                SalFillCommonData();
            }
            catch (Exception ex)
            {
                objLog.SalError("CareerJobPosting_Legal_Accessibility", ex);
            }
            return View("~/Views/Legal/Accessibility/LayoutScripts/Seol_Crr_Legal_Accessibility.cshtml");
        }


    }
}