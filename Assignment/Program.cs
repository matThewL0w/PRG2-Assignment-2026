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
List<Order> orderlist = new List<Order>();
Stack<Order> refundStack = new Stack<Order>();

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
    string[] ordereditems = orderlines[i].Split("\"");
    string[] orderDATA = orderlines[i].Split(',');
    for (int j = 10; j < orderDATA.Length; j++)
    {
        orderDATA[9] += $"{orderDATA[9]},{orderDATA[j]}";
    }
    //id datetime total status address paymentmethod paid
    Order order = new Order(Convert.ToInt32(orderDATA[0]), Convert.ToDateTime(orderDATA[6]), Convert.ToDouble(orderDATA[7]), orderDATA[8], orderDATA[5], "-", true);
    string[] ordereditem = ordereditems[1].Split('|');
    foreach (string item in ordereditem)
    {
        string[] itemdetails = item.Split(',');
        int itemQty = Convert.ToInt32(itemdetails[1]);
        //"Chicken Katsu Bento,1|Salmon Teriyaki Bento,1"
        string finalitem = itemdetails[0];
        foreach (FoodItem food in foodlist)
        {
            if (food.itemName == finalitem)
            {
                order.AddOrderedFoodItem(
                    new OrderedFoodItem(
                        food.itemName,
                        food.itemDesc,
                        food.itemPrice,
                        "-",
                        itemQty
                    )
                );
                break;
            }
        }

    }

    orderlist.Add(order);
    for (int z = 0; z < customers.Count(); z++)
    {
        if (customers[z].emailAddress == orderDATA[1]) //CustomerEmail
        {
            customers[z].AddOrder(order);
            break;
        }
    }
    for (int x = 0; x < restaurants.Count(); x++)
    {
        if (restaurants[x].restaurantId == orderDATA[2]) //RestaurantId
        {
            restaurants[x].orderQueue.Enqueue(order);
            break;
        }
    }

}

Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count()} restaurants loaded.");
Console.WriteLine($"{foodlist.Count()} food items loaded!");
Console.WriteLine($"{customers.Count()} customers loaded!"); 
Console.WriteLine($"{orderlist.Count()} orders loaded!");


void DisplayAllOrders()
{
    Console.WriteLine("All Orders");
    Console.WriteLine("==========");
    Console.WriteLine(
        $"{"Order ID",-9}  {"Customer",-13}  {"Restaurant",-15}  {"Delivery Date/Time",-18}  {"Amount",-7}  {"Status",-9}"
    );
    Console.WriteLine(
        $"{"---------",-9}  {"----------",-13}  {"-------------",-15}  {"------------------",-18}  {"------",-7}  {"---------",-9}"
    );

    foreach (Restaurant restaurant in restaurants)
    {
        foreach (Order order in restaurant.orderQueue)
        {
            string customerName = "Unknown";

            // FINDER
            foreach (Customer customer in customers)
            {
                if (customer.orders.Contains(order))
                {
                    customerName = customer.customerName;
                    break;
                }
            }

            Console.WriteLine(
                $"{order.orderId,-9}  " +
                $"{customerName,-13}  " +
                $"{restaurant.restaurantName,-15}  " +
                $"{order.orderDateTime:dd/MM/yyyy HH:mm}    " +
                $"${order.orderTotal,-6:F2}  " +
                $"{order.orderStatus,-9}"
            );
        }
    }
}

void RemoveExistingOrder()
{
    Customer CorrectCustomer = null;
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");
    Console.Write("Enter Customer Email: ");
    string customerEmail = Console.ReadLine();
    foreach (Customer customer in customers)
    {
        if (customerEmail == customer.emailAddress)
        {
            CorrectCustomer = customer;
            break;
        }
    }
    Console.WriteLine("Pending Orders:");
    foreach (Order order in CorrectCustomer.orders)
    {
        if (order.orderStatus == "Pending")
        {
            Console.WriteLine(order.orderId);
        }
    }
    Console.Write("Enter Order ID: ");
    int orderId = Convert.ToInt32(Console.ReadLine());
    Order SelectedOrder = null;
    foreach (Order order in CorrectCustomer.orders)
    {
        if (orderId == order.orderId)
        {
            SelectedOrder = order;
            break;
        }
    }
    Console.WriteLine();
    Console.WriteLine($"Customer: {CorrectCustomer.customerName}");
    Console.WriteLine("Ordered Items:");
    SelectedOrder.DisplayFoodItems();
    Console.WriteLine($"Delivery date/time: {SelectedOrder.orderDateTime:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Total Amount: ${SelectedOrder.orderTotal:F2}");
    Console.WriteLine($"Order Status: {SelectedOrder.orderStatus}");
    Console.Write("Confirm deletion? [Y/N]: ");
    string confirm = Console.ReadLine();
    if (confirm.ToUpper() == "Y")
    {
        SelectedOrder.orderStatus = "Cancelled";
        refundStack.Push(SelectedOrder);

        Console.WriteLine(
            $"Order {SelectedOrder.orderId} cancelled. " +
            $"Refund of ${SelectedOrder.orderTotal:F2} processed."
        );
    }
    else
    {
        Console.WriteLine("Order deletion cancelled.");
        return;
    }

}
