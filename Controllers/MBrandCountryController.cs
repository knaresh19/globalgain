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
    public class MBrandCountryController : MyBaseController
    {
        // GET: MBrandCountry
        public ActionResult Index()
        {
            return View();
        }

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));

        [ValidateInput(false)]
        public ActionResult GrdBrandCountryPartial()
        {
           List<mbrand> lst = db.mbrands.Where(s => s.isDeleted == "N").ToList();
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["BrandList"] = lst;
            ViewData["Subcountry"] = db.msubcountries.ToList();
            var model = db.mbrandcountries;
            return PartialView("_GrdBrandCountryPartial", model.ToList().Where(P => lst.Any(s => s.id == P.brandid)));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandCountryPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrandcountry item)
        {
            var model = db.mbrandcountries;
            if (ModelState.IsValid)
            {
                try
                {
                   // item.msubcountry.isActive = "Y";
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
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["BrandList"] = lst;
            ViewData["Subcountry"] = db.msubcountries.ToList();
            return PartialView("_GrdBrandCountryPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandCountryPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrandcountry item)
        {
            var model = db.mbrandcountries;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.id == item.id);
                    if (modelItem != null)
                    {
                        modelItem.brandid = item.brandid;
                        modelItem.countryid = item.countryid;
                        modelItem.subcountryid = item.subcountryid;
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
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["BrandList"] = lst;
            ViewData["Subcountry"] = db.msubcountries.ToList();
            return PartialView("_GrdBrandCountryPartial", model.ToList().Where(P => lst.Any(s => s.id == P.brandid)));
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdBrandCountryPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.mbrandcountry itemx )
        {
            var model = db.mbrandcountries;
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
            ViewData["CountryList"] = db.mcountries.ToList();
            ViewData["BrandList"] = lst;
            ViewData["Subcountry"] = db.msubcountries.ToList();
            return PartialView("_GrdBrandCountryPartial", model.ToList().Where(P => lst.Any(s => s.id == P.brandid)));
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Getsubcountrybycountry(int CountryId)
        {
            
            //var costcontrolid = db.t_subctry_costcntrlsite.Where(sc => sc.subcountryid == subcountryID && sc.brandid==brandId).FirstOrDefault().costcontrolid;
            //ml.Costcontrolsite = db.mcostcontrolsites.Where(c => c.id == costcontrolid).Select(s => s.CostControlSiteName).FirstOrDefault();
            ////var costcontrolsite = (from costcontrol in db.t_subctry_costcntrlsite
            ////                       join costcontrolsites in db.mcostcontrolsites
            ////                       on costcontrol.costcontrolid equals costcontrolsites.id
            ////                       where costcontrol.subcountryid == subcountryID &&
            ////                       costcontrol.brandid == brandId
            ////                       select costcontrolsites.id).FirstOrDefault();
            // ml.Costcontrolsite = db.mcostcontrolsites.Select(x=>x.)
            //var country = (from subcountries in db.msubcountries
            //               join countries in db.mcountries
            //               on subcountries.CountryID equals countries.id
            //               where subcountries.id == subcountryID
            //               select countries.id).FirstOrDefault();
            //var subcountrylist = from subcountries in db.msubcountries
            //                     where subcountries.CountryID == CountryId
            //                     select subcountries;
            List<SubCountryList> SubCountryList = new List<SubCountryList>();

            SubCountryList = (from subcountries in db.msubcountries
                             where subcountries.CountryID == CountryId
                             select subcountries).ToList().Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();

            //ml.Countryname = country.ToString();
            //ml.Costcontrolsite = costcontrolsite.ToString();
            //lst_me.Add(ml);
            //List<db.msubcountries> GDSC = new List<>();
            return Json(SubCountryList, JsonRequestBehavior.AllowGet);
        }
    }
}