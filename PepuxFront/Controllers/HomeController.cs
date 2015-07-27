using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PepuxFront.IpServiceLink;

namespace PepuxFront.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (AccountController.IsAuth && AccountController.Ugroup == "PepuxAdmins")
            {
                this.RedirectToAction("Index", "Admin");
            }
            else if (AccountController.IsAuth && AccountController.Ugroup == "PepuxUsers")
            {
                this.RedirectToAction("Index", "User");
            }
            return this.RedirectToAction("Login", "Account");
        }
        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}