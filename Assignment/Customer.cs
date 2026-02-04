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
        public List<Order> orders { get; set; }
        public Customer() { }
        public Customer(string CustomerName, string CustomerEmail)
        {
            customerName = CustomerName;
            emailAddress = CustomerEmail;
        }
        public void AddOrder(Order order)
        { 
            orders.Add(order);
        }
        public void DisplayAllOrders()
        {
            for (int i = 0; i < orders.Count(); i++)
            {
                Console.WriteLine($"{i+1}. {orders[i].orderId} - {orders[i].orderStatus}");
            }
        }
           
        public bool RemoveOrder(Order order)
        {
            return orders.Remove(order);
        }
        public override string ToString()
        {
            return $"Customer Name: {customerName}, Email Address: {emailAddress}";
        }
    }
}