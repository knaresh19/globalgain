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
    public class SubCostInitiativeController : MyBaseController
    {
        // GET: CostActionType
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdSubCostInitiativePartial() 
        {
            var model = db.t_subcostinitiative;
            ViewData["SavingTypeName"] = db.msavingtypes.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["CostTypeName"] = db.mcosttypes.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCostInitiativePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }

        [HttpPost, ValidateInput(false)] 
        public ActionResult GrdSubCostInitiativePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostinitiative item)
        {
            var model = db.t_subcostinitiative;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.savingtypeid != 0 && item.costitemid != 0 && item.subcostid != 0)
            {
                if (tmodel.Where(x => x.savingtypeid == item.savingtypeid && x.costitemid== item.costitemid && x.subcostid== item.subcostid).ToList().Count == 0)
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

            return PartialView("_GrdSubCostInitiativePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostInitiativePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostinitiative item)
        {
            var model = db.t_subcostinitiative;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.savingtypeid != 0 && item.costitemid != 0 && item.subcostid != 0)
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.savingtypeid == item.savingtypeid && x.costitemid == item.costitemid && x.subcostid== item.subcostid && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.savingtypeid = item.savingtypeid;
                                modelItem.costitemid = item.costitemid;
                                modelItem.subcostid = item.subcostid;
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

            return PartialView("_GrdSubCostInitiativePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostInitiativePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostinitiative itemx)
        {
            var model = db.t_subcostinitiative;
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
            return PartialView("_GrdSubCostInitiativePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}