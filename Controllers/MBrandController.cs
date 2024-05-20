using DevExpress.Web.Mvc;
using GAIN.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MBrandController : MyBaseController
    {
        // GET: MBrand
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdBrandPartial()
        
        {
            var model = db.mbrands.Where(s => s.InitYear== Constants.defaultyear);
            ViewData["BrandList"] = model.ToList();
            return PartialView("_GrdBrandPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrand item)
        {
            var model = db.mbrands;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.brandname != null && item.brandname != string.Empty && item.isActive != null)
            {
                if (tmodel.Where(x => x.brandname.ToLower() == item.brandname.ToLower()).ToList().Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            item.InitYear = Constants.defaultyear;
                            item.isDeleted = "N";
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
            return PartialView("_GrdBrandPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrand item)
        {
            var model = db.mbrands;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.brandname != null && item.brandname != string.Empty && item.isActive != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.brandname.ToLower() == item.brandname.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.brandname = item.brandname;
                                modelItem.isActive = item.isActive;
                                modelItem.InitYear = Constants.defaultyear;
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
            return PartialView("_GrdBrandPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrand itemx)
        {
            var model = db.mbrands;
            if (itemx.id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.id == itemx.id);
                    if (item != null)
                    {
                        item.isDeleted = "Y";  //For Soft delete.
                        item.isActive = "N";
                        // model.Remove(item);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GrdBrandPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}