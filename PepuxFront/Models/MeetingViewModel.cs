using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PepuxFront.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Kendo.Mvc.UI;

    public class MeetingViewModel : ISchedulerEvent
    {
        public int MeetingID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        private DateTime start;
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        private DateTime end;
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }

        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }

        public string RecurrenceRule { get; set; }
        public int? RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }
        public int? RoomID { get; set; }
        public IEnumerable<int> Attendees { get; set; }
    }

}