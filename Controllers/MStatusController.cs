using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MStatusController : MyBaseController
    {
        // GET: MStatus
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdStatusPartial()
        {
            var model = db.mstatus.Where(x => x.InitYear == 2024);
            return PartialView("_GrdStatusPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdStatusPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mstatu item)
        {
            var model = db.mstatus;
            var tmodel = model.Where(x => x.InitYear == 2024).ToList();
            if (item.Status != null && item.Status != string.Empty)
            {
                if (tmodel.Where(x => x.Status.ToLower() == item.Status.ToLower()).ToList().Count == 0)
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
            }
            else
                ViewData["EditError"] = "Please fill out all required fields.";
            return PartialView("_GrdStatusPartial", model.Where(x => x.InitYear == 2024).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdStatusPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mstatu item)
        {
            var model = db.mstatus;
            var tmodel = model.Where(x => x.InitYear == 2024).ToList();
            if (item.Status != null && item.Status != string.Empty)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.Status.ToLower() == item.Status.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.Status = item.Status;
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

            return PartialView("_GrdStatusPartial", model.Where(x => x.InitYear == 2024).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdStatusPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mstatu itemx)
        {
            var model = db.mstatus;
            if (itemx.id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.id == itemx.id);
                    if (item != null)
                        item.isActive = "N";
                        //model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GrdStatusPartial", model.Where(x => x.InitYear == 2024).ToList());
        }
    }
}