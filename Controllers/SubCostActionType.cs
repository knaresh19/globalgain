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
    public class SubCostActionTypeController : MyBaseController
    {
        // GET: CostActionType
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdSubCostActionTypePartial()
        {
            var model = db.t_subcostactiontype;
            ViewData["ActionTypeName"] = db.mactiontypes.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCostActionTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }

        [HttpPost, ValidateInput(false)] 
        public ActionResult GrdSubCostActionTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostactiontype item)
        {
            

            var model = db.t_subcostactiontype;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.actiontypeid != 0 && item.subcostid != 0 )
            {
                if (tmodel.Where(x => x.actiontypeid == item.actiontypeid && x.subcostid == item.subcostid ).ToList().Count == 0)
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



            ViewData["ActionTypeName"] = db.mactiontypes.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCostActionTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostActionTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostactiontype item)
        {
            var model = db.t_subcostactiontype;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.actiontypeid != 0 && item.subcostid != 0 )
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.actiontypeid == item.actiontypeid && x.subcostid == item.subcostid && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.actiontypeid = item.actiontypeid;
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



            ViewData["ActionTypeName"] = db.mactiontypes.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCostActionTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSubCostActionTypePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_subcostactiontype itemx)
        {
            var model = db.t_subcostactiontype;
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

            ViewData["ActionTypeName"] = db.mactiontypes.Where(x => x.InitYear == Constants.defaultyear).ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(x => x.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdSubCostActionTypePartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}