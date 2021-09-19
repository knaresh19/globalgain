using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MSynImpactController : Controller
    {
        // GET: MSynImpact
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdSynergyImpactPartial()
        {
            var model = db.msynimpacts;
            return PartialView("_GrdSynergyImpactPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSynergyImpactPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msynimpact item)
        {
            var model = db.msynimpacts;
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
            return PartialView("_GrdSynergyImpactPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSynergyImpactPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msynimpact item)
        {
            var model = db.msynimpacts;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.SynImpactName = item.SynImpactName;
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
            return PartialView("_GrdSynergyImpactPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSynergyImpactPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msynimpact itemx)
        {
            var model = db.msynimpacts;
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
            return PartialView("_GrdSynergyImpactPartial", model.ToList());
        }
    }
}