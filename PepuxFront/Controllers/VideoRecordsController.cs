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
        }
        // Delete Records method
        public void VideoRecords_Delete(object[] pbrArray)
        {
            foreach (int pbr in pbrArray)
            {
                DeleteFromRecords(pbr);
            }
        }

        // Delete Phonebook records method
        public void DeleteFromRecords(int ids)
        {
            IpServiceLink.PServiceClient act = new PServiceClient();
            act.DeleteRecordsFromDb(ids);
            //ViewBag.DeletedRecs = String.Format("Удалено {0} записей", i);
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