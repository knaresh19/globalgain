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

        GAIN.Models.GainEntities db = new GAIN.Models.GainEntities();

        public ActionResult Index()
        {
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
                    if (AuthenticateLocal(model.UserName, model.Password))
                    {
                        LoginSession LoginSession = new LoginSession
                        {
                            ID = model.UserName,
                            ProjectYear = DateTime.Now.Year,
                            UserType = (int)isRegistered.userType,
                            CountryCode = isRegistered.COUNTRY_CODE
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
                                CountryCode = isRegistered.COUNTRY_CODE
                            };
                            this.Session["DefaultGAINSess"] = LoginSession;

                            return RedirectToAction("Index", "Home");
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("CustomError", "Username and password doesnot match!");
                            return RedirectToAction("Index", "Login");
                        }

                    }
                }
            }
            ModelState.AddModelError("CustomError", "User was not registered in the system");
            return RedirectToAction("Index", "Login");
        }
        #endregion

        #region AuthenticateLocal
        public static bool AuthenticateLocal(string username, string password, bool isEncrypt = true)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;
            else
            {
                using (GainEntities db = new GainEntities())
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
            return RedirectToAction("Index", "Login");
        }
        #endregion
    }
}