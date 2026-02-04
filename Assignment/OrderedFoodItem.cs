using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    internal class OrderedFoodItem : FoodItem
    {
        public int qtyOrdered { get; set; }
        public double subTotal { get; set; }
        public OrderedFoodItem(string name, string desc, double price, string customise, int quantity) : base(name, desc, price, customise)
        {
            qtyOrdered = quantity;
            subTotal = CalculateSubTotal();
        }
        public double CalculateSubTotal()
        {
            return itemPrice * qtyOrdered;
        }
    }
        
}
