using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    internal class Restaurant
    {
        public List<Menu> menus = new List<Menu>();
        public string restaurantId { get; set; }
        public string restaurantName { get; set; }
        public string restaurantEmail { get; set; }
        public void DisplayOrders()
        {
            Console.WriteLine($"Restaurant: {restaurantName}");
            foreach (Menu m in menus)
            {
                Console.WriteLine(m);        
                m.DisplayFoodItems();        
                Console.WriteLine();
            }
        }
        public void DisplaySpecialOffers()
        {
            Console.WriteLine($"Special Offers at {restaurantName}:");
            foreach (Menu m in menus)
            {
                foreach (FoodItem fi in m.foodItems)
                {
                    if (fi.customise.ToLower().Contains("special"))
                    {
                        Console.WriteLine(fi.ToString());
                    }
                }
            }
        }
        public void DisplayMenus()
        {
            Console.WriteLine($"Menus at {restaurantName}:");
            foreach (Menu m in menus)
            {
                Console.WriteLine(m.ToString());
            }
        }
        public void AddMenu(Menu menu)
        {
            menus.Add(menu);
        }
        public bool RemoveMenu(Menu menu)
        {
            bool result = menus.Remove(menu);
            return result;
        }
        public string ToString()
        {
            return $"Restaurant ID: {restaurantId}, Restaurant Name: {restaurantName}, Restaurant Email: {restaurantEmail}";
        }
        public Restaurant() { }
        public Restaurant(string id, string name, string email)
        {
            restaurantId = id;
            restaurantName = name;
            restaurantEmail = email;
        }
    }
}
