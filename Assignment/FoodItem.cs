using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//==========================================================
// Student Number : S10272211E
// Student Name : Low Yong Jin Matthew
// Partner Name : Lee Hua Jay
//==========================================================

namespace Assignment
{
    internal class FoodItem
    {
        public string itemName { get; set; }
        public string itemDesc { get; set; }
        public double itemPrice { get; set; }
        public string customise { get; set; }
        public FoodItem() { }
        public FoodItem(string name, string desc, double price, string custom)
        {
            itemName = name;
            itemDesc = desc;
            itemPrice = price;
            customise = custom;
        }
        public override string ToString()
        {
            return $"  - {itemName}: {itemDesc} - ${itemPrice.ToString()}";
        }
    }
}
