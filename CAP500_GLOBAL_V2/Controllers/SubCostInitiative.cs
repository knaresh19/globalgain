using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class SubCostInitiativeController : MyBaseController
    {
        // GET: CostActionType
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdSubCostInitiativePartial() 
        {
            var model = db.t_subcostinitiative;
            ViewData["SavingTypeName"] = db.msavingtypes.ToList();
            ViewData["CostTypeName"] = db.mcosttypes.ToList();
            ViewData["SubCostName"] = db.msubcosts.ToList();
            return PartialView("_GrdSubCostInitiativePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)] 
        public ActionResult GrdSubCostInitiativePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostinitiative item)
        {
            var model = db.t_subcostinitiative;
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
            return PartialView("_GrdSubCostInitiativePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostInitiativePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostinitiative item)
        {
            var model = db.t_subcostinitiative;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.savingtypeid = item.savingtypeid;
                        modelItem.costitemid = item.costitemid;
                        modelItem.subcostid = item.subcostid;
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
            return PartialView("_GrdSubCostInitiativePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostInitiativePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostinitiative itemx)
        {
            var model = db.t_subcostinitiative;
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
            return PartialView("_GrdSubCostInitiativePartial", model.ToList());
        }
    }
}