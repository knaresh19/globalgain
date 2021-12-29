using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class SummaryDashboardController : MyBaseController
    {
        // GET: SummaryDashboard

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        public ActionResult Index()
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            if (profileData == null) { Response.Redirect("~/"); }
            return View();
        }

        [ValidateInput(false)]
        public ActionResult SummaryDashboardPartial()
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var tahun = (profileData == null ? DateTime.Now.Year : profileData.ProjectYear);
            var model = db.vwsummarydashboards.Where(c => c.ProjectYear == tahun);
            return PartialView("_GrdSummaryDashboardPartial", model.ToList());
        }
        public ActionResult SummaryDashboardDetailPartial(string RegionName) 
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var tahun = (profileData == null ? DateTime.Now.Year : profileData.ProjectYear);
            var model = db.vwsummarydashboarddetails.Where(c => c.RegionName == RegionName && c.ProjectYear == tahun);
            return PartialView("_GrdSummaryDashboardDetailPartial", model.ToList());
        }
    }
}