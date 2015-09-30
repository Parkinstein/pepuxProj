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

        public ActionResult VideoRecords_Ajax(VideoRecords.DTResult param)
        {
            IEnumerable<IpServiceLink.allrecords> filteredresult = GetRec();
            List<IpServiceLink.allrecords> list = new List<allrecords>();
            if (!String.IsNullOrEmpty(param.Search.Value))
            {
                filteredresult = GetRec().Where(c => (c.Conf.Contains(param.Search.Value) || c.PName.Contains(param.Search.Value)));
            }

                return Json(new
            {
                recordsTotal = GetRec().Count(),
                recordsFiltered = filteredresult.Count(),
                data = filteredresult,
            }, JsonRequestBehavior.AllowGet);
        }


        public IEnumerable<IpServiceLink.allrecords> GetRec()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();

            if (AccountController.Ugroup != "PepuxAdmins")
            {
                return obj.Videorecords(AccountController.SAMUname);
            }
            else
            {
               return obj.Videorecords("");
            }
            //using (var allvrecs = new IpServiceLink.PServiceClient())
            //{
            //    IQueryable<IpServiceLink.allrecords> vrecs = allvrecs.Videorecords(val).AsQueryable();
            //    DataSourceResult result = vrecs.ToDataSourceResult(request); 
            //}

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