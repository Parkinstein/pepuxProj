using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepuxService
{
   public class Sync : FluentScheduler.ITask
   {
        public static string GetProperty(SearchResult searchResult,string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public void Execute()
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
                dirSearcher.PropertiesToLoad.Add("mail");
                dirSearcher.PropertiesToLoad.Add("mobile");
                dirSearcher.PropertiesToLoad.Add("facsimileTelephoneNumber");
                SearchResultCollection resultCol = dirSearcher.FindAll();
                foreach (SearchResult resul in resultCol)
                {
                    PBPlusrecord objSurveyUsers = new PBPlusrecord();
                    objSurveyUsers.name = GetProperty(resul, "givenName");//(String)resul.Properties["givenName"][0];
                    objSurveyUsers.surname = GetProperty(resul, "sn"); //(String)resul.Properties["sn"][0];
                    objSurveyUsers.tel_int = GetProperty(resul, "telephoneNumber"); //(String)resul.Properties["telephoneNumber"][0];
                    objSurveyUsers.position = GetProperty(resul, "title"); //(String)resul.Properties["title"][0];
                    objSurveyUsers.email = GetProperty(resul, "mail"); //(String)resul.Properties["email"][0];
                    objSurveyUsers.samaccountname = GetProperty(resul, "sAMAccountName"); //(String)resul.Properties["sAMAccountName"][0];
                    objSurveyUsers.dispname = GetProperty(resul, "displayName"); //(String)resul.Properties["displayName"][0];
                    objSurveyUsers.tel_ext = GetProperty(resul, "facsimileTelephoneNumber");
                    objSurveyUsers.tel_mob = GetProperty(resul, "mobile");
                    allreco.Add(objSurveyUsers);
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
            var temp_list2 = new List<int>();
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
                        temp_list2.Add(customer.Id);

                        #region piece of insanity

                        if (!String.IsNullOrEmpty(customer.Phone_ext))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.Phone_mob))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.dispName))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.email))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.H323Add))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.Name))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.Phone_int))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.Position))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.SipAdd))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.samaccountname))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.Surname))
                        {

                        }
                        if (!String.IsNullOrEmpty(customer.TimeZone))
                        {

                        }

                        #endregion

                    }
                }
                foreach (var stroke in temp_list)
                {
                    var deleteUsers =
                        from samaccountname in db.PhonebookDBs
                        where samaccountname.samaccountname == stroke
                        select samaccountname;
                    var deleteowners = from samaccountname in db.PrivatePhBs
                                       where samaccountname.OwSAN == stroke
                                       select samaccountname;
                    foreach (var deleted in deleteowners)
                    {
                        db.PrivatePhBs.DeleteOnSubmit(deleted);
                    }
                    db.PhonebookDBs.DeleteOnSubmit(deleteUsers.First());
                    db.SubmitChanges();
                }
                foreach (var stroke in temp_list2)
                {
                    var deleteUsers =
                        from samaccountname in db.PrivatePhBs
                        where samaccountname.IdREC == stroke
                        select samaccountname;
                    foreach (var id in deleteUsers)
                    {
                        db.PrivatePhBs.DeleteOnSubmit(id);  
                    }
                    
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
                    new_rec.email = adus.email;
                    new_rec.dispName = adus.dispname;
                    new_rec.Phone_ext = adus.tel_ext;
                    new_rec.Phone_mob = adus.tel_mob;
                    db.PhonebookDBs.InsertOnSubmit(new_rec);
                    db.SubmitChanges();
                }
            }


            
        }

       
   }

}
