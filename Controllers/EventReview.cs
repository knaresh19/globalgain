using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{    public class EventReviewController : MyBaseController
     {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        public ActionResult GrdEventReviewPartial()
        {
            var profileData = Session["EventReviewID"] as EventReviewSession;

            var ID = Int64.Parse(profileData.ID);
            var model = db.logtables.Where(c => c.id == ID);
            return PartialView("~/Views/ActiveInitiative/_GrdEventReviewPartial.cshtml", model.ToList());
        }

        public ActionResult SetEventReviewID(FormPost PostedData)
        {
            EventReviewSession EventReviewSession = new EventReviewSession
            {
                ID = PostedData.ID
            };
            Session["EventReviewID"] = EventReviewSession;

            var model = new List<logtable>();
            var ID = Int64.Parse(PostedData.ID);
            var initiative = db.t_initiative.Where(c => c.id == ID).FirstOrDefault();
            if (initiative.GenKey != null)
            {
                model = db.logtables.Where(c => c.initnumber == initiative.InitNumber && c.genKey == initiative.GenKey).ToList();
            }
            else
            {
                model = db.logtables.Where(c => c.initnumber == initiative.InitNumber).ToList();
            }

            ViewBag.Initnumber = initiative.InitNumber;
            return PartialView("~/Views/ActiveInitiative/_GrdEventReviewPartial.cshtml", model);
        }
    }
}