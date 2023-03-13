using System;
using System.Web;
using System.Collections;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using CareerCom.Cool.Gadgets;

namespace CareerCom.Cool.Models
{
    public class Seom_Crr_GPTAdsLibrary
    {
        Seog_Crr_Log objLog = new Seog_Crr_Log();

        private string strCookieDomain { get; set; } = string.Empty;
        public string strAdsGPTID { get; set; } = "5588";
        public string strAdsAreaID { get; set; } = string.Empty;
        public string strAdsSIP { get; set; } = string.Empty;
        public string strMVCadsUrl { get; set; } = string.Empty;
        public string strAdsContentSiteID { get; set; } = string.Empty;
        public string strAdsFirstTierSite { get; set; } = string.Empty;
        public string strAdsEnv { get; set; } = string.Empty;
        public string strAdsJobCode { get; set; } = string.Empty;
        public string strAdsJobTitle { get; set; } = string.Empty;
        public string strAdsNarrowCode { get; set; } = string.Empty;
        public string strAdsWSRCode { get; set; } = string.Empty;
        public string strAdsGeoMetroCode { get; set; } = string.Empty;
        public string strAdsZipCode { get; set; } = string.Empty;
        public string strAdsStateCode { get; set; } = string.Empty;
        public string strAdsCity { get; set; } = string.Empty;
        public string strAdsCountry { get; set; } = string.Empty;
        public string strAdsJobFamilyCode { get; set; } = string.Empty;
        public string strAdsIndustryFamilyCode { get; set; } = string.Empty;
        public string strAdsEducationCode { get; set; } = string.Empty;
        public string strAdsFTE { get; set; } = string.Empty;
        public string strAdsSemSeo { get; set; } = string.Empty;
        public string strAdsJobLevelCode { get; set; } = string.Empty;
        public string strAdsJobLevelDesc { get; set; } = string.Empty;
        public string strAdsKeyWord { get; set; } = string.Empty;
        public string strAdSrc { get; set; } = string.Empty;
        public string strAdsPageName { get; set; } = string.Empty;
        public string strSearchKeyword { get; set; } = string.Empty;
        public string strAdsPNarrowCode { get; set; } = string.Empty;
        public string strAdsPJobCode { get; set; } = string.Empty;
        public string strAdsPJobLevelCode { get; set; } = string.Empty;
        public string strAdsPJobLevelDesc { get; set; } = string.Empty;
        public string strAdsPActualJobTitle { get; set; } = string.Empty;
        public string strAdsPSal { get; set; } = string.Empty;
        public string strAdsPYearsOfOccupationExp { get; set; } = string.Empty;
        public string strAdsPReportToLevelCode { get; set; } = string.Empty;
        public string strAdsPLoc { get; set; } = string.Empty;
        public string strAdsPIndustryFamilyCode { get; set; } = string.Empty;
        public string strAdsPFTERangeCode { get; set; } = string.Empty;
        public string strAdsPEducationCode1 { get; set; } = string.Empty;
        public string strAdsTitleType { get; set; } = "na";
        public string strChannelId { get; set; }

        public Seom_Crr_GPTAdsLibrary() { }
        public Seom_Crr_GPTAdsLibrary(Hashtable hshParam, HttpRequest Request, HttpResponse Response)
        {
            try
            {

                //ContentSiteID
                strCookieDomain = WebConfigurationManager.AppSettings["CookieDomain"] ?? "career.com";

                //ContentSiteID
                strAdsContentSiteID = WebConfigurationManager.AppSettings["contentsiteid"] ?? "career";

                strMVCadsUrl = WebConfigurationManager.AppSettings["MVCADSURL"] ?? string.Empty;
                //AreaID

                strAdsAreaID = WebConfigurationManager.AppSettings["areaid"];

                //FirstTierSite
                if (String.Compare(strAdsContentSiteID, "salary", true) != 0)    //Content Site
                {
                    strAdsFirstTierSite = strAdsContentSiteID.ToLower() + ".salary";
                }
                else
                {
                    strAdsFirstTierSite = strAdsContentSiteID.ToLower();
                }

                //SearchKeyword
                if (hshParam["searchkeyword"] != null)
                {
                    strSearchKeyword = hshParam["searchkeyword"].ToString() ?? string.Empty;
                }

                //Env
                string strHostURL = string.Empty;
                strHostURL = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
                Match objMahMatch = Regex.Match(strHostURL.ToLower(), @"(^(salarystaging[0-9]+\.)|(192\.)|(intranet)|(localhost)|(data.career.com)|(delta.career.com))");
                if (objMahMatch.Success)
                {
                    strAdsEnv = "S";
                }
                else
                {
                    strAdsEnv = "P";
                }

                //AdsSIP
                if (hshParam["sip"] != null)
                {
                    strAdsSIP = hshParam["sip"].ToString();
                }

                //AdsPageName
                if (hshParam["pname"] != null)
                {
                    strAdsPageName = hshParam["pname"].ToString();
                }

                //AdsJobCode
                if (hshParam["jobcode"] != null)
                {
                    strAdsJobCode = hshParam["jobcode"].ToString();
                }

                //AdsNarrowCode
                if (hshParam["narrowcode"] != null)
                {
                    strAdsNarrowCode = hshParam["narrowcode"].ToString();
                }

                //AdsWSRCode
                if (hshParam["wsrcode"] != null)
                {
                    strAdsWSRCode = hshParam["wsrcode"].ToString();
                }

                //AdsJobFamilyCode
                if (hshParam["jobfamilycode"] != null)
                {
                    strAdsJobFamilyCode = hshParam["jobfamilycode"].ToString();
                }

                //AdsIndustryFamilyCode
                if (hshParam["industryfamilycode"] != null)
                {
                    strAdsIndustryFamilyCode = hshParam["industryfamilycode"].ToString();
                }

                //AdsEducationCode
                if (hshParam["educationcode"] != null)
                {
                    strAdsEducationCode = hshParam["educationcode"].ToString();
                }

                if (hshParam["companysize"] != null)
                {
                    strAdsFTE = hshParam["companysize"].ToString();
                }

                if (hshParam["semseo"] != null)
                {
                    strAdsSemSeo = hshParam["semseo"].ToString();
                }

                //AdsJobLevelDesc
                if (hshParam["joblevelcode"] != null)
                {
                    strAdsJobLevelCode = hshParam["joblevelcode"].ToString();
                }
                strAdsJobLevelDesc = SalGetJobLevelDescByJobLevelCode(strAdsJobLevelCode, strAdsAreaID);

                //AdsJobTitle
                if (hshParam["jobtitle"] != null)
                {
                    strAdsJobTitle = hshParam["jobtitle"].ToString();
                }

                //AdsNarrowDesc
                string strAdsNarrowDesc = string.Empty;
                if (hshParam["narrowdesc"] != null)
                {
                    strAdsNarrowDesc = hshParam["narrowdesc"].ToString();
                }
                //AdsKeyWord
                //add a new ads variable adsKeyWord for google ads. Try to use keyword value first. If it is empty ) try to use jobtitle value. If it is still empty ) use location
                if (hshParam["keyword"] != null)
                {
                    strAdsKeyWord = hshParam["keyword"].ToString();
                }
                if (string.IsNullOrEmpty(strAdsKeyWord))
                {
                    strAdsKeyWord = strAdsJobTitle;
                }
                if (string.IsNullOrEmpty(strAdsKeyWord))
                {
                    if (!string.IsNullOrEmpty(strAdsNarrowDesc))
                    {
                        strAdsKeyWord = strAdsNarrowDesc;
                    }
                }

                if (hshParam["channelid"] != null)
                {
                    strChannelId = hshParam["channelid"].ToString();
                }

                //AdsSrc
                if (!string.IsNullOrEmpty(strAdsKeyWord))
                {
                    strAdSrc = "pg";
                }

                //AdsGeoMetroCode
                if (hshParam["geometrocode"] != null)
                {
                    strAdsGeoMetroCode = hshParam["geometrocode"].ToString();
                }

                //AdsCountry
                if (hshParam["country"] != null)
                {
                    strAdsCountry = hshParam["country"].ToString();
                }

                //AdsZipCode
                if (hshParam["zipcode"] != null)
                {
                    strAdsZipCode = hshParam["zipcode"].ToString();
                }

                //AdsStateCode
                if (hshParam["statecode"] != null)
                {
                    strAdsStateCode = hshParam["statecode"].ToString();
                }
                if (strAdsAreaID.ToUpper() == "CSWZ")
                {
                    strAdsStateCode = "";
                }

                if (hshParam["city"] != null)
                {
                    strAdsCity = hshParam["city"].ToString();
                }

                #region handle especial strAdsZipCode and fill strAdsZipCode, strAdsStateCode and strAdsCity when they is empty
                //AdsGeo
                string strAdsLocation = string.Empty;
                if (hshParam["location"] != null)
                {
                    strAdsLocation = hshParam["location"].ToString();
                }

                string[] arrZSC = null;
                if (strAdsZipCode.Length > 5)
                    arrZSC = SalSeparateLoacation(strAdsZipCode);
                else
                    arrZSC = SalSeparateLoacation(strAdsLocation);

                strAdsZipCode = arrZSC[0];
                strAdsStateCode = string.IsNullOrEmpty(strAdsStateCode) ? arrZSC[1] : strAdsStateCode;
                strAdsCity = string.IsNullOrEmpty(strAdsCity) ? arrZSC[2] : strAdsCity;
                #endregion


                #region read value from cookie SALAD
                //we are going to use following order to display and write the cookie for SALAD
                //strSALAD = "adsJobCode=0|||adsNarrowCode=1|||adsWSRCode=2|||adsGeoMetroCode=3|||adsStateCode=4|||adsSIP=5|||adsJobFamilyCode=6|||adsIndustryFamilyCode=7|||adsEducationCode=8|||adsFTE=9|||adsSemSeo=10|||adsJobLevelDesc=11" 
                string strSALAD = string.Empty;
                if (Request.Cookies["SALAD"] != null)
                {
                    strSALAD = HttpContext.Current.Server.UrlDecode(Request.Cookies["SALAD"].Value);
                }
                if (strSALAD != "")
                {
                    string[] arrSalad = strSALAD.Split("|||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] arrCookieValue = null;

                    foreach (string strCookieValue in arrSalad)
                    {
                        arrCookieValue = strCookieValue.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (arrCookieValue.Length > 1)
                        {

                            switch (arrCookieValue[0])
                            {
                                case "adsJobCode":
                                    if (string.IsNullOrEmpty(strAdsJobCode))
                                    {
                                        strAdsJobCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsNarrowCode":
                                    if (string.IsNullOrEmpty(strAdsNarrowCode))
                                    {
                                        strAdsNarrowCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsWSRCode":
                                    if (string.IsNullOrEmpty(strAdsWSRCode))
                                    {
                                        strAdsWSRCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsGeoMetroCode":
                                    if (string.IsNullOrEmpty(strAdsGeoMetroCode))
                                    {
                                        strAdsGeoMetroCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsStateCode":
                                    if (string.IsNullOrEmpty(strAdsStateCode))
                                    {
                                        strAdsStateCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsSIP":
                                    if (string.IsNullOrEmpty(strAdsSIP))
                                    {
                                        strAdsSIP = arrCookieValue[1];
                                    }
                                    break;

                                case "adsJobFamilyCode":
                                    if (string.IsNullOrEmpty(strAdsJobFamilyCode))
                                    {
                                        strAdsJobFamilyCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsIndustryFamilyCode":
                                    if (strAdsIndustryFamilyCode == "")
                                    {
                                        strAdsIndustryFamilyCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsEducationCode":
                                    if (strAdsEducationCode == "")
                                    {
                                        strAdsEducationCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsFTE":
                                    if (strAdsFTE == "")
                                    {
                                        strAdsFTE = arrCookieValue[1];
                                    }
                                    break;


                                case "adsSemSeo":
                                    if (strAdsSemSeo == "")
                                    {
                                        strAdsSemSeo = arrCookieValue[1];
                                    }
                                    break;

                                case "adsJobLevelDesc":
                                    if (strAdsJobLevelDesc == "")
                                    {
                                        strAdsJobLevelDesc = arrCookieValue[1];
                                    }
                                    break;

                                case "adsKeyword":
                                    if (strAdsKeyWord == "")
                                    {
                                        strAdsKeyWord = arrCookieValue[1];
                                    }
                                    if (strAdsKeyWord != "" && strAdSrc.Length == 0)
                                    {
                                        strAdSrc = "ck";
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
                #endregion read value from cookie SALAD

                #region read value from cookie SALADP
                //we are going to use following order to display and write the cookie for SALADP
                //strSALADP = "adsPNarrowCode=0|||adsPJobCode=1|||adsPJobLevelCode=2|||adsPJobLevelDesc=3|||adsPActualJobTitle=4|||adsPSal=5|||adsPYearsOfOccupationExp=6|||adsPReportToLevelCode=7|||adsPLoc=8|||adsPIndustryFamilyCode=9|||adsPFTERangeCode=10|||adsPEducationCode1=11"
                //cookie code begin
                string strSALADP = string.Empty;
                if (Request.Cookies["SALADP"] != null)
                {
                    strSALADP = HttpContext.Current.Server.UrlDecode(Request.Cookies["SALADP"].Value);
                }


                if (strSALADP != "")
                {
                    string[] aSALADP = strSALADP.Split("|||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] arrCookieValue = null;

                    foreach (string strCookieValue in aSALADP)
                    {
                        arrCookieValue = strCookieValue.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (arrCookieValue.Length > 1)
                        {
                            switch (arrCookieValue[0])
                            {
                                case "adsPNarrowCode":
                                    if (strAdsPNarrowCode == "")
                                    {
                                        strAdsPNarrowCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPJobCode":
                                    if (strAdsPJobCode == "")
                                    {
                                        strAdsPJobCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPJobLevelCode":
                                    if (strAdsPJobLevelCode == "")
                                    {
                                        strAdsPJobLevelCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPJobLevelDesc":
                                    if (strAdsPJobLevelDesc == "")
                                    {
                                        strAdsPJobLevelDesc = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPActualJobTitle":
                                    if (strAdsPActualJobTitle == "")
                                    {
                                        strAdsPActualJobTitle = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPSal":
                                    if (strAdsPSal == "")
                                    {
                                        strAdsPSal = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPYearsOfOccupationExp":
                                    if (strAdsPYearsOfOccupationExp == "")
                                    {
                                        strAdsPYearsOfOccupationExp = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPReportToLevelCode":
                                    if (strAdsPReportToLevelCode == "")
                                    {
                                        strAdsPReportToLevelCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPLoc":
                                    if (strAdsPLoc == "")
                                    {
                                        strAdsPLoc = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPIndustryFamilyCode":
                                    if (strAdsPIndustryFamilyCode == "")
                                    {
                                        strAdsPIndustryFamilyCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPFTERangeCode":
                                    if (strAdsPFTERangeCode == "")
                                    {
                                        strAdsPFTERangeCode = arrCookieValue[1];
                                    }
                                    break;

                                case "adsPEducationCode1":
                                    if (strAdsPEducationCode1 == "")
                                    {
                                        strAdsPEducationCode1 = arrCookieValue[1];
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
                #endregion read value from cookie SALADP

                if (strAdsSemSeo == "")
                {
                    strAdsSemSeo = "Organic";
                }
                #region update cookie value for SALAD
                string strisdenysaladsc = string.Empty;
                bool bIsDenyAdsCookie = false;
                if (Request.Cookies["isdenysaladsc"] != null)
                {
                    strisdenysaladsc = HttpContext.Current.Server.UrlDecode(Request.Cookies["isdenysaladsc"].Value);
                }

                if (strisdenysaladsc.ToLower() == "deny")
                    bIsDenyAdsCookie = true;

                if (bIsDenyAdsCookie)
                {
                    strSALAD = "";
                }
                else
                {
                    strSALAD = "adsJobCode=" + strAdsJobCode + "|||adsNarrowCode=" + strAdsNarrowCode +
                        "|||adsWSRCode=" + strAdsWSRCode + "|||adsGeoMetroCode=" + strAdsGeoMetroCode +
                        "|||adsStateCode=" + strAdsStateCode + "|||adsSIP=" + strAdsSIP +
                        "|||adsJobFamilyCode=" + strAdsJobFamilyCode + "|||adsIndustryFamilyCode=" + strAdsIndustryFamilyCode +
                        "|||adsEducationCode=" + strAdsEducationCode + "|||adsFET=" + strAdsFTE +
                        "|||adsSemSeo=" + strAdsSemSeo + "|||adsJobLevelDesc=" + strAdsJobLevelDesc +
                        "|||adsKeyword=" + strAdsKeyWord;
                }
                HttpCookie ckiSALAD = new HttpCookie("SALAD");
                ckiSALAD.Value = HttpContext.Current.Server.UrlEncode(strSALAD);
                ckiSALAD.Domain = strCookieDomain;
                ckiSALAD.Expires = DateTime.Now.AddYears(2);
                Response.Cookies.Add(ckiSALAD);

                #endregion update cookie value for SALAD

                if (hshParam["titletype"] != null)
                {
                    strAdsTitleType = hshParam["titletype"].ToString();
                }
                else
                {
                    strAdsTitleType = "na";
                }
            }
            catch (Exception ex)
            {
                objLog.SalError("Seom_Red_GPTAdsLibrary: ", ex);
            }
            finally { }
        }

        private string[] SalSeparateLoacation(string strLocation)
        {
            string[] arrResult = new string[] { "", "", "" };
            if (!string.IsNullOrEmpty(strLocation))
            {
                string[] strArr = strLocation.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                switch (strArr.Length.ToString())
                {
                    case "1":
                        if (SalIsZipCode(strArr[0]))
                            arrResult[0] = strArr[0].Trim();
                        else
                            arrResult[2] = strArr[0].Trim();
                        break;
                    case "2":
                        if (SalIsZipCode(strArr[0]))
                        {
                            arrResult[0] = strArr[0].Trim();
                            arrResult[2] = strArr[1].Trim();
                        }
                        else
                        {
                            arrResult[1] = strArr[1].Trim();
                            arrResult[2] = strArr[0].Trim();
                        }
                        break;
                    case "3":
                        if (SalIsZipCode(strArr[0]))
                        {
                            arrResult[0] = strArr[0].Trim();
                            arrResult[1] = strArr[2].Trim();
                            arrResult[2] = strArr[1].Trim();
                        }
                        else if (SalIsZipCode(strArr[2]))
                        {
                            arrResult[0] = strArr[2].Trim();
                            arrResult[1] = strArr[1].Trim();
                            arrResult[2] = strArr[0].Trim();
                        }
                        break;
                }
            }
            return arrResult;
        }

        private bool SalIsZipCode(string strTarget)
        {
            if (strTarget.Trim().Length == 5)
            {
                Regex regex = new Regex(@"^\d+$");
                return regex.IsMatch(strTarget);
            }
            else
                return false;

        }

        private string SalGetJobLevelDescByJobLevelCode(string strAdsJobLevelCode, string strAdsAreaID)
        {
            string strAdsJobLevelDesc = string.Empty;
            switch (strAdsJobLevelCode)
            {
                case "1":
                case "2":
                    strAdsJobLevelDesc = "Entry";
                    break;

                case "3":
                case "4":
                case "5":
                    strAdsJobLevelDesc = "Mid";
                    break;

                case "6":
                case "7":
                    strAdsJobLevelDesc = "Executive";
                    break;

            }

            if (strAdsAreaID.ToUpper() == "Exc")
            {
                strAdsJobLevelDesc = "Executive";
            }

            return strAdsJobLevelDesc;
        }
    }

    //  enum variable name should map one-to-one to adsconfig.xml 
    public enum AdsPositionType
    {
        None,
        TopAd,
        BottomAd,
        RightNavAd,
        MobileAd,
        InContentAd
    }

    //// enum variable name should map one-to-one to adsconfig.xml 
    public enum AdsType : byte
    {
        None,
        /// <summary>
        /// gpt ads
        /// </summary>
        Ad,
        /// <summary>
        /// google text ads
        /// </summary>
        GoogleAd,
        MediaNetAd,
        WidgetAd
    }

    public class AdsUnit
    {
        public string ID { get; set; }
        public string CssClass { get; set; }
        public string Pos { get; set; }
        public string Size { get; set; }
        public string SizeMapping { get; set; }
        public string HtmlString { get; set; }
        public int Order { get; set; }
        public AdsPositionType AdsPositionType { get; set; }
        public AdsType AdsType { get; set; }
    }

}