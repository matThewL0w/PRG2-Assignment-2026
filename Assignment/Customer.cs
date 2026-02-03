using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    internal class Customer
    {
        public string emailAddress { get; set; }
        public string customerName { get; set; }
        public Customer(string EmailAddress, string CustomerName)
        {
            emailAddress = EmailAddress;
            customerName = CustomerName;
        }
        public void AddOrder(Order order)
        {
            //put stuff here, error shld go away after implementing Order class
        }
        public override string ToString()
        {
            return $"Customer Name: {customerName}, Email Address: {emailAddress}";
        }
    }
}
