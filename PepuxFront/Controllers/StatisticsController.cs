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
    public class StatisticsController : Controller
    {
        // GET: Phonebook view
        public ActionResult Stats()
        {
            return View();
        }
        
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
                else { ViewBag.Message = "Запись уже существует"; Debug.WriteLine("Запись уже существует"); }
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