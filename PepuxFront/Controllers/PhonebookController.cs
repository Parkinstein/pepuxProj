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

namespace PepuxFront.Controllers
{
    public class PhonebookController : Controller
    {
        //public ActionResult Phonebook_Ajax(Phonebook.DTResult param, string Uname)
        //{
        //    IEnumerable<Phonebook> filteredresult;

        //    if (!string.IsNullOrEmpty(param.Search.Value))
        //    {
        //        filteredresult = GetData(Uname).Where(c => (c.UserName.Contains(param.Search.Value) || c.AdName.Contains(param.Search.Value)));
        //    }
        //    else
        //    {
        //        filteredresult = GetData(Uname);
        //    }

        //    return Json(new
        //    {
        //        recordsTotal = GetData(Uname).Count(),
        //        recordsFiltered = filteredresult.Count(),
        //        data = filteredresult,
        //    }, JsonRequestBehavior.AllowGet);
        //}
        
        private ActionResult Phonebook_Ajax([DataSourceRequest]DataSourceRequest request)
        {
            using (var allrecs = new PServiceClient())
            {

                IQueryable<addrec> recs = allrecs.GetPhBOw(AccountController.SAMUname,null).AsQueryable();

                DataSourceResult result = recs.ToDataSourceResult(request);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Phonebook
        public ActionResult Phonebook()
        {
            
            return View();
        }

        public ActionResult Delete(int id)
        {
            Debug.WriteLine(id);
            return null;
        }
    }
}