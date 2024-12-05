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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdCountryPartial()
        {
            var model = db.mcountries;
            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear && x.isActive == 1).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdCountryPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCountryPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcountry item)
        {
            var model = db.mcountries;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.RegionID != 0 && item.SubRegionID != 0 && item.CountryName != null)
            {

                if (tmodel.Where(x => x.RegionID == item.RegionID && x.SubRegionID == item.SubRegionID && x.CountryName.ToLower() == item.CountryName.ToLower()).ToList().Count == 0)
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
                            log.Error(e.Message, e);
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

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear && x.isActive == 1).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdCountryPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCountryPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcountry item)
        {
            var model = db.mcountries;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.RegionID != 0 && item.SubRegionID != 0 && item.CountryName != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.RegionID == item.RegionID && x.SubRegionID == item.SubRegionID && x.CountryName.ToLower() == item.CountryName.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.RegionID = item.RegionID;
                                modelItem.SubRegionID = item.SubRegionID;
                                modelItem.CountryName = item.CountryName;
                                db.SaveChanges();
                            }
                            else
                                ViewData["EditError"] = "Already Exists!.";
                        }
                    }
                    catch (Exception e)
                    {
                        ViewData["EditError"] = e.Message;
                        log.Error(e.Message, e);
                    }
                }
                else
                    ViewData["EditError"] = "Please, correct all errors.";
            }
            else
                ViewData["EditError"] = "Please fill out all required fields.";

                ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear && x.isActive == 1).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdCountryPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
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
                    log.Error(e.Message, e);
                }
            }

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear && x.isActive == 1).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdCountryPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }


        [HttpPost]
        public ActionResult GetRegionDetailsbySubRegion(int subRegionID)
        {
            List<MasterCountry> lst_me = new List<MasterCountry>();
            MasterCountry ml = new MasterCountry();
            var region_name = (from subregion in db.msubregions
                               join region in db.mregions
                               on subregion.RegionID equals region.id
                               where subregion.id == subRegionID && subregion.InitYear == Constants.defaultyear
                               select region.id).FirstOrDefault();
            ml.RegionName = region_name.ToString();
            lst_me.Add(ml);
            return Json(lst_me);
        }

        

    }
}