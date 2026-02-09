using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10271111E
// Student Name : Lee Hua Jay
// Partner Name : Low Yong Jin Matthew
//==========================================================

namespace Assignment
{
    internal class Customer
    {
        public string emailAddress { get; set; }
        public string customerName { get; set; }
        public List<Order> orders { get; set; } = new List<Order>();
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