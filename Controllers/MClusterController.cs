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
            ViewData["RegionList"] = db.mregions.ToList();
            ViewData["SubRegionList"] = db.msubregions.ToList();
            ViewData["CountryList"] = db.mcountries.ToList();
            return PartialView("_GrdClusterPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdClusterPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcluster item)
        {
            var model = db.mclusters;
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
            ViewData["CountryList"] = db.mcountries.ToList();
            return PartialView("_GrdClusterPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdClusterPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcluster item)
        {
            var model = db.mclusters;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.RegionID = item.RegionID;
                        modelItem.SubRegionID = item.SubRegionID;
                        modelItem.CountryID = item.CountryID;
                        modelItem.ClusterName = item.ClusterName;
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
            ViewData["CountryList"] = db.mcountries.ToList();
            return PartialView("_GrdClusterPartial", model.ToList());
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

            ViewData["RegionList"] = db.mregions.ToList();
            ViewData["SubRegionList"] = db.msubregions.ToList();
            ViewData["CountryList"] = db.mcountries.ToList();
            return PartialView("_GrdClusterPartial", model.ToList());
        }
    }
}