using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MCostTypeController : MyBaseController
    {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        // GET: MCostType
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GrdCostTypePartial()
        {
            var model = db.mcosttypes;
            return PartialView("_GrdCostTypePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcosttype item)
        {
            var model = db.mcosttypes;
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
            return PartialView("_GrdCostTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcosttype item)
        {
            var model = db.mcosttypes;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.CostTypeName = item.CostTypeName;
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
            return PartialView("_GrdCostTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostTypePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcosttype itemx)
        {
            var model = db.mcosttypes;
            if (itemx.id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.id == itemx.id);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GrdCostTypePartial", model.ToList());
        }
    }
}