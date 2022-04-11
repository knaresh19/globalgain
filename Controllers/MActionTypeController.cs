using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MActionTypeController : MyBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdActionTypePartial()
        {
            var model = db.mactiontypes;
            return PartialView("_GrdActionTypePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdActionTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mactiontype item)
        {
            var model = db.mactiontypes;
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
            return PartialView("_GrdActionTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdActionTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mactiontype item)
        {
            var model = db.mactiontypes;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.ActionTypeName = item.ActionTypeName;
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
            return PartialView("_GrdActionTypePartial", model.ToList());
        }
        [HttpPost]
        public ActionResult GrdActionTypePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mactiontype itemx)
        {
            var model = db.mactiontypes;
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
            return PartialView("_GrdActionTypePartial", model.ToList());
        }
    }
}
