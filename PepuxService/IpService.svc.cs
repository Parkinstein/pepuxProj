using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using System.Web.Script.Serialization;
using MySql.Data.MySqlClient;
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
         public List<ActiveConfs> AllConfs;
         public RootObject AllPartsRoot;
         public List<Participants> PartForConf;
         public List<AllVmrs> All_Vmrs;
         public VmrParent All_VM_obj;
        public RootToken root_token;
         public ResultTok resultreq;

        private string Win1251ToUTF8(string source)
        {

            Encoding utf8 = Encoding.GetEncoding("windows-1251");
            Encoding win1251 = Encoding.GetEncoding("utf-8");

            byte[] utf8Bytes = win1251.GetBytes(source);
            byte[] win1251Bytes = Encoding.Convert(win1251, utf8, utf8Bytes);
            source = win1251.GetString(win1251Bytes);
            return source;

        }

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
         List<ActiveConfs> IPService.GetActiveConfs()
         {
             try
             {

                 AllConfs = new List<ActiveConfs>();
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
                             conf.lock_path = "<img src=\"../images/lock.png\")\" style=\"max-width: 28px; max-height: 28px;\" />";
                         }
                         if (!conf.is_locked)
                         {
                             conf.lock_path = "<img src=\"../images/unlock.png\")\" style=\"max-width: 28px; max-height: 28px;\" />";
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
                     PartForConf = AllPartsRoot.participants.FindAll(p => p.has_media);
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

         #region Get All VMRS
        public List<AllVmrs> GetVmrList()
        {
            All_Vmrs = new List<AllVmrs>();
            ServiceDataContext db = new ServiceDataContext();
            string pepix_address = "10.129.15.128";
            Uri confapi = new Uri("https://" + pepix_address + "/api/admin/configuration/v1/conference/");
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("admin", "ciscovoip");
            client.Headers.Add("auth", "admin,ciscovoip");
            client.Headers.Add("veryfy", "False");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            string reply = client.DownloadString(confapi);
            string reply1 = Win1251ToUTF8(reply);
            if (reply.ToString() != null)
            {
                All_VM_obj = JsonConvert.DeserializeObject<VmrParent>(reply1);
                All_Vmrs = All_VM_obj.obj;
                foreach (var vm in All_Vmrs)
                {
                    AllVmr confroom = new AllVmr();
                    confroom.Id = vm.id;
                    confroom.allow_guests = vm.allow_guests;
                    confroom.description = vm.description;
                    confroom.force_presenter_into_main = vm.force_presenter_into_main;
                    confroom.guest_pin = vm.guest_pin;
                    confroom.guest_view = vm.guest_view;
                    confroom.host_view = vm.host_view;
                    confroom.ivr_theme_ = vm.ivr_theme;
                    confroom.max_callrate_in_ = vm.max_callrate_in;
                    confroom.max_callrate_out_ = vm.max_callrate_out;
                    confroom.name = vm.name;
                    confroom.participant_limit = vm.participant_limit;
                    confroom.pin = vm.pin;
                    confroom.resource_uri = vm.resource_uri;
                    confroom.service_type = vm.service_type;
                    confroom.tag = vm.tag;
                    db.AllVmrs.InsertOnSubmit(confroom);
                    foreach (var ali in vm.aliases)
                    {
                        VmrAliase alias = new VmrAliase();
                        alias.Id = ali.id;
                        alias.alias = ali.alias;
                        alias.description = ali.description;
                        alias.conference = ali.conference;
                        alias.vmid = confroom.Id;
                        db.VmrAliases.InsertOnSubmit(alias);
                        db.SubmitChanges();
                    }
                    
                    db.SubmitChanges();

                }
                
                return All_Vmrs;
            }
            return All_Vmrs;
        }
        #endregion

         #region Get token
        public string GetToken(string confname, string dispname)
        {
            Timer tokenTimer = new Timer(600);
            ServiceDataContext db = new ServiceDataContext();
            string pin;
            var search_alias = db.VmrAliases.FirstOrDefault(m => m.alias == confname);
            int search_id = search_alias.Id;
            var search_vmr = db.AllVmrs.FirstOrDefault(m => m.Id == search_id);
            var current_user = db.Service.FirstOrDefault(m => m.UserName == dispname);
            var role = current_user.Role;
            if (role == "PepuxAdmins")
            {
                pin = search_vmr.pin;
            }
            else
            {
                pin = search_vmr.guest_pin; }
            
            // return pin;
            string pepix_address = "10.157.155.11";
            Uri confapi = new Uri("https://" + pepix_address + "/api/client/v2/conferences/" + confname + "/request_token");
            WebClient client = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            client.Credentials = new NetworkCredential("admin", "ciscovoip");
            client.Headers.Add("ContentType","application/json");
            client.Headers.Add("auth", "admin,ciscovoip");
            client.Headers.Add("veryfy", "False");
            client.Headers.Add("pin",pin);
            string response = client.UploadString(confapi, "POST", "{\"display_name\":\"" + dispname + "\"}");
            root_token = JsonConvert.DeserializeObject<RootToken>(response);
            string token = root_token.result.token;
            if (token != "" || token != null)
            {
                tokenTimer.Interval = 600;
                tokenTimer.Start();
            }
            tokenTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            Debug.WriteLine(token);
            return token;
        }
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           //  https://10.157.155.11/api/client/v2/conferences/Parkin/refresh_token 
            Token_refresh("","");
        }

        public string Token_refresh(string confname, string old_token)
        {
            string pepix_address = "10.157.155.11";
            Uri confapi = new Uri("https://" + pepix_address + "/api/client/v2/conferences/" + confname + "/refresh_token");
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("admin", "ciscovoip");
            client.Headers.Add("ContentType", "application/json");
            client.Headers.Add("auth", "admin,ciscovoip");
            client.Headers.Add("veryfy", "False");
            client.Headers.Add("token", old_token);
            string response = client.UploadString(confapi, "POST", "{}");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var token_all = JsonConvert.DeserializeObject<ResultTokRef>(response);
            string token = token_all.token;
            return token;
        }

        public List<Vrecords> Videorecords(string filter, string val)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySqlDataAdapter daVrec;
            DataSet dsVrec;
            string myConnectionString;
            List<Vrecords> all_recs = new List<Vrecords>();

            myConnectionString = "server="+Properties.Settings.Default.SQLServ+";uid="+Properties.Settings.Default.SQLUser+";" +
                "pwd="+Properties.Settings.Default.SQLPass+";database="+Properties.Settings.Default.SQLBd+ ";Convert Zero Datetime=True";
            if (filter != null && val != null)
            {
                try
                {
                    string sql = "SELECT * FROM records WHERE " + filter + "='" + val + "'";
                    daVrec = new MySqlDataAdapter(sql, myConnectionString);
                    MySqlCommandBuilder cb = new MySqlCommandBuilder(daVrec);
                    dsVrec = new DataSet();
                    daVrec.Fill(dsVrec, "records");
                    foreach (DataRow dr in dsVrec.Tables["records"].Rows)
                    {
                        all_recs.Add(new Vrecords { ID = Int32.Parse(dr["ID"].ToString()), Conf = Convert.ToString(dr["Conf"]), PName = Convert.ToString(dr["PName"]), Tstart = DateTime.Parse(dr["Tstart"].ToString()), Tfinish = DateTime.Parse(dr["Tfinish"].ToString()), Link = Convert.ToString(dr["Link"]) });
                    }
                    return all_recs;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                }
            }
            try
            {

                string sql = "SELECT * FROM records";
                daVrec = new MySqlDataAdapter(sql, myConnectionString);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(daVrec);
                dsVrec = new DataSet();
                daVrec.Fill(dsVrec, "records");
                foreach (DataRow dr in dsVrec.Tables["records"].Rows)
                {
                    all_recs.Add(new Vrecords { ID = Int32.Parse(dr["ID"].ToString()), Conf = Convert.ToString(dr["Conf"]), PName = Convert.ToString(dr["PName"]), Tstart = DateTime.Parse(dr["Tstart"].ToString()), Tfinish = DateTime.Parse(dr["Tfinish"].ToString()), Link = Convert.ToString(dr["Link"]) });
                }
                return all_recs;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.ToString());
            }


            return all_recs;
        }

        #endregion
    }
}
