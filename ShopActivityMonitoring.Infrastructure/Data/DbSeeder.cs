using ShopActivityMonitoring.Application.Interfaces;
using ShopActivityMonitoring.Domain.Entities;

namespace ShopActivityMonitoring.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context, IPasswordHasher passwordHasher)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Analyst" },
                new Role { Name = "Operator" });
            context.SaveChanges();
        }

        if (!context.Users.Any())
        {
            var adminRole = context.Roles.First(r => r.Name == "Admin");
            var analystRole = context.Roles.First(r => r.Name == "Analyst");
            var operatorRole = context.Roles.First(r => r.Name == "Operator");

            context.Users.AddRange(
                new User { UserName = "admin", PasswordHash = passwordHasher.Hash("admin123"), RoleId = adminRole.Id },
                new User { UserName = "analyst", PasswordHash = passwordHasher.Hash("analyst123"), RoleId = analystRole.Id },
                new User { UserName = "operator", PasswordHash = passwordHasher.Hash("operator123"), RoleId = operatorRole.Id }
            );
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Ноутбук ASUS VivoBook", Category = "Электроника", Price = 54990m },
                new Product { Name = "Смартфон Samsung Galaxy A55", Category = "Электроника", Price = 32990m },
                new Product { Name = "Наушники Sony WH-1000XM5", Category = "Электроника", Price = 27990m },
                new Product { Name = "Футболка хлопковая", Category = "Одежда", Price = 1290m },
                new Product { Name = "Кроссовки Nike Air", Category = "Одежда", Price = 8990m },
                new Product { Name = "Кофеварка Delonghi", Category = "Дом", Price = 12990m },
                new Product { Name = "Набор посуды Tefal", Category = "Дом", Price = 6490m },
                new Product { Name = "Книга 'Чистый код'", Category = "Книги", Price = 1490m }
            );
            context.SaveChanges();
        }
    }
}