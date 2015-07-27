using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepuxFront.Models
{
    public class ActiveConference
    {
        public class Meta
        {
            public int limit { get; set; }
            public object next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total_count { get; set; }
        }

        public class Object
        {
            public string id { get; set; }
            public bool is_locked { get; set; }
            public string name { get; set; }
            public string resource_uri { get; set; }
            public string service_type { get; set; }
            public string start_time { get; set; }
            public string tag { get; set; }
        }

        public class RootObject
        {
            public Meta meta { get; set; }
            public List<Object> objects { get; set; }


        }
    }
}
