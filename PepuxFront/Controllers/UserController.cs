using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;

namespace PepuxFront.Controllers
{

    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ActiveConf_Read_User(string usnam, [DataSourceRequest]DataSourceRequest request)
        {
            using (var allconfs = new PServiceClient())
            {

                IQueryable<ActiveConfs> confs = allconfs.GetActiveConfs().AsQueryable();
                DatabsDataContext db = new DatabsDataContext();
                var virtvmid = db.VmrAliases.FirstOrDefault(v => v.alias == usnam);
                var virtvmr = db.AllVmrs.FirstOrDefault(v => v.Id == virtvmid.vmid);
                List<ActiveConfs> resultsearch = new List<ActiveConfs>();
                ActiveConfs reserv = confs.FirstOrDefault(v => v.name == virtvmr.name);
                resultsearch.Add(reserv);
                DataSourceResult result = resultsearch.ToDataSourceResult(request);

                return Json(result);
            }

        }
    }
}