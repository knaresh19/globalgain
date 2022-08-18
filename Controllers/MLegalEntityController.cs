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

        [ValidateInput(false)]
        public ActionResult GrdLegalEntityPartial()
        {
            List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N").ToList();            
            ViewData["BrandList"] = lst;
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["Subcountrylist"] = db.msubcountries.ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            var model = db.mlegalentities.ToList();
            return PartialView("_GrdLegalEntityPartial", model.Where(P => lst.Any(s => s.id == P.BrandID)));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdLegalEntityPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mlegalentity item)
        {
            var model = db.mlegalentities;
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
            List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N").ToList();
            ViewData["BrandList"] = lst;
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["Subcountrylist"] = db.msubcountries.ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            return PartialView("_GrdLegalEntityPartial", model.Where(P => lst.Any(s => s.id == P.BrandID)));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdLegalEntityPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mlegalentity item)
        {
            var model = db.mlegalentities;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.BrandID = item.BrandID;
                        modelItem.CountryID = item.CountryID;
                        modelItem.LegalEntityName = item.LegalEntityName;
                        modelItem.SubCountryID = item.SubCountryID;
                        modelItem.CostControlSiteID = item.CostControlSiteID;
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
            List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N").ToList();
            ViewData["BrandList"] = lst;
            ViewData["CountryList"] = db.mcountries.ToList();
            return PartialView("_GrdLegalEntityPartial", model.Where(P => lst.Any(s => s.id == P.BrandID)));
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
                }
            }
            List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N").ToList();
            ViewData["BrandList"] = lst;
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["Subcountrylist"] = db.msubcountries.ToList();
            ViewData["Costcontrolsite"] = db.mcostcontrolsites.ToList();
            return PartialView("_GrdLegalEntityPartial", model.Where(P => lst.Any(s => s.id == P.BrandID)));
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