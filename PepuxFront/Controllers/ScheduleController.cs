using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PepuxFront.Models;
using Newtonsoft.Json;

namespace PepuxFront.Controllers
{
    public class ScheduleController : Controller
    {
        private SchedulerMeetingService meetingService;


        public ScheduleController()
        {
            this.meetingService = new SchedulerMeetingService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(meetingService.GetAll().ToDataSourceResult(request));
        }

        public virtual JsonResult Destroy([DataSourceRequest] DataSourceRequest request, MeetingViewModel task)
        {
            if (ModelState.IsValid)
            {
                meetingService.Delete(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        //public virtual JsonResult Create([DataSourceRequest] DataSourceRequest request, MeetingViewModel task)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        meetingService.Insert(task, ModelState);
        //    }

        //    return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        //}

        public virtual JsonResult Update([DataSourceRequest] DataSourceRequest request, MeetingViewModel task)
        {
            //example custom validation:
            if (task.Start.Hour < 8 || task.Start.Hour > 22)
            {
                ModelState.AddModelError("start", "Start date must be in working hours (8h - 22h)");
            }

            if (ModelState.IsValid)
            {
                meetingService.Update(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            meetingService.Dispose();
            meetingService.Dispose();
            base.Dispose(disposing);
        }
    }
}
