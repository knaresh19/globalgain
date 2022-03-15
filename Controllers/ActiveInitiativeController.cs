using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using GAIN.Models;
using Newtonsoft.Json;
using DevExpress.Web;
using DevExpress.Web.Export;
using DevExpress.XtraCharts;
using System.Configuration;

/*
 * Adding comment here
 */

namespace GAIN.Controllers
{
  
    public class ActiveInitiativeController : MyBaseController
    {

        //GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        //GAIN.Models.GainEntities db = new GAIN.Models.GainEntities("metadata=res://*/Models.GainModel.csdl|res://*/Models.GainModel.ssdl|res://*/Models.GainModel.msl;provider=MySql.Data.MySqlClient;provider connection string='server=127.0.0.1; Port=3306; user id=root;password=admin;Sslmode=none;persistsecurityinfo=True;database=gain_v2;Persist Security Info=True;Convert Zero Datetime=true'");
        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        //[Authorize]
        // GET: ActiveInitiative
        private static readonly log4net.ILog log =
log4net.LogManager.GetLogger
(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            //vwheaderinitiative model = new vwheaderinitiative();
            return View();
        }
        public ActionResult ProjectYear(Models.GetInfoByIDModel GetInfo)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            LoginSession LoginSession = new LoginSession
            {
                ID = profileData.ID,
                ProjectYear = GetInfo.Id,
                UserType = profileData.UserType,
                CountryCode = profileData.CountryCode,
                RegionID = profileData.RegionID,
                CountryID = profileData.CountryID,
                CostControlSite = profileData.CostControlSite,
                subcountry_right = profileData.subcountry_right,
                RegionalOffice_right = profileData.RegionalOffice_right,
                CostControlSite_right = profileData.CostControlSite_right,
                Brand_right = profileData.Brand_right,
                CostItem_right = profileData.CostItem_right,
                SubCostItem_right = profileData.SubCostItem_right,
                validity_right = profileData.validity_right,
                confidential_right = profileData.confidential_right,
                years_right = profileData.years_right,
                istoadmin = profileData.istoadmin
            };
            Session["DefaultGAINSess"] = LoginSession;
            return Content("Ok");
        }
        public ActionResult NewInitiative()
        {
            ViewData["NEWIN"] = "Y";
            return PartialView("_NewInitiative");
        }
        [ValidateInput(false)]
        public ActionResult GrdMainInitiativePartial() // Main Function to show data in Main Grid
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var where = "";

            if (profileData.confidential_right == 0) where += " and Confidential != 'Y'";
            if (profileData.RegionalOffice_right!=null && profileData.RegionalOffice_right != "" && profileData.RegionalOffice_right.Substring(0, 1) == "|")
            {
                var regofficetext = profileData.RegionalOffice_right.Replace("|", "','");
                int lenRegionalOffice_right = regofficetext.Length;
                regofficetext = "(" + regofficetext.Substring(2, (lenRegionalOffice_right - 4)) + ")";
                var RegionalOffice_rightid = db.mregional_office.SqlQuery("select id,RegionID,CountryID,RegionalOffice_Name,SubCountryID,BrandID from mregional_office where RegionalOffice_Name in " + regofficetext + " group by id,RegionID,CountryID,RegionalOffice_Name,SubCountryID,BrandID").ToList();
                var RegionalOffice_rightcondition = "";
                for (var i = 0; i < RegionalOffice_rightid.Count(); i++)
                {
                    RegionalOffice_rightcondition += RegionalOffice_rightid[i].id.ToString() + ",";
                }

                if (RegionalOffice_rightcondition != "") //to prevent error when regional office is disabled but some user still set to that regional office.
                {
                    RegionalOffice_rightcondition = RegionalOffice_rightcondition.Substring(0, RegionalOffice_rightcondition.Length - 1);
                    where += " and a.RegionalOfficeID in (" + RegionalOffice_rightcondition + ")";
                }
            }
            if (profileData.Brand_right != "" && (profileData.Brand_right != "ALL"|| profileData.Brand_right !="|ALL|"))
            {
                var brandtext = profileData.Brand_right.Replace("|", "','");
                int lenbrand = brandtext.Length;
                brandtext = "(" + brandtext.Substring(2, (lenbrand - 4)) + ")";
                var brandid = db.mbrands.SqlQuery("select id,brandname,isActive from mbrand where brandname in " + brandtext + " group by id,brandname").ToList();
                var brandcondition = "";
                for (var i = 0; i < brandid.Count(); i++)
                {
                    brandcondition += brandid[i].id.ToString() + ",";
                }

                if (brandcondition != "") //to prevent error when brand is disabled but some user still set to that brand.
                {
                    brandcondition = brandcondition.Substring(0, brandcondition.Length - 1);
                    where += " and a.BrandID in (" + brandcondition + ")";
                }
            }
            if (profileData.CostItem_right != "" && (profileData.CostItem_right != "ALL" || profileData.CostItem_right != "|ALL|"))
            {
                var costitemtext = profileData.CostItem_right.Replace("|", "','");
                int lencostitem = costitemtext.Length;
                costitemtext = "(" + costitemtext.Substring(2, (lencostitem - 4)) + ")";
                var costitemid = db.mcosttypes.SqlQuery("select id,CostTypeName,isActive from mcosttype where CostTypeName in " + costitemtext + " group by id,CostTypeName,isActive").ToList();
                var costitemcondition = "";
                for (var i = 0; i < costitemid.Count(); i++)
                {
                    costitemcondition += costitemid[i].id.ToString() + ",";
                }

                if (costitemcondition != "") //to prevent error when cost item is disabled but some user still set to that cost item.
                {
                    costitemcondition = costitemcondition.Substring(0, costitemcondition.Length - 1);
                    where += " and a.CostCategoryID in (" + costitemcondition + ")";
                }
            }
            if (profileData.SubCostItem_right !=null && profileData.SubCostItem_right != "" && profileData.SubCostItem_right != "ALL" )
            {
                var subcostitemtext = profileData.SubCostItem_right.Replace("|", "','");
                int lensubcostitem = subcostitemtext.Length;
                subcostitemtext = "(" + subcostitemtext.Substring(2, (lensubcostitem - 4)) + ")";
                var subcostitemid = db.msubcosts.SqlQuery("select id,SubCostName,isActive from msubcost where SubCostName in " + subcostitemtext + " group by id,SubCostName,isActive").ToList();
                var subcostitemcondition = "";
                for (var i = 0; i < subcostitemid.Count(); i++)
                {
                    subcostitemcondition += subcostitemid[i].id.ToString() + ",";
                }
                if (subcostitemcondition != "") //to prevent error when sub cost item is disabled but some user still set to that sub cost item.
                {
                    subcostitemcondition = subcostitemcondition.Substring(0, subcostitemcondition.Length - 1);
                    where += " and a.SubCostCategoryID in (" + subcostitemcondition + ")";
                }
            }

            var model = db.vwheaderinitiatives.ToList();

            if (profileData.UserType == 2)  //rpoc
            {
                if (profileData.RegionID != "" && (profileData.RegionID != "|ALL|" || profileData.RegionID !="ALL"))
                {
                    var rpoctext = profileData.RegionID.Replace("|", "','");
                    int lenrpoc = rpoctext.Length;
                    rpoctext = "(" + rpoctext.Substring(2, (lenrpoc - 4)) + ")";
                    var rpocid = db.mregions.SqlQuery("select id,RegionName,isActive from mregion where RegionName in " + rpoctext + " group by id,RegionName,isActive").ToList();
                    var rpoccondition = "";
                    for (var i = 0; i < rpocid.Count(); i++)
                    {
                        rpoccondition += rpocid[i].id.ToString() + ",";
                    }
                    rpoccondition = rpoccondition.Substring(0, rpoccondition.Length - 1);
                    where += " and a.RegionID in (" + rpoccondition + ")";
                }
                else
                {
                    if (profileData.CostControlSite != "|ALL|" || profileData.RegionID != "ALL")
                    {
                        var cctext = profileData.CostControlSite.Replace("|", "','");
                        int lencctext = cctext.Length;
                        cctext = "(" + cctext.Substring(2, (lencctext - 4)) + ")";
                        var ccid = db.mcostcontrolsites.SqlQuery("SELECT id,CostControlSiteName FROM mcostcontrolsite WHERE CostControlSiteName IN " + cctext + " group by id,CostControlSiteName ").ToList();
                        var cccondition = "";
                        for (var i = 0; i < ccid.Count(); i++)
                        {
                            cccondition += ccid[i].id.ToString() + ",";
                        }
                        cccondition = cccondition.Substring(0, cccondition.Length - 1);
                        where += " and a.CostControlID in (" + cccondition + ")";
                    }
                }
            }
            else if (profileData.UserType == 3) //agency
            {
                var cntrytext = profileData.subcountry_right.Replace("|", "','");
                int lencntrytext = cntrytext.Length;
                cntrytext = "(" + cntrytext.Substring(2, (lencntrytext - 4)) + ")";
                var cntryid = db.msubcountries.SqlQuery("SELECT id,CountryID,SubCountryName,CountryCode,isActive FROM msubcountry WHERE SubCountryName IN " + cntrytext + " group by id,CountryID,SubCountryName,CountryCode,isActive").ToList();
                var cntryidcondition = "(";
                for (var i = 0; i < cntryid.Count(); i++)
                {
                    cntryidcondition += cntryid[i].id.ToString() + ",";
                }
                cntryidcondition = cntryidcondition.Substring(0, cntryidcondition.Length - 1);

                where += " and a.SubCountryID in " + cntryidcondition + ")";
            }

            //ConsoleLog(" UserType: " + profileData.UserType + "\\n RegionID: " + profileData.RegionID + "\\n CostControlSite: " + profileData.CostControlSite + "\\n Country: " + profileData.CountryID + "\\n Condition: " + where);

           // model = db.vwheaderinitiatives.SqlQuery("select * from vwheaderinitiative as a where isDeleted = 0 and ProjectYear = '" + profileData.ProjectYear + "' " + where + " order by CreatedDate desc").ToList();
            model = db.vwheaderinitiatives.SqlQuery("select * from vwheaderinitiative as a where   isDeleted = 0 and (Year(StartMonth) = '" + profileData.ProjectYear + "' or Year(EndMonth) = '" + profileData.ProjectYear + "') " + where + " order by CreatedDate desc").ToList();

            ViewData["mregions"] = db.mregions.ToList();
            ViewData["brandname"] = db.mbrands.Where(c => c.isActive == "Y").ToList();
            ViewData["msubregion"] = db.msubregions.Where(c => c.SubRegionName != null && c.SubRegionName != "").ToList();
            //ViewData["mcluster"] = db.mclusters.SqlQuery("SELECT * FROM mcluster where ClusterName != \'\'").ToList();
            ViewData["mcluster"] = db.mclusters.Where(c => c.ClusterName != "").GroupBy(g => g.ClusterName).Select(s => new { ClusterName = s.Key }).ToList();
            ViewData["mregional_office"] = db.mregional_office.SqlQuery("SELECT * FROM mregional_office").GroupBy(g => g.RegionalOffice_Name).Select(s => new { RegionalOffice_Name = s.Key }).ToList();
            ViewData["CostControlSiteName"] = db.mcostcontrolsites.Where(c => c.CostControlSiteName != "").ToList();
            ViewData["CountryName"] = db.mcountries.Where(c => c.CountryName != "").ToList();
            ViewData["SubCountryName"] = db.msubcountries.Where(c => c.SubCountryName != "" && c.isActive == "Y").ToList();
            ViewData["LegalEntityName"] = db.mlegalentities.GroupBy(g => g.LegalEntityName).Select(s => new { LegalEntityName = s.Key }).ToList();
            ViewData["SavingTypeName"] = db.msavingtypes.Where(c => c.isActive == "Y").ToList();
            ViewData["CostTypeName"] = db.mcosttypes.Where(c => c.isActive == "Y").ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(c => c.isActive == "Y").ToList();
            ViewData["ActionTypeName"] = db.mactiontypes.Where(c => c.isActive == "Y").ToList();
            ViewData["SynImpactName"] = db.msynimpacts.Where(c => c.isActive == "Y").ToList();
            ViewData["Status"] = db.mstatus.Where(c => c.isActive == "Y").ToList();
            ViewData["portName"] = db.mports.ToList();
            ViewData["SourceCategoryName"] = db.msourcecategories.ToList();
          

            foreach (var item in model)
            {
                if (Convert.ToDateTime(item.StartMonth).Year < profileData.ProjectYear)
                {
                    item.TargetJan = item.TargetNexJan;
                    item.TargetFeb = item.TargetNexFeb;
                    item.TargetMar = item.TargetNexMar;
                    item.TargetApr = item.TargetNexApr;
                    item.TargetMay = item.TargetNexMay;
                    item.TargetJun = item.TargetNexJun;
                    item.TargetJul = item.TargetNexJul;
                    item.TargetAug = item.TargetNexAug;
                    item.TargetSep = item.TargetNexSep;
                    item.TargetOct = item.TargetNexOct;
                    item.TargetNov = item.TargetNexNov;
                    item.TargetDec = item.TargetNexDec;

                    item.TargetNexJan = null;
                    item.TargetNexFeb = null;
                    item.TargetNexMar = null;
                    item.TargetNexApr = null;
                    item.TargetNexMay = null;
                    item.TargetNexJun = null;
                    item.TargetNexJul = null;
                    item.TargetNexAug = null;
                    item.TargetNexSep = null;
                    item.TargetNexOct = null;
                    item.TargetNexNov = null;
                    item.TargetNexDec = null;

                    item.AchJan = item.AchNexJan;
                    item.AchFeb = item.AchNexFeb;
                    item.AchMar = item.AchNexMar;
                    item.AchApr = item.AchNexApr;
                    item.AchMay = item.AchNexMay;
                    item.AchJun = item.AchNexJun;
                    item.AchJul = item.AchNexJul;
                    item.AchAug = item.AchNexAug;
                    item.AchSep = item.AchNexSep;
                    item.AchOct = item.AchNexOct;
                    item.AchNov = item.AchNexNov;
                    item.AchDec = item.AchNexDec;

                    item.AchNexJan = null;
                    item.AchNexFeb = null;
                    item.AchNexMar = null;
                    item.AchNexApr = null;
                    item.AchNexMay = null;
                    item.AchNexJun = null;
                    item.AchNexJul = null;
                    item.AchNexAug = null;
                    item.AchNexSep = null;
                    item.AchNexOct = null;
                    item.AchNexNov = null;
                    item.AchNexDec = null;
                }
               // ViewData["STARTYEAR"] = Convert.ToDateTime(item.StartMonth).Year;
            }

            return PartialView("_GrdMainInitiativePartial", model);
        }
        public static void ConsoleLog(string message)
        {
            string scriptTag = "<script type=\"\" language=\"\">console.clear(); {0}</script>";
            string function = "console.log('{0}');";
            string log = string.Format(string.Format(scriptTag, function), message.Replace("'", "\\'"));
            System.Web.HttpContext.Current.Response.Write(log);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMainInitiativePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_initiative item)
        {
            var model = db.t_initiative;

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
            return PartialView("_GrdMainInitiativePartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMainInitiativePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.vwheaderinitiative item)
        {
            var model = db.t_initiative;
            var modelItem = model.FirstOrDefault(it => it.id == item.id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (modelItem != null)
                    {
                        modelItem.InitNumber = item.InitNumber; modelItem.BrandID = item.BrandID; modelItem.RegionID = item.RegionID; modelItem.SubRegionID = item.SubRegionID; modelItem.ClusterID = item.ClusterID; modelItem.RegionalOfficeID = item.RegionalOfficeID;
                        modelItem.CostControlID = item.CostControlID; modelItem.LegalEntityID = item.LegalEntityID; modelItem.CountryID = item.CountryID; modelItem.SubCountryID = item.SubCountryID; modelItem.Confidential = item.Confidential;
                        modelItem.Description = item.Description; modelItem.ResponsibleFullName = item.ResponsibleFullName; modelItem.InitiativeType = item.InitiativeType; modelItem.CostCategoryID = item.CostCategoryID; modelItem.SubCostCategoryID = item.SubCostCategoryID;
                        modelItem.ActionTypeID = item.ActionTypeID; modelItem.SynergyImpactID = item.SynergyImpactID; modelItem.InitStatus = item.InitStatus; modelItem.isDeleted = item.isDeleted; modelItem.StartMonth = item.StartMonth; modelItem.EndMonth = item.EndMonth;
                        modelItem.LaraCode = item.LaraCode; modelItem.TargetTY = item.TargetTY; modelItem.TargetNY = item.TargetNY; modelItem.HOValidity = item.HOValidity; modelItem.RPOCControl = item.RPOCControl; modelItem.YTDTarget = item.YTDTarget; modelItem.YTDAchieved = item.YTDAchieved;
                        modelItem.TargetJan = item.TargetJan; modelItem.AchJan = item.AchJan; modelItem.TargetFeb = item.TargetFeb; modelItem.AchFeb = item.AchFeb; modelItem.TargetMar = item.TargetMar; modelItem.AchMar = item.AchMar; modelItem.TargetApr = item.TargetApr; modelItem.AchApr = item.AchApr;
                        modelItem.TargetMay = item.TargetMay; modelItem.AchMay = item.AchMay; modelItem.TargetJun = item.TargetJun; modelItem.AchJun = item.AchJun; modelItem.TargetJul = item.TargetJul; modelItem.AchJul = item.AchJul; modelItem.TargetAug = item.TargetAug; modelItem.AchAug = item.AchAug;
                        modelItem.TargetSep = item.TargetSep; modelItem.AchSep = item.AchSep; modelItem.TargetOct = item.TargetOct; modelItem.AchOct = item.AchOct; modelItem.TargetNov = item.TargetNov; modelItem.AchNov = item.AchNov; modelItem.TargetDec = item.TargetDec; modelItem.AchDec = item.AchDec;
                        modelItem.AgencyComment = item.AgencyComment; modelItem.RPOCComment = item.RPOCComment; modelItem.HOComment = item.HOComment; modelItem.AdditionalInfo = item.AdditionalInfo; modelItem.PortID = item.PortID; modelItem.VendorName = item.VendorName;
                        modelItem.UploadedFile = item.UploadedFile;

                        //this.UpdateModel(modelItem);
                        db.SaveChanges();
                        return RedirectToAction("", "ActiveInitiative");
                    }
                    else
                    {
                        return PartialView("_GrdMainInitiativePartial", model.ToList());
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    return PartialView("_GrdMainInitiativePartial", model.ToList());
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
                return PartialView("_GrdMainInitiativePartial", model.ToList());
            }
        }
        [HttpPost, ValidateInput(false)]
        //public ActionResult GrdMainInitiativePartialDelete(System.Int64 id)
        public ActionResult GrdMainInitiativePartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.t_initiative item)
        {
            var model = db.t_initiative;
            var model2 = db.vwheaderinitiatives.OrderByDescending(o => o.CreatedDate);

            if (item.id >= 0)
            {
                try
                {
                    var itemx = model.FirstOrDefault(it => it.id == item.id);
                    var item2 = model2.FirstOrDefault(it => it.id == item.id);

                    if (itemx != null)
                    {
                        //model.Remove(itemx);
                        itemx.InitStatus = 1;
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            ViewData["mregions"] = db.mregions.ToList();
            ViewData["brandname"] = db.mbrands.Where(c => c.isActive == "Y").ToList();
            ViewData["msubregion"] = db.msubregions.Where(c => c.SubRegionName != null && c.SubRegionName != "").ToList();
            ViewData["mcluster"] = db.mclusters.Where(c => c.ClusterName != "").GroupBy(g => g.ClusterName).Select(s => new { ClusterName = s.Key }).ToList();
            ViewData["mregional_office"] = db.mregional_office.SqlQuery("SELECT * FROM mregional_office").GroupBy(g => g.RegionalOffice_Name).Select(s => new { RegionalOffice_Name = s.Key }).ToList();
            ViewData["CostControlSiteName"] = db.mcostcontrolsites.Where(c => c.CostControlSiteName != "").ToList();
            ViewData["CountryName"] = db.mcountries.Where(c => c.CountryName != "").ToList();
            ViewData["SubCountryName"] = db.msubcountries.Where(c => c.SubCountryName != "" && c.isActive == "Y").ToList();
            ViewData["LegalEntityName"] = db.mlegalentities.GroupBy(g => g.LegalEntityName).Select(s => new { LegalEntityName = s.Key }).ToList();
            ViewData["SavingTypeName"] = db.msavingtypes.Where(c => c.isActive == "Y").ToList();
            ViewData["CostTypeName"] = db.mcosttypes.Where(c => c.isActive == "Y").ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(c => c.isActive == "Y").ToList();
            ViewData["ActionTypeName"] = db.mactiontypes.Where(c => c.isActive == "Y").ToList();
            ViewData["SynImpactName"] = db.msynimpacts.Where(c => c.isActive == "Y").ToList();
            ViewData["Status"] = db.mstatus.Where(c => c.isActive == "Y").ToList();
            ViewData["portName"] = db.mports.ToList();
            ViewData["SourceCategoryName"] = db.msourcecategories.ToList();
            return PartialView("_GrdMainInitiativePartial", model2.ToList());
        }
        public ActionResult GrdRegionalPartial()
        {
            int GetID = Convert.ToInt32(Session["RegionID"]);
            if (Session["RegionID"] != null)
            {
                var model = db.mregions.Where(c => c.id == GetID);
                return PartialView("_GrdRegionalPartial", model.ToList());
            }
            else
            {
                var model = db.mregions;
                return PartialView("_GrdRegionalPartial", model.ToList());
            }
        }
        public ActionResult GrdSubRegionPartial()
        {
            int GetID = Convert.ToInt32(Session["SubRegionID"]);
            if (Session["SubRegionID"] != null)
            {
                var model = db.msubregions.Where(c => c.id == GetID);
                return PartialView("_GrdSubRegionPartial", model.ToList());
            }
            else
            {
                var model = db.msubregions;
                return PartialView("_GrdSubRegionPartial", model.ToList());
            }
        }
        public ActionResult GrdInitStatusPartial()
        {
            int GetID = Convert.ToInt32(Session["StatusID"]);
            if (Session["StatusID"] != null)
            {
                var model = db.mstatus.Where(c => c.id == GetID);
                return PartialView("_GrdInitStatusPartial", model.ToList());
            }
            else
            {
                var model = db.mstatus;
                return PartialView("_GrdInitStatusPartial", model.ToList());
            }
        }
        public ActionResult GrdSubCountryPartial(Models.GetInfoByIDModel GetInfo)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession; var where = "";
            if (profileData.UserType == 3) //agency
            {
                //var cntrytext = profileData.CountryID.Replace("|", "','");
                var cntrytext = profileData.subcountry_right.Replace("|", "','");
                int lencntrytext = cntrytext.Length;
                cntrytext = "(" + cntrytext.Substring(2, (lencntrytext - 4)) + ")";
                var cntryid = db.msubcountries.SqlQuery("select id,CountryID,SubCountryName,CountryCode,isActive from msubcountry where SubCountryName is not null and isActive = 'Y' and SubCountryName in " + cntrytext + " ").ToList();
                var cntryidcondition = "(";
                for (var i = 0; i < cntryid.Count(); i++)
                {
                    cntryidcondition += cntryid[i].id.ToString() + ",";
                }
                cntryidcondition = cntryidcondition.Substring(0, cntryidcondition.Length - 1);

                //where += " and a.CountryID in " + cntryidcondition + ")";
                where += cntryidcondition + ")";
            }

            //db.Configuration.ProxyCreationEnabled = false;
            //var model = db.msubcountries.ToList();

            List<SubCountryList> model = new List<SubCountryList>();
            model = db.msubcountries.Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();

            if (GetInfo.Id != 0)
            {
                model = db.msubcountries.Where(c => c.id == GetInfo.Id && c.isActive == "Y" && !(string.IsNullOrEmpty(c.SubCountryName))).Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                //model = db.msubcountries.SqlQuery("select id,CountryID,SubCountryName,CountryCode,isActive from msubcountry where SubCountryName is not null and isActive = 'Y' and id = " + GetInfo.Id + " ").ToList();
            }
            else
            {
                if (profileData.UserType == 3) //agency
                {
                    //model = db.msubcountries.Where(c => !string.IsNullOrEmpty(c.SubCountryName) && c.CountryCode   && c.isActive == "Y").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                    model = db.msubcountries.SqlQuery("select id,CountryID,SubCountryName,CountryCode,isActive from msubcountry where SubCountryName is not null and isActive = 'Y' and id in " + where + " ").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                }
                else
                {
                    model = db.msubcountries.Where(c => !string.IsNullOrEmpty(c.SubCountryName) && c.isActive == "Y").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                    //model = db.msubcountries.SqlQuery("select id,CountryID,SubCountryName,CountryCode,isActive from msubcountry where SubCountryName is not null and isActive = 'Y';").ToList();
                }
            }

            List<GetDataFromSubCountry> GDSC = new List<GetDataFromSubCountry>();
            GDSC.Add(new GetDataFromSubCountry
            {
                SubCountryData = model
            });

            return Json(GDSC, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GrdBrandPartial()
        {
            int GetID = Convert.ToInt32(Session["BrandID"]);
            if (Session["BrandID"] != null)
            {
                var model = db.mbrands.Where(c => c.id == GetID);
                return PartialView("_GrdBrandPartial", model.ToList());
            }
            else
            {
                var model = db.mbrands;
                return PartialView("_GrdBrandPartial", model.ToList());
            }
        }
        public ActionResult GrdLegalEntityPartial()
        {
            int GetID = Convert.ToInt32(Session["LegalEntityID"]);
            if (Session["LegalEntityID"] != null)
            {
                var model = db.mlegalentities.Where(c => c.id == GetID);
                return PartialView("_GrdLegalEntityPartial", model.ToList());
            }
            else
            {
                var model = db.mlegalentities;
                return PartialView("_GrdLegalEntityPartial", model.ToList());
            }
        }
        public ActionResult GrdCountryPartial()
        {
            int GetID = Convert.ToInt32(Session["CountryID"]);
            if (Session["CountryID"] != null)
            {
                var model = db.mcountries.Where(c => c.id == GetID);
                return PartialView("_GrdCountryPartial", model.ToList());
            }
            else
            {
                var model = db.mcountries;
                return PartialView("_GrdCountryPartial", model.ToList());
            }
        }
        public ActionResult GrdClusterPartial()
        {
            int GetID = Convert.ToInt32(Session["ClusterID"]);
            if (Session["ClusterID"] != null)
            {
                var model = db.mclusters.Where(c => c.id == GetID);
                return PartialView("_GrdClusterPartial", model.ToList());
            }
            else
            {
                var model = db.mclusters;
                return PartialView("_GrdClusterPartial", model.ToList());
            }
        }
        public ActionResult GrdRegionalOfficePartial()
        {
            int GetID = Convert.ToInt32(Session["RegionalOfficeID"]);
            if (Session["RegionalOfficeID"] != null)
            {
                var model = db.mregional_office.Where(c => c.id == GetID);
                return PartialView("_GrdRegionalOfficePartial", model.ToList());
            }
            else
            {
                var model = db.mregional_office;
                return PartialView("_GrdRegionalOfficePartial", model.ToList());
            }
        }
        public ActionResult GrdCostControlPartial()
        {
            int GetID = Convert.ToInt32(Session["CostControlID"]);
            if (Session["CostControlID"] != null)
            {
                var model = db.mcostcontrolsites.Where(c => c.id == GetID);
                return PartialView("_GrdCostControlPartial", model.ToList());
            }
            else
            {
                var model = db.mcostcontrolsites;
                return PartialView("_GrdCostControlPartial", model.ToList());
            }
        }
        public ActionResult GrdInitTypePartial()
        {
            int GetID = Convert.ToInt32(Session["InitTypeID"]);
            if (Session["InitTypeID"] != null)
            {
                var model = db.msavingtypes.Where(c => c.id == GetID);
                return PartialView("_GrdInitTypePartial", model.ToList());
            }
            else
            {
                var model = db.msavingtypes;
                return PartialView("_GrdInitTypePartial", model.ToList());
            }
        }
        public ActionResult GrdInitCategoryPartial()
        {
            int GetID = Convert.ToInt32(Session["CostCategoryID"]);
            if (Session["CostCategoryID"] != null)
            {
                var model = db.mcosttypes.Where(c => c.id == GetID);
                return PartialView("_GrdInitCategoryPartial", model.ToList());
            }
            else
            {
                var model = db.mcosttypes;
                return PartialView("_GrdInitCategoryPartial", model.ToList());
            }
        }
        public ActionResult GrdSubCostPartial()
        {
            int GetID = Convert.ToInt32(Session["SubCostCategoryID"]);
            if (Session["SubCostCategoryID"] != null)
            {
                var model = db.msubcosts.Where(c => c.id == GetID);
                return PartialView("_GrdSubCostPartial", model.ToList());
            }
            else
            {
                var model = db.msubcosts;
                return PartialView("_GrdSubCostPartial", model.ToList());
            }
        }
        public ActionResult GrdActionTypePartial()
        {
            int GetID = Convert.ToInt32(Session["ActionTypeID"]);
            if (Session["ActionTypeID"] != null)
            {
                var model = db.mactiontypes.Where(c => c.id == GetID);
                return PartialView("_GrdActionTypePartial", model.ToList());
            }
            else
            {
                var model = db.mactiontypes;
                return PartialView("_GrdActionTypePartial", model.ToList());
            }
        }
        public ActionResult GrdSynImpactPartial()
        {
            int GetID = Convert.ToInt32(Session["SynergyImpactID"]);
            if (Session["SynergyImpactID"] != null)
            {
                var model = db.msynimpacts.Where(c => c.id == GetID);
                return PartialView("_GrdSynImpactPartial", model.ToList());
            }
            else
            {
                var model = db.msynimpacts;
                return PartialView("_GrdSynImpactPartial", model.ToList());
            }
        }
        [HttpPost]
        public ActionResult SaveNew(Models.InitiativeModel NewInitiative)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            string KodeNegara = profileData.CountryCode;
            string UserID = profileData.ID;
            Int64 FormID = NewInitiative.FormID;
            string FormStatus = NewInitiative.FormStatus;
            long GrdSubCountry = NewInitiative.GrdSubCountry;
            msubcountry SubNegara = db.msubcountries.SqlQuery("SELECT * FROM msubcountry a WHERE a.id = " + GrdSubCountry + "; ").FirstOrDefault();
            KodeNegara = SubNegara.CountryCode;

            long GrdBrand = NewInitiative.GrdBrand;
            long GrdLegalEntity = NewInitiative.GrdLegalEntity;
            long GrdCountry = NewInitiative.GrdCountry;
            long GrdRegional = NewInitiative.GrdRegional;
            long GrdSubRegion = NewInitiative.GrdSubRegion;
            long GrdCluster = NewInitiative.GrdCluster;
            long GrdRegionalOffice = NewInitiative.GrdRegionalOffice;
            long GrdCostControl = NewInitiative.GrdCostControl;
            string CboConfidential = NewInitiative.CboConfidential;
            string TxResponsibleName = NewInitiative.TxResponsibleName;
            string TxDesc = NewInitiative.TxDesc;
            long GrdInitStatus = NewInitiative.GrdInitStatus;
            string TxLaraCode = NewInitiative.TxLaraCode;
            Int64 TxPortName = NewInitiative.TxPortName;
            string TxVendorSupp = NewInitiative.TxVendorSupp;
            string TxAdditionalInfo = NewInitiative.TxAdditionalInfo;
            long GrdInitType = NewInitiative.GrdInitType;
            long GrdInitCategory = NewInitiative.GrdInitCategory;
            long GrdSubCost = NewInitiative.GrdSubCost;
            long GrdActionType = NewInitiative.GrdActionType;
            long GrdSynImpact = NewInitiative.GrdSynImpact;
            DateTime StartMonth = NewInitiative.StartMonth;
            DateTime EndMonth = NewInitiative.EndMonth;
            string TxAgency = NewInitiative.TxAgency;
            string TxRPOCComment = NewInitiative.TxRPOCComment;
            string TxHOComment = NewInitiative.TxHOComment;
            string CboHoValidity = NewInitiative.CboHoValidity;
            string CboRPOCValidity = NewInitiative.CboRPOCValidity;
            Decimal TxTarget12 = NewInitiative.TxTarget12;
            Decimal TxTargetFullYear = NewInitiative.TxTargetFullYear;
            Decimal TxYTDTargetFullYear = NewInitiative.TxYTDTargetFullYear;
            Decimal TxYTDSavingFullYear = NewInitiative.TxYTDSavingFullYear;
            Int64 ProjectYear = NewInitiative.ProjectYear;
            string RelatedInitiative = NewInitiative.RelatedInitiative;
            long? SourceCategory = NewInitiative.SourceCategory;
            SourceCategory = (long?)(SourceCategory == 0 ? (long?)null : SourceCategory);

            decimal? targetjan = NewInitiative.targetjan; decimal? targetjan2 = NewInitiative.targetjan2;
            decimal? targetfeb = NewInitiative.targetfeb; decimal? targetfeb2 = NewInitiative.targetfeb2;
            decimal? targetmar = NewInitiative.targetmar; decimal? targetmar2 = NewInitiative.targetmar2;
            decimal? targetapr = NewInitiative.targetapr; decimal? targetapr2 = NewInitiative.targetapr2;
            decimal? targetmay = NewInitiative.targetmay; decimal? targetmay2 = NewInitiative.targetmay2;
            decimal? targetjun = NewInitiative.targetjun; decimal? targetjun2 = NewInitiative.targetjun2;
            decimal? targetjul = NewInitiative.targetjul; decimal? targetjul2 = NewInitiative.targetjul2;
            decimal? targetaug = NewInitiative.targetaug; decimal? targetaug2 = NewInitiative.targetaug2;
            decimal? targetsep = NewInitiative.targetsep; decimal? targetsep2 = NewInitiative.targetsep2;
            decimal? targetoct = NewInitiative.targetoct; decimal? targetoct2 = NewInitiative.targetoct2;
            decimal? targetnov = NewInitiative.targetnov; decimal? targetnov2 = NewInitiative.targetnov2;
            decimal? targetdec = NewInitiative.targetdec; decimal? targetdec2 = NewInitiative.targetdec2;

            decimal? savingjan = NewInitiative.savingjan; decimal? savingjan2 = NewInitiative.savingjan2;
            decimal? savingfeb = NewInitiative.savingfeb; decimal? savingfeb2 = NewInitiative.savingfeb2;
            decimal? savingmar = NewInitiative.savingmar; decimal? savingmar2 = NewInitiative.savingmar2;
            decimal? savingapr = NewInitiative.savingapr; decimal? savingapr2 = NewInitiative.savingapr2;
            decimal? savingmay = NewInitiative.savingmay; decimal? savingmay2 = NewInitiative.savingmay2;
            decimal? savingjun = NewInitiative.savingjun; decimal? savingjun2 = NewInitiative.savingjun2;
            decimal? savingjul = NewInitiative.savingjul; decimal? savingjul2 = NewInitiative.savingjul2;
            decimal? savingaug = NewInitiative.savingaug; decimal? savingaug2 = NewInitiative.savingaug2;
            decimal? savingsep = NewInitiative.savingsep; decimal? savingsep2 = NewInitiative.savingsep2;
            decimal? savingoct = NewInitiative.savingoct; decimal? savingoct2 = NewInitiative.savingoct2;
            decimal? savingnov = NewInitiative.savingnov; decimal? savingnov2 = NewInitiative.savingnov2;
            decimal? savingdec = NewInitiative.savingdec; decimal? savingdec2 = NewInitiative.savingdec2;

            if (FormStatus == "New")
            {
                try
                {
                    using (GainEntities db = new GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"])))
                    {
                        var tinitiative = db.Set<t_initiative>();
                        var YearInitiative = profileData.ProjectYear;
                        Random rand = new Random();
                        //int InitNextNum = rand.Next(1,999);
                        //string OutInitNextNum = "00" + InitNextNum.ToString();
                        string nomerterakhir = "";
                        t_initiative initz = db.t_initiative.Where(c => c.InitNumber.StartsWith(YearInitiative + KodeNegara)).FirstOrDefault();
                        if (initz != null)
                            nomerterakhir = db.t_initiative.Where(c => c.InitNumber.StartsWith(YearInitiative + KodeNegara) && c.CountryID == GrdCountry && c.SubCountryID == GrdSubCountry).OrderByDescending(o => o.InitNumber).FirstOrDefault().InitNumber;
                        else
                            nomerterakhir = YearInitiative + KodeNegara + "000";

                        nomerterakhir = nomerterakhir.Substring((nomerterakhir.Length - 3), 3);
                        int nomerurut = (Int32.Parse(nomerterakhir) + 1);
                        string nomerselanjutnya = ("00" + nomerurut.ToString());
                        nomerselanjutnya = nomerselanjutnya.Substring((nomerselanjutnya.Length - 3), 3);

                        tinitiative.Add(new t_initiative
                        {
                            InitNumber = YearInitiative + KodeNegara + nomerselanjutnya,
                            RelatedInitiative = RelatedInitiative,
                            SourceCategory = SourceCategory,
                            BrandID = GrdBrand,
                            RegionID = GrdRegional,
                            SubRegionID = GrdSubRegion,
                            ClusterID = GrdCluster,
                            RegionalOfficeID = GrdRegionalOffice,
                            CostControlID = GrdCostControl,
                            LegalEntityID = GrdLegalEntity,
                            CountryID = GrdCountry,
                            SubCountryID = GrdSubCountry,
                            Confidential = CboConfidential,
                            Description = TxDesc,
                            ResponsibleFullName = TxResponsibleName,
                            InitiativeType = GrdInitType,
                            CostCategoryID = GrdInitCategory,
                            SubCostCategoryID = GrdSubCost,
                            ActionTypeID = GrdActionType,
                            SynergyImpactID = GrdSynImpact,
                            InitStatus = GrdInitStatus,
                            isDeleted = 0,
                            StartMonth = StartMonth,
                            EndMonth = EndMonth,
                            LaraCode = TxLaraCode,
                            TargetTY = TxTarget12,
                            TargetNY = TxTargetFullYear,
                            HOValidity = CboHoValidity,
                            RPOCControl = CboRPOCValidity,
                            YTDTarget = TxYTDTargetFullYear,
                            YTDAchieved = TxYTDSavingFullYear,
                            AgencyComment = TxAgency,
                            RPOCComment = TxRPOCComment,
                            HOComment = TxHOComment,
                            AdditionalInfo = TxAdditionalInfo,
                            PortID = (TxPortName == 0 ? 1 : TxPortName),
                            ProjectYear = (short)ProjectYear,
                            VendorName = TxVendorSupp,
                            TargetJan = targetjan,
                            TargetFeb = targetfeb,
                            TargetMar = targetmar,
                            TargetApr = targetapr,
                            TargetMay = targetmay,
                            TargetJun = targetjun,
                            TargetJul = targetjul,
                            TargetAug = targetaug,
                            TargetSep = targetsep,
                            TargetOct = targetoct,
                            TargetNov = targetnov,
                            TargetDec = targetdec,
                            TargetNexJan = targetjan2,
                            TargetNexFeb = targetfeb2,
                            TargetNexMar = targetmar2,
                            TargetNexApr = targetapr2,
                            TargetNexMay = targetmay2,
                            TargetNexJun = targetjun2,
                            TargetNexJul = targetjul2,
                            TargetNexAug = targetaug2,
                            TargetNexSep = targetsep2,
                            TargetNexOct = targetoct2,
                            TargetNexNov = targetnov2,
                            TargetNexDec = targetdec2,
                            AchJan = savingjan,
                            AchFeb = savingfeb,
                            AchMar = savingmar,
                            AchApr = savingapr,
                            AchMay = savingmay,
                            AchJun = savingjun,
                            AchJul = savingjul,
                            AchAug = savingaug,
                            AchSep = savingsep,
                            AchOct = savingoct,
                            AchNov = savingnov,
                            AchDec = savingdec,
                            AchNexJan = savingjan2,
                            AchNexFeb = savingfeb2,
                            AchNexMar = savingmar2,
                            AchNexApr = savingapr2,
                            AchNexMay = savingmay2,
                            AchNexJun = savingjun2,
                            AchNexJul = savingjul2,
                            AchNexAug = savingaug2,
                            AchNexSep = savingsep2,
                            AchNexOct = savingoct2,
                            AchNexNov = savingnov2,
                            AchNexDec = savingdec2,
                            CreatedDate = DateTime.Now,
                            CreatedBy = UserID,
                            ModifiedBy = UserID
                        });
                        db.SaveChanges();
                        return Content("saved|" + YearInitiative + KodeNegara + nomerselanjutnya);
                    }
                }
                catch(Exception E)
                {
                    log.Error("Exception occured in saving new initiative" + E.Message);
                    return Content("Error occired during Initiative save");
                }
            }
            else
            {
                try
                {
                    var initdata = db.t_initiative.Where(x => x.id == FormID).FirstOrDefault();
                    initdata.RelatedInitiative = RelatedInitiative;
                    initdata.SourceCategory = SourceCategory;
                    initdata.BrandID = GrdBrand;
                    initdata.RegionID = GrdRegional;
                    initdata.SubRegionID = GrdSubRegion;
                    initdata.ClusterID = GrdCluster;
                    initdata.RegionalOfficeID = GrdRegionalOffice;
                    initdata.CostControlID = GrdCostControl;
                    initdata.LegalEntityID = GrdLegalEntity;
                    initdata.CountryID = GrdCountry;
                    initdata.SubCountryID = GrdSubCountry;
                    initdata.Confidential = CboConfidential;
                    initdata.Description = TxDesc;
                    initdata.ResponsibleFullName = TxResponsibleName;
                    initdata.InitiativeType = GrdInitType;
                    initdata.CostCategoryID = GrdInitCategory;
                    initdata.SubCostCategoryID = GrdSubCost;
                    initdata.ActionTypeID = GrdActionType;
                    initdata.SynergyImpactID = GrdSynImpact;
                    initdata.InitStatus = GrdInitStatus;
                    initdata.StartMonth = StartMonth;
                    initdata.EndMonth = EndMonth;
                    initdata.LaraCode = TxLaraCode;
                    initdata.TargetTY = TxTarget12;
                    initdata.TargetNY = TxTargetFullYear;
                    initdata.HOValidity = CboHoValidity;
                    initdata.RPOCControl = CboRPOCValidity;
                    initdata.YTDTarget = TxYTDTargetFullYear;
                    initdata.YTDAchieved = TxYTDSavingFullYear;
                    initdata.AgencyComment = TxAgency;
                    initdata.RPOCComment = TxRPOCComment;
                    initdata.HOComment = TxHOComment;
                    initdata.AdditionalInfo = TxAdditionalInfo;
                    initdata.PortID = (TxPortName == 0 ? 1 : TxPortName);
                    initdata.VendorName = TxVendorSupp;
                    //Manipulate if this is previous year initiative 
                    //if (StartMonth.Year == ProjectYear)
                    //{
                        initdata.TargetJan = targetjan; initdata.TargetFeb = targetfeb; initdata.TargetMar = targetmar; initdata.TargetApr = targetapr; initdata.TargetMay = targetmay; initdata.TargetJun = targetjun;
                        initdata.TargetJul = targetjul; initdata.TargetAug = targetaug; initdata.TargetSep = targetsep; initdata.TargetOct = targetoct; initdata.TargetNov = targetnov; initdata.TargetDec = targetdec;
                        initdata.TargetNexJan = targetjan2; initdata.TargetNexFeb = targetfeb2; initdata.TargetNexMar = targetmar2; initdata.TargetNexApr = targetapr2; initdata.TargetNexMay = targetmay2; initdata.TargetNexJun = targetjun2;
                        initdata.TargetNexJul = targetjul2; initdata.TargetNexAug = targetaug2; initdata.TargetNexSep = targetsep2; initdata.TargetNexOct = targetoct2; initdata.TargetNexNov = targetnov2; initdata.TargetNexDec = targetdec2;

                        initdata.AchJan = savingjan; initdata.AchFeb = savingfeb; initdata.AchMar = savingmar; initdata.AchApr = savingapr; initdata.AchMay = savingmay; initdata.AchJun = savingjun;
                        initdata.AchJul = savingjul; initdata.AchAug = savingaug; initdata.AchSep = savingsep; initdata.AchOct = savingoct; initdata.AchNov = savingnov; initdata.AchDec = savingdec;
                        initdata.AchNexJan = savingjan2; initdata.AchNexFeb = savingfeb2; initdata.AchNexMar = savingmar2; initdata.AchNexApr = savingapr2; initdata.AchNexMay = savingmay2; initdata.AchNexJun = savingjun2;
                        initdata.AchNexJul = savingjul2; initdata.AchNexAug = savingaug2; initdata.AchNexSep = savingsep2; initdata.AchNexOct = savingoct2; initdata.AchNexNov = savingnov2; initdata.AchNexDec = savingdec2;
                   // }
                    //else
                    //{
                        //Capture the records only for the next year 
                        //initdata.TargetNexJan = targetjan2; initdata.TargetNexFeb = targetfeb2; initdata.TargetNexMar = targetmar2; initdata.TargetNexApr = targetapr2; initdata.TargetNexMay = targetmay2; initdata.TargetNexJun = targetjun2;
                        //initdata.TargetNexJul = targetjul2; initdata.TargetNexAug = targetaug2; initdata.TargetNexSep = targetsep2; initdata.TargetNexOct = targetoct2; initdata.TargetNexNov = targetnov2; initdata.TargetNexDec = targetdec2;

                        //initdata.AchNexJan = savingjan2; initdata.AchNexFeb = savingfeb2; initdata.AchNexMar = savingmar2; initdata.AchNexApr = savingapr2; initdata.AchNexMay = savingmay2; initdata.AchNexJun = savingjun2;
                        //initdata.AchNexJul = savingjul2; initdata.AchNexAug = savingaug2; initdata.AchNexSep = savingsep2; initdata.AchNexOct = savingoct2; initdata.AchNexNov = savingnov2; initdata.AchNexDec = savingdec2;


                    //}

                    //initdata.ModifiedDate = DateTime.Now;
                    initdata.CreatedBy = initdata.CreatedBy;// (initdata.CreatedBy == null ? UserID : initdata.CreatedBy);
                    initdata.ModifiedBy = UserID;

                    //initdata.CreatedDate = DateTime.Now;
                    db.SaveChanges();
                    return Content("saved|" + initdata.InitNumber);
                }
                catch(Exception E)
                {
                    log.Error("Exception occured during Saving exisisting initiative" + E.Message);
                    return Content("Error ocuured during initiativeSave");
                }
            }
        }
        [HttpPost]
        public ActionResult GetInfoById(Models.GetInfoByIDModel GetInfo)
        {
            //var model = db.t_initiative.Where(c => c.id == GetInfo.Id).FirstOrDefault();
            //var modeldetail = db.t_initative_detail.Where(c => c.id == model.id).FirstOrDefault();
            //var InitiativeNumber = model.InitNumber.ToString();
            //var SubCountryID = model.SubCountryID.ToString();
            //var BrandID = model.BrandID.ToString();
            //var LegalEntityID = model.LegalEntityID.ToString();
            //var CountryID = model.CountryID.ToString();
            //var RegionID = model.RegionID.ToString();
            //var SubRegionID = model.SubRegionID.ToString();
            //var ClusterID = (model.ClusterID != null ? model.ClusterID : 0);
            //var RegionalOfficeID = model.RegionalOfficeID.ToString();
            //var CostControlID = model.CostControlID.ToString();
            //var InitTypeID = model.InitiativeType.ToString();
            //var CostCategoryID = model.CostCategoryID.ToString();
            //var SubCostCategoryID = model.SubCostCategoryID.ToString();
            //var ActionTypeID = model.ActionTypeID.ToString();
            //var SynergyImpactID = model.SynergyImpactID.ToString();
            //Session["GetInfoByID"] = SubCountryID;
            //Session["BrandID"] = BrandID;
            //Session["LegalEntityID"] = LegalEntityID;
            //Session["CountryID"] = CountryID;
            //Session["RegionID"] = RegionID;
            //Session["SubRegionID"] = SubRegionID;
            //Session["ClusterID"] = ClusterID;
            //Session["RegionalOfficeID"] = RegionalOfficeID;
            //Session["CostControlID"] = CostControlID;
            //Session["InitTypeID"] = InitTypeID;
            //Session["CostCategoryID"] = CostCategoryID;
            //Session["SubCostCategoryID"] = SubCostCategoryID;
            //Session["ActionTypeID"] = ActionTypeID;
            //Session["SynergyImpactID"] = SynergyImpactID;
            try
            {
                var model = db.vwheaderinitiatives.Where(c => c.id == GetInfo.Id).FirstOrDefault();
                //var modeldetail = db.t_initative_detail.Where(c => c.id == model.id).FirstOrDefault();
                var InitiativeNumber = model.InitNumber.ToString();
                var SubCountryID = model.SubCountryID.ToString();
                var BrandID = model.BrandID.ToString();
                //var LegalEntityID = model.LegalEntityID.ToString() + ";" + model.LegalEntityName.ToString();
                var StatusID = model.InitStatus.ToString();
                var LegalEntityID = model.LegalEntityID.ToString();
                var CountryID = model.CountryID.ToString();
                var RegionID = model.RegionID.ToString();
                var SubRegionID = model.SubRegionID.ToString();
                var ClusterID = (model.ClusterID != null ? model.ClusterID : 0);
                var RegionalOfficeID = model.RegionalOfficeID.ToString();
                var CostControlID = model.CostControlID.ToString();
                var InitTypeID = model.InitiativeType.ToString();
                var CostCategoryID = model.CostCategoryID.ToString();
                var SubCostCategoryID = model.SubCostCategoryID.ToString();
                var ActionTypeID = model.ActionTypeID.ToString();
                var SynergyImpactID = model.SynergyImpactID.ToString();
                Session["GetInfoByID"] = SubCountryID;
                Session["BrandID"] = BrandID;
                Session["StatusID"] = StatusID;
                Session["LegalEntityID"] = LegalEntityID;
                Session["CountryID"] = CountryID;
                Session["RegionID"] = RegionID;
                Session["SubRegionID"] = SubRegionID;
                Session["ClusterID"] = ClusterID;
                Session["RegionalOfficeID"] = RegionalOfficeID;
                Session["CostControlID"] = CostControlID;
                Session["InitTypeID"] = InitTypeID;
                Session["CostCategoryID"] = CostCategoryID;
                Session["SubCostCategoryID"] = SubCostCategoryID;
                Session["ActionTypeID"] = ActionTypeID;
                Session["SynergyImpactID"] = SynergyImpactID;

                return Content(JsonConvert.SerializeObject(model));
            }
            catch(Exception e)
            {
                log.Error("Exception occured during GetInfobyID" + e.Message);
                return Content("Error");
            }
        }
        [HttpPost]
        public ActionResult RemoveSelectedGridLookup()
        {
            Session.Remove("GetInfoByID");
            Session.Remove("BrandID");
            Session.Remove("StatusID");
            Session.Remove("LegalEntityID");
            Session.Remove("CountryID");
            Session.Remove("RegionID");
            Session.Remove("SubRegionID");
            Session.Remove("ClusterID");
            Session.Remove("RegionalOfficeID");
            Session.Remove("CostControlID");
            Session.Remove("InitTypeID");
            Session.Remove("CostCategoryID");
            Session.Remove("SubCostCategoryID");
            Session.Remove("ActionTypeID");
            Session.Remove("SynergyImpactID");
            return Content("ok");
        }
        public ActionResult OnSubCountryChanged(Models.GetInfoByIDModel GetInfo)
        {
            var model = db.mbrands.Where(c => c.id == GetInfo.Id).FirstOrDefault();
            return Content("OnSubCountryChanged: Ok");
        }
        public ActionResult GetCountryBySub(Models.GetInfoByIDModel GetInfo)
        {
            msubcountry modelsubcountry = new msubcountry();
            mcountry modelcountry = new mcountry();
            mregion modelregion = new mregion();
            t_initiative modelinitiative = new t_initiative();

            if (GetInfo.Id != 0)
                modelsubcountry = db.msubcountries.Where(sc => sc.id == GetInfo.Id).FirstOrDefault();
            else
                modelsubcountry = db.msubcountries.FirstOrDefault();

            if (GetInfo.Id2 == 0)
            {
                modelinitiative = db.t_initiative.Where(init => init.SubCountryID == GetInfo.Id).FirstOrDefault();
            }
            else
            {
                modelinitiative = db.t_initiative.Where(init => init.id == GetInfo.Id2).FirstOrDefault();
            }

            var brandcountry = new mbrandcountry();
            Int64 CountryID = (long)modelsubcountry.CountryID;
            Int64 SubCountryID = (long)modelsubcountry.id;
            Int64 BrandIDx = 0;
            if (modelinitiative != null)
            {
                if (modelinitiative.BrandID.HasValue)
                {
                    BrandIDx = (long)modelinitiative.BrandID;
                }
                else
                {
                    brandcountry = db.mbrandcountries.Where(c => c.subcountryid == SubCountryID).FirstOrDefault();
                    BrandIDx = brandcountry.brandid;
                }
            }
            else
            {
                brandcountry = db.mbrandcountries.Where(c => c.subcountryid == SubCountryID).FirstOrDefault();
                BrandIDx = brandcountry.brandid;
            }

            modelcountry = db.mcountries.Where(c => c.id == CountryID).FirstOrDefault();
            Int64 RegionID = (long)modelcountry.RegionID;
            Int64 SubRegionID = (long)modelcountry.SubRegionID;

            //List<GeneralEntity> GeneralList = new List<GeneralEntity>();
            //GeneralList = db.mcountries.Select(s => new GeneralEntity { id = s.id, def = s.CountryName }).ToList();

            List<GetDataFromSubCountry> GDSC = new List<GetDataFromSubCountry>();
            List<SubCountryList> SubCountryList = new List<SubCountryList>();
            List<CountryList> CountryList = new List<CountryList>();
            List<BrandList> BrandList = new List<BrandList>();
            List<RegionList> RegionList = new List<RegionList>();
            List<SubRegionList> SubRegionList = new List<SubRegionList>();
            List<ClusterList> ClusterList = new List<ClusterList>();
            List<RegionalOfficeList> RegionalOfficeList = new List<RegionalOfficeList>();
            List<CostControlList> CostControlList = new List<CostControlList>();
            List<LegalEntityList> LegalEntityList = new List<LegalEntityList>();
            List<TypeInitiativeList> TypeInitiativeList = new List<TypeInitiativeList>();

            db.Configuration.ProxyCreationEnabled = false;
            SubCountryList = db.msubcountries.Where(c => c.id == SubCountryID && c.isActive == "Y").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
            CountryList = db.mcountries.Where(c => c.id == CountryID).Select(s => new CountryList { id = s.id, CountryName = s.CountryName }).ToList();
            if (GetInfo.Id2 == 0)
            {
                BrandList = db.mbrands.SqlQuery("SELECT a.id,a.brandname,c.CountryName,d.SubCountryName,a.isActive FROM mbrand a LEFT JOIN mbrandcountry b ON a.id = b.brandid LEFT JOIN mcountry c ON b.countryid=c.id LEFT JOIN msubcountry d ON d.CountryID = c.id AND d.id = b.subcountryid WHERE a.isActive = 'Y' and c.id = " + CountryID + " AND d.id = " + SubCountryID + " ORDER BY c.CountryName,d.SubCountryName asc").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
            }
            else
            {
                BrandList = db.mbrands.Where(c => c.id == modelinitiative.BrandID && c.isActive == "Y").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
            }
            RegionList = db.mregions.Where(c => c.id == RegionID).Select(s => new RegionList { id = s.id, RegionName = s.RegionName }).ToList();
            SubRegionList = db.msubregions.Where(c => c.id == SubRegionID).Select(s => new SubRegionList { id = s.id, SubRegionName = s.SubRegionName }).ToList();
            ClusterList = db.mclusters.Where(cl => cl.CountryID == CountryID && cl.RegionID == RegionID && cl.SubRegionID == SubRegionID && cl.ClusterName != "").Select(s => new ClusterList { id = s.id, ClusterName = s.ClusterName }).ToList();
            RegionalOfficeList = db.mregional_office.Where(ro => ro.RegionID == RegionID && ro.CountryID == CountryID).Select(s => new RegionalOfficeList { id = s.id, RegionalOfficeName = s.RegionalOffice_Name }).ToList();
            var costcontrolid = db.t_subctry_costcntrlsite.Where(sc => sc.subcountryid == SubCountryID).FirstOrDefault().costcontrolid;
            CostControlList = db.mcostcontrolsites.Where(c => c.id == costcontrolid).Select(s => new CostControlList { id = s.id, CostControlSiteName = s.CostControlSiteName }).ToList();
            LegalEntityList = db.mlegalentities.Where(le => le.CountryID == CountryID && le.BrandID == BrandIDx && le.SubCountryID == SubCountryID && le.CostControlSiteID == costcontrolid).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
            //LegalEntityList = db.mlegalentities.Where(le => le.CountryID == CountryID).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
            if (modelinitiative != null)
            {
                TypeInitiativeList = db.msavingtypes.Where(st => st.id == modelinitiative.InitiativeType).Select(s => new TypeInitiativeList { id = s.id, SavingTypeName = s.SavingTypeName }).ToList();
            }
            else
            {
                TypeInitiativeList = db.msavingtypes.Select(s => new TypeInitiativeList { id = s.id, SavingTypeName = s.SavingTypeName }).ToList();
            }

            if (GetInfo.Id > 0)
            {
                GDSC.Add(new GetDataFromSubCountry
                {
                    SubCountryData = SubCountryList,
                    CountryData = CountryList,
                    BrandData = BrandList,
                    RegionData = RegionList,
                    SubRegionData = SubRegionList,
                    ClusterData = ClusterList,
                    RegionalOfficeData = RegionalOfficeList,
                    CostControlSiteData = CostControlList,
                    LegalEntityData = LegalEntityList,
                    TypeInitiativeData = TypeInitiativeList
                });

            }
            else
            {
                GDSC.Add(new GetDataFromSubCountry
                {
                    SubCountryData = null,
                    CountryData = null,
                    BrandData = null,
                    RegionData = null,
                    SubRegionData = null,
                    ClusterData = null,
                    RegionalOfficeData = null,
                    CostControlSiteData = null,
                    LegalEntityData = null,
                    TypeInitiativeData = null
                });

            }

            return Json(GDSC, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCountryBySub2(Models.GetInfoByIDModel GetInfo)
        {
            msubcountry modelsubcountry = new msubcountry();
            mcountry modelcountry = new mcountry();
            mregion modelregion = new mregion();
            t_initiative modelinitiative = new t_initiative();

            if (GetInfo.Id != 0)
                modelsubcountry = db.msubcountries.Where(sc => sc.id == GetInfo.Id).FirstOrDefault();
            else
                modelsubcountry = db.msubcountries.FirstOrDefault();

            modelinitiative = db.t_initiative.Where(init => init.id == GetInfo.Id).FirstOrDefault();

            Int64 CountryID = (long)modelsubcountry.CountryID;
            Int64 SubCountryID = (long)modelsubcountry.id;
            Int64 BrandIDx = (long)modelinitiative.BrandID;
            modelcountry = db.mcountries.Where(c => c.id == CountryID).FirstOrDefault();
            Int64 RegionID = (long)modelcountry.RegionID;
            Int64 SubRegionID = (long)modelcountry.SubRegionID;

            //List<GeneralEntity> GeneralList = new List<GeneralEntity>();
            //GeneralList = db.mcountries.Select(s => new GeneralEntity { id = s.id, def = s.CountryName }).ToList();

            List<GetDataFromSubCountry> GDSC = new List<GetDataFromSubCountry>();
            List<SubCountryList> SubCountryList = new List<SubCountryList>();
            List<CountryList> CountryList = new List<CountryList>();
            List<BrandList> BrandList = new List<BrandList>();
            List<RegionList> RegionList = new List<RegionList>();
            List<SubRegionList> SubRegionList = new List<SubRegionList>();
            List<ClusterList> ClusterList = new List<ClusterList>();
            List<RegionalOfficeList> RegionalOfficeList = new List<RegionalOfficeList>();
            List<CostControlList> CostControlList = new List<CostControlList>();
            List<LegalEntityList> LegalEntityList = new List<LegalEntityList>();
            List<TypeInitiativeList> TypeInitiativeList = new List<TypeInitiativeList>();

            db.Configuration.ProxyCreationEnabled = false;
            SubCountryList = db.msubcountries.Where(c => c.id == SubCountryID).Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
            CountryList = db.mcountries.Where(c => c.id == CountryID).Select(s => new CountryList { id = s.id, CountryName = s.CountryName }).ToList();
            BrandList = db.mbrands.SqlQuery("SELECT a.id,a.brandname,c.CountryName,d.SubCountryName FROM mbrand a LEFT JOIN mbrandcountry b ON a.id = b.brandid LEFT JOIN mcountry c ON b.countryid=c.id LEFT JOIN msubcountry d ON d.CountryID = c.id WHERE c.id = " + CountryID + " AND d.id = " + SubCountryID + " ORDER BY c.CountryName,d.SubCountryName asc").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
            RegionList = db.mregions.Where(c => c.id == RegionID).Select(s => new RegionList { id = s.id, RegionName = s.RegionName }).ToList();
            SubRegionList = db.msubregions.Where(c => c.id == SubRegionID).Select(s => new SubRegionList { id = s.id, SubRegionName = s.SubRegionName }).ToList();
            ClusterList = db.mclusters.Where(cl => cl.CountryID == CountryID && cl.RegionID == RegionID && cl.SubRegionID == SubRegionID && cl.ClusterName != "").Select(s => new ClusterList { id = s.id, ClusterName = s.ClusterName }).ToList();
            RegionalOfficeList = db.mregional_office.Where(ro => ro.RegionID == RegionID && ro.CountryID == CountryID).Select(s => new RegionalOfficeList { id = s.id, RegionalOfficeName = s.RegionalOffice_Name }).ToList();
            CostControlList = db.mcostcontrolsites.Select(s => new CostControlList { id = s.id, CostControlSiteName = s.CostControlSiteName }).ToList();
            LegalEntityList = db.mlegalentities.Where(le => le.CountryID == CountryID && le.BrandID == BrandIDx).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
            TypeInitiativeList = db.msavingtypes.Where(st => st.id == modelinitiative.InitiativeType).Select(s => new TypeInitiativeList { id = s.id, SavingTypeName = s.SavingTypeName }).ToList();

            GDSC.Add(new GetDataFromSubCountry
            {
                SubCountryData = SubCountryList,
                CountryData = CountryList,
                BrandData = BrandList,
                RegionData = RegionList,
                SubRegionData = SubRegionList,
                ClusterData = ClusterList,
                RegionalOfficeData = RegionalOfficeList,
                CostControlSiteData = CostControlList,
                LegalEntityData = LegalEntityList,
                TypeInitiativeData = TypeInitiativeList
            });

            return Json(GDSC, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLegalFromBrand(Models.GetInfoByBrandIDModel GetInfo)
        {
            List<LegalEntityList> LegalEntity = new List<LegalEntityList>();
            LegalEntity = db.mlegalentities.Where(c => c.BrandID == GetInfo.BrandID && c.CountryID == GetInfo.CountryID && c.SubCountryID == GetInfo.SubCountryID && c.CostControlSiteID == GetInfo.CostControlSiteID).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
            List<GetDataFromSubCountry> GDSC = new List<GetDataFromSubCountry>();
            GDSC.Add(new GetDataFromSubCountry
            {
                LegalEntityData = LegalEntity
            });
            return Json(GDSC, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemFromInitiativeType(Models.GetInfoByIDModel GetInfo)
        {
            long SCID = GetInfo.Id;
            List<GetItemCategoryDataFromInitiative> GDFI = new List<GetItemCategoryDataFromInitiative>();

            db.Configuration.ProxyCreationEnabled = false;
            GDFI.Add(new GetItemCategoryDataFromInitiative
            {
                CostTypeData = db.mcosttypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS CostTypeName, \'\' as isActive UNION ALL SELECT b.id, b.CostTypeName, b.isActive FROM t_subcostinitiative a LEFT JOIN mcosttype b ON a.costitemid = b.id WHERE a.savingtypeid = " + SCID + " and b.isActive = \'Y\' GROUP BY b.id, b.CostTypeName").ToList()
            });
            return Json(GDFI, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemFromSubCost(Models.GetInfoByIDModel GetInfo)
        {
            long SCID = GetInfo.Id;
            List<GetItemCategoryDataFromInitiative> GDFI = new List<GetItemCategoryDataFromInitiative>();

            db.Configuration.ProxyCreationEnabled = false;
            GDFI.Add(new GetItemCategoryDataFromInitiative
            {
                ActionTypeData = db.mactiontypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS ActionTypeName,\'\' as isActive UNION ALL SELECT c.id, c.ActionTypeName,c.isActive FROM t_subcostactiontype a LEFT JOIN msubcost b ON a.subcostid = b.id LEFT JOIN mactiontype c ON a.actiontypeid = c.id WHERE b.id = " + SCID + " and c.isActive = \'Y\';").ToList()
            });
            return Json(GDFI, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemFromCostCategory(Models.GetInfoByIDModel GetInfo)
        {
            long SCID = GetInfo.Id; long SCID2 = GetInfo.Id2; long SCID3 = GetInfo.Id3;
            List<GetItemSubCategoryDataFromCategory> GDFC = new List<GetItemSubCategoryDataFromCategory>();

            db.Configuration.ProxyCreationEnabled = false;
            GDFC.Add(new GetItemSubCategoryDataFromCategory
            {
                SubCostData = db.msubcosts.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SubCostName, \'\' as isActive UNION ALL SELECT b.id,b.SubCostName,b.isActive FROM t_subcostbrand a LEFT JOIN msubcost b ON a.subcostid = b.id WHERE a.savingtypeid = " + SCID + " AND a.costtypeid = " + SCID2 + " AND a.brandid = " + SCID3 + " and b.isActive = \'Y\' GROUP BY b.id, b.SubCostName").ToList(),
                ActionTypeData = db.mactiontypes.SqlQuery("SELECT a.actiontypeid AS id,c.ActionTypeName, b.isActive FROM t_cost_actiontype a LEFT JOIN mcosttype b ON a.costitemid = b.id LEFT JOIN mactiontype c ON a.actiontypeid = c.id WHERE a.costitemid = " + SCID2 + " and c.isActive = \'Y\'; ").ToList()
            });
            return Json(GDFC, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoForPopUp(Models.GetInfoByIDModel GetInfo)
        {
            List<OutInitiative> GIFP = new List<OutInitiative>();

            db.Configuration.ProxyCreationEnabled = false;
            if (GetInfo.Id > 0)
            {
                var model = db.vwheaderinitiatives.Where(c => c.id == GetInfo.Id).FirstOrDefault();

                GIFP.Add(new OutInitiative
                {
                    SavingTypeData = db.msavingtypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SavingTypeName,\'\' as isActive UNION ALL Select id, SavingTypeName,isActive From msavingtype where isActive = 'Y' ").ToList(),
                    ActionTypeData = db.mactiontypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS ActionTypeName, \'\' as isActive UNION ALL Select id, ActionTypeName, isActive From mactiontype where isActive = 'Y' ").ToList(),
                    SynImpactData = db.msynimpacts.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SynImpactName, \'\' as isActive UNION ALL Select id, SynImpactName, isActive From msynimpact where isActive = 'Y' ").ToList(),
                    InitStatusData = db.mstatus.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS Status, \'\' AS isActive UNION ALL Select id, Status, isActive From mstatus where isActive ='Y'; ").ToList(),
                    PortNameData = db.mports.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS PortName UNION ALL Select id, PortName From mport").ToList(),
                    MCostTypeData = db.mcosttypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS CostTypeName, \'\' as isActive UNION ALL SELECT b.id, b.CostTypeName,b.isActive FROM t_subcostinitiative a LEFT JOIN mcosttype b ON a.costitemid = b.id WHERE a.savingtypeid = " + model.InitiativeType + " and b.isActive = 'Y' GROUP BY b.id, b.CostTypeName; ").ToList(),
                    MSubCostData = db.msubcosts.SqlQuery("SELECT b.id,b.SubCostName,b.isActive FROM t_subcostbrand a LEFT JOIN msubcost b ON a.subcostid = b.id WHERE a.savingtypeid = " + model.InitiativeType + " AND a.costtypeid = " + model.CostCategoryID + " AND a.brandid = " + model.BrandID + " and b.isActive = 'Y' GROUP BY b.id,b.SubCostName; ").ToList(),
                    MSourceCategory = db.msourcecategories.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS categoryname UNION ALL SELECT id, categoryname FROM msourcecategory").ToList()
                });
            }
            else
            {
                GIFP.Add(new OutInitiative
                {
                    SavingTypeData = db.msavingtypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SavingTypeName, \'\' as isActive UNION ALL Select id, SavingTypeName,isActive From msavingtype where isActive = 'Y' ").ToList(),
                    ActionTypeData = db.mactiontypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS ActionTypeName, \'\' as isActive UNION ALL Select id, ActionTypeName,isActive From mactiontype where isActive = 'Y' ").ToList(),
                    SynImpactData = db.msynimpacts.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SynImpactName,\'\' as isActive UNION ALL Select id, SynImpactName,isActive From msynimpact where isActive = 'Y' ").ToList(),
                    InitStatusData = db.mstatus.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS Status, \'\' AS isActive UNION ALL Select id, Status, isActive From mstatus where isActive ='Y'").ToList(),
                    PortNameData = db.mports.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS PortName UNION ALL Select id, PortName From mport").ToList(),
                    MCostTypeData = db.mcosttypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS CostTypeName, \'\' as isActive UNION ALL Select id, CostTypeName, isActive From mcosttype where isActive = 'Y' ").ToList(),
                    MSourceCategory = db.msourcecategories.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS categoryname UNION ALL SELECT id, categoryname FROM msourcecategory").ToList()
                });
            }
            return Json(GIFP, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CboYear()
        {
            List<myear> model = db.myears.Where(c => c.yrStatus == 1).ToList();
            return PartialView("~/Views/Shared/_CboYearPartial.cshtml", model);
        }
        public ActionResult MultiFileReportUpload(object container)
        {
            var GetDataForUpload = Session["GetDataForUpload"] as GetDataForUpload;
            var InitiativeNumber = GetDataForUpload.InitiativeNumber;

            string UploadDirectory = System.Configuration.ConfigurationManager.AppSettings["UPLOADEDPATH"];

            long DefaultMaxFileSize = 1000 * 1048576; // 1000MB
            //LoginSession profileData = this.Session["DefaultGAINSess"] as LoginSession;

            string filename;
            string resultFileName;
            string resultFileUrl;
            string resultFilePath;
            // e.UploadedFile.SaveAs(resultFilePath);

            string FileDiDB = "";
            LoginSession profileData= this.Session["DefaultGAINSess"] as LoginSession; 
            //t_initiative InitID = db.t_initiative.Where(c => c.InitNumber == InitiativeNumber).FirstOrDefault();
            //if (InitID.InitNumber != null)
            //{
            //    if ((InitID.UploadedFile != null) && (InitID.UploadedFile != ""))
            //    {
            //        FileDiDB = InitID.UploadedFile;
            //        FileDiDB += "|" + filename + "|" + InitiativeNumber;
            //    }
            //    else
            //    {
            //        FileDiDB = filename + "|" + InitiativeNumber;
            //    }
            //    db.Database.ExecuteSqlCommand("update t_initiative set UploadedFile = CONCAT(if(UploadedFile IS NULL,\'\',UploadedFile), \'" + FileDiDB + "|\'), ModifiedBy = \'" + profileData.ID + "\' where InitNumber = \'" + InitiativeNumber + "\' and ProjectYear = '" + profileData.ProjectYear + "' ");
            //    db.SaveChanges();
            //    db.Database.ExecuteSqlCommand("update t_initiative set UploadedFile = CONCAT(if(UploadedFile IS NULL,\'\',UploadedFile), \'" + FileDiDB + "|\'), ModifiedBy = \'" + profileData.ID + "\' where InitNumber = \'" + InitiativeNumber + "\' and ProjectYear = '" + profileData.ProjectYear + "' ");
            //db.SaveChanges();
            UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
            {
                AllowedFileExtensions = new string[] { ".txt", ".xls", ".xlsx", ".pdf", ".doc", "docx", ".pptx", ".ppt", ".msg", ".mseg" },
                MaxFileSize = DefaultMaxFileSize
            };

            UploadControlExtension.GetUploadedFiles("ucDragAndDrop", UploadValidationSettings, (sender, e) =>
            {
                if (e.UploadedFile.IsValid)
                {
                   profileData = this.Session["DefaultGAINSess"] as LoginSession;
                    //e.UploadedFile.fi
                    filename = e.UploadedFile.FileName;
                   resultFileName = e.UploadedFile.FileName;
                     resultFileUrl = UploadDirectory + resultFileName;
                    resultFilePath = HttpContext.Request.MapPath(resultFileUrl);
                    e.UploadedFile.SaveAs(resultFilePath);

                     FileDiDB = "";
                    t_initiative InitID = db.t_initiative.Where(c => c.InitNumber == InitiativeNumber).FirstOrDefault();
                    if (InitID.InitNumber != null)
                    {
                        if ((InitID.UploadedFile != null) && (InitID.UploadedFile != ""))
                        {
                           // FileDiDB = InitID.UploadedFile;
                            FileDiDB +=   filename + "|" + InitiativeNumber;
                        }
                        else
                        {
                            FileDiDB = filename + "|" + InitiativeNumber;
                        }
                       // db.Database.ExecuteSqlCommand("update t_initiative set UploadedFile = \'" + FileDiDB + "\', ModifiedBy = \'" + profileData.ID + "\' where InitNumber = \'" + InitiativeNumber + "\' and ProjectYear = '" + profileData.ProjectYear + "' ");
                        db.Database.ExecuteSqlCommand("update t_initiative set UploadedFile = CONCAT(if(UploadedFile IS NULL,\'\',UploadedFile), \'" + FileDiDB + "|\'), ModifiedBy = \'" + profileData.ID + "\' where InitNumber = \'" + InitiativeNumber + "\' AND isDeleted = 0");
                        db.SaveChanges();
                    }
                    e.CallbackData = filename + "|" + InitiativeNumber;
                }
            });
            //string sqlcommand = "update t_initiative set UploadedFile = CONCAT(if(UploadedFile IS NULL,\'\',UploadedFile), \'" + FileDiDB + "|\'), ModifiedBy = \'" + profileData.ID + "\' where InitNumber = \'" + InitiativeNumber + "\' and ProjectYear = '" + profileData.ProjectYear + "' ";
            return null;
        }
        public ActionResult SetIDUploadFile(GetInfoByIDModel GetInfo)
        {
            var model = new List<t_initiative>();
            var ID = GetInfo.Id;
            var initiative = db.t_initiative.Where(c => c.id == ID).FirstOrDefault();

            GetDataForUpload GetDataForUpload = new GetDataForUpload
            {
                InitiativeNumber = initiative.InitNumber
            };
            Session["GetDataForUpload"] = GetDataForUpload;

            List<GetDataForUpload> GDFU = new List<GetDataForUpload>();
            GDFU.Add(new GetDataForUpload
            {
                InitiativeNumber = initiative.InitNumber,
                UploadedFileData = db.t_initiative.Where(c => c.id == GetInfo.Id).Select(s => new UploadedFileList { id = s.id, UploadedFileName = s.UploadedFile }).ToList()
            });

            return Json(GDFU, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadFilesUI()
        {
            return PartialView("~/Views/ActiveInitiative/_UploadPage.cshtml");
        }
        public ActionResult removefile(RemoveFilePost PostedData)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            string UploadDirectory = System.Configuration.ConfigurationManager.AppSettings["UPLOADEDPATH"];
            string resultFileUrl = Request.MapPath(UploadDirectory + PostedData.Filename);

            // ini buat ngapus yang belakangnya ada tanda |
            db.Database.ExecuteSqlCommand("UPDATE t_initiative SET uploadedfile = REPLACE(UploadedFile,'" + PostedData.Filename + "|" + PostedData.Initiativenumber + "|',''), ModifiedBy = '" + profileData.ID + "' WHERE InitNumber = '" + PostedData.Initiativenumber + "'");

            // ini buat ngapus yang depannya ada tanda |
            db.Database.ExecuteSqlCommand("UPDATE t_initiative SET uploadedfile = REPLACE(UploadedFile,'|" + PostedData.Filename + "|" + PostedData.Initiativenumber + "',''), ModifiedBy = '" + profileData.ID + "' WHERE InitNumber = '" + PostedData.Initiativenumber + "'");

            // ini buat ngapus klo depannya gak ada tanda |
            db.Database.ExecuteSqlCommand("UPDATE t_initiative SET uploadedfile = REPLACE(UploadedFile,'" + PostedData.Filename + "|" + PostedData.Initiativenumber + "',''), ModifiedBy = '" + profileData.ID + "' WHERE InitNumber = '" + PostedData.Initiativenumber + "'");
            db.SaveChanges();

            if (System.IO.File.Exists(resultFileUrl))
            {
                System.IO.File.Delete(resultFileUrl);
                return Content("Ok");
            }
            else
            {
                return Content("File not deleted");
            }

        }
        public ActionResult GetInitiativeComment(GetInfoByIDModel GetInfo)
        {
            t_initiative Initiative = db.t_initiative.Where(c => c.id == GetInfo.Id).FirstOrDefault();
            string agency = Initiative.AgencyComment;
            string rpoc = Initiative.RPOCComment;
            string ho = Initiative.HOComment;

            ResultComment result = new ResultComment
            {
                AgencyComment = agency,
                RPOCComment = rpoc,
                HOComment = ho
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}