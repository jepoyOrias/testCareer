using System.Web;
using System.Web.Mvc;
using CareerCom.Cool.Gadgets;

namespace CareerCom.Cool.App_Start
{
    public class AuthenticationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly Seog_Crr_Cookie objCookie = new Seog_Crr_Cookie();
        public void OnAuthorization(AuthorizationContext objFilterContext)
        {

        }

    }
}