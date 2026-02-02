using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    internal class FoodItem
    {
        public string itemName { get; set; }
        public string itemDesc { get; set; }
        public double itemPrice { get; set; }
        public string customise { get; set; }
        public override string ToString()
        {
            return $"Item Name: {itemName}, Description: {itemDesc}, Price: {itemPrice.ToString()}, Customise: {customise}";
        }
        public FoodItem() { }
        public FoodItem(string name, string desc, double price, string custom)
        {
            itemName = name;
            itemDesc = desc;
            itemPrice = price;
            customise = custom;
        }
    }
}
