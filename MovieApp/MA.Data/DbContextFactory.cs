using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MA.Data;

public class DbContextFactory : IDesignTimeDbContextFactory<MADBContext>
{
    public MADBContext CreateDbContext(string[] args)
    {
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "../MovieApp");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<MADBContext>();
        string? connectionString = configuration.GetConnectionString("Default");

        optionsBuilder.UseNpgsql(connectionString);

        return new MADBContext(optionsBuilder.Options);
    }
    
}