using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.Web.Mvc;
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

        [ValidateInput(false)]
        public ActionResult GrdClusterPartial()
        {
            var model = db.mclusters;
            ViewData["RegionList"] = db.mregions.Where(x=> x.InitYear ==2024).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == 2024).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == 2024).ToList();
            return PartialView("_GrdClusterPartial", model.Where(x => x.InitYear == 2024).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdClusterPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcluster item)
        {
            var model = db.mclusters;
            var tmodel = model.Where(x => x.InitYear == 2024).ToList();

            if (ModelState.IsValid)
            {
                if (tmodel.Where(x => x.ClusterName == item.ClusterName && x.RegionID==item.RegionID && x.SubRegionID==item.SubRegionID
                && x.CountryID==item.CountryID).ToList().Count == 0)
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
                    ViewData["EditError"] = "Already Exists!.";
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == 2024).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == 2024).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == 2024).ToList();
            return PartialView("_GrdClusterPartial", model.Where(x => x.InitYear == 2024).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdClusterPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcluster item)
        {
            var model = db.mclusters;
            var tmodel = model.Where(x => x.InitYear == 2024).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        if (tmodel.Where(x => x.ClusterName == item.ClusterName && x.RegionID == item.RegionID && x.SubRegionID == item.SubRegionID && x.CountryID==item.CountryID && x.id != item.id).ToList().Count == 0)
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
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == 2024).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == 2024).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == 2024).ToList();
            return PartialView("_GrdClusterPartial", model.Where(x => x.InitYear == 2024).ToList());
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
                }
            }

            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == 2024).ToList();
            ViewData["SubRegionList"] = db.msubregions.Where(x => x.InitYear == 2024).ToList();
            ViewData["CountryList"] = db.mcountries.Where(x => x.InitYear == 2024).ToList();
            return PartialView("_GrdClusterPartial", model.Where(x => x.InitYear == 2024).ToList());
        }
    }
}