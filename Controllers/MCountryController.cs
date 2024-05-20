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
            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == 2024 && x.isActive == 1).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == 2024).ToList();
            return PartialView("_GrdCountryPartial", model.Where(x => x.InitYear == 2024).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCountryPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcountry item)
        {
            var model = db.mcountries;
            var tmodel = model.Where(x => x.InitYear == 2024).ToList();

            if (tmodel.Where(x => x.CountryName == item.CountryName).ToList().Count == 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        item.InitYear = 2024;
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

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == 2024 && x.isActive == 1).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == 2024).ToList();
            return PartialView("_GrdCountryPartial", model.Where(x => x.InitYear == 2024).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCountryPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcountry item)
        {
            var model = db.mcountries;
            var tmodel = model.Where(x => x.InitYear == 2024).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        if (tmodel.Where(x => x.RegionID == item.RegionID && x.SubRegionID == item.SubRegionID && x.CountryName == item.CountryName && x.id != item.id).ToList().Count == 0)
                        {
                            modelItem.RegionID = item.RegionID;
                            modelItem.SubRegionID = item.SubRegionID;
                            modelItem.CountryName = item.CountryName;
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == 2024 && x.isActive == 1).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == 2024).ToList();
            return PartialView("_GrdCountryPartial", model.Where(x => x.InitYear == 2024).ToList());
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

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == 2024 && x.isActive == 1).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == 2024).ToList();
            return PartialView("_GrdCountryPartial", model.Where(x => x.InitYear == 2024).ToList());
        }


        //[HttpPost]
        //public ActionResult GetRegionDetailsbySubRegion(int subRegionID)
        //{
        //    List<MasterCountry> lst_me = new List<MasterCountry>();
        //    MasterCountry ml = new MasterCountry();            
        //    var region_name = (from subregion in db.msubregions
        //                   join region in db.mregions
        //                   on subregion.RegionID equals region.id
        //                   where subregion.id ==  subRegionID
        //                   select region.id).FirstOrDefault();
        //    ml.RegionName = region_name.ToString();
        //    lst_me.Add(ml);
        //    return Json(lst_me);
        //}

        [HttpPost]
        public ActionResult GetSubRegionByRegionID(int RegionID)
        {
            List<MasterCountry> lst_me = new List<MasterCountry>();
            MasterCountry ml = new MasterCountry();
            var subregion_name = (from subregion in db.msubregions
                               join region in db.mregions
                               on subregion.RegionID equals region.id
                               where region.id == RegionID
                               select region.id).FirstOrDefault();
            ml.SubRegionName = subregion_name.ToString();
            lst_me.Add(ml);
            return Json(lst_me);
        }

    }
}