using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PepuxFront.IpServiceLink;
using PepuxFront.Models;

namespace PepuxFront.Controllers
{
    public class VideoRecordsController : Controller
    {
        // GET: Records
        public ActionResult VideoRecords()
        {
            return View();
        }
        public ActionResult All_VideoRec(string filter,string val,[DataSourceRequest]DataSourceRequest request)
        {
            using (var allvrecs = new IpServiceLink.PServiceClient())
            {

                IQueryable<IpServiceLink.allrecords> vrecs = allvrecs.Videorecords(filter,val).AsQueryable();

                DataSourceResult result = vrecs.ToDataSourceResult(request);

                return Json(result);
            }

        }
    }
}