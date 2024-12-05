using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;
using System.Configuration;

namespace GAIN.Controllers
{
    public class ToBeDeletedListController : MyBaseController
    {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: ToBeDeletedList
        public ActionResult Index()
        {
            if(Session["DefaultGAINSess"] == null)
                return Redirect(Url.Action("index", "login"));

            return View();
        }
        [ValidateInput(false)]
        public ActionResult GrdInitDeletedPartial(short year)
        {
            ViewData["ProjectionYear"] = year;
            IQueryable<t_initiative> model = db.t_initiative.Where(c => c.InitStatus == 1 && c.isDeleted == 0 && c.ProjectYear == DateTime.Now.Year);
            SetSetupData();
            return PartialView("_GrdInitDeletedPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitDeletedPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_initiative item, short year)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.t_initiative.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    log.Error(e.Message, e);
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            ViewData["ProjectionYear"] = year;
            IQueryable<t_initiative> model = db.t_initiative.Where(c => c.InitStatus == 1 && c.isDeleted == 0 && c.ProjectYear == year);
            SetSetupData();
            return PartialView("_GrdInitDeletedPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitDeletedPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_initiative item, short year)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    t_initiative modelItem = db.t_initiative.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    log.Error(e.Message, e);
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            ViewData["ProjectionYear"] = year;
            IQueryable<t_initiative> model = db.t_initiative.Where(c => c.InitStatus == 1 && c.isDeleted == 0 && c.ProjectYear == year);
            SetSetupData();
            return PartialView("_GrdInitDeletedPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitDeletedPartialDelete(long id, short year)
        {
            if (id >= 0)
            {
                try
                {
                    t_initiative item = db.t_initiative.Find(id);
                    if (item != null)
                        item.isDeleted = 1;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    log.Error(e.Message, e);
                }
            }

            ViewData["ProjectionYear"] = year;
            IQueryable<t_initiative> model = db.t_initiative.Where(c => c.InitStatus == 1 && c.isDeleted == 0 && c.ProjectYear == year);
            SetSetupData();
            return PartialView("_GrdInitDeletedPartial", model);
        }

        
        public ActionResult CustomCallback(string selectedKeys, short year)
        {
            if (!String.IsNullOrEmpty(selectedKeys))
            {
                List<long> deletedRowKey = new List<long>();
                foreach (var item in selectedKeys.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    long key;
                    if (long.TryParse(item, out key))
                    {
                        deletedRowKey.Add(key);
                    }
                }

                List<t_initiative> listDeleteItems = db.t_initiative.Where(c => deletedRowKey.Contains(c.id)).ToList();
                foreach (t_initiative item in listDeleteItems)
                {
                    item.isDeleted = 1;
                }
                db.SaveChanges();
            }

            ViewData["ProjectionYear"] = year;
            IQueryable<t_initiative> model = db.t_initiative.Where(c => c.InitStatus == 1 && c.isDeleted == 0 && c.ProjectYear == year);
            SetSetupData();
            return PartialView("_GrdInitDeletedPartial", model);
        }
        public ActionResult CboYear()
        {
            List<myear> model = db.myears.Where(c => c.yrStatus == 1).ToList();
            return PartialView("~/Views/Shared/_CboYearPartial.cshtml", model);
        }

        private void SetSetupData()
        {
            ViewData["mregions"] = db.mregions.ToList();
            ViewData["brandname"] = db.mbrands.ToList().Where(s => s.isDeleted == "N");
            ViewData["msubregion"] = db.msubregions.Where(c => c.SubRegionName != null && c.SubRegionName != "").ToList();
            ViewData["mcluster"] = db.mclusters.Where(c => c.ClusterName != "").ToList();
            ViewData["mregional_office"] = db.mregional_office.Where(c => c.RegionalOffice_Name != "").ToList();
            ViewData["CostControlSiteName"] = db.mcostcontrolsites.Where(c => c.CostControlSiteName != "").ToList();
            ViewData["CountryName"] = db.mcountries.Where(c => c.CountryName != "").ToList();
            ViewData["SubCountryName"] = db.msubcountries.Where(c => c.SubCountryName != "").ToList();
            ViewData["LegalEntityName"] = db.mlegalentities.Where(c => c.LegalEntityName != "").ToList();
            ViewData["SavingTypeName"] = db.msavingtypes.ToList();
            ViewData["CostTypeName"] = db.mcosttypes.ToList();
            ViewData["SubCostName"] = db.msubcosts.ToList();
            ViewData["ActionTypeName"] = db.mactiontypes.ToList();
            ViewData["SynImpactName"] = db.msynimpacts.ToList();
            ViewData["Status"] = db.mstatus.ToList();
            ViewData["portName"] = db.mports.ToList();
        }
    }
}