using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    internal class Menu
    {
        List<FoodItem> foodItems { get; set; } =  new List<FoodItem>();
        public string menuId { get; set; }
        public string menuName { get; set; }
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
        public string ToString()
        {
            return $"Menu ID: {menuId}, Menu Name: {menuName}";
        }
        public Menu() { }
        public Menu(string id, string name)
        {
            menuId = id;
            menuName = name;
        }
    }
}
