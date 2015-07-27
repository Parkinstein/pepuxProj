using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Timers;
using System.Web.Security;
using System.Web.Services.Description;

using PepuxService.Properties;
using Newtonsoft.Json;

namespace PepuxService
{
    
     public class IpService : IPService
    {

         #region Variables

         public List<ADUsers> lstADUsers;
         public List<string> lstLOCUsers;
         public ResponseParent AllConfs_wm;
         public List<Objects> AllConfs;
         public RootObject AllPartsRoot;
         public List<Participants> PartForConf; 

         #endregion

         #region Get_AD_Users

        public List<ADUsers> GetADUsvrs(string groupname)
        {
            try
            {

                var context = new PrincipalContext(ContextType.Domain, Settings.Default.Domen, Settings.Default.DN_login, Settings.Default.Dn_pass); //+ "/DC=rad,DC=lan,DC=local"
                using (var searcher = new PrincipalSearcher())
                {
                    var sp = new GroupPrincipal(context, groupname);
                    searcher.QueryFilter = sp;
                    var group = searcher.FindOne() as GroupPrincipal;

                    if (group == null)
                        Debug.WriteLine("Invalid Group Name: {0}", groupname);

                    foreach (var f in group.GetMembers())
                    {
                        var principal = f as UserPrincipal;

                        if (principal == null || string.IsNullOrEmpty(principal.Name))
                            continue;
                        else
                        {
                            ADUsers objSurveyUsers = new ADUsers();
                            objSurveyUsers.Group = groupname;
                            objSurveyUsers.Email = principal.EmailAddress;
                            objSurveyUsers.UserName = principal.SamAccountName;
                            objSurveyUsers.DisplayName = principal.DisplayName;
                            lstADUsers.Add(objSurveyUsers);

                        }
                        Debug.WriteLine("{0}", principal.Name);
                    }
                }
            }
            catch(Exception er)
            {
                Debug.WriteLine(er.HResult);
                Debug.WriteLine(er.Message);
            }
            return lstADUsers;
        }

        #endregion

         #region Get_local_Users_&_Compare
         public List<Service> GetDataLocal()
        {
            lstADUsers = new List<ADUsers>();
            lstLOCUsers = new List<string>();
            ServiceDataContext db = new ServiceDataContext();
            try
            {
                
               {
                    string grname = Settings.Default.Admin_group;
                    GetADUsvrs(grname);
                    string grname2 = Settings.Default.User_group;
                    GetADUsvrs(grname2);

                    var NameQuery =
                    from adName in db.Service
                    select adName;
                    foreach (var customer in NameQuery)
                    {
                        if (!lstADUsers.Exists(x => x.UserName == customer.AdName))
                        lstLOCUsers.Add(customer.AdName);

                    }
                   
                    foreach (var stroke in lstLOCUsers)
                    {
                        var deleteUsers =
                        from AdName in db.Service
                        where AdName.AdName == stroke
                        select AdName;
                        db.Service.DeleteOnSubmit(deleteUsers.First());
                        db.SubmitChanges();
                    }
                    foreach (var das in lstADUsers)
                    {
                        
                        if (!NameQuery.AsEnumerable().ToList().Exists(x => x.AdName == das.UserName))
                        {
                            Service new_user = new Service();
                            new_user.AdName = das.UserName;
                            new_user.Email = das.Email;
                            new_user.UserName = das.DisplayName;
                            new_user.Role = das.Group;
                            db.Service.InsertOnSubmit(new_user);
                            db.SubmitChanges();
                            Debug.WriteLine("Был добавлен " + das.UserName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return db.Service.ToList();
        }
        #endregion

         #region GetActiveConfs
         List<Objects> IPService.GetActiveConfs()
         {
             try
             {

                 AllConfs = new List<Objects>();
                 string pepix_address = "10.129.15.128";
                 Uri statusapi = new Uri("https://" + pepix_address + "/api/admin/status/v1/conference/");

                 WebClient client = new WebClient();
                 client.Credentials = new NetworkCredential("admin", "ciscovoip");
                 client.Headers.Add("auth", "admin,ciscovoip");
                 client.Headers.Add("veryfy", "False");
                 ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                 string reply = client.DownloadString(statusapi);
                 if (reply.ToString() != null)
                 {
                     AllConfs_wm = JsonConvert.DeserializeObject<ResponseParent>(reply);
                     AllConfs = AllConfs_wm.obj;
                     foreach (var conf in AllConfs)
                     {
                         DateTime dt = DateTime.Parse(conf.start_time);
                         DateTime dt2 = dt + TimeSpan.FromHours(3);
                         string result = dt2.ToString("dd-MMM-yyyy  HH:mm:ss");
                         conf.start_time2 = result;
                         if (conf.is_locked)
                         {
                             conf.lock_path = "<img src=\"images/lock.png\")\" style=\"max-width: 28px; max-height: 28px;\" />";
                         }
                         if (!conf.is_locked)
                         {
                             conf.lock_path = "<img src=\"images/unlock.png\")\" style=\"max-width: 28px; max-height: 28px;\" />";
                         }
                     }
                 }
                 return AllConfs;
             }
             catch (Exception errException)
             {
                 Debug.WriteLine(errException.Message);
             }
             return AllConfs;
         }
         #endregion

         #region GetPartsForConf

         List<Participants> IPService.GetActiveParts(string confname)
         {
             try
             {
                 PartForConf = new List<Participants>();
                 string pepix_address = "10.129.15.128";
                 Uri statusapi = new Uri("https://" + pepix_address + "/api/admin/status/v1/participant/?conference="+confname);
                 WebClient client = new WebClient();
                 client.Credentials = new NetworkCredential("admin", "ciscovoip");
                 client.Headers.Add("auth", "admin,ciscovoip");
                 client.Headers.Add("veryfy", "False");
                 ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                 string reply = client.DownloadString(statusapi);
                 if (reply.ToString() != null)
                 {
                     Debug.WriteLine(reply);
                     AllPartsRoot = JsonConvert.DeserializeObject<RootObject>(reply);
                     PartForConf = AllPartsRoot.participants;
                     foreach (var part in PartForConf)
                     {
                         Debug.WriteLine(part.display_name);
                     }
                 }
                 return PartForConf;
              }
             catch (Exception errException)
             {
                 Debug.WriteLine(errException.Message);
             }
             return PartForConf;
         }
        #endregion

    }
}
