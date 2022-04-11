using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class TypeofSavingController : MyBaseController
    {

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        // GET: SummaryDashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TypeofSavingPartial()
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var tahun = (profileData == null ? DateTime.Now.Year : profileData.ProjectYear);
            var model = db.vwtypeofsavings.Where(c => c.projectyear == tahun);
            return PartialView("_GrdTypeofSavingPartial", model.ToList());
        }
        public ActionResult ChartPartial()
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var tahun = (profileData == null ? DateTime.Now.Year : profileData.ProjectYear);
            List<ReportModel> model = db.Database.SqlQuery<ReportModel>("SELECT 'YTDAchieved' AS TypeOfCost, SavingTypeName as CostTypeName,IFNULL(YTDAchieved,0) AS Nilai FROM vwtypeofsaving WHERE projectyear = " + tahun + " UNION ALL SELECT 'YTDTarget' AS TypeOfCost, SavingTypeName as CostTypeName,IFNULL(YTDTarget, 0) AS Nilai FROM vwtypeofsaving WHERE projectyear = " + tahun + " ").ToList();
            return PartialView("~/Views/TypeOfSaving/_ChartPartial.cshtml", model.ToList());
        }
    }
}