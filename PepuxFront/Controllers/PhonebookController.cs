using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PepuxFront.Controllers
{
    public class PhonebookController : Controller
    {
        // GET: Phonebook
        public ActionResult Phonebook()
        {
            return View();
        }
    }
}