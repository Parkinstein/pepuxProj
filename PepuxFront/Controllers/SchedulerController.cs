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
        public static IEnumerable<MeetingViewModel> meetings_all, mettingsFiltered;

        public SchedulerController()
        {
            this.meetingService = new SchedulerMeetingService();
        }



        public JsonResult GetUsers([DataSourceRequest] DataSourceRequest request)
        {
            // applicationDbContext = new ApplicationDbContext();


            return Json(GetPB().ToDataSourceResult(request));
            //return Json(applicationDbContext.Users, JsonRequestBehavior.AllowGet);
            //return null;
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
            RegexUtilities util = new RegexUtilities();
            var idf = GetAllPB().FirstOrDefault(m => m.samaccountname == AccountController.SAMUname);
            meeting.RoomID = AccountController.UID;
            int rid = meeting.RoomID;
            var init = GetAllPB().FirstOrDefault(m => m.Id == rid);
            meeting.OpLink = string.Concat("https://", "10.129.15.129", "/webapp/?conference=", init.samaccountname, "&name=Operator&bw=512&join=1");
            //List<object> attend = meeting.Attendees.Select(att => GetAllPB().FirstOrDefault(m => m.Id == att)).Cast<object>().ToList();
            meeting.InitName = init.samaccountname;
            meeting.InitFullname = init.dispName;
            if (meeting.Start < DateTime.Today + TimeSpan.FromHours(3))
            { Debug.WriteLine("@@@"); }
            List<PhonebookDB> emaillist = new List<PhonebookDB>();
            emaillist.Add(init);
            StringBuilder strB = new StringBuilder();
            foreach (var att in meeting.Attendees)
            {
                PhonebookDB attemail = (GetAllPB().FirstOrDefault(m => m.Id == att));
                emaillist.Add(attemail);
            }
            List<string> AddAtt = new List<string>();
            if (meeting.AddAttend != null) { AddAtt = (meeting.AddAttend.Split((",").ToCharArray())).ToList(); }
            foreach (var aa in AddAtt)
            {
                strB.Append(aa + ";" + aa + Environment.NewLine);
                if (util.IsValidEmail(aa))
                {
                    PhonebookDB ar = new PhonebookDB();
                    ar.email = aa;
                    ar.dispName = aa;
                    emaillist.Add(ar);
                }
                else { }
            }

            foreach (var mail in emaillist)
            {
                

                    string link = "https://" + "10.129.15.129" + "/webapp/?conference=" + init.dispName + "&name=" +
                                  Uri.EscapeDataString(mail.dispName) + "&bw=512&join=1";
                    string body = "Уважамый(ая), " + mail.dispName + "!" + Environment.NewLine + meeting.Start +
                                  TimeSpan.FromHours(3) + " состоится конференция на тему \"" + meeting.Title + "\"." +
                                  Environment.NewLine + "Инициатор конференции: " + init.dispName + Environment.NewLine +
                                  "В указанное время, для участия в конференции, просьба перейти по ссылке: " +
                                  Environment.NewLine + link;
                    Debug.WriteLine(mail.email);
                    Sendmail(mail.email, meeting.Title, body);
                
            }

            


            //if (ModelState.IsValid)
            //{
            //    var owner = AccountController.currentuser;
            //    var filename = "meeting-" + owner.GivenName + "-" +
            //                       (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm") + ".csv";
            //    string path = Path.Combine(Server.MapPath("~/Content/OpFiles/CSV"), filename);
            //    Debug.WriteLine(path);
            //    meeting.FileLink = "Content/OpFiles/CSV/" + filename;

            //    Debug.WriteLine("Valid");
            //    using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            //    {
            //        using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            //        {
            //            streamWriter.Write(strB.ToString());
            //        }
            //    }
            //    if (meeting.Record)
            //    {
            //        Debug.WriteLine("Задача на запись создана");
            //        var tasktitle = String.Concat("rec-", owner.GivenName, "-",
            //            (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm"));
            //        var taskapp = Path.Combine(Server.MapPath("~/Content/OpFiles"), "flvstreamer.exe");
            //        var file_name = "rec-" + owner.GivenName + "-" +
            //                        (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm") + ".flv";
            //        var pathflv = Path.Combine(Server.MapPath("~/Content/OpFiles/FLV"), file_name);
            //        var stream_link = "rtmp://www.planeta-online.tv:1936/live/soyuz";
            //        meeting.Recfile = "Content/OpFiles/FLV/" + file_name;
            //        var comment = "Запись конференции " + owner.GivenName + "-" +
            //                      (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm");
            //        var acc_un = "boris_000";
            //        var acc_pass = "1Q2w3e4r!";
            //        var task_start = meeting.Start;
            //        var task_end = meeting.End;
            //        RecordTask(tasktitle, taskapp, pathflv, stream_link, comment, acc_un, acc_pass, task_start, task_end);

            //    }
            //    
            //}
            meetingService.Insert(meeting, ModelState);
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
            meetings_all = meetingService.GetAll();
            mettingsFiltered = meetings_all.AsEnumerable().Where(m => m.InitName == AccountController.SAMUname);
            foreach (var all in meetings_all)
            {
                if (!all.Recfile.IsNullOrWhiteSpace())
                {
                    ViewBag.Rec = true;
                }

            }

            if (AccountController.Ugroup == "PepuxAdmins")
            {
                return Json(meetingService.GetAll().ToDataSourceResult(request));
            }
            if (AccountController.Ugroup == "PepuxUsers")
            {
                return Json(mettingsFiltered.ToDataSourceResult(request));
            }
            return null;
        }

        public virtual JsonResult Meetings_Update([DataSourceRequest] DataSourceRequest request, MeetingViewModel meeting)
        {
            if (ModelState.IsValid)
            {
                meetingService.Update(meeting, ModelState);
                RegexUtilities util = new RegexUtilities();
                var idf = GetAllPB().FirstOrDefault(m => m.samaccountname == AccountController.SAMUname);
                meeting.RoomID = AccountController.UID;
                int rid = meeting.RoomID;
                var init = GetAllPB().FirstOrDefault(m => m.Id == rid);
                meeting.OpLink = string.Concat("https://", "10.129.15.129", "/webapp/?conference=", init.samaccountname, "&name=Operator&bw=512&join=1");
                //var attend = meeting.Attendees.Select(att => GetAllPB().FirstOrDefault(m => m.Id == att)).Cast<object>().ToList();
                StringBuilder strB = new StringBuilder();
                List<string> AddAtt = new List<string>();
                if (meeting.AddAttend != null) { AddAtt = (meeting.AddAttend.Split((",").ToCharArray())).ToList(); }
                //foreach (int at in attend)
                //{
                //    string em = at.
                    //var init = AccountController.currentuser.DisplayName;
                    //var name = AccountController.currentuser;
                    ////spisok = string.Concat(name.PhoneNumber, ";", name.UserName);
                    //strB.Append(name.VoiceTelephoneNumber + ";" + name.DisplayName + Environment.NewLine);
                    //if (name.EmailAddress != null)
                    //{
                    //    string link = "https://" + "10.129.15.129" + "/webapp/?conference=" + init.UserConfID + "&name=" +
                    //                  Uri.EscapeDataString(name.UserName) + "&bw=512&join=1";
                    //    string body = "Уважамый(ая), " + name.UserName + "!" + Environment.NewLine +
                    //                  " конференция на тему \"" + meeting.Title + "\" переносится на " + meeting.Start +
                    //                  TimeSpan.FromHours(3) + Environment.NewLine + "Инициатор конференции: " +
                    //                  init.UserName + Environment.NewLine +
                    //                  "В указанное время, для участия в конференции, просьба перейти по ссылке: " +
                    //                  Environment.NewLine + link;
                    //    //Sendmail(name.Email, meeting.Title, body);
                    //}
                //}
                //foreach (var aa in AddAtt)
                //{
                //    strB.Append(aa + ";" + aa + Environment.NewLine);
                //}
                ////var owner = applicationDbContext.Users.FirstOrDefault(p => p.UserConfID == roomID);
                //var filename = "meeting-" + AccountController.Uname + "-" +
                //                   (meeting.Start + TimeSpan.FromHours(3)).ToString("dd-MM-yyyy_hh-mm") + ".csv";
                //string path = Path.Combine(Server.MapPath("~/Content/OpFiles/CSV"), filename);
                //Debug.WriteLine(path);
                //meeting.FileLink = "Content/OpFiles/CSV/" + filename;
                //meetingService.Update(meeting, ModelState);

                //using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
                //{
                //    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                //    {
                //        streamWriter.Write(strB.ToString());
                //    }
                //}
            }
            return Json(new[] { meeting }.ToDataSourceResult(request, ModelState));
        }

        public Task<ActionResult> Sendmail(string to, string subj, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",465)
            {
                Credentials = new NetworkCredential("bparkin", "lfybbk"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
               
            };
            MailMessage mailMessage = new MailMessage()
            {
                Priority = MailPriority.High,
                From = new MailAddress("bparkin@gmail.com", "Планировщик системы видео-конференц-связи 'Сова'")
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
        private IEnumerable<IpServiceLink.PhonebookDB> GetAllPB()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetPB();
            return data;
        }
    }
}