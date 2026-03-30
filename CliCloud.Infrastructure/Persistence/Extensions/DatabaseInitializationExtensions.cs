#nullable enable
using CliCloud.Infrastructure.Persistence.Contexts;
using CliCloud.Infrastructure.Persistence.Initializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CliCloud.Infrastructure.Persistence.Extensions
{
  // apply pending migrations, seed database
  public static class DatabaseInitializationExtensions
  {
    private static readonly Action<ILogger, Exception?> LogStartingInitialization =
      LoggerMessage.Define(
        LogLevel.Information,
        new EventId(1, "StartingInitialization"),
        "[DATABASE INFO] Starting database initialization"
      );

    private static readonly Action<ILogger, Exception?> LogUsingConnectionString =
      LoggerMessage.Define(
        LogLevel.Information,
        new EventId(2, "UsingConnectionString"),
        "[DATABASE INFO] Using connection string 'DefaultConnection' for migrations"
      );

    private static readonly Action<ILogger, int, string, Exception?> LogFoundPendingMigrations =
      LoggerMessage.Define<int, string>(
        LogLevel.Information,
        new EventId(3, "FoundPendingMigrations"),
        "[DATABASE INFO] Found {MigrationCount} pending migration(s): {Migrations}"
      );

    private static readonly Action<ILogger, Exception?> LogMigrationsApplied = LoggerMessage.Define(
      LogLevel.Information,
      new EventId(4, "MigrationsApplied"),
      "[DATABASE INFO] Database migrations applied successfully"
    );

    private static readonly Action<ILogger, Exception?> LogNoPendingMigrations =
      LoggerMessage.Define(
        LogLevel.Information,
        new EventId(5, "NoPendingMigrations"),
        "[DATABASE INFO] No pending migrations - database is up to date"
      );

    private static readonly Action<ILogger, Exception?> LogSeedingDatabase = LoggerMessage.Define(
      LogLevel.Information,
      new EventId(6, "SeedingDatabase"),
      "[DATABASE INFO] Seeding database with initial data (admin and roles)"
    );

    private static readonly Action<ILogger, Exception?> LogSeedingCompleted = LoggerMessage.Define(
      LogLevel.Information,
      new EventId(7, "SeedingCompleted"),
      "[DATABASE INFO] Database seeding completed"
    );

    private static readonly Action<ILogger, Exception?> LogInitializationCompleted =
      LoggerMessage.Define(
        LogLevel.Information,
        new EventId(8, "InitializationCompleted"),
        "[DATABASE INFO] Database initialization completed successfully"
      );

    private static readonly Action<ILogger, Exception?> LogInitializationFailed =
      LoggerMessage.Define(
        LogLevel.Error,
        new EventId(9, "InitializationFailed"),
        "[DATABASE ERROR] Database initialization failed"
      );

    public static IServiceCollection AddAndMigrateDatabase<T>(
      this IServiceCollection services,
      IConfiguration _ // Reserved for future use
    )
      where T : ApplicationDbContext
    {
      // Build a temporary service provider to resolve the DbContext and logger
      using IServiceScope scopeBase = services.BuildServiceProvider().CreateScope();
      ILoggerFactory? loggerFactory = scopeBase.ServiceProvider.GetService<ILoggerFactory>();
      ILogger? logger = loggerFactory?.CreateLogger("DatabaseInitialization");
      T applicationDbContext = scopeBase.ServiceProvider.GetRequiredService<T>();

      try
      {
        // Prefixed messages to make database initialization logs easy to identify in stdout
        if (logger != null)
        {
          LogStartingInitialization(logger, null);
          LogUsingConnectionString(logger, null);
        }

        // Check for pending migrations
        List<string> pendingMigrations = applicationDbContext
          .Database.GetPendingMigrations()
          .ToList();

        if (pendingMigrations.Count > 0)
        {
          if (logger != null)
          {
            LogFoundPendingMigrations(
              logger,
              pendingMigrations.Count,
              string.Join(", ", pendingMigrations),
              null
            );
          }

          applicationDbContext.Database.Migrate(); // apply any pending migrations
          if (logger != null)
          {
            LogMigrationsApplied(logger, null);
          }
        }
        else
        {
          if (logger != null)
          {
            LogNoPendingMigrations(logger, null);
          }
        }

        // Seed initial data (admin user and roles)
        if (logger != null)
        {
          LogSeedingDatabase(logger, null);
        }
        DbInitializer.SeedAdminAndRoles(applicationDbContext);
        if (logger != null)
        {
          LogSeedingCompleted(logger, null);
          LogInitializationCompleted(logger, null);
        }
      }
      catch (Exception ex)
      {
        // Log the error and rethrow so the application fails fast and Program.cs can log it too
        if (logger != null)
        {
          LogInitializationFailed(logger, ex);
        }
        throw;
      }

      return services;
    }
  }
}
