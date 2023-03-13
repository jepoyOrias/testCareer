using System.Web.Mvc;
using System.Collections;
using System.Configuration;
using CareerCom.Cool.App_Start;
using CareerCom.Cool.Models;
using CareerCom.Cool.Gadgets;
using System.Collections.Generic;
using System.Linq;

namespace CareerCom.Cool.Controllers
{
    [Authentication]
    public partial class CareerController : Controller
    {
        protected void SalFillCommonData()
        {
            ViewBag.ReleaseNum = ConfigurationManager.AppSettings["ReleaseNum"] ?? "1.0.0.1";
            ViewBag.DestinationHost = ConfigurationManager.AppSettings["DestinationHost"]?.ToLower();
            ViewBag.IsEnableDebugMode = ConfigurationManager.AppSettings["IsEnableDebugMode"];
        }

        protected void SalSetupPageAds(string strPageId, Hashtable hshAdParams)
        {
            var current = System.Web.HttpContext.Current;
            ViewBag.ObjAds = new Seom_Crr_GPTAdsLibrary(hshAdParams, current.Request, current.Response);
            ViewBag.AllAds = objXmlHelper.SalGetPageAdsFromAdsConfigXmlFile(strPageId, current.Request.Browser.IsMobileDevice);
        }
    }
}