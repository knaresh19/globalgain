using DevExpress.Web.Mvc;
using GAIN.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MCostTypeController : MyBaseController
    {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        // GET: MCostType
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GrdCostTypePartial()
        {
            var model = db.mcosttypes.Where(x => x.InitYear == Constants.defaultyear); 
            return PartialView("_GrdCostTypePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcosttype item)
        {
            var model = db.mcosttypes;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.CostTypeName != null && item.isActive != null)
            {
                if (tmodel.Where(x => x.CostTypeName.ToLower() == item.CostTypeName.ToLower()).ToList().Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            item.InitYear = Constants.defaultyear;
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
                }
                else
                    ViewData["EditError"] = "Already Exists!.";
            }
            else
                ViewData["EditError"] = "Please fill out all required fields.";

            return PartialView("_GrdCostTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcosttype item)
        {
            var model = db.mcosttypes;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.CostTypeName != null && item.isActive != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.CostTypeName.ToLower() == item.CostTypeName.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.CostTypeName = item.CostTypeName;
                                modelItem.isActive = item.isActive;
                                db.SaveChanges();
                            }
                            else
                                ViewData["EditError"] = "Already Exists!.";
                        }
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] = e.Message;
                    }
                }
                else
                    ViewData["EditError"] = "Please, correct all errors.";
            }
            else
                ViewData["EditError"] = "Please fill out all required fields.";

            return PartialView("_GrdCostTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostTypePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcosttype itemx)
        {
            var model = db.mcosttypes;
            if (itemx.id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.id == itemx.id);
                    if (item != null)
                    {
                        //model.Remove(item);
                        item.isActive = "N";
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GrdCostTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}