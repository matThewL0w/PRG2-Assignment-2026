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
    internal class Restaurant
    {
        public string restaurantId { get; set; }
        public string restaurantName { get; set; }
        public string restaurantEmail { get; set; }
        public List<Menu> menus { get; set; }
        private List<Order> orders = new List<Order>();
        public List<Order> orderQueue { get; set; } = new List<Order>();
        private List<SpecialOffer> specialOffers = new List<SpecialOffer>();
        public List<SpecialOffer> SpecialOffers {get; set;}
        public Restaurant() { }
        public Restaurant(string id, string name, string email)
        {
            restaurantId = id;
            restaurantName = name;
            restaurantEmail = email;
            menus = new List<Menu>();
        }
        public void DisplayOrders()
        {
            foreach (Order queues in orderQueue)
            {
                Console.WriteLine(queues);
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
