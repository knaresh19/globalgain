using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class CostActionTypeController : MyBaseController
    {
        // GET: CostActionType
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdCostActionTypePartial()
        {
            var model = db.t_cost_actiontype;
            ViewData["SavingTypeName"] = db.msavingtypes.ToList();
            ViewData["CostTypeName"] = db.mcosttypes.ToList();
            return PartialView("_GrdCostActionTypePartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)] 
        public ActionResult GrdCostActionTypePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_cost_actiontype item)
        {
            var model = db.t_cost_actiontype;
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
                    log.Error(e.Message, e);
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GrdCostActionTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostActionTypePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_cost_actiontype item)
        {
            var model = db.t_cost_actiontype;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.costitemid = item.costitemid;
                        modelItem.actiontypeid = item.actiontypeid;
                        db.SaveChanges();
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
            return PartialView("_GrdCostActionTypePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdCostActionTypePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_cost_actiontype itemx)
        {
            var model = db.t_cost_actiontype;
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
            return PartialView("_GrdCostActionTypePartial", model.ToList());
        }
    }
}