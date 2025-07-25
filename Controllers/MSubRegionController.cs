﻿using DevExpress.Web.Mvc;
using GAIN.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MSubRegionController : MyBaseController
    {
        // GET: MSubRegion
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdSubRegionPartial()
        {
            var model = db.msubregions.Where(x => x.InitYear == Constants.defaultyear);
            ViewData["RegionList"] = db.mregions.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubRegionPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubRegionPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubregion item)
        {
            var model = db.msubregions;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.SubRegionName != null && item.SubRegionName != string.Empty && item.RegionID != 0)
            {
                if (tmodel.Where(x => x.SubRegionName.ToLower() == item.SubRegionName.ToLower() && x.RegionID ==item.RegionID).ToList().Count == 0)
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
            return PartialView("_GrdSubRegionPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubRegionPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubregion item)
        {
            var model = db.msubregions;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.SubRegionName != null && item.SubRegionName != string.Empty && item.RegionID != 0)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.SubRegionName.ToLower() == item.SubRegionName.ToLower() && x.RegionID == item.RegionID && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.RegionID = item.RegionID;
                                modelItem.SubRegionName = item.SubRegionName;
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
            return PartialView("_GrdSubRegionPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubRegionPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubregion itemx)
        {
            var model = db.msubregions;
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

            ViewData["RegionList"] = db.mregions.ToList();
            return PartialView("_GrdSubRegionPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}