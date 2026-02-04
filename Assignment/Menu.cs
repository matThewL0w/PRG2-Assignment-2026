using System;
using System.Collections.Generic;
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
    internal class Menu
    {
        public List<FoodItem> foodItems { get; set; } = new List<FoodItem>();
        public string menuId { get; set; }
        public string menuName { get; set; }
        public Menu(string id, string name)
        {
            menuId = id;
            menuName = name;
        }

        public void AddFoodItem(FoodItem item)
        {
            foodItems.Add(item);
        }
        public bool RemoveFoodItem(FoodItem item)
        {
            bool result = foodItems.Remove(item);
            return result;
        }
        public void DisplayFoodItems()
        {
            foreach (FoodItem foodItem in foodItems)
            {
                Console.WriteLine(foodItem.ToString());
            }
        }
        public override string ToString()
        {
            return $"Restaurant: {menuName} ({menuId})";
        }
    }

}
