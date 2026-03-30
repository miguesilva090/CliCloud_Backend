using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Infrastructure.Persistence.Contexts;

namespace CliCloud.Infrastructure.Persistence.Initializer
{
  public static class PaisDbInitializer
  {
    public static void Seed(ApplicationDbContext context)
    {
      if (context == null)
      {
        return;
      }

      Guid seedUserId = Guid.Parse("EEDEE592-CFBE-4382-B35D-7370E7886275");

      List<Pais> countriesToSeed =
      [
        new()
        {
          Id = Guid.Parse("76357508-B553-45E7-E209-08DE16FDAA36"),
          Codigo = "PT",
          Nome = "Portugal",
          Prefixo = "+351",
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:14:35.3675914", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse("2025-10-30T14:32:27.4496140", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E3FEF6B1-380C-4B90-E20A-08DE16FDAA36"),
          Codigo = "FR",
          Nome = "França",
          Prefixo = "+33",
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:15:57.6265903", CultureInfo.InvariantCulture),
          LastModifiedBy = null,
          LastModifiedOn = null,
        },
        new()
        {
          Id = Guid.Parse("616B3142-AD32-45C6-B2B1-08DE17AA8B00"),
          Codigo = "DE",
          Nome = "Alemanha",
          Prefixo = "+32",
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:50:37.7344999", CultureInfo.InvariantCulture),
          LastModifiedBy = null,
          LastModifiedOn = null,
        },
        new()
        {
          Id = Guid.Parse("FDAE982F-AF9C-4B7F-B2B2-08DE17AA8B00"),
          Codigo = "ES",
          Nome = "Espanha",
          Prefixo = "+34",
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:50:40.2250037", CultureInfo.InvariantCulture),
          LastModifiedBy = null,
          LastModifiedOn = null,
        },
        new()
        {
          Id = Guid.Parse("EC375232-540A-4340-4079-08DE22C7D576"),
          Codigo = "IT",
          Nome = "Itália",
          Prefixo = "+31",
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-13T15:18:00.8791077", CultureInfo.InvariantCulture),
          LastModifiedBy = null,
          LastModifiedOn = null,
        },
      ];

      HashSet<Guid> existingCountryIds = context.Paises.Select(p => p.Id).ToHashSet();
      List<Pais> countriesToInsert = countriesToSeed
        .Where(country => !existingCountryIds.Contains(country.Id))
        .ToList();

      if (countriesToInsert.Count == 0)
      {
        return;
      }

      context.Paises.AddRange(countriesToInsert);
      context.SaveChanges();
    }
  }
}
