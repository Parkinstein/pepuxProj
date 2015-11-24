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

        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            else
            {
                IpServiceLink.PServiceClient obj = new PServiceClient();
                
                bool auth = obj.Authenticate(model.UserName, model.Password, model.Domen);
                if (auth)
                {
                    IsAuth = auth;
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    var grps = GetGroups(model.UserName, model.Domen, model.Password);
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
                        return this.RedirectToAction("Phonebook", "Phonebook");
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

            PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain, domain, null, ContextOptions.SimpleBind, "admin", "Ciscocisco123");

            currentuser = UserPrincipal.FindByIdentity(yourDomain, IdentityType.SamAccountName, userName);

            if (currentuser != null)
            {
                Uname = currentuser.DisplayName;
                SAMUname = currentuser.SamAccountName;
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
    }
}