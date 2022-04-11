using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MLegalEntityController : MyBaseController
    {
        // GET: MLegalEntity
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdLegalEntityPartial()
        {
            var model = db.mlegalentities;
            ViewData["BrandList"] = db.mbrands.ToList();
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["Subcountrylist"] = db.msubcountries.ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            return PartialView("_GrdLegalEntityPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdLegalEntityPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mlegalentity item)
        {
            var model = db.mlegalentities;
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

            ViewData["BrandList"] = db.mbrands.ToList();
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["Subcountrylist"] = db.msubcountries.ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            return PartialView("_GrdLegalEntityPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdLegalEntityPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mlegalentity item)
        {
            var model = db.mlegalentities;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.BrandID = item.BrandID;
                        modelItem.CountryID = item.CountryID;
                        modelItem.LegalEntityName = item.LegalEntityName;
                        modelItem.SubCountryID = item.SubCountryID;
                        modelItem.CostControlSiteID = item.CostControlSiteID;
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

            ViewData["BrandList"] = db.mbrands.ToList();
            ViewData["CountryList"] = db.mcountries.ToList();
            return PartialView("_GrdLegalEntityPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdLegalEntityPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mlegalentity itemx)
        {
            var model = db.mlegalentities;
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

            ViewData["BrandList"] = db.mbrands.ToList();
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["Subcountrylist"] = db.msubcountries.ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            return PartialView("_GrdLegalEntityPartial", model.ToList());
        }
    }
}