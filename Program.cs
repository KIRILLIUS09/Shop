using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using orm.Models;
using System;

namespace orm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 1. Настройка сервисов
            var services = new ServiceCollection();

            // 2. Регистрация контекста БД
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=ShopDB;Username=postgres;Password=081979"));

            // 3. Создание провайдера сервисов
            var serviceProvider = services.BuildServiceProvider();

            // 4. Применение миграций
            using (var scope = serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    db.Database.Migrate();
                    Console.WriteLine("Миграции успешно применены!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при применении миграций: {ex.Message}");
                }
            }
        }
    }
}