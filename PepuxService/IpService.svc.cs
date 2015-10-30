using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using System.Configuration;
using System.Web.Script.Serialization;
//using Kendo.Mvc.Extensions;
using MySql.Data.MySqlClient;
using PepuxService.Properties;
using Newtonsoft.Json;
using Renci.SshNet;

namespace PepuxService
{

    public sealed class IpService : IPService
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
         public int last_id;
        

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
        public IpService()
        {
            
        }
        
        #region Get_AD_Users

        public List<ADUsers> GetADUsvrs(string groupname)
        {
            try
            {

                var context = new PrincipalContext(ContextType.Domain, Settings.Default.Domen, "pepuxadmin", "1Q2w3e4r"); //+ "/DC=rad,DC=lan,DC=local"
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
                string reply1 = Win1251ToUTF8(reply);
                if (reply.ToString() != null)
                 {
                     AllConfs_wm = JsonConvert.DeserializeObject<ResponseParent>(reply1);
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

        public List<Vrecords> Videorecords(string val)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySqlDataAdapter daVrec;
            DataSet dsVrec;
            string myConnectionString;
            List<Vrecords> all_recs = new List<Vrecords>();

            myConnectionString = "server="+Properties.Settings.Default.SQLServ+";uid="+Properties.Settings.Default.SQLUser+";" +
                "pwd="+Properties.Settings.Default.SQLPass+";database="+Properties.Settings.Default.SQLBd+ ";Convert Zero Datetime=True";
            if (val != "")
            {
                try
                {
                    string sql = "SELECT * FROM records WHERE Conf " + " = \"" + val +"\""; //
                    Debug.WriteLine(sql);
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

        public bool DeleteRecordsFromDb(int id)
        {
            var myConnectionString = "server=" + Properties.Settings.Default.SQLServ + ";uid=" + Properties.Settings.Default.SQLUser + ";" +
                "pwd=" + Properties.Settings.Default.SQLPass + ";database=" + Properties.Settings.Default.SQLBd + ";Convert Zero Datetime=True";
            MySqlConnection conn = new MySqlConnection(myConnectionString);
            try
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT Link FROM records WHERE ID = " + id + "", conn);
                    conn.Open();
                    Debug.WriteLine(cmd.CommandText);
                    var mysqlAdp = new MySqlDataAdapter(cmd);
                    var mysqlDS = new DataSet();
                    mysqlAdp.Fill(mysqlDS, "Links");
                    foreach (DataRow dr in mysqlDS.Tables["Links"].Rows)
                    {
                        var link = Convert.ToString(dr["Link"]);
                        Debug.WriteLine(link);
                        int found = link.IndexOf("/records");
                        Debug.WriteLine(link.Substring(found));
                        cmd = new MySqlCommand("DELETE FROM records WHERE ID = " + id + "", conn);
                        Debug.WriteLine(cmd.CommandText);
                        cmd.ExecuteNonQuery();
                        // SSH procedure to delete records files
                        var PasswordConnection = new PasswordAuthenticationMethod("remote", "Rerih!123");
                        var KeyboardInteractive = new KeyboardInteractiveAuthenticationMethod("remote");
                        
                        var connectionInfo = new ConnectionInfo(Properties.Settings.Default.SQLServ, 22, "remote", PasswordConnection, KeyboardInteractive);
                        using (SshClient ssh = new SshClient(connectionInfo))
                        {
                            ssh.Connect();
                            var command = ssh.RunCommand("rm -f /home/rerih/public_html" + link.Substring(found));
                            Debug.WriteLine(command.CommandText);
                            ssh.Disconnect();
                    }

                }
                conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message.ToString());
                    return false;
                }
        }
        
        public  List<PBPlusrecord> GetPhonebookUsers()
        {
           List<PBPlusrecord> allreco = new List<PBPlusrecord>();
           try
            {

                var domainPath = "dc0.rad.lan.local/OU=Pepux,DC=rad,DC=lan,DC=local";
                var directoryEntry = new DirectoryEntry("LDAP://"+domainPath, Properties.Settings.Default.DN_login, Properties.Settings.Default.Dn_pass);
                var dirSearcher = new DirectorySearcher(directoryEntry);
                dirSearcher.SearchScope = SearchScope.Subtree;
                dirSearcher.Filter = string.Format("(objectClass=user)");
                dirSearcher.PropertiesToLoad.Add("givenName");
                dirSearcher.PropertiesToLoad.Add("sn");
                dirSearcher.PropertiesToLoad.Add("title");
                dirSearcher.PropertiesToLoad.Add("telephoneNumber");
                dirSearcher.PropertiesToLoad.Add("sAMAccountName");
                dirSearcher.PropertiesToLoad.Add("email");
                //var searchResults = dirSearcher.FindAll();
                SearchResult result;
                SearchResultCollection resultCol = dirSearcher.FindAll();
                if (resultCol != null)
                {
                    for (int i = 0; i < resultCol.Count; i++)
                    {

                        result = resultCol[i];
                        if (result.Properties.Contains("givenName") &&
                            result.Properties.Contains("sn") &&
                            result.Properties.Contains("telephoneNumber"))
                        {
                            PBPlusrecord objSurveyUsers = new PBPlusrecord();
                            objSurveyUsers.name = (String) result.Properties["givenName"][0];
                            objSurveyUsers.surname = (String) result.Properties["sn"][0];
                            objSurveyUsers.tel_int = (String) result.Properties["telephoneNumber"][0];
                            objSurveyUsers.position = (String) result.Properties["title"][0];
                            objSurveyUsers.email = (String)result.Properties["email"][0];
                            objSurveyUsers.samaccountname = (String)result.Properties["sAMAccountName"][0];
                            objSurveyUsers.dispname = (String)result.Properties["displayName"][0];
                            allreco.Add(objSurveyUsers);
                        }
                    }

                }
                else{}
                CompareUsers(allreco);

            }
            catch (Exception er)
            {
                Debug.WriteLine(er.HResult);
                Debug.WriteLine(er.Message);
            }
            return allreco;

        }

        public  void CompareUsers(List<PBPlusrecord> adusList)
        {
            ServiceDataContext db = new ServiceDataContext();
            var temp_list = new List<string>();
            var id_list = new List<int>();
            List<PBPlusrecord> allr = new List<PBPlusrecord>();
            var NameQuery =
                    from samaccountname in db.PhonebookDBs
                    select samaccountname;
            if (NameQuery != null)
            {
                foreach (var customer in NameQuery)
                {
                    if ((!adusList.Exists(x => x.samaccountname == customer.samaccountname) && !customer.location))
                    {
                        temp_list.Add(customer.samaccountname);
                        id_list.Add(customer.Id);
                    }
                }
                foreach (var stroke in temp_list)
                {
                    var deleteUsers =
                        from samaccountname in db.PhonebookDBs
                        where samaccountname.samaccountname == stroke
                        select samaccountname;
                    db.PhonebookDBs.DeleteOnSubmit(deleteUsers.First());
                    
                    
                }
                foreach (var ids in id_list)
                {
                    var deleteRecs =
                        from Id in db.PrivatePhBs
                        where Id.IdREC == ids
                        select Id;
                    db.PrivatePhBs.DeleteOnSubmit(deleteRecs.First());
                }
                db.SubmitChanges();

            }
            
                foreach (var adus in adusList)
                {
                    if (!NameQuery.AsEnumerable().ToList().Exists(x => x.samaccountname == adus.samaccountname))
                    {
                        PhonebookDB new_rec = new PhonebookDB();
                        new_rec.Name = adus.name;
                        new_rec.Surname = adus.surname;
                        new_rec.Position = adus.position;
                        new_rec.samaccountname = adus.samaccountname;
                        new_rec.Phone_int = adus.tel_int;
                        new_rec.email = adus.email;
                        new_rec.dispName = adus.dispname;
                        new_rec.location = false;
                        db.PhonebookDBs.InsertOnSubmit(new_rec);
                        db.SubmitChanges();
                        Debug.WriteLine("Был добавлен " + new_rec.Name + " " + new_rec.Surname);
                    }
                }
            

            foreach (var temp in temp_list)
            {
                Debug.WriteLine(temp);
            }
        }

        public bool DeleteRecFromDb(int id, string ownm)
        {
            ServiceDataContext db = new ServiceDataContext();
            var deleteDetails =
            from prop in db.PrivatePhBs
            where prop.IdREC == id && prop.OwSAN == ownm
            select prop;
            foreach (var detal in deleteDetails)
            {
                db.PrivatePhBs.DeleteOnSubmit(detal);
            }

            try
            {
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public bool addUserToPrivat(string Owner, int IdRec, string Group)
        {
            ServiceDataContext db = new ServiceDataContext();
            
                PrivatePhB newrecpriv = new PrivatePhB();
                newrecpriv.OwSAN = Owner;
                newrecpriv.IdREC = IdRec;
                newrecpriv.Group = Group;
                db.PrivatePhBs.InsertOnSubmit(newrecpriv);
            
            try
            {
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<PhonebookDB> GetPB()
        {
            ServiceDataContext db = new ServiceDataContext();
            List<PhonebookDB> allpbrecs = new List<PhonebookDB>();
            var records =
            from prop in db.PhonebookDBs
            select prop;
            foreach (var record in records)
            {
                allpbrecs.Add(record);
            }
            return allpbrecs;
        }

        public PhonebookDB AddRecordsToPB(string in_name, string in_surname, string in_position, string tel_int, string tel_ext, string tel_mob, string h323_add, string sip_add, string timezone, string group, string email, string OwNam)
        {
            PhonebookDB nr = new PhonebookDB();
            
            nr.Name = in_name;
            nr.Surname = in_surname;
            nr.Position = in_position;
            nr.Phone_int = tel_int;
            nr.Phone_ext = tel_ext;
            nr.Phone_mob = tel_mob;
            nr.H323Add = h323_add;
            nr.SipAdd = sip_add;
            nr.TimeZone = timezone;
            nr.email = email;
            nr.dispName = in_name + " " + in_surname;
            nr.location = true;
            AddRecToPB(nr, group, OwNam);
            return nr;
        }

        public void AddRecToPB(PhonebookDB obj, string group, string Ownam)
        {
            ServiceDataContext db = new ServiceDataContext();
            db.PhonebookDBs.InsertOnSubmit(obj);
            db.SubmitChanges();
            PrivatePhB owPhB = new PrivatePhB();
            owPhB.OwSAN = Ownam;
            owPhB.IdREC = obj.Id;
            owPhB.Group = group;
            db.PrivatePhBs.InsertOnSubmit(owPhB);
            db.SubmitChanges();
            Debug.WriteLine(obj.Id);
        }


        #endregion
        public bool Authenticate(string userName,string password, string domain)
        {
            bool authentic = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain,
                    userName, password);
                var nativeObject = entry.NativeObject;
                
                authentic = true;
            }
            catch (DirectoryServicesCOMException) { }
            return authentic;
        }

        public List<PBPlusrecord> GetPhBOw(string OwName) //get phonebook
        {
            ServiceDataContext db = new ServiceDataContext();
            List<PBPlusrecord> selrec = new List<PBPlusrecord>();
            var selectets = db.PrivatePhBs.Where(m => m.OwSAN == OwName);
            foreach (var sel in selectets)
            {
                PBPlusrecord temp = new PBPlusrecord();
                var srec = db.PhonebookDBs.FirstOrDefault(m => m.Id == sel.IdREC);
                temp.id = srec.Id;
                temp.name = srec.Name;
                temp.surname = srec.Surname;
                temp.position = srec.Position;
                temp.tel_int = srec.Phone_int;
                temp.tel_ext = srec.Phone_ext;
                temp.tel_mob = srec.Phone_mob;
                temp.h323_add = srec.H323Add;
                temp.sip_add = srec.SipAdd;
                temp.dispname = srec.dispName;
                temp.email = srec.email;
                temp.timezone = srec.TimeZone;
                if (!String.IsNullOrEmpty(sel.Group))
                {
                    temp.group = sel.Group;
                }
                if (String.IsNullOrEmpty(sel.Group))
                {
                    temp.group = "Группа не назначена";
                }
                
                
                selrec.Add(temp);
                
            }
            
            return selrec;
        }
    }
}
