using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class SubCostActionTypeController : Controller
    {
        // GET: CostActionType
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdSubCostActionTypePartial()
        {
            var model = db.t_subcostactiontype;
            ViewData["SavingTypeName"] = db.msavingtypes.ToList();
            ViewData["SubCostName"] = db.msubcosts.ToList();
            return PartialView("_GrdSubCostActionTypePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)] 
        public ActionResult GrdSubCostActionTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostactiontype item)
        {
            var model = db.t_subcostactiontype;
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
            return PartialView("_GrdSubCostActionTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostActionTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostactiontype item)
        {
            var model = db.t_subcostactiontype;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.subcostid = item.subcostid;
                        modelItem.actiontypeid = item.actiontypeid;
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
            return PartialView("_GrdSubCostActionTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostActionTypePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostactiontype itemx)
        {
            var model = db.t_subcostactiontype;
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
            return PartialView("_GrdSubCostActionTypePartial", model.ToList());
        }
    }
}