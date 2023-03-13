using System.Web.Mvc;
using System.Web.Routing;

namespace CareerCom.Cool.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "OldCareerSearchPage",
                url: "career/layoutscripts/{*.aspx}",
                defaults: new { controller = "Career", action = "CareerJobPosting_SearchIndex_Old" }
            );

            routes.MapRoute(
                name: "Crr_JobPosting_JobDetail",
                url: "company/{strSeoFriendlyCompanyName}/job/{strSeoFriendlyJobTitle}/{strLocation}",
                defaults: new { controller = "Career", action = "CareerJobPosting_JobDetailIndex" }
            );

            routes.MapRoute(
                name: "Crr_JobPosting_Search",
                url: "jobs",
                defaults: new { controller = "Career", action = "CareerJobPosting_SearchIndex" }
            );

            routes.MapRoute(
               name: "Crr_JobPosting_GenericSearch",
               url: "search",
               defaults: new { controller = "Career", action = "CareerJobPosting_GenericSearchIndex" }
            );

            routes.MapRoute(
              name: "Crr_JobPosting_JobsSearch",
              url: "jobs/search",
              defaults: new { controller = "Career", action = "CareerJobPosting_JobSearchIndex" }
           );

            routes.MapRoute(
              name: "Crr_Jobs_SearchJobsByIndustry",
              url: "jobs/-by-industry",
              defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsByIndustry" }
            );

            routes.MapRoute(
             name: "Crr_Jobs_SearchJobsByIndustry_DrillIn",
             url: "jobs/-in-{strSeoIndustry}-industry",
             defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsByIndustry_DrillIn" }
           );

            routes.MapRoute(
              name: "Crr_Jobs_SearchJobsByCategory",
              url: "jobs/-by-category",
              defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsByCategory" }
           );

            routes.MapRoute(
              name: "Crr_Jobs_SearchJobsByCategory_DrillIn",
              url: "jobs/-in-{strSeoCategory}-category",
              defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsByCategory_DrillIn" }
            );

            routes.MapRoute(
               name: "Crr_Jobs_SearchJobsBySkill",
               url: "jobs/-by-skill",
               defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsBySkill" }
            );

            routes.MapRoute(
               name: "Crr_Jobs_SearchJobsBySkill_DrillIn",
               url: "jobs/-with-{strSeoSkill}-skill",
               defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsBySkill_DrillIn" }
            );

            routes.MapRoute(
             name: "Crr_Jobs_SearchJobsByState",
             url: "jobs/-by-state",
             defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsByState" }
           );

            routes.MapRoute(
              name: "Crr_Jobs_SearchJobsByState_DrillIn",
              url: "jobs/-in-{strStateCode}",
             defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsByState_DrillIn" }
           );

            routes.MapRoute(
              name: "Crr_Jobs_SearchJobsByCity",
              url: "jobs/-by-city",
              defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsByCity" }
            );

            routes.MapRoute(
             name: "Crr_Jobs_SearchJobsByCity_DrillIn",
             url: "jobs/-in-{strSeoCityName}-{strStateCode}",
             defaults: new { controller = "Career", action = "CareerJobPosting_SearchJobsByCity_DrillIn" }
           );

            routes.MapRoute(
              name: "Crr_Jobs_SavedJobs",
              url: "jobs/saved-jobs",
              defaults: new { controller = "Career", action = "CareerJobPosting_SavedJobs" }
            );

            routes.MapRoute(
              name: "Crr_Jobs_JobAlert",
              url: "jobs/job-alert",
              defaults: new { controller = "Career", action = "CareerJobPosting_JobAlert" }
            );

            routes.MapRoute(
              name: "Crr_Company_SearchCompanyiesByIndustry",
              url: "companies/-by-industry",
              defaults: new { controller = "Career", action = "CareerJobPosting_SearchCompanyiesByIndustry" }
            );

            routes.MapRoute(
              name: "Crr_Company_SearchCompanyiesByIndustry_DrillIn",
              url: "companies/-in-{strSeoIndustry}-industry",
              defaults: new { controller = "Career", action = "CareerJobPosting_SearchCompanyiesByIndustry_DrillIn" }
            );

            routes.MapRoute(
              name: "Crr_Company_BrowseCompanyDetail",
              url: "companies/{strSeoCompanyName}",
              defaults: new { controller = "Career", action = "CareerJobPosting_BrowseCompanyDetail" }
            );

            routes.MapRoute(
              name: "Crr_JobPosting_Account",
              url: "account",
              defaults: new { controller = "Career", action = "CareerJobPosting_Account" }
            );

            routes.MapRoute(
              name: "Crr_JobPosting_AccountSetting",
              url: "account/setting",
              defaults: new { controller = "Career", action = "CareerJobPosting_AccountSetting" }
            );

            routes.MapRoute(
             name: "Crr_JobPosting_Legal",
             url: "legal",
             defaults: new { controller = "Career", action = "CareerJobPosting_LegalIndex" }
            );

            routes.MapRoute(
               name: "Crr_JobPosting_Legal_DSA",
               url: "legal/dsa",
               defaults: new { controller = "Career", action = "CareerJobPosting_Legal_DSA" }
            );

            routes.MapRoute(
             name: "Crr_JobPosting_Legal_PC",
             url: "legal/privacy-controls",
             defaults: new { controller = "Career", action = "CareerJobPosting_Legal_PC" }
            );

            routes.MapRoute(
             name: "Crr_JobPosting_Legal_DPA",
             url: "legal/dpa",
             defaults: new { controller = "Career", action = "CareerJobPosting_Legal_DPA" }
            );

            routes.MapRoute(
             name: "Crr_JobPosting_Legal_PP",
             url: "legal/pp",
             defaults: new { controller = "Career", action = "CareerJobPosting_Legal_PP" }
            );

            routes.MapRoute(
            name: "Crr_JobPosting_Legal_Accessibility",
            url: "legal/accessibility",
            defaults: new { controller = "Career", action = "CareerJobPosting_Legal_Accessibility" }
           );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Career", action = "CrrIndex", id = UrlParameter.Optional }
           );

        }
    }
}
