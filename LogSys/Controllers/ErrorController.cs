#region Usings

using System.Web.Mvc;

#endregion

namespace LogSys.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult AjaxError()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return PartialView("_AjaxError");
        }
    }
}
