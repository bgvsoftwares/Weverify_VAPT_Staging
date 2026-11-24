using System.Web.Mvc;

namespace WebAppBGV.Areas.ClientArea
{
    public class ClientAreaAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ClientArea";
            }
        }


        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ClientArea_default",
                "ClientArea/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new string[] { "WebAppBGV.Areas.ClientArea.Controllers" }
            );


            context.MapRoute(
               "ClientDataEntry",
               "ClientArea/{controller}/{action}/client/{ClientRowId}/candidate/{PersonalRowID}/check/{ChFamilRowId}/{subCheckId}",
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