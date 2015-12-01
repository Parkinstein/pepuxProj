using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace PepuxFront.Models
{
    public sealed class CurrentUser : MembershipUser
    {
        public string sammaccountname { get; set; }
        public string publicname { get; set; }
        public string usergroup { get; set; }
        public int uid { get; set; }
        public bool isAuth { get; set; }
    }
}
