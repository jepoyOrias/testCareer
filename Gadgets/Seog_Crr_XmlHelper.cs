using System;
using System.IO;
using System.Web;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using CareerCom.Cool.Models;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Collections.Concurrent;

namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_XmlHelper
    {
        private readonly Seog_Crr_Log objLog = new Seog_Crr_Log();
        private readonly Seog_Crr_Data objData = new Seog_Crr_Data();

        private readonly string strXmlVirtualPath;

        public Seog_Crr_XmlHelper()
        {
            string strIsMapth = WebConfigurationManager.AppSettings["isMapPath"];
            if (strIsMapth == "1")
                strXmlVirtualPath = HttpContext.Current.Server.MapPath(@"/career/docs/");
            else
                strXmlVirtualPath = @"F:\JobBoardDocs\";
        }
        public string SalGetCareerJobBoardFullXmlPath(string strFileName)
        {
            string strJobBoardXmlVirtualPath = string.Empty;
            string strIsMapth = WebConfigurationManager.AppSettings["isMapPath"];
            if (strIsMapth == "1")
                strJobBoardXmlVirtualPath = HttpContext.Current.Server.MapPath(@"/career/careerjobboarddocs/");
            else
                strJobBoardXmlVirtualPath = @"F:\CareerJobBoardDocs\";

            return Path.Combine(strJobBoardXmlVirtualPath, strFileName);
        }
        public string SalGetJobBoardFullXmlPath(string strFileName)
        {
            string strJobBoardXmlVirtualPath = string.Empty;
            string strIsMapth = WebConfigurationManager.AppSettings["isMapPath"];
            if (strIsMapth == "1")
                strJobBoardXmlVirtualPath = HttpContext.Current.Server.MapPath(@"/career/jobboarddocs/");
            else
                strJobBoardXmlVirtualPath = @"F:\JobBoardDocs\";

            return Path.Combine(strJobBoardXmlVirtualPath, strFileName);
        }
        public string SalCompanyGetFullXmlPath(string strFileName)
        {
            string strCompanyXmlVirtualPath = string.Empty;
            string strIsMapth = WebConfigurationManager.AppSettings["isMapPath"];
            if (strIsMapth == "1")
                strCompanyXmlVirtualPath = HttpContext.Current.Server.MapPath(@"/companydocs/");
            else
                strCompanyXmlVirtualPath = @"F:\CompanyDocs\";

            return Path.Combine(strCompanyXmlVirtualPath, strFileName);
        }
        public string SalGetHowToBecomeFullXmlPath(string strFileName)
        {
            string strHowToBecomeXmlVirtualPath = string.Empty;

            string strIsMapth = WebConfigurationManager.AppSettings["isMapPath"];
            if (strIsMapth == "1")
                strHowToBecomeXmlVirtualPath = HttpContext.Current.Server.MapPath(@"/docs/");
            else
                strHowToBecomeXmlVirtualPath = @"F:\CareerSEOContentLibrary\";

            return Path.Combine(strHowToBecomeXmlVirtualPath, strFileName);

        }
        public string SalGetJobSalaryFullXmlPath(string strFileName)
        {
            string strJobSalaryXmlVirtualPath = string.Empty;
            if (WebConfigurationManager.AppSettings["isMapPath"] == "1")
                strJobSalaryXmlVirtualPath = HttpContext.Current.Server.MapPath(@"/docs/");
            else
                strJobSalaryXmlVirtualPath = @"F:\docsalarycom\";

            return Path.Combine(strJobSalaryXmlVirtualPath, strFileName);
        }

        private DataSet SalReadXml(string strXmlFilePath)
        {
            DataSet dstData = new DataSet();
            FileStream objFileStream = null;
            try
            {
                if (!File.Exists(strXmlFilePath))
                {
                    objLog.SalError("SalReadXml. Not find file:" + strXmlFilePath);
                    return dstData;
                }

                objFileStream = new FileStream(strXmlFilePath, FileMode.Open, FileAccess.Read);
                dstData.ReadXml(objFileStream);
                return dstData;
            }
            catch (Exception ex)
            {
                objLog.SalError("SalReadXml. XmlFilePath=" + strXmlFilePath, ex);

                if (objFileStream != null)
                {
                    objFileStream.Close();
                    objFileStream.Dispose();
                    objFileStream = null;
                }
                return dstData;
            }
            finally
            {
                if (objFileStream != null)
                {
                    objFileStream.Close();
                    objFileStream.Dispose();
                    objFileStream = null;
                }
                objData.SalCleanDataSet(ref dstData);
            }
        }

        public ConcurrentDictionary<string, AdsUnit> SalGetPageAdsFromAdsConfigXmlFile(string strPageId, bool bolIsMobileDevice)
        {
            Hashtable hshAllAds = !bolIsMobileDevice ? (Hashtable)HttpContext.Current.Application["AllDesktopAds"] : (Hashtable)HttpContext.Current.Application["AllMobileAds"];

            ConcurrentDictionary<string, AdsUnit> objDicPageAds = new ConcurrentDictionary<string, AdsUnit>(StringComparer.OrdinalIgnoreCase);
            string strAdsConfigXmlFilePath = HttpContext.Current.Server.MapPath(@"~\adsConfig.xml");
            try
            {
                using (StreamReader streamReader = new StreamReader(strAdsConfigXmlFilePath))
                using (XmlReader reader = XmlReader.Create(streamReader))
                {
                    XElement objXElement = null;
                    AdsPositionType adsPositionType;
                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.IsStartElement("Page"))
                        {
                            if (!strPageId.Equals(reader.GetAttribute("ID"), StringComparison.OrdinalIgnoreCase))
                            {
                                reader.Skip();
                                continue;
                            }
                            if ("false".Equals(reader.GetAttribute("Show"), StringComparison.OrdinalIgnoreCase))
                            {
                                break;
                            }

                            using (var readerSubTree = reader.ReadSubtree())
                            {
                                readerSubTree.ReadToDescendant("Page");
                                readerSubTree.MoveToContent();
                                int intOrder = 1;
                                while (readerSubTree.Read())
                                {
                                    if (readerSubTree.NodeType == XmlNodeType.Element)
                                    {
                                        if (bolIsMobileDevice)  // directly skip to MobileAd section
                                        {
                                            readerSubTree.ReadToNextSibling("MobileAd");
                                            readerSubTree.MoveToContent();
                                        }

                                        // move to Name element
                                        adsPositionType = (AdsPositionType)Enum.Parse(typeof(AdsPositionType), readerSubTree.Name, true);
                                        // move to Name element
                                        switch (adsPositionType)
                                        {
                                            case AdsPositionType.TopAd:
                                            case AdsPositionType.RightNavAd:
                                            case AdsPositionType.InContentAd:
                                            case AdsPositionType.BottomAd:
                                                objXElement = XElement.ReadFrom(readerSubTree) as XElement;
                                                foreach (XElement item in objXElement.Elements())
                                                {
                                                    string strAdsId = item.Attribute("ID").Value;
                                                    AdsUnit objAd = (AdsUnit)hshAllAds[strAdsId];
                                                    objAd.Order = intOrder++;
                                                    if (hshAllAds.ContainsKey(strAdsId))
                                                    {
                                                        objDicPageAds.TryAdd(strAdsId, objAd);
                                                    }
                                                }
                                                break;
                                            case AdsPositionType.MobileAd:
                                                if (!bolIsMobileDevice)
                                                {
                                                    readerSubTree.Skip();  // if device is desktop, don't need to read MobileAd section
                                                    continue;
                                                }

                                                objXElement = XElement.ReadFrom(readerSubTree) as XElement;
                                                foreach (XElement item in objXElement.Elements())
                                                {
                                                    string strAdsId = item.Attribute("ID").Value;
                                                    if (hshAllAds.ContainsKey(strAdsId))
                                                    {
                                                        objDicPageAds.TryAdd(strAdsId, (AdsUnit)hshAllAds[strAdsId]);
                                                    }
                                                }
                                                break;
                                        } // switch

                                    }
                                }
                            }

                            break; // AllAds section must put in the end of file adsConfig.xml , or you should comment this line code;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                objLog.SalError("SalGetPageAdsFromAdsConfigXmlFile", ex);
            }

            return objDicPageAds;
        }

        public DataSet SalGetAvailableCountofJobPostingFromXmlFile()
        {
            string strXmlFilePath = SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.AvailableCountofJobPostingXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetPopularSearchesFromXmlFile()
        {
            string strXmlFilePath = SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.PopularSearchesXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }
        public DataSet SalGetPopularSkillFromXmlFile()
        {
            string strXmlFilePath = SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.PopularSkillsXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }
        public DataSet SalGetPopularCompanyFromXmlFile()
        {
            string strXmlFilePath = SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.PopularCompanyXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetPopularLocationFromXmlFile()
        {
            string strXmlFilePath = SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.PopularLocationXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }
        public DataSet SalGetCareerAdviceFromXmlFile()
        {
            string strXmlFilePath = SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.CareerAdviceXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }
        public DataSet SalGetPopularHowToBecomeFromXmlFile()
        {
            string strXmlFilePath = SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.PopularHowToBecomeXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }
        public DataSet SalGetAllJobPostingToCareerFromXmlFile(string strCareerJobPostingID)
        {
            if (string.IsNullOrEmpty(strCareerJobPostingID))
                throw new ArgumentException(nameof(strCareerJobPostingID));

            string strTheFirstThreeCharacters = strCareerJobPostingID.Substring(0, 3);
            string strXmlFilePath = Path.Combine(SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.AllJobPostingToCareerXmlFileDirPath), strTheFirstThreeCharacters.Substring(0, 1), $"{strTheFirstThreeCharacters}.xml");

            return SalReadXml(strXmlFilePath);
        }
        public DataSet SalGetCareerPostingJobDataByCareerJobIDFromXmlFile(string strJobPostingID)
        {
            if (string.IsNullOrEmpty(strJobPostingID))
                throw new ArgumentException(nameof(strJobPostingID));
            string strFolderName = strJobPostingID.Substring(0, 1);
            if (strFolderName.ToLower() == "j")
            {
                strFolderName = strJobPostingID.Substring(0, 9);
            }
            else
            {
                strFolderName = strJobPostingID.Substring(0, 2);
            }

           string strXmlFilePath = Path.Combine(SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.AllCareerJobPostingInfoXmlFilePath), strFolderName, $"{strJobPostingID}.xml");

            return SalReadXml(strXmlFilePath);
        }
        public DataSet SalGetSingleJobPostingFromXmlFile(string strJobPostingID)
        {
            if (string.IsNullOrEmpty(strJobPostingID))
                return null;

            string strFolderName = strJobPostingID.Substring(0, 1);
            if (strFolderName.ToLower() == "j")
            {
                strFolderName = strJobPostingID.Substring(0, 9);
            }
            else
            {
                strFolderName = strJobPostingID.Substring(0, 2);
            }

            string strXmlFilePath = Path.Combine(SalGetJobBoardFullXmlPath(Seog_Crr_Constant.SingleJobPostingXmlFileDirPath), strFolderName, $"{strJobPostingID}.xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetJobsForCareerPathFromXmlFile(string strBenchmarkJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleBenchmarkJobCareerPathXmlFileDirPath), strBenchmarkJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetAllCareerSkillFromXmlFile()
        {
            string strXmlFilePath = Path.Combine(SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.AllCareerSkillXmlFilePath));
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetAllIndustryFamilyFromXmlFile()
        {
            string strXmlFilePath = SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.AllIndustryFamilyXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetFeaturedJobFromXmlFile()
        {
            string strXmlFilePath = SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.FeaturedJobXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleArticleSectionContentFromXmlFile(string strArticleHotJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.SingleArticleSectionContentXmlFilePath), strArticleHotJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetAllHotTitlesFromXmlFile()
        {
            string strXmlFilePath = SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.AllArticlesXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleArticleHotJobFromXmlFile(string strArticleHotJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.SingleArticleHotJobXmlFilePath), strArticleHotJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleArticleHotJobToBenchmarkJobFromXmlFile(string strArticleHotJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.SingleArticleHotJobToBenchmarkJobXmlFilePath), strArticleHotJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleBenchmarkJobFromXmlFile(string strBenchmarkJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleBenchmarkJobXmlFileDirPath), strBenchmarkJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetAllStateFromXmlFile()
        {
            string strXmlFilePath = SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.AllStatesXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleBenchmarkJobSkillFromXmlFile(string strBenchmarkJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleBenchmarkJobSkillXmlFileDirPath), strBenchmarkJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleTXNYJobFamilyToUniversitySubjectFromXmlFile(string strTXNYJobFamilyCode)
        {
            string strXmlFilePath = Path.Combine(SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.SingleTXNYJobFamilyToUniversitySubjectXmlFilePath), "TXNY_" + strTXNYJobFamilyCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetJobDataAsDateFromXmlFile()
        {
            string strXmlFilePath = SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.JobUpdateDateXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSimilarHotJobTitleFromXmlFile(string strArticleHotJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.SearchSimilarHotJobTitleXmlFilePath), strArticleHotJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetAllArticleSkillDescFromXmlFile()
        {
            string strXmlFilePath = SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.AllArticleSkillDescXmlFilePath);
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalSingleArticleSkillFromXmlFile(string strArticleHotJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetHowToBecomeFullXmlPath(Seog_Crr_Constant.SingleArticleSkillXmlFilePath), strArticleHotJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleAlternateJobFromXmlFile(string strAlternateJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleAlternateJobXmlFileDirPath), strAlternateJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleCompanyJobFromXmlFile(string strCompanyJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleCompanyJobXmlFileDirPath), strCompanyJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSinglePostingJobFromXmlFile(string strPostingJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SinglePostingJobXmlFileDirPath), strPostingJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleNoLevelJobFromXmlFile(string strNoLevelJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleNoLevelJobXmlFileDirPath), strNoLevelJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleFunctionJobFromXmlFile(string strNoLevelJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleFunctionJobXmlFileDirPath), strNoLevelJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleCertificateJobFromXmlFile(string strNoLevelJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleCertificateJobXmlFileDirPath), strNoLevelJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleListingJobFromXmlFile(string strListingJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleListingJobXmlFileDirPath), $"{strListingJobCode}.xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetCompanyInfoByCompanyIDFromXmlFile(string strCompanyID)
        {
            string strForder = strCompanyID.Substring(strCompanyID.Length - 3, 3);

            string strXmlFilePath = string.Format("{0}\\{1}\\{2}", SalCompanyGetFullXmlPath(Seog_Crr_Constant.SingleJBDCompanyXmlFilePath), strForder, $"{strCompanyID}.xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetRelatedCompanyInfoByCompanyIDFromXmlFile(string strCompanyID)
        {
            string strForder = strCompanyID.Substring(strCompanyID.Length - 3, 3);

            string strXmlFilePath = string.Format("{0}\\{1}\\{2}", SalCompanyGetFullXmlPath(Seog_Crr_Constant.SingleJBDCompanyRelatedXmlFileDirPath), strForder, $"{strCompanyID}.xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSkillsByBenchmarkJobCodeFromXmlFile(string strBenchmarkJobCode)
        {
            string strXmlFilePath = Path.Combine(SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.SingleBenchmarkJobToCareerSkillXmlFilePath), strBenchmarkJobCode + ".xml");
            return SalReadXml(strXmlFilePath);
        }

        public DataSet SalGetSingleStateMajorCityFrmoXmlFile(string strStateCode)
        {
            string strXmlFilePath = Path.Combine(SalGetJobSalaryFullXmlPath(Seog_Crr_Constant.SingleStateMajorCityXmlFileDirPath), $"MajorCity-{strStateCode}.xml");
            return SalReadXml(strXmlFilePath);
        }
        public DataSet SalGetCareerSkillToJobOpenings(string strSkillCode, string strStateCode)
        {
            string strXmlFilePath = Path.Combine(SalGetCareerJobBoardFullXmlPath(Seog_Crr_Constant.CareerSkillToJobOpeningsXmlFileDirPath), strSkillCode, $"{strStateCode}.xml");
            return SalReadXml(strXmlFilePath);
        }
    }
}