using System.Web.Mvc;

namespace E_Mall_Api.Controllers
{
    public class HomeController : Controller
    {
        public static string IP { get { return "http://10.52.177.59:80"; } }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Pages";
            return View();
        }
    }
}