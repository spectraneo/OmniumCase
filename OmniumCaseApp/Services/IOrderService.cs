using OmniumCaseApp.Models;

namespace OmniumCaseApp.Services;

public interface IOrderService
{
    List<Order> GetOrders();
    
    decimal CalculateOrderTotal(Order order);
    
    Order? GetOrderByOrderId(int orderId);
    
    List<Order> GetOrdersByCustomerId(int customerId);
    
    List<Order> GetOrdersByProductId(int productId);
    
    List<ProductSalesSummary> GetTopSellingProducts(); 
}