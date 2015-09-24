using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Timers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;

namespace PepuxFront.Controllers
{
    public class PhonebookController : Controller
    {
        // GET: Phonebook view
        public ActionResult Phonebook()
        {
            return View();
        }


        //Get Full Phonebook
        public ActionResult PhonebookAll_Ajax(Phonebook.DTResult param)
        {
            IEnumerable<IpServiceLink.PhonebookDB> filteredresult = GetAllPB();
            List<IpServiceLink.PhonebookDB> list = new List<PhonebookDB>();

            if (!String.IsNullOrEmpty(param.Search.Value))
            {
                foreach (var recs in filteredresult)
                {
                    if (!String.IsNullOrEmpty(recs.Surname))
                    {
                        if (recs.Surname.IndexOf(param.Search.Value,0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            list.Add(recs);
                        }
                    }
                    if (!String.IsNullOrEmpty(recs.Name))
                    {
                        if (recs.Name.IndexOf(param.Search.Value, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            list.Add(recs);
                        }
                    }
                    if (!String.IsNullOrEmpty(recs.Phone_int))
                    {
                        if (recs.Phone_int.IndexOf(param.Search.Value, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            list.Add(recs);
                        }
                    }
                    if (!String.IsNullOrEmpty(recs.email))
                    {
                        if (recs.email.IndexOf(param.Search.Value, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                        {
                            list.Add(recs);
                        }
                    }
                    
                }
                filteredresult = list;
                //filteredresult = GetAllPB().Where(c => c.Surname.Contains(param.Search.Value)); //|| c.Name.Contains(param.Search.Value) || c.Phone_int.Contains(param.Search.Value) || c.Phone_ext.Contains(param.Search.Value)));
                
                
            }
            
            return Json(new
            {
                recordsTotal = GetAllPB().Count(),
                recordsFiltered = filteredresult.Count(),
                data = filteredresult
            }, JsonRequestBehavior.AllowGet);
        }


        //Get Personal Phonebook
        public ActionResult Phonebook_Ajax(Phonebook.DTResult param)
        {
            IEnumerable<IpServiceLink.addrec> filteredresult = GetPB();

            if (!string.IsNullOrEmpty(param.Search.Value))
            {
                filteredresult = GetPB().Where(c => c.surname.Contains(param.Search.Value));
            }
            else
            {
                filteredresult = GetPB();
            }

            return Json(new
            {
                recordsTotal = GetPB().Count(),
                recordsFiltered = filteredresult.Count(),
                data = filteredresult,
            }, JsonRequestBehavior.AllowGet);
        }


        // Add Phonebook records method
        public void Phonebook_Add(object[] pbrArray)
        {
            foreach (int pbr in pbrArray)
            {
                AddToPrivat(pbr);
            }
        }

        // Delete Phonebook records method
        public void Phonebook_Delete(object[] pbrArray)
        {
            foreach (int pbr in pbrArray)
            {
                DeleteFromPrivat(pbr);
            }
        }



        // Get full phonebook from service
        private IEnumerable<IpServiceLink.PhonebookDB> GetAllPB()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetPB();
            return data;
        }


        // Get personal phonebook from service
        private IEnumerable<IpServiceLink.addrec> GetPB()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetPhBOw(AccountController.SAMUname);
            return data;
        }

        // Add records to private phonebook
        public void AddToPrivat(int ids)
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            bool result = obj.addUserToPrivat("a_pilugin", ids, null);
        }

        // Delete Phonebook records method
        public void DeleteFromPrivat(int ids)
        {
            IpServiceLink.PServiceClient act = new PServiceClient();
            act.DeleteRecFromDb(ids, "a_pilugin");
            //ViewBag.DeletedRecs = String.Format("Удалено {0} записей", i);
        }


    }
}