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
while (true)
{
    Console.WriteLine("==== Welcome to the Gruberoo Food Delivery System ====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Processs an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("7. Exit");
    Console.Write("Enter your choice: ");
    int choice = Convert.ToInt32(Console.ReadLine());
    if (choice == 1)
    {
        Console.WriteLine("Implement in the future");
    }
    else if (choice == 2)
    {
        DisplayAllOrders();
        return;
    }
    else if (choice == 3)
    {
        Console.WriteLine("Implement in the future");
    }
    else if (choice == 4)
    {
        ProcessOrder();
        return;
    }
    else if (choice == 5)
    {
        //ModifyExistingOrder();
        return;
    }
    else if (choice == 6)
    {
        RemoveExistingOrder();
        return;
    }
    else if (choice == 7)
    {
        Console.WriteLine("Exiting the system. Goodbye!");
    }
    else
    {
        Console.WriteLine("Invalid choice. Please try again.");
    }
}



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
        else
        {
            Console.WriteLine("Customer not found.");
            return;
        }
    }
    Console.WriteLine("Pending Orders:");
    foreach (Order order in CorrectCustomer.orders)
    {
        if (order.orderStatus == "Pending")
        {
            Console.WriteLine(order.orderId);
        }
        else
        {
            Console.WriteLine("No pending orders found."); return;
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
        else
        {
            Console.WriteLine("Order not found.");
            return;
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

//void ModifyExistingOrder()
//{
//    Customer CorrectCustomer = null;
//    Console.WriteLine("Modify Order");
//    Console.WriteLine("============");
//    Console.Write("Enter Customer Email: ");
//    string customerEmail = Console.ReadLine();
//    foreach (Customer customer in customers)
//    {
//        if (customerEmail == customer.emailAddress)
//        {
//            CorrectCustomer = customer;
//            break;
//        }
//    }
//    Console.WriteLine("Pending Orders:");
//    foreach (Order order in CorrectCustomer.orders)
//    {
//        if (order.orderStatus == "Pending")
//        {
//            Console.WriteLine(order.orderId);
//        }
//    }
//    Console.Write("Enter Order ID: ");
//    int orderId = Convert.ToInt32(Console.ReadLine());
//    Order SelectedOrder = null;
//    foreach (Order order in CorrectCustomer.orders)
//    {
//        if (orderId == order.orderId)
//        {
//            SelectedOrder = order;
//            break;
//        }
//    }
//    Console.WriteLine("Order Items:");
//    int ordernumber = 1;
//    foreach (OrderedFoodItem item in SelectedOrder.OrderedItems)
//    {
//        Console.WriteLine($"{ordernumber}.{item.itemName} - {item.qtyOrdered}");
//        ordernumber++;
//    }
//    Console.WriteLine("Address:");
//    Console.WriteLine(SelectedOrder.deliveryAddress);
//    Console.WriteLine("Delivery Date/Time:");
//    Console.WriteLine(SelectedOrder.orderDateTime.ToString("dd/MM/yyyy HH:mm"));
//    Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
//    int choice = Convert.ToInt32(Console.ReadLine());
//    if (choice == 1)
//    {
//        Console.WriteLine("Enter the order (number) to modify:");
//        int itemnumber = Convert.ToInt32(Console.ReadLine());
//        OrderedFoodItem SelectedItem = SelectedOrder.OrderedItems[itemnumber - 1];
//        Console.WriteLine($"Selected Item: {SelectedItem.itemName} - {SelectedItem.qtyOrdered}");
//        Console.Write("Enter new quantity: ");
//        int newQty = Convert.ToInt32(Console.ReadLine());
//        SelectedItem.qtyOrdered = newQty;
//        Console.WriteLine("Item quantity updated.");
//        foreach (OrderedFoodItem item in SelectedOrder.OrderedItems)
//        {
//            Console.WriteLine($"{ordernumber}.{item.itemName} - {item.qtyOrdered}");
//            ordernumber++;
//        }
//    }
//    else if (choice == 2)
//    {
//        Console.Write("Enter new delivery address: ");
//        string newAddress = Console.ReadLine();
//        SelectedOrder.deliveryAddress = newAddress;
//        Console.WriteLine("Delivery address updated.");
//    }
//    else if (choice == 3)
//    {
//        Console.Write("Enter new delivery date and time (dd/MM/yyyy HH:mm): ");
//        DateTime newDateTime = Convert.ToDateTime(Console.ReadLine());
//        SelectedOrder.orderDateTime = newDateTime;
//        Console.WriteLine("Delivery date and time updated.");
//    }
//    Console.WriteLine();
//    Console.WriteLine($"Order {SelectedOrder.orderId} updated. New Delivery Time: {SelectedOrder.orderDateTime}");
//}

void ProcessOrder()
{
    Console.WriteLine("Process Order");
    Console.WriteLine("=============");
    Console.Write("Enter Restaurant ID: ");
    string restaurantId = Console.ReadLine();
    Restaurant SelectedRestaurant = null;
    Console.WriteLine();
    foreach (Restaurant restaurant in restaurants)
    {
        if (restaurant.restaurantId == restaurantId)
        {
            SelectedRestaurant = restaurant;
            break;
        }
        else
        {
            Console.WriteLine("Restaurant not found.");
            return;
        }
    }
    if (SelectedRestaurant.orderQueue.Count == 0 || SelectedRestaurant == null)
    {
        Console.WriteLine("No pending orders for this restaurant./Restaurant not found");
        return;
    }
    int orderCount = SelectedRestaurant.orderQueue.Count;

    for (int i = 0; i < orderCount; i++)
    {
        Order order = SelectedRestaurant.orderQueue.Dequeue();

        Console.WriteLine($"Order {order.orderId}:");

        // Find customer name
        string customerName = "Unknown";
        foreach (Customer c in customers)
        {
            if (c.orders.Contains(order))
            {
                customerName = c.customerName;
                break;
            }
            else
            {
                Console.WriteLine("Customer not found.");
                return;
            }
        }

        Console.WriteLine($"Customer: {customerName}");
        Console.WriteLine("Ordered Items:");
        order.DisplayFoodItems();
        Console.WriteLine($"Delivery date/time: {order.orderDateTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Total Amount: ${order.orderTotal:F2}");
        Console.WriteLine($"Order Status: {order.orderStatus}");

        Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
        string choice = Console.ReadLine().ToUpper();

        if (choice == "C" && order.orderStatus == "Pending")
        {
            order.orderStatus = "Preparing";
            Console.WriteLine($"Order {order.orderId} confirmed. Status: Preparing");
        }
        else if (choice == "R" && order.orderStatus == "Pending")
        {
            order.orderStatus = "Rejected";
            Console.WriteLine($"Order {order.orderId} rejected. Refund processed.");
        }
        else if (choice == "S" && order.orderStatus == "Cancelled")
        {
            Console.WriteLine("Order skipped.");
        }
        else if (choice == "D" && order.orderStatus == "Preparing")
        {
            order.orderStatus = "Delivered";
            Console.WriteLine($"Order {order.orderId} delivered.");
        }
        else
        {
            Console.WriteLine("Invalid action for current order status.");
        }

        Console.WriteLine();

        SelectedRestaurant.orderQueue.Enqueue(order);




    }
}