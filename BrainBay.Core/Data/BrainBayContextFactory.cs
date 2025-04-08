using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BrainBay.Core.Data;

public class BrainBayContextFactory : IDesignTimeDbContextFactory<BrainBayContext>
{
    public BrainBayContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BrainBayContext>();
        optionsBuilder.UseSqlServer("Server=rtdatabase.cfoay2c6o63s.eu-central-1.rds.amazonaws.com,1433;Database=BrainBay;User Id=admin;Password=brainBay123;TrustServerCertificate=True;");

        return new BrainBayContext(optionsBuilder.Options);
    }
} 