using DevExpress.Data.ODataLinq.Helpers;
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
    public class MClusterController : MyBaseController
    {
        // GET: MBrand
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdClusterPartial()
        {
            var model = db.mclusters;
            ViewData["RegionList"] = db.mregions.Where(x=> x.InitYear == Constants.defaultyear).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdClusterPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdClusterPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcluster item)
        {
            var model = db.mclusters;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.RegionID != 0 && item.SubRegionID != 0 && item.CountryID != 0 && item.ClusterName != null)
            {
                if (ModelState.IsValid)
                {
                    if (tmodel.Where(x => x.ClusterName.ToLower() == item.ClusterName.ToLower() && x.RegionID == item.RegionID && x.SubRegionID == item.SubRegionID
                    && x.CountryID == item.CountryID).ToList().Count == 0)
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
                        ViewData["EditError"] = "Already Exists!.";
                }
                else
                    ViewData["EditError"] = "Please, correct all errors.";
            }
            else
                ViewData["EditError"] = "Please fill out all required fields.";

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdClusterPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdClusterPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcluster item)
        {
            var model = db.mclusters;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.RegionID != 0 && item.SubRegionID != 0 && item.CountryID != 0 && item.ClusterName != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.ClusterName.ToLower() == item.ClusterName.ToLower() && x.RegionID == item.RegionID && x.SubRegionID == item.SubRegionID && x.CountryID == item.CountryID && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.RegionID = item.RegionID;
                                modelItem.SubRegionID = item.SubRegionID;
                                modelItem.CountryID = item.CountryID;
                                modelItem.ClusterName = item.ClusterName;
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
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdClusterPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdClusterPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcluster itemx)
        {
            var model = db.mclusters;
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
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdClusterPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}