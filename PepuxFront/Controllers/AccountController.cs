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
using Telerik.OpenAccess.Metadata.Fluent;
using Service = PepuxFront.Models.Service;

namespace PepuxFront.Controllers
{

    public class AccountController : Controller
    {
        public static string Uname;
        public static string SAMUname;
        public static string Ugroup;
        public static bool IsAuth;
        public static int UID;
        public static UserPrincipal currentuser;
        
        
          
        public ActionResult Login()
        {
            ViewBag.Auth = false;
            return this.View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)

        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            else
            {
                
                IpServiceLink.PServiceClient obj = new PServiceClient();
                bool auth = obj.Authenticate(model.UserName, model.Password, model.Domen);
                CurrentUser Cui = new CurrentUser();
                if (auth)
                {
                    IsAuth = auth;
                    Cui.isAuth = auth;
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    var grps = GetGroups(model.UserName, model.Domen, model.Password);
                    ArrayList groups = new ArrayList();
                    foreach (var grp in grps)
                    {
                        groups.Add(grp.Name);
                    }
                    if (groups.Contains("PepuxAdmins"))
                    {
                        Ugroup  = "PepuxAdmins";
                        return this.RedirectToAction("Index", "Controlpanel", ViewBag.Auth = "true");
                    }
                    else if (groups.Contains("PepuxUsers"))
                    {
                        Ugroup = "PepuxUsers";
                        return this.RedirectToAction("Phonebook", "Phonebook", ViewBag.Auth = "true");
                    }
                }
                else { this.ModelState.AddModelError(string.Empty, "Имя пользователя или пароль указаны неверно.");
                    return this.View(model); }
            }

            return null;
        }
        
        public  List<GroupPrincipal> GetGroups(string userName, string domain, string pass)
        {
            List<GroupPrincipal> result = new List<GroupPrincipal>();

            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain, domain, null, ContextOptions.SimpleBind, "admin", "Ciscocisco123");

            currentuser = UserPrincipal.FindByIdentity(yourDomain, IdentityType.SamAccountName, userName);

            if (currentuser != null)
            {
              ViewBag.Name = currentuser.DisplayName;
              TempData["user"] = currentuser;
                SAMUname= currentuser.SamAccountName;
                 UID= (GetAllPB().FirstOrDefault(m => m.samaccountname == currentuser.SamAccountName)).Id;
                PrincipalSearchResult<Principal> groups = currentuser.GetAuthorizationGroups();
                foreach (Principal p in groups)
                {
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
        private IEnumerable<IpServiceLink.PhonebookDB> GetAllPB()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetPB();
            return data;
        }
    }
}