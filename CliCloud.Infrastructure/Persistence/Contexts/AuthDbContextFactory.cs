using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CliCloud.Infrastructure.Persistence.Contexts
{
  /// <summary>
  /// Design-time factory para migrations do AuthDbContext (Identity).
  /// </summary>
  public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
  {
    public AuthDbContext CreateDbContext(string[] args)
    {
      string environment =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"appsettings.{environment}.json", optional: false)
        .Build();

      string connectionString = configuration.GetConnectionString("DefaultConnection");
      if (string.IsNullOrEmpty(connectionString))
      {
        throw new InvalidOperationException(
          "Connection string 'DefaultConnection' not found for AuthDbContext."
        );
      }

      DbContextOptionsBuilder<AuthDbContext> optionsBuilder = new();
      _ = optionsBuilder.UseSqlServer(connectionString);
      return new AuthDbContext(optionsBuilder.Options);
    }
  }
}

