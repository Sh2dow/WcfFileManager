using WebApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly string customerServiceUri = "http://localhost:1786/CustomerService.svc/";

        public ActionResult Index()
        {
            List<Models.File> customerList = new List<Models.File>();
            using (WebClient webClient = new WebClient())
            {
                string dwml;
                dwml = webClient.DownloadString(customerServiceUri + "GetAllCustomers");
                customerList = JsonConvert.DeserializeObjectAsync<List<Models.File>>(dwml).Result;
            }
            return View(customerList);
        }

        [HttpGet]
        public ActionResult GetCustomerPV()
        {
            return PartialView("CreateCustomerPV");
        }

        public ActionResult CreateCustomer(Models.File customer)
        {
            using (WebClient wc = new WebClient())
            {

                var ms = new MemoryStream();
                var serializerToUplaod = new DataContractJsonSerializer(typeof(Models.File));
                serializerToUplaod.WriteObject(ms, customer);

                wc.Headers["Content-type"] = "application/json";
                wc.UploadData(customerServiceUri + "AddCustomer", "POST", ms.ToArray());
            }

            int pageToShow;
            int totalCustomers;
            List<Models.File> customerList = new List<Models.File>();

            using (WebClient webClient = new WebClient())
            {
                string customerCount;
                customerCount = webClient.DownloadString(customerServiceUri + "GetCustomersCount");
                totalCustomers = Convert.ToInt32(customerCount);
            }

            if (totalCustomers % 5 != 0)
                pageToShow = (totalCustomers / 5) + 1;
            else pageToShow = totalCustomers / 5;

            return Redirect(HttpRuntime.AppDomainAppVirtualPath + "?page=" + pageToShow);
        }

        public void DeleteCustomer(int id)
        {
            using (WebClient wc = new WebClient())
            {

                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer serializerToUplaod = new DataContractJsonSerializer(typeof(Models.File));
                serializerToUplaod.WriteObject(ms, id);

                wc.Headers["Content-type"] = "application/json";
                wc.UploadData(customerServiceUri + "DeleteCustomer", "DELETE", ms.ToArray());
            }
        }

        public void EditCustomer(Models.File customer)
        {
            using (WebClient wc = new WebClient())
            {

                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer serializerToUplaod = new DataContractJsonSerializer(typeof(Models.File));
                serializerToUplaod.WriteObject(ms, customer);

                wc.Headers["Content-type"] = "application/json";
                wc.UploadData(customerServiceUri + "EditCustomer", "PUT", ms.ToArray());
            }
        }
    }
}