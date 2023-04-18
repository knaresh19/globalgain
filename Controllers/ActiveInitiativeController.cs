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
using MySql.Data.MySqlClient;
using System.Data;
using GAIN.Helper;
//using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using DevExpress.Spreadsheet;
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

        private FlatFileHelper objFlatFileHelper = new FlatFileHelper();
        List<SubCountryBrand> lstSubCountryBrand = new List<SubCountryBrand>();
        List<InitTypeCostSubCost> lstInitTypeCostSubCosts = new List<InitTypeCostSubCost>();
        List<mInitiativeStatus> lstInitiativeStatus = new List<mInitiativeStatus>();
        List<t_initiative> lstOOInitiatives = new List<t_initiative>();
        List<t_initiative> lstSCMInitiatives = new List<t_initiative>();
        List<mactiontype> lstActionType = new List<mactiontype>();
        t_initiative tInitRecord = new t_initiative();
        public ActionResult Index()
        {
            //vwheaderinitiative model = new vwheaderinitiative();
            return View();
        }
        public ActionResult ProjectYear(Models.GetInfoByIDModel GetInfo)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;

            var result = DateTime.Parse(DateTime.Now.ToString()).Year;
            var curmonth = DateTime.Parse(DateTime.Now.ToString()).Month;

            if (GetInfo.Id < Convert.ToInt64(result))
            {
                profileData.ProjectMonth = 12;
            }
            if (GetInfo.Id == Convert.ToInt64(result))
            {
                profileData.ProjectMonth = curmonth;
            }

            LoginSession LoginSession = new LoginSession
            {
                ID = profileData.ID,
                ProjectYearOld = profileData.ProjectYear,
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
                istoadmin = profileData.istoadmin,
                ProjectMonth = profileData.ProjectMonth,
                role_code = profileData.role_code
            };
            Session["DefaultGAINSess"] = LoginSession;
            return Content("Ok");
        }
        public ActionResult ProjectMonth(Models.GetInfoByIDModel GetInfo)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            LoginSession LoginSession = new LoginSession
            {
                ID = profileData.ID,
                ProjectYearOld = profileData.ProjectYearOld,
                ProjectYear = profileData.ProjectYear,
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
                istoadmin = profileData.istoadmin,
                ProjectMonth = Convert.ToInt32(GetInfo.Id),
                role_code = profileData.role_code
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
            var projYear = profileData.ProjectYear;
            var spprojyear = projYear;
            var projMonth = profileData.ProjectMonth;

            if (profileData.ProjectYear < 2023)
                profileData.ProjectYear = 2022;


            var where = "";


            if (profileData.confidential_right == 0) where += " and Confidential != 'Y'";
            if (profileData.RegionalOffice_right != null && profileData.RegionalOffice_right != "" && profileData.RegionalOffice_right.Substring(0, 1) == "|")
            {
                var regofficetext = profileData.RegionalOffice_right.Replace("|", "','");
                int lenRegionalOffice_right = regofficetext.Length;
                regofficetext = "(" + regofficetext.Substring(2, (lenRegionalOffice_right - 4)) + ")";
                var RegionalOffice_rightid = db.mregional_office.SqlQuery("select id,RegionID,CountryID,RegionalOffice_Name,SubCountryID,BrandID ,InitYear from mregional_office where RegionalOffice_Name in " + regofficetext + "  group by id,RegionID,CountryID,RegionalOffice_Name,SubCountryID,BrandID").ToList();
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
            if (profileData.Brand_right != null && profileData.Brand_right != "" && (profileData.Brand_right != "ALL" && profileData.Brand_right != "|ALL|"))
            {
                var brandtext = profileData.Brand_right.Replace("|", "','");
                int lenbrand = brandtext.Length;
                brandtext = "(" + brandtext.Substring(2, (lenbrand - 4)) + ")";
                var brandid = db.mbrands.SqlQuery("select id,brandname,isActive,isDeleted , InitYear from mbrand where brandname in " + brandtext + "  group by id,brandname").ToList();
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
            if (profileData.CostItem_right != null && profileData.CostItem_right != "" && (profileData.CostItem_right != "ALL" && profileData.CostItem_right != "|ALL|"))
            {
                var costitemtext = profileData.CostItem_right.Replace("|", "','");
                int lencostitem = costitemtext.Length;
                costitemtext = "(" + costitemtext.Substring(2, (lencostitem - 4)) + ")";
                var costitemid = db.mcosttypes.SqlQuery("select id,CostTypeName,isActive , InitYear from mcosttype where CostTypeName in " + costitemtext + "  group by id,CostTypeName,isActive").ToList();
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
            if (profileData.SubCostItem_right != null && profileData.SubCostItem_right != "" && profileData.SubCostItem_right != "ALL")
            {


                var subcostitemtext = profileData.SubCostItem_right.Replace("|", "','");
                int lensubcostitem = subcostitemtext.Length;
                subcostitemtext = "(" + subcostitemtext.Substring(2, (lensubcostitem - 4)) + ")";
                var subcostitemid = db.msubcosts.SqlQuery("select id,SubCostName,isActive , InitYear from msubcost where SubCostName in " + subcostitemtext + "  group by id,SubCostName,isActive").ToList();
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

            //   var model = db.vwheaderinitiatives.ToList();

            if (profileData.UserType == 2)  //rpoc
            {
                if (profileData.RegionID != null && profileData.RegionID != null && profileData.RegionID != "" && (profileData.RegionID != "|ALL|" && profileData.RegionID != "ALL"))
                {
                    var rpoctext = profileData.RegionID.Replace("|", "','");
                    int lenrpoc = rpoctext.Length;
                    rpoctext = "(" + rpoctext.Substring(2, (lenrpoc - 4)) + ")";
                    var rpocid = db.mregions.SqlQuery("select id,RegionName,isActive , InitYear from mregion where RegionName in " + rpoctext + "  group by id,RegionName,isActive").ToList();
                    var rpoccondition = "";
                    for (var i = 0; i < rpocid.Count(); i++)
                    {
                        rpoccondition += rpocid[i].id.ToString() + ",";
                    }
                    if (rpoccondition != "")
                    {
                        rpoccondition = rpoccondition.Substring(0, rpoccondition.Length - 1);
                        where += " and a.RegionID in (" + rpoccondition + ")";
                    }
                }
                else
                {
                    if (profileData.CostControlSite != null && profileData.CostControlSite != "|ALL|" && profileData.CostControlSite != "ALL")
                    {
                        var cctext = profileData.CostControlSite.Replace("|", "','");
                        int lencctext = cctext.Length;
                        cctext = "(" + cctext.Substring(2, (lencctext - 4)) + ")";
                        var ccid = db.mcostcontrolsites.SqlQuery("SELECT id,CostControlSiteName , Inityear FROM mcostcontrolsite WHERE CostControlSiteName IN " + cctext + "  group by id,CostControlSiteName ").ToList();
                        var cccondition = "";
                        for (var i = 0; i < ccid.Count(); i++)
                        {
                            cccondition += ccid[i].id.ToString() + ",";
                        }
                        if (cccondition != "")
                        {
                            cccondition = cccondition.Substring(0, cccondition.Length - 1);
                            where += " and a.CostControlID in (" + cccondition + ")";
                        }
                    }
                }
            }
            else if (profileData.UserType == 3) //agency
            {
                var cntrytext = profileData.subcountry_right.Replace("|", "','");
                int lencntrytext = cntrytext.Length;
                cntrytext = "(" + cntrytext.Substring(2, (lencntrytext - 4)) + ")";
                var cntryid = db.msubcountries.SqlQuery("SELECT id,CountryID,SubCountryName,CountryCode,isActive , InitYear FROM msubcountry WHERE SubCountryName IN " + cntrytext + "  group by id,CountryID,SubCountryName,CountryCode,isActive").ToList();
                var cntryidcondition = "";
                for (var i = 0; i < cntryid.Count(); i++)
                {
                    cntryidcondition += cntryid[i].id.ToString() + ",";
                }
                if (cntryidcondition != "")
                {
                    cntryidcondition = cntryidcondition.Substring(0, cntryidcondition.Length - 1);
                    where += " and a.SubCountryID in (" + cntryidcondition + ")";
                }
            }

            //ConsoleLog(" UserType: " + profileData.UserType + "\\n RegionID: " + profileData.RegionID + "\\n CostControlSite: " + profileData.CostControlSite + "\\n Country: " + profileData.CountryID + "\\n Condition: " + where);


            string YTD_Achieved_PRICE_EF_months = string.Empty;
            string YTD_Achieved_VOLUME_EF_months = string.Empty;

            string N_YTD_Sec_PRICE_EF_months = string.Empty;
            string N_YTD_Sec_VOLUME_EF_months = string.Empty;

            string N_YTD_ST_Total_EF_months = string.Empty;

            string YTD_Cost_Avoid_Vs_CPI_months = string.Empty;
            List<string> arrMonth = new List<string>() { "jan", "feb", "march", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            int _counter = 0;

            while (_counter < projMonth)
            {

                #region Secured
                //N_YTD_Sec_PRICE_EF "_ST_Price_effect"
                if (String.IsNullOrEmpty(N_YTD_Sec_PRICE_EF_months))
                {
                    N_YTD_Sec_PRICE_EF_months = "" + arrMonth[_counter] + "_ST_Price_effect";
                }
                else
                {
                    N_YTD_Sec_PRICE_EF_months = N_YTD_Sec_PRICE_EF_months + "," + arrMonth[_counter] + "_ST_Price_effect";
                }

                //N_FY_Sec_VOLUME_EF "ST_Volume_Effect" 
                if (String.IsNullOrEmpty(N_YTD_Sec_VOLUME_EF_months))
                {
                    N_YTD_Sec_VOLUME_EF_months = "" + arrMonth[_counter] + "_ST_Volume_Effect";
                }
                else
                {
                    N_YTD_Sec_VOLUME_EF_months = N_YTD_Sec_VOLUME_EF_months + "," + arrMonth[_counter] + "_ST_Volume_Effect";
                }

                //N_YTD_Secured "_FY_Secured_Target"
                if (String.IsNullOrEmpty(N_YTD_ST_Total_EF_months))
                {
                    N_YTD_ST_Total_EF_months = "" + arrMonth[_counter] + "_FY_Secured_Target";
                }
                else
                {
                    N_YTD_ST_Total_EF_months = N_YTD_ST_Total_EF_months + "," + arrMonth[_counter] + "_FY_Secured_Target";
                }

                #endregion

                #region Acheived
                //YTD_Achieved_PRICE_EF "_A_Price_effect"
                if (String.IsNullOrEmpty(YTD_Achieved_PRICE_EF_months))
                {
                    YTD_Achieved_PRICE_EF_months = "" + arrMonth[_counter] + "_A_Price_effect";
                }
                else
                {
                    YTD_Achieved_PRICE_EF_months = YTD_Achieved_PRICE_EF_months + "," + arrMonth[_counter] + "_A_Price_effect";
                }

                //YTD_Achieved_VOLUME_EF "_A_Volume_Effect"
                if (String.IsNullOrEmpty(YTD_Achieved_VOLUME_EF_months))
                {
                    YTD_Achieved_VOLUME_EF_months = "" + arrMonth[_counter] + "_A_Volume_Effect";
                }
                else
                {
                    YTD_Achieved_VOLUME_EF_months = YTD_Achieved_VOLUME_EF_months + "," + arrMonth[_counter] + "_A_Volume_Effect";
                }
                #endregion

                #region CPI
                //YTD_Cost_Avoid_Vs_CPI "_CPI_Effect"
                if (String.IsNullOrEmpty(YTD_Cost_Avoid_Vs_CPI_months))
                {
                    YTD_Cost_Avoid_Vs_CPI_months = "" + arrMonth[_counter] + "_CPI_Effect";
                }
                else
                {
                    YTD_Cost_Avoid_Vs_CPI_months = YTD_Cost_Avoid_Vs_CPI_months + "," + arrMonth[_counter] + "_CPI_Effect";
                }

                #endregion

                _counter++;
            }

            var spcondi = "a.isDeleted =0 " + where;
            //if (Session["issaveupdtae"] == "1")
            //{
            //    Session["issaveupdtae"] = 0;
            //    model = GetGridData(profileData.ProjectYear, spcondi, projMonth, 1);
            //}
            //else
            //{
            //    model = GetGridData(profileData.ProjectYear, spcondi, projMonth, 0);

            //}
            var model = GetGridData(spprojyear, spcondi, projMonth, 1);

            // model = db.vwheaderinitiatives.SqlQuery("select * from vwheaderinitiative as a where isDeleted = 0 and ProjectYear = '" + profileData.ProjectYear + "' " + where + " order by CreatedDate desc").ToList();
            //model = db.vwheaderinitiatives.SqlQuery("select * from vwheaderinitiative as a where   isDeleted = 0 and (Year(StartMonth) = '" + profileData.ProjectYear + "' or Year(EndMonth) = '" + profileData.ProjectYear + "') " + where + " order by CreatedDate desc").ToList();
            // var  model = db.vwheaderinitiatives.SqlQuery("select * from vwheaderinitiative as a where   isDeleted = 0 and (Year(StartMonth) = '" + profileData.ProjectYear + "') "+ where + "  order by CreatedDate desc").ToList();
            //  var model = db.vwheaderinitiatives.SqlQuery("select * from vwheaderinitiative as a where   isDeleted = 0 and (Year(StartMonth) = '" + profileData.ProjectYear + "' or Year(EndMonth)='" + profileData.ProjectYear + "')  " + where + "  order by CreatedDate desc").ToList();
            ViewData["mregions"] = db.mregions.Where(c => c.InitYear == projYear).ToList();
            ViewData["brandname"] = db.mbrands.Where(c => c.isActive == "Y" && c.isDeleted == "N" && c.InitYear == projYear).ToList();
            ViewData["msubregion"] = db.msubregions.Where(c => c.SubRegionName != null && c.SubRegionName != "" && c.InitYear == projYear).ToList();
            //ViewData["mcluster"] = db.mclusters.SqlQuery("SELECT * FROM mcluster where ClusterName != \'\'").ToList();
            ViewData["mcluster"] = db.mclusters.Where(c => c.ClusterName != "" && c.InitYear == projYear).GroupBy(g => g.ClusterName).Select(s => new { ClusterName = s.Key }).ToList();
            ViewData["mregional_office"] = db.mregional_office.SqlQuery("SELECT * FROM mregional_office where InitYear=" + projYear + "").GroupBy(g => g.RegionalOffice_Name).Select(s => new { RegionalOffice_Name = s.Key }).ToList();
            ViewData["CostControlSiteName"] = db.mcostcontrolsites.Where(c => c.CostControlSiteName != "" && c.InitYear == projYear).ToList();
            ViewData["CountryName"] = db.mcountries.Where(c => c.CountryName != "" && c.InitYear == projYear).ToList();
            ViewData["SubCountryName"] = db.msubcountries.Where(c => c.SubCountryName != "" && c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["LegalEntityName"] = db.mlegalentities.Where(c => c.InitYear == projYear).GroupBy(g => g.LegalEntityName).Select(s => new { LegalEntityName = s.Key }).ToList();
            ViewData["SavingTypeName"] = db.msavingtypes.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["CostTypeName"] = db.mcosttypes.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["ActionTypeName"] = db.mactiontypes.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["SynImpactName"] = db.msynimpacts.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["Status"] = db.mstatus.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["portName"] = db.mports.Where(c => c.InitYear == projYear).ToList();
            ViewData["SourceCategoryName"] = db.msourcecategories.Where(c => c.InitYear == projYear).ToList();

            //ViewData["mregions"] = db.mregions.SqlQuery("SET GLOBAL sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY','')); SELECT * FROM mregion group by RegionName").ToList();
            //ViewData["brandname"] = db.mbrands.SqlQuery("SELECT * FROM mbrand group by brandname").Where(c => c.isActive == "Y" && c.isDeleted == "N").ToList();
            //ViewData["msubregion"] = db.msubregions.SqlQuery("SELECT * FROM msubregion group by SubRegionName").Where(c => c.SubRegionName != null && c.SubRegionName != "").ToList();
            //ViewData["mcluster"] = db.mclusters.SqlQuery("SELECT * FROM mcluster where ClusterName != \'\'").ToList();
            //ViewData["mcluster"] = db.mclusters.SqlQuery("SELECT * FROM mcluster group by ClusterName").Where(c => c.ClusterName != "").GroupBy(g => g.ClusterName).Select(s => new { ClusterName = s.Key }).ToList();
            //ViewData["mregional_office"] = db.mregional_office.SqlQuery("SELECT * FROM mregional_office").GroupBy(g => g.RegionalOffice_Name).Select(s => new { RegionalOffice_Name = s.Key }).ToList();
            //ViewData["CostControlSiteName"] = db.mcostcontrolsites.SqlQuery("SELECT * FROM mcostcontrolsite group by CostControlSiteName").Where(c => c.CostControlSiteName != "").ToList();
            //ViewData["CountryName"] = db.mcountries.SqlQuery("SELECT * FROM mcountry group by CountryName").Where(c => c.CountryName != "").ToList();
            //ViewData["SubCountryName"] = db.msubcountries.SqlQuery("SELECT * FROM msubcountry group by SubCountryName").Where(c => c.SubCountryName != "" && c.isActive == "Y").ToList();
            //ViewData["LegalEntityName"] = db.mlegalentities.SqlQuery("SELECT * FROM mlegalentity group by LegalEntityName").GroupBy(g => g.LegalEntityName).Select(s => new { LegalEntityName = s.Key }).ToList();
            //ViewData["SavingTypeName"] = db.msavingtypes.SqlQuery("SELECT * FROM msavingtype group by SavingTypeName").Where(c => c.isActive == "Y").ToList();
            //ViewData["CostTypeName"] = db.mcosttypes.SqlQuery("SELECT * FROM mcosttype group by CostTypeName").Where(c => c.isActive == "Y").ToList();
            //ViewData["SubCostName"] = db.msubcosts.SqlQuery("SELECT * FROM msubcost group by SubCostName").Where(c => c.isActive == "Y").ToList();
            //ViewData["ActionTypeName"] = db.mactiontypes.SqlQuery("SELECT * FROM mactiontype group by ActionTypeName").Where(c => c.isActive == "Y").ToList();
            //ViewData["SynImpactName"] = db.msynimpacts.SqlQuery("SELECT * FROM msynimpact group by SynImpactName").Where(c => c.isActive == "Y").ToList();
            //ViewData["Status"] = db.mstatus.SqlQuery("SELECT * FROM mstatus group by Status").Where(c => c.isActive == "Y").ToList();
            //ViewData["portName"] = db.mports.SqlQuery("SELECT * FROM mport group by PortName").ToList();
            //ViewData["SourceCategoryName"] = db.msourcecategories.SqlQuery("SELECT * FROM msourcecategory group by categoryname").ToList();


            //ViewData["mregions"] = db.mregions.ToList();
            //ViewData["brandname"] = db.mbrands.Where(c => c.isActive == "Y" && c.isDeleted == "N").ToList();
            //ViewData["msubregion"] = db.msubregions.Where(c => c.SubRegionName != null && c.SubRegionName != "").ToList();
            //ViewData["mcluster"] = db.mclusters.SqlQuery("SELECT * FROM mcluster where ClusterName != \'\'").ToList();
            //ViewData["mcluster"] = db.mclusters.Where(c => c.ClusterName != "").GroupBy(g => g.ClusterName).Select(s => new { ClusterName = s.Key }).ToList();
            //ViewData["mregional_office"] = db.mregional_office.SqlQuery("SELECT * FROM mregional_office").GroupBy(g => g.RegionalOffice_Name).Select(s => new { RegionalOffice_Name = s.Key }).ToList();
            //ViewData["CostControlSiteName"] = db.mcostcontrolsites.Where(c => c.CostControlSiteName != "").ToList();
            //ViewData["CountryName"] = db.mcountries.Where(c => c.CountryName != "").ToList();
            //ViewData["SubCountryName"] = db.msubcountries.Where(c => c.SubCountryName != "" && c.isActive == "Y").ToList();
            //ViewData["LegalEntityName"] = db.mlegalentities.GroupBy(g => g.LegalEntityName).Select(s => new { LegalEntityName = s.Key }).ToList();
            //ViewData["SavingTypeName"] = db.msavingtypes.Where(c => c.isActive == "Y").ToList();
            //ViewData["CostTypeName"] = db.mcosttypes.Where(c => c.isActive == "Y").ToList();
            //ViewData["SubCostName"] = db.msubcosts.Where(c => c.isActive == "Y").ToList();
            //ViewData["ActionTypeName"] = db.mactiontypes.Where(c => c.isActive == "Y").ToList();
            //ViewData["SynImpactName"] = db.msynimpacts.Where(c => c.isActive == "Y").ToList();
            //ViewData["Status"] = db.mstatus.Where(c => c.isActive == "Y").ToList();
            //ViewData["portName"] = db.mports.ToList();
            //ViewData["SourceCategoryName"] = db.msourcecategories.ToList();

            if (1 == 2)
            {
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

                    #region procurement YTD calcs
                    if (profileData.ProjectYear >= 2023)
                    {
                        t_initiative_calcs _t_initiative_calcs = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault();
                        if (_t_initiative_calcs != null)
                        {
                            string YTD_Achieved_PRICE_EF_months_ = string.Empty;
                            string YTD_Achieved_VOLUME_EF_months_ = string.Empty;

                            string N_YTD_Sec_PRICE_EF_months_ = string.Empty;
                            string N_YTD_Sec_VOLUME_EF_months_ = string.Empty;

                            string N_YTD_ST_Total_EF_months_ = string.Empty;

                            string YTD_Cost_Avoid_Vs_CPI_months_ = string.Empty;
                            List<string> arrMonth__ = new List<string>() { "jan", "feb", "march", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
                            int counter__ = 0;

                            while (counter__ < projMonth)
                            {

                                #region Secured
                                //N_YTD_Sec_PRICE_EF "_ST_Price_effect"
                                if (String.IsNullOrEmpty(N_YTD_Sec_PRICE_EF_months_))
                                {
                                    N_YTD_Sec_PRICE_EF_months_ = "" + arrMonth__[counter__] + "_ST_Price_effect";
                                }
                                else
                                {
                                    N_YTD_Sec_PRICE_EF_months_ = N_YTD_Sec_PRICE_EF_months_ + "," + arrMonth__[counter__] + "_ST_Price_effect";
                                }

                                //N_FY_Sec_VOLUME_EF "ST_Volume_Effect" 
                                if (String.IsNullOrEmpty(N_YTD_Sec_VOLUME_EF_months_))
                                {
                                    N_YTD_Sec_VOLUME_EF_months_ = "" + arrMonth__[counter__] + "_ST_Volume_Effect";
                                }
                                else
                                {
                                    N_YTD_Sec_VOLUME_EF_months_ = N_YTD_Sec_VOLUME_EF_months_ + "," + arrMonth__[counter__] + "_ST_Volume_Effect";
                                }

                                //N_YTD_Secured "_FY_Secured_Target"
                                if (String.IsNullOrEmpty(N_YTD_ST_Total_EF_months_))
                                {
                                    N_YTD_ST_Total_EF_months_ = "" + arrMonth__[counter__] + "_FY_Secured_Target";
                                }
                                else
                                {
                                    N_YTD_ST_Total_EF_months_ = N_YTD_ST_Total_EF_months_ + "," + arrMonth__[counter__] + "_FY_Secured_Target";
                                }

                                #endregion

                                #region Acheived
                                //YTD_Achieved_PRICE_EF "_A_Price_effect"
                                if (String.IsNullOrEmpty(YTD_Achieved_PRICE_EF_months_))
                                {
                                    YTD_Achieved_PRICE_EF_months_ = "" + arrMonth__[counter__] + "_A_Price_effect";
                                }
                                else
                                {
                                    YTD_Achieved_PRICE_EF_months_ = YTD_Achieved_PRICE_EF_months_ + "," + arrMonth__[counter__] + "_A_Price_effect";
                                }

                                //YTD_Achieved_VOLUME_EF "_A_Volume_Effect"
                                if (String.IsNullOrEmpty(YTD_Achieved_VOLUME_EF_months_))
                                {
                                    YTD_Achieved_VOLUME_EF_months_ = "" + arrMonth__[counter__] + "_A_Volume_Effect";
                                }
                                else
                                {
                                    YTD_Achieved_VOLUME_EF_months_ = YTD_Achieved_VOLUME_EF_months_ + "," + arrMonth__[counter__] + "_A_Volume_Effect";
                                }
                                #endregion

                                #region CPI
                                //YTD_Cost_Avoid_Vs_CPI "_CPI_Effect"
                                if (String.IsNullOrEmpty(YTD_Cost_Avoid_Vs_CPI_months_))
                                {
                                    YTD_Cost_Avoid_Vs_CPI_months_ = "" + arrMonth__[counter__] + "_CPI_Effect";
                                }
                                else
                                {
                                    YTD_Cost_Avoid_Vs_CPI_months_ = YTD_Cost_Avoid_Vs_CPI_months_ + "," + arrMonth__[counter__] + "_CPI_Effect";
                                }

                                #endregion

                                counter__++;
                            }

                            #region FY Secured Target
                            //N FY Secured (TOTAL EFFECT) -------------------------------------------------------------------------------------
                            item.N_FY_ST_Total_EF = Math.Round(Convert.ToDecimal(item.N_FY_Sec_PRICE_EF) + Convert.ToDecimal(item.N_FY_Sec_VOLUME_EF), 0);
                            #endregion


                            #region YTD Secured Target
                            //N YTD Secured (PRICE EFFECT) -------------------------------------------------------------------------------------
                            var result_SPE = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id)
                                                                .Select(GAIN.Models.Utilities.DynamicSelectGenerator<t_initiative_calcs>("t_initiative_ID," + N_YTD_Sec_PRICE_EF_months_)).ToList();
                            var _N_YTD_Sec_PRICE_EF = result_SPE.Where(x => x.t_initiative_ID == item.id)
                                                                                .Select(y => y.jan_ST_Price_effect + y.feb_ST_Price_effect + y.march_ST_Price_effect + y.apr_ST_Price_effect + y.may_ST_Price_effect +
                                                                                        y.jun_ST_Price_effect + y.jul_ST_Price_effect + y.aug_ST_Price_effect + y.sep_ST_Price_effect + y.oct_ST_Price_effect +
                                                                                        y.nov_ST_Price_effect + y.dec_ST_Price_effect).FirstOrDefault().ToString();
                            item.N_YTD_Sec_PRICE_EF = Math.Round(Convert.ToDecimal(_N_YTD_Sec_PRICE_EF), 0);

                            //N YTD Secured (VOLUME EFFECT) -------------------------------------------------------------------------------------
                            var result_SVE = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id)
                                                                .Select(GAIN.Models.Utilities.DynamicSelectGenerator<t_initiative_calcs>("t_initiative_ID," + N_YTD_Sec_VOLUME_EF_months_)).ToList();
                            var _N_YTD_Sec_VOLUME_EF = result_SVE.Where(x => x.t_initiative_ID == item.id)
                                                                                .Select(y => y.jan_ST_Volume_Effect + y.feb_ST_Volume_Effect + y.march_ST_Volume_Effect + y.apr_ST_Volume_Effect + y.may_ST_Volume_Effect +
                                                                                        y.jun_ST_Volume_Effect + y.jul_ST_Volume_Effect + y.aug_ST_Volume_Effect + y.sep_ST_Volume_Effect + y.oct_ST_Volume_Effect +
                                                                                        y.nov_ST_Volume_Effect + y.dec_ST_Volume_Effect).FirstOrDefault().ToString();
                            item.N_YTD_Sec_VOLUME_EF = Math.Round(Convert.ToDecimal(_N_YTD_Sec_VOLUME_EF), 0);

                            //N YTD Secured (TOTAL EFFECT) -------------------------------------------------------------------------------------
                            var result_FYS = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id)
                                                                .Select(GAIN.Models.Utilities.DynamicSelectGenerator<t_initiative_calcs>("t_initiative_ID," + N_YTD_ST_Total_EF_months_)).ToList();
                            var _N_YTD_Secured = result_FYS.Where(x => x.t_initiative_ID == item.id)
                                                                                .Select(y => y.jan_FY_Secured_Target + y.feb_FY_Secured_Target + y.march_FY_Secured_Target + y.apr_FY_Secured_Target + y.may_FY_Secured_Target +
                                                                                        y.jun_FY_Secured_Target + y.jul_FY_Secured_Target + y.aug_FY_Secured_Target + y.sep_FY_Secured_Target + y.oct_FY_Secured_Target +
                                                                                        y.nov_FY_Secured_Target + y.dec_FY_Secured_Target).FirstOrDefault().ToString();
                            item.N_YTD_ST_Total_EF = Math.Round(Convert.ToDecimal(_N_YTD_Secured), 0);
                            #endregion



                            #region YTD Acheivement
                            //YTD Achieved (PRICE EFFECT) -------------------------------------------------------------------------------------
                            var result_APE = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id)
                                                                .Select(GAIN.Models.Utilities.DynamicSelectGenerator<t_initiative_calcs>("t_initiative_ID," + YTD_Achieved_PRICE_EF_months_)).ToList();
                            var _YTD_Achieved_PRICE_EF = result_APE.Where(x => x.t_initiative_ID == item.id)
                                                                                .Select(y => y.jan_A_Price_effect + y.feb_A_Price_effect + y.march_A_Price_effect + y.apr_A_Price_effect + y.may_A_Price_effect +
                                                                                        y.jun_A_Price_effect + y.jul_A_Price_effect + y.aug_A_Price_effect + y.sep_A_Price_effect + y.oct_A_Price_effect +
                                                                                        y.nov_A_Price_effect + y.dec_A_Price_effect).FirstOrDefault().ToString();
                            item.YTD_Achieved_PRICE_EF = Math.Round(Convert.ToDecimal(_YTD_Achieved_PRICE_EF), 0);

                            //YTD Achieved (VOLUME EFFECT)  -------------------------------------------------------------------------------------
                            var result_AVE = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id)
                                                                .Select(GAIN.Models.Utilities.DynamicSelectGenerator<t_initiative_calcs>("t_initiative_ID," + YTD_Achieved_VOLUME_EF_months_)).ToList();
                            var _YTD_Achieved_VOLUME_EF = result_AVE.Where(x => x.t_initiative_ID == item.id)
                                                                                .Select(y => y.jan_A_Volume_Effect + y.feb_A_Volume_Effect + y.march_A_Volume_Effect + y.apr_A_Volume_Effect + y.may_A_Volume_Effect +
                                                                                        y.jun_A_Volume_Effect + y.jul_A_Volume_Effect + y.aug_A_Volume_Effect + y.sep_A_Volume_Effect + y.oct_A_Volume_Effect +
                                                                                        y.nov_A_Volume_Effect + y.dec_A_Volume_Effect).FirstOrDefault().ToString();
                            item.YTD_Achieved_VOLUME_EF = Math.Round(Convert.ToDecimal(_YTD_Achieved_VOLUME_EF), 0);

                            //YTD Achieved (TOTAL EFFECT)  -------------------------------------------------------------------------------------
                            item.N_YTD_A_Total_EF = Math.Round(Convert.ToDecimal(item.YTD_Achieved_PRICE_EF) + Convert.ToDecimal(item.YTD_Achieved_VOLUME_EF), 0);
                            #endregion


                            #region CPI
                            //YTD Cost Avoidance Vs CPI -------------------------------------------------------------------------------------
                            var result_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id)
                                                                .Select(GAIN.Models.Utilities.DynamicSelectGenerator<t_initiative_calcs>("t_initiative_ID," + YTD_Cost_Avoid_Vs_CPI_months_)).ToList();
                            var _YTD_Cost_Avoid_Vs_CPI = result_CPI.Where(x => x.t_initiative_ID == item.id)
                                                                                .Select(y => y.jan_CPI_Effect + y.feb_CPI_Effect + y.march_CPI_Effect + y.apr_CPI_Effect + y.may_CPI_Effect +
                                                                                        y.jun_CPI_Effect + y.jul_CPI_Effect + y.aug_CPI_Effect + y.sep_CPI_Effect + y.oct_CPI_Effect +
                                                                                        y.nov_CPI_Effect + y.dec_CPI_Effect).FirstOrDefault().ToString();
                            item.YTD_Cost_Avoid_Vs_CPI = Math.Round(Convert.ToDecimal(_YTD_Cost_Avoid_Vs_CPI), 0);


                            item.jan_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().jan_CPI;
                            item.feb_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().feb_CPI;
                            item.mar_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().mar_CPI;
                            item.apr_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().apr_CPI;
                            item.may_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().may_CPI;
                            item.jun_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().jun_CPI;
                            item.jul_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().jul_CPI;
                            item.aug_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().aug_CPI;
                            item.sep_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().sep_CPI;
                            item.oct_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().oct_CPI;
                            item.nov_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().nov_CPI;
                            item.dec_CPI = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id).FirstOrDefault().dec_CPI;

                            #endregion

                            #region TMonthly Target and Achieved
                            var result_Init_Calcs = db.t_initiative_calcs.Where(x => x.t_initiative_ID == item.id);

                            //item.AchJan = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.jan_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchFeb = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.feb_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchMar = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.march_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchApr = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.apr_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchMay = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.may_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchJun = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.jun_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchJul = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.jul_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchAug = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.aug_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchSep = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.sep_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchOct = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.oct_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchNov = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.nov_Achievement).FirstOrDefault().ToString()), 0);
                            //item.AchDec = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.dec_Achievement).FirstOrDefault().ToString()), 0);

                            //item.TargetJan = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.jan_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetFeb = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.feb_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetMar = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.march_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetApr = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.apr_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetMay = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.may_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetJun = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.jun_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetJul = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.jul_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetAug = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.aug_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetSep = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.sep_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetOct = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.oct_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetNov = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.nov_FY_Secured_Target).FirstOrDefault().ToString()), 0);
                            //item.TargetDec = Math.Round(Convert.ToDecimal(result_Init_Calcs.Where(x => x.t_initiative_ID == item.id).Select(y => y.dec_FY_Secured_Target).FirstOrDefault().ToString()), 0);

                            #endregion
                        }
                        else
                        {
                            if (item.isProcurement == 0)
                            {
                                List<string> __arrMonth = new List<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                                string YTD_Target_months = string.Empty;
                                string YTD_Acheived_months = string.Empty;
                                int counter1__ = 0;
                                while (counter1__ < projMonth)
                                {
                                    if (String.IsNullOrEmpty(YTD_Target_months))
                                    {
                                        YTD_Target_months = "Target" + __arrMonth[counter1__];
                                    }
                                    else
                                    {
                                        YTD_Target_months = YTD_Target_months + "," + "Target" + __arrMonth[counter1__];
                                    }

                                    if (String.IsNullOrEmpty(YTD_Acheived_months))
                                    {
                                        YTD_Acheived_months = "Ach" + __arrMonth[counter1__];
                                    }
                                    else
                                    {
                                        YTD_Acheived_months = YTD_Acheived_months + "," + "Ach" + __arrMonth[counter1__];
                                    }

                                    counter1__++;
                                }
                                var result_FY_Target = db.t_initiative.Where(x => x.id == item.id)
                                                                              .Select(GAIN.Models.Utilities.DynamicSelectGenerator<t_initiative>("id," + YTD_Target_months)).ToList();
                                var _N_YTD_Secured = result_FY_Target.Where(x => x.id == item.id)
                                                                                    .Select(y => (y.TargetJan == null ? 0 : y.TargetJan) +
                                                                                                (y.TargetFeb == null ? 0 : y.TargetFeb) +
                                                                                                (y.TargetMar == null ? 0 : y.TargetMar) +
                                                                                                (y.TargetApr == null ? 0 : y.TargetApr) +
                                                                                                (y.TargetMay == null ? 0 : y.TargetMay) +
                                                                                                (y.TargetJun == null ? 0 : y.TargetJun) +
                                                                                                (y.TargetJul == null ? 0 : y.TargetJul) +
                                                                                                (y.TargetAug == null ? 0 : y.TargetAug) +
                                                                                                (y.TargetSep == null ? 0 : y.TargetSep) +
                                                                                                (y.TargetOct == null ? 0 : y.TargetOct) +
                                                                                                (y.TargetNov == null ? 0 : y.TargetNov) +
                                                                                                (y.TargetDec == null ? 0 : y.TargetDec)).FirstOrDefault().ToString();
                                item.N_YTD_ST_Total_EF = Math.Round(Convert.ToDecimal(_N_YTD_Secured), 0);
                                item.YTDTargetUb = Math.Round(Convert.ToDecimal(_N_YTD_Secured), 0);
                                item.TargetNYUB = Math.Round(Convert.ToDecimal(_N_YTD_Secured), 0);


                                var result_YTD_Acheived = db.t_initiative.Where(x => x.id == item.id)
                                                                              .Select(GAIN.Models.Utilities.DynamicSelectGenerator<t_initiative>("id," + YTD_Acheived_months)).ToList();
                                var _N_YTD_A_Total_EF = result_YTD_Acheived.Where(x => x.id == item.id)
                                                                                    .Select(y => (y.AchJan == null ? 0 : y.AchJan) +
                                                                                                (y.AchFeb == null ? 0 : y.AchFeb) +
                                                                                                (y.AchMar == null ? 0 : y.AchMar) +
                                                                                                (y.AchApr == null ? 0 : y.AchApr) +
                                                                                                (y.AchMay == null ? 0 : y.AchMay) +
                                                                                                (y.AchJun == null ? 0 : y.AchJun) +
                                                                                                (y.AchJul == null ? 0 : y.AchJul) +
                                                                                                (y.AchAug == null ? 0 : y.AchAug) +
                                                                                                (y.AchSep == null ? 0 : y.AchSep) +
                                                                                                (y.AchOct == null ? 0 : y.AchOct) +
                                                                                                (y.AchNov == null ? 0 : y.AchNov) +
                                                                                                (y.AchDec == null ? 0 : y.AchDec)).FirstOrDefault().ToString();
                                item.N_YTD_A_Total_EF = Math.Round(Convert.ToDecimal(_N_YTD_A_Total_EF), 0);
                                item.YTDAchievedUb = Math.Round(Convert.ToDecimal(_N_YTD_A_Total_EF), 0);
                            }
                        }
                    }
                    #endregion

                }
            }


            return PartialView("_GrdMainInitiativePartial", model);
        }

        public List<GAIN.Models.vwheaderinitiative> GetGridData(long ProjectYear, string condi, int projMonth, int issave)
        {

            string YTD_Achieved_PRICE_EF_months = string.Empty;
            string YTD_Achieved_VOLUME_EF_months = string.Empty;

            string N_YTD_Sec_PRICE_EF_months = string.Empty;
            string N_YTD_Sec_VOLUME_EF_months = string.Empty;

            string N_YTD_ST_Total_EF_months = string.Empty;

            string YTD_Cost_Avoid_Vs_CPI_months = string.Empty;

            string YTD_Achieved_PRICE_EF_monthschk = string.Empty;
            string YTD_Achieved_VOLUME_EF_monthschk = string.Empty;

            string N_YTD_Sec_PRICE_EF_monthschk = string.Empty;
            string N_YTD_Sec_VOLUME_EF_monthschk = string.Empty;

            string N_YTD_ST_Total_EF_monthschk = string.Empty;

            string YTD_Cost_Avoid_Vs_CPI_monthschk = string.Empty;

            List<string> arrMonth = new List<string>() { "jan", "feb", "march", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            int _counter = 0;

            while (_counter < projMonth)
            {

                #region Secured
                //N_YTD_Sec_PRICE_EF "_ST_Price_effect"
                if (String.IsNullOrEmpty(N_YTD_Sec_PRICE_EF_months))
                {
                    N_YTD_Sec_PRICE_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_ST_Price_effect, 0)";
                    N_YTD_Sec_PRICE_EF_months = "" + N_YTD_Sec_PRICE_EF_monthschk;
                }
                else
                {
                    N_YTD_Sec_PRICE_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_ST_Price_effect, 0)";
                    N_YTD_Sec_PRICE_EF_months = N_YTD_Sec_PRICE_EF_months + "+" + N_YTD_Sec_PRICE_EF_monthschk;
                }

                //N_FY_Sec_VOLUME_EF "ST_Volume_Effect" 
                if (String.IsNullOrEmpty(N_YTD_Sec_VOLUME_EF_months))
                {
                    N_YTD_Sec_VOLUME_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_ST_Volume_Effect, 0)";
                    N_YTD_Sec_VOLUME_EF_months = "" + N_YTD_Sec_VOLUME_EF_monthschk;
                }
                else
                {
                    N_YTD_Sec_VOLUME_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_ST_Volume_Effect, 0)";
                    N_YTD_Sec_VOLUME_EF_months = N_YTD_Sec_VOLUME_EF_months + "+" + N_YTD_Sec_VOLUME_EF_monthschk;
                }

                //N_YTD_Secured "_FY_Secured_Target"
                if (String.IsNullOrEmpty(N_YTD_ST_Total_EF_months))
                {
                    N_YTD_ST_Total_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_FY_Secured_Target, 0)";
                    N_YTD_ST_Total_EF_months = "" + N_YTD_ST_Total_EF_monthschk;
                }
                else
                {
                    N_YTD_ST_Total_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_FY_Secured_Target, 0)";
                    N_YTD_ST_Total_EF_months = N_YTD_ST_Total_EF_months + "+" + N_YTD_ST_Total_EF_monthschk;
                }

                #endregion

                #region Acheived
                //YTD_Achieved_PRICE_EF "_A_Price_effect"
                if (String.IsNullOrEmpty(YTD_Achieved_PRICE_EF_months))
                {
                    YTD_Achieved_PRICE_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_A_Price_effect, 0)";

                    YTD_Achieved_PRICE_EF_months = YTD_Achieved_PRICE_EF_monthschk;
                }
                else
                {
                    YTD_Achieved_PRICE_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_A_Price_effect, 0)";
                    YTD_Achieved_PRICE_EF_months = YTD_Achieved_PRICE_EF_months + "+" + YTD_Achieved_PRICE_EF_monthschk;
                }

                //YTD_Achieved_VOLUME_EF "_A_Volume_Effect"
                if (String.IsNullOrEmpty(YTD_Achieved_VOLUME_EF_months))
                {
                    YTD_Achieved_VOLUME_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_A_Volume_Effect, 0)";
                    YTD_Achieved_VOLUME_EF_months = "" + YTD_Achieved_VOLUME_EF_monthschk;
                }
                else
                {
                    YTD_Achieved_VOLUME_EF_monthschk = "ifnull(" + arrMonth[_counter] + "_A_Volume_Effect, 0)";
                    YTD_Achieved_VOLUME_EF_months = YTD_Achieved_VOLUME_EF_months + "+" + YTD_Achieved_VOLUME_EF_monthschk;
                }
                #endregion

                #region CPI
                //YTD_Cost_Avoid_Vs_CPI "_CPI_Effect"
                if (String.IsNullOrEmpty(YTD_Cost_Avoid_Vs_CPI_months))
                {
                    YTD_Cost_Avoid_Vs_CPI_monthschk = "ifnull(" + arrMonth[_counter] + "_CPI_Effect, 0)";
                    YTD_Cost_Avoid_Vs_CPI_months = "" + arrMonth[_counter] + "_CPI_Effect";
                }
                else
                {
                    YTD_Cost_Avoid_Vs_CPI_monthschk = "ifnull(" + arrMonth[_counter] + "_CPI_Effect, 0)";
                    YTD_Cost_Avoid_Vs_CPI_months = YTD_Cost_Avoid_Vs_CPI_months + "+" + YTD_Cost_Avoid_Vs_CPI_monthschk;
                }

                #endregion

                _counter++;
            }

            List<string> _arrMonth = new List<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string YTD_Target_months = string.Empty;
            string YTD_Acheived_months = string.Empty;
            string YTD_Target_months_in_year = string.Empty;

            string YTD_Acheived_months_in_year = string.Empty;
            string chkYTD_Target_months = "", chkYTD_Target_monthsNext = "";
            string chKYTD_Acheived_months = "", chkYTD_Acheived_months_in_year = "";

            int _counter1 = 0;
            int _counter2 = 0;

            while (_counter2 < projMonth)
            {
                if (String.IsNullOrEmpty(YTD_Target_months))
                {

                    chkYTD_Target_months = "ifnull(Target" + _arrMonth[_counter2] + ", 0)";
                    chkYTD_Target_monthsNext = "ifnull(TargetNex" + _arrMonth[_counter2] + ", 0)";
                    YTD_Target_months = chkYTD_Target_months;
                    YTD_Target_months_in_year = chkYTD_Target_monthsNext;
                }
                else
                {
                    chkYTD_Target_months = "ifnull(Target" + _arrMonth[_counter2] + ", 0)";
                    chkYTD_Target_monthsNext = "ifnull(TargetNex" + _arrMonth[_counter2] + ", 0)";
                    YTD_Target_months = YTD_Target_months + "+" + chkYTD_Target_months;
                    YTD_Target_months_in_year = YTD_Target_months_in_year + "+" + chkYTD_Target_monthsNext;
                }
                _counter2++;
            }


            while (_counter1 < projMonth)
            {


                if (String.IsNullOrEmpty(YTD_Acheived_months))
                {
                    chKYTD_Acheived_months = "ifnull(Ach" + _arrMonth[_counter1] + ", 0)";
                    chkYTD_Acheived_months_in_year = "ifnull(AchNex" + _arrMonth[_counter1] + ", 0)";
                    YTD_Acheived_months = chKYTD_Acheived_months;
                    YTD_Acheived_months_in_year = chkYTD_Acheived_months_in_year;
                }
                else
                {
                    chKYTD_Acheived_months = "ifnull(Ach" + _arrMonth[_counter1] + ", 0)";
                    chkYTD_Acheived_months_in_year = "ifnull(AchNex" + _arrMonth[_counter1] + ", 0)";
                    YTD_Acheived_months = YTD_Acheived_months + "+" + chKYTD_Acheived_months;
                    YTD_Acheived_months_in_year = YTD_Acheived_months_in_year + "+" + chkYTD_Acheived_months_in_year;

                }

                _counter1++;
            }


            List<GAIN.Models.vwheaderinitiative> lstGrid = new List<GAIN.Models.vwheaderinitiative>();
            string conn = clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]);
            conn = conn.Substring(conn.IndexOf("server"));
            conn = conn.Replace("'", "");


            if (ProjectYear == 2023) { condi = condi + "and IsShowCrossYear=1"; }
            if (Session["Maingrid"] == null || issave == 1)
            {
                using (MySqlConnection sql = new MySqlConnection(conn))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SP_GridMaster", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "SP_GridMaster";
                        cmd.Parameters.Add(new MySqlParameter("@inProjectYear", ProjectYear));
                        cmd.Parameters.Add(new MySqlParameter("@condi", condi));
                        cmd.Parameters.Add(new MySqlParameter("@YTD_Achieved_PRICE_EF_months", YTD_Achieved_PRICE_EF_months));
                        cmd.Parameters.Add(new MySqlParameter("@YTD_Achieved_VOLUME_EF_months", YTD_Achieved_VOLUME_EF_months));
                        cmd.Parameters.Add(new MySqlParameter("@N_YTD_Sec_PRICE_EF_months", N_YTD_Sec_PRICE_EF_months));
                        cmd.Parameters.Add(new MySqlParameter("@N_YTD_Sec_VOLUME_EF_months", N_YTD_Sec_VOLUME_EF_months));
                        cmd.Parameters.Add(new MySqlParameter("@N_YTD_ST_Total_EF_months", N_YTD_ST_Total_EF_months));
                        cmd.Parameters.Add(new MySqlParameter("@YTD_Cost_Avoid_Vs_CPI_months", YTD_Cost_Avoid_Vs_CPI_months));
                        cmd.Parameters.Add(new MySqlParameter("@YTD_Target_months", YTD_Target_months));
                        cmd.Parameters.Add(new MySqlParameter("@YTD_Acheived_months", YTD_Acheived_months));
                        cmd.Parameters.Add(new MySqlParameter("@YTD_Target_months_next", YTD_Target_months_in_year));
                        cmd.Parameters.Add(new MySqlParameter("@YTD_Acheived_months_next", YTD_Acheived_months_in_year));

                        sql.Open();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {

                            var dataTable = new DataTable();
                            dataTable.Load(reader);
                            //  condi = condi.Replace("a.", "");
                            //    dataTable = dataTable.Select(condi).CopyToDataTable();
                            //  Session["Maingrid"] = dataTable;
                            if (dataTable.Rows.Count > 0)
                            {
                                var serializedMyObjects = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
                                lstGrid = (List<vwheaderinitiative>)Newtonsoft.Json.JsonConvert.DeserializeObject(serializedMyObjects, typeof(List<vwheaderinitiative>));
                            }

                        }
                        sql.Close();
                    }
                }
            }
            else
            {
                var dataTable = new DataTable();

                dataTable = Session["Maingrid"] as DataTable;

                condi = condi.Replace("a.", "");
                dataTable = dataTable.Select(condi).CopyToDataTable();

                if (dataTable.Rows.Count > 0)
                {
                    var serializedMyObjects = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
                    lstGrid = (List<vwheaderinitiative>)Newtonsoft.Json.JsonConvert.DeserializeObject(serializedMyObjects, typeof(List<vwheaderinitiative>));
                }
            }

            return lstGrid;
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
                    Session["issaveupdtae"] = 1;
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
                        Session["issaveupdtae"] = 1;
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
            var profileData = Session["DefaultGAINSess"] as LoginSession;
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


            //ViewData["mregions"] = db.mregions.SqlQuery("SET GLOBAL sql_mode=(SELECT REPLACE(@@sql_mode,'ONLY_FULL_GROUP_BY','')); SELECT * FROM mregion group by RegionName").ToList();
            //ViewData["brandname"] = db.mbrands.SqlQuery("SELECT * FROM mbrand group by brandname").Where(c => c.isActive == "Y" && c.isDeleted == "N").ToList();
            //ViewData["msubregion"] = db.msubregions.SqlQuery("SELECT * FROM msubregion group by SubRegionName").Where(c => c.SubRegionName != null && c.SubRegionName != "").ToList();
            ////ViewData["mcluster"] = db.mclusters.SqlQuery("SELECT * FROM mcluster where ClusterName != \'\'").ToList();
            //ViewData["mcluster"] = db.mclusters.SqlQuery("SELECT * FROM mcluster group by ClusterName").Where(c => c.ClusterName != "").GroupBy(g => g.ClusterName).Select(s => new { ClusterName = s.Key }).ToList();
            //ViewData["mregional_office"] = db.mregional_office.SqlQuery("SELECT * FROM mregional_office").GroupBy(g => g.RegionalOffice_Name).Select(s => new { RegionalOffice_Name = s.Key }).ToList();
            //ViewData["CostControlSiteName"] = db.mcostcontrolsites.SqlQuery("SELECT * FROM mcostcontrolsite group by CostControlSiteName").Where(c => c.CostControlSiteName != "").ToList();
            //ViewData["CountryName"] = db.mcountries.SqlQuery("SELECT * FROM gain.mcountry group by CountryName").Where(c => c.CountryName != "").ToList();
            //ViewData["SubCountryName"] = db.msubcountries.SqlQuery("SELECT * FROM gain.msubcountry group by SubCountryName").Where(c => c.SubCountryName != "" && c.isActive == "Y").ToList();
            //ViewData["LegalEntityName"] = db.mlegalentities.SqlQuery("SELECT * FROM gain.mlegalentity group by LegalEntityName").GroupBy(g => g.LegalEntityName).Select(s => new { LegalEntityName = s.Key }).ToList();
            //ViewData["SavingTypeName"] = db.msavingtypes.SqlQuery("SELECT * FROM gain.msavingtype group by SavingTypeName").Where(c => c.isActive == "Y").ToList();
            //ViewData["CostTypeName"] = db.mcosttypes.SqlQuery("SELECT * FROM gain.mcosttype group by CostTypeName").Where(c => c.isActive == "Y").ToList();
            //ViewData["SubCostName"] = db.msubcosts.SqlQuery("SELECT * FROM gain.msubcost group by SubCostName").Where(c => c.isActive == "Y").ToList();
            //ViewData["ActionTypeName"] = db.mactiontypes.SqlQuery("SELECT * FROM gain.mactiontype group by ActionTypeName").Where(c => c.isActive == "Y").ToList();
            //ViewData["SynImpactName"] = db.msynimpacts.SqlQuery("SELECT * FROM gain.msynimpact group by SynImpactName").Where(c => c.isActive == "Y").ToList();
            //ViewData["Status"] = db.mstatus.SqlQuery("SELECT * FROM gain.mstatus group by Status").Where(c => c.isActive == "Y").ToList();
            //ViewData["portName"] = db.mports.SqlQuery("SELECT * FROM gain.mport group by PortName").ToList();
            //ViewData["SourceCategoryName"] = db.msourcecategories.SqlQuery("SELECT * FROM gain.msourcecategory group by categoryname").ToList();
            var projYear = profileData.ProjectYear;



            ViewData["mregions"] = db.mregions.Where(c => c.InitYear == projYear).ToList();
            ViewData["brandname"] = db.mbrands.Where(c => c.isActive == "Y" && c.isDeleted == "N" && c.InitYear == projYear).ToList();
            ViewData["msubregion"] = db.msubregions.Where(c => c.SubRegionName != null && c.SubRegionName != "" && c.InitYear == projYear).ToList();
            //ViewData["mcluster"] = db.mclusters.SqlQuery("SELECT * FROM mcluster where ClusterName != \'\'").ToList();
            ViewData["mcluster"] = db.mclusters.Where(c => c.ClusterName != "" && c.InitYear == projYear).GroupBy(g => g.ClusterName).Select(s => new { ClusterName = s.Key }).ToList();
            ViewData["mregional_office"] = db.mregional_office.SqlQuery("SELECT * FROM mregional_office where InitYear=" + projYear + "").GroupBy(g => g.RegionalOffice_Name).Select(s => new { RegionalOffice_Name = s.Key }).ToList();
            ViewData["CostControlSiteName"] = db.mcostcontrolsites.Where(c => c.CostControlSiteName != "" && c.InitYear == projYear).ToList();
            ViewData["CountryName"] = db.mcountries.Where(c => c.CountryName != "" && c.InitYear == projYear).ToList();
            ViewData["SubCountryName"] = db.msubcountries.Where(c => c.SubCountryName != "" && c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["LegalEntityName"] = db.mlegalentities.Where(c => c.InitYear == projYear).GroupBy(g => g.LegalEntityName).Select(s => new { LegalEntityName = s.Key }).ToList();
            ViewData["SavingTypeName"] = db.msavingtypes.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["CostTypeName"] = db.mcosttypes.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["SubCostName"] = db.msubcosts.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["ActionTypeName"] = db.mactiontypes.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["SynImpactName"] = db.msynimpacts.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["Status"] = db.mstatus.Where(c => c.isActive == "Y" && c.InitYear == projYear).ToList();
            ViewData["portName"] = db.mports.Where(c => c.InitYear == projYear).ToList();
            ViewData["SourceCategoryName"] = db.msourcecategories.Where(c => c.InitYear == projYear).ToList();

            //ViewData["mregions"] = db.mregions.ToList();
            //ViewData["brandname"] = db.mbrands.Where(c => c.isActive == "Y" && c.isDeleted == "N").ToList();
            //ViewData["msubregion"] = db.msubregions.Where(c => c.SubRegionName != null && c.SubRegionName != "").ToList();
            //ViewData["mcluster"] = db.mclusters.Where(c => c.ClusterName != "").GroupBy(g => g.ClusterName).Select(s => new { ClusterName = s.Key }).ToList();
            //ViewData["mregional_office"] = db.mregional_office.SqlQuery("SELECT * FROM mregional_office").GroupBy(g => g.RegionalOffice_Name).Select(s => new { RegionalOffice_Name = s.Key }).ToList();
            //ViewData["CostControlSiteName"] = db.mcostcontrolsites.Where(c => c.CostControlSiteName != "").ToList();
            //ViewData["CountryName"] = db.mcountries.Where(c => c.CountryName != "").ToList();
            //ViewData["SubCountryName"] = db.msubcountries.Where(c => c.SubCountryName != "" && c.isActive == "Y").ToList();
            //ViewData["LegalEntityName"] = db.mlegalentities.GroupBy(g => g.LegalEntityName).Select(s => new { LegalEntityName = s.Key }).ToList();
            //ViewData["SavingTypeName"] = db.msavingtypes.Where(c => c.isActive == "Y").ToList();
            //ViewData["CostTypeName"] = db.mcosttypes.Where(c => c.isActive == "Y").ToList();
            //ViewData["SubCostName"] = db.msubcosts.Where(c => c.isActive == "Y").ToList();
            //ViewData["ActionTypeName"] = db.mactiontypes.Where(c => c.isActive == "Y").ToList();
            //ViewData["SynImpactName"] = db.msynimpacts.Where(c => c.isActive == "Y").ToList();
            //ViewData["Status"] = db.mstatus.Where(c => c.isActive == "Y").ToList();
            //ViewData["portName"] = db.mports.ToList();
            //ViewData["SourceCategoryName"] = db.msourcecategories.ToList();

            Session["issaveupdtae"] = 1;
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
            var projYear = profileData.ProjectYear;
            if (profileData.ProjectYear < 2023)
                profileData.ProjectYear = 2022;

            if (profileData.UserType == 3) //agency
            {
                //var cntrytext = profileData.CountryID.Replace("|", "','");

                var cntrytext = profileData.subcountry_right.Replace("|", "','");
                int lencntrytext = cntrytext.Length;
                cntrytext = "(" + cntrytext.Substring(2, (lencntrytext - 4)) + ")";

                string sQuery = string.Empty;

                if (cntrytext.Contains("'ALL'"))
                {
                    sQuery = "select id,CountryID,SubCountryName,CountryCode,isActive , Inityear from msubcountry where SubCountryName is not null and isActive = 'Y' and  Inityear = " + projYear + "";
                }
                else
                {
                    sQuery = "select id,CountryID,SubCountryName,CountryCode,isActive , Inityear from msubcountry where SubCountryName is not null and isActive = 'Y' and SubCountryName in " + cntrytext + " and  Inityear = " + projYear + "";
                }

                var cntryid = db.msubcountries.SqlQuery(sQuery).ToList();

                var cntryidcondition = "";
                for (var i = 0; i < cntryid.Count(); i++)
                {
                    cntryidcondition += cntryid[i].id.ToString() + ",";
                }
                cntryidcondition = cntryidcondition.Substring(0, cntryidcondition.Length - 1);
                if (cntryid.Count() > 0)
                    where += cntryidcondition + ")";
                else
                    where += "";
                //where += " and a.CountryID in " + cntryidcondition + ")";

            }

            //db.Configuration.ProxyCreationEnabled = false;
            //var model = db.msubcountries.ToList();

            List<SubCountryList> model = new List<SubCountryList>();
            model = db.msubcountries.ToList().Where(c => c.InitYear == projYear).Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName, InitYear = s.InitYear }).ToList();
            if (GetInfo.Id != 0)
            {
                model = db.msubcountries.Where(c => c.id == GetInfo.Id && c.InitYear == projYear && c.isActive == "Y" && !(string.IsNullOrEmpty(c.SubCountryName))).Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                //model = db.msubcountries.SqlQuery("select id,CountryID,SubCountryName,CountryCode,isActive from msubcountry where SubCountryName is not null and isActive = 'Y' and id = " + GetInfo.Id + " ").ToList();
            }
            else
            {
                if (profileData.UserType == 3) //agency
                {
                    //model = db.msubcountries.Where(c => !string.IsNullOrEmpty(c.SubCountryName) && c.CountryCode   && c.isActive == "Y").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                    // model = db.msubcountries.SqlQuery("select id,CountryID,SubCountryName,CountryCode,isActive ,InitYear from msubcountry where SubCountryName is not null and isActive = 'Y' and InitYear = " + projYear + " and id in ( " + where + " ").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                    if (string.IsNullOrEmpty(where))
                        model = db.msubcountries.SqlQuery("select id,CountryID,SubCountryName,CountryCode,isActive ,InitYear from msubcountry where SubCountryName is not null and isActive = 'Y' and InitYear = " + projYear + "").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                    else

                        model = db.msubcountries.SqlQuery("select id,CountryID,SubCountryName,CountryCode,isActive ,InitYear from msubcountry where SubCountryName is not null and isActive = 'Y' and InitYear = " + projYear + " and id in ( " + where + " ").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
                    // model = db.msubcountries.Where(c => !string.IsNullOrEmpty(c.SubCountryName) && c.isActive == "Y" && c.InitYear == projYear).Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();

                }
                else
                {
                    model = db.msubcountries.Where(c => !string.IsNullOrEmpty(c.SubCountryName) && c.isActive == "Y" && c.InitYear == projYear).Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
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

        // File upload functionality
        public bool isUserSubCountry(string subCountryDesc)
        {
            bool isUserSubCountry = false;
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            string usercountryIds = (profileData.UserType == 3) ? profileData.subcountry_right : "|ALL|";
            string[] arrUserCountry = objFlatFileHelper.GetUserCountries(usercountryIds);
            var userCntry = arrUserCountry.ToList().Where(ucntry => ucntry.ToLower() == subCountryDesc.ToLower());
            var matchingSubCntry = lstSubCountryBrand.Where(uSubcntry => uSubcntry.subCountryName.ToLower() == subCountryDesc.ToLower()).FirstOrDefault();

            if ((profileData.subcountry_right == "|ALL|" && matchingSubCntry != null) || (profileData.UserType != 3))
                isUserSubCountry = true;
            else
            {
                if (userCntry.Count() > 0)
                    isUserSubCountry = true;
            }
            return isUserSubCountry;
        }
        public bool isValidBrand(string subCountryDesc, string brandDesc)
        {
            bool isValidBrand = false;
            var subCountryBrand = lstSubCountryBrand.Where(sc => sc.subCountryName.ToLower() == subCountryDesc.ToLower() && sc.brandName.ToLower() == brandDesc.ToLower());
            isValidBrand = (subCountryBrand.Count() > 0) ? true : false;
            return isValidBrand;
        }
        public void SetSubCountryBrand()
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            string usercountryIds = (profileData.UserType == 3) ? profileData.subcountry_right : "|ALL|";
            string[] arrUserCountry = objFlatFileHelper.GetUserCountries(usercountryIds);
            string subCntryCondn = "(";
            for (int i = 0; i < arrUserCountry.Length; i++)
            {
                subCntryCondn += "'" + arrUserCountry[i] + "'";
                subCntryCondn += (i < arrUserCountry.Length - 1) ? "," : "";
            }
            subCntryCondn += ")";

            string sqlQuery = "SELECT mb.brandname As brandName, mbc.brandid As brandId, mbc.subcountryid As subCountryId, ms.SubCountryName,ms.CountryCode FROM mbrandcountry mbc inner join "
                                + " mbrand mb on mbc.brandid = mb.id Inner join msubcountry ms on ms.id = mbc.subcountryid"
                                + " Where ms.isActive = 'Y' and mbc.inityear = " + System.DateTime.Now.Year;

            sqlQuery += ((usercountryIds != "|ALL|") && (profileData.UserType != 1 || profileData.UserType != 2)) ? " And ms.SubCountryName in " + subCntryCondn : "";
            lstSubCountryBrand = db.Database.SqlQuery<SubCountryBrand>(sqlQuery).ToList();
        }

        public bool isValidTypeCostSubCost(string strMatchingText, string field, string itemCat, string initType)
        {
            bool isValidItem = false;
            if (field != null)
            {
                switch (field)
                {
                    case "initType":
                        {
                            var matchingItem = lstInitTypeCostSubCosts.Where(item => item.initType.ToLower() == strMatchingText.ToLower()).FirstOrDefault();
                            isValidItem = (matchingItem != null) ? true : false;
                            break;
                        }
                    case "itemCategory":
                        {
                            var matchingItem = lstInitTypeCostSubCosts.Where(item => item.itemCategory.ToLower() == strMatchingText.ToLower() && item.initType.ToLower() == initType.ToLower()).FirstOrDefault();
                            isValidItem = (matchingItem != null) ? true : false;
                            break;
                        }
                    case "subCost":
                        {
                            var matchingItem = lstInitTypeCostSubCosts.Where(item => item.subCostName.ToLower() == strMatchingText.ToLower() && item.itemCategory.ToLower() == itemCat.ToLower()).FirstOrDefault();
                            isValidItem = (matchingItem != null) ? true : false;
                            break;
                        }
                }
            }
            return isValidItem;
        }

        public void setInitTypeCostSubCost()
        {
            int projectYear = System.DateTime.Now.Year;
            string strQuery = "SELECT ms.id As InitTypeId, ms.SavingTypeName As InitType, mct.CostTypeName As ItemCategory, mct.id As ItemCategoryId, b.id As SubCostId," +
                "b.SubCostName FROM t_subcostbrand a"
+ " Inner JOIN msubcost b ON a.subcostid = b.id  Inner Join mcostType mct on mct.id = a.costtypeid"
 + " Inner join msavingtype ms on ms.id = a.savingtypeid"
 + " WHERE b.isActive = 'Y' And ms.InitYear = " + projectYear.ToString() + " And ms.isActive = 'Y' And a.InitYear = " + projectYear.ToString()
 + " And mct.isActive = 'Y' And  mct.InitYear = " + projectYear.ToString() + " Group by InitTypeId, InitType, ItemCategoryId, ItemCategory, SubCostId, b.SubCostName Order by ms.SavingTypeName, mct.CostTypeName, b.SubCostName";

            lstInitTypeCostSubCosts = db.Database.SqlQuery<InitTypeCostSubCost>(strQuery).ToList();
        }

        public void setInitiativeStatus(int initYear)
        {
            string strQry = "Select id, status From mstatus Where InitYear = " + initYear + " And isActive = 'Y'";
            lstInitiativeStatus = db.Database.SqlQuery<mInitiativeStatus>(strQry).ToList();
        }
        public List<MonthlyCPIValues> GetMonthlyCPIValuesList(string subCountryDesc, int initYear)
        {
            string strQry = "Select msc.SubCountryName, mcpi.* From msubcountry msc"
   + " inner join mcountry mc on mc.id = msc.countryId Inner join"
   + " mcpi on mcpi.mcountry_id = msc.CountryID"
   + " Where mcpi.InitYear = " + initYear + " And msc.SubCountryName = '" + subCountryDesc + "'";
            List<MonthlyCPIValues> _mCPI = db.Database.SqlQuery<MonthlyCPIValues>(strQry).ToList();
            return _mCPI;
        }
       
        private string GetGeneralRemarks(string sInitNumber, string subCountry, string brand, string sConfidential, string sInitiativeStatus, DataRow dataRow,
            int userType)
        {
            string remarks = string.Empty;
            bool isUserSubCountry = false;
            bool isValidBrand = false;
            List<msubcountry> lstmSubCountry = new List<msubcountry>();

            if (!string.IsNullOrEmpty(sInitNumber))
            {
                tInitRecord = db.t_initiative.Where(init => init.InitNumber == sInitNumber).FirstOrDefault();
            }
            // Checks for init num, if empty then new, else, check from entity, if not exist, invalid init num.
            remarks += (string.IsNullOrEmpty(sInitNumber)) ? "" :
                (tInitRecord != null) ? "" : " Invalid Init Number,";
            remarks += (string.IsNullOrEmpty(subCountry)) ? " Invalid Subcountry." : "";
            if (subCountry != "")
            {
                isUserSubCountry = this.isUserSubCountry(subCountry);
                remarks += (!isUserSubCountry) ? " Subcountry not mapped to user," : "";
            }
            if (!string.IsNullOrEmpty(sInitNumber))
            {
                var subCountryItem = lstSubCountryBrand.Where(item => item.subCountryName.ToLower().Trim() == subCountry.ToLower().Trim()).FirstOrDefault();
                if ((subCountryItem != null && tInitRecord != null) && (subCountryItem.subCountryId != tInitRecord.SubCountryID))
                {
                    remarks += " Subcountry cannot be changed on edit mode,";
                }
            }
            remarks += (string.IsNullOrEmpty(brand)) ? " Invalid Brand." : "";

            if (isUserSubCountry && (subCountry != "" && brand != ""))
            {
                isValidBrand = this.isValidBrand(subCountry, brand);
                remarks += (!isValidBrand) ? " Brand not mapped with subcountry" : "";
            }
            if (sConfidential != "N" && sConfidential != "Y")
                remarks += " Invalid confidential.";

            if (sInitiativeStatus != "cancelled" && sInitiativeStatus != "ongoing" && sInitiativeStatus != "work in progress")
                remarks += " Invalid Initiative Status.";

            // Validation for Agency not to change init status to Work in progress
            List<t_initiative> lstExistingInits = lstOOInitiatives;
            lstExistingInits.Concat(lstSCMInitiatives);
            if (userType == 3)
            {
                if (sInitiativeStatus.ToLower() == "work in progress")
                {
                    var initStatusCheck = lstExistingInits.Where(tInit => tInit.InitNumber == sInitNumber
                    && tInit.InitStatus != objFlatFileHelper.getInitStatus(sInitiativeStatus, lstInitiativeStatus)).ToList();
                    if (initStatusCheck.Count > 0)
                    {
                        remarks += "Agency user not authorized to change to Work in progress,";
                    }
                }
            }
            remarks += (!this.isValidTypeCostSubCost(Convert.ToString(dataRow["TypeOfInitiative"]), "initType", "", "")) ?
                " Invalid Initiative type," : "";
            remarks += (!this.isValidTypeCostSubCost(Convert.ToString(dataRow["ItemCategory"]), "itemCategory", "", Convert.ToString(dataRow["TypeOfInitiative"]))) ?
                " Invalid item category," : "";
            remarks += (!this.isValidTypeCostSubCost(Convert.ToString(dataRow["SubCostItemImpacted"]), "subCost", Convert.ToString(dataRow["ItemCategory"]), "")) ?
                " Invalid sub cost," : "";
            remarks += (string.IsNullOrEmpty(dataRow["StartMonth"].ToString())) ?
                 " Invalid Start month." : "";
            remarks += (string.IsNullOrEmpty(dataRow["EndMonth"].ToString())) ?
                 " Invalid End month." : "";
            return remarks;
        }
        private void SetInitiativeList(int initYear)
        {
            long ooTypeId = db.mactiontypes.Where(action => action.ActionTypeName == ActionType.ooActionType
                            && action.isActive == "Y" && action.InitYear == initYear).ToList().FirstOrDefault().id;
            long scmTypeId = db.mactiontypes.Where(action => action.ActionTypeName == ActionType.scmType
           && action.isActive == "Y" && action.InitYear == initYear).ToList().FirstOrDefault().id;

            // Getting only cross yr inits 2022-2023 only for Opern Efficiency

            lstOOInitiatives = db.t_initiative.Where(tInit =>
                  tInit.ActionTypeID == ooTypeId && ((tInit.ProjectYear == initYear)
                  || (tInit.EndMonth.Value.Year == initYear))
                  ).ToList();

            lstSCMInitiatives = db.t_initiative.Where(tInit =>
                  tInit.ActionTypeID == scmTypeId && tInit.ProjectYear == initYear).ToList();
        }
        private void setActionTypeList(int initYear)
        {
            lstActionType = db.mactiontypes.Where(action =>
                              action.isActive == "Y" && action.InitYear == initYear).ToList();
        }
        private List<mport> getPortList(int initYear) {
            List<mport> lstMports = new List<mport>();
            string sqlQry = "Select id, PortName, InitYear From mport Where inityear = 2023";
            lstMports = db.Database.SqlQuery<mport>(sqlQry).ToList();
            return lstMports;
        }

        private void deleteFiles30Days(string path)
        {
            string errFilePath = path + "ErrorExcel";
            string updatedFilePath = path + "UpdatedInit";

            if (System.IO.Directory.Exists(path))
                Directory.GetFiles(path).Select(f => new FileInfo(f))
          .Where(f => f.LastWriteTime < DateTime.Now.AddMonths(-1))
          .ToList().ForEach(f => f.Delete());

            if (System.IO.Directory.Exists(errFilePath))
                Directory.GetFiles(errFilePath).Select(f => new FileInfo(f))
       .Where(f => f.LastWriteTime < DateTime.Now.AddMonths(-1))
       .ToList().ForEach(f => f.Delete());

            if (System.IO.Directory.Exists(updatedFilePath))
                Directory.GetFiles(updatedFilePath).Select(f => new FileInfo(f))
         .Where(f => f.LastWriteTime < DateTime.Now.AddMonths(-1))
         .ToList().ForEach(f => f.Delete());
        }

        // File upload functionality
        public ActionResult UploadFile(HttpPostedFileBase fileBase)
        {
            try
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];
                IActionTypeValidation validationRemarks = null;
                IActionTypeCalculation actionTypeCalculation = null;
                var profileData = Session["DefaultGAINSess"] as LoginSession;
                int userType = profileData.UserType;
                ResultCount resultCount = null;
                int initYear = System.DateTime.Now.Year;
                bool isActionTypeChanged = false;
                bool isValidActionType = false;
                List<int> lstValidRowIndexes = new List<int>();
                int intValidIndex = -1;
                List<t_initiative> lstExistingInits = new List<t_initiative>();
                List<t_initiative> lstMergeDBRows = new List<t_initiative>();
                t_initiative initRecord = new t_initiative();
                #region 

                String _path = Server.MapPath("~/UploadedFiles/");
                // create the uploads folder if it doesn't exist
                if (!System.IO.Directory.Exists(_path))
                    Directory.CreateDirectory(_path);

                // Delete Files Uploaded 30 days before
                this.deleteFiles30Days(_path);

                string fileName = Path.GetRandomFileName() + "_" + file.FileName;
                string _inputpath = Path.Combine(_path, fileName);
                // save the file
                file.SaveAs(_inputpath);

                // Will set the subcountry brand list
                this.SetSubCountryBrand();

                //Will set the InitTypeCostSubCost
                this.setInitTypeCostSubCost();
                // Set the initiative status list
                this.setInitiativeStatus(initYear);
                // Setting the OO and SCM Initiatives list as per the inityear
                this.SetInitiativeList(initYear);
                // Setting ActionTypes
                this.setActionTypeList(initYear);
                // Setting port types                
                List<mport> lstPorts = this.getPortList(initYear);

                // Error Excel file
                string outExcelfileName = "errorExcel_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                int successCount = 0, errCount = 0, updateCount = 0;
                if (!System.IO.Directory.Exists(_path + "ErrorExcel\\"))
                    System.IO.Directory.CreateDirectory(_path + "ErrorExcel\\");
                string outputExcelPath = Path.Combine(_path + "ErrorExcel\\", outExcelfileName);

                // Updated Initiatives txt file.
                string updatedInitFile = "updatedInit_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

                if (!System.IO.Directory.Exists(_path + "UpdatedInit\\"))
                    System.IO.Directory.CreateDirectory(_path + "UpdatedInit\\");
                string updatedInitPath = Path.Combine(_path + "UpdatedInit\\", updatedInitFile);
                TextWriter twFileUpdatedInit = null;

                ResultCount resultCountobj = new ResultCount();
                DataTable dtExcelInitiatives = FlatFileHelper.ConvertExcelToDataTable(_inputpath, "Sheet$");

                using (FileStream stream = new FileStream(_inputpath, FileMode.Open))
                {
                    Workbook workbook = new Workbook();
                    workbook.LoadDocument(stream);
                    Worksheet worksheet = workbook.Worksheets[0];
                    RowCollection rows = worksheet.Rows;
                    CellRange dataRange = worksheet.GetUsedRange();
                    DataRow drFirstRow = dtExcelInitiatives.Rows[0];
                    drFirstRow.Delete();
                    dtExcelInitiatives.AcceptChanges();

                    if (!(dtExcelInitiatives.Columns.Count >= 91 && dtExcelInitiatives.Columns.Count < 93))
                    {
                        resultCountobj.validationMsg = "Please upload valid excel template";
                        objFlatFileHelper.DisposeFile(_inputpath);
                        return Content(JsonConvert.SerializeObject(resultCountobj));
                    }
                    else
                    {
                        // Column names space removal
                        foreach (DataColumn col in dtExcelInitiatives.Columns)
                        {
                            col.ColumnName = col.ColumnName.Replace(" ", "");
                            col.ColumnName = col.ColumnName.Replace("(", "");
                            col.ColumnName = col.ColumnName.Replace(")", "");
                            col.ColumnName = col.ColumnName.Replace("/", "");
                        }
                        string[] arrPrevCols = new string[] { "F36", "F40", "F44", "F48", "F52", "F56", "F60", "F64", "F68", "F72", "F76", "F80",
            "ActualsVolumesN-1", "$SPENDN-1", "$SPENDN"};

                        string[] arrNewCols = new string[] { "JanActualVolumes", "FebActualVolumes", "MarActualVolumes", "AprActualVolumes",
                "MayActualVolumes" , "JunActualVolumes","JulActualVolumes", "AugActualVolumes", "SepActualVolumes", "OctActualVolumes",
                "NovActualVolumes", "DecActualVolumes", "InputActualsVolumesNmin1", "SpendNmin1","SpendN"};

                        for (int i = 0; i < arrPrevCols.Length; i++)
                        {
                            dtExcelInitiatives.Columns[dtExcelInitiatives.Columns.IndexOf(arrPrevCols[i])].ColumnName = arrNewCols[i];
                        }
                        // Setting OO Columns Target and Achieved cols for current yr
                        string[] monthCols = new string[]{
                            "January" + initYear.ToString(), "February" + initYear.ToString(), "March" + initYear.ToString(), "April" + initYear.ToString(), "May" + initYear.ToString(),
                        "June" + initYear.ToString(), "July" + initYear.ToString(), "August" + initYear.ToString(), "September" + initYear.ToString(), "October" + initYear.ToString(), "November" + initYear.ToString(), "December" + initYear.ToString(),
                       "F35", "F39", "F43", "F47", "F51", "F55", "F59", "F63", "F67", "F71", "F75", "F79"
                       };

                        string[] monthColsNew = new string[] { "TargetJan", "TargetFeb", "TargetMar",
                        "TargetApr", "TargetMay", "TargetJun", "TargetJul", "TargetAug", "TargetSep", "TargetOct",
                        "TargetNov", "TargetDec", "AchJan", "AchFeb", "AchMar", "AchApr", "AchMay", "AchJun", "AchJul",
                        "AchAug", "AchSep", "AchOct", "AchNov", "AchDec"};

                        for (int i = 0; i < monthCols.Length; i++)
                        {
                            dtExcelInitiatives.Columns[dtExcelInitiatives.Columns.IndexOf(monthCols[i])].ColumnName = monthColsNew[i];
                        }
                        dtExcelInitiatives.AcceptChanges();

                        // Gets the valid rows
                        var newInitiatives = dtExcelInitiatives.AsEnumerable()
                            .Where(myRow =>
                            (String.IsNullOrEmpty(myRow.Field<string>("InitNumber")))
                         ).ToList();

                        DataTable dtExisting = objFlatFileHelper.GetUpdatedRows(dtExcelInitiatives, lstOOInitiatives, lstSCMInitiatives, lstInitiativeStatus,
                            lstSubCountryBrand, lstPorts, lstInitTypeCostSubCosts, lstActionType, userType);

                        if (newInitiatives.Count > 0)
                            dtExisting.Merge(newInitiatives.CopyToDataTable());
                        var lstUnchanged = dtExcelInitiatives.AsEnumerable().Except(dtExisting.AsEnumerable(), DataRowComparer.Default).ToList();
                        DataTable dtUnchanged = (lstUnchanged.Count > 0) ? lstUnchanged.CopyToDataTable() : null;
                        if (dtUnchanged != null && dtUnchanged.Rows.Count > 0)
                        {
                            for (int i = dtUnchanged.Rows.Count - 1; i >= 0; i--)
                            {
                                intValidIndex = dtExcelInitiatives.Rows.IndexOf(
                                  (dtExcelInitiatives.AsEnumerable().Where(excelInit => Convert.ToString(excelInit["InitNumber"]) ==
                                   Convert.ToString(dtUnchanged.Rows[i]["InitNumber"]))).FirstOrDefault());
                                //lstValidRowIndexes.Add(intValidIndex);
                                (worksheet.Rows[(intValidIndex + 2)]).Delete();
                            }
                        }
                        if (dtExisting != null && dtExisting.Rows.Count > 0)
                        {
                            DataTable dtInit = dtExisting;
                            dtInit.Columns.Add("ProjectYear", typeof(int));
                            dtInit.Columns.Add("dbFlag", typeof(String));
                            dtInit.Columns.Add("isProcurement", typeof(System.Int32));
                            // Adding next yr columns for OO Init types in cross yr scenarios.
                            dtInit.Columns.Add("TargetNexJan", typeof(double));
                            dtInit.Columns.Add("TargetNexFeb", typeof(double));
                            dtInit.Columns.Add("TargetNexMar", typeof(double));
                            dtInit.Columns.Add("TargetNexApr", typeof(double));
                            dtInit.Columns.Add("TargetNexMay", typeof(double));
                            dtInit.Columns.Add("TargetNexJun", typeof(double));
                            dtInit.Columns.Add("TargetNexJul", typeof(double));
                            dtInit.Columns.Add("TargetNexAug", typeof(double));
                            dtInit.Columns.Add("TargetNexSep", typeof(double));
                            dtInit.Columns.Add("TargetNexOct", typeof(double));
                            dtInit.Columns.Add("TargetNexNov", typeof(double));
                            dtInit.Columns.Add("TargetNexDec", typeof(double));

                            dtInit.Columns.Add("AchNexJan", typeof(double));
                            dtInit.Columns.Add("AchNexFeb", typeof(double));
                            dtInit.Columns.Add("AchNexMar", typeof(double));
                            dtInit.Columns.Add("AchNexApr", typeof(double));
                            dtInit.Columns.Add("AchNexMay", typeof(double));
                            dtInit.Columns.Add("AchNexJun", typeof(double));
                            dtInit.Columns.Add("AchNexJul", typeof(double));
                            dtInit.Columns.Add("AchNexAug", typeof(double));
                            dtInit.Columns.Add("AchNexSep", typeof(double));
                            dtInit.Columns.Add("AchNexOct", typeof(double));
                            dtInit.Columns.Add("AchNexNov", typeof(double));
                            dtInit.Columns.Add("AchNexDec", typeof(double));

                            dtInit.Columns.Add("TargetNY", typeof(double));
                            
                            List<InitiativeCalcs> lstInitiativeCalcs = new List<InitiativeCalcs>();
                            DataTable dtValidInit = dtInit.Clone();

                            // Perform Mandatory validation
                            if (dtInit.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtInit.Rows.Count; i++)
                                {
                                    // Generic Validations
                                    string remarks = string.Empty;
                                    DateTime dtStartMonth = new DateTime();
                                    DateTime dtEndMonth = new DateTime();
                                    string sInitNumber = Convert.ToString(dtInit.Rows[i]["InitNumber"]).Trim();
                                    string subCountry = Convert.ToString(dtInit.Rows[i]["SubCountry"].ToString());
                                    string brand = Convert.ToString(dtInit.Rows[i]["Brand"].ToString());
                                    string sConfidential = dtInit.Rows[i]["Confidential"] != null ? dtInit.Rows[i]["Confidential"].ToString().ToUpper() : "";
                                    string sInitiativeStatus = dtInit.Rows[i]["InitiativeStatus"] != null ? dtInit.Rows[i]["InitiativeStatus"].ToString().ToLower() : "";

                                    // Duplicate Initnumber validation
                                    if (sInitNumber != "")
                                    {
                                        remarks += (dtExcelInitiatives.AsEnumerable().Where(init =>
                                        Convert.ToString(init["InitNumber"].ToString().Trim().ToUpper()) == sInitNumber.Trim().ToUpper()).Count() > 1) ?
                                        "Duplicate Initiatives," : "";
                                    }
                                    // Get General validation for all action types
                                    remarks += this.GetGeneralRemarks(sInitNumber, subCountry, brand, sConfidential,
                                   sInitiativeStatus, dtInit.Rows[i], userType);

                                    // Get the actionType and validations based on action type
                                    string actionType =
                                        objFlatFileHelper.GetActionType(Convert.ToString(dtInit.Rows[i]["ActionType"]));
                                    isValidActionType = (actionType.ToLower() == ActionType.ooActionType.ToLower() ||
                                        actionType.ToLower() == ActionType.scmType.ToLower()) ? true : false;

                                    if (sInitNumber != "")
                                    {
                                        lstMergeDBRows = lstOOInitiatives.Concat(lstSCMInitiatives).ToList();
                                        isActionTypeChanged = lstMergeDBRows.AsEnumerable().Where(
                                            tInit => tInit.InitNumber == sInitNumber
                                                && tInit.ActionTypeID != objFlatFileHelper.getActionTypeId(actionType.ToLower(), lstActionType))
                                            .Count() > 0 ? true : false;
                                    }
                                    //Datetime check
                                    remarks += (!(DateTime.TryParse(Convert.ToString(dtInit.Rows[i]["StartMonth"]), out dtStartMonth))) ? " Please enter a valid start date," : "";
                                    remarks += (!(DateTime.TryParse(Convert.ToString(dtInit.Rows[i]["EndMonth"]), out dtEndMonth))) ? " Please enter a valid end date" : "";
                                    if (!isActionTypeChanged)
                                    {
                                        if (isValidActionType)
                                        {
                                            if (actionType.ToLower() == ActionType.ooActionType.ToLower())
                                            {
                                                // OO Type Validation
                                                lstExistingInits = lstOOInitiatives;
                                                validationRemarks = new OperationEfficiency();
                                            }
                                            else if (actionType.ToLower() == ActionType.scmType.ToLower())
                                            {
                                                // SCM Type validations
                                                lstExistingInits = lstSCMInitiatives;
                                                validationRemarks = new SupplyContractMonitor();
                                            }
                                            remarks += validationRemarks.GetValidationRemarks(dtInit.Rows[i], dtStartMonth,
                                                dtEndMonth, initYear, userType, lstExistingInits, lstInitTypeCostSubCosts, lstInitiativeStatus, tInitRecord);

                                            remarks += validationRemarks.GetCrossYrRemarks(tInitRecord, dtStartMonth, dtEndMonth, initYear);
                                        }
                                        else
                                        {
                                            remarks += " Invalid Action Type,";
                                        }
                                    }
                                    else
                                    {
                                        remarks += " Change in Action type is not allowed,";
                                    }
                                    if ((!(DateTime.TryParse(Convert.ToString(dtInit.Rows[i]["StartMonth"]), out dtStartMonth)))
                                        || (!(DateTime.TryParse(Convert.ToString(dtInit.Rows[i]["EndMonth"]), out dtEndMonth))))
                                        remarks += " Please enter a valid start and end date,";
                                    else
                                    {
                                        //remarks += (objFlatFileHelper.isValidMonth(dtStartMonth)) == false ? " Start year should be from " + initYear + " onwards." : "";
                                        remarks += (objFlatFileHelper.isValidMonth(dtEndMonth, initYear)) == false ? " End year should be from " + initYear + " onwards." : "";
                                        remarks += (objFlatFileHelper.isValidEndMonth(dtStartMonth, dtEndMonth)) == false ?
                                            " Start month should be lesser than end month and difference should be less than/ equal to 12 months" : "";
                                    }
                                    if (remarks != "")
                                    {
                                        (worksheet.Cells[1, 91]).Value = "Remarks";
                                        (worksheet.Cells[(i + 2), 91]).Value = remarks;
                                        (worksheet.Cells[(i + 2), 90]).Value = "";
                                        (worksheet.Cells[(i + 2), 89]).Value = "";
                                        (worksheet.Cells[(i + 2), 88]).Value = "";
                                        (worksheet.Rows[(i + 2)]).FillColor = System.Drawing.Color.Yellow;
                                        errCount++;
                                    }
                                    else
                                    {
                                        List<MonthlyCPIValues> lstMonthlyCPIValues = null;
                                        DataRow drRow = dtInit.Rows[i];
                                        drRow["dbFlag"] = (!string.IsNullOrEmpty(sInitNumber)) ? "U" : "I";

                                        // Valid Rows
                                        if (actionType.ToLower() == ActionType.ooActionType.ToLower())
                                        {
                                            drRow["isProcurement"] = 0;
                                            actionTypeCalculation = new OperationEfficiency();
                                        }
                                        else if (actionType.ToLower() == ActionType.scmType.ToLower())
                                        {
                                            drRow["isProcurement"] = 1;                                           
                                            lstMonthlyCPIValues = this.GetMonthlyCPIValuesList(subCountry, initYear);
                                            actionTypeCalculation = new SupplyContractMonitor();
                                        }
                                        // Comments for HO/ Agency/ RPOC assignments
                                        if (!string.IsNullOrEmpty(sInitNumber))
                                        {
                                            initRecord = lstMergeDBRows.Where(dbRow => dbRow.InitNumber.ToLower().Trim() == sInitNumber.ToLower().Trim()).FirstOrDefault();
                                        }
                                        if (userType == 1)
                                        { // HO User
                                            drRow["HOComment"] = Convert.ToString(drRow["HOComment"]);
                                            drRow["RPOCComment"] = (!string.IsNullOrEmpty(sInitNumber) && initRecord != null) ? Convert.ToString(initRecord.RPOCComment) : "";
                                            drRow["AgencyComment"] = (!string.IsNullOrEmpty(sInitNumber) && initRecord != null) ? Convert.ToString(initRecord.AgencyComment) : "";
                                        }
                                        else if (userType == 2)
                                        { // RPOC user
                                            drRow["RPOCComment"] = Convert.ToString(drRow["RPOCComment"]);
                                            drRow["HOComment"] = (!string.IsNullOrEmpty(sInitNumber) && initRecord != null) ? Convert.ToString(initRecord.HOComment) : "";
                                            drRow["AgencyComment"] = (!string.IsNullOrEmpty(sInitNumber) && initRecord != null) ? Convert.ToString(initRecord.AgencyComment) : "";
                                        }
                                        else if (userType == 3)
                                        { // set the agency comments
                                            drRow["AgencyComment"] = Convert.ToString(drRow["AgencyComment"]);
                                            drRow["HOComment"] = (!string.IsNullOrEmpty(sInitNumber) && initRecord != null) ? Convert.ToString(initRecord.HOComment) : "";
                                            drRow["RPOCComment"] = (!string.IsNullOrEmpty(sInitNumber) && initRecord != null) ? Convert.ToString(initRecord.RPOCComment) : "";
                                        }

                                        drRow["ProjectYear"] = objFlatFileHelper.GetProjectYear(tInitRecord);
                                        InitiativeSaveModelXL initiativeSaveModelXL =
                                            actionTypeCalculation.GetCalculatedValues(drRow, dtStartMonth, dtEndMonth, 
                                            lstMonthlyCPIValues, profileData.ID, initYear, tInitRecord);
                                        drRow = initiativeSaveModelXL.drInitiatives;
                                        InitiativeCalcs initiativeCalcs = initiativeSaveModelXL.initiativeCalcs;
                                        dtInit.AcceptChanges();
                                        dtValidInit.ImportRow(dtInit.Rows[i]);
                                        lstInitiativeCalcs.Add(initiativeCalcs);
                                        lstValidRowIndexes.Add(i);
                                        if (drRow["dbFlag"].ToString() == "I") successCount++;
                                        else if (drRow["dbFlag"].ToString() == "U")
                                        {
                                            if (twFileUpdatedInit == null)
                                            {
                                                twFileUpdatedInit = new StreamWriter(updatedInitPath);
                                            }
                                            twFileUpdatedInit.WriteLine(sInitNumber);
                                            updateCount++;
                                        }
                                    }
                                }
                            }
                            // To remove valid rowindexes from the excel.
                            int validRowIndex = 0;
                            for (int i = lstValidRowIndexes.Count - 1; i >= 0; i--)
                            {
                                validRowIndex = lstValidRowIndexes[i];
                                (worksheet.Rows[(validRowIndex + 2)]).Delete();
                            }

                            dtInit.AcceptChanges();
                            string initText = "inits";
                            string initCalcText = "initCalcs";
                            if (successCount > 0 || updateCount > 0)
                            {
                                var json = "{" + JsonConvert.SerializeObject(initText) + ":" + JsonConvert.SerializeObject(dtValidInit) + ", " +
                                    JsonConvert.SerializeObject(initCalcText) + ":" + JsonConvert.SerializeObject(lstInitiativeCalcs) + "}";
                                // Call SP for saving
                                DBOperations objDBOperations = new DBOperations();
                                objDBOperations.CallSaveInitiativesSP("SP_SAVEINITIATIVES", json, initYear);
                            }
                        }
                        else
                        {
                            // To display message that no new initiatives available.
                            resultCount = new ResultCount()
                            {
                                validationMsg = "No new/ updated initiatives uploaded"
                            };
                        }

                        workbook.SaveDocument(outputExcelPath);
                        workbook.Dispose();
                        stream.Dispose();
                        if (twFileUpdatedInit != null)
                        {
                            twFileUpdatedInit.Close();
                        }

                        if (dtExisting.Rows.Count > 0)
                        {
                            resultCount = new ResultCount()
                            {
                                errCount = errCount.ToString(),
                                successCount = successCount.ToString(),
                                updateCount = updateCount.ToString(),
                                updatedInitPath = "UploadedFiles/UpdatedInit/" + updatedInitFile,
                                outputExcelPath = "UploadedFiles/ErrorExcel/" + outExcelfileName,
                                validationMsg = ""
                            };
                        }
                        return Content(JsonConvert.SerializeObject(resultCount));
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                ResultCount resultCount = new ResultCount()
                {
                    validationMsg = ex.Message.ToString()
                };
                return Content(JsonConvert.SerializeObject(resultCount));
            }
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

        //public string getInitNumber(int year, string subcountry)
        //{
        //    var countrycodelst = lstSubCountryBrand.Where(sc => sc.subCountryName == subcountry).FirstOrDefault();
        //    string intnum = year.ToString() + countrycodelst.CountryCode;
        //    t_initiative initz = db.t_initiative.Where(c => c.InitNumber.StartsWith(intnum)).FirstOrDefault();
        //    string nomerterakhir = string.Empty;
        //    if (initz != null)
        //        nomerterakhir = db.t_initiative.Where(c => c.InitNumber.StartsWith(intnum)).OrderByDescending(o => o.InitNumber).FirstOrDefault().InitNumber;
        //    else
        //        nomerterakhir = intnum + "000";

        //    nomerterakhir = nomerterakhir.Substring((nomerterakhir.Length - 3), 3);
        //    int nomerurut = (Int32.Parse(nomerterakhir) + 1);
        //    string nomerselanjutnya = ("00" + nomerurut.ToString());
        //    nomerselanjutnya = intnum + nomerselanjutnya.Substring((nomerselanjutnya.Length - 3), 3);

        //    return nomerselanjutnya;
        //}


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

            #region new properties enh153-2 Mapping from view to variables

            string Unit_of_volumes = NewInitiative.Unit_of_volumes;
            decimal? Input_Actuals_Volumes_Nmin1 = NewInitiative.Input_Actuals_Volumes_Nmin1 == null ? 0 : NewInitiative.Input_Actuals_Volumes_Nmin1;
            decimal? Input_Target_Volumes = NewInitiative.Input_Target_Volumes == null ? 0 : NewInitiative.Input_Target_Volumes;
            decimal? Total_Actual_volume_N = NewInitiative.Total_Actual_volume_N == null ? 0 : NewInitiative.Total_Actual_volume_N;
            decimal? Spend_Nmin1 = NewInitiative.Spend_Nmin1 == null ? 0 : NewInitiative.Spend_Nmin1;
            decimal? Spend_N = NewInitiative.Spend_N == null ? 0 : NewInitiative.Spend_N;
            decimal? CPI = NewInitiative.CPI == null ? 0 : NewInitiative.CPI;

            decimal? janActual_volume_N = NewInitiative.janActual_volume_N == null ? 0 : NewInitiative.janActual_volume_N;
            decimal? febActual_volume_N = NewInitiative.febActual_volume_N == null ? 0 : NewInitiative.febActual_volume_N;
            decimal? marActual_volume_N = NewInitiative.marActual_volume_N == null ? 0 : NewInitiative.marActual_volume_N;
            decimal? aprActual_volume_N = NewInitiative.aprActual_volume_N == null ? 0 : NewInitiative.aprActual_volume_N;
            decimal? mayActual_volume_N = NewInitiative.mayActual_volume_N == null ? 0 : NewInitiative.mayActual_volume_N;
            decimal? junActual_volume_N = NewInitiative.junActual_volume_N == null ? 0 : NewInitiative.junActual_volume_N;
            decimal? julActual_volume_N = NewInitiative.julActual_volume_N == null ? 0 : NewInitiative.julActual_volume_N;
            decimal? augActual_volume_N = NewInitiative.augActual_volume_N == null ? 0 : NewInitiative.augActual_volume_N;
            decimal? sepActual_volume_N = NewInitiative.sepActual_volume_N == null ? 0 : NewInitiative.sepActual_volume_N;
            decimal? octActual_volume_N = NewInitiative.octActual_volume_N == null ? 0 : NewInitiative.octActual_volume_N;
            decimal? novActual_volume_N = NewInitiative.novActual_volume_N == null ? 0 : NewInitiative.novActual_volume_N;
            decimal? decActual_volume_N = NewInitiative.decActual_volume_N == null ? 0 : NewInitiative.decActual_volume_N;
            decimal? N_FY_Sec_PRICE_EF = NewInitiative.N_FY_Sec_PRICE_EF == null ? 0 : NewInitiative.N_FY_Sec_PRICE_EF;
            decimal? N_FY_Sec_VOLUME_EF = NewInitiative.N_FY_Sec_VOLUME_EF == null ? 0 : NewInitiative.N_FY_Sec_VOLUME_EF;
            decimal? N_YTD_Sec_PRICE_EF = NewInitiative.N_YTD_Sec_PRICE_EF == null ? 0 : NewInitiative.N_YTD_Sec_PRICE_EF;
            decimal? N_YTD_Sec_VOLUME_EF = NewInitiative.N_YTD_Sec_VOLUME_EF == null ? 0 : NewInitiative.N_YTD_Sec_VOLUME_EF;
            decimal? YTD_Achieved_PRICE_EF = NewInitiative.YTD_Achieved_PRICE_EF == null ? 0 : NewInitiative.YTD_Achieved_PRICE_EF;
            decimal? YTD_Achieved_VOLUME_EF = NewInitiative.YTD_Achieved_VOLUME_EF == null ? 0 : NewInitiative.YTD_Achieved_VOLUME_EF;
            decimal? YTD_Cost_Avoid_Vs_CPI = NewInitiative.YTD_Cost_Avoid_Vs_CPI == null ? 0 : NewInitiative.YTD_Cost_Avoid_Vs_CPI;
            decimal? FY_Cost_Avoid_Vs_CPI = NewInitiative.FY_Cost_Avoid_Vs_CPI == null ? 0 : NewInitiative.FY_Cost_Avoid_Vs_CPI;
            t_initiative_calcs _t_initiative_calcs = NewInitiative._t_initiative_calcs;
            int isProcurement = NewInitiative.isProcurement;
            #endregion

            #region new properties backend calculation enh153-2 Mapping from view to variables

            decimal jan_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.jan_Actual_CPU_Nmin1;
            decimal feb_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.feb_Actual_CPU_Nmin1;
            decimal march_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.march_Actual_CPU_Nmin1;
            decimal apr_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.apr_Actual_CPU_Nmin1;
            decimal may_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.may_Actual_CPU_Nmin1;
            decimal jun_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.jun_Actual_CPU_Nmin1;
            decimal jul_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.jul_Actual_CPU_Nmin1;
            decimal aug_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.aug_Actual_CPU_Nmin1;
            decimal sep_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.sep_Actual_CPU_Nmin1;
            decimal oct_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.oct_Actual_CPU_Nmin1;
            decimal nov_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.nov_Actual_CPU_Nmin1;
            decimal dec_Actual_CPU_Nmin1 = NewInitiative._t_initiative_calcs.dec_Actual_CPU_Nmin1;
            decimal jan_Target_CPU_N = NewInitiative._t_initiative_calcs.jan_Target_CPU_N;
            decimal feb_Target_CPU_N = NewInitiative._t_initiative_calcs.feb_Target_CPU_N;
            decimal march_Target_CPU_N = NewInitiative._t_initiative_calcs.march_Target_CPU_N;
            decimal apr_Target_CPU_N = NewInitiative._t_initiative_calcs.apr_Target_CPU_N;
            decimal may_Target_CPU_N = NewInitiative._t_initiative_calcs.may_Target_CPU_N;
            decimal jun_Target_CPU_N = NewInitiative._t_initiative_calcs.jun_Target_CPU_N;
            decimal jul_Target_CPU_N = NewInitiative._t_initiative_calcs.jul_Target_CPU_N;
            decimal aug_Target_CPU_N = NewInitiative._t_initiative_calcs.aug_Target_CPU_N;
            decimal sep_Target_CPU_N = NewInitiative._t_initiative_calcs.sep_Target_CPU_N;
            decimal oct_Target_CPU_N = NewInitiative._t_initiative_calcs.oct_Target_CPU_N;
            decimal nov_Target_CPU_N = NewInitiative._t_initiative_calcs.nov_Target_CPU_N;
            decimal dec_Target_CPU_N = NewInitiative._t_initiative_calcs.dec_Target_CPU_N;
            decimal jan_A_Price_effect = NewInitiative._t_initiative_calcs.jan_A_Price_effect;
            decimal feb_A_Price_effect = NewInitiative._t_initiative_calcs.feb_A_Price_effect;
            decimal march_A_Price_effect = NewInitiative._t_initiative_calcs.march_A_Price_effect;
            decimal apr_A_Price_effect = NewInitiative._t_initiative_calcs.apr_A_Price_effect;
            decimal may_A_Price_effect = NewInitiative._t_initiative_calcs.may_A_Price_effect;
            decimal jun_A_Price_effect = NewInitiative._t_initiative_calcs.jun_A_Price_effect;
            decimal jul_A_Price_effect = NewInitiative._t_initiative_calcs.jul_A_Price_effect;
            decimal aug_A_Price_effect = NewInitiative._t_initiative_calcs.aug_A_Price_effect;
            decimal sep_A_Price_effect = NewInitiative._t_initiative_calcs.sep_A_Price_effect;
            decimal oct_A_Price_effect = NewInitiative._t_initiative_calcs.oct_A_Price_effect;
            decimal nov_A_Price_effect = NewInitiative._t_initiative_calcs.nov_A_Price_effect;
            decimal dec_A_Price_effect = NewInitiative._t_initiative_calcs.dec_A_Price_effect;
            decimal jan_A_Volume_Effect = NewInitiative._t_initiative_calcs.jan_A_Volume_Effect;
            decimal feb_A_Volume_Effect = NewInitiative._t_initiative_calcs.feb_A_Volume_Effect;
            decimal march_A_Volume_Effect = NewInitiative._t_initiative_calcs.march_A_Volume_Effect;
            decimal apr_A_Volume_Effect = NewInitiative._t_initiative_calcs.apr_A_Volume_Effect;
            decimal may_A_Volume_Effect = NewInitiative._t_initiative_calcs.may_A_Volume_Effect;
            decimal jun_A_Volume_Effect = NewInitiative._t_initiative_calcs.jun_A_Volume_Effect;
            decimal jul_A_Volume_Effect = NewInitiative._t_initiative_calcs.jul_A_Volume_Effect;
            decimal aug_A_Volume_Effect = NewInitiative._t_initiative_calcs.aug_A_Volume_Effect;
            decimal sep_A_Volume_Effect = NewInitiative._t_initiative_calcs.sep_A_Volume_Effect;
            decimal oct_A_Volume_Effect = NewInitiative._t_initiative_calcs.oct_A_Volume_Effect;
            decimal nov_A_Volume_Effect = NewInitiative._t_initiative_calcs.nov_A_Volume_Effect;
            decimal dec_A_Volume_Effect = NewInitiative._t_initiative_calcs.dec_A_Volume_Effect;
            decimal jan_Achievement = NewInitiative._t_initiative_calcs.jan_Achievement;
            decimal feb_Achievement = NewInitiative._t_initiative_calcs.feb_Achievement;
            decimal march_Achievement = NewInitiative._t_initiative_calcs.march_Achievement;
            decimal apr_Achievement = NewInitiative._t_initiative_calcs.apr_Achievement;
            decimal may_Achievement = NewInitiative._t_initiative_calcs.may_Achievement;
            decimal jun_Achievement = NewInitiative._t_initiative_calcs.jun_Achievement;
            decimal jul_Achievement = NewInitiative._t_initiative_calcs.jul_Achievement;
            decimal aug_Achievement = NewInitiative._t_initiative_calcs.aug_Achievement;
            decimal sep_Achievement = NewInitiative._t_initiative_calcs.sep_Achievement;
            decimal oct_Achievement = NewInitiative._t_initiative_calcs.oct_Achievement;
            decimal nov_Achievement = NewInitiative._t_initiative_calcs.nov_Achievement;
            decimal dec_Achievement = NewInitiative._t_initiative_calcs.dec_Achievement;
            decimal jan_ST_Price_effect = NewInitiative._t_initiative_calcs.jan_ST_Price_effect;
            decimal feb_ST_Price_effect = NewInitiative._t_initiative_calcs.feb_ST_Price_effect;
            decimal march_ST_Price_effect = NewInitiative._t_initiative_calcs.march_ST_Price_effect;
            decimal apr_ST_Price_effect = NewInitiative._t_initiative_calcs.apr_ST_Price_effect;
            decimal may_ST_Price_effect = NewInitiative._t_initiative_calcs.may_ST_Price_effect;
            decimal jun_ST_Price_effect = NewInitiative._t_initiative_calcs.jun_ST_Price_effect;
            decimal jul_ST_Price_effect = NewInitiative._t_initiative_calcs.jul_ST_Price_effect;
            decimal aug_ST_Price_effect = NewInitiative._t_initiative_calcs.aug_ST_Price_effect;
            decimal sep_ST_Price_effect = NewInitiative._t_initiative_calcs.sep_ST_Price_effect;
            decimal oct_ST_Price_effect = NewInitiative._t_initiative_calcs.oct_ST_Price_effect;
            decimal nov_ST_Price_effect = NewInitiative._t_initiative_calcs.nov_ST_Price_effect;
            decimal dec_ST_Price_effect = NewInitiative._t_initiative_calcs.dec_ST_Price_effect;
            decimal jan_ST_Volume_Effect = NewInitiative._t_initiative_calcs.jan_ST_Volume_Effect;
            decimal feb_ST_Volume_Effect = NewInitiative._t_initiative_calcs.feb_ST_Volume_Effect;
            decimal march_ST_Volume_Effect = NewInitiative._t_initiative_calcs.march_ST_Volume_Effect;
            decimal apr_ST_Volume_Effect = NewInitiative._t_initiative_calcs.apr_ST_Volume_Effect;
            decimal may_ST_Volume_Effect = NewInitiative._t_initiative_calcs.may_ST_Volume_Effect;
            decimal jun_ST_Volume_Effect = NewInitiative._t_initiative_calcs.jun_ST_Volume_Effect;
            decimal jul_ST_Volume_Effect = NewInitiative._t_initiative_calcs.jul_ST_Volume_Effect;
            decimal aug_ST_Volume_Effect = NewInitiative._t_initiative_calcs.aug_ST_Volume_Effect;
            decimal sep_ST_Volume_Effect = NewInitiative._t_initiative_calcs.sep_ST_Volume_Effect;
            decimal oct_ST_Volume_Effect = NewInitiative._t_initiative_calcs.oct_ST_Volume_Effect;
            decimal nov_ST_Volume_Effect = NewInitiative._t_initiative_calcs.nov_ST_Volume_Effect;
            decimal dec_ST_Volume_Effect = NewInitiative._t_initiative_calcs.dec_ST_Volume_Effect;
            decimal jan_FY_Secured_Target = NewInitiative._t_initiative_calcs.jan_FY_Secured_Target;
            decimal feb_FY_Secured_Target = NewInitiative._t_initiative_calcs.feb_FY_Secured_Target;
            decimal march_FY_Secured_Target = NewInitiative._t_initiative_calcs.march_FY_Secured_Target;
            decimal apr_FY_Secured_Target = NewInitiative._t_initiative_calcs.apr_FY_Secured_Target;
            decimal may_FY_Secured_Target = NewInitiative._t_initiative_calcs.may_FY_Secured_Target;
            decimal jun_FY_Secured_Target = NewInitiative._t_initiative_calcs.jun_FY_Secured_Target;
            decimal jul_FY_Secured_Target = NewInitiative._t_initiative_calcs.jul_FY_Secured_Target;
            decimal aug_FY_Secured_Target = NewInitiative._t_initiative_calcs.aug_FY_Secured_Target;
            decimal sep_FY_Secured_Target = NewInitiative._t_initiative_calcs.sep_FY_Secured_Target;
            decimal oct_FY_Secured_Target = NewInitiative._t_initiative_calcs.oct_FY_Secured_Target;
            decimal nov_FY_Secured_Target = NewInitiative._t_initiative_calcs.nov_FY_Secured_Target;
            decimal dec_FY_Secured_Target = NewInitiative._t_initiative_calcs.dec_FY_Secured_Target;
            decimal jan_CPI_Effect = NewInitiative._t_initiative_calcs.jan_CPI_Effect;
            decimal feb_CPI_Effect = NewInitiative._t_initiative_calcs.feb_CPI_Effect;
            decimal march_CPI_Effect = NewInitiative._t_initiative_calcs.march_CPI_Effect;
            decimal apr_CPI_Effect = NewInitiative._t_initiative_calcs.apr_CPI_Effect;
            decimal may_CPI_Effect = NewInitiative._t_initiative_calcs.may_CPI_Effect;
            decimal jun_CPI_Effect = NewInitiative._t_initiative_calcs.jun_CPI_Effect;
            decimal jul_CPI_Effect = NewInitiative._t_initiative_calcs.jul_CPI_Effect;
            decimal aug_CPI_Effect = NewInitiative._t_initiative_calcs.aug_CPI_Effect;
            decimal sep_CPI_Effect = NewInitiative._t_initiative_calcs.sep_CPI_Effect;
            decimal oct_CPI_Effect = NewInitiative._t_initiative_calcs.oct_CPI_Effect;
            decimal nov_CPI_Effect = NewInitiative._t_initiative_calcs.nov_CPI_Effect;
            decimal dec_CPI_Effect = NewInitiative._t_initiative_calcs.dec_CPI_Effect;

            decimal jan_CPI = NewInitiative._t_initiative_calcs.jan_CPI;
            decimal feb_CPI = NewInitiative._t_initiative_calcs.feb_CPI;
            decimal mar_CPI = NewInitiative._t_initiative_calcs.mar_CPI;
            decimal apr_CPI = NewInitiative._t_initiative_calcs.apr_CPI;
            decimal may_CPI = NewInitiative._t_initiative_calcs.may_CPI;
            decimal jun_CPI = NewInitiative._t_initiative_calcs.jun_CPI;
            decimal jul_CPI = NewInitiative._t_initiative_calcs.jul_CPI;
            decimal aug_CPI = NewInitiative._t_initiative_calcs.aug_CPI;
            decimal sep_CPI = NewInitiative._t_initiative_calcs.sep_CPI;
            decimal oct_CPI = NewInitiative._t_initiative_calcs.oct_CPI;
            decimal nov_CPI = NewInitiative._t_initiative_calcs.nov_CPI;
            decimal dec_CPI = NewInitiative._t_initiative_calcs.dec_CPI;

            #endregion
            if (FormStatus == "New")
            {
                try
                {
                    using (GainEntities db = new GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"])))
                    {
                        var tinitiative = db.Set<t_initiative>();
                        var t_initiative_calcs = db.Set<t_initiative_calcs>();
                        var YearInitiative = profileData.ProjectYear;
                        Random rand = new Random();
                        //int InitNextNum = rand.Next(1,999);
                        //string OutInitNextNum = "00" + InitNextNum.ToString();
                        string nomerterakhir = "";
                        t_initiative initz = db.t_initiative.Where(c => c.InitNumber.StartsWith(YearInitiative + KodeNegara)
                        && c.SubCountryID == GrdSubCountry).FirstOrDefault();
                        if (initz != null)
                            //nomerterakhir = db.t_initiative.Where(c => c.InitNumber.StartsWith(YearInitiative + KodeNegara) && c.CountryID == GrdCountry && c.SubCountryID == GrdSubCountry).OrderByDescending(o => o.InitNumber).FirstOrDefault().InitNumber;
                            nomerterakhir = db.t_initiative.Where(c => c.InitNumber.StartsWith(YearInitiative + KodeNegara)
                             && c.SubCountryID == GrdSubCountry).OrderByDescending(o => o.id).FirstOrDefault().InitNumber;
                        else
                            nomerterakhir = YearInitiative + KodeNegara + "000";

                        nomerterakhir = nomerterakhir.Remove(0, 4).Replace(KodeNegara, ""); 

                        //nomerterakhir = nomerterakhir.Substring((nomerterakhir.Length - 3), 3);
                        int nomerurut = (Int32.Parse(nomerterakhir) + 1);
                        //string nomerselanjutnya = ("00" + nomerurut.ToString());
                        string nomerselanjutnya = (nomerurut < 10) ? ("00" + nomerurut.ToString()) : (nomerurut < 100) ?
                             ("0" + nomerurut.ToString()) : nomerurut.ToString();
                        //nomerselanjutnya = nomerselanjutnya.Substring((nomerselanjutnya.Length - 3), 3);

                        if (isProcurement != 1)
                        {
                            #region normal initiative save
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
                                PortID = (TxPortName == 0 ? (profileData.ProjectYear <= 2022 ? 1 : 570) : TxPortName),
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
                                ModifiedBy = UserID,

                                #region inserting new properties enh153-2
                                Unit_of_volumes = Unit_of_volumes,
                                Input_Actuals_Volumes_Nmin1 = Input_Actuals_Volumes_Nmin1,
                                Input_Target_Volumes = Input_Target_Volumes,
                                Total_Actual_volume_N = Total_Actual_volume_N,
                                Spend_Nmin1 = Spend_Nmin1,
                                Spend_N = Spend_N,
                                CPI = CPI,
                                janActual_volume_N = janActual_volume_N,
                                febActual_volume_N = febActual_volume_N,
                                marActual_volume_N = marActual_volume_N,
                                aprActual_volume_N = aprActual_volume_N,
                                mayActual_volume_N = mayActual_volume_N,
                                junActual_volume_N = junActual_volume_N,
                                julActual_volume_N = julActual_volume_N,
                                augActual_volume_N = augActual_volume_N,
                                sepActual_volume_N = sepActual_volume_N,
                                octActual_volume_N = octActual_volume_N,
                                novActual_volume_N = novActual_volume_N,
                                decActual_volume_N = decActual_volume_N,
                                N_FY_Sec_PRICE_EF = N_FY_Sec_PRICE_EF,
                                N_FY_Sec_VOLUME_EF = N_FY_Sec_VOLUME_EF,
                                N_YTD_Sec_PRICE_EF = N_YTD_Sec_PRICE_EF,
                                N_YTD_Sec_VOLUME_EF = N_YTD_Sec_VOLUME_EF,
                                YTD_Achieved_PRICE_EF = YTD_Achieved_PRICE_EF,
                                YTD_Achieved_VOLUME_EF = YTD_Achieved_VOLUME_EF,
                                YTD_Cost_Avoid_Vs_CPI = YTD_Cost_Avoid_Vs_CPI,
                                FY_Cost_Avoid_Vs_CPI = FY_Cost_Avoid_Vs_CPI,
                                isProcurement = isProcurement
                                #endregion

                            });
                            //db.SaveChanges(); 
                            #endregion
                        }
                        else
                        {
                            #region Procurement initiative save
                            t_initiative_calcs.Add(new t_initiative_calcs
                            {
                                t_initiative = new t_initiative
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
                                    PortID = (TxPortName == 0 ? (profileData.ProjectYear <= 2022 ? 1 : 570) : TxPortName),
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
                                    ModifiedBy = UserID,

                                    #region inserting new properties enh153-2
                                    Unit_of_volumes = Unit_of_volumes,
                                    Input_Actuals_Volumes_Nmin1 = Input_Actuals_Volumes_Nmin1,
                                    Input_Target_Volumes = Input_Target_Volumes,
                                    Total_Actual_volume_N = Total_Actual_volume_N,
                                    Spend_Nmin1 = Spend_Nmin1,
                                    Spend_N = Spend_N,
                                    CPI = CPI,
                                    janActual_volume_N = janActual_volume_N,
                                    febActual_volume_N = febActual_volume_N,
                                    marActual_volume_N = marActual_volume_N,
                                    aprActual_volume_N = aprActual_volume_N,
                                    mayActual_volume_N = mayActual_volume_N,
                                    junActual_volume_N = junActual_volume_N,
                                    julActual_volume_N = julActual_volume_N,
                                    augActual_volume_N = augActual_volume_N,
                                    sepActual_volume_N = sepActual_volume_N,
                                    octActual_volume_N = octActual_volume_N,
                                    novActual_volume_N = novActual_volume_N,
                                    decActual_volume_N = decActual_volume_N,
                                    N_FY_Sec_PRICE_EF = N_FY_Sec_PRICE_EF,
                                    N_FY_Sec_VOLUME_EF = N_FY_Sec_VOLUME_EF,
                                    N_YTD_Sec_PRICE_EF = N_YTD_Sec_PRICE_EF,
                                    N_YTD_Sec_VOLUME_EF = N_YTD_Sec_VOLUME_EF,
                                    YTD_Achieved_PRICE_EF = YTD_Achieved_PRICE_EF,
                                    YTD_Achieved_VOLUME_EF = YTD_Achieved_VOLUME_EF,
                                    YTD_Cost_Avoid_Vs_CPI = YTD_Cost_Avoid_Vs_CPI,
                                    FY_Cost_Avoid_Vs_CPI = FY_Cost_Avoid_Vs_CPI,
                                    isProcurement = isProcurement

                                    #endregion

                                },
                                jan_Actual_CPU_Nmin1 = jan_Actual_CPU_Nmin1,
                                feb_Actual_CPU_Nmin1 = feb_Actual_CPU_Nmin1,
                                march_Actual_CPU_Nmin1 = march_Actual_CPU_Nmin1,
                                apr_Actual_CPU_Nmin1 = apr_Actual_CPU_Nmin1,
                                may_Actual_CPU_Nmin1 = may_Actual_CPU_Nmin1,
                                jun_Actual_CPU_Nmin1 = jun_Actual_CPU_Nmin1,
                                jul_Actual_CPU_Nmin1 = jul_Actual_CPU_Nmin1,
                                aug_Actual_CPU_Nmin1 = aug_Actual_CPU_Nmin1,
                                sep_Actual_CPU_Nmin1 = sep_Actual_CPU_Nmin1,
                                oct_Actual_CPU_Nmin1 = oct_Actual_CPU_Nmin1,
                                nov_Actual_CPU_Nmin1 = nov_Actual_CPU_Nmin1,
                                dec_Actual_CPU_Nmin1 = dec_Actual_CPU_Nmin1,
                                jan_Target_CPU_N = jan_Target_CPU_N,
                                feb_Target_CPU_N = feb_Target_CPU_N,
                                march_Target_CPU_N = march_Target_CPU_N,
                                apr_Target_CPU_N = apr_Target_CPU_N,
                                may_Target_CPU_N = may_Target_CPU_N,
                                jun_Target_CPU_N = jun_Target_CPU_N,
                                jul_Target_CPU_N = jul_Target_CPU_N,
                                aug_Target_CPU_N = aug_Target_CPU_N,
                                sep_Target_CPU_N = sep_Target_CPU_N,
                                oct_Target_CPU_N = oct_Target_CPU_N,
                                nov_Target_CPU_N = nov_Target_CPU_N,
                                dec_Target_CPU_N = dec_Target_CPU_N,
                                jan_A_Price_effect = jan_A_Price_effect,
                                feb_A_Price_effect = feb_A_Price_effect,
                                march_A_Price_effect = march_A_Price_effect,
                                apr_A_Price_effect = apr_A_Price_effect,
                                may_A_Price_effect = may_A_Price_effect,
                                jun_A_Price_effect = jun_A_Price_effect,
                                jul_A_Price_effect = jul_A_Price_effect,
                                aug_A_Price_effect = aug_A_Price_effect,
                                sep_A_Price_effect = sep_A_Price_effect,
                                oct_A_Price_effect = oct_A_Price_effect,
                                nov_A_Price_effect = nov_A_Price_effect,
                                dec_A_Price_effect = dec_A_Price_effect,
                                jan_A_Volume_Effect = jan_A_Volume_Effect,
                                feb_A_Volume_Effect = feb_A_Volume_Effect,
                                march_A_Volume_Effect = march_A_Volume_Effect,
                                apr_A_Volume_Effect = apr_A_Volume_Effect,
                                may_A_Volume_Effect = may_A_Volume_Effect,
                                jun_A_Volume_Effect = jun_A_Volume_Effect,
                                jul_A_Volume_Effect = jul_A_Volume_Effect,
                                aug_A_Volume_Effect = aug_A_Volume_Effect,
                                sep_A_Volume_Effect = sep_A_Volume_Effect,
                                oct_A_Volume_Effect = oct_A_Volume_Effect,
                                nov_A_Volume_Effect = nov_A_Volume_Effect,
                                dec_A_Volume_Effect = dec_A_Volume_Effect,
                                jan_Achievement = jan_Achievement,
                                feb_Achievement = feb_Achievement,
                                march_Achievement = march_Achievement,
                                apr_Achievement = apr_Achievement,
                                may_Achievement = may_Achievement,
                                jun_Achievement = jun_Achievement,
                                jul_Achievement = jul_Achievement,
                                aug_Achievement = aug_Achievement,
                                sep_Achievement = sep_Achievement,
                                oct_Achievement = oct_Achievement,
                                nov_Achievement = nov_Achievement,
                                dec_Achievement = dec_Achievement,
                                jan_ST_Price_effect = jan_ST_Price_effect,
                                feb_ST_Price_effect = feb_ST_Price_effect,
                                march_ST_Price_effect = march_ST_Price_effect,
                                apr_ST_Price_effect = apr_ST_Price_effect,
                                may_ST_Price_effect = may_ST_Price_effect,
                                jun_ST_Price_effect = jun_ST_Price_effect,
                                jul_ST_Price_effect = jul_ST_Price_effect,
                                aug_ST_Price_effect = aug_ST_Price_effect,
                                sep_ST_Price_effect = sep_ST_Price_effect,
                                oct_ST_Price_effect = oct_ST_Price_effect,
                                nov_ST_Price_effect = nov_ST_Price_effect,
                                dec_ST_Price_effect = dec_ST_Price_effect,
                                jan_ST_Volume_Effect = jan_ST_Volume_Effect,
                                feb_ST_Volume_Effect = feb_ST_Volume_Effect,
                                march_ST_Volume_Effect = march_ST_Volume_Effect,
                                apr_ST_Volume_Effect = apr_ST_Volume_Effect,
                                may_ST_Volume_Effect = may_ST_Volume_Effect,
                                jun_ST_Volume_Effect = jun_ST_Volume_Effect,
                                jul_ST_Volume_Effect = jul_ST_Volume_Effect,
                                aug_ST_Volume_Effect = aug_ST_Volume_Effect,
                                sep_ST_Volume_Effect = sep_ST_Volume_Effect,
                                oct_ST_Volume_Effect = oct_ST_Volume_Effect,
                                nov_ST_Volume_Effect = nov_ST_Volume_Effect,
                                dec_ST_Volume_Effect = dec_ST_Volume_Effect,
                                jan_FY_Secured_Target = jan_FY_Secured_Target,
                                feb_FY_Secured_Target = feb_FY_Secured_Target,
                                march_FY_Secured_Target = march_FY_Secured_Target,
                                apr_FY_Secured_Target = apr_FY_Secured_Target,
                                may_FY_Secured_Target = may_FY_Secured_Target,
                                jun_FY_Secured_Target = jun_FY_Secured_Target,
                                jul_FY_Secured_Target = jul_FY_Secured_Target,
                                aug_FY_Secured_Target = aug_FY_Secured_Target,
                                sep_FY_Secured_Target = sep_FY_Secured_Target,
                                oct_FY_Secured_Target = oct_FY_Secured_Target,
                                nov_FY_Secured_Target = nov_FY_Secured_Target,
                                dec_FY_Secured_Target = dec_FY_Secured_Target,
                                jan_CPI_Effect = jan_CPI_Effect,
                                feb_CPI_Effect = feb_CPI_Effect,
                                march_CPI_Effect = march_CPI_Effect,
                                apr_CPI_Effect = apr_CPI_Effect,
                                may_CPI_Effect = may_CPI_Effect,
                                jun_CPI_Effect = jun_CPI_Effect,
                                jul_CPI_Effect = jul_CPI_Effect,
                                aug_CPI_Effect = aug_CPI_Effect,
                                sep_CPI_Effect = sep_CPI_Effect,
                                oct_CPI_Effect = oct_CPI_Effect,
                                nov_CPI_Effect = nov_CPI_Effect,
                                dec_CPI_Effect = dec_CPI_Effect,
                                jan_CPI = jan_CPI,
                                feb_CPI = feb_CPI,
                                mar_CPI = mar_CPI,
                                apr_CPI = apr_CPI,
                                may_CPI = may_CPI,
                                jun_CPI = jun_CPI,
                                jul_CPI = jul_CPI,
                                aug_CPI = aug_CPI,
                                sep_CPI = sep_CPI,
                                oct_CPI = oct_CPI,
                                nov_CPI = nov_CPI,
                                dec_CPI = dec_CPI
                            });
                            #endregion

                        }

                        db.SaveChanges();
                        return Content("saved|" + YearInitiative + KodeNegara + nomerselanjutnya);
                    }
                }
                catch (Exception E)
                {
                    log.Error("Exception occured in saving new initiative" + E.Message);
                    return Content("Error occured during Initiative save");
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
                    if (GrdLegalEntity != 0) initdata.LegalEntityID = GrdLegalEntity;
                    initdata.CountryID = GrdCountry;
                    initdata.SubCountryID = GrdSubCountry;
                    initdata.Confidential = CboConfidential;
                    initdata.Description = TxDesc;
                    initdata.ResponsibleFullName = TxResponsibleName;
                    initdata.InitiativeType = GrdInitType;
                    initdata.CostCategoryID = GrdInitCategory;
                    initdata.SubCostCategoryID = GrdSubCost;
                    initdata.ActionTypeID = GrdActionType;
                    if (GrdSynImpact != 0) initdata.SynergyImpactID = GrdSynImpact;
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
                    initdata.PortID = (TxPortName == 0 ? (profileData.ProjectYear <= 2022 ? 1 : 570) : TxPortName);
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

                    #region updating new properties enh153-2
                    initdata.Unit_of_volumes = Unit_of_volumes;
                    initdata.Input_Actuals_Volumes_Nmin1 = Input_Actuals_Volumes_Nmin1;
                    initdata.Input_Target_Volumes = Input_Target_Volumes;
                    initdata.Total_Actual_volume_N = Total_Actual_volume_N;
                    initdata.Spend_Nmin1 = Spend_Nmin1;
                    initdata.Spend_N = Spend_N;
                    initdata.CPI = CPI;
                    initdata.janActual_volume_N = janActual_volume_N;
                    initdata.febActual_volume_N = febActual_volume_N;
                    initdata.marActual_volume_N = marActual_volume_N;
                    initdata.aprActual_volume_N = aprActual_volume_N;
                    initdata.mayActual_volume_N = mayActual_volume_N;
                    initdata.junActual_volume_N = junActual_volume_N;
                    initdata.julActual_volume_N = julActual_volume_N;
                    initdata.augActual_volume_N = augActual_volume_N;
                    initdata.sepActual_volume_N = sepActual_volume_N;
                    initdata.octActual_volume_N = octActual_volume_N;
                    initdata.novActual_volume_N = novActual_volume_N;
                    initdata.decActual_volume_N = decActual_volume_N;
                    initdata.N_FY_Sec_PRICE_EF = N_FY_Sec_PRICE_EF;
                    initdata.N_FY_Sec_VOLUME_EF = N_FY_Sec_VOLUME_EF;
                    initdata.N_YTD_Sec_PRICE_EF = N_YTD_Sec_PRICE_EF;
                    initdata.N_YTD_Sec_VOLUME_EF = N_YTD_Sec_VOLUME_EF;
                    initdata.YTD_Achieved_PRICE_EF = YTD_Achieved_PRICE_EF;
                    initdata.YTD_Achieved_VOLUME_EF = YTD_Achieved_VOLUME_EF;
                    initdata.YTD_Cost_Avoid_Vs_CPI = YTD_Cost_Avoid_Vs_CPI;
                    initdata.FY_Cost_Avoid_Vs_CPI = FY_Cost_Avoid_Vs_CPI;

                    #endregion
                    initdata.CreatedBy = initdata.CreatedBy;// (initdata.CreatedBy == null ? UserID : initdata.CreatedBy);
                    initdata.ModifiedBy = UserID;
                    if (isProcurement == 1)
                    {
                        var initdata_procure = db.t_initiative_calcs.Where(x => x.t_initiative_ID == FormID).FirstOrDefault();
                        initdata_procure.jan_Actual_CPU_Nmin1 = jan_Actual_CPU_Nmin1;
                        initdata_procure.feb_Actual_CPU_Nmin1 = feb_Actual_CPU_Nmin1;
                        initdata_procure.march_Actual_CPU_Nmin1 = march_Actual_CPU_Nmin1;
                        initdata_procure.apr_Actual_CPU_Nmin1 = apr_Actual_CPU_Nmin1;
                        initdata_procure.may_Actual_CPU_Nmin1 = may_Actual_CPU_Nmin1;
                        initdata_procure.jun_Actual_CPU_Nmin1 = jun_Actual_CPU_Nmin1;
                        initdata_procure.jul_Actual_CPU_Nmin1 = jul_Actual_CPU_Nmin1;
                        initdata_procure.aug_Actual_CPU_Nmin1 = aug_Actual_CPU_Nmin1;
                        initdata_procure.sep_Actual_CPU_Nmin1 = sep_Actual_CPU_Nmin1;
                        initdata_procure.oct_Actual_CPU_Nmin1 = oct_Actual_CPU_Nmin1;
                        initdata_procure.nov_Actual_CPU_Nmin1 = nov_Actual_CPU_Nmin1;
                        initdata_procure.dec_Actual_CPU_Nmin1 = dec_Actual_CPU_Nmin1;
                        initdata_procure.jan_Target_CPU_N = jan_Target_CPU_N;
                        initdata_procure.feb_Target_CPU_N = feb_Target_CPU_N;
                        initdata_procure.march_Target_CPU_N = march_Target_CPU_N;
                        initdata_procure.apr_Target_CPU_N = apr_Target_CPU_N;
                        initdata_procure.may_Target_CPU_N = may_Target_CPU_N;
                        initdata_procure.jun_Target_CPU_N = jun_Target_CPU_N;
                        initdata_procure.jul_Target_CPU_N = jul_Target_CPU_N;
                        initdata_procure.aug_Target_CPU_N = aug_Target_CPU_N;
                        initdata_procure.sep_Target_CPU_N = sep_Target_CPU_N;
                        initdata_procure.oct_Target_CPU_N = oct_Target_CPU_N;
                        initdata_procure.nov_Target_CPU_N = nov_Target_CPU_N;
                        initdata_procure.dec_Target_CPU_N = dec_Target_CPU_N;
                        initdata_procure.jan_A_Price_effect = jan_A_Price_effect;
                        initdata_procure.feb_A_Price_effect = feb_A_Price_effect;
                        initdata_procure.march_A_Price_effect = march_A_Price_effect;
                        initdata_procure.apr_A_Price_effect = apr_A_Price_effect;
                        initdata_procure.may_A_Price_effect = may_A_Price_effect;
                        initdata_procure.jun_A_Price_effect = jun_A_Price_effect;
                        initdata_procure.jul_A_Price_effect = jul_A_Price_effect;
                        initdata_procure.aug_A_Price_effect = aug_A_Price_effect;
                        initdata_procure.sep_A_Price_effect = sep_A_Price_effect;
                        initdata_procure.oct_A_Price_effect = oct_A_Price_effect;
                        initdata_procure.nov_A_Price_effect = nov_A_Price_effect;
                        initdata_procure.dec_A_Price_effect = dec_A_Price_effect;
                        initdata_procure.jan_A_Volume_Effect = jan_A_Volume_Effect;
                        initdata_procure.feb_A_Volume_Effect = feb_A_Volume_Effect;
                        initdata_procure.march_A_Volume_Effect = march_A_Volume_Effect;
                        initdata_procure.apr_A_Volume_Effect = apr_A_Volume_Effect;
                        initdata_procure.may_A_Volume_Effect = may_A_Volume_Effect;
                        initdata_procure.jun_A_Volume_Effect = jun_A_Volume_Effect;
                        initdata_procure.jul_A_Volume_Effect = jul_A_Volume_Effect;
                        initdata_procure.aug_A_Volume_Effect = aug_A_Volume_Effect;
                        initdata_procure.sep_A_Volume_Effect = sep_A_Volume_Effect;
                        initdata_procure.oct_A_Volume_Effect = oct_A_Volume_Effect;
                        initdata_procure.nov_A_Volume_Effect = nov_A_Volume_Effect;
                        initdata_procure.dec_A_Volume_Effect = dec_A_Volume_Effect;
                        initdata_procure.jan_Achievement = jan_Achievement;
                        initdata_procure.feb_Achievement = feb_Achievement;
                        initdata_procure.march_Achievement = march_Achievement;
                        initdata_procure.apr_Achievement = apr_Achievement;
                        initdata_procure.may_Achievement = may_Achievement;
                        initdata_procure.jun_Achievement = jun_Achievement;
                        initdata_procure.jul_Achievement = jul_Achievement;
                        initdata_procure.aug_Achievement = aug_Achievement;
                        initdata_procure.sep_Achievement = sep_Achievement;
                        initdata_procure.oct_Achievement = oct_Achievement;
                        initdata_procure.nov_Achievement = nov_Achievement;
                        initdata_procure.dec_Achievement = dec_Achievement;
                        initdata_procure.jan_ST_Price_effect = jan_ST_Price_effect;
                        initdata_procure.feb_ST_Price_effect = feb_ST_Price_effect;
                        initdata_procure.march_ST_Price_effect = march_ST_Price_effect;
                        initdata_procure.apr_ST_Price_effect = apr_ST_Price_effect;
                        initdata_procure.may_ST_Price_effect = may_ST_Price_effect;
                        initdata_procure.jun_ST_Price_effect = jun_ST_Price_effect;
                        initdata_procure.jul_ST_Price_effect = jul_ST_Price_effect;
                        initdata_procure.aug_ST_Price_effect = aug_ST_Price_effect;
                        initdata_procure.sep_ST_Price_effect = sep_ST_Price_effect;
                        initdata_procure.oct_ST_Price_effect = oct_ST_Price_effect;
                        initdata_procure.nov_ST_Price_effect = nov_ST_Price_effect;
                        initdata_procure.dec_ST_Price_effect = dec_ST_Price_effect;
                        initdata_procure.jan_ST_Volume_Effect = jan_ST_Volume_Effect;
                        initdata_procure.feb_ST_Volume_Effect = feb_ST_Volume_Effect;
                        initdata_procure.march_ST_Volume_Effect = march_ST_Volume_Effect;
                        initdata_procure.apr_ST_Volume_Effect = apr_ST_Volume_Effect;
                        initdata_procure.may_ST_Volume_Effect = may_ST_Volume_Effect;
                        initdata_procure.jun_ST_Volume_Effect = jun_ST_Volume_Effect;
                        initdata_procure.jul_ST_Volume_Effect = jul_ST_Volume_Effect;
                        initdata_procure.aug_ST_Volume_Effect = aug_ST_Volume_Effect;
                        initdata_procure.sep_ST_Volume_Effect = sep_ST_Volume_Effect;
                        initdata_procure.oct_ST_Volume_Effect = oct_ST_Volume_Effect;
                        initdata_procure.nov_ST_Volume_Effect = nov_ST_Volume_Effect;
                        initdata_procure.dec_ST_Volume_Effect = dec_ST_Volume_Effect;
                        initdata_procure.jan_FY_Secured_Target = jan_FY_Secured_Target;
                        initdata_procure.feb_FY_Secured_Target = feb_FY_Secured_Target;
                        initdata_procure.march_FY_Secured_Target = march_FY_Secured_Target;
                        initdata_procure.apr_FY_Secured_Target = apr_FY_Secured_Target;
                        initdata_procure.may_FY_Secured_Target = may_FY_Secured_Target;
                        initdata_procure.jun_FY_Secured_Target = jun_FY_Secured_Target;
                        initdata_procure.jul_FY_Secured_Target = jul_FY_Secured_Target;
                        initdata_procure.aug_FY_Secured_Target = aug_FY_Secured_Target;
                        initdata_procure.sep_FY_Secured_Target = sep_FY_Secured_Target;
                        initdata_procure.oct_FY_Secured_Target = oct_FY_Secured_Target;
                        initdata_procure.nov_FY_Secured_Target = nov_FY_Secured_Target;
                        initdata_procure.dec_FY_Secured_Target = dec_FY_Secured_Target;
                        initdata_procure.jan_CPI_Effect = jan_CPI_Effect;
                        initdata_procure.feb_CPI_Effect = feb_CPI_Effect;
                        initdata_procure.march_CPI_Effect = march_CPI_Effect;
                        initdata_procure.apr_CPI_Effect = apr_CPI_Effect;
                        initdata_procure.may_CPI_Effect = may_CPI_Effect;
                        initdata_procure.jun_CPI_Effect = jun_CPI_Effect;
                        initdata_procure.jul_CPI_Effect = jul_CPI_Effect;
                        initdata_procure.aug_CPI_Effect = aug_CPI_Effect;
                        initdata_procure.sep_CPI_Effect = sep_CPI_Effect;
                        initdata_procure.oct_CPI_Effect = oct_CPI_Effect;
                        initdata_procure.nov_CPI_Effect = nov_CPI_Effect;
                        initdata_procure.dec_CPI_Effect = dec_CPI_Effect;

                        initdata_procure.jan_CPI = jan_CPI;
                        initdata_procure.feb_CPI = feb_CPI;
                        initdata_procure.mar_CPI = mar_CPI;
                        initdata_procure.apr_CPI = apr_CPI;
                        initdata_procure.may_CPI = may_CPI;
                        initdata_procure.jun_CPI = jun_CPI;
                        initdata_procure.jul_CPI = jul_CPI;
                        initdata_procure.aug_CPI = aug_CPI;
                        initdata_procure.sep_CPI = sep_CPI;
                        initdata_procure.oct_CPI = oct_CPI;
                        initdata_procure.nov_CPI = nov_CPI;
                        initdata_procure.dec_CPI = dec_CPI;

                    }
                    //initdata.CreatedDate = DateTime.Now;
                    db.SaveChanges();

                    var dataTable = new DataTable();

                    dataTable = Session["Maingrid"] as DataTable;
                    return Content("saved|" + initdata.InitNumber);
                }
                catch (Exception E)
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
                var isProcurement = model.isProcurement;
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
            catch (Exception e)
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

            List<mcpi> mCPI = new List<mcpi>();

            Dictionary<int, string> _quaters = new Dictionary<int, string>();
            _quaters.Add(1, "1,2,3");
            _quaters.Add(2, "4,5,6");
            _quaters.Add(3, "7,8,9");
            _quaters.Add(4, "10,11,12");

            List<mcpi> _mCPI = new List<mcpi>();

            if (modelsubcountry != null)
            {
                mCPI = db.mcpi.Where(x => x.mCountry_id == modelsubcountry.CountryID).ToList();

                int i = 1;
                while (i <= 12)
                {
                    decimal final_CPI = 0;
                    decimal MONTHLY = 0;
                    decimal QUARTERLY = 0;
                    decimal ANNUALLY = 0;


                    mcpi obj_mcpi = new mcpi();

                    MONTHLY = mCPI.Where(x => x.Period_index == 1 && x.Period_type == "MONTHLY").Select(y => y.CPI).FirstOrDefault();

                    if (MONTHLY > 0)
                    {
                        final_CPI = MONTHLY;
                        obj_mcpi = (mcpi)mCPI.Where(x => x.Period_index == 1 && x.Period_type == "MONTHLY");
                    }
                    else
                    {
                        int _quarter = _quaters.Where(key => key.Value.Contains(i.ToString())).Select(y => y.Key).FirstOrDefault();

                        QUARTERLY = mCPI.Where(x => x.Period_index == _quarter && x.Period_type == "QUARTERLY").Select(y => y.CPI).FirstOrDefault();

                        if (QUARTERLY > 0)
                        {
                            final_CPI = QUARTERLY;
                        }
                        else
                        {
                            ANNUALLY = mCPI.Where(x => x.Period_index == 1 && x.Period_type == "ANNUALLY").Select(y => y.CPI).FirstOrDefault();

                            if (ANNUALLY > 0)
                            {
                                final_CPI = ANNUALLY;
                            }
                            else
                            {
                                final_CPI = 0;
                            }
                        }
                    }

                    obj_mcpi.Period_index = i;
                    obj_mcpi.Period_type = "MONTHLY";
                    obj_mcpi.CPI = final_CPI;
                    _mCPI.Add(obj_mcpi);

                    i++;
                }
            }

            db.Configuration.ProxyCreationEnabled = false;
            SubCountryList = db.msubcountries.Where(c => c.id == SubCountryID && c.isActive == "Y").Select(s => new SubCountryList { id = s.id, SubCountryName = s.SubCountryName }).ToList();
            CountryList = db.mcountries.Where(c => c.id == CountryID).Select(s => new CountryList { id = s.id, CountryName = s.CountryName }).ToList();
            if (GetInfo.Id2 == 0)
            {
                BrandList = db.mbrands.SqlQuery("SELECT a.id,a.brandname,c.CountryName,d.SubCountryName,a.isActive,a.isDeleted ,a.InitYear FROM mbrand a LEFT JOIN mbrandcountry b ON a.id = b.brandid LEFT JOIN mcountry c ON b.countryid=c.id LEFT JOIN msubcountry d ON d.CountryID = c.id AND d.id = b.subcountryid WHERE a.isActive = 'Y' and a.isDeleted='N' and c.id = " + CountryID + " AND d.id = " + SubCountryID + " ORDER BY c.CountryName,d.SubCountryName asc").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
            }
            else
            {
                // BrandList = db.mbrands.Where(c => c.id == modelinitiative.BrandID && c.isActive == "Y").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
                BrandList = db.mbrands.SqlQuery("SELECT a.id,a.brandname,c.CountryName,d.SubCountryName,a.isActive,a.isDeleted ,a.InitYear FROM mbrand a LEFT JOIN mbrandcountry b ON a.id = b.brandid LEFT JOIN mcountry c ON b.countryid=c.id LEFT JOIN msubcountry d ON d.CountryID = c.id AND d.id = b.subcountryid WHERE a.isActive = 'Y' and a.isDeleted='N' and c.id = " + CountryID + " AND d.id = " + SubCountryID + " ORDER BY c.CountryName,d.SubCountryName asc").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
            }
            RegionList = db.mregions.Where(c => c.id == RegionID).Select(s => new RegionList { id = s.id, RegionName = s.RegionName }).ToList();
            SubRegionList = db.msubregions.Where(c => c.id == SubRegionID).Select(s => new SubRegionList { id = s.id, SubRegionName = s.SubRegionName }).ToList();
            ClusterList = db.mclusters.Where(cl => cl.CountryID == CountryID && cl.RegionID == RegionID && cl.SubRegionID == SubRegionID && cl.ClusterName != "").Select(s => new ClusterList { id = s.id, ClusterName = s.ClusterName }).ToList();
            RegionalOfficeList = db.mregional_office.Where(ro => ro.RegionID == RegionID && ro.CountryID == CountryID).Select(s => new RegionalOfficeList { id = s.id, RegionalOfficeName = s.RegionalOffice_Name }).ToList();
            var costcontrolid = db.t_subctry_costcntrlsite.Where(sc => sc.subcountryid == SubCountryID).FirstOrDefault().costcontrolid;
            CostControlList = db.mcostcontrolsites.Where(c => c.id == costcontrolid).Select(s => new CostControlList { id = s.id, CostControlSiteName = s.CostControlSiteName }).ToList();
            LegalEntityList = db.mlegalentities.Where(le => le.CountryID == CountryID && le.BrandID == BrandIDx && le.SubCountryID == SubCountryID).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
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
                    TypeInitiativeData = TypeInitiativeList,
                    mcpi = _mCPI
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
                    TypeInitiativeData = null,
                    mcpi = null
                });

            }

            return Json(GDSC, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllDll(Models.GetInfoByIDModel GetInfo)
        {
            msubcountry modelsubcountry = new msubcountry();
            mcountry modelcountry = new mcountry();
            mregion modelregion = new mregion();
            t_initiative modelinitiative = new t_initiative();

            if (GetInfo.Id2 != 0)
                modelsubcountry = db.msubcountries.Where(sc => sc.id == GetInfo.Id2).FirstOrDefault();
            else
                modelsubcountry = db.msubcountries.FirstOrDefault();

            if (GetInfo.Id == 0)
            {
                modelinitiative = db.t_initiative.Where(init => init.SubCountryID == GetInfo.Id2).FirstOrDefault();
            }
            else
            {
                modelinitiative = db.t_initiative.Where(init => init.id == GetInfo.Id).FirstOrDefault();
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

            List<GetAllData> GDSC = new List<GetAllData>();
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
                BrandList = db.mbrands.SqlQuery("SELECT a.id,a.brandname,c.CountryName,d.SubCountryName,a.isActive,a.isDeleted ,a.InitYear FROM mbrand a LEFT JOIN mbrandcountry b ON a.id = b.brandid LEFT JOIN mcountry c ON b.countryid=c.id LEFT JOIN msubcountry d ON d.CountryID = c.id AND d.id = b.subcountryid WHERE a.isActive = 'Y' and a.isDeleted='N' and c.id = " + CountryID + " AND d.id = " + SubCountryID + " ORDER BY c.CountryName,d.SubCountryName asc").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
            }
            else
            {
                // BrandList = db.mbrands.Where(c => c.id == modelinitiative.BrandID && c.isActive == "Y").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
                BrandList = db.mbrands.SqlQuery("SELECT a.id,a.brandname,c.CountryName,d.SubCountryName,a.isActive,a.isDeleted ,a.InitYear FROM mbrand a LEFT JOIN mbrandcountry b ON a.id = b.brandid LEFT JOIN mcountry c ON b.countryid=c.id LEFT JOIN msubcountry d ON d.CountryID = c.id AND d.id = b.subcountryid WHERE a.isActive = 'Y' and a.isDeleted='N' and c.id = " + CountryID + " AND d.id = " + SubCountryID + " ORDER BY c.CountryName,d.SubCountryName asc").Select(s => new BrandList { id = s.id, BrandName = s.brandname }).ToList();
            }
            RegionList = db.mregions.Where(c => c.id == RegionID).Select(s => new RegionList { id = s.id, RegionName = s.RegionName }).ToList();
            SubRegionList = db.msubregions.Where(c => c.id == SubRegionID).Select(s => new SubRegionList { id = s.id, SubRegionName = s.SubRegionName }).ToList();
            ClusterList = db.mclusters.Where(cl => cl.CountryID == CountryID && cl.RegionID == RegionID && cl.SubRegionID == SubRegionID && cl.ClusterName != "").Select(s => new ClusterList { id = s.id, ClusterName = s.ClusterName }).ToList();
            RegionalOfficeList = db.mregional_office.Where(ro => ro.RegionID == RegionID && ro.CountryID == CountryID).Select(s => new RegionalOfficeList { id = s.id, RegionalOfficeName = s.RegionalOffice_Name }).ToList();
            var costcontrolid = db.t_subctry_costcntrlsite.Where(sc => sc.subcountryid == SubCountryID).FirstOrDefault().costcontrolid;
            CostControlList = db.mcostcontrolsites.Where(c => c.id == costcontrolid).Select(s => new CostControlList { id = s.id, CostControlSiteName = s.CostControlSiteName }).ToList();
            LegalEntityList = db.mlegalentities.Where(le => le.CountryID == CountryID && le.BrandID == BrandIDx && le.SubCountryID == SubCountryID).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
            //LegalEntityList = db.mlegalentities.Where(le => le.CountryID == CountryID).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
            if (modelinitiative != null)
            {
                TypeInitiativeList = db.msavingtypes.Where(st => st.id == modelinitiative.InitiativeType).Select(s => new TypeInitiativeList { id = s.id, SavingTypeName = s.SavingTypeName }).ToList();
            }
            else
            {
                TypeInitiativeList = db.msavingtypes.Select(s => new TypeInitiativeList { id = s.id, SavingTypeName = s.SavingTypeName }).ToList();
            }

            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var projYear = profileData.ProjectYear;
            if (profileData.ProjectYear < 2023)
                profileData.ProjectYear = 2022;

            db.Configuration.ProxyCreationEnabled = false;

            if (GetInfo.Id > 0)
            {
                var model = db.vwheaderinitiatives.Where(c => c.id == GetInfo.Id).FirstOrDefault();

                GDSC.Add(new GetAllData
                {
                    SavingTypeData = db.msavingtypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SavingTypeName,\'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, SavingTypeName,isActive  , InitYear From msavingtype where isActive = 'Y' and InitYear =" + projYear + "").ToList(),
                    ActionTypeData = db.mactiontypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS ActionTypeName, \'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, ActionTypeName, isActive ,InitYear From mactiontype where isActive = 'Y' and InitYear =" + projYear + "").ToList(),
                    SynImpactData = db.msynimpacts.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SynImpactName, \'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, SynImpactName, isActive ,InitYear From msynimpact where isActive = 'Y' and InitYear =" + projYear + "").ToList(),
                    InitStatusData = db.mstatus.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS Status, \'\' AS isActive , \'0\' AS  InitYear UNION ALL Select id, Status, isActive , InitYear From mstatus where isActive ='Y' and InitYear =" + projYear + "; ").ToList(),
                    PortNameData = db.mports.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS PortName , \'0\' AS  InitYear UNION ALL Select id, PortName , InitYear From mport  where InitYear =" + projYear + "").ToList(),

                    MSourceCategory = db.msourcecategories.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS categoryname  ,\'0\' AS  InitYear UNION ALL SELECT id, categoryname ,InitYear FROM msourcecategory  where InitYear =" + projYear + "").ToList(),



                    MSubCostData = db.msubcosts.SqlQuery("SELECT b.id,b.SubCostName,b.isActive ,  InitYear  FROM t_subcostbrand a LEFT JOIN msubcost b ON a.subcostid = b.id WHERE a.savingtypeid = " + model.InitiativeType + " AND a.costtypeid = " + model.CostCategoryID + " and b.isActive = 'Y' and  b.InitYear =" + projYear + "  GROUP BY b.id,b.SubCostName; ").ToList(),
                    //  MSourceCategory = db.msourcecategories.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS categoryname UNION ALL SELECT id, categoryname FROM msourcecategory").ToList()

                    MCostTypeData = db.mcosttypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS CostTypeName, \'\' as isActive,\'0\' AS  InitYear  UNION ALL SELECT b.id, b.CostTypeName,b.isActive , b.InitYear FROM t_subcostinitiative a LEFT JOIN mcosttype b ON a.costitemid = b.id WHERE a.savingtypeid = " + model.InitiativeType + " and b.isActive = 'Y' and  b.InitYear =" + projYear + " GROUP BY b.id, b.CostTypeName; ").ToList(),

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
            LegalEntityList = db.mlegalentities.Where(le => le.CountryID == CountryID && le.SubCountryID == SubCountryID && le.BrandID == BrandIDx).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
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
            List<CostControlList> CostControl = new List<CostControlList>();
            var costControlId = db.mlegalentities.Where(le => le.CountryID == GetInfo.CountryID && le.BrandID == GetInfo.BrandID && le.SubCountryID == GetInfo.SubCountryID).FirstOrDefault().CostControlSiteID;
            CostControl = db.mcostcontrolsites.Where(c => c.id == costControlId).Select(s => new CostControlList { id = s.id, CostControlSiteName = s.CostControlSiteName }).ToList();
            LegalEntity = db.mlegalentities.Where(c => c.BrandID == GetInfo.BrandID && c.CountryID == GetInfo.CountryID && c.SubCountryID == GetInfo.SubCountryID && c.CostControlSiteID == costControlId).Select(s => new LegalEntityList { id = s.id, LegalEntityName = s.LegalEntityName }).ToList();
            List<GetDataFromSubCountry> GDSC = new List<GetDataFromSubCountry>();
            GDSC.Add(new GetDataFromSubCountry
            {
                LegalEntityData = LegalEntity,
                CostControlSiteData = CostControl

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
                CostTypeData = db.mcosttypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS CostTypeName, \'\' as isActive , \'0\' AS  InitYear UNION ALL SELECT b.id, b.CostTypeName, b.isActive , b.InitYear FROM t_subcostinitiative a LEFT JOIN mcosttype b ON a.costitemid = b.id WHERE a.savingtypeid = " + SCID + " and b.isActive = \'Y\' GROUP BY b.id, b.CostTypeName").ToList()
            });
            return Json(GDFI, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemFromSubCost(Models.GetInfoByIDModel GetInfo)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var projYear = profileData.ProjectYear;
            long SCID = GetInfo.Id;
            List<GetItemCategoryDataFromInitiative> GDFI = new List<GetItemCategoryDataFromInitiative>();

            db.Configuration.ProxyCreationEnabled = false;
            GDFI.Add(new GetItemCategoryDataFromInitiative
            {
                ActionTypeData = db.mactiontypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS ActionTypeName,\'\' as isActive , \'0\' AS  InitYear UNION ALL SELECT c.id, c.ActionTypeName,c.isActive , c.InitYear FROM t_subcostactiontype a LEFT JOIN msubcost b ON a.subcostid = b.id LEFT JOIN mactiontype c ON a.actiontypeid = c.id WHERE b.id = " + SCID + " and c.isActive = \'Y\' and c.InitYear =" + projYear + "").ToList()
            });
            return Json(GDFI, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemFromCostCategory(Models.GetInfoByIDModel GetInfo)
        {
            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var projYear = profileData.ProjectYear;
            long SCID = GetInfo.Id; long SCID2 = GetInfo.Id2; long SCID3 = GetInfo.Id3;
            List<GetItemSubCategoryDataFromCategory> GDFC = new List<GetItemSubCategoryDataFromCategory>();

            db.Configuration.ProxyCreationEnabled = false;
            GDFC.Add(new GetItemSubCategoryDataFromCategory
            {
                SubCostData = db.msubcosts.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SubCostName, \'\' as isActive ,\'0\' AS  InitYear UNION ALL SELECT b.id,b.SubCostName,b.isActive , b.InitYear FROM t_subcostbrand a LEFT JOIN msubcost b ON a.subcostid = b.id WHERE a.savingtypeid = " + SCID + " AND a.costtypeid = " + SCID2 + " and b.isActive = \'Y\' GROUP BY b.id, b.SubCostName").ToList(),
                //  ActionTypeData = db.mactiontypes.SqlQuery("SELECT a.actiontypeid AS id,c.ActionTypeName, b.isActive ,  c.inityear FROM t_cost_actiontype a LEFT JOIN mcosttype b ON a.costitemid = b.id LEFT JOIN mactiontype c ON a.actiontypeid = c.id WHERE a.costitemid = " + SCID2 + " and c.isActive = \'Y\'; ").ToList()
            });
            return Json(GDFC, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInfoForPopUp(Models.GetInfoByIDModel GetInfo)
        {
            List<OutInitiative> GIFP = new List<OutInitiative>();

            var profileData = Session["DefaultGAINSess"] as LoginSession;
            var projYear = profileData.ProjectYear;
            if (profileData.ProjectYear < 2023)
                projYear = 2022;
            else
                projYear = 2023;


            db.Configuration.ProxyCreationEnabled = false;
            if (GetInfo.Id > 0)
            {
                var model = db.vwheaderinitiatives.Where(c => c.id == GetInfo.Id).FirstOrDefault();

                var savingdatayear = db.msavingtypes.Where(c => c.id == model.InitiativeType).Select(x => x.InitYear).FirstOrDefault();


                if (savingdatayear == 2022)
                    projYear = 2022;
                else
                    projYear = 2023;

                GIFP.Add(new OutInitiative
                {
                    SavingTypeData = db.msavingtypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SavingTypeName,\'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, SavingTypeName,isActive  , InitYear From msavingtype where isActive = 'Y' and InitYear =" + projYear + "").ToList(),
                    ActionTypeData = db.mactiontypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS ActionTypeName, \'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, ActionTypeName, isActive ,InitYear From mactiontype where isActive = 'Y' and InitYear =" + projYear + "").ToList(),
                    SynImpactData = db.msynimpacts.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SynImpactName, \'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, SynImpactName, isActive ,InitYear From msynimpact where isActive = 'Y' and InitYear =" + projYear + "").ToList(),
                    InitStatusData = db.mstatus.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS Status, \'\' AS isActive , \'0\' AS  InitYear UNION ALL Select id, Status, isActive , InitYear From mstatus where isActive ='Y' and InitYear =" + projYear + "; ").ToList(),
                    PortNameData = db.mports.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS PortName , \'0\' AS  InitYear UNION ALL Select id, PortName , InitYear From mport  where InitYear =" + projYear + "").ToList(),

                    MSourceCategory = db.msourcecategories.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS categoryname  ,\'0\' AS  InitYear UNION ALL SELECT id, categoryname ,InitYear FROM msourcecategory  where InitYear =" + projYear + "").ToList(),



                    MSubCostData = db.msubcosts.SqlQuery("SELECT b.id,b.SubCostName,b.isActive ,  b.InitYear  FROM t_subcostbrand a LEFT JOIN msubcost b ON a.subcostid = b.id WHERE a.savingtypeid = " + model.InitiativeType + " AND a.costtypeid = " + model.CostCategoryID + "  and b.isActive = 'Y' and  b.InitYear =" + projYear + "  GROUP BY b.id,b.SubCostName; ").ToList(),
                    //  MSourceCategory = db.msourcecategories.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS categoryname UNION ALL SELECT id, categoryname FROM msourcecategory").ToList()

                    MCostTypeData = db.mcosttypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS CostTypeName, \'\' as isActive,\'0\' AS  InitYear  UNION ALL SELECT b.id, b.CostTypeName,b.isActive , b.InitYear FROM t_subcostinitiative a LEFT JOIN mcosttype b ON a.costitemid = b.id WHERE a.savingtypeid = " + model.InitiativeType + " and b.isActive = 'Y' and  b.InitYear =" + projYear + " GROUP BY b.id, b.CostTypeName; ").ToList(),

                    //MSourceCategory = db.msourcecategories.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS categoryname ,\'0\' AS  InitYear UNION ALL SELECT id, categoryname , InitYear FROM msourcecategory where  InitYear =" + projYear + "").ToList()
                });
            }
            else
            {
                GIFP.Add(new OutInitiative
                {
                    SavingTypeData = db.msavingtypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SavingTypeName,\'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, SavingTypeName,isActive  , InitYear From msavingtype where isActive = 'Y' and InitYear =" + projYear + "").ToList(),
                    ActionTypeData = db.mactiontypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS ActionTypeName, \'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, ActionTypeName, isActive ,InitYear From mactiontype where isActive = 'Y' and InitYear =" + projYear + "").ToList(),
                    SynImpactData = db.msynimpacts.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SynImpactName, \'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, SynImpactName, isActive ,InitYear From msynimpact where isActive = 'Y' and InitYear =" + projYear + "").ToList(),

                    //  SavingTypeData = db.msavingtypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SavingTypeName, \'\' as isActive UNION ALL Select id, SavingTypeName,isActive From msavingtype where isActive = 'Y' ").ToList(),
                    // ActionTypeData = db.mactiontypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS ActionTypeName, \'\' as isActive UNION ALL Select id, ActionTypeName,isActive From mactiontype where isActive = 'Y' ").ToList(),
                    //  SynImpactData = db.msynimpacts.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS SynImpactName,\'\' as isActive UNION ALL Select id, SynImpactName,isActive From msynimpact where isActive = 'Y' ").ToList(),

                    InitStatusData = db.mstatus.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS Status, \'\' AS isActive , \'0\' AS  InitYear  UNION ALL Select id, Status, isActive ,InitYear From mstatus where isActive ='Y' and InitYear =" + projYear + "").ToList(),
                    PortNameData = db.mports.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS PortName , \'0\' AS  InitYear UNION ALL Select id, PortName ,InitYear From mport where InitYear =" + projYear + "").ToList(),
                    MCostTypeData = db.mcosttypes.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS CostTypeName, \'\' as isActive ,\'0\' AS  InitYear UNION ALL Select id, CostTypeName, isActive ,InitYear From mcosttype where isActive = 'Y' and InitYear =" + projYear + " ").ToList(),
                    MSourceCategory = db.msourcecategories.SqlQuery("SELECT \'0\' AS id, \'[Please Select]\' AS categoryname ,\'0\' AS  InitYear UNION ALL SELECT id, categoryname , InitYear FROM msourcecategory where  InitYear =" + projYear + "").ToList()
                });
            }
            return Json(GIFP, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CboYear()
        {
            List<myear> model = db.myears.Where(c => c.yrStatus == 1).ToList();
            return PartialView("~/Views/Shared/_CboYearPartial.cshtml", model);
        }
        public ActionResult CboMonth()
        {
            List<mMonth> model = new List<mMonth>();

            //model.Add(new mMonth(1, "jan"));
            //model.Add(new mMonth(2, "feb"));
            //model.Add(new mMonth(3, "mar"));
            //model.Add(new mMonth(4, "apr"));
            //model.Add(new mMonth(5, "may"));
            //model.Add(new mMonth(6, "jun"));
            //model.Add(new mMonth(7, "jul"));
            //model.Add(new mMonth(8, "aug"));
            //model.Add(new mMonth(9, "sep"));
            //model.Add(new mMonth(10, "oct"));
            //model.Add(new mMonth(11, "nov"));
            //model.Add(new mMonth(12, "dec"));

            model.Add(new mMonth(1, "JANUARY"));
            model.Add(new mMonth(2, "FEBRUARY"));
            model.Add(new mMonth(3, "MARCH"));
            model.Add(new mMonth(4, "APRIL"));
            model.Add(new mMonth(5, "MAY"));
            model.Add(new mMonth(6, "JUNE"));
            model.Add(new mMonth(7, "JULY"));
            model.Add(new mMonth(8, "AUGUST"));
            model.Add(new mMonth(9, "SEPTEMBER"));
            model.Add(new mMonth(10, "OCTOBER"));
            model.Add(new mMonth(11, "NOVEMBER"));
            model.Add(new mMonth(12, "DECEMBER"));


            return PartialView("~/Views/Shared/_CboMonthPartial.cshtml", model);
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
            LoginSession profileData = this.Session["DefaultGAINSess"] as LoginSession;
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
                            FileDiDB += filename + "|" + InitiativeNumber;
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

        #region ENH153-2

        public ActionResult getProcuementCalcs(GetInfoByIDModel GetInfo)
        {

            long t_initiative_id = GetInfo.Id;

            t_initiative_calcs model = db.t_initiative_calcs.Where(x => x.t_initiative_ID == t_initiative_id).FirstOrDefault();
            //List<t_initiative_calcs> model = db.t_initiative_calcs.Where(x => x.t_initiative_ID == t_initiative_id).ToList();

            //List<t_initiative_calcs> result = new List<t_initiative_calcs>();

            //foreach (t_initiative_calcs _model in model)
            //{
            //    t_initiative_calcs _result = new t_initiative_calcs
            //    {
            //        id = _model.id,
            //        t_initiative_ID = _model.t_initiative_ID,
            //        jan_Actual_CPU_Nmin1 = _model.jan_Actual_CPU_Nmin1
            //    };

            //    result.Add(_result);
            //}

            t_initiative_calcs result = new t_initiative_calcs();

            if (model != null)
            {
                result.id = model.id;
                result.t_initiative_ID = model.t_initiative_ID;

                result.jan_Actual_CPU_Nmin1 = model.jan_Actual_CPU_Nmin1;
                result.feb_Actual_CPU_Nmin1 = model.feb_Actual_CPU_Nmin1;
                result.march_Actual_CPU_Nmin1 = model.march_Actual_CPU_Nmin1;
                result.apr_Actual_CPU_Nmin1 = model.apr_Actual_CPU_Nmin1;
                result.may_Actual_CPU_Nmin1 = model.may_Actual_CPU_Nmin1;
                result.jun_Actual_CPU_Nmin1 = model.jun_Actual_CPU_Nmin1;
                result.jul_Actual_CPU_Nmin1 = model.jul_Actual_CPU_Nmin1;
                result.aug_Actual_CPU_Nmin1 = model.aug_Actual_CPU_Nmin1;
                result.sep_Actual_CPU_Nmin1 = model.sep_Actual_CPU_Nmin1;
                result.oct_Actual_CPU_Nmin1 = model.oct_Actual_CPU_Nmin1;
                result.nov_Actual_CPU_Nmin1 = model.nov_Actual_CPU_Nmin1;
                result.dec_Actual_CPU_Nmin1 = model.dec_Actual_CPU_Nmin1;
                result.jan_Target_CPU_N = model.jan_Target_CPU_N;
                result.feb_Target_CPU_N = model.feb_Target_CPU_N;
                result.march_Target_CPU_N = model.march_Target_CPU_N;
                result.apr_Target_CPU_N = model.apr_Target_CPU_N;
                result.may_Target_CPU_N = model.may_Target_CPU_N;
                result.jun_Target_CPU_N = model.jun_Target_CPU_N;
                result.jul_Target_CPU_N = model.jul_Target_CPU_N;
                result.aug_Target_CPU_N = model.aug_Target_CPU_N;
                result.sep_Target_CPU_N = model.sep_Target_CPU_N;
                result.oct_Target_CPU_N = model.oct_Target_CPU_N;
                result.nov_Target_CPU_N = model.nov_Target_CPU_N;
                result.dec_Target_CPU_N = model.dec_Target_CPU_N;
                result.jan_A_Price_effect = model.jan_A_Price_effect;
                result.feb_A_Price_effect = model.feb_A_Price_effect;
                result.march_A_Price_effect = model.march_A_Price_effect;
                result.apr_A_Price_effect = model.apr_A_Price_effect;
                result.may_A_Price_effect = model.may_A_Price_effect;
                result.jun_A_Price_effect = model.jun_A_Price_effect;
                result.jul_A_Price_effect = model.jul_A_Price_effect;
                result.aug_A_Price_effect = model.aug_A_Price_effect;
                result.sep_A_Price_effect = model.sep_A_Price_effect;
                result.oct_A_Price_effect = model.oct_A_Price_effect;
                result.nov_A_Price_effect = model.nov_A_Price_effect;
                result.dec_A_Price_effect = model.dec_A_Price_effect;
                result.jan_A_Volume_Effect = model.jan_A_Volume_Effect;
                result.feb_A_Volume_Effect = model.feb_A_Volume_Effect;
                result.march_A_Volume_Effect = model.march_A_Volume_Effect;
                result.apr_A_Volume_Effect = model.apr_A_Volume_Effect;
                result.may_A_Volume_Effect = model.may_A_Volume_Effect;
                result.jun_A_Volume_Effect = model.jun_A_Volume_Effect;
                result.jul_A_Volume_Effect = model.jul_A_Volume_Effect;
                result.aug_A_Volume_Effect = model.aug_A_Volume_Effect;
                result.sep_A_Volume_Effect = model.sep_A_Volume_Effect;
                result.oct_A_Volume_Effect = model.oct_A_Volume_Effect;
                result.nov_A_Volume_Effect = model.nov_A_Volume_Effect;
                result.dec_A_Volume_Effect = model.dec_A_Volume_Effect;
                result.jan_Achievement = model.jan_Achievement;
                result.feb_Achievement = model.feb_Achievement;
                result.march_Achievement = model.march_Achievement;
                result.apr_Achievement = model.apr_Achievement;
                result.may_Achievement = model.may_Achievement;
                result.jun_Achievement = model.jun_Achievement;
                result.jul_Achievement = model.jul_Achievement;
                result.aug_Achievement = model.aug_Achievement;
                result.sep_Achievement = model.sep_Achievement;
                result.oct_Achievement = model.oct_Achievement;
                result.nov_Achievement = model.nov_Achievement;
                result.dec_Achievement = model.dec_Achievement;
                result.jan_ST_Price_effect = model.jan_ST_Price_effect;
                result.feb_ST_Price_effect = model.feb_ST_Price_effect;
                result.march_ST_Price_effect = model.march_ST_Price_effect;
                result.apr_ST_Price_effect = model.apr_ST_Price_effect;
                result.may_ST_Price_effect = model.may_ST_Price_effect;
                result.jun_ST_Price_effect = model.jun_ST_Price_effect;
                result.jul_ST_Price_effect = model.jul_ST_Price_effect;
                result.aug_ST_Price_effect = model.aug_ST_Price_effect;
                result.sep_ST_Price_effect = model.sep_ST_Price_effect;
                result.oct_ST_Price_effect = model.oct_ST_Price_effect;
                result.nov_ST_Price_effect = model.nov_ST_Price_effect;
                result.dec_ST_Price_effect = model.dec_ST_Price_effect;
                result.jan_ST_Volume_Effect = model.jan_ST_Volume_Effect;
                result.feb_ST_Volume_Effect = model.feb_ST_Volume_Effect;
                result.march_ST_Volume_Effect = model.march_ST_Volume_Effect;
                result.apr_ST_Volume_Effect = model.apr_ST_Volume_Effect;
                result.may_ST_Volume_Effect = model.may_ST_Volume_Effect;
                result.jun_ST_Volume_Effect = model.jun_ST_Volume_Effect;
                result.jul_ST_Volume_Effect = model.jul_ST_Volume_Effect;
                result.aug_ST_Volume_Effect = model.aug_ST_Volume_Effect;
                result.sep_ST_Volume_Effect = model.sep_ST_Volume_Effect;
                result.oct_ST_Volume_Effect = model.oct_ST_Volume_Effect;
                result.nov_ST_Volume_Effect = model.nov_ST_Volume_Effect;
                result.dec_ST_Volume_Effect = model.dec_ST_Volume_Effect;
                result.jan_FY_Secured_Target = model.jan_FY_Secured_Target;
                result.feb_FY_Secured_Target = model.feb_FY_Secured_Target;
                result.march_FY_Secured_Target = model.march_FY_Secured_Target;
                result.apr_FY_Secured_Target = model.apr_FY_Secured_Target;
                result.may_FY_Secured_Target = model.may_FY_Secured_Target;
                result.jun_FY_Secured_Target = model.jun_FY_Secured_Target;
                result.jul_FY_Secured_Target = model.jul_FY_Secured_Target;
                result.aug_FY_Secured_Target = model.aug_FY_Secured_Target;
                result.sep_FY_Secured_Target = model.sep_FY_Secured_Target;
                result.oct_FY_Secured_Target = model.oct_FY_Secured_Target;
                result.nov_FY_Secured_Target = model.nov_FY_Secured_Target;
                result.dec_FY_Secured_Target = model.dec_FY_Secured_Target;
                result.jan_CPI_Effect = model.jan_CPI_Effect;
                result.feb_CPI_Effect = model.feb_CPI_Effect;
                result.march_CPI_Effect = model.march_CPI_Effect;
                result.apr_CPI_Effect = model.apr_CPI_Effect;
                result.may_CPI_Effect = model.may_CPI_Effect;
                result.jun_CPI_Effect = model.jun_CPI_Effect;
                result.jul_CPI_Effect = model.jul_CPI_Effect;
                result.aug_CPI_Effect = model.aug_CPI_Effect;
                result.sep_CPI_Effect = model.sep_CPI_Effect;
                result.oct_CPI_Effect = model.oct_CPI_Effect;
                result.nov_CPI_Effect = model.nov_CPI_Effect;
                result.dec_CPI_Effect = model.dec_CPI_Effect;

                //result.jan_CPI = _mCPI[0].CPI;
                //result.feb_CPI = _mCPI[1].CPI;
                //result.mar_CPI = _mCPI[2].CPI;
                //result.apr_CPI = _mCPI[3].CPI;
                //result.may_CPI = _mCPI[4].CPI;
                //result.jun_CPI = _mCPI[5].CPI;
                //result.jul_CPI = _mCPI[6].CPI;
                //result.aug_CPI = _mCPI[7].CPI;
                //result.sep_CPI = _mCPI[8].CPI;
                //result.oct_CPI = _mCPI[9].CPI;
                //result.nov_CPI = _mCPI[10].CPI;
                //result.dec_CPI = _mCPI[11].CPI;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult get_Monthly_CPI(GetInfoByIDModel GetInfo)
        {
            List<mcpi> mCPI = new List<mcpi>();

            Dictionary<int, string> _quaters = new Dictionary<int, string>();
            _quaters.Add(1, "1,2,3");
            _quaters.Add(2, "4,5,6");
            _quaters.Add(3, "7,8,9");
            _quaters.Add(4, "10,11,12");

            List<mcpi> _mCPI = new List<mcpi>();

            if (GetInfo != null)
            {
                mCPI = db.mcpi.Where(x => x.mCountry_id == GetInfo.Id).ToList();

                int i = 1;
                while (i <= 12)
                {
                    decimal final_CPI = 0;
                    decimal MONTHLY = 0;
                    decimal QUARTERLY = 0;
                    decimal ANNUALLY = 0;


                    mcpi obj_mcpi = new mcpi();

                    MONTHLY = mCPI.Where(x => x.Period_index == 1 && x.Period_type == "MONTHLY").Select(y => y.CPI).FirstOrDefault();

                    if (MONTHLY > 0)
                    {
                        final_CPI = MONTHLY;
                        obj_mcpi = (mcpi)mCPI.Where(x => x.Period_index == 1 && x.Period_type == "MONTHLY");
                    }
                    else
                    {
                        int _quarter = _quaters.Where(key => key.Value.Contains(i.ToString())).Select(y => y.Key).FirstOrDefault();

                        QUARTERLY = mCPI.Where(x => x.Period_index == _quarter && x.Period_type == "QUARTERLY").Select(y => y.CPI).FirstOrDefault();

                        if (QUARTERLY > 0)
                        {
                            final_CPI = QUARTERLY;
                        }
                        else
                        {
                            ANNUALLY = mCPI.Where(x => x.Period_index == 1 && x.Period_type == "ANNUALLY").Select(y => y.CPI).FirstOrDefault();

                            if (ANNUALLY > 0)
                            {
                                final_CPI = ANNUALLY;
                            }
                            else
                            {
                                final_CPI = 0;
                            }
                        }
                    }

                    obj_mcpi.Period_index = i;
                    obj_mcpi.Period_type = "MONTHLY";
                    obj_mcpi.CPI = final_CPI;
                    _mCPI.Add(obj_mcpi);

                    i++;
                }
            }

            return Json(_mCPI, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
