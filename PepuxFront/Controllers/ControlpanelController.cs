using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PepuxFront.Models;

namespace PepuxFront.Controllers
{
    public class ControlpanelController : Controller
    {      
        // GET: Chat
        public ActionResult Control(string confname, string dispname)
        {
            ViewData["confnm"] = confname;
            ViewData["dispnm"] = dispname;
            DatabsDataContext db = new DatabsDataContext();
            var virtvmid = db.VmrAliases.FirstOrDefault(v => v.alias == confname);
            var virtvmr = db.AllVmrs.FirstOrDefault(v => v.Id == virtvmid.vmid);
            string pinc = virtvmr.pin;
            ViewData["pinc"] = pinc;
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}