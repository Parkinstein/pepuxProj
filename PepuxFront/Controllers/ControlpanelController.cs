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
            Timer renew = new Timer();
            renew.Interval = 3000;
            renew.Enabled = true;
            renew.AutoReset = true;
            renew.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            return View();
        }
        private  void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            GetData();
        }
        public ActionResult ActiveConf_Read([DataSourceRequest]DataSourceRequest request)
        {
            using (var allconfs = new PServiceClient())
            {

                IQueryable<ActiveConfs> confs = allconfs.GetActiveConfs().AsQueryable();

                DataSourceResult result = confs.ToDataSourceResult(request);

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        private IEnumerable<ActiveConfs> GetData()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetActiveConfs();

            return data;
        }
    }
}