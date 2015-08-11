using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.SignalR;
using PepuxFront.IpServiceLink;
using Newtonsoft.Json;
using PepuxFront.Controllers;

using PepuxFront.Models;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.Services.Description;
using RestSharp;

namespace PepuxFront.Hubs
{
    public class VmrHub : Hub
    {
        private IpServiceLink.PServiceClient vmrService;

        public VmrHub()
        {
            vmrService = new PServiceClient();

        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            using (vmrService)
            {
                IQueryable<ActiveConfs> confs = vmrService.GetActiveConfs().AsQueryable();
                DataSourceResult result = confs.ToDataSourceResult(request);

                return null;
            }
        }
        

        //public void Update(ProductViewModel product)
        //{
        //    productService.Update(product);
        //    Clients.Others.update(product);
        //}

        //public void Destroy(ProductViewModel product)
        //{
        //    productService.Destroy(product);
        //    Clients.Others.destroy(product);
        //}

        //public ProductViewModel Create(ProductViewModel product)
        //{
        //    productService.Create(product);
        //    Clients.Others.create(product);

        //    return product;
        //}
    }
}