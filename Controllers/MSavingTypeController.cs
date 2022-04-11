using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MSavingTypeController : MyBaseController
    {
        // GET: MSavingType
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdSavingTypePartial()
        {
            var model = db.msavingtypes;
            return PartialView("_GrdSavingTypePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSavingTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msavingtype item)
        {
            var model = db.msavingtypes;
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
            return PartialView("_GrdSavingTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSavingTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msavingtype item)
        {
            var model = db.msavingtypes;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.SavingTypeName = item.SavingTypeName;
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
            return PartialView("_GrdSavingTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSavingTypePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msavingtype itemx)
        {
            var model = db.msavingtypes;
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
            return PartialView("_GrdSavingTypePartial", model.ToList());
        }
    }
}