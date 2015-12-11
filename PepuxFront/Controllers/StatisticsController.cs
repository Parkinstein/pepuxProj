using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Timers;
using System.Web.Services.Description;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;

namespace PepuxFront.Controllers
{
    public class StatisticsController : Controller
    {
        public VMRstats_response historyVMR_full;
        public List<VMRstats> historyVMR_data;


        public ActionResult Statistics()
        {
            return View();
        }

        public ActionResult GetHistoryVMR()
        {
            IEnumerable<VMRstats> result = VMRstats();
            List<VMRstats> list = new List<VMRstats>();
            return Json(new
            {
                data = result
            }, JsonRequestBehavior.AllowGet);
        }


        private List<VMRstats> VMRstats()
        {
            try
            {
                historyVMR_data = new List<VMRstats>();
                string coba_address = "10.129.15.128";
                Uri historyapi = new Uri("https://" + coba_address + "/api/admin/history/v1/conference/?limit=1000");
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential("admin", "ciscovoip");
                client.Headers.Add("auth", "admin,ciscovoip");
                client.Headers.Add("veryfy", "False");
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                string reply = client.DownloadString(historyapi);
                string reply1 = Win1251ToUTF8(reply);
                if (reply.ToString() != null)
                {
                    historyVMR_full = JsonConvert.DeserializeObject<VMRstats_response>(reply1);
                    Debug.WriteLine(historyVMR_full);
                    historyVMR_data = historyVMR_full.obj;
                    Debug.WriteLine(historyVMR_data);
                    foreach (var historyRecords in historyVMR_data)
                    {
                        historyRecords.start_time2 =
                            (DateTime.Parse(historyRecords.start_time) + TimeSpan.FromHours(3)).ToString("dd-MMM-yyyy  HH:mm:ss");
                        historyRecords.end_time2 =
                            (DateTime.Parse(historyRecords.end_time) + TimeSpan.FromHours(3)).ToString("dd-MMM-yyyy  HH:mm:ss");
                    }
                }
                return historyVMR_data;
            }
            catch (Exception errException)
            {
                Debug.WriteLine(errException.Message);
            }
            return historyVMR_data;
        }

        private string Win1251ToUTF8(string source)
        {
            Encoding utf8 = Encoding.GetEncoding("windows-1251");
            Encoding win1251 = Encoding.GetEncoding("utf-8");
            byte[] utf8Bytes = win1251.GetBytes(source);
            byte[] win1251Bytes = Encoding.Convert(win1251, utf8, utf8Bytes);
            source = win1251.GetString(win1251Bytes);
            return source;
        }
    }
}