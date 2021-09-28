using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class SavingMonitoringController : MyBaseController
    {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();
        // GET: SavingMonitoringList
        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult GrdInitSavingMonitoringPartial()
        {
            var model = db.t_initiative.Where(c => c.isDeleted == 1);
            return PartialView("_GrdInitSavingMonitoringPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitSavingMonitoringPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_initiative item)
        {
            var model = db.t_initiative;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GrdInitSavingMonitoringPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitSavingMonitoringPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_initiative item)
        {
            var model = db.t_initiative;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GrdInitSavingMonitoringPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitSavingMonitoringPartialDelete(System.Int64 id)
        {
            var model = db.t_initiative;
            if (id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.id == id);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GrdInitSavingMonitoringPartial", model.ToList());
        }
        public ActionResult CboYear()
        {
            List<myear> model = db.myears.Where(c => c.yrStatus == 1).ToList();
            return PartialView("~/Views/Shared/_CboYearPartial.cshtml", model);
        }
    }
}