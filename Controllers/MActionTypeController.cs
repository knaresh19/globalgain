using DevExpress.Web.Mvc;
using GAIN.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MActionTypeController : MyBaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        


        [ValidateInput(false)]
        public ActionResult GrdActionTypePartial()
        {
            var model = db.mactiontypes.Where(x => x.InitYear == Constants.defaultyear);
            return PartialView("_GrdActionTypePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdActionTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mactiontype item)
        {
            var model = db.mactiontypes;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.ActionTypeName != null && item.ActionTypeName != string.Empty && item.isActive !=null)
            {
                if (tmodel.Where(x => x.ActionTypeName.ToLower() == item.ActionTypeName.ToLower()).ToList().Count == 0)
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

            return PartialView("_GrdActionTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdActionTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mactiontype item)
        {
            var model = db.mactiontypes;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.ActionTypeName != null && item.ActionTypeName != string.Empty && item.isActive != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null )
                        {
                            
                            if (tmodel.Where(x => x.ActionTypeName.ToLower() == item.ActionTypeName.ToLower() && x.id != item.id).ToList().Count==0)
                            {
                                modelItem.ActionTypeName = item.ActionTypeName;
                                modelItem.isActive = item.isActive;
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
            return PartialView("_GrdActionTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost]
        public ActionResult GrdActionTypePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mactiontype itemx)
        {
            var model = db.mactiontypes;
            if (itemx.id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.id == itemx.id);
                    if (item != null)
                        item.isActive = "N"; //soft delete
                        //model.Remove(item); 
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GrdActionTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}
