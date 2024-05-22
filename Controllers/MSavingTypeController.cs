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
            var model = db.msavingtypes.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear);
            return PartialView("_GrdSavingTypePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSavingTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msavingtype item)
        {
            var model = db.msavingtypes;
            var tmodel = model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList();
            if (item.SavingTypeName != null && item.SavingTypeName != string.Empty && item.isActive !=null)
            {
                if (tmodel.Where(x => x.SavingTypeName.ToLower() == item.SavingTypeName.ToLower()).ToList().Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            item.InitYear = GAIN.Models.Constants.defaultyear;
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

            return PartialView("_GrdSavingTypePartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSavingTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msavingtype item)
        {
            var model = db.msavingtypes;
            var tmodel = model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList();
            if (item.SavingTypeName != null && item.SavingTypeName != string.Empty && item.isActive != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.SavingTypeName.ToLower() == item.SavingTypeName.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.SavingTypeName = item.SavingTypeName;
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

            return PartialView("_GrdSavingTypePartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
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
                        item.isActive = "N";
                        //model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GrdSavingTypePartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
    }
}