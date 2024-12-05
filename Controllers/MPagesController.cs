using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MPagesController : MyBaseController
    {
        // GET: MPages
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdMPagesPartial()
        {
            var model = db.mmenus;
            return PartialView("_GrdMPagesPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMUsersPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mmenu item)
        {
            var model = db.mmenus;
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
            return PartialView("_GrdMPagesPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMPagesPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mmenu item)
        {
            var model = db.mmenus;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.code = item.code;
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
            return PartialView("_GrdMPagesPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMPagesPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mmenu itemx)
        {
            var model = db.mmenus;
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
            return PartialView("_GrdMPagesPartial", model.ToList());
        }
    }
}