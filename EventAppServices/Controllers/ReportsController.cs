using System.Web.Http.Cors;
using System.Web.Mvc;

namespace EventAppServices.Controllers
{ 
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportsController : Controller
    {
        public ActionResult BarChartView()
        { 
            return View("BarChartView");
        }

        public ActionResult UserView()
        { 
            return View("UserView");
        }
    }
}
