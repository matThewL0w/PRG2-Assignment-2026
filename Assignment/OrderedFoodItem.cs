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
        public OrderedFoodItem() { }
        public OrderedFoodItem(double ItemPrice, int quantity, double subtotal)
        {
            itemPrice = ItemPrice;
            qtyOrdered = quantity;
            subTotal = subtotal;
        }
        public double CalculateSubTotal()
        {
            subTotal = itemPrice * qtyOrdered;
            return (subTotal);
        }
    }
        
}
