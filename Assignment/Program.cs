using System.Globalization;
using Assignment;
List<Restaurant> restaurants = new List<Restaurant>();
List<Customer> customers = new List<Customer>();

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
using (StreamReader orderINFO = new StreamReader("orders - Copy.csv"))
{
    orderINFO.ReadLine(); //SKIP HEADER
    while (!orderINFO.EndOfStream)
    {
        string[] orderDATA = orderINFO.ReadLine().Split(',');
        
    }
}
Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count} restaurants loaded.");
Console.WriteLine($"{foodCount} food items loaded!");
Console.WriteLine($"{customers.Count} customers loaded!"); 


//void DisplayAllOrders()
//{
//    Console.WriteLine();
//    Console.WriteLine("All Orders:");
//    Console.WriteLine("==========");
//    foreach (Restaurant restaurant in restaurants)
//    {


//    }

//}

