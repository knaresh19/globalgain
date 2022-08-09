using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdBrandPartial()
        {
            var model = db.mbrands.Where(s => s.isDeleted == "N");
            ViewData["BrandList"] = model.ToList().Where(s => s.isDeleted == "N");
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
                    item.isDeleted = "N";
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
            return PartialView("_GrdBrandPartial", model.ToList().Where(s => s.isDeleted == "N"));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrand item)
        {
            var model = db.mbrands.Where(s => s.isDeleted == "N");
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.brandname = item.brandname;
                        modelItem.isActive = item.isActive;
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
            var model = db.mbrands.Where(s => s.isDeleted == "N");
            if (itemx.id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.id == itemx.id);
                    if (item != null)
                        item.isDeleted="Y";  //For Soft delete.
                        // model.Remove(item);
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