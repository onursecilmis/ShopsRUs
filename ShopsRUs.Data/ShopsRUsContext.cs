using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShopsRUs.Core.Configurations;
using ShopsRUs.Core.Data;
using ShopsRUs.Data.Domain;
using ShopsRUs.Data.Mapping;

namespace ShopsRUs.Data;

public class ShopsRUsContext : DbContext
{
    private readonly string? _connectionString;

    public ShopsRUsContext(IOptions<AppSettings> appSettings)
    {
        _connectionString = appSettings.Value?.ConnectionStrings?.DefaultConnection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging().UseSqlServer(_connectionString!);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomersMap());
        modelBuilder.ApplyConfiguration(new DiscountsMap());
        modelBuilder.ApplyConfiguration(new InvoicesMap());
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var domainAssembly = typeof(Domain.Customers).Assembly;
        var entityTypes = domainAssembly.GetTypes().Where(x => x.BaseType == typeof(BaseEntity)).ToList();
        foreach (var type in entityTypes.Where(type => modelBuilder.Model.FindEntityType(type) == null))
        {
            modelBuilder.Model.AddEntityType(type);
        }
    }
}