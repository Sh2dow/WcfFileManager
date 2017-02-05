using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Xml.Linq;
using DemoWCFServiceApp.Repository;

namespace DemoWCFServiceApp
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository customerRepository;

        public CustomerService()
        {
            customerRepository = new CustomerXmlRepository();
        }

        public IEnumerable<CustomerEntity> GetAllCustomers()
        {
            var allCustomer = customerRepository.GetAllCustomers();
            return allCustomer;
        }

        public void AddCustomer(CustomerEntity customer)
        {
            customerRepository.AddCustomer(customer);
        }

        public void DeleteCustomer(int id)
        {
            customerRepository.DeleteCustomer(id);
        }

        public void EditCustomer(CustomerEntity customer)
        {
            customerRepository.EditCustomer(customer);
        }

        public int GetCustomersCount()
        {
          return  customerRepository.GetCustomersCount();
        }
    }
}
