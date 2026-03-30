using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Infrastructure.Persistence.Contexts;

namespace CliCloud.Infrastructure.Persistence.Initializer
{
  public static class DistritoDbInitializer
  {
    public static void Seed(ApplicationDbContext context)
    {
      if (context == null)
      {
        return;
      }

      Guid seedUserId = Guid.Parse("EEDEE592-CFBE-4382-B35D-7370E7886275");
      Guid portugalId = Guid.Parse("76357508-B553-45E7-E209-08DE16FDAA36");

      List<Distrito> districtsToSeed =
      [
        new()
        {
          Id = Guid.Parse("56873484-7333-4E4D-1EA9-08DE16FE1A2C"),
          Nome = "Braga",
          PaisId = Guid.Parse("76357508-B553-45E7-E209-08DE16FDAA36"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:16:14.9946092", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("43DE7D9C-D4B8-4D34-1EAD-08DE16FE1A2C"),
          Nome = "Porto",
          PaisId = Guid.Parse("76357508-B553-45E7-E209-08DE16FDAA36"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:17:41.0909541", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("81930A74-79FD-4B4C-1EAE-08DE16FE1A2C"),
          Nome = "Lisboa",
          PaisId = Guid.Parse("76357508-B553-45E7-E209-08DE16FDAA36"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:17:47.9238652", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4FD61A34-915F-44F5-64FC-08DE17AAB7DD"),
          Nome = "Paris",
          PaisId = Guid.Parse("E3FEF6B1-380C-4B90-E20A-08DE16FDAA36"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:51:52.9986289", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-10-30T14:24:02.0899991",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("83492CCB-5C8C-4ACC-64FD-08DE17AAB7DD"),
          Nome = "Marselha",
          PaisId = Guid.Parse("E3FEF6B1-380C-4B90-E20A-08DE16FDAA36"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:51:54.7538879", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-10-30T12:19:29.9890914",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("3C974DEF-2285-45C5-64FE-08DE17AAB7DD"),
          Nome = "Lyon",
          PaisId = Guid.Parse("E3FEF6B1-380C-4B90-E20A-08DE16FDAA36"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:51:57.7010656", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-10-30T12:19:35.1791098",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("9E156E78-A4E2-41A0-B4F9-08DE23872D18"),
          Nome = "Milão",
          PaisId = Guid.Parse("EC375232-540A-4340-4079-08DE22C7D576"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:07:41.8043292", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("235239AC-5103-41CD-B4FA-08DE23872D18"),
          Nome = "Roma",
          PaisId = Guid.Parse("EC375232-540A-4340-4079-08DE22C7D576"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:07:51.6110086", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("6F797E30-F0F0-4B0C-B4FB-08DE23872D18"),
          Nome = "Nápoles",
          PaisId = Guid.Parse("EC375232-540A-4340-4079-08DE22C7D576"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:08:05.2500140", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-11-14T14:21:01.4296627",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("BEC2A26E-7F14-4BF2-B4FC-08DE23872D18"),
          Nome = "Munique",
          PaisId = Guid.Parse("616B3142-AD32-45C6-B2B1-08DE17AA8B00"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:08:20.8913578", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("CEC42C1F-A536-4420-B4FD-08DE23872D18"),
          Nome = "Dusseldorf",
          PaisId = Guid.Parse("616B3142-AD32-45C6-B2B1-08DE17AA8B00"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:08:32.2246756", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("B9D26AC2-D146-437F-B4FE-08DE23872D18"),
          Nome = "Berlim",
          PaisId = Guid.Parse("616B3142-AD32-45C6-B2B1-08DE17AA8B00"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:08:40.2547830", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("75E83D69-6076-4BFD-B4FF-08DE23872D18"),
          Nome = "Madrid",
          PaisId = Guid.Parse("FDAE982F-AF9C-4B7F-B2B2-08DE17AA8B00"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:08:59.1125210", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("1008B6F3-8BC1-4FB6-B500-08DE23872D18"),
          Nome = "Valência",
          PaisId = Guid.Parse("FDAE982F-AF9C-4B7F-B2B2-08DE17AA8B00"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:09:08.3227415", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("B92B85F8-5977-48F6-B501-08DE23872D18"),
          Nome = "Barcelona",
          PaisId = Guid.Parse("FDAE982F-AF9C-4B7F-B2B2-08DE17AA8B00"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:09:15.8805797", CultureInfo.InvariantCulture),
        },
      ];

      HashSet<Guid> existingDistrictIds = context.Distritos.Select(d => d.Id).ToHashSet();
      List<Distrito> districtsToInsert = districtsToSeed
        .Where(district => !existingDistrictIds.Contains(district.Id))
        .ToList();

      if (districtsToInsert.Count == 0)
      {
        return;
      }

      context.Distritos.AddRange(districtsToInsert);
      _ = context.SaveChanges();
    }
  }
}
