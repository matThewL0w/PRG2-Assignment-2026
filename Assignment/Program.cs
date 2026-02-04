using System.Globalization;
using Assignment;
List<Restaurant> restaurants = new List<Restaurant>();
List<Customer> customers = new List<Customer>();
List<FoodItem> foodlist = new List<FoodItem>();
using (StreamReader reader = new StreamReader("restaurants.csv"))
{
    reader.ReadLine(); //SKIP HEADER
    while (!reader.EndOfStream)// execute until the end of the file
    {
        string[] restaurantINFO = reader.ReadLine().Split(',');
        Restaurant restaurant = new Restaurant (restaurantINFO[0], restaurantINFO[1], restaurantINFO[2]);

        restaurant.menus.Add(new Menu("M001", "Main Menu"));
        restaurants.Add(restaurant);
    } 

}
int foodCount = 0;
using (StreamReader foodinfo = new StreamReader("fooditems - Copy.csv"))
{
    foodinfo.ReadLine(); //SKIP HEADER
    while (!foodinfo.EndOfStream)// execute until the end of the file
    {
        string[] foodITEM = foodinfo.ReadLine().Split(',');
        string restaurantId = foodITEM[0];
        FoodItem foodItem = new FoodItem(foodITEM[1], foodITEM[2], Convert.ToDouble(foodITEM[3]), "");
        foodlist.Add(foodItem);
        foreach (Restaurant restaurant in restaurants)
        {
            if (restaurant.restaurantId == restaurantId)
            {
                restaurant.menus[0].foodItems.Add(foodItem);
                foodCount++;
                break;
            }
        }
    } 
}

using (StreamReader customerINFO = new StreamReader("customers.csv"))
{
    customerINFO.ReadLine(); //SKIP HEADER
    while (!customerINFO.EndOfStream)// execute until the end of the file
    {
        string[] customerDATA = customerINFO.ReadLine().Split(',');
        Customer customer = new Customer(customerDATA[0], customerDATA[1]); //Name then email
        customers.Add(customer);
    }
}
string[] orderlines = File.ReadAllLines("orders - Copy.csv");
//OrderId,CustomerEmail,RestaurantId,DeliveryDate,DeliveryTime,DeliveryAddress,CreatedDateTime,TotalAmount,Status,Items
for (int i = 1; i < orderlines.Count(); i++) 
{
    string[] orderDATA = orderlines[i].Split(',');
    //id datetime total status address paymentmethod paid
    Order order = new Order(Convert.ToInt32(orderDATA[0]), Convert.ToDateTime(orderDATA[6]), Convert.ToDouble(orderDATA[7]), orderDATA[8], orderDATA[5], "-" , true);
    foreach (string orderload in orderDATA)
    {
        string[] ordereditems = orderDATA[9].Split(';');
        foreach (string item in ordereditems)
        {
            string[] itemsplitter = item.Split('|');
            foreach (string indivitems in itemsplitter)
            {
                string[] itemdetails = Convert.ToString(indivitems).Split(',');
                int itemQty = Convert.ToInt32(itemdetails[1]);
                //"Chicken Katsu Bento,1|Salmon Teriyaki Bento,1"
                string finalitem = itemdetails[0];
                OrderedFoodItem orderadder = new OrderedFoodItem();
                foreach (FoodItem food in foodlist)
                    if(food.itemName == finalitem)
                    {
                        OrderedFoodItem ordereplacer = new OrderedFoodItem(food.itemName, food.itemDesc, food.itemPrice, "-", itemQty);
                        orderadder = ordereplacer;
                        break;
                    }//name, desc, price, customise
                order.AddOrderedFoodItem(orderadder);
            }
        }
        
    }
    
    
}

Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count} restaurants loaded.");
Console.WriteLine($"{foodCount} food items loaded!");
Console.WriteLine($"{customers.Count} customers loaded!"); 
Console.WriteLine($"{orderlines.Count()} orders loaded!");


//void DisplayAllOrders()
//{
//    Console.WriteLine();
//    Console.WriteLine("All Orders:");
//    Console.WriteLine("==========");
//    foreach (Restaurant restaurant in restaurants)
//    {


//    }

//}

