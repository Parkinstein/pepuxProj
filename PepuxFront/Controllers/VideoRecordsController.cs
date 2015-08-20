using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;
using Renci.SshNet;

namespace PepuxFront.Controllers
{
    public class VideoRecordsController : Controller
    {
        // GET: Records
        public ActionResult VideoRecords()
        {
            return View();
        }
        public ActionResult All_VideoRec(string val,[DataSourceRequest]DataSourceRequest request)
        {
            using (var allvrecs = new IpServiceLink.PServiceClient())
            {

                IQueryable<IpServiceLink.allrecords> vrecs = allvrecs.Videorecords(val).AsQueryable();

                DataSourceResult result = vrecs.ToDataSourceResult(request);

                return Json(result);
            }

        }
        public ActionResult VideoRecDelete(string filepath)
        {
            using (var client = new SshClient("10.157.5.87", "username", "password"))
            {
                client.Connect();
                client.RunCommand("etc/init.d/networking restart");
                client.Disconnect();
            }
            return null;
        }
    }
}