using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;

namespace GAIN.Controllers
{
    public class MLegalEntityController : MyBaseController
    {
        // GET: MLegalEntity
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ValidateInput(false)]
        public ActionResult GrdLegalEntityPartial()
        {
            List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N" && s.InitYear == Constants.defaultyear).ToList();            
            ViewData["BrandList"] = lst;
            ViewData["CountryList"] = db.mcountries.Where(s =>  s.InitYear == Constants.defaultyear).ToList();
            ViewData["Subcountrylist"] = db.msubcountries.Where(s =>  s.InitYear == Constants.defaultyear).ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.Where(s => s.InitYear == Constants.defaultyear).ToList();
            var model = db.mlegalentities.Where(s => s.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdLegalEntityPartial", model.ToList().Where(P => lst.Any(s => s.id == P.BrandID)));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdLegalEntityPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mlegalentity item)
        {
            var model = db.mlegalentities;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.BrandID != 0 && item.CountryID != 0 && item.LegalEntityName != null)
            {
                if (tmodel.Where(x => x.LegalEntityName.ToLower() == item.LegalEntityName.ToLower() && x.BrandID==item.BrandID && x.CountryID == item.CountryID && 
                x.SubCountryID == item.SubCountryID && x.CostControlSiteID == item.CostControlSiteID).ToList().Count == 0)
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

            List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N" && s.InitYear == Constants.defaultyear).ToList();
            ViewData["BrandList"] = lst;
            ViewData["CountryList"] = db.mcountries.Where(s => s.InitYear == Constants.defaultyear).ToList();
            ViewData["Subcountrylist"] = db.msubcountries.Where(s => s.InitYear == Constants.defaultyear).ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.Where(s => s.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdLegalEntityPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList().Where(P => lst.Any(s => s.id == P.BrandID)));

        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdLegalEntityPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mlegalentity item)
        {
            var model = db.mlegalentities;
            var tmodel = model.Where(x => x.InitYear == Constants.defaultyear).ToList();

            if (item.BrandID != 0 && item.CountryID != 0 && item.LegalEntityName != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var modelItem = model.FirstOrDefault(it => it.id == item.id);
                        if (modelItem != null)
                        {
                            if (tmodel.Where(x => x.LegalEntityName.ToLower() == item.LegalEntityName.ToLower() && x.BrandID == item.BrandID && x.CountryID == item.CountryID &&
                x.SubCountryID == item.SubCountryID && x.CostControlSiteID == item.CostControlSiteID && x.id != item.id).ToList().Count == 0)
                            {
                                modelItem.BrandID = item.BrandID;
                                modelItem.CountryID = item.CountryID;
                                modelItem.LegalEntityName = item.LegalEntityName;
                                modelItem.SubCountryID = item.SubCountryID;
                                modelItem.CostControlSiteID = item.CostControlSiteID;
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

            List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N" && s.InitYear == Constants.defaultyear).ToList();
            ViewData["BrandList"] = lst;
            ViewData["CountryList"] = db.mcountries.Where(s => s.InitYear == Constants.defaultyear).ToList();
            ViewData["Subcountrylist"] = db.msubcountries.Where(s => s.InitYear == Constants.defaultyear).ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.Where(s => s.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdLegalEntityPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList().Where(P => lst.Any(s => s.id == P.BrandID)));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdLegalEntityPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mlegalentity itemx)
        {
            var model = db.mlegalentities;
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
            List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N" && s.InitYear == Constants.defaultyear).ToList();
            ViewData["BrandList"] = lst;
            ViewData["CountryList"] = db.mcountries.Where(s => s.InitYear == Constants.defaultyear).ToList();
            ViewData["Subcountrylist"] = db.msubcountries.Where(s => s.InitYear == Constants.defaultyear).ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.Where(s => s.InitYear == Constants.defaultyear).ToList();
            return PartialView("_GrdLegalEntityPartial", model.Where(x => x.InitYear == Constants.defaultyear).ToList().Where(P => lst.Any(s => s.id == P.BrandID)));
        }
        [HttpPost]
        public ActionResult Getdetailsbysubcountry(int subcountryID, int brandId)
        {
            List<MasterLegalentity> lst_me = new List<MasterLegalentity>();
            MasterLegalentity ml = new MasterLegalentity();
            //var costcontrolid = db.t_subctry_costcntrlsite.Where(sc => sc.subcountryid == subcountryID && sc.brandid==brandId).FirstOrDefault().costcontrolid;
            //ml.Costcontrolsite = db.mcostcontrolsites.Where(c => c.id == costcontrolid).Select(s => s.CostControlSiteName).FirstOrDefault();
            var costcontrolsite = (from costcontrol in db.t_subctry_costcntrlsite
                                   join costcontrolsites in db.mcostcontrolsites
                                   on costcontrol.costcontrolid equals costcontrolsites.id
                                   where costcontrol.subcountryid == subcountryID &&
                                   costcontrol.brandid == brandId
                                   select costcontrolsites.id).FirstOrDefault();
            // ml.Costcontrolsite = db.mcostcontrolsites.Select(x=>x.)
            var country = (from subcountries in db.msubcountries
                           join countries in db.mcountries
                           on subcountries.CountryID equals countries.id
                           where subcountries.id == subcountryID
                           select countries.id).FirstOrDefault();
                             
            ml.Countryname = country.ToString();
            ml.Costcontrolsite = costcontrolsite.ToString();
            lst_me.Add(ml);
            return Json(lst_me);
        }
    }
}