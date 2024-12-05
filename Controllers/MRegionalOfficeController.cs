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
    public class MRegionalOfficeController : MyBaseController
    {
        // GET: MRegionalOffice
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdRegionalOfficePartial()
        {
            var model = db.mregional_office;
            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdRegionalOfficePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdRegionalOfficePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mregional_office item)
        {
            var model = db.mregional_office;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.RegionID != null && item.CountryID != null && item.RegionalOffice_Name != null)
            {
                if (tmodel.Where(x => x.RegionID == item.RegionID && x.CountryID == item.CountryID && x.RegionalOffice_Name.ToLower() == item.RegionalOffice_Name.ToLower()).ToList().Count == 0)
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

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdRegionalOfficePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdRegionalOfficePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mregional_office item)
        {
            var model = db.mregional_office;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.RegionID != null && item.CountryID != null && item.RegionalOffice_Name != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.RegionID == item.RegionID && x.CountryID == item.CountryID && x.RegionalOffice_Name.ToLower() == item.RegionalOffice_Name.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.RegionID = item.RegionID;
                                modelItem.CountryID = item.CountryID;
                                modelItem.RegionalOffice_Name = item.RegionalOffice_Name;
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

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdRegionalOfficePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdRegionalOfficePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mregional_office itemx)
        {
            var model = db.mregional_office;
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

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdRegionalOfficePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}