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
        public List<SpecialOffer> specialOffers { get; set; } = new List<SpecialOffer>();
        public Queue<Order> orderQueue { get; set; } = new Queue<Order>();
        public string restaurantId { get; set; }
        public string restaurantName { get; set; }
        public string restaurantEmail { get; set; }
        public void DisplayOrders()
        {
            foreach (Order queues in orderQueue)
            {
                Console.WriteLine(queues);
            }
        }
        public void DisplaySpecialOffers()
        {
            Console.WriteLine($"Special Offers at {restaurantName}:");

            if (specialOffers.Count == 0)
            {
                Console.WriteLine("No special offers available.");
                return;
            }

            foreach (SpecialOffer offer in specialOffers)
            {
                Console.WriteLine(offer);
            }
        }
        public void DisplayMenu()
        {
            Console.WriteLine($"Restaurant: {restaurantName} ({restaurantId})");
            foreach (Menu m in menus)
            {
                Console.WriteLine(m);
                m.DisplayFoodItems();
                Console.WriteLine();
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
        public Restaurant() { }
        public Restaurant(string id, string name, string email)
        {
            restaurantId = id;
            restaurantName = name;
            restaurantEmail = email;
        }
    }
}
