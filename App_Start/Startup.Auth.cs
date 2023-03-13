using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using Owin.Security.Providers.LinkedIn;
using Microsoft.Owin.Security.MicrosoftAccount;
using System.Configuration;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System;
using Lucene.Net.Util;

namespace Career
{
    partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseKentorOwinCookieSaver();

            app.SetDefaultSignInAsAuthenticationType("ExternalCookie");

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ExternalCookie",
                AuthenticationMode = AuthenticationMode.Passive,
                CookieName = ".AspNet.ExternalCookie",
                ExpireTimeSpan = TimeSpan.FromMinutes(5)
            });

            var googleOAuth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId =  ConfigurationManager.AppSettings["GoogleClientId"],
                ClientSecret =  ConfigurationManager.AppSettings["GoogleClientSecret"],
                Provider = new GoogleOAuth2AuthenticationProvider()
                {


                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new Claim("email", context.User["email"].ToString()));
                        context.Identity.AddClaim(new Claim("id", context.User["id"].ToString()));
                        context.Identity.AddClaim(new Claim("given_name", context.User["given_name"].ToString()));
                        context.Identity.AddClaim(new Claim("family_name", context.User["family_name"].ToString()));
                        context.Identity.AddClaim(new Claim("picture", context.User["picture"].ToString()));
                        context.Identity.AddClaim(new Claim("name", context.User["name"].ToString()));
                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                }
            };
            googleOAuth2AuthenticationOptions.Scope.Add("email");
            googleOAuth2AuthenticationOptions.Scope.Add("profile");
            app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions);

            var facebookAuthenticationOptions = new FacebookAuthenticationOptions()
            {
                AppId = ConfigurationManager.AppSettings.Get("FacebookAppId"),
                AppSecret = ConfigurationManager.AppSettings.Get("FacebookAppSecret"),
                Provider = new FacebookAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {

                        context.Identity.AddClaim(new Claim("email", context.User["email"].ToString()));
                        context.Identity.AddClaim(new Claim("id", context.User["id"].ToString()));
                        context.Identity.AddClaim(new Claim("given_name", context.User["first_name"].ToString()));
                        context.Identity.AddClaim(new Claim("family_name", context.User["last_name"].ToString()));
                        //Need to verify business in 
                        //  context.Identity.AddClaim(new Claim("picture", context.User["picture"].ToString()));
                        context.Identity.AddClaim(new Claim("picture", "https://graph.facebook.com/v5.0/me/picture?redirect=false&type=normal&access_token="+context.AccessToken));


                        context.Identity.AddClaim(new Claim("name", context.User["name"].ToString()));
                        return System.Threading.Tasks.Task.CompletedTask;
                    }
                }

            };
       
            facebookAuthenticationOptions.Scope.Add("email");
          
            app.UseFacebookAuthentication(facebookAuthenticationOptions);


            app.UseLinkedInAuthentication(
                clientId:ConfigurationManager.AppSettings["LinkedInAPIKey"],
                clientSecret :ConfigurationManager.AppSettings["LinkedInAPISecret"]
            );



            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountAuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings.Get("MicrosoftAPIKey"),
                ClientSecret = ConfigurationManager.AppSettings.Get("MicrosoftAPISecret"),
               
            });
        }
    }
}
