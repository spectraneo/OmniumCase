namespace OmniumCaseApp.Models;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<OrderLine> OrderLines { get; set; } = [];
}