using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class SubCostBrandController : Controller
    {
        // GET: CostActionType
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdSubCostBrandPartial() 
        {
            var model = db.t_subcostbrand;
            ViewData["SavingTypeName"] = db.msavingtypes.ToList();
            ViewData["CostTypeName"] = db.mcosttypes.ToList();
            ViewData["SubCostName"] = db.msubcosts.ToList();
            ViewData["BrandName"] = db.mbrands.ToList();
            return PartialView("_GrdSubCostBrandPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)] 
        public ActionResult GrdSubCostBrandPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostbrand item)
        {
            var model = db.t_subcostbrand;
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
            return PartialView("_GrdSubCostBrandPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostBrandPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostbrand item)
        {
            var model = db.t_subcostbrand;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.savingtypeid = item.savingtypeid;
                        modelItem.costtypeid = item.costtypeid;
                        modelItem.subcostid = item.subcostid;
                        modelItem.brandid = item.brandid;
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
            return PartialView("_GrdSubCostBrandPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostBrandPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostbrand itemx)
        {
            var model = db.t_subcostbrand;
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
            return PartialView("_GrdSubCostBrandPartial", model.ToList());
        }
    }
}