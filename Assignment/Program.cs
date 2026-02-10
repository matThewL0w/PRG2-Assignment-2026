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

using Assignment;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Globalization;
List<Restaurant> restaurants = new List<Restaurant>();
List<Customer> customers = new List<Customer>();
List<FoodItem> foodlist = new List<FoodItem>();
List<Order> orderlist = new List<Order>();
Stack<Order> refundStack = new Stack<Order>();

void SaveDataOnExit()
{
    SaveRestaurantQueuesToCSV();
    SaveRefundStackToCSV();
    Console.WriteLine("Data saved to queue.csv and stack.csv");
}

void SaveRestaurantQueuesToCSV()
{
    using (StreamWriter writer = new StreamWriter("queue.csv"))
    {
        writer.WriteLine("RestaurantId,OrderId,CustomerEmail,OrderDateTime,TotalAmount,Status,DeliveryAddress");

        foreach (Restaurant restaurant in restaurants)
        {
            foreach (Order order in restaurant.orderQueue)
            {
                // Find customer email for this order
                string customerEmail = "Unknown";
                foreach (Customer customer in customers)
                {
                    if (customer.orders.Contains(order))
                    {
                        customerEmail = customer.emailAddress;
                        break;
                    }
                }

                writer.WriteLine($"{restaurant.restaurantId},{order.orderId}," +
                               $"{customerEmail},{order.orderDateTime:yyyy-MM-dd HH:mm}," +
                               $"{order.orderTotal:F2},{order.orderStatus},{order.deliveryAddress}");
            }
        }
    }
}

void SaveRefundStackToCSV()
{
    using (StreamWriter writer = new StreamWriter("stack.csv"))
    {
        writer.WriteLine("OrderId,CustomerEmail,OrderDateTime,TotalAmount,Status,DeliveryAddress");

        // Convert stack to array to iterate without modification issues
        Order[] refunds = refundStack.ToArray();

        foreach (Order order in refunds)
        {
            // Find customer email for this order
            string customerEmail = "Unknown";
            foreach (Customer customer in customers)
            {
                if (customer.orders.Contains(order))
                {
                    customerEmail = customer.emailAddress;
                    break;
                }
            }

            writer.WriteLine($"{order.orderId},{customerEmail}," +
                           $"{order.orderDateTime:yyyy-MM-dd HH:mm}," +
                           $"{order.orderTotal:F2},{order.orderStatus},{order.deliveryAddress}");
        }
    }
}


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
            restaurants[x].orderQueue.Add(order);
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
    Console.WriteLine("7. Display total order amount");
    Console.WriteLine("8. Bulk processing of orders");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");
    int choice = Convert.ToInt32(Console.ReadLine());
    if (choice == 1)
    {
        DisplayAllItems();
    }
    else if (choice == 2)
    {
        DisplayAllOrders();
    }
    else if (choice == 3)
    {
        //OrderCreation();
    }
    else if (choice == 4)
    {
        //ProcessOrder();
    }
    else if (choice == 5)
    {
        ModifyExistingOrder();
    }
    else if (choice == 6)
    {
        DeleteOrder();
    }
    else if (choice == 7)
    {
        DisplayTotalOrderAmount();
    }
    else if (choice == 8)
    {
        //BulkProcessing()
    }

    else if (choice == 0)
    {
        SaveDataOnExit();
        Console.WriteLine("Exiting the system. Goodbye!");
        break;
    }
    else
    {
        Console.WriteLine("Invalid choice. Please try again.");
    }
}

//string ValidateEmail()
//{
//    string customeremail = Console.ReadLine();
//    for (int i = 0; i < customers.Count(); i++)
//    {
//        if (customers[i].emailAddress == customeremail)
//        {
//            return customeremail;
//        }
//    }
//    Console.WriteLine("Customer not found. Please register first.");
//    return null;
//}

//string ValidateRestaurant()
//{
//    string restaurantId = Console.ReadLine();
//    for (int i = 0; i < restaurants.Count(); i++)
//    {
//        if (restaurants[i].restaurantId == restaurantId.ToUpper())
//        {
//            return restaurantId;
//        }
//    }
//    Console.WriteLine("Restaurant not found. Please try again.");
//    return null;
//}

    void DisplayAllItems()
{
    Console.WriteLine("All Restaurants and Menu Items");
    Console.WriteLine("==============================");
    foreach (Restaurant restaurant in restaurants)  
    {
        restaurant.DisplayMenu();
        Console.WriteLine();
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

    foreach (Order order in orderlist)
    {
        string customerName = "Unknown";
        string restaurantName = "Unknown";




        // FINDER
        foreach (Customer customer in customers)
        {
            if (customer.orders.Contains(order))
            {
                customerName = customer.customerName;
                break;
            }
        }

        foreach (Restaurant restaurant in restaurants)
        {
            if (restaurant.orderQueue.Contains(order))
            {
                restaurantName = restaurant.restaurantName;
                break;
            }
        }

        Console.WriteLine(
            $"{order.orderId,-9}  " +
            $"{customerName,-13}  " +
            $"{restaurantName,-15}  " +
            $"{order.orderDateTime:dd/MM/yyyy HH:mm}    " +
            $"${order.orderTotal,-6:F2}  " +
            $"{order.orderStatus,-9}"
        );

    }
}




    void OrderCreation()
    {
        Customer selectedcustomer = new Customer();
        Restaurant selectedrestaurant = new Restaurant();
        List<OrderedFoodItem> allorderedfooditems = new List<OrderedFoodItem>();
        string specialreq = "-";
        Console.WriteLine("Create New Order");
        Console.WriteLine("================");
        Console.Write("Enter Customer email: "); //public Order(int id, DateTime dateTime, double total, string status, string address, string paymentMethod, bool paid)
        string customeremail = Console.ReadLine();
        for (int i = 0; i < customers.Count(); i++)
        {
            if (customers[i].emailAddress == customeremail)
            {
                selectedcustomer = customers[i];
                break;
            }
            else if (i == customers.Count() - 1)
            {
                Console.WriteLine("Customer not found. Please register first.");
                return;
            }
        }
        Console.Write("Enter Restaurant ID: ");
        string restaurantId = Console.ReadLine();
        for (int i = 0; i < restaurants.Count(); i++)
        {
            if (restaurants[i].restaurantId == restaurantId.ToUpper())
            {
                selectedrestaurant = restaurants[i];
                break;
            }
            else if (i == restaurants.Count() - 1)
            {
                Console.WriteLine("Restaurant not found. Please try again.");
                return;
            }
        }
        Console.Write("Enter Delivery Date(dd / mm / yyyy): ");
        string deliveryDate = Console.ReadLine();
        Console.Write("Enter Delivery Time (hh:mm): ");
        string deliveryTime = Console.ReadLine();
        try
        {
            DateTime deliveryDateTime = DateTime.ParseExact(
                $"{deliveryDate} {deliveryTime}",
                "dd/MM/yyyy HH:mm",
                CultureInfo.InvariantCulture //ignore local regional settings
            );
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid date/time format. Please use dd/mm/yyyy for date and hh:mm for time.");
            return;
        }
        Console.Write("Enter Delivery Address: ");
        string deliveryAddress = Console.ReadLine();
        string[] addressvalidator = deliveryAddress.Split(' ');
        //clarified by teacher that not all addresses have unit numbers (landed property house number?)
        if (addressvalidator.Length < 2)//3) (3 for unit number)
        {
            Console.WriteLine("Invalid address. Please provide a complete address.");
            return;
        }
        else
        {
            try
            {
                Convert.ToInt32(addressvalidator[0]);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid address. Addresses start with a numeric street number.");
                return;
            }
            //string unit = addressvalidator[^1];

            //if (unit.Length != 6 ||
            //    unit[0] != '#' ||
            //    unit[3] != '-' ||
            //    !char.IsDigit(unit[1]) ||
            //    !char.IsDigit(unit[2]) ||
            //    !char.IsDigit(unit[4]) ||
            //    !char.IsDigit(unit[5]))
            //{
            //    Console.WriteLine("Invalid address. Unit numbers should be in the format #XX-XX.");
            //    return;
            //} //unit number validation
        }
        Console.WriteLine("Available Food Items: ");
        selectedrestaurant.menus[0].DisplayFoodItems();
        Menu selectedmenu = selectedrestaurant.menus[0];
        int itemnumber = 1;
        do
        {
            Console.Write("Enter item number (0 to finish): ");
            try
            {
                itemnumber = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }
            if (itemnumber > selectedmenu.foodItems.Count())
            {
                Console.WriteLine("Invalid item number. Please try again.");
                continue;
            }
            FoodItem selectedfooditem = selectedmenu.foodItems[itemnumber - 1];
            Console.Write($"Enter quantity: {selectedfooditem.itemName}: ");
            int quantity;
            try
            {
                quantity = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }
            OrderedFoodItem orderedfooditem = new OrderedFoodItem(
                selectedfooditem.itemName,
                selectedfooditem.itemDesc,
                selectedfooditem.itemPrice,
                "-",
                quantity
            );
            allorderedfooditems.Add(orderedfooditem);
        } while (itemnumber != 0);
        if (allorderedfooditems.Count() == 0)
        {
            Console.WriteLine("No items ordered. Order creation cancelled.");
            return;
        }
        Console.Write("Add special request? [Y/N]: ");
        string specialrequestchoice = Console.ReadLine();
        if (specialrequestchoice.ToUpper() == "Y")
        {
            Console.Write("Enter special request: ");
            specialreq = Console.ReadLine();
        }
        else if (specialrequestchoice.ToUpper() != "N")
        {
            Console.WriteLine("Invalid choice. No special request added.");
        }
        Order createdorder = new Order(
        orderlist[^1].orderId + 1,
        Convert.ToDateTime($"{deliveryDate} {deliveryTime}"),
        0.0,
        "Pending",
        deliveryAddress,
        specialreq,
        false
        );
        foreach (OrderedFoodItem item in allorderedfooditems)
        {
            createdorder.AddOrderedFoodItem(item);
        }
        double totalamount = createdorder.CalculateOrderTotal() + 5;
        Console.WriteLine();
        Console.WriteLine($"Order Total: ${createdorder.CalculateOrderTotal:F2} + $5.00 (delivery) = ${totalamount:F2}");
        Console.Write("Proceed to payment? [Y/N]: ");
        string paymentchoice = Console.ReadLine();
        if (paymentchoice.ToUpper() == "Y")
        {
            createdorder.orderPaid = true;
            Console.WriteLine("Payment successful.");
        }
        else
        {
            Console.WriteLine("Order creation cancelled.");
            Console.WriteLine();
            return;
        }
        Console.WriteLine();
        string paymentmethod = " ";
        do
        {
            Console.WriteLine("[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
            paymentmethod = Console.ReadLine();
            if (paymentmethod.ToUpper() == "CC" || paymentmethod.ToUpper() == "PP" || paymentmethod.ToUpper() == "CD")
            {
                break;
            }
            Console.WriteLine("Invalid payment method. Please try again.");
        } while (true);
        Console.WriteLine();
        createdorder.orderPaymentMethod = paymentmethod.ToUpper();
        Console.WriteLine($"Order {createdorder.orderId} created successfully! Status: {createdorder.orderStatus}");
        selectedcustomer.AddOrder(createdorder);
        selectedrestaurant.orderQueue.Add(createdorder);
        orderlist.Add(createdorder);
    }

void DeleteOrder()
{
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");

    Console.Write("Enter Customer Email: ");
    string email = Console.ReadLine();

    Customer selectedCustomer = null;

    foreach (Customer c in customers)
    {
        if (c.emailAddress == email)
        {
            selectedCustomer = c;
            break;
        }
    }

    if (selectedCustomer == null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    Console.WriteLine("Pending Orders:");

    List<Order> pendingOrders = new List<Order>();

    foreach (Order o in selectedCustomer.orders)
    {
        if (o.orderStatus == "Pending")
        {
            Console.WriteLine(o.orderId);
            pendingOrders.Add(o);
        }
    }

    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders.");
        return;
    }

    Console.Write("Enter Order ID: ");
    int orderId = Convert.ToInt32(Console.ReadLine());

    Order selectedOrder = null;

    foreach (Order o in pendingOrders)
    {
        if (o.orderId == orderId)
        {
            selectedOrder = o;
            break;
        }
    }

    if (selectedOrder == null)
    {
        Console.WriteLine("Invalid Order ID.");
        return;
    }

    Console.WriteLine($"Customer: {selectedCustomer.customerName}");
    Console.WriteLine("Ordered Items:");
    selectedOrder.DisplayOrderedFoodItems();
    Console.WriteLine($"Delivery date/time: {selectedOrder.orderDateTime:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Total Amount: ${selectedOrder.orderTotal:F2}");
    Console.WriteLine($"Order Status: {selectedOrder.orderStatus}");

    string confirm;

    while (true)
    {
        Console.Write("Confirm deletion? [Y/N]: ");
        confirm = Console.ReadLine().Trim().ToUpper();

        if (confirm == "Y" || confirm == "N")
            break;

        Console.WriteLine("Please enter Y or N only.");
    }

    if (confirm == "Y")
    {
        selectedOrder.orderStatus = "Cancelled";
        refundStack.Push(selectedOrder);

        Console.WriteLine(
            $"Order {selectedOrder.orderId} cancelled. " +
            $"Refund of ${selectedOrder.orderTotal:F2} processed."
        );
    }
    else
    {
        Console.WriteLine("Order deletion cancelled.");
    }
}

void ModifyExistingOrder()
{
    Customer selectedcustomer = new Customer();
    Order selectedOrder = new Order();
    int pendingorders = 0;
    int customerposition = 0;
    int restaurantposition = 0;
    int orderpositionrestaurant = 0;
    Console.WriteLine("Modify Order");
    Console.WriteLine("============");
    Console.Write("Enter Customer email: "); //public Order(int id, DateTime dateTime, double total, string status, string address, string paymentMethod, bool paid)
    string customeremail = Console.ReadLine();
    for (int i = 0; i < customers.Count(); i++)
    {
        if (customers[i].emailAddress == customeremail)
        {
            customerposition = i;
            selectedcustomer = customers[i];
            break;
        }
        else if (i == customers.Count() - 1)
        {
            Console.WriteLine("Customer not found. Please register first.");
            return;
        }
    }
    foreach (Order order in selectedcustomer.orders)
    {
        if (order.orderStatus == "Pending")
        {
            pendingorders += 1;
        }
    }
    if (pendingorders == 0)
    {
        Console.WriteLine("No pending orders for this customer.");
        return;
    }
    Console.WriteLine("Pending Orders:");
    foreach (Order order in selectedcustomer.orders)
    {
        if (order.orderStatus == "Pending")
        {
            Console.WriteLine(order.orderId);
        }
    }
    Console.Write("Enter Order ID: ");
    string chosenorderId = Console.ReadLine();
    bool valid = true;
    do
    {
        foreach (Order order in selectedcustomer.orders)
        {
            if (chosenorderId == Convert.ToString(order.orderId))
            {
                selectedOrder = order;
                valid = false;
                break;
            }
        }
        Console.Write("Invalid order ID. Please enter a valid order ID: ");
        chosenorderId = Console.ReadLine();
    } while (valid);
    for (int i = 0; i < restaurants.Count(); i++)
    {
        if (restaurants[i].orderQueue.Contains(selectedOrder))
        {
            restaurantposition = i;
            for (int j = 0; j < restaurants[i].orderQueue.Count(); j++)
            {
                if (restaurants[i].orderQueue[j] == selectedOrder)
                {
                    orderpositionrestaurant = j;
                    break;
                }
            }
            break;
        }
    }
            Console.WriteLine("Order Items:");
    int ordernumber = 1;
    foreach (OrderedFoodItem item in selectedOrder.OrderedItems)
    {
        Console.WriteLine($"{ordernumber}.{item.itemName} - {item.qtyOrdered}");
        ordernumber++;
    }
    Console.WriteLine("Address:");
    Console.WriteLine(selectedOrder.deliveryAddress);
    Console.WriteLine("Delivery Date/Time:");
    Console.WriteLine(selectedOrder.orderDateTime.ToString("dd/MM/yyyy HH:mm"));
    Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
    string choice = Console.ReadLine();
    if (choice == "1")
    {
        Restaurant selectedrestaurant = restaurants[restaurantposition];
        List<OrderedFoodItem> allorderedfooditems = new List<OrderedFoodItem>();
        selectedrestaurant.menus[0].DisplayFoodItems();
        Menu selectedmenu = selectedrestaurant.menus[0];
        int itemnumber = 1;
        do
        {
            Console.Write("Enter item number (0 to finish): ");
            try
            {
                itemnumber = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }
            if (itemnumber > selectedmenu.foodItems.Count())
            {
                Console.WriteLine("Invalid item number. Please try again.");
                continue;
            }
            FoodItem selectedfooditem = selectedmenu.foodItems[itemnumber - 1];
            Console.Write($"Enter quantity: {selectedfooditem.itemName}: ");
            int quantity;
            try
            {
                quantity = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }
            OrderedFoodItem orderedfooditem = new OrderedFoodItem(
                selectedfooditem.itemName,
                selectedfooditem.itemDesc,
                selectedfooditem.itemPrice,
                "-",
                quantity
            );
            allorderedfooditems.Add(orderedfooditem);
        } while (itemnumber != 0);
        if (allorderedfooditems.Count() == 0)
        {
            Console.WriteLine("No items ordered. Order modification cancelled.");
            return;
        }
        for (int i = 0; i < selectedOrder.OrderedItems.Count(); i++)
        {
            selectedOrder.OrderedItems.RemoveAt(0);
        }
        for (int i = 0; i < allorderedfooditems.Count(); i++)
        {
            selectedOrder.OrderedItems.Add(allorderedfooditems[i]);
        }
        //Console.WriteLine("Enter the item number to modify:");
        //int itemnumber = 0;
        //do
        //{
        //    try
        //    {
        //        itemnumber = Convert.ToInt32(Console.ReadLine());
        //    }
        //    catch (FormatException)
        //    {
        //        Console.Write("Please enter a valid item number: ");
        //    }
        //  if ((itemnumber - 1) <= selectedOrder.OrderedItems.Count)
        //  {
        //      break;
        //  }
        //  Console.Write("Please enter a valid item number: ");
        //} while (true);

        //OrderedFoodItem SelectedItem = selectedOrder.OrderedItems[itemnumber - 1];
        //Console.WriteLine($"Selected Item: {SelectedItem.itemName} - {SelectedItem.qtyOrdered}");
        //Console.Write("Enter new quantity: ");
        //int newQty = Convert.ToInt32(Console.ReadLine());
        //SelectedItem.qtyOrdered = newQty;
        Console.WriteLine("Item quantity updated.");
        for (int i = 0; i < selectedOrder.OrderedItems.Count(); i++)
        {
            Console.WriteLine($"{i + 1}.{selectedOrder.OrderedItems[i].itemName} - {selectedOrder.OrderedItems[i].qtyOrdered}");
        }
    }
    else if (choice == "2")
    {
        Console.Write("Enter new delivery address: ");
        string newAddress = Console.ReadLine();
        //didn't validate address as lecturer said it wasn't necessary
        selectedOrder.deliveryAddress = newAddress;
        Console.WriteLine("Delivery address updated.");
    }
    else if (choice == "3")
    {
        Console.Write("Enter new delivery date and time (dd/MM/yyyy HH:mm): ");
        DateTime newDateTime;
        do
        {
            try
            {
                newDateTime = Convert.ToDateTime(Console.ReadLine());
                selectedOrder.orderDateTime = newDateTime;
                break;
            }
            catch (FormatException)
            {
                Console.Write("Invalid date/time format. Please use dd/MM/yyyy HH:mm: ");
            }
        } while (true);
        selectedOrder.orderDateTime = newDateTime;
        Console.WriteLine("Delivery date and time updated.");
    }
    else
    {
        Console.WriteLine("Invalid choice. No modifications made.");
        return;
    }
    customers[customerposition] = selectedcustomer;
    restaurants[restaurantposition].orderQueue[orderpositionrestaurant] = selectedOrder;
    Console.WriteLine();
    Console.WriteLine($"Order {selectedOrder.orderId} updated. New Delivery Time: {selectedOrder.orderDateTime}");
}

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
        }
        if (SelectedRestaurant == null)
        {
            Console.WriteLine("Restaurant not found.");
            return;
    }
    if (SelectedRestaurant.orderQueue.Count == 0 )
        {
            Console.WriteLine("No pending orders for this restaurant.");
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
            }

            Console.WriteLine($"Customer: {customerName}");
            Console.WriteLine("Ordered Items:");
            order.DisplayOrderedFoodItems();
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
                refundStack.Push(order);
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

    void DisplayTotalOrderAmount() //Assuming that 30% commission is already insided the order total
{
        const double deliveryFee = 5.00;
        double grandTotal = 0.0;
        double refundTotal = 0.0;
        Console.WriteLine("Total Order Amount Summary");
        Console.WriteLine("==========================");
        foreach (Restaurant restaurant in restaurants)
        {
            double restaurantEarn = 0.0;
            double restaurantRefundTotal = 0.0;

            foreach (Order order in restaurant.orderQueue)
            {
                double foodTotal = order.orderTotal - deliveryFee;
                if (order.orderStatus == "Delivered")
                {
                    restaurantEarn += (foodTotal);
                }
                else if (order.orderStatus == "Rejected" || order.orderStatus == "Cancelled")
                {
                    restaurantRefundTotal += foodTotal;
                }
            }
            Console.WriteLine($"Restaurant: {restaurant.restaurantName}");
            Console.WriteLine($"  Total Delivered Orders Amount: ${restaurantEarn:F2}");
            Console.WriteLine($"  Total Refunds: ${restaurantRefundTotal:F2}");
            Console.WriteLine();

            grandTotal += restaurantEarn;
            refundTotal += restaurantRefundTotal;
        }

        Console.WriteLine("Overall Summary");
        Console.WriteLine("---------------");
        Console.WriteLine($"Total Order Amount: ${grandTotal:F2}");
        Console.WriteLine($"Total Refunds: ${refundTotal:F2}");
        Console.WriteLine($"Final Amount Gruberoo Earns: ${(grandTotal - refundTotal):F2}");

    }


