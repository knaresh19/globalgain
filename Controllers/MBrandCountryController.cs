using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MBrandCountryController : MyBaseController
    {
        // GET: MBrandCountry
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdBrandCountryPartial()
        {
            var model = db.mbrandcountries;
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["BrandList"] = db.mbrands.ToList();
            return PartialView("_GrdBrandCountryPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandCountryPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrandcountry item)
        {
            var model = db.mbrandcountries;
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

            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["BrandList"] = db.mbrands.ToList();
            return PartialView("_GrdBrandCountryPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandCountryPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrandcountry item)
        {
            var model = db.mbrandcountries;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.brandid = item.brandid;
                        modelItem.countryid = item.countryid;
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

            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["BrandList"] = db.mbrands.ToList();
            return PartialView("_GrdBrandCountryPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandCountryPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrandcountry itemx )
        {
            var model = db.mbrandcountries;
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
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["BrandList"] = db.mbrands.ToList();
            return PartialView("_GrdBrandCountryPartial", model.ToList());
        }
    }
}