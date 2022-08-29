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
    public class MCountryController : MyBaseController
    {
        // GET: MCountry
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdCountryPartial()
        {
            var model = db.mcountries;
            ViewData["RegionList"] = db.mregions.ToList();
            ViewData["SubRegionList"] = db.msubregions.ToList();
            return PartialView("_GrdCountryPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCountryPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcountry item)
        {
            var model = db.mcountries;
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

            ViewData["RegionList"] = db.mregions.ToList();
            ViewData["SubRegionList"] = db.msubregions.ToList();
            return PartialView("_GrdCountryPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCountryPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcountry item)
        {
            var model = db.mcountries;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.RegionID = item.RegionID;
                        modelItem.SubRegionID = item.SubRegionID;
                        modelItem.CountryName = item.CountryName;
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

            ViewData["RegionList"] = db.mregions.ToList();
            ViewData["SubRegionList"] = db.msubregions.ToList();
            return PartialView("_GrdCountryPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCountryPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcountry itemx)
        {
            var model = db.mcountries;
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

            ViewData["RegionList"] = db.mregions.ToList();
            ViewData["SubRegionList"] = db.msubregions.ToList();
            return PartialView("_GrdCountryPartial", model.ToList());
        }


        [HttpPost]
        public ActionResult GetRegionDetailsbySubRegion(int subRegionID)
        {
            List<MasterCountry> lst_me = new List<MasterCountry>();
            MasterCountry ml = new MasterCountry();            
            var region_name = (from subregion in db.msubregions
                           join region in db.mregions
                           on subregion.RegionID equals region.id
                           where subregion.id ==  subRegionID
                           select region.id).FirstOrDefault();
            ml.RegionName = region_name.ToString();
            lst_me.Add(ml);
            return Json(lst_me);
        }

    }
}