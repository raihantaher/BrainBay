using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BrainBay.Core.Data;

public class BrainBayContextFactory : IDesignTimeDbContextFactory<BrainBayContext>
{
    public BrainBayContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BrainBayContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new BrainBayContext(optionsBuilder.Options);
    }
} 