#region Usings

using System.Web.Mvc;

#endregion

namespace LogSys.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Loggy!";

            return View();
        }
    }
}
