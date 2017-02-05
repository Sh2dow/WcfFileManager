using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DemoWCFServiceApp
{
    [DataContract]
    public class CustomerEntity
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerAddress { get; set; }

        public CustomerEntity(int id, string name, string address)
        {
            this.CustomerId = id;
            this.CustomerName = name;
            this.CustomerAddress = address;
        }

    }
}