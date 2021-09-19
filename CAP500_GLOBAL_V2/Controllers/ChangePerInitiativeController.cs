using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class ChangePerInitiativeController : Controller
    {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();
        // GET: ChangePerInitiativeList
        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult GrdInitChangePerInitiativePartial()
        {
            var model = db.t_initiative.Where(c => c.isDeleted == 1);
            return PartialView("_GrdInitChangePerInitiativePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitChangePerInitiativePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_initiative item)
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
            return PartialView("_GrdInitChangePerInitiativePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitChangePerInitiativePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_initiative item)
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
            return PartialView("_GrdInitChangePerInitiativePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdInitChangePerInitiativePartialDelete(System.Int64 id)
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
            return PartialView("_GrdInitChangePerInitiativePartial", model.ToList());
        }
        public ActionResult CboYear()
        {
            List<myear> model = db.myears.Where(c => c.yrStatus == 1).ToList();
            return PartialView("~/Views/Shared/_CboYearPartial.cshtml", model);
        }
    }
}