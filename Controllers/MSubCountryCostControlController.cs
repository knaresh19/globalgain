using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class MSubCountryCostControlController : MyBaseController
    {
        // GET: MBrandCountry
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdSubCountryCostControlPartial()
        {
            var model = db.t_subctry_costcntrlsite;
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            ViewData["BrandList"] = db.mbrands.ToList().Distinct();
            ViewData["Subcountry"] = db.msubcountries.ToList();
            return PartialView("_GrdSubCountryCostControlPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCountryCostControlPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subctry_costcntrlsite item)
        {
            var model = db.t_subctry_costcntrlsite;
            if (ModelState.IsValid)
            {
                try
                {
                   // item.msubcountry.isActive = "Y";
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

            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            ViewData["BrandList"] = db.mbrands.ToList();
            ViewData["Subcountry"] = db.msubcountries.ToList();
            return PartialView("_GrdSubCountryCostControlPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCountryCostControlPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subctry_costcntrlsite item)
        {
            var model = db.t_subctry_costcntrlsite;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.brandid = item.brandid;
                        modelItem.subcountryid = item.subcountryid;
                        modelItem.costcontrolid = item.costcontrolid;
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

            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            ViewData["BrandList"] = db.mbrands.ToList();
            ViewData["Subcountry"] = db.msubcountries.ToList();
            return PartialView("_GrdSubCountryCostControlPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCountryCostControlPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subctry_costcntrlsite itemx )
        {
            var model = db.t_subctry_costcntrlsite;
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
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            ViewData["BrandList"] = db.mbrands.ToList();
            ViewData["Subcountry"] = db.msubcountries.ToList();
            return PartialView("_GrdSubCountryCostControlPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Getsubcountrybycountry(int CountryId)
        {
           List<SubCountryList> SubCountryList = new List<SubCountryList>();

            SubCountryList = (from subcountries in db.msubcountries
                             where subcountries.CountryID == CountryId
                             select subcountries).ToList().Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();

            return Json(SubCountryList, JsonRequestBehavior.AllowGet);
        }
    }
}