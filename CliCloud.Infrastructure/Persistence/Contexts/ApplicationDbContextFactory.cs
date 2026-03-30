using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

// neccessary for EF migration designer work when running migrations on startup
namespace CliCloud.Infrastructure.Persistence.Contexts
{
  public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    public ApplicationDbContext CreateDbContext(string[] args)
    {
      // Build the configuration by reading from appsettings files (requires Microsoft.Extensions.Configuration.Json Nuget Package)
      // For design-time operations (migrations), default to Development if environment is not set
      // This is more developer-friendly since migrations are typically run during development
      string environment =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"appsettings.{environment}.json", optional: false)
        .Build();

      // Retrieve the connection string from the configuration
      string connectionString = configuration.GetConnectionString("DefaultConnection");

      if (string.IsNullOrEmpty(connectionString))
      {
        throw new InvalidOperationException(
          $"Connection string 'DefaultConnection' not found. "
            + $"Make sure ASPNETCORE_ENVIRONMENT is set correctly and the corresponding appsettings file exists."
        );
      }

      DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
      _ = optionsBuilder.UseSqlServer(connectionString);
      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}
