using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepuxFront.Models
{
    public class Phonebook
    {
        public class Object
        {
            public int id { get; set; }
            public string name { get; set; }
            public string surname { get; set; }
            public string position { get; set; }
            public string tel_int { get; set; }
            public string tel_ext { get; set; }
            public string tel_mob { get; set; }
            public string timezone { get; set; }
            public string sip_add { get; set; }
            public string h323_add { get; set; }
            public string group { get; set; }
            public string email { get; set; }
        }
        public class DTResult
        {
            public string recordsTotal { get; set; }
            public string recordsFiltered { get; set; }
            public DTSearch Search { get; set; }
        }
        public class DTSearch
        {
            public string Value { get; set; }
            public bool Regex { get; set; }
        }
    }
}