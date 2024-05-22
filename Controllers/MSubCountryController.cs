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
    public class msubcountryController : MyBaseController
    {
        // GET: msubcountries
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdSubCountryPartial()
        {
            var model = db.msubcountries;
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCountryPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCountryPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubcountry item)
        {
            var model = db.msubcountries;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.CountryID != 0 && item.SubCountryName != null && item.CountryCode != null && item.isActive != null)
            {
                if (tmodel.Where(x => x.CountryID == item.CountryID && x.CountryCode == item.CountryCode && x.SubCountryName.ToLower() == item.SubCountryName.ToLower()).ToList().Count == 0)
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

            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCountryPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCountryPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubcountry item)
        {
            var model = db.msubcountries;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.CountryID != 0 && item.SubCountryName != null && item.CountryCode != null && item.isActive != null)
            {


                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.CountryID == item.CountryID && x.CountryCode == item.CountryCode && x.SubCountryName.ToLower() == item.SubCountryName.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.CountryID = item.CountryID;
                                modelItem.SubCountryName = item.SubCountryName;
                                modelItem.CountryCode = item.CountryCode;
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

            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCountryPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCountryPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubcountry itemx)
        {
            var model = db.msubcountries;
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

            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCountryPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}