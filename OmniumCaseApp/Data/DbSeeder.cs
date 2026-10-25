using OmniumCaseApp.Models;

namespace OmniumCaseApp.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Orders.Any())
            return;

        var customers = new[]
        {
            (Id: 101, Name: "Alice Hansen"),
            (Id: 102, Name: "Bob Nilsen"),
            (Id: 103, Name: "Carla Berg"),
            (Id: 104, Name: "David Lund"),
            (Id: 105, Name: "Eva Storm"),
            (Id: 106, Name: "Fredrik Dahl"),
            (Id: 107, Name: "Gro Holm"),
            (Id: 108, Name: "Henrik Moe"),
            (Id: 109, Name: "Ida Viken"),
            (Id: 110, Name: "Jonas Brekke"),
        };

        var productNames = new[]
        {
            "Keyboard", "Mouse", "Monitor", "USB Hub", "Webcam",
            "Headset", "Laptop Stand", "HDMI Cable", "USB-C Dock", "Mousepad",
            "SSD 256GB", "SSD 512GB", "RAM 8GB", "RAM 16GB", "CPU Cooler",
            "Case Fan", "Power Strip", "Surge Protector", "Cable Organizer", "Monitor Arm",
            "Desk Mat", "LED Strip", "Smart Plug", "Network Switch", "Ethernet Cable",
            "Wifi Adapter", "Bluetooth Adapter", "Speaker", "Microphone", "Webcam Pro",
            "Drawing Tablet", "Numpad", "Stream Deck", "Capture Card", "Green Screen",
            "Ring Light", "Phone Stand", "Tablet Stand", "VR Headset", "Controller",
            "Gaming Chair", "Desk Riser", "Footrest", "Wrist Rest", "Laptop Bag",
            "Backpack", "Cable Sleeve", "Label Maker", "Shredder", "Laminator",
            "Whiteboard", "Corkboard", "Desk Organizer", "Pen Holder", "Sticky Notes",
            "Stapler", "Hole Punch", "Scissors", "Tape Dispenser", "Ruler",
            "Notebook", "Planner", "Highlighter Set", "Pen Set", "Marker Set",
            "Printer", "Scanner", "Ink Cartridge", "Paper Ream", "Label Tape",
            "External HDD", "Flash Drive", "SD Card", "Card Reader", "OTG Adapter",
            "Phone Charger", "Wireless Charger", "Power Bank", "Battery Pack", "UPS",
            "Projector", "Projector Screen", "Laser Pointer", "Presentation Remote", "HDMI Switch",
            "KVM Switch", "Display Port Cable", "VGA Adapter", "DVI Cable", "Thunderbolt Cable",
            "USB Fan", "USB Light", "USB Heater", "USB Cup Warmer", "Laptop Cooler",
            "Privacy Screen", "Screen Cleaner", "Compressed Air", "Thermal Paste", "Anti-static Wrist Strap",
        };

        var rng = new Random(1);

        var products = productNames.Select((name, i) => (
            Id: i + 1,
            Name: name,
            Price: Math.Round((decimal)(rng.NextDouble() * 2000 + 50), 2)
        )).ToArray();

        var lineId = 1;
        var orders = new List<Order>();

        // seed 20 regular orders
        for (var i = 1; i <= 20; i++)
        {
            var customer = customers[rng.Next(customers.Length)];
            var numLines = rng.Next(1, 5);
            var lines = new List<OrderLine>();

            for (var j = 0; j < numLines; j++)
            {
                var product = products[rng.Next(products.Length)];
                lines.Add(new OrderLine
                {
                    OrderLineId = lineId++,
                    ProductId   = product.Id,
                    ProductName = product.Name,
                    Quantity    = rng.Next(1, 6),
                    Price       = product.Price
                });
            }

            var order = new Order
            {
                OrderId      = i,
                CustomerId   = customer.Id,
                CustomerName = customer.Name,
                OrderLines   = lines
            };
            order.Total = order.OrderLines.Sum(l => l.Price * l.Quantity);
            orders.Add(order);
        }

        // seed 10 PosOrders, one per customer
        for (var i = 0; i < customers.Length; i++)
        {
            var customer = customers[i];
            var product = products[rng.Next(products.Length)];

            var posOrder = new PosOrder
            {
                OrderId      = 21 + i,
                CustomerId   = customer.Id,
                CustomerName = customer.Name,
                PosId        = i + 1,
                OrderLines   =
                [

                    new OrderLine
                    {
                        OrderLineId = lineId++,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = rng.Next(1, 6),
                        Price = product.Price
                    }
                ]
            };
            posOrder.Total = posOrder.OrderLines.Sum(l => l.Price * l.Quantity);
            orders.Add(posOrder);
        }

        db.Orders.AddRange(orders);
        db.SaveChanges();
        Console.WriteLine($"Seeded {orders.Count} orders ({orders.OfType<PosOrder>().Count()} PosOrders).");
    }
}