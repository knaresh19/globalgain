using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MRegionController : MyBaseController
    {
        // GET: MRegion
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdRegionPartial()
        {
            var model = db.mregions;
            return PartialView("_GrdRegionPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdRegionPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mregion item)
        {
            var model = db.mregions;
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
            return PartialView("_GrdRegionPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdRegionPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mregion item)
        {
            var model = db.mregions;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.RegionName = item.RegionName;
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
            return PartialView("_GrdRegionPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdRegionPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mregion itemx)
        {
            var model = db.mregions;
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
            return PartialView("_GrdRegionPartial", model.ToList());
        }
    }
}