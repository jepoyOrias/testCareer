using System;
using System.Web;
using CareerCom.Cool.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using CareerCom.Cool.Gadgets;
using CareerCom.Cool.App_Start;
using System.Collections;

namespace Career
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly Seog_Crr_Log objLog = new Seog_Crr_Log();

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;

            Seog_Crr_ApplicationCache objAppCache = new Seog_Crr_ApplicationCache();
            objAppCache.SalAddDataToCache();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();

            var errorArea = string.Empty;
            var errorController = "Career";
            var errorAction = "Error";
            var pathToViewFile = $"~/Views/Shared/Error.cshtml";

            var requestControllerName = "Career";
            var requestActionName = "ErrorHandler";

            var controller = new CareerController();
            var routeData = new RouteData { DataTokens = { { "area", errorArea } }, Values = { { "controller", errorController }, { "action", errorAction } } };
            var controllerContext = new ControllerContext(new HttpContextWrapper(HttpContext.Current), routeData, controller);

            controller.ControllerContext = controllerContext;

            try
            {
                var sw = new System.IO.StringWriter();
                var razorView = new RazorView(controller.ControllerContext, pathToViewFile, "", false, null);
                var model = new ViewDataDictionary(new HandleErrorInfo(exception, requestControllerName, requestActionName));
                var viewContext = new ViewContext(controller.ControllerContext, razorView, model, new TempDataDictionary(), sw);
                viewContext.ViewBag.RelativeUrl = controller.HttpContext.Request.RawUrl;
                viewContext.ViewBag.IsDebugMode = controller.HttpContext.Request.QueryString["force_debug"] == "1" ? true : false;

                objLog.SalError("Application_Error", exception);

                controller.HttpContext.Response.StatusCode = 404;
                controller.HttpContext.Response.TrySkipIisCustomErrors = true;

                //for adobe tracking
                viewContext.ViewBag.titletype = "na";
                //TBD: Add more parameters here

                Hashtable hshPageParams = new Hashtable();
                viewContext.ViewBag.PageParams = hshPageParams;

                razorView.Render(viewContext, sw);
                HttpContext.Current.Response.Write(sw);
            }
            catch (Exception ex)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            finally
            {
                Server.ClearError();
                HttpContext.Current?.Response.End();
            }
        }
    }
}
