using System.Numerics;
using Microsoft.EntityFrameworkCore;
using OmniumCaseApp.Data;
using OmniumCaseApp.Models;
using OmniumCaseApp.Services;

using var db = new AppDbContext();
db.Database.Migrate();
DbSeeder.Seed(db);
IOrderService service = new OrderService(db);

// Task 2 : GetOrders 
var orders = service.GetOrders();
DisplayOrdersWithDetails(orders);

// Task 4: GetOrderByOrderId
var order = service.GetOrderByOrderId(19);
if (order != null)
{
    Console.WriteLine($"Task 4 - GetOrderByOrderId(19): {order.CustomerName}");
}
else
{
    Console.WriteLine("Task 4 - Order not found.");
}

// Task 5 - GetOrdersByCustomerId
var customerOrders = service.GetOrdersByCustomerId(101);
Console.WriteLine("Task 5 - Orders for CustomerId=101:");
DisplayOrdersWithDetails(customerOrders);

// Task 6 - GetOrdersByProductId
var productOrders = service.GetOrdersByProductId(9);
Console.WriteLine("Task 6 - Orders containing ProductId=9:");
DisplayOrdersWithDetails(productOrders);

// Task 7 - PosOrders
var posOrders = orders.OfType<PosOrder>().ToList();
Console.WriteLine($"Task 7 - PosOrders found: {posOrders.Count}");
DisplayOrdersWithDetails(posOrders.Cast<Order>().ToList());


// Task 8 - GetTopSellingProducts
var topProducts = service.GetTopSellingProducts();
Console.WriteLine("Task 8 - Top 5 selling products:");
foreach (var p in topProducts)
    Console.WriteLine($"  {p.ProductName}: {p.TotalQuantitySold} units, revenue {p.TotalRevenue}");


void DisplayOrdersWithDetails(List<Order> list)
{
    foreach (var o in list)
    {
        // Oppgave 3 – CalculateOrderTotal
        var calculated = service.CalculateOrderTotal(o);
        Console.WriteLine();
        Console.WriteLine($"OrderId={o.OrderId}, Customer={o.CustomerName}, Total={o.Total}, Calculated={calculated}");
        foreach (var l in o.OrderLines)
            Console.WriteLine($"  - {l.ProductName}, Qty={l.Quantity}, Price={l.Price}");
        Console.WriteLine();
    }
}