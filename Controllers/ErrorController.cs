using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GeneralError()
        {
            return View();
        }
    }
}