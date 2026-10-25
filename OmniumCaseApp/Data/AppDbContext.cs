using Microsoft.EntityFrameworkCore;
using OmniumCaseApp.Models;

namespace OmniumCaseApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<PosOrder> PosOrders => Set<PosOrder>();
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=omnium.db");
}