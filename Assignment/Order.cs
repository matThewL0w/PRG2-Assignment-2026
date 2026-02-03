using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    internal class Order
    {
        public int orderId { get; set; }
        public DateTime orderDateTime { get; set; }
        public double orderTotal { get; set; }
        public string orderStatus { get; set; }
        public string deliveryAddress { get; set; }
        public string orderPaymentMethod { get; set; }
        public bool orderPaid { get; set; }
        private List<OrderedFoodItem> orderedItems = new List<OrderedFoodItem>();
        public List<OrderedFoodItem> OrderedItems { get; set; }
        
        public Order() { }
        public Order(int id, DateTime dateTime, double total, string status, string address, string paymentMethod, bool paid)
        {
            orderId = id;
            orderDateTime = dateTime;
            orderTotal = total;
            orderStatus = status;
            deliveryAddress = address;
            orderPaymentMethod = paymentMethod;
            orderPaid = paid;
        }
        public double CalculateOrderTotal()
        {
            double total = 0;
            foreach (OrderedFoodItem item in OrderedItems)
            {
                total += item.CalculateSubTotal();
            }
            orderTotal = total;
            return orderTotal;
        }
        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            OrderedItems.Add(item);
        }
        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            bool result = OrderedItems.Remove(item);
            return result;
        }
        public void DisplayFoodItems()
        {
            for (int i = 0; i < OrderedItems.Count(); i++)
            {
                Console.WriteLine($"{i+1}. {OrderedItems[i].itemName} - {OrderedItems[i].qtyOrdered}");
            }
        }
        public override string ToString()
        {
            return $"Order ID: {orderId}, Order DateTime: {orderDateTime}, Order Total: {orderTotal}, Order Status: {orderStatus}, Delivery Address: {deliveryAddress}, Payment Method: {orderPaymentMethod}, Paid: {orderPaid}";
        }

    }
}
