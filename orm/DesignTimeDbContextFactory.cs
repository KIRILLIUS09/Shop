using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using orm;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ShopDB;Username=postgres;Password=081979");
        return new AppDbContext(optionsBuilder.Options);
    }
}