using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class MyBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["DefaultGAINSess"] == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (filterContext.HttpContext.Request.Params["DXCallbackName"] != null)
                    {
                        filterContext.Result = RedirectToAction("", "login", new { Messages = "Session Expired. Please login to the system !" });
                    }
                    else
                        filterContext.Result = Content("__sessiontimeout");
                }
                else
                    filterContext.Result = RedirectToAction("index", "login", new { Messages = "Session Expired. Please login to the system !" });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}