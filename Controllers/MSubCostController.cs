using DevExpress.Web.Mvc;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MSubCostController : MyBaseController
    {
        // GET: MSubCost
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdSubCostPartial()
        {
            var model = db.msubcosts.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear);
            return PartialView("_GrdSubCostPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubcost item)
        {
            var model = db.msubcosts;
            var tmodel = model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList();

            if (item.SubCostName != null && item.SubCostName != string.Empty && item.isActive!=null)
            {
                if (tmodel.Where(x => x.SubCostName.ToLower() == item.SubCostName.ToLower()).ToList().Count == 0)
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

            return PartialView("_GrdSubCostPartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubcost item)
        {
            var model = db.msubcosts;
            var tmodel = model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList();
            if (item.SubCostName != null && item.SubCostName != string.Empty && item.isActive != null)
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.SubCostName.ToLower() == item.SubCostName.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.SubCostName = item.SubCostName;
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

            return PartialView("_GrdSubCostPartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msubcost itemx)
        {
            var model = db.msubcosts;
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
            return PartialView("_GrdSubCostPartial", model.Where(x => x.InitYear == GAIN.Models.Constants.defaultyear).ToList());
        }
    }
}