using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;

namespace TronCell.Queue.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var fitting = ServiceLocator.Current.GetInstance<RetailDataContext>();
            var test = fitting.Fittings.FirstOrDefault(p => p.FittingRoom.RoomName.Contains("002"));
            var name = test.FittingRoom.RoomName;
            ViewBag.RoomName = name;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}