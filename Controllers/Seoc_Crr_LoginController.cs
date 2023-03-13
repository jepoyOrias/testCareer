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
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Linq;

namespace CareerCom.Cool.Controllers
{
    public partial class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string provider ,string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //[HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {

            //TestViewModel model = new TestViewModel();


            var authenticateResult = await HttpContext.GetOwinContext().Authentication.AuthenticateAsync("ExternalCookie");
            if (authenticateResult != null)
            {

                var providerKey = authenticateResult.Identity.Claims.FirstOrDefault(claim => claim.Type == "id");
                var name = authenticateResult.Identity.Claims.FirstOrDefault(claim => claim.Type == "name");
                var email = authenticateResult.Identity.Claims.FirstOrDefault(claim => claim.Type == "email");
                var picture = authenticateResult.Identity.Claims.FirstOrDefault(claim => claim.Type == "picture");
             


                    //model.Id = id.Value;
                    //model.Name = name.Value;
                    //model.Picture = picture.Value;
                    //model.Email = email.Value;

                    //  ViewBag.userInfo = model;

                    return RedirectToLocal(returnUrl);
                
            }
            return RedirectToLocal("/");
        }

        // Implementation copied from a standard MVC Project, with some stuff
        // that relates to linking a new external login to an existing identity
        // account removed.
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }



        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("CrrIndex", "Career");
        }

    }
}