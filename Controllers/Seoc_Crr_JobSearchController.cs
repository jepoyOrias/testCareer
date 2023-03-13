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
        public ActionResult CareerJobPosting_JobSearchIndex()
        {
            Seom_Crr_CareerJob objJob = new Seom_Crr_CareerJob();
            Seog_Crr_LuceneSearch objSearch = new Seog_Crr_LuceneSearch();
            try
            {
                SalFillCommonData();
            }
            catch (Exception ex)
            {

            }
            return View("~/Views/JobSearch/LayoutScripts/Seol_Crr_JobSearch.cshtml");
        }

    }
}