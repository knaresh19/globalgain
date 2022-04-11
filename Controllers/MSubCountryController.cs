using DevExpress.Web.Mvc;
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
            ViewData["CountryList"] = db.mcountries.ToList();
            return PartialView("_GrdSubCountryPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCountryPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubcountry item)
        {
            var model = db.msubcountries;
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
            return PartialView("_GrdSubCountryPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCountryPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubcountry item)
        {
            var model = db.msubcountries;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.CountryID = item.CountryID;
                        modelItem.SubCountryName = item.SubCountryName;
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
            return PartialView("_GrdSubCountryPartial", model.ToList());
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
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            ViewData["CountryList"] = db.mcountries.ToList();
            return PartialView("_GrdSubCountryPartial", model.ToList());
        }
    }
}