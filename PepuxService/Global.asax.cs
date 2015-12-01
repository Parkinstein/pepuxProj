using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using FluentScheduler;

namespace PepuxService
{
    public class Global : System.Web.HttpApplication
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        public static string GetProperty(SearchResult searchResult, 
 string PropertyName)
  {
   if(searchResult.Properties.Contains(PropertyName))
   {
    return searchResult.Properties[PropertyName][0].ToString() ;
   }
   else
   {
    return string.Empty;
   }
  }

        protected void Application_Start(object sender, EventArgs e)
        {
            TaskManager.Initialize(new MyRegistry());
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(update_Elapsed);
            //timer.Interval = 120000;
            //timer.Enabled = true;
            //timer.Start();
        }
        void update_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetPhonebookUsers();

        }
        public List<PBPlusrecord> GetPhonebookUsers()
        {
            List<PBPlusrecord> allreco = new List<PBPlusrecord>();
           try
            {

                string domainPath = "dc0.rad.lan.local/OU=Pepux,DC=rad,DC=lan,DC=local";
                DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + domainPath, Properties.Settings.Default.DN_login, Properties.Settings.Default.Dn_pass);
                DirectorySearcher dirSearcher = new DirectorySearcher(directoryEntry);
                dirSearcher.SearchScope = SearchScope.Subtree;
                dirSearcher.Filter = "(objectClass=user)";
                dirSearcher.PropertiesToLoad.Add("givenName");
                dirSearcher.PropertiesToLoad.Add("sn");
                dirSearcher.PropertiesToLoad.Add("title");
                dirSearcher.PropertiesToLoad.Add("telephoneNumber");
                dirSearcher.PropertiesToLoad.Add("sAMAccountName");
                dirSearcher.PropertiesToLoad.Add("displayName");
                dirSearcher.PropertiesToLoad.Add("email");
                SearchResultCollection resultCol = dirSearcher.FindAll();
                foreach (SearchResult resul in resultCol)
                {
                    PBPlusrecord objSurveyUsers = new PBPlusrecord();
                    objSurveyUsers.name = GetProperty(resul, "givenName");//(String)resul.Properties["givenName"][0];
                    objSurveyUsers.surname = GetProperty(resul, "sn"); //(String)resul.Properties["sn"][0];
                    objSurveyUsers.tel_int = GetProperty(resul, "telephoneNumber"); //(String)resul.Properties["telephoneNumber"][0];
                    objSurveyUsers.position = GetProperty(resul, "title"); //(String)resul.Properties["title"][0];
                    objSurveyUsers.email = GetProperty(resul, "email"); //(String)resul.Properties["email"][0];
                    objSurveyUsers.samaccountname = GetProperty(resul, "sAMAccountName"); //(String)resul.Properties["sAMAccountName"][0];
                    objSurveyUsers.dispname = GetProperty(resul, "displayName"); //(String)resul.Properties["displayName"][0];
                    allreco.Add(objSurveyUsers);
                    Debug.WriteLine(objSurveyUsers.email);
                }

                CompareUsers(allreco);

            }
            catch (Exception er)
            {
                Debug.WriteLine(er.HResult);
                Debug.WriteLine(er.Message);
            }
            return allreco;
         

        }

        public void CompareUsers(List<PBPlusrecord> adusList)
        {
            ServiceDataContext db = new ServiceDataContext();
            var temp_list = new List<string>();
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
                    }
                    //Debug.WriteLine("Все уже есть");
                }
                foreach (var stroke in temp_list)
                {
                    var deleteUsers =
                        from samaccountname in db.PhonebookDBs
                        where samaccountname.samaccountname == stroke
                        select samaccountname;
                    db.PhonebookDBs.DeleteOnSubmit(deleteUsers.First());
                    db.SubmitChanges();
                }

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
                    new_rec.location = false;
                    db.PhonebookDBs.InsertOnSubmit(new_rec);
                    db.SubmitChanges();
                    //Debug.WriteLine("Был добавлен " + new_rec.Name + " " + new_rec.Surname);
                }
            }


            foreach (var temp in temp_list)
            {
                Debug.WriteLine(temp);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
           // Debug.WriteLine("App_Session_started");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
           // Debug.WriteLine("App_Begin_request");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
           // Debug.WriteLine("App_Auth_request");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Debug.WriteLine("App_Error");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            //Debug.WriteLine("App_Session_ended");
            timer.Dispose();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Debug.WriteLine("App_stopped");
        }
    }
}