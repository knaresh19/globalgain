using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MStatusController : MyBaseController
    {
        // GET: MStatus
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdStatusPartial()
        {
            var model = db.mstatus;
            return PartialView("_GrdStatusPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdStatusPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mstatu item)
        {
            var model = db.mstatus;
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
            return PartialView("_GrdStatusPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdStatusPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mstatu item)
        {
            var model = db.mstatus;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.Status = item.Status;
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
            return PartialView("_GrdStatusPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdStatusPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mstatu itemx)
        {
            var model = db.mstatus;
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
            return PartialView("_GrdStatusPartial", model.ToList());
        }
    }
}