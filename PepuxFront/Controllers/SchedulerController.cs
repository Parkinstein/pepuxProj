using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using TaskScheduler;
using DaysOfTheWeek = TaskScheduler.DaysOfTheWeek;
using Task = TaskScheduler.Task;
using WeeklyTrigger = TaskScheduler.WeeklyTrigger;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.BuilderProperties;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;
//using Telerik.Charting;

namespace PepuxFront.Controllers
{
    public class SchedulerController : Controller
    {
        private SchedulerMeetingService meetingService;

        public string spisok;

        public static string oplink;
        public static IEnumerable<MeetingViewModel> meetings_all;

        public SchedulerController()
        {
            this.meetingService = new SchedulerMeetingService();
        }



        public JsonResult GetUsers([DataSourceRequest] DataSourceRequest request)
        {
            // applicationDbContext = new ApplicationDbContext();


            return Json(GetPB().ToDataSourceResult(request));
            //return Json(applicationDbContext.Users, JsonRequestBehavior.AllowGet);
            return null;
        }

        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<IpServiceLink.addrec> filteredresult = GetPB();
            ViewBag.Users = filteredresult;
            return View();
        }
        
        public virtual JsonResult Meetings_Create([DataSourceRequest] DataSourceRequest request, MeetingViewModel meeting)
        {

            var idf = GetPB().FirstOrDefault(m => m.samaccountname == AccountController.SAMUname);
            meeting.RoomID = idf.id;
            int roomID = (int)meeting.RoomID;
            List<int> nums = new List<int>();
            nums.Add(roomID);
            nums.AddRange(meeting.Attendees);
            oplink = string.Concat("https://", "10.129.15.129", "/webapp/?conference=", roomID, "&name=Operator&bw=512&join=1");
            meeting.OpLink = oplink;
            if (meeting.Start < DateTime.Today + TimeSpan.FromHours(3))
            { Debug.WriteLine("@@@"); }
            StringBuilder strB = new StringBuilder();
            List<string> AddAtt = new List<string>();
            if (meeting.AddAttend != null) { AddAtt = (meeting.AddAttend.Split((",").ToCharArray())).ToList(); }

            //foreach (int num in nums)
            //{
            //    var init = applicationDbContext.Users.FirstOrDefault(p => p.UserConfID == roomID);
            //    var name = applicationDbContext.Users.FirstOrDefault(p => p.UserConfID == num);
            //    strB.Append(name.PhoneNumber + ";" + name.UserName + Environment.NewLine);
            //    if (name.Email != null)
            //    {
            //        string link = "https://" + "10.129.15.129" + "/webapp/?conference=" + init.UserConfID + "&name=" +
            //                      Uri.EscapeDataString(name.UserName) + "&bw=512&join=1";
            //        string body = "Уважамый(ая), " + name.UserName + "!" + Environment.NewLine + meeting.Start +
            //                      TimeSpan.FromHours(3) + " состоится конференция на тему \"" + meeting.Title + "\"." +
            //                      Environment.NewLine + "Инициатор конференции: " + init.UserName + Environment.NewLine +
            //                      "В указанное время, для участия в конференции, просьба перейти по ссылке: " +
            //                      Environment.NewLine + link;
            //        //Sendmail(name.Email, meeting.Title, body);
            //    }
            //}

            foreach (var aa in AddAtt)
            {
                strB.Append(aa + ";" + aa + Environment.NewLine);
            }


            if (ModelState.IsValid)
            {
                var owner = AccountController.currentuser;
                var filename = "meeting-" + owner.GivenName + "-" +
                                   (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm") + ".csv";
                string path = Path.Combine(Server.MapPath("~/Content/OpFiles/CSV"), filename);
                Debug.WriteLine(path);
                meeting.FileLink = "Content/OpFiles/CSV/" + filename;

                Debug.WriteLine("Valid");
                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.Write(strB.ToString());
                    }
                }
                if (meeting.Record)
                {
                    Debug.WriteLine("Задача на запись создана");
                    var tasktitle = String.Concat("rec-", owner.GivenName, "-",
                        (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm"));
                    var taskapp = Path.Combine(Server.MapPath("~/Content/OpFiles"), "flvstreamer.exe");
                    var file_name = "rec-" + owner.GivenName + "-" +
                                    (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm") + ".flv";
                    var pathflv = Path.Combine(Server.MapPath("~/Content/OpFiles/FLV"), file_name);
                    var stream_link = "rtmp://www.planeta-online.tv:1936/live/soyuz";
                    meeting.Recfile = "Content/OpFiles/FLV/" + file_name;
                    var comment = "Запись конференции " + owner.GivenName + "-" +
                                  (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm");
                    var acc_un = "boris_000";
                    var acc_pass = "1Q2w3e4r!";
                    var task_start = meeting.Start;
                    var task_end = meeting.End;
                    RecordTask(tasktitle, taskapp, pathflv, stream_link, comment, acc_un, acc_pass, task_start, task_end);

                }
                meetingService.Insert(meeting, ModelState);
            }

            return Json(new[] { meeting }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Meetings_Destroy([DataSourceRequest] DataSourceRequest request, MeetingViewModel meeting)
        {
            if (ModelState.IsValid)
            {
                meetingService.Delete(meeting, ModelState);
            }
            return Json(new[] { meeting }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Meetings_Read([DataSourceRequest] DataSourceRequest request)
        {
            int i = 0;
            meetings_all = meetingService.GetAll();
            foreach (var all in meetings_all)
            {
                if (!all.Recfile.IsNullOrWhiteSpace())
                {
                    ViewBag.Rec = true;
                }

            }


            return Json(meetingService.GetAll().ToDataSourceResult(request));
        }

        public virtual JsonResult Meetings_Update([DataSourceRequest] DataSourceRequest request, MeetingViewModel meeting)
        {
            //if (ModelState.IsValid)
            //{
            //    meetingService.Update(meeting, ModelState);
            //    ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            //    int roomID = (int)meeting.RoomID;
            //    List<int> nums = new List<int>();
            //    nums.Add(meeting.RoomID);
            //    nums.AddRange(meeting.Attendees);
            //    oplink = string.Concat("https://", "10.129.15.129", "/webapp/?conference=", roomID, "&name=Operator&bw=512&join=1");
            //    meeting.OpLink = oplink;
            //    StringBuilder strB = new StringBuilder();
            //    List<string> AddAtt = new List<string>();
            //    if (meeting.AddAttend != null) { AddAtt = (meeting.AddAttend.Split((",").ToCharArray())).ToList(); }
            //    foreach (int num in nums)
            //    {
            //        var init = AccountController.currentuser.GivenName;
            //        var name = applicationDbContext.Users.FirstOrDefault(p => p.UserConfID == num);
            //        //spisok = string.Concat(name.PhoneNumber, ";", name.UserName);
            //        strB.Append(name.PhoneNumber + ";" + name.UserName + Environment.NewLine);
            //        if (name.Email != null)
            //        {
            //            string link = "https://" + "10.129.15.129" + "/webapp/?conference=" + init.UserConfID + "&name=" +
            //                          Uri.EscapeDataString(name.UserName) + "&bw=512&join=1";
            //            string body = "Уважамый(ая), " + name.UserName + "!" + Environment.NewLine +
            //                          " конференция на тему \"" + meeting.Title + "\" переносится на " + meeting.Start +
            //                          TimeSpan.FromHours(3) + Environment.NewLine + "Инициатор конференции: " +
            //                          init.UserName + Environment.NewLine +
            //                          "В указанное время, для участия в конференции, просьба перейти по ссылке: " +
            //                          Environment.NewLine + link;
            //            //Sendmail(name.Email, meeting.Title, body);
            //        }
            //    }
            //    foreach (var aa in AddAtt)
            //    {
            //        strB.Append(aa + ";" + aa + Environment.NewLine);
            //    }
            //    var owner = applicationDbContext.Users.FirstOrDefault(p => p.UserConfID == roomID);
            //    var filename = "meeting-" + AccountController.Uname + "-" +
            //                       (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm") + ".csv";
            //    string path = Path.Combine(Server.MapPath("~/Content/OpFiles/CSV"), filename);
            //    Debug.WriteLine(path);
            //    meeting.FileLink = "Content/OpFiles/CSV/" + filename;
            //    meetingService.Update(meeting, ModelState);

            //    using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            //    {
            //        using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            //        {
            //            streamWriter.Write(strB.ToString());
            //        }
            //    }
            //}
            return Json(new[] { meeting }.ToDataSourceResult(request, ModelState));
        }

        public Task<ActionResult> Sendmail(string to, string subj, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Credentials = new NetworkCredential("borisparkin", "1Q2w3e4r!"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            MailMessage mailMessage = new MailMessage()
            {
                Priority = MailPriority.High,
                From = new MailAddress("Планировщик системы видео-конференц-связи 'Рерих'", "Планировщик системы видео-конференц-связи 'Рерих'")
            };
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = subj;
            mailMessage.Body = body;
            smtpClient.Send(mailMessage);
            return null;
        }

        public Task<ActionResult> RecordTask(string tasktitle, string taskapp, string stream_link, string pathflv, string comment, string acc_un, string accpass, DateTime task_start, DateTime task_end)
        {
            ScheduledTasks st = new ScheduledTasks();
            Task t;
            t = st.CreateTask(tasktitle);
            t.ApplicationName = taskapp;
            t.Parameters = " -r " + "-q" + pathflv + " -o " + "\"" + stream_link + "\"";
            t.Comment = comment;
            t.SetAccountInformation(acc_un, accpass);
            t.IdleWaitMinutes = 10;
            TimeSpan worktime = task_end - task_start;
            Debug.WriteLine(worktime);
            t.MaxRunTime = new TimeSpan(worktime.Ticks);

            t.Priority = System.Diagnostics.ProcessPriorityClass.Idle;
            DateTime starttask = new DateTime();
            starttask = task_start + TimeSpan.FromHours(3);
            t.Triggers.Add(new RunOnceTrigger(starttask));
            t.Save();
            t.Close();
            st.Dispose();
            return null;
        }
        private IEnumerable<IpServiceLink.addrec> GetPB()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetPhBOw(AccountController.SAMUname);
            return data;
        }
    }
}