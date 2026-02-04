//==========================================================
// Student Number : S10272211E
// Student Name : Low Yong Jin Matthew
// Partner Name : Lee Hua Jay
//==========================================================

//==========================================================
// Student Number : S10271111E
// Student Name : Lee Hua Jay
// Partner Name : Low Yong Jin Matthew
//==========================================================

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

for (int i = 1; i < orderlines.Length; i++)
{
    string[] orderDATA = orderlines[i].Split(',');

    Order order = new Order(
        Convert.ToInt32(orderDATA[0]),
        Convert.ToDateTime(orderDATA[6]),
        Convert.ToDouble(orderDATA[7]),
        orderDATA[8],
        orderDATA[5],
        "-",
        true
    );

    // "Chicken Katsu Bento,1|Salmon Teriyaki Bento,1"
    string[] orderedItems = orderDATA[9].Split('|');

    foreach (string item in orderedItems)
    {
        string[] itemDetails = item.Split(',');

        string itemName = itemDetails[0];
        int itemQty = Convert.ToInt32(itemDetails[1]);

        foreach (FoodItem food in foodlist)
        {
            if (food.itemName == itemName)
            {
                OrderedFoodItem orderedFood = new OrderedFoodItem(
                    food.itemName,
                    food.itemDesc,
                    food.itemPrice,
                    "-",
                    itemQty
                );

                order.AddOrderedFoodItem(orderedFood);
                break;
            }
        }
    }

    // add order to customer + restaurant here
}


Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count} restaurants loaded.");
Console.WriteLine($"{foodCount} food items loaded!");
Console.WriteLine($"{customers.Count} customers loaded!"); 
Console.WriteLine($"{orderlines.Count()} orders loaded!");


void DisplayAllOrders()
{
    Console.WriteLine("All Orders");
    Console.WriteLine("==========");
    Console.WriteLine($"{"Order ID",-9}  {"Customer",-13}  {"Restaurant",-15}  {"Delivery Date/Time",-18}  {"Amount",-7}  {"Status",-9}");
    Console.WriteLine($"{"---------",-9}  {"----------",-13}  {"-------------",-15}  {"------------------",-18}  {"------",-7}  {"---------",-9}");
}