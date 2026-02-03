using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment
{
    internal class Restaurant
    {
        public string restaurantId { get; set; }
        public string restaurantName { get; set; }
        public string restaurantEmail { get; set; }
        public List<Menu> menus { get; set; }
        private List<Order> orders = new List<Order>();
        public List<Order> Orders { get; set; }
        private List<SpecialOffer> specialOffers = new List<SpecialOffer>();
        public List<SpecialOffer> SpecialOffers {get; set;}
        public Restaurant() { }
        public Restaurant(string id, string name, string email)
        {
            restaurantId = id;
            restaurantName = name;
            restaurantEmail = email;
        }
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
            foreach (SpecialOffer specialoffer in specialOffers)
            {
                Console.WriteLine(specialoffer);
            }
            
        }
        public void DisplayMenu()
        {
            Console.WriteLine($"Restaurant: {restaurantName} ({restaurantId})");
            foreach (Menu m in menus)
            {
                foreach (FoodItem f in m.foodItems)
                {
                    Console.WriteLine(f.ToString());
                }
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
        public override string ToString()
        {
            return $"Restaurant ID: {restaurantId}, Restaurant Name: {restaurantName}, Restaurant Email: {restaurantEmail}";
        }
        
    }
}
