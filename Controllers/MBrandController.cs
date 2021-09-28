using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MBrandController : MyBaseController
    {
        // GET: MBrand
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdBrandPartial()
        {
            var model = db.mbrands;
            return PartialView("_GrdBrandPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrand item)
        {
            var model = db.mbrands;
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
            return PartialView("_GrdBrandPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrand item)
        {
            var model = db.mbrands;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.brandname = item.brandname;
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
            return PartialView("_GrdBrandPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrand itemx)
        {
            var model = db.mbrands;
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
            return PartialView("_GrdBrandPartial", model.ToList());
        }
    }
}