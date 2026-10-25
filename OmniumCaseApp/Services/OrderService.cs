using OmniumCaseApp.Data;
using OmniumCaseApp.Models;
using Microsoft.EntityFrameworkCore;

namespace OmniumCaseApp.Services;

public class OrderService (AppDbContext db) : IOrderService
{
    public List<Order> GetOrders()
    {
        return db.Orders
            .Include(o => o.OrderLines)
            .ToList();
    }

    public decimal CalculateOrderTotal(Order order)
    {
        return order.OrderLines.Sum(ol => ol.Price * ol.Quantity);
    }

    public Order? GetOrderByOrderId(int orderId)
    {
        return db.Orders
            .Include(o => o.OrderLines)
            .FirstOrDefault(o => o.OrderId == orderId);
    }

    public List<Order> GetOrdersByCustomerId(int customerId)
    {
        return db.Orders
            .Include(o => o.OrderLines)
            .Where(o => o.CustomerId == customerId)
            .ToList();
    }

    public List<Order> GetOrdersByProductId(int productId)
    {
        return db.Orders
            .Include(o => o.OrderLines)
            .Where(o => o.OrderLines.Any(ol => ol.ProductId == productId))
            .ToList();
    }

    // get top 5 products by quantity sold, with total revenue per product
// ToList() first because SQLite stores decimal as TEXT — can't sort/sum in the database
    public List<ProductSalesSummary> GetTopSellingProducts()
    {
        return db.Orders
            .Include(o => o.OrderLines)
            .ToList()
            .SelectMany(o => o.OrderLines)
            .GroupBy(l => new { l.ProductId, l.ProductName })
            .Select(g => new ProductSalesSummary
            {
                ProductId         = g.Key.ProductId,
                ProductName       = g.Key.ProductName,
                TotalQuantitySold = g.Sum(l => l.Quantity),
                TotalRevenue      = g.Sum(l => l.Price * l.Quantity)
            })
            .OrderByDescending(p => p.TotalQuantitySold)
            .Take(5)
            .ToList();
    }
}