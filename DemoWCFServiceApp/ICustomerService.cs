using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DemoWCFServiceApp
{
    [ServiceContract]
    public interface ICustomerService
    {
        // WebGet attribute is used to make GET request in WCF REST service
        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "GetAllCustomers")]
        IEnumerable<CustomerEntity> GetAllCustomers();

        // WebInvoke attribute is used to make POST, DELETE and PUT request in WCF REST service
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "POST", UriTemplate = "AddCustomer")]
        void AddCustomer(CustomerEntity customer);

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "DELETE", UriTemplate = "DeleteCustomer")]
        void DeleteCustomer(int id);

        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, Method = "PUT", UriTemplate = "EditCustomer")]
        void EditCustomer(CustomerEntity customer);


        [WebGet(ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "GetCustomersCount")]
        int GetCustomersCount();
    }
}
