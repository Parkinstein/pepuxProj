using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.Services.Description;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;
using RestSharp;

namespace PepuxFront.Controllers
{


    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();

            return View(obj.GetActiveConfs());
        }
        public ActionResult ActiveConf_Read([DataSourceRequest]DataSourceRequest request)
        {
            using (var allconfs = new  PServiceClient())
            {
                
                IQueryable<ActiveConfs> confs = allconfs.GetActiveConfs().AsQueryable();
                
                DataSourceResult result = confs.ToDataSourceResult(request);

                return Json(result);
            }

        }
        
        public ActionResult UserGet()
        {
            IpServiceLink.PServiceClient obj1 = new PServiceClient();
            ViewData["AllUsers"] = obj1.GetDataLocal();
            return View(obj1.GetDataLocal());
        }
        public ActionResult GetParticipants(string confname)
        {
            PServiceClient obj2 = new PServiceClient();
            return PartialView(obj2.GetActiveParts(confname));
        }
        private IEnumerable<ActiveConfs> GetData()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetActiveConfs();
 
            return data;
        }
        public PartialViewResult GetACData()
        {
            return PartialView(GetData());
        }

        private IEnumerable<participants> getparti(string confname)
        {
            PServiceClient req = new PServiceClient();
            var parts = req.GetActiveParts(confname);
            return parts;
        }

        public ActionResult GetParts(string confnam)
        {
            return View(getparti(confnam));
        }

        public ActionResult LockConf(string conid)
        {
            Debug.WriteLine(conid);
            
            string pepix_address = "10.129.15.128";
            Uri statusapi = new Uri("https://" + pepix_address + "/api/admin/command/v1/conference/lock/");
                 WebClient client = new WebClient();
                 ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                 client.Credentials = new NetworkCredential("admin", "ciscovoip");
                 client.Headers.Add("auth", "admin,ciscovoip");
                 client.Headers.Add("veryfy", "False");
                 client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                 client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                 NameValueCollection string_lock = new NameValueCollection();
                 string_lock.Add("conference_id", conid);
                 var json = new JavaScriptSerializer().Serialize(string_lock);
                 client.UploadValues(statusapi,"POST",string_lock);
            return null;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingPopup_Update([DataSourceRequest] DataSourceRequest request)
        {
            

            return null;
        }




    }
}