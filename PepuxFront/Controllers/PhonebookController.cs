using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Timers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;
using Timer = System.Timers.Timer;

namespace PepuxFront.Controllers
{
    public class PhonebookController : Controller
    {
        public ActionResult Phonebook_Ajax(Phonebook.DTResult param, string Uname)
        {
            IEnumerable<Phonebook> filteredresult;

            if (!string.IsNullOrEmpty(param.Search.Value))
            {
                filteredresult = GetData(Uname).Where(c => (c.UserName.Contains(param.Search.Value) || c.AdName.Contains(param.Search.Value)));
            }
            else
            {
                filteredresult = GetData(Uname);
            }

            return Json(new
            {
                recordsTotal = GetData(Uname).Count(),
                recordsFiltered = filteredresult.Count(),
                data = filteredresult,
            }, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<Phonebook> GetData(string Uname)
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetPhonebookUsers(Uname);
            return data;
        }
        // GET: Phonebook
        public ActionResult Phonebook()
        {
            return View();
        }
    }
}