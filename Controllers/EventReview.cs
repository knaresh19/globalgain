using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;
using System.Configuration;

namespace GAIN.Controllers
{    public class EventReviewController : MyBaseController
     {
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        public ActionResult GrdEventReviewPartial()
        {
            var profileData = Session["EventReviewID"] as EventReviewSession;
            var profileData2 = Session["DefaultGAINSess"] as LoginSession;

            //var ID = Int64.Parse(profileData.ID);
            //var model = db.logtables.Where(c => c.id == ID);
            //return PartialView("~/Views/ActiveInitiative/_GrdEventReviewPartial.cshtml", model.ToList());
            var model = new List<logtable>();
            var ID = (profileData == null ? 0 : Int64.Parse(profileData.ID));
            var initiative = db.t_initiative.Where(c => c.id == ID).FirstOrDefault();
            if (initiative != null)
            {
                if (initiative.GenKey != null)
                {
                    model = db.logtables.Where(c => (c.initnumber == initiative.InitNumber || c.genKey == initiative.GenKey) && c.projectyear >= (profileData2.ProjectYear - 1)).ToList();
                }
                else
                {
                    model = db.logtables.Where(c => c.initnumber == initiative.InitNumber && c.projectyear >= (profileData2.ProjectYear-1)).ToList();
                }

                //manipulate the model accordingly to show the current value for File upload
                foreach(logtable lt in model)
                {
                    if (lt.newValues != null && lt.newValues.Length > 0)
                    {
                        if (lt.columnsName.Equals("UploadedFile"))
                        {
                            List<string> oldfiles = lt.oldValues != null ? lt.oldValues.Split('|').ToList() : new List<string>();
                            List<string> newfiles = lt.newValues != null ? lt.newValues.Split('|').ToList() : new List<string>();
                            List<string> diff;
                            if (oldfiles.Count > newfiles.Count)
                            {
                                lt.columnsName = "RemovedFile";
                                diff = oldfiles.Except(newfiles).ToList();
                                diff.Remove(initiative.InitNumber);
                                lt.newValues = diff.FirstOrDefault();
                            }
                            else
                            {
                                lt.columnsName = "UploadedFIle";
                                diff = newfiles.Except(oldfiles).ToList();
                                diff.Remove(initiative.InitNumber);
                                lt.newValues = diff.FirstOrDefault();
                            }
                            //calculate the new values

                            //empty the old values 
                            lt.oldValues = string.Empty;
                        }
                    }
                }
                ViewBag.Initnumber = initiative.InitNumber;
            } else
            {
                model = db.logtables.Where(c => c.initnumber == ID.ToString()).ToList();
                ViewBag.Initnumber = "";
            }

            return PartialView("~/Views/ActiveInitiative/_GrdEventReviewPartial.cshtml", model);
        }

        public ActionResult SetEventReviewID(FormPost PostedData)
        {
            EventReviewSession EventReviewSession = new EventReviewSession
            {
                ID = PostedData.ID
            };
            Session["EventReviewID"] = EventReviewSession;
            var profileData = Session["DefaultGAINSess"] as LoginSession;

            var model = new List<logtable>();
            var ID = Int64.Parse(PostedData.ID);
            var initiative = db.t_initiative.Where(c => c.id == ID).FirstOrDefault();
            if (initiative.GenKey != null)
            {
                model = db.logtables.Where(c => c.initnumber == initiative.InitNumber || c.genKey == initiative.GenKey).ToList();
            }
            else
            {
                model = db.logtables.Where(c => c.initnumber == initiative.InitNumber).ToList();
            }

            //EventReviewSession.ID  = PostedData.ID;
            //EventReviewSession.InitiativeNumber = initiative.InitNumber;
            //Session["EventReviewID"] = EventReviewSession;

            return Content(initiative.InitNumber);
            //return PartialView("~/Views/ActiveInitiative/_GrdEventReviewPartial.cshtml", model);
        }
    }
}
