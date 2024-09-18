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
    public class MPortController : MyBaseController
    {
        // GET: MPort
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdPortPartial()
        {
            var model = db.mports.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear);
            return PartialView("_GrdPortPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdPortPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mport item)
        {
            var model = db.mports;
            var tmodel = model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList();
            if (item.PortName != null)
            {
                if (tmodel.Where(x => x.PortName.ToLower() == item.PortName.ToLower()).ToList().Count == 0)
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

            return PartialView("_GrdPortPartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdPortPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mport item)
        {
            var model = db.mports;
            var tmodel = model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList();
            if (item.PortName != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.PortName.ToLower() == item.PortName.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.PortName = item.PortName;
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

            return PartialView("_GrdPortPartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdPortPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mport itemx)
        {
            var model = db.mports;
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
            return PartialView("_GrdPortPartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
    }
}