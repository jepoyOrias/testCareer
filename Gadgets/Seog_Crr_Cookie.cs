using System;
using System.Web;
using System.Configuration;


namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_Cookie
    {
        private readonly string strCookieDomain = ConfigurationManager.AppSettings["CookieDomain"] ?? "career.com";

        public string SalGetUserHRIDFromDomainCookie(HttpCookieCollection httpCookie)
        {
            var hrID = httpCookie != null && httpCookie["mhr"] != null && httpCookie["mhr"]["mhi"] != null ? httpCookie["mhr"]["mhi"] : "";
            var myHRID = HttpContext.Current.Server.UrlDecode(hrID);
            return myHRID;
        }

        private void SalSetRateLimitCookie(HttpContextBase requestContext, DateTime dtFirstSubmitTime, int intRateLimit)
        {
            string strCookieForRateLimit = $"z={dtFirstSubmitTime}|||r={intRateLimit}";
            HttpCookie cookieForRateLimit = new HttpCookie("_x_c");
            cookieForRateLimit.Value = HttpContext.Current.Server.UrlEncode(strCookieForRateLimit);
            cookieForRateLimit.Domain = strCookieDomain;
            cookieForRateLimit.Expires = DateTime.Now.AddDays(1);
            requestContext.Response.Cookies.Add(cookieForRateLimit);
        }

        public bool SalIsRequestExceedQuota(HttpContextBase requestContext)
        {
            string strCookieForRateLimit = string.Empty;
            DateTime dtFirstSumbimtDateTime = DateTime.UtcNow;
            int intRateLimit = 0;
            var cookies = requestContext.Request.Cookies;
            int intMaxRateLimit = Convert.ToInt32(ConfigurationManager.AppSettings["IPRateLimit"] ?? "3");
            if (cookies["_x_c"] != null)
            {
                strCookieForRateLimit = requestContext.Server.UrlDecode(cookies["_x_c"].Value);
                if (!string.IsNullOrEmpty(strCookieForRateLimit))
                {
                    string[] arrRateLimit = strCookieForRateLimit.Split("|||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] arrCookieValue = null;
                    foreach (string strCookieValue in arrRateLimit)
                    {
                        arrCookieValue = strCookieValue.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (arrCookieValue.Length > 1)
                        {
                            switch (arrCookieValue[0])
                            {
                                case "z":
                                    dtFirstSumbimtDateTime = DateTime.Parse(arrCookieValue[1]);
                                    break;
                                case "r":
                                    int.TryParse(arrCookieValue[1], out intRateLimit);
                                    break;
                            }
                        }
                    }

                    if (intRateLimit <= intMaxRateLimit)
                    {
                        SalSetRateLimitCookie(requestContext, dtFirstSumbimtDateTime, intRateLimit + 1);
                    }
                }

            }
            else
            {
                SalSetRateLimitCookie(requestContext, DateTime.UtcNow, 1);
            }

            if (intRateLimit >= intMaxRateLimit)
                return true;
            return false;
        }
    }
}

