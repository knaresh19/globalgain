using DevExpress.Web.Mvc;
using GAIN.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MSynImpactController : MyBaseController
    {
        // GET: MSynImpact
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdSynergyImpactPartial()
        {
            var model = db.msynimpacts;
            return PartialView("_GrdSynergyImpactPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSynergyImpactPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msynimpact item)
        {
            var model = db.msynimpacts;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.SynImpactName != null && item.SynImpactName != string.Empty && item.isActive !=null)
            {
                if (tmodel.Where(x => x.SynImpactName.ToLower() == item.SynImpactName.ToLower()).ToList().Count == 0)
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

            return PartialView("_GrdSynergyImpactPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSynergyImpactPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msynimpact item)
        {
            var model = db.msynimpacts;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();
            if (item.SynImpactName != null && item.SynImpactName != string.Empty && item.isActive != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.SynImpactName.ToLower() == item.SynImpactName.ToLower() && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.SynImpactName = item.SynImpactName;
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
                        log.Error(e.Message, e);
                    }
                }
                else
                    ViewData["EditError"] = "Please, correct all errors.";
            }
            else
                ViewData["EditError"] = "Please fill out all required fields.";

            return PartialView("_GrdSynergyImpactPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdSynergyImpactPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.msynimpact itemx)
        {
            var model = db.msynimpacts;
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
                    log.Error(e.Message, e);
                }
            }
            return PartialView("_GrdSynergyImpactPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList());
        }
    }
}