using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Timers;
using System.Web.Services.Description;
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
        //public ActionResult PhonebookAll_Ajax(Phonebook.DTResult param)
        //{
        //    IEnumerable<IpServiceLink.PhonebookDB> filteredresult = GetAllPB();
        //    List<IpServiceLink.PhonebookDB> list = new List<PhonebookDB>();

        //    if (!String.IsNullOrEmpty(param.Search.Value))
        //    {
        //        foreach (var recs in filteredresult)
        //        {
        //            if (!String.IsNullOrEmpty(recs.Surname))
        //            {
        //                if (recs.Surname.IndexOf(param.Search.Value,0, StringComparison.CurrentCultureIgnoreCase) != -1)
        //                {
        //                    list.Add(recs);
        //                }
        //            }
        //            if (!String.IsNullOrEmpty(recs.Name))
        //            {
        //                if (recs.Name.IndexOf(param.Search.Value, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
        //                {
        //                    list.Add(recs);
        //                }
        //            }
        //            if (!String.IsNullOrEmpty(recs.Phone_int))
        //            {
        //                if (recs.Phone_int.IndexOf(param.Search.Value, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
        //                {
        //                    list.Add(recs);
        //                }
        //            }
        //            if (!String.IsNullOrEmpty(recs.email))
        //            {
        //                if (recs.email.IndexOf(param.Search.Value, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
        //                {
        //                    list.Add(recs);
        //                }
        //            }

        //        }
        //        filteredresult = list; 
        //    }

        //    return Json(new
        //    {
        //        recordsTotal = GetAllPB().Count(),
        //        recordsFiltered = filteredresult.Count(),
        //        data = filteredresult
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //Get Full Phonebook
        public ActionResult PhonebookAll_Ajax()
        {
            IEnumerable<IpServiceLink.PhonebookDB> filteredresult = GetAllPB();
            List<IpServiceLink.PhonebookDB> list = new List<PhonebookDB>();
            return Json(new
            {
                data = filteredresult
            }, JsonRequestBehavior.AllowGet);
        }


        //Get Personal Phonebook
        //public ActionResult Phonebook_Ajax(Phonebook.DTResult param)
        //{
        //    IEnumerable<IpServiceLink.addrec> filteredresult = GetPB();

        //    if (!string.IsNullOrEmpty(param.Search.Value))
        //    {
        //        filteredresult = GetPB().Where(c => (c.surname.Contains(param.Search.Value) || c.name.Contains(param.Search.Value)));
        //    }

        //    return Json(new
        //    {
        //        recordsTotal = GetPB().Count(),
        //        recordsFiltered = filteredresult.Count(),
        //        data = filteredresult,
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //Get Personal Phonebook
        public ActionResult Phonebook_Ajax()
        {
            IEnumerable<IpServiceLink.addrec> filteredresult = GetPB();

            return Json(new
            {
                data = filteredresult,
            }, JsonRequestBehavior.AllowGet);
        }


        // Add Phonebook records method
        public void Phonebook_Add(object[] pbrArray)
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var allpriv = obj.GetPhBOw(AccountController.SAMUname).AsQueryable();
            foreach (int pbr in pbrArray)
            {
                if (!allpriv.Any(m => m.id == pbr))
                    AddToPrivat(pbr);
                else { ViewBag.Message = "Запись уже существует"; Debug.WriteLine("Запись уже существует");}
            }
            obj.Close();
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
            obj.Close();
            return data;
        }


        // Get personal phonebook from service
        private IEnumerable<IpServiceLink.addrec> GetPB()
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            var data = obj.GetPhBOw(AccountController.SAMUname);
            obj.Close();
            return data;
        }

        // Add records to private phonebook
        public void AddToPrivat(int ids)
        {
            IpServiceLink.PServiceClient obj = new PServiceClient();
            bool result = obj.addUserToPrivat(AccountController.SAMUname, ids, null);
        }

        // Delete Phonebook records method
        public void DeleteFromPrivat(int ids)
        {
            IpServiceLink.PServiceClient act = new PServiceClient();
            act.DeleteRecFromDb(ids, AccountController.SAMUname);
            //ViewBag.DeletedRecs = String.Format("Удалено {0} записей", i);
        }


    }
}