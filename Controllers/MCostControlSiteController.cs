using DevExpress.DashboardWeb.Native;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GAIN.Controllers
{
    public class MCostControlSiteController : MyBaseController
    {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        // GET: MCostControlSite
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GrdCostControlSitePartial()
        {
            var model = db.mcostcontrolsites.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear);
            return PartialView("_GrdCostControlSitePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostControlSitePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcostcontrolsite item)
        {
            var model = db.mcostcontrolsites;
            var tmodel = model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList();
            if (item.CostControlSiteName != null && item.CostControlSiteName != string.Empty )
            {
                if (tmodel.Where(x => x.CostControlSiteName.ToLower() == item.CostControlSiteName.ToLower()).ToList().Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            item.InitYear = GAIN.Models.Constants.defaultyear;
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
            }
            else
                ViewData["EditError"] = "Please fill out all required fields.";

            return PartialView("_GrdCostControlSitePartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostControlSitePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcostcontrolsite item)
        {
            var model = db.mcostcontrolsites;
            var tmodel = model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList();
            if (item.CostControlSiteName != null && item.CostControlSiteName != string.Empty)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.CostControlSiteName.ToLower() == item.CostControlSiteName.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.CostControlSiteName = item.CostControlSiteName;
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
            }
            else
                ViewData["EditError"] = "Please fill out all required fields.";

            return PartialView("_GrdCostControlSitePartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostControlSitePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mcostcontrolsite itemx)
        {
            var model = db.mcostcontrolsites;
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
            return PartialView("_GrdCostControlSitePartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
    }
}
