using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MonthlySavingController : MyBaseController
    {
        // GET: SummaryDashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}