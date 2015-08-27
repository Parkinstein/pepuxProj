using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

namespace PepuxFront.Models
{
    public  class SchedulerMeetingService : ISchedulerEventService<MeetingViewModel>
    {
        private DatabsDataContext db;

        public SchedulerMeetingService(DatabsDataContext context)
        {
            db = context;
        }

        public SchedulerMeetingService()
            : this(new DatabsDataContext())
        {
        }

        public IQueryable<MeetingViewModel> GetAll()
        {
            return db.Meetings.ToList().Select(meeting => new MeetingViewModel
            {
                MeetingID = meeting.MeetingID,
                Title = meeting.Title,
                Start = DateTime.SpecifyKind(meeting.Start, DateTimeKind.Utc),
                End = DateTime.SpecifyKind(meeting.End, DateTimeKind.Utc),
                StartTimezone = meeting.StartTimezone,
                EndTimezone = meeting.EndTimezone,
                Description = meeting.Description,
                IsAllDay = meeting.IsAllDay,
                RoomID = meeting.RoomID,
                RecurrenceRule = meeting.RecurrenceRule,
                RecurrenceException = meeting.RecurrenceException,
                RecurrenceID = meeting.RecurrenceID,
                Attendees = meeting.MeetingAttendees.Select(m => m.AttendeeID).ToArray()
            }).AsQueryable();
        }

        public void Insert(MeetingViewModel appointment, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }


        //public virtual void Insert(MeetingViewModel meeting, ModelStateDictionary modelState)
        //{
        //    if (ValidateModel(meeting, modelState))
        //    {
        //        if (meeting.Attendees == null)
        //        {
        //            meeting.Attendees = new int[0];
        //        }

        //        if (string.IsNullOrEmpty(meeting.Title))
        //        {
        //            meeting.Title = "";
        //        }

        //        var entity = meeting.ToEntity();

        //        foreach (var attendeeId in meeting.Attendees)
        //        {
        //            entity.MeetingAttendees.Add(new MeetingAttendee
        //            {
        //                AttendeeID = attendeeId
        //            });
        //        }

        //        db.Meetings.Attach(entity); //  .Add(entity);
        //        db.SubmitChanges();//SaveChanges();

        //        meeting.MeetingID = entity.MeetingID;
        //    }
        //}

        public void Update(MeetingViewModel meeting, ModelStateDictionary modelState)
        {
            if (ValidateModel(meeting, modelState))
            {
                if (string.IsNullOrEmpty(meeting.Title))
                {
                    meeting.Title = "";
                }

                var entity = db.Meetings.Include(model => model.MeetingAttendees).FirstOrDefault(m => m.MeetingID == meeting.MeetingID);

                entity.Title = meeting.Title;
                entity.Start = meeting.Start;
                entity.End = meeting.End;
                entity.Description = meeting.Description;
                entity.IsAllDay = meeting.IsAllDay;
                entity.RoomID = meeting.RoomID;
                entity.RecurrenceID = meeting.RecurrenceID;
                entity.RecurrenceRule = meeting.RecurrenceRule;
                entity.RecurrenceException = meeting.RecurrenceException;
                entity.StartTimezone = meeting.StartTimezone;
                entity.EndTimezone = meeting.EndTimezone;

                foreach (var meetingAttendee in entity.MeetingAttendees.ToList())
                {
                    entity.MeetingAttendees.Remove(meetingAttendee);
                }

                if (meeting.Attendees != null)
                {
                    foreach (var attendeeId in meeting.Attendees)
                    {
                        var meetingAttendee = new MeetingAttendee
                        {
                            MeetingID = entity.MeetingID,
                            AttendeeID = attendeeId
                        };

                        entity.MeetingAttendees.Add(meetingAttendee);
                    }
                }

                db.SubmitChanges();//   .SaveChanges();
            }
        }

        public void Delete(MeetingViewModel meeting, ModelStateDictionary modelState)
        {
            if (meeting.Attendees == null)
            {
                meeting.Attendees = new int[0];
            }

            var entity = meeting.ToEntity();

            db.Meetings.Attach(entity);

            var attendees = meeting.Attendees.Select(attendee => new MeetingAttendee
            {
                AttendeeID = attendee,
                MeetingID = entity.MeetingID
            });

            foreach (var attendee in attendees)
            {
                db.MeetingAttendees.Attach(attendee);
            }

            entity.MeetingAttendees.Clear();

            var recurrenceExceptions = db.Meetings.Where(m => m.RecurrenceID == entity.MeetingID);

            foreach (var recurrenceException in recurrenceExceptions)
            {
                db.Meetings.DeleteOnSubmit(recurrenceException);//.Remove(recurrenceException);
            }

            db.Meetings.DeleteOnSubmit(entity);//.Remove(entity);
            db.SubmitChanges();
        }

        private bool ValidateModel(MeetingViewModel appointment, ModelStateDictionary modelState)
        {
            if (appointment.Start > appointment.End)
            {
                modelState.AddModelError("errors", "End date must be greater or equal to Start date.");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}