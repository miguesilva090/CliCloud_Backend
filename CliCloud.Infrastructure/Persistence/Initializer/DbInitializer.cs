using CliCloud.Infrastructure.Persistence.Contexts;

namespace CliCloud.Infrastructure.Persistence.Initializer
{
  public static class DbInitializer
  {
    public static void SeedAdminAndRoles(ApplicationDbContext context)
    {
      PaisDbInitializer.Seed(context);
      DistritoDbInitializer.Seed(context);
      ConcelhoDbInitializer.Seed(context);
      FreguesiaDbInitializer.Seed(context);
    }
  }
}
