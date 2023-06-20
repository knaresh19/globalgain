using DevExpress.Web.Mvc;
using GAIN.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DevExpress.Spreadsheet;
using System.Web;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace GAIN.Controllers
{
    public class MUsersController : MyBaseController
    {
        private GAIN.Models.GainEntities db;


        public MUsersController()
        {
            db = new GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        }

        // GET: MUsers
        public ActionResult Index()
        {
            SetReferenceData();
            return View();
        }

        #region GrdMUsersPartial
        [ValidateInput(false)]
        public ActionResult GrdMUsersPartial()
        {
            var model = db.user_list;
            SetReferenceData();
            return PartialView("_GrdMUsersPartial", model.ToList());
        }
        #endregion

        #region GrdMUsersPartialAddNew
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMUsersPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.user_list item)
        {
            var model = db.user_list;
            var profileData = Session["DefaultGAINSess"] as LoginSession;

            ProcessOthersData(ref item);

            SetReferenceData();

            ClearSomeErrorState();
            //ValidateModel(item);

            item.USER_MIDDLE_NAME = "-";
            item.DEPARTMENT_CODE = "-";
            item.PICTURE_PATH = "-";
            if (String.IsNullOrEmpty(item.USER_LAST_NAME))
            {
                item.USER_LAST_NAME = "";
            }

            if(string.IsNullOrEmpty(item.region_right))
            {
                item.region_right = "";
            }
            if (string.IsNullOrEmpty(item.subregion_right))
            {
                item.subregion_right = "";
            }

            if (string.IsNullOrEmpty(item.RegionalOffice_right))
            {
                item.RegionalOffice_right = "";
            }

            if (string.IsNullOrEmpty(item.CostItem_right))
            {
                item.CostItem_right = "";
            }

            if (string.IsNullOrEmpty(item.SubCostItem_right))
            {
                item.SubCostItem_right = "";
            }

            if (string.IsNullOrEmpty(item.Brand_right))
            {
                item.Brand_right = "";
            }
            //Copy the cost control site right to cost control site

            item.costcontrolsite = item.CostControlSite_right;
            item.updatedBy = profileData.ID;
            item.updatedDate = System.DateTime.Now;
            if (TryValidateModel(item))
            {
                DbEntityValidationResult resultVal = db.Entry(item).GetValidationResult();
                if (resultVal.IsValid)
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
                {
                    StringBuilder errorMsg = new StringBuilder("");
                    foreach (DbValidationError error in resultVal.ValidationErrors)
                    {
                        errorMsg.Append(error.ErrorMessage + " ");
                    }
                    ViewData["EditError"] = errorMsg.ToString();
                }
            }
            else
            {
                StringBuilder errorMsg = new StringBuilder();
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errorMsg.AppendLine(error.ErrorMessage);
                        errorMsg.Append(", ");
                        //DoSomethingWith(error);
                    }
                }
                string strErrorMsg = errorMsg.ToString().TrimEnd(new char[] { ',', ' ' });

                ViewData["EditError"] = String.Format("Please, correct all errors. ({0})", strErrorMsg);
            }

            ViewData["EditableUser"] = item;
            return PartialView("_GrdMUsersPartial", model.ToList());
        }
        #endregion

        #region GrdMUsersPartialUpdate
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMUsersPartialUpdate(GAIN.Models.user_list item)
        {
            var model = db.user_list;
            var profileData = Session["DefaultGAINSess"] as LoginSession;

            if (Request.Form["user_id"] != null)
            {
                string s = Request.Form["user_id"];
                item.user_id = int.Parse(s.Trim(new char[] { '"', '\\' }));
            }

            ProcessOthersData(ref item);

            SetReferenceData();

            ClearSomeErrorState();
            /*try
            {
                ValidateModel(item);

            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }*/

            item.USER_MIDDLE_NAME = "-";
            item.DEPARTMENT_CODE = "-";
            item.PICTURE_PATH = "-";
            if (String.IsNullOrEmpty(item.USER_LAST_NAME))
            {
                item.USER_LAST_NAME = "";
            }
            if (string.IsNullOrEmpty(item.RegionalOffice_right))
            {
                item.RegionalOffice_right = "";
            }

            if (string.IsNullOrEmpty(item.CostItem_right))
            {
                item.CostItem_right = "";
            }

            if (string.IsNullOrEmpty(item.SubCostItem_right))
            {
                item.SubCostItem_right = "";
            }
            if (string.IsNullOrEmpty(item.Brand_right))
            {
                item.Brand_right = "";
            }

            if (TryValidateModel(item))
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

                        modelItem.COUNTRY_CODE = item.COUNTRY_CODE;
                        modelItem.Company_code = item.Company_code;
                        modelItem.userType = item.userType;
                        modelItem.istoadmin = item.istoadmin;
                        modelItem.isNew = item.isNew;
                        modelItem.status = item.status;

                        modelItem.country_right = item.country_right;
                        modelItem.subcountry_right = item.subcountry_right;
                        modelItem.region_right = item.region_right;
                        modelItem.subregion_right = item.subregion_right;
                        modelItem.RegionalOffice_right = item.RegionalOffice_right;
                        //modelItem.costcontrolsite = item.costcontrolsite;
                        modelItem.costcontrolsite = item.CostControlSite_right;
                        modelItem.CostControlSite_right = item.CostControlSite_right;
                        modelItem.Brand_right = item.Brand_right;
                        modelItem.CostItem_right = item.CostItem_right;
                        modelItem.SubCostItem_right = item.SubCostItem_right;
                        modelItem.validity_right = item.validity_right;
                        modelItem.confidential_right = item.confidential_right;
                        modelItem.years_right = item.years_right;
                        modelItem.updatedBy = profileData.ID;
                        modelItem.updatedDate = System.DateTime.Now;
                        //modelItem
                        //modelItem.encPassword = GAIN.Controllers.LoginController.GetSha1(item.encPassword.Trim());
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                StringBuilder errorMsg = new StringBuilder();
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errorMsg.AppendLine(error.ErrorMessage);
                        errorMsg.Append(" ");
                        //DoSomethingWith(error);
                    }
                }
                string strErrorMsg = errorMsg.ToString();

                ViewData["EditError"] = String.Format("Please, correct all errors. ({0})", strErrorMsg);
            }

            ViewData["EditableUser"] = item;


            return PartialView("_GrdMUsersPartial", model.ToList());
        }
        #endregion

        #region GrdMUsersPartialDelete
        [HttpPost, ValidateInput(false)]
        public ActionResult GrdMUsersPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] GAIN.Models.user_list itemx)
        {
            var model = db.user_list;

            SetReferenceData();

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
        #endregion

        #region ClearSomeErrorState
        private void ClearSomeErrorState()
        {
            ModelState.Remove("user_id");
            ModelState.Remove("isNew");
            ModelState.Remove("istoadmin");
            ModelState.Remove("userType");
            ModelState.Remove("status");
            ModelState.Remove("validity_right");
            ModelState.Remove("confidential_right");
            ModelState.Remove("USER_MIDDLE_NAME");
            ModelState.Remove("USER_LAST_NAME");

        }
        #endregion

        #region UploadProcess
        [HttpPost]
        public ActionResult UploadProcess(HttpPostedFileBase file)
        {
            LoginSession profileData = Session["DefaultGAINSess"] as LoginSession;

            string[] AllowedFileExtensions = new string[] { ".xlsx", ".xls" };
            long DefaultMaxFileSize = 4194304;

            UploadStatusExportXls exportResult = null;

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    String path = ConfigurationSettings.AppSettings["UPLOADEDFILEUSERPATH"];
                    FileInfo fileInfo = new FileInfo(file.FileName);
                    string fileName = file.FileName.ToLower();
                    bool isValidExtenstion = AllowedFileExtensions.Any(ext =>
                    {
                        return fileName.LastIndexOf(ext) > -1;
                    });

                    if (isValidExtenstion)
                    {
                        string resultFileName = Path.GetRandomFileName() + "_" + file.FileName;
                        string resultFilePath = path + resultFileName;
                        //string resultFilePath = HttpContext.Request.MapPath(resultFileUrl);

                        file.SaveAs(resultFilePath);

                        ExportUserListExcelToDb importTool = new ExportUserListExcelToDb(db);

                        exportResult = importTool.UserExcelDataToDatabase(resultFilePath, profileData.ID);
                        if (!String.IsNullOrWhiteSpace(exportResult.error_message))
                            ViewData["EditError"] = exportResult.error_message;


                        if (exportResult.row_status.Count() > 0)
                        {
                            foreach (ExportUserPerRowStatus m in exportResult.row_status)
                            {
                                if (m.status == "OK" || m.status == "EXIST")
                                {


                                }
                            }
                        }

                        UploadingUtils.RemoveFileWithDelay(resultFileName, resultFilePath, 5);
                        //us.filename = e.UploadedFile.FileName;

                        //ViewData["EditSuccess"] = "User is Invalid!";
                    }
                    else
                    {
                        ViewData["EditError"] = "Invalid file format!!";
                    }



                }
                catch (Exception ex)
                {
                    ViewData["EditError"] = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewData["EditError"] = "You have not specified a file.";
            }

            return View(exportResult);
        }
        #endregion

        #region SetReferenceData
        private void SetReferenceData()
        {
            List<string> listSubCountry = new List<string>();
            if (Session["SESS_LISTSUBCOUNTRIES"] != null)
            {
                listSubCountry = Session["SESS_LISTSUBCOUNTRIES"] as List<string>;
            }
            else
            {
                listSubCountry = db.msubcountries.OrderBy(o => o.SubCountryName).Select(x => x.SubCountryName).ToList();
                listSubCountry.Insert(0, "ALL");
            }

            List<SimpleModel> listCountry = new List<SimpleModel>();
            if (Session["SESS_LISTCOUNTRIES"] != null)
            {
                listCountry = Session["SESS_LISTCOUNTRIES"] as List<SimpleModel>;
            }
            else
            {
                listCountry = db.mcountries.OrderBy(o => o.CountryName).Select(x => new SimpleModel { id = (int)x.id, def = x.CountryName }).ToList();
                listCountry.Insert(0, new SimpleModel { id = 0, def = "ALL" });
            }


            List<SimpleModel> listRegion = new List<SimpleModel>();
            if (Session["SESS_LISTREGIONS"] != null)
            {
                listRegion = Session["SESS_LISTREGIONS"] as List<SimpleModel>;
            }
            else
            {
                listRegion = db.mregions.Where(f => f.isActive == 1).OrderBy(o => o.RegionName).Select(x => new SimpleModel { id = (int)x.id, def = x.RegionName }).ToList();
                listRegion.Insert(0, new SimpleModel { id = 0, def = "ALL" });
            }

            List<SimpleModel2> listSubRegion = new List<SimpleModel2>();
            if (Session["SESS_LISTSUBREGIONS"] != null)
            {
                listSubRegion = Session["SESS_LISTSUBREGIONS"] as List<SimpleModel2>;
            }
            else
            {
                listSubRegion = db.msubregions.OrderBy(o => o.SubRegionName).Select(x => new SimpleModel2 { id = (int)x.id, parent_id = (int)x.RegionID, def = x.SubRegionName }).ToList();
                listSubRegion.Insert(0, new SimpleModel2 { id = 0, parent_id = 0, def = "ALL" });
            }


            List<SimpleModel> listBrand = new List<SimpleModel>();
            if (Session["SESS_LISTBRANDS"] != null)
            {
                listBrand = Session["SESS_LISTBRANDS"] as List<SimpleModel>;
            }
            else
            {
                listBrand = db.mbrands.OrderBy(o => o.brandname).Select(x => new SimpleModel { id = (int)x.id, def = x.brandname }).ToList();
                listBrand.Insert(0, new SimpleModel { id = 0, def = "ALL" });
            }


            List<RegionalOfficeDto> listRegionOffice = new List<RegionalOfficeDto>();
            if (Session["SESS_LISTREGIONOFFICES"] != null)
            {
                listRegionOffice = Session["SESS_LISTREGIONOFFICES"] as List<RegionalOfficeDto>;
            }
            else
            {
                listRegionOffice = db.mregional_office.GroupBy(g => g.RegionalOffice_Name).OrderBy(o => o.Key).Select(x => new RegionalOfficeDto { id = 0, RegionID = 0, CountryID = 0, SubCountryID = 0, BrandID = 0, RegionalOffice_Name = x.Key }).ToList();
                //listRegionOffice = db.mregional_office.OrderBy(o => o.RegionalOffice_Name).Select(x => new RegionalOfficeDto { id = x.id, RegionID = x.RegionID, CountryID = x.CountryID, SubCountryID = x.SubCountryID, BrandID = x.SubCountryID, RegionalOffice_Name = x.Key }).ToList();
                listRegionOffice.Insert(0, new RegionalOfficeDto { id = 0, RegionID = 0, CountryID = 0, BrandID = 0, SubCountryID = 0, RegionalOffice_Name = "ALL" });
            }


            List<string> listCostControlSite = new List<string>();
            if (Session["SESS_LISTCOSTCONTROLSITES"] != null)
            {
                listCostControlSite = Session["SESS_LISTCOSTCONTROLSITES"] as List<string>;
            }
            else
            {
                listCostControlSite = db.mcostcontrolsites.OrderBy(o => o.CostControlSiteName).Select(x => x.CostControlSiteName).ToList();
                listCostControlSite.Insert(0, "ALL");
            }


            List<string> listCostItemType = new List<string>();
            if (Session["SESS_LISTCOSTITEMTYPES"] != null)
            {
                listCostItemType = Session["SESS_LISTCOSTITEMTYPES"] as List<string>;
            }
            else
            {
                listCostItemType = db.mcosttypes.OrderBy(o => o.CostTypeName).Select(x => x.CostTypeName).ToList();
                listCostItemType.Insert(0, "ALL");
            }


            List<string> listSubCostItem = new List<string>();
            if (Session["SESS_LISTSUBCOSTITEMS"] != null)
            {
                listSubCostItem = Session["SESS_LISTSUBCOSTITEMS"] as List<string>;
            }
            else
            {
                listSubCostItem = db.msubcosts.Select(x => x.SubCostName).ToList();
                listSubCostItem.Insert(0, "ALL");
            }

            ViewData["LISTSUBCOUNTRIES"] = listSubCountry;
            ViewData["LISTCOUNTRIES"] = listCountry;
            ViewData["LISTREGIONS"] = listRegion;
            ViewData["LISTSUBREGIONS"] = listSubRegion;
            ViewData["LISTBRANDS"] = listBrand;
            ViewData["LISTREGIONOFFICES"] = listRegionOffice;
            ViewData["LISTCOSTCONTROLSITES"] = listCostControlSite;
            ViewData["LISTCOSTITEMTYPES"] = listCostItemType;
            ViewData["LISTSUBCOSTITEMS"] = listSubCostItem;

        }
        #endregion

        #region ProcessOthersData
        private void ProcessOthersData(ref user_list model)
        {
            //model.user_id = int.Parse(Request.Form["user_id"]);

            string[] tokens = TokenBoxExtension.GetSelectedValues<string>("subcountry_right");
            if (tokens.Count() > 0)
            {
                model.subcountry_right = string.Join("|", tokens);
                model.subcountry_right = Formatdata(model.subcountry_right);
            }

            else
                model.subcountry_right = null;

            tokens = TokenBoxExtension.GetSelectedValues<string>("country_right");
            if (tokens.Count() > 0)
            {
                model.country_right = string.Join("|", tokens);
                model.country_right = Formatdata(model.country_right);
            }
            else
                model.country_right = null;

            tokens = TokenBoxExtension.GetSelectedValues<string>("region_right");
            if (tokens.Count() > 0)
            {
                model.region_right = string.Join("|", tokens);
                model.region_right = Formatdata(model.region_right);
            }
            else
                model.region_right = null;

            tokens = TokenBoxExtension.GetSelectedValues<string>("subregion_right");
            if (tokens.Count() > 0)
            {
                model.subregion_right = string.Join("|", tokens);
                model.subregion_right = Formatdata(model.subregion_right);
            }
            else
                model.subregion_right = null;

            tokens = TokenBoxExtension.GetSelectedValues<string>("RegionalOffice_right");
            if (tokens.Count() > 0)
            {
                model.RegionalOffice_right = string.Join("|", tokens);
                model.RegionalOffice_right = Formatdata(model.RegionalOffice_right);
            }
            else
                model.RegionalOffice_right = null;

            tokens = TokenBoxExtension.GetSelectedValues<string>("Brand_right");
            if (tokens.Count() > 0)
            {
                model.Brand_right = string.Join("|", tokens);
                model.Brand_right = Formatdata(model.Brand_right);
            }
            else
                model.Brand_right = null;

            tokens = TokenBoxExtension.GetSelectedValues<string>("CostItem_right");
            if (tokens.Count() > 0)
            {
                model.CostItem_right = string.Join("|", tokens);
                model.CostItem_right = Formatdata(model.CostItem_right);
            }
            else
                model.CostItem_right = null;

            tokens = TokenBoxExtension.GetSelectedValues<string>("SubCostItem_right");
            if (tokens.Count() > 0)
            {
                model.SubCostItem_right = string.Join("|", tokens);
                model.SubCostItem_right = Formatdata(model.SubCostItem_right);
            }
            else
                model.SubCostItem_right = null;

            tokens = TokenBoxExtension.GetSelectedValues<string>("CostControlSite_right");
            if (tokens.Count() > 0)
            {
                model.CostControlSite_right = string.Join("|", tokens);
                model.CostControlSite_right = Formatdata(model.CostControlSite_right);
            }
            else
                model.CostControlSite_right = null;

            if (Request.Params["isNew"] != null && !String.IsNullOrWhiteSpace(Request.Params["isNew"]))
                model.isNew = ComboBoxExtension.GetValue<int>("isNew");

            if (Request.Params["istoadmin"] != null && !String.IsNullOrWhiteSpace(Request.Params["istoadmin"]))
                model.istoadmin = ComboBoxExtension.GetValue<int>("istoadmin");

            if (Request.Params["status"] != null && !String.IsNullOrWhiteSpace(Request.Params["status"]))
                model.status = ComboBoxExtension.GetValue<int>("status");

            if (Request.Params["validity_right"] != null && !String.IsNullOrWhiteSpace(Request.Params["validity_right"]))
                model.validity_right = ComboBoxExtension.GetValue<int>("validity_right");

            if (Request.Params["confidential_right"] != null && !String.IsNullOrWhiteSpace(Request.Params["confidential_right"]))
                model.confidential_right = ComboBoxExtension.GetValue<int>("confidential_right");

            if (Request.Params["userType"] != null && !String.IsNullOrWhiteSpace(Request.Params["userType"]))
                model.userType = ComboBoxExtension.GetValue<int>("userType");
        }
        #endregion

        private string Formatdata(string Value)
        {
            if(!Value.StartsWith("|"))
            {
                Value = "|" + Value;
            }
            if(!Value.EndsWith("|"))
            {
                Value = Value + "|";
            }

            return Value;
        }

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

    }
}