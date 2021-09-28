using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MSubRegionController : MyBaseController
    {
        // GET: MSubRegion
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdSubRegionPartial()
        {
            var model = db.msubregions;
            ViewData["RegionList"] = db.mregions.ToList();
            return PartialView("_GrdSubRegionPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubRegionPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubregion item)
        {
            var model = db.msubregions;
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

            ViewData["RegionList"] = db.mregions.ToList();
            return PartialView("_GrdSubRegionPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubRegionPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubregion item)
        {
            var model = db.msubregions;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.RegionID = item.RegionID;
                        modelItem.SubRegionName = item.SubRegionName;
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

            ViewData["RegionList"] = db.mregions.ToList();
            return PartialView("_GrdSubRegionPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubRegionPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubregion itemx)
        {
            var model = db.msubregions;
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

            ViewData["RegionList"] = db.mregions.ToList();
            return PartialView("_GrdSubRegionPartial", model.ToList());
        }
    }
}