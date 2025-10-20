namespace Pixelz.Infrastructure.Persistence.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(PixelzWriteDbContext db)
    {
        // Nếu đã có dữ liệu thì bỏ qua (seed chỉ chạy 1 lần)
        if (await db.Customers.AnyAsync())
        {
            Console.WriteLine("Database already seeded. Skipping...");
            return;
        }

        // ----------------------------------
        // Common variables
        // ----------------------------------
        var systemUser = Guid.Empty.ToString();
        var utcNow = DateTime.UtcNow;
        var random = new Random();
        var faker = new Faker("en"); // English locale

        // ----------------------------------
        // 1️. Customers
        // ----------------------------------
        var customers = new Faker<Customer>("en")
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.FullName, f => f.Person.FullName)
            .RuleFor(c => c.CreatedBy, _ => systemUser)
            .RuleFor(c => c.CreatedAt, _ => utcNow)
            .Generate(50);

        await db.Customers.AddRangeAsync(customers);
        await db.SaveChangesAsync();

        // ----------------------------------
        // 2️. CustomerAddresses
        // ----------------------------------
        var customerAddresses = new List<CustomerAddress>();

        foreach (var c in customers)
        {
            var addrFaker = new Faker<CustomerAddress>("en")
                .RuleFor(a => a.CustomerId, c.Id)
                .RuleFor(a => a.FullName, _ => c.FullName)
                .RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber("+1##########"))
                .RuleFor(a => a.Line1, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.State, f => f.Address.State())
                .RuleFor(a => a.Country, f => f.Address.Country())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Type, f => f.PickRandom<AddressType>())
                .RuleFor(a => a.IsDefault, f => f.Random.Bool(0.8f))
                .RuleFor(a => a.CreatedBy, _ => systemUser)
                .RuleFor(a => a.CreatedAt, _ => utcNow);

            customerAddresses.AddRange(addrFaker.Generate(random.Next(1, 3)));
        }

        await db.CustomerAddresses.AddRangeAsync(customerAddresses);
        await db.SaveChangesAsync();

        // ----------------------------------
        // 3️. Orders + OrderItems + OrderAddresses
        // ----------------------------------
        var orders = new List<Order>();
        var orderItems = new List<OrderItem>();
        var orderAddresses = new List<OrderAddress>();

        var orderFaker = new Faker<Order>("en")
            .RuleFor(o => o.OrderNumber, f => $"ORD-{f.Random.Number(1000, 99999)}")
            .RuleFor(o => o.OrderName, f => $"Order for {f.Commerce.ProductName()}")
            .RuleFor(o => o.Status, _ => OrderStatus.PendingPayment)
            .RuleFor(o => o.TotalAmount, 0m)
            .RuleFor(o => o.CreatedBy, _ => systemUser)
            .RuleFor(o => o.CreatedAt, _ => utcNow);

        var itemFaker = new Faker<OrderItem>("en")
            .RuleFor(i => i.ImageFileName, f => f.System.FileName("jpg"))
            .RuleFor(i => i.RetouchType, f => f.PickRandom(
                "Background Removal", "Color Correction", "Shadow Enhancement", "Dust & Scratch Removal"))
            .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10, 80))
            .RuleFor(i => i.Quantity, f => f.Random.Number(1, 5))
            .RuleFor(i => i.CreatedBy, _ => systemUser)
            .RuleFor(i => i.CreatedAt, _ => utcNow);

        foreach (var c in customers)
        {
            var orderCount = random.Next(1, 4);
            var custOrders = orderFaker.Generate(orderCount);

            foreach (var o in custOrders)
            {
                o.CustomerId = c.Id;

                // Items
                var items = itemFaker.Generate(random.Next(2, 6));
                o.Items = items;
                o.TotalAmount = items.Sum(i => i.UnitPrice * i.Quantity);

                orders.Add(o);
                orderItems.AddRange(items);

                // OrderAddresses (Shipping & Billing)
                var custAddrs = customerAddresses.Where(a => a.CustomerId == c.Id).ToList();
                var shipping = faker.PickRandom(custAddrs);
                var billing = faker.PickRandom(custAddrs);

                orderAddresses.AddRange(new[]
                {
                    new OrderAddress
                    {
                        Order = o,
                        FullName = shipping.FullName,
                        PhoneNumber = shipping.PhoneNumber,
                        Line1 = shipping.Line1,
                        City = shipping.City,
                        State = shipping.State,
                        Country = shipping.Country,
                        PostalCode = shipping.PostalCode,
                        Type = AddressType.Shipping                
                    },
                    new OrderAddress
                    {
                        Order = o,
                        FullName = billing.FullName,
                        PhoneNumber = billing.PhoneNumber,
                        Line1 = billing.Line1,
                        City = billing.City,
                        State = billing.State,
                        Country = billing.Country,
                        PostalCode = billing.PostalCode,
                        Type = AddressType.Billing                        
                    }
                });
            }
        }

        await db.Orders.AddRangeAsync(orders);
        await db.OrderItems.AddRangeAsync(orderItems);
        await db.OrderAddresses.AddRangeAsync(orderAddresses);
        await db.SaveChangesAsync();

        // ----------------------------------
        // 4️. ayments (update Order.Status accordingly)
        // ----------------------------------
        var paymentFaker = new Faker<PaymentTransaction>("en")
            .RuleFor(p => p.Provider, f => f.PickRandom<PaymentProvider>())
            .RuleFor(p => p.TransactionId, _ => Guid.NewGuid().ToString())
            .RuleFor(p => p.Amount, f => f.Random.Decimal(50, 1000))
            .RuleFor(p => p.Status, f => f.PickRandom(Domain.Enums.PaymentStatus.Success, Domain.Enums.PaymentStatus.Pending, Domain.Enums.PaymentStatus.Failed))
            .RuleFor(p => p.CreatedBy, _ => systemUser)
            .RuleFor(p => p.CreatedAt, _ => utcNow);

        var payments = new List<PaymentTransaction>();

        foreach (var o in orders)
        {
            var payment = paymentFaker.Generate();
            payment.OrderId = o.Id;
            payments.Add(payment);

            // Update Order state based on payment result
            switch (payment.Status)
            {
                case Domain.Enums.PaymentStatus.Success:
                    o.Status = OrderStatus.Paid;
                    o.PaidAt = utcNow;
                    o.UpdatedAt = utcNow;
                    o.UpdatedBy = systemUser;
                    break;

                case Domain.Enums.PaymentStatus.Failed:
                    o.Status = OrderStatus.PaymentFailed;
                    o.UpdatedAt = utcNow;
                    o.UpdatedBy = systemUser;
                    break;

                default:
                    o.Status = OrderStatus.PendingPayment;
                    break;
            }
        }

        await db.PaymentTransactions.AddRangeAsync(payments);
        await db.SaveChangesAsync();

        // ----------------------------------
        // 5️. Invoices (for successful payments only)
        // ----------------------------------
        var invoices = new List<Invoice>();
        var invoiceFaker = new Faker<Invoice>("en")
            .RuleFor(i => i.InvoiceNumber, f => $"INV-{f.Random.Number(10000, 99999)}")
            .RuleFor(i => i.TotalAmount, f => f.Random.Decimal(50, 1000))
            .RuleFor(i => i.CreatedBy, _ => systemUser)
            .RuleFor(i => i.CreatedAt, _ => utcNow);

        foreach (var p in payments.Where(p => p.Status == Domain.Enums.PaymentStatus.Success))
        {
            var order = orders.First(o => o.Id == p.OrderId);
            var invoice = invoiceFaker.Generate();
            invoice.OrderId = order.Id;
            invoice.TotalAmount = order.TotalAmount;
            invoices.Add(invoice);
        }

        await db.Invoices.AddRangeAsync(invoices);
        await db.SaveChangesAsync();

        // ----------------------------------
        // Summary
        // ----------------------------------
        Console.WriteLine($"✅ Seed completed at {utcNow:yyyy-MM-dd HH:mm:ss} UTC");
        Console.WriteLine($" - {customers.Count} customers");
        Console.WriteLine($" - {customerAddresses.Count} customer_addresses");
        Console.WriteLine($" - {orders.Count} orders");
        Console.WriteLine($" - {orderItems.Count} order_items");
        Console.WriteLine($" - {orderAddresses.Count} order_addresses");
        Console.WriteLine($" - {payments.Count} payments");
        Console.WriteLine($" - {invoices.Count} invoices (for paid orders)");
    }
}
