using System.Web.Mvc;

namespace WebAppBGV.Areas.Candidate
{
    public class CandidateAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Candidate";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Candidate_default",
                "Candidate/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new string[] { "WebAppBGV.Areas.Candidate.Controllers" }
            );

            context.MapRoute(
              "CandidateDataEntry",
              "Candidate/{controller}/{action}/client/{ClientRowId}/candidate/{PersonalRowID}/check/{ChFamilRowId}/{subCheckId}",
             new
             {
                 controller = "Home",
                 action = "Index",
                 ClientRowId = UrlParameter.Optional,
                 PersonalRowID = UrlParameter.Optional,
                 ChFamilRowId = UrlParameter.Optional,
                 subCheckId = UrlParameter.Optional
             }
         );
        }
    }
}