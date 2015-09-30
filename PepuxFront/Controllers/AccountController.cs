using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;
using Service = PepuxFront.Models.Service;

namespace PepuxFront.Controllers
{

    public class AccountController : Controller
    {
        public static string Uname;
        public static string SAMUname;
        public static string Ugroup;
        public static bool IsAuth;
        public static UserPrincipal currentuser;
        public ActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)

            #region old

//{

            //    if (!this.ModelState.IsValid)
            //    {
            //        return this.View(model);
            //    }

            //    if (Membership.ValidateUser(model.UserName, model.Password))
            //    {
            //        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            //        DatabsDataContext db = new DatabsDataContext();
            //        var uname = db.Service.FirstOrDefault(m => m.AdName == model.UserName);
            //        if (uname != null)
            //            Uname = (uname.UserName);
            //        var ugroup = db.Service.FirstOrDefault(m => m.AdName == model.UserName);
            //        if (ugroup != null)
            //            Ugroup = (ugroup.Role);
            //        IsAuth = true;
            //        if (Ugroup == "PepuxAdmins")
            //        {
            //            Debug.WriteLine("Вход выполнен " + model.UserName);
            //            return this.RedirectToAction("Index", "Controlpanel");

            //        }
            //        else if (Ugroup == "PepuxUsers")
            //        {
            //            Debug.WriteLine("Вход выполнен " + model.UserName);
            //            return this.RedirectToAction("Index", "User");
            //        }
            //        return this.RedirectToAction("Login", "Account");
            //    }

            //    this.ModelState.AddModelError(string.Empty, "Имя пользователя или пароль указаны неверно.");
            //    IsAuth = false;
            //    Uname = "test"; //null;
            //    return this.View(model);
            //}

            #endregion

        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            else
            {
                IpServiceLink.PServiceClient obj = new PServiceClient();
                Debug.WriteLine(model.Domen);
                Debug.WriteLine(model.UserName);
                Debug.WriteLine(model.Password);
                bool auth = obj.Authenticate(model.UserName, model.Password, model.Domen);
                Debug.WriteLine(auth);
                if (auth)
                {
                    IsAuth = auth;
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    var grps = GetGroups(model.UserName,model.Domen,model.Password);
                    ArrayList groups = new ArrayList();
                    
                    foreach (var grp in grps)
                    {
                        groups.Add(grp.Name);
                    }
                    
                    if (groups.Contains("PepuxAdmins"))
                    {
                       
                        Ugroup = "PepuxAdmins";
                        return this.RedirectToAction("Index", "Controlpanel");
                    }
                    else if (groups.Contains("PepuxUsers"))
                    {
                        Ugroup = "PepuxUsers";
                        return this.RedirectToAction("Index", "User");
                    }
                }
                else { this.ModelState.AddModelError(string.Empty, "Имя пользователя или пароль указаны неверно.");
                    return this.View(model); }
            }

            return null;
        }
        
        public List<GroupPrincipal> GetGroups(string userName, string domain, string pass)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();

            // establish domain context
            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain, domain, userName, pass);

            // find your user
            currentuser = UserPrincipal.FindByIdentity(yourDomain, userName);

            // if found - grab its groups
            if (currentuser != null)
            {
                PrincipalSearchResult<Principal> groups = currentuser.GetAuthorizationGroups();
                Uname = currentuser.DisplayName;
                SAMUname = currentuser.SamAccountName;

                // iterate over all groups
                foreach (Principal p in groups)
                {
                    // make sure to add only group principals
                    if (p is GroupPrincipal)
                    {
                        result.Add((GroupPrincipal)p);
                    }
                }
            }

            return result;
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            IsAuth = false;
            Uname = null;
            return this.RedirectToAction("Login", "Account");
        }
    }
}