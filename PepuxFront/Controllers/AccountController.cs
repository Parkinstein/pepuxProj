using System.Diagnostics;
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
        public static string Ugroup;
        public static bool IsAuth;
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

            if (Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                DatabsDataContext db = new DatabsDataContext();
                var uname = db.Service.FirstOrDefault(m => m.AdName == model.UserName);
                if (uname != null)
                    Uname = (uname.UserName);
                var ugroup = db.Service.FirstOrDefault(m => m.AdName == model.UserName);
                if (ugroup != null)
                    Ugroup = (ugroup.Role);
                IsAuth = true;
                if (Ugroup == "PepuxAdmins")
                {
                    Debug.WriteLine("Вход выполнен " + model.UserName);
                    return this.RedirectToAction("Index", "Controlpanel");

                }
                else if (Ugroup == "PepuxUsers")
                {
                    Debug.WriteLine("Вход выполнен " + model.UserName);
                    return this.RedirectToAction("Index", "User");
                }
                return this.RedirectToAction("Login", "Account");
            }

            this.ModelState.AddModelError(string.Empty, "Имя пользователя или пароль указаны неверно.");
            IsAuth = false;
            Uname = "test"; //null;
            return this.View(model);
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