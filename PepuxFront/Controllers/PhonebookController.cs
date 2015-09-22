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

            if (!string.IsNullOrEmpty(param.Search.Value))
            {
                filteredresult = GetAllPB().Where(c => c.Surname.Contains(param.Search.Value)); //|| c.Name.Contains(param.Search.Value) || c.Phone_int.Contains(param.Search.Value) || c.Phone_ext.Contains(param.Search.Value)));
            }
            else
            {
                filteredresult = GetAllPB();
            }

            return Json(new
            {
                recordsTotal = GetAllPB().Count(),
                recordsFiltered = filteredresult.Count(),
                data = filteredresult,
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



        // Delete Phonebook records method
        public void Delete(int[] ids, string owner)
        {
            IpServiceLink.PServiceClient act = new PServiceClient();
            int i = 0;
            foreach (var id in ids)
            {
                try
                {
                    i++;
                    act.DeleteRecFromDb(id, owner);
                }
                catch(Exception ex) { Debug.WriteLine(ex.Message);}
            }
            ViewBag.DeletedRecs = String.Format("Удалено {0} записей", i);
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
    }
}