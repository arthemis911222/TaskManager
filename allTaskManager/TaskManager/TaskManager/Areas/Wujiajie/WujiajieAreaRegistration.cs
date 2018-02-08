using System.Web.Mvc;

namespace TaskManager.Areas.Wujiajie
{
    public class WujiajieAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Wujiajie";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Wujiajie_default",
                "Wujiajie/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}