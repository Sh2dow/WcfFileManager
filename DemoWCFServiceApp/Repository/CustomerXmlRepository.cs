using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;

namespace DemoWCFServiceApp.Repository
{
    public class CustomerXmlRepository : ICustomerRepository
    {
        private List<CustomerEntity> allCustomers;
        private XDocument customerData;
        private string filePath = AppDomain.CurrentDomain.BaseDirectory +
                                              "/App_Data/CustomerDB.xml";

        public CustomerXmlRepository()
        {
            allCustomers = new List<CustomerEntity>();
            customerData = XDocument.Load(filePath);

            var customers = from t in customerData.Descendants("customer")
                            select new CustomerEntity(
                                (int)t.Element("id"),
                                t.Element("name").Value,
                                t.Element("address").Value);

            allCustomers.AddRange(customers.ToList<CustomerEntity>());
        }

        // returns the all customers data 
        public IEnumerable<CustomerEntity> GetAllCustomers()
        {
            return allCustomers;
        }

        // accepts new customer object
        public void AddCustomer(CustomerEntity customer)
        {
            customer.CustomerId = (int)(from b in customerData.Descendants("customer") orderby (int)b.Element("id") descending select (int)b.Element("id")).FirstOrDefault() + 1;
            customerData.Root.Add(new XElement("customer", new XElement("id", customer.CustomerId),
                                    new XElement("name", customer.CustomerName),
                                    new XElement("address", customer.CustomerAddress)));
            customerData.Save(filePath);
        }

        // accepts id of a particular customer to be deleted
        public void DeleteCustomer(int id)
        {
            customerData.Root.Elements("customer").Where(i => (int)i.Element("id") == id).Remove();

            customerData.Save(filePath);
        }

        // accepts modified customer object
        public void EditCustomer(CustomerEntity customer)
        {
            XElement customerExisting = customerData.Root.Elements("customer").Where(i => (int)i.Element("id") == customer.CustomerId).FirstOrDefault();

            customerExisting.SetElementValue("name", customer.CustomerName);
            customerExisting.SetElementValue("address", customer.CustomerAddress);

            customerData.Save(filePath);
        }

        // returns total count of customers
        public int GetCustomersCount()
        {
            return allCustomers.Count;
        }
    }
}