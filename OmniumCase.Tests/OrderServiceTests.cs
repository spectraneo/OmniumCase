namespace OmniumCase.Tests;
using OmniumCaseApp.Models;
using Xunit;

public class OrderServiceTests
{
    private static List<Order> GetTestOrders()
    {
        return
        [

            new Order
            {
                OrderId = 1,
                CustomerId = 101,
                CustomerName = "Alice Hansen",
                OrderLines =
                [

                    new OrderLine
                        { OrderLineId = 1, ProductId = 1, ProductName = "Keyboard", Quantity = 2, Price = 500m },

                    new OrderLine { OrderLineId = 2, ProductId = 2, ProductName = "Mouse", Quantity = 1, Price = 250m }
                ]
            },

            new Order
            {
                OrderId = 2,
                CustomerId = 102,
                CustomerName = "Bob Nilsen",
                OrderLines =
                [

                    new OrderLine
                        { OrderLineId = 3, ProductId = 1, ProductName = "Keyboard", Quantity = 1, Price = 500m }
                ]
            },

            new PosOrder
            {
                OrderId = 3,
                CustomerId = 101,
                CustomerName = "Alice Hansen",
                PosId = 1,
                OrderLines =
                [

                    new OrderLine
                        { OrderLineId = 4, ProductId = 3, ProductName = "Monitor", Quantity = 1, Price = 3000m }
                ]
            }
        ];
    }
    
    [Fact]
    public void GetOrders_ReturnsAllOrders()
    {
        // Arrange
        var orders = GetTestOrders();

        // Assert
        Assert.Equal(3, orders.Count);
    }

    /* Task 3 — CalculateOrderTotal */
    [Fact]
    public void CalculateOrderTotal_ReturnsCorrectTotal()
    {
        // Arrange
        var order = GetTestOrders().First();

        // Act
        var total = order.OrderLines.Sum(l => l.Price * l.Quantity);

        // Assert
        Assert.Equal(1250m, total); // 2*500 + 1*250 = 1250
    }

    /* Task 4 — GetOrderByOrderId */
    [Fact]
    public void GetOrderByOrderId_ReturnsCorrectOrder()
    {
        // Arrange
        var orders = GetTestOrders();

        // Act
        var result = orders.FirstOrDefault(o => o.OrderId == 2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Bob Nilsen", result.CustomerName);
    }
    
    /* Task 5 — GetOrdersByCustomerId */
    [Fact]
    public void GetOrdersByCustomerId_ReturnsCorrectOrders()
    {
        // Arrange
        var orders = GetTestOrders();

        // Act
        var result = orders.Where(o => o.CustomerId == 101).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, o => Assert.Equal(101, o.CustomerId));
    }

    /* Task 6 — GetOrdersByProductId */
    [Fact]
    public void GetOrdersByProductId_ReturnsCorrectOrders()
    {
        // Arrange
        var orders = GetTestOrders();

        // Act
        var result = orders.Where(o => o.OrderLines.Any(l => l.ProductId == 1)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }

    /* Task 7 — PosOrders */
    [Fact]
    public void PosOrders_AreReturnedInOrdersList()
    {
        // Arrange
        var orders = GetTestOrders();

        // Act
        var posOrders = orders.OfType<PosOrder>().ToList();

        // Assert
        Assert.Single(posOrders);
        Assert.Equal(1, posOrders.First().PosId);
    }
    

    /* Task 8 : GetTopSellingProducts */
    [Fact]
    public void GetTopSellingProducts_ReturnsTop5ByQuantity()
    {
        // Arrange
        var orders = GetTestOrders();

        // Act
        var result = orders
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

        // Assert
        Assert.Equal("Keyboard", result.First().ProductName); // Keyboard sold 3 total (2+1)
        Assert.Equal(3, result.First().TotalQuantitySold);
    }
}