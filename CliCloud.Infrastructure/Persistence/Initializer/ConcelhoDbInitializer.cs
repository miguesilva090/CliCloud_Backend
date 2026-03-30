using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Infrastructure.Persistence.Contexts;

namespace CliCloud.Infrastructure.Persistence.Initializer
{
  public static class ConcelhoDbInitializer
  {
    public static void Seed(ApplicationDbContext context)
    {
      if (context == null)
      {
        return;
      }

      Guid seedUserId = Guid.Parse("EEDEE592-CFBE-4382-B35D-7370E7886275");

     List<Concelho> municipalitiesToSeed =
      [
        new()
        {
          Id = Guid.Parse("14099DA2-4D81-4CD9-C6FB-08DE16FE6DFC"),
          Nome = "Barcelos",
          DistritoId = Guid.Parse("56873484-7333-4E4D-1EA9-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:18:35.6389275", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("526E84AB-214C-4FCC-C6FD-08DE16FE6DFC"),
          Nome = "Braga",
          DistritoId = Guid.Parse("56873484-7333-4E4D-1EA9-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:19:04.2917025", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("5236DFFC-043E-4272-C6FE-08DE16FE6DFC"),
          Nome = "Vila Verde",
          DistritoId = Guid.Parse("56873484-7333-4E4D-1EA9-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:19:18.7491381", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-10-29T15:19:37.5310138",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("182CE137-8C88-4F40-C6FF-08DE16FE6DFC"),
          Nome = "Esposende",
          DistritoId = Guid.Parse("56873484-7333-4E4D-1EA9-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:19:31.4148128", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("BC030406-B610-42E6-59C1-08DE17AB6051"),
          Nome = "Nanterre",
          DistritoId = Guid.Parse("4FD61A34-915F-44F5-64FC-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:56:35.6185643", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("0E671A71-1A7D-4D98-59C2-08DE17AB6051"),
          Nome = "Créteil",
          DistritoId = Guid.Parse("4FD61A34-915F-44F5-64FC-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:57:22.2262069", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-10-30T14:20:23.8093436",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("299B68B1-1C2E-4E5F-59C3-08DE17AB6051"),
          Nome = "Saint-Denis",
          DistritoId = Guid.Parse("4FD61A34-915F-44F5-64FC-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:57:24.1253031", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("69710515-1830-4639-06A8-08DE2387E9B4"),
          Nome = "Girona",
          DistritoId = Guid.Parse("B92B85F8-5977-48F6-B501-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:12:58.1925963", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("830690E1-56BE-412A-06A9-08DE2387E9B4"),
          Nome = "Salou",
          DistritoId = Guid.Parse("B92B85F8-5977-48F6-B501-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:13:07.2833482", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("A082C57A-2660-401B-06AA-08DE2387E9B4"),
          Nome = "Reus",
          DistritoId = Guid.Parse("B92B85F8-5977-48F6-B501-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:13:21.8044672", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("884E79B8-9FAC-4F59-06AB-08DE2387E9B4"),
          Nome = "Benidorm",
          DistritoId = Guid.Parse("1008B6F3-8BC1-4FB6-B500-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:14:00.5606789", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("A5F6C141-F2AD-4668-06AC-08DE2387E9B4"),
          Nome = "Alicante",
          DistritoId = Guid.Parse("1008B6F3-8BC1-4FB6-B500-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:14:08.6240003", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("DC979C3D-3BD1-4376-06AD-08DE2387E9B4"),
          Nome = "Albacete",
          DistritoId = Guid.Parse("1008B6F3-8BC1-4FB6-B500-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:14:33.9282539", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("9A8D0C25-05B0-4EC7-06AE-08DE2387E9B4"),
          Nome = "Getafe",
          DistritoId = Guid.Parse("75E83D69-6076-4BFD-B4FF-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:14:59.1952012", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("57C86220-56B4-4688-06AF-08DE2387E9B4"),
          Nome = "Guadalajara",
          DistritoId = Guid.Parse("75E83D69-6076-4BFD-B4FF-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:15:20.2002907", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("98C55707-7DCE-4C24-06B0-08DE2387E9B4"),
          Nome = "Toledo",
          DistritoId = Guid.Parse("75E83D69-6076-4BFD-B4FF-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:15:40.0724017", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("1385AAF1-4688-4F1B-06B1-08DE2387E9B4"),
          Nome = "Mitte",
          DistritoId = Guid.Parse("B9D26AC2-D146-437F-B4FE-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:19:03.7858994", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4F25AA79-B803-4A2A-06B2-08DE2387E9B4"),
          Nome = "Gatow",
          DistritoId = Guid.Parse("B9D26AC2-D146-437F-B4FE-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:19:14.7370026", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("56C72180-CB28-4FE0-06B3-08DE2387E9B4"),
          Nome = "Rudow",
          DistritoId = Guid.Parse("B9D26AC2-D146-437F-B4FE-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:19:25.5085515", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("B183D7FC-EC4D-4702-06B4-08DE2387E9B4"),
          Nome = "Heerdt",
          DistritoId = Guid.Parse("CEC42C1F-A536-4420-B4FD-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:19:46.8550485", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("192F8BCB-8998-4E22-06B5-08DE2387E9B4"),
          Nome = "Eller",
          DistritoId = Guid.Parse("CEC42C1F-A536-4420-B4FD-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:19:56.5424300", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("BA4CB3DC-2C2B-4F02-06B6-08DE2387E9B4"),
          Nome = "Rath",
          DistritoId = Guid.Parse("CEC42C1F-A536-4420-B4FD-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:20:04.7277033", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("5042659E-EC5E-40DA-06B7-08DE2387E9B4"),
          Nome = "Laim",
          DistritoId = Guid.Parse("BEC2A26E-7F14-4BF2-B4FC-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:20:26.6116219", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("ADCF872D-F3ED-4B66-06B8-08DE2387E9B4"),
          Nome = "Riem",
          DistritoId = Guid.Parse("BEC2A26E-7F14-4BF2-B4FC-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:20:35.9752854", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("9EE35E85-52B5-453D-06B9-08DE2387E9B4"),
          Nome = "Solln",
          DistritoId = Guid.Parse("BEC2A26E-7F14-4BF2-B4FC-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:20:51.7926428", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("729006EA-14BB-4110-06BA-08DE2387E9B4"),
          Nome = "Barra",
          DistritoId = Guid.Parse("6F797E30-F0F0-4B0C-B4FB-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:21:34.2486977", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E65E5E58-DBAF-4EAA-06BB-08DE2387E9B4"),
          Nome = "Agnano",
          DistritoId = Guid.Parse("6F797E30-F0F0-4B0C-B4FB-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:21:45.3982244", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4885A0A7-6D49-4818-06BC-08DE2387E9B4"),
          Nome = "Traiano",
          DistritoId = Guid.Parse("6F797E30-F0F0-4B0C-B4FB-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:22:05.5678414", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("B202D26F-680D-46E0-06BD-08DE2387E9B4"),
          Nome = "Torrino",
          DistritoId = Guid.Parse("235239AC-5103-41CD-B4FA-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:22:35.0925742", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("AC97CEBA-BE51-4A68-06BE-08DE2387E9B4"),
          Nome = "Garbatella",
          DistritoId = Guid.Parse("235239AC-5103-41CD-B4FA-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:23:11.1278985", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("13573EF8-BFBE-4E86-06BF-08DE2387E9B4"),
          Nome = "Parioli",
          DistritoId = Guid.Parse("235239AC-5103-41CD-B4FA-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:23:26.4656401", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("D5CA07F0-810A-405F-06C0-08DE2387E9B4"),
          Nome = "Navigli",
          DistritoId = Guid.Parse("9E156E78-A4E2-41A0-B4F9-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:23:56.9773257", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("1B80452E-EFA9-4E5A-06C1-08DE2387E9B4"),
          Nome = "Lampugnano",
          DistritoId = Guid.Parse("9E156E78-A4E2-41A0-B4F9-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:24:15.9832947", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("DD20CD99-2FFB-4354-06C2-08DE2387E9B4"),
          Nome = "Bicocca",
          DistritoId = Guid.Parse("9E156E78-A4E2-41A0-B4F9-08DE23872D18"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:24:34.2398901", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("06B3A645-E94C-470F-06C3-08DE2387E9B4"),
          Nome = "Croix-Rousse",
          DistritoId = Guid.Parse("3C974DEF-2285-45C5-64FE-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:25:16.5258963", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("C84B0936-1089-4F4B-06C4-08DE2387E9B4"),
          Nome = "Écully",
          DistritoId = Guid.Parse("3C974DEF-2285-45C5-64FE-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:25:48.2479291", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("5DFBC274-2D7F-4CB1-06C5-08DE2387E9B4"),
          Nome = "Saint-Fons",
          DistritoId = Guid.Parse("3C974DEF-2285-45C5-64FE-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:26:08.9507483", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("013A03B1-809D-47A5-06C6-08DE2387E9B4"),
          Nome = "Saména",
          DistritoId = Guid.Parse("83492CCB-5C8C-4ACC-64FD-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:26:48.7201595", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("978D17B0-137C-4287-06C7-08DE2387E9B4"),
          Nome = "Allauch",
          DistritoId = Guid.Parse("83492CCB-5C8C-4ACC-64FD-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:27:08.4634474", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-11-14T14:27:29.7383784",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("81A94386-7FFA-4E38-06C8-08DE2387E9B4"),
          Nome = "Cassis",
          DistritoId = Guid.Parse("83492CCB-5C8C-4ACC-64FD-08DE17AAB7DD"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:27:42.8301658", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("EB4F2F7C-B6C5-4AF7-06C9-08DE2387E9B4"),
          Nome = "Vila Franca de Xira",
          DistritoId = Guid.Parse("81930A74-79FD-4B4C-1EAE-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:28:10.0065034", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("253ECB6F-E6E0-474C-06CA-08DE2387E9B4"),
          Nome = "Oeiras",
          DistritoId = Guid.Parse("81930A74-79FD-4B4C-1EAE-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:28:21.5000138", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("84A1380D-28DA-4130-06CB-08DE2387E9B4"),
          Nome = "Loures",
          DistritoId = Guid.Parse("81930A74-79FD-4B4C-1EAE-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:29:23.9301965", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E2D09C0B-9B66-45ED-06CC-08DE2387E9B4"),
          Nome = "Rio Tinto",
          DistritoId = Guid.Parse("43DE7D9C-D4B8-4D34-1EAD-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:30:18.6730862", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("37DEF8EC-8610-4F4E-06CD-08DE2387E9B4"),
          Nome = "Matosinhos",
          DistritoId = Guid.Parse("43DE7D9C-D4B8-4D34-1EAD-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:30:33.9808410", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("78A39495-400E-46F4-06CE-08DE2387E9B4"),
          Nome = "Vila Nova de Gaia",
          DistritoId = Guid.Parse("43DE7D9C-D4B8-4D34-1EAD-08DE16FE1A2C"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:30:50.1056195", CultureInfo.InvariantCulture),
        },
      ];

      HashSet<Guid> existingMunicipalityIds = context.Concelhos.Select(c => c.Id).ToHashSet();
      List<Concelho> municipalitiesToInsert = municipalitiesToSeed
        .Where(concelho => !existingMunicipalityIds.Contains(concelho.Id))
        .ToList();

      if (municipalitiesToInsert.Count == 0)
      {
        return;
      }

      context.Concelhos.AddRange(municipalitiesToInsert);
      context.SaveChanges();
    }
  }
}
