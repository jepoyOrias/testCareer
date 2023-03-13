using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using CareerCom.Cool.Models;

namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_ApplicationCache
    {
        private readonly Seog_Crr_Log objLog = new Seog_Crr_Log();
        private readonly Seog_Crr_XmlHelper objXmlHelper = new Seog_Crr_XmlHelper();

        public Seog_Crr_ApplicationCache() { }
        public void SalAddDataToCache()
        {
            SalAddAllAdsTypeConfigToCache();
            if (HttpContext.Current.Application["States"] == null)
            {
                Hashtable hstStates = SalAddStatesToCache(Seog_Crr_Constant.AllStatesXmlFilePath);
                HttpContext.Current.Application["States"] = hstStates;
            }
            if (HttpContext.Current.Application["JBDCompanyNames"] == null)
            {
                Hashtable hshJBDCompanyNames = SalAddCompanyNamesToCache(Seog_Crr_Constant.AllJBDCompanyXmlFilePath);
                HttpContext.Current.Application["JBDCompanyNames"] = hshJBDCompanyNames;
            }
            if (HttpContext.Current.Application["AllHotJobToBenchmarkJobCode"] == null)
            {
                Hashtable hshAllHotJobToBenchmarkJobCode = SalAddAllHotJobToBenchmarkCodeToCache(Seog_Crr_Constant.AllHotJobPostingTitleToBenchmarkJobCodeXmlFilePath);
                HttpContext.Current.Application["AllHotJobToBenchmarkJobCode"] = hshAllHotJobToBenchmarkJobCode;
            }
            if (HttpContext.Current.Application["Skills"] == null)
            {
                Hashtable hstSkills = SalAddCareerSkillsToCache();
                HttpContext.Current.Application["Skills"] = hstSkills;
            }
        }
        private Hashtable SalAddAllHotJobToBenchmarkCodeToCache(string strXmlPath)
        {
            XmlReader objXmlReader = null;
            Hashtable hshJobs = new Hashtable(StringComparer.OrdinalIgnoreCase);
            string strKey = string.Empty;
            string strValue = string.Empty;
            string strNodeKeyName = "JobPostingTitle";
            string strNodeValueName = "BenchmarkJobCode";

            string strFullXmlPath = objXmlHelper.SalGetCareerJobBoardFullXmlPath(strXmlPath);
            if (!System.IO.File.Exists(strFullXmlPath))
            {
                objLog.SalError($"SalAddAllHotJobToBenchmarkCodeToCache: can't find file {strFullXmlPath}");
                return hshJobs;
            }

            try
            {

                objXmlReader = XmlReader.Create(strFullXmlPath);
                objXmlReader.MoveToContent();
                while (objXmlReader.Read())
                {
                    if (objXmlReader.NodeType == System.Xml.XmlNodeType.Element && objXmlReader.Name == "row")
                    {
                        objXmlReader.ReadToDescendant(strNodeKeyName);


                        strKey = objXmlReader.ReadElementString(strNodeKeyName).Trim().ToLower();
                        strValue = objXmlReader.ReadElementString(strNodeValueName).Trim();

                        if (String.IsNullOrEmpty(strKey) || String.IsNullOrEmpty(strValue))
                        {
                            string strMsg = string.Format("SalAddAllHotJobToBenchmarkCodeToCache:{0} or {1} is null. FilePath={2}", strNodeKeyName, strNodeValueName, strFullXmlPath);
                            objLog.SalError(strMsg);
                            continue;
                        }

                        hshJobs[strKey] = strValue;
                    }
                }

            }
            catch (Exception ex)
            {
                objLog.SalError("SalAddJobsToCache", ex);
            }
            finally
            {
                if (objXmlReader != null)
                {
                    objXmlReader.Close();
                    objXmlReader.Dispose();
                    objXmlReader = null;
                }
            }

            return hshJobs;


        }
        private Hashtable SalAddCompanyNamesToCache(string strXmlPath)
        {
            XmlReader objXmlReader = null;
            Hashtable hshJobs = new Hashtable(StringComparer.OrdinalIgnoreCase);
            string strKey = string.Empty;
            string strValue = string.Empty;
            string strNodeKeyName = "SEOCompanyName";
            string strNodeValueName = "CompanyID";

            string strFullXmlPath = objXmlHelper.SalCompanyGetFullXmlPath(strXmlPath);
            if (!System.IO.File.Exists(strFullXmlPath))
            {
                objLog.SalError($"SalAddJobsToCache: can't find file {strFullXmlPath}");
                return hshJobs;
            }

            try
            {

                objXmlReader = XmlReader.Create(strFullXmlPath);
                objXmlReader.MoveToContent();
                while (objXmlReader.Read())
                {
                    if (objXmlReader.NodeType == System.Xml.XmlNodeType.Element && objXmlReader.Name == "row")
                    {
                        objXmlReader.ReadToDescendant(strNodeValueName);

                        strValue = objXmlReader.ReadElementString(strNodeValueName).Trim();
                        strKey = objXmlReader.ReadElementString(strNodeKeyName).Trim();


                        if (String.IsNullOrEmpty(strKey) || String.IsNullOrEmpty(strValue))
                        {
                            string strMsg = string.Format("SalAddJobsToCache:{0} or {1} is null. FilePath={2}", strNodeKeyName, strNodeValueName, strFullXmlPath);
                            objLog.SalError(strMsg);
                            continue;
                        }

                        hshJobs[strKey] = strValue;
                    }

                }

            }
            catch (Exception ex)
            {
                objLog.SalError("SalAddJobsToCache", ex);
            }
            finally
            {
                if (objXmlReader != null)
                {
                    objXmlReader.Close();
                    objXmlReader.Dispose();
                    objXmlReader = null;
                }
            }
            return hshJobs;


        }

        private Hashtable SalAddStatesToCache(string strXmlPath)
        {
            XmlReader objXmlReader = null;
            Hashtable hshStates = new Hashtable(StringComparer.OrdinalIgnoreCase);
            try
            {
                string strStateCode = "";
                string strStateName = "";
                string strFullXmlPath = objXmlHelper.SalGetJobSalaryFullXmlPath(strXmlPath);


                objXmlReader = XmlReader.Create(strFullXmlPath);
                objXmlReader.MoveToContent();
                while (objXmlReader.Read())
                {
                    if (objXmlReader.NodeType == System.Xml.XmlNodeType.Element && objXmlReader.Name == "row")
                    {
                        objXmlReader.ReadToDescendant("StateCode");
                        strStateCode = objXmlReader.ReadElementString("StateCode");
                        strStateName = objXmlReader.ReadElementString("State");

                        if (String.IsNullOrEmpty(strStateCode) || String.IsNullOrEmpty(strStateName))
                        {
                            objLog.SalError("SalAddStatesToCache" + "StateCode or State is null. FilePath=" + strXmlPath);
                            throw new ArgumentException("SalAddStatesToCache" + "StateCode or State is null. FilePath=" + strXmlPath);
                        }
                        hshStates[strStateCode.Trim()] = strStateName.Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                objLog.SalError($"{System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            }
            finally
            {
                if (objXmlReader != null)
                {
                    objXmlReader.Close();
                    objXmlReader.Dispose();
                    objXmlReader = null;
                }
            }
            return hshStates;
        }

        private Hashtable SalAddCareerSkillsToCache()
        {
            XmlReader objXmlReader = null;
            Hashtable hshSkills = new Hashtable(StringComparer.OrdinalIgnoreCase);
            string strKey = string.Empty;
            string strValue = string.Empty;
            string strNodeKeyName = "CareerSkillCode";
            string strNodeValueName = "CareerSkillName";

            string strFullXmlPath = objXmlHelper.SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.AllCareerSkillXmlFilePath);
            if (!System.IO.File.Exists(strFullXmlPath))
            {
                objLog.SalError($"SalAddCareerSkillsToCache: can't find file {strFullXmlPath}");
                return hshSkills;
            }

            try
            {
                objXmlReader = XmlReader.Create(strFullXmlPath);
                objXmlReader.MoveToContent();
                while (objXmlReader.Read())
                {
                    if (objXmlReader.NodeType == System.Xml.XmlNodeType.Element && objXmlReader.Name == "row")
                    {
                        objXmlReader.ReadToDescendant(strNodeKeyName);
                        strKey = objXmlReader.ReadElementString(strNodeKeyName).Trim();
                        strValue = objXmlReader.ReadElementString(strNodeValueName).Trim();

                        if (String.IsNullOrEmpty(strKey) || String.IsNullOrEmpty(strValue))
                        {
                            string strMsg = string.Format("SalAddCareerSkillsToCache:{0} or {1} is null. FilePath={2}", strNodeKeyName, strNodeValueName, strFullXmlPath);
                            objLog.SalError(strMsg);
                            return new Hashtable();
                        }
                        hshSkills[strKey] = strValue;
                    }
                }
            }
            catch (Exception ex)
            {
                objLog.SalError("SalAddCareerSkillsToCache", ex);
            }
            finally
            {
                if (objXmlReader != null)
                {
                    objXmlReader.Close();
                    objXmlReader.Dispose();
                    objXmlReader = null;
                }
            }
            return hshSkills;
        }


        private void SalAddAllAdsTypeConfigToCache()
        {
            if (HttpContext.Current.Application["AllMobileAds"] == null || HttpContext.Current.Application["AllDesktopAds"] == null)
            {
                Hashtable hshAllMobileAds = new Hashtable(StringComparer.OrdinalIgnoreCase);
                Hashtable hshAllDesktopAds = new Hashtable(StringComparer.OrdinalIgnoreCase);
                SalGetAllAdsTypeUnitFromAdsConfigXmlFile(ref hshAllMobileAds, ref hshAllDesktopAds);
                HttpContext.Current.Application["AllMobileAds"] = hshAllMobileAds;
                HttpContext.Current.Application["AllDesktopAds"] = hshAllDesktopAds;
            }
        }

        private void SalGetAllAdsTypeUnitFromXElement(XElement objXElement, string strCssClass, AdsPositionType adsPositionType, ref Hashtable hshAllAds)
        {
            foreach (XElement item in objXElement.Elements())
            {
                string strAdsId = item.Attribute("ID").Value;
                AdsType adsType = (AdsType)Enum.Parse(typeof(AdsType), item.Name.ToString());
                AdsUnit objAdsUnit = new AdsUnit { ID = strAdsId, AdsType = adsType, AdsPositionType = adsPositionType };
                switch (adsType)
                {
                    case AdsType.Ad:
                        objAdsUnit.Pos = item.Attribute("Pos").Value;
                        objAdsUnit.Size = item.Attribute("Size").Value;
                        objAdsUnit.SizeMapping = item.Attribute("SizeMapping").Value;
                        objAdsUnit.CssClass = item.Attribute("CssClass")?.Value ?? strCssClass;
                        break;
                    case AdsType.WidgetAd:
                        objAdsUnit.CssClass = item.Attribute("CssClass")?.Value ?? strCssClass;
                        objAdsUnit.HtmlString = item.Value;
                        break;
                    case AdsType.GoogleAd:
                        objAdsUnit.Order = Convert.ToInt32(item.Attribute("Order")?.Value ?? "1");
                        break;
                    case AdsType.MediaNetAd:
                        objAdsUnit.Size = item.Attribute("Size").Value;
                        break;
                }

                hshAllAds.Add(strAdsId, objAdsUnit);
            }
        }

        private void SalGetAllAdsTypeUnitFromAdsConfigXmlFile(ref Hashtable hshAllMobileAds, ref Hashtable hshAllDesktopAds)
        {
            string strAdsConfigXmlFilePath = HttpContext.Current.Server.MapPath("~/adsConfig.xml");
            using (StreamReader streamReader = new StreamReader(strAdsConfigXmlFilePath))
            using (XmlReader reader = XmlReader.Create(streamReader))
            {
                XElement objXElement = null;
                AdsPositionType adsPositionType;
                string strCssClass = null;
                reader.ReadToDescendant("AllAds");
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        adsPositionType = (AdsPositionType)Enum.Parse(typeof(AdsPositionType), reader.Name, true);
                        strCssClass = reader.GetAttribute("CssClass");
                        objXElement = XElement.ReadFrom(reader) as XElement;
                        // move to Name element
                        switch (adsPositionType)
                        {
                            case AdsPositionType.TopAd:
                            case AdsPositionType.RightNavAd:
                            case AdsPositionType.InContentAd:
                            case AdsPositionType.BottomAd:
                                SalGetAllAdsTypeUnitFromXElement(objXElement, strCssClass, adsPositionType, ref hshAllDesktopAds);
                                break;
                            case AdsPositionType.MobileAd:
                                SalGetAllAdsTypeUnitFromXElement(objXElement, strCssClass, adsPositionType, ref hshAllMobileAds);
                                break;
                        }
                    }
                }
            }
        }  // method

        public string SalGetStateNameByStateCode(string strStateCode)
        {
            Hashtable hstStates = (Hashtable)HttpContext.Current.Application["States"];
            if (!hstStates.ContainsKey(strStateCode.ToUpper()))
                return string.Empty;
            return hstStates[strStateCode.ToUpper()].ToString();
        }

    }
}