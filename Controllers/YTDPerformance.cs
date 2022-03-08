using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class YTDPerformanceController : MyBaseController
    {
        // GET: SummaryDashboard

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult YTDPerformancePartial()
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var tahun = (profileData == null ? DateTime.Now.Year : profileData.ProjectYear);
            var model = db.vwsummarydashboards.Where(c=>c.ProjectYear == tahun);
            return PartialView("_GrdYTDPerformancePartial", model.ToList());
        }
        public ActionResult YTDPerformanceDetailPartial(string RegionName)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var tahun = (profileData == null ? DateTime.Now.Year : profileData.ProjectYear);
            var model = db.vwsummarydashboarddetails.Where(c => c.RegionName == RegionName && c.ProjectYear == tahun);
            return PartialView("_GrdYTDPerformanceDetailPartial", model.ToList());
        }
    }
}