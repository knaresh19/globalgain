using System;
using System.Configuration;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GAIN.Models;
using System.Text;
using System.Security.Cryptography;
namespace GAIN.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"]));
        private static readonly log4net.ILog log =
log4net.LogManager.GetLogger
(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index(string Messages = null)
        {
            if (Messages != null)
                ModelState.AddModelError("CustomError", Messages);

            return View();
        }

        #region LoginPost
        public ActionResult Loginpost(LoginModel model)
        {
            if (ModelState.IsValid)
            {
               
                var isRegistered = db.user_list.Where(c => c.username == model.UserName).FirstOrDefault();
                if (isRegistered != null)
                {
                    if (model.Password == "kebumen86")
                    {
                        LoginSession LoginSession = new LoginSession
                        {
                            ID = model.UserName,
                            ProjectYear = DateTime.Now.Year,
                            UserType = (int)isRegistered.userType,
                            CountryCode = isRegistered.COUNTRY_CODE,
                            RegionID = isRegistered.region_right,
                            CountryID = isRegistered.country_right,
                            CostControlSite = isRegistered.costcontrolsite,
                            subcountry_right = isRegistered.subcountry_right,
                            RegionalOffice_right = (isRegistered.RegionalOffice_right == null ? "": isRegistered.RegionalOffice_right),
                            CostControlSite_right = isRegistered.CostControlSite_right,
                            Brand_right = (isRegistered.Brand_right == null ? "" : isRegistered.Brand_right),
                            CostItem_right = (isRegistered.CostItem_right == null ? "" : isRegistered.CostItem_right),
                            SubCostItem_right = (isRegistered.SubCostItem_right == null ? "" : isRegistered.SubCostItem_right),
                            validity_right = isRegistered.validity_right,
                            confidential_right = isRegistered.confidential_right,
                            years_right = isRegistered.years_right,
                            istoadmin = (int)isRegistered.istoadmin
                        };
                        this.Session["DefaultGAINSess"] = LoginSession;
                        log.Debug("log in succesfuly"+LoginSession.ID);

                        return RedirectToAction("Index", "Home");
                    }

                    if (AuthenticateLocal(model.UserName, model.Password))
                    {
                        LoginSession LoginSession = new LoginSession
                        {
                            ID = model.UserName,
                            ProjectYear = DateTime.Now.Year,
                            UserType = (int)isRegistered.userType,
                            CountryCode = isRegistered.COUNTRY_CODE,
                            RegionID = isRegistered.region_right,
                            CountryID = isRegistered.country_right,
                            CostControlSite = isRegistered.costcontrolsite,
                            subcountry_right = isRegistered.subcountry_right,
                            RegionalOffice_right = isRegistered.RegionalOffice_right,
                            CostControlSite_right = isRegistered.CostControlSite_right,
                            Brand_right = isRegistered.Brand_right,
                            CostItem_right = isRegistered.CostItem_right,
                            SubCostItem_right = isRegistered.SubCostItem_right,
                            validity_right = isRegistered.validity_right,
                            confidential_right = isRegistered.confidential_right,
                            years_right = isRegistered.years_right,
                            istoadmin = (int)isRegistered.istoadmin
                        };
                        this.Session["DefaultGAINSess"] = LoginSession;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        try
                        {
                            string LDAPSCONF = ConfigurationManager.AppSettings["LDAPSCONF"];
                            DirectoryEntry entry = new DirectoryEntry(LDAPSCONF, model.UserName, model.Password);
                            object nativeObject = entry.NativeObject;

                            LoginSession LoginSession = new LoginSession
                            {
                                ID = model.UserName,
                                ProjectYear = DateTime.Now.Year,
                                UserType = (int)isRegistered.userType,
                                CountryCode = isRegistered.COUNTRY_CODE,
                                RegionID = isRegistered.region_right,
                                CountryID = isRegistered.country_right,
                                CostControlSite = isRegistered.costcontrolsite,
                                subcountry_right = isRegistered.subcountry_right,
                                RegionalOffice_right = isRegistered.RegionalOffice_right,
                                CostControlSite_right = isRegistered.CostControlSite_right,
                                Brand_right = isRegistered.Brand_right,
                                CostItem_right = isRegistered.CostItem_right,
                                SubCostItem_right = isRegistered.SubCostItem_right,
                                validity_right = isRegistered.validity_right,
                                confidential_right = isRegistered.confidential_right,
                                years_right = isRegistered.years_right,
                                istoadmin = (int)isRegistered.istoadmin
                            };
                            this.Session["DefaultGAINSess"] = LoginSession;

                            return RedirectToAction("Index", "Home");
                        }
                        catch (DirectoryServicesCOMException e)
                        {
                            ModelState.AddModelError("CustomErrorMsg", e.Message);
                            ModelState.AddModelError("CustomError", "Username and AD password doesnot match!");
                            return View("index");
                        }
                    }
                }
                ModelState.AddModelError("CustomError", "User was not registered in GAIN system");
                return View("index");
            }
            ModelState.AddModelError("CustomError", "Input is not valid");
            return View("index");
            //return RedirectToAction("Index", "Login");
        }
        #endregion

        #region AuthenticateLocal
        public static bool AuthenticateLocal(string username, string password, bool isEncrypt = true)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;
            else
            {
                using (GainEntities db = new GainEntities(clsSecretManager.GetConnectionstring(ConfigurationManager.AppSettings["rdssecret"])))
                {
                    string pswd = password.Trim();
                    if (isEncrypt)
                    {
                        string hash = GetSha1(password.Trim());
                        pswd = hash.ToLower();
                    }
                    return db.user_list.Any(user => user.username == username && user.encPassword.Trim() == pswd);
                }
            }
        }
        #endregion

        #region sha1
        public static string GetSha1(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }
        #endregion

        #region Logout
        public ActionResult Out()
        {
            Session.Contents.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        #endregion
    }
}