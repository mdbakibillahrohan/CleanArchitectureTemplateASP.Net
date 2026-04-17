using Domain.Products;
using Domain.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seed;

/// <summary>
/// Seeds initial data into the database.
/// Demonstrates how to seed data with strongly typed IDs and Value Objects.
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureCreatedAsync();

        if (!await context.Users.AnyAsync())
        {
            var admin = User.Create(
                "Admin",
                "User",
                "admin@example.com",
                BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role.Admin);

            var user = User.Create(
                "Regular",
                "User",
                "user@example.com",
                BCrypt.Net.BCrypt.HashPassword("user123"),
                Role.User);

            // Clear domain events since we don't want to dispatch during seeding
            admin.ClearDomainEvents();
            user.ClearDomainEvents();

            context.Users.AddRange(admin, user);
        }

        if (!await context.Products.AnyAsync())
        {
            var products = new[]
            {
                Product.Create("Laptop Pro 16", "High-performance laptop with 16-inch display", 1299.99m, "USD", 50),
                Product.Create("Wireless Mouse", "Ergonomic wireless mouse with USB-C receiver", 29.99m, "USD", 200),
                Product.Create("Mechanical Keyboard", "RGB mechanical keyboard with Cherry MX switches", 89.99m, "USD", 150),
                Product.Create("USB-C Hub", "7-in-1 USB-C hub with HDMI and ethernet", 49.99m, "USD", 100),
                Product.Create("Monitor 27\"", "4K IPS monitor with HDR support", 449.99m, "USD", 30),
            };

            foreach (var product in products)
                product.ClearDomainEvents();

            context.Products.AddRange(products);
        }

        await context.SaveChangesAsync();
    }
}
