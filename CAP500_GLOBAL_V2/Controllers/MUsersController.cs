using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GAIN.Controllers
{
    public class MUsersController : Controller
    {
        // GET: MUsers
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        [ValidateInput(false)]
        public ActionResult GrdMUsersPartial()
        {
            var model = db.user_list;
            return PartialView("_GrdMUsersPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMUsersPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.user_list item)
        {
            var model = db.user_list;
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
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GrdMUsersPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMUsersPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.user_list item)
        {
            var model = db.user_list;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.user_id == item.user_id);
                    if (modelItem != null)
                    {
                        modelItem.username = item.username;
                        modelItem.email = item.email;
                        modelItem.USER_FIRST_NAME = item.USER_FIRST_NAME;
                        modelItem.USER_MIDDLE_NAME = item.USER_MIDDLE_NAME;
                        modelItem.USER_LAST_NAME = item.USER_LAST_NAME;
                        modelItem.encPassword = GAIN.Controllers.LoginController.GetSha1(item.encPassword.Trim());
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GrdMUsersPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMUsersPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.user_list itemx)
        {
            var model = db.user_list;
            if (itemx.user_id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.user_id == itemx.user_id);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GrdMUsersPartial", model.ToList());
        }
    }
}