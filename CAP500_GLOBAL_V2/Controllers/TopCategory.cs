using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class TopCategoryController : Controller
    {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        // GET: SummaryDashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TopCategoryPartial()
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var tahun = (profileData == null ? DateTime.Now.Year : profileData.ProjectYear);
            var model = db.vwtopcategories.Where(c => c.projectyear == tahun);
            return PartialView("_GrdTopCategoryPartial", model.ToList());
        }
        public ActionResult ChartPartial() {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var tahun = (profileData == null ? DateTime.Now.Year : profileData.ProjectYear);
            List<Models.ReportModel> model = db.Database.SqlQuery<Models.ReportModel>("SELECT 'YTD Saving Achieved' AS TypeOfCost, CostTypeName,ifnull(ytdachieved,0) AS Nilai FROM vwtopcategory WHERE projectyear = " + tahun + " UNION ALL SELECT 'YTD Target' AS TypeOfCost, CostTypeName, ifnull(ytdtarget,0) AS Nilai FROM vwtopcategory WHERE projectyear = " + tahun + " ").ToList();
            //var model = db.vwtopcategories;
            return PartialView("~/Views/TopCategory/_ChartPartial.cshtml", model.ToList());
        }
    }
}