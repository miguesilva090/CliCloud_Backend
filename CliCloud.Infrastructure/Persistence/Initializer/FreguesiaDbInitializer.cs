using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Infrastructure.Persistence.Contexts;

namespace CliCloud.Infrastructure.Persistence.Initializer
{
  public static class FreguesiaDbInitializer
  {
    public static void Seed(ApplicationDbContext context)
    {
      if (context == null)
      {
        return;
      }

      Guid seedUserId = Guid.Parse("EEDEE592-CFBE-4382-B35D-7370E7886275");

      List<Freguesia> parishesToSeed =
      [
        new()
        {
          Id = Guid.Parse("EC331D46-CF59-4E15-7625-08DE16FEB1C4"),
          Nome = "Barcelinhos",
          ConcelhoId = Guid.Parse("14099DA2-4D81-4CD9-C6FB-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:20:29.3274844", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("0EAA802B-7F9B-468C-7626-08DE16FEB1C4"),
          Nome = "Areias de Vilar e Encourados",
          ConcelhoId = Guid.Parse("14099DA2-4D81-4CD9-C6FB-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:21:17.9363349", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-11-14T11:09:18.1507665",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("6122A12F-D729-43C3-7627-08DE16FEB1C4"),
          Nome = "Pousa",
          ConcelhoId = Guid.Parse("14099DA2-4D81-4CD9-C6FB-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:21:51.2274432", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("0F551AC3-948E-4702-7628-08DE16FEB1C4"),
          Nome = "Alvelos",
          ConcelhoId = Guid.Parse("14099DA2-4D81-4CD9-C6FB-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:22:00.0711133", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("7993B262-2A53-417D-7629-08DE16FEB1C4"),
          Nome = "Gamil",
          ConcelhoId = Guid.Parse("14099DA2-4D81-4CD9-C6FB-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-29T15:22:09.4537122", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("0BF6703D-0316-4001-6645-08DE17ABC8C5"),
          Nome = "Vitry-sur-Seine",
          ConcelhoId = Guid.Parse("0E671A71-1A7D-4D98-59C2-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:59:30.8559925", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("11716730-C77B-4835-6646-08DE17ABC8C5"),
          Nome = "Orly",
          ConcelhoId = Guid.Parse("0E671A71-1A7D-4D98-59C2-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-10-30T11:59:53.7603444", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-10-30T14:31:38.2128784",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("67710351-7A7E-4076-1716-08DE238C96A9"),
          Nome = "Ramalde",
          ConcelhoId = Guid.Parse("37DEF8EC-8610-4F4E-06CD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:46:26.3390176", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("D5AC681D-C3A3-416F-1717-08DE238C96A9"),
          Nome = "Aldoar",
          ConcelhoId = Guid.Parse("37DEF8EC-8610-4F4E-06CD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:46:36.9961058", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("16A9D34F-AF6C-46BF-1718-08DE238C96A9"),
          Nome = "Guifões",
          ConcelhoId = Guid.Parse("37DEF8EC-8610-4F4E-06CD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:47:16.6297965", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("7CFA8473-5CEC-4E96-1719-08DE238C96A9"),
          Nome = "Perlinhas",
          ConcelhoId = Guid.Parse("E2D09C0B-9B66-45ED-06CC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:47:44.1520178", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("8D3D9F80-8D43-465C-171A-08DE238C96A9"),
          Nome = "Soutelo",
          ConcelhoId = Guid.Parse("E2D09C0B-9B66-45ED-06CC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:47:56.2288412", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E371E9D8-51D7-43B2-171B-08DE238C96A9"),
          Nome = "Tardinhade",
          ConcelhoId = Guid.Parse("E2D09C0B-9B66-45ED-06CC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:48:31.8201676", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("F56EBDE0-B0EF-4C60-171C-08DE238C96A9"),
          Nome = "Lavandeira",
          ConcelhoId = Guid.Parse("78A39495-400E-46F4-06CE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:49:41.8770480", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("EE397A66-BB90-4A78-171D-08DE238C96A9"),
          Nome = "Raza de Cima",
          ConcelhoId = Guid.Parse("78A39495-400E-46F4-06CE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:50:01.6093061", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("37DE973C-0D4A-45E2-171E-08DE238C96A9"),
          Nome = "Bandeira",
          ConcelhoId = Guid.Parse("78A39495-400E-46F4-06CE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:50:09.8765867", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("84C133A9-9A11-49E3-171F-08DE238C96A9"),
          Nome = "Espargal",
          ConcelhoId = Guid.Parse("253ECB6F-E6E0-474C-06CA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:50:41.2821741", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("CEC25B14-3B21-458A-1720-08DE238C96A9"),
          Nome = "Carcavelos",
          ConcelhoId = Guid.Parse("253ECB6F-E6E0-474C-06CA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:50:56.8459322", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("68E3DE07-5211-4E58-1721-08DE238C96A9"),
          Nome = "Arneiro",
          ConcelhoId = Guid.Parse("253ECB6F-E6E0-474C-06CA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:51:19.0282593", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E2FA91E6-5F4B-491C-1722-08DE238C96A9"),
          Nome = "Palhais",
          ConcelhoId = Guid.Parse("84A1380D-28DA-4130-06CB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:51:49.6952362", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("DA45ECD7-937C-4324-1723-08DE238C96A9"),
          Nome = "A-Dos-Calvos",
          ConcelhoId = Guid.Parse("84A1380D-28DA-4130-06CB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:52:04.7642953", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4D4D4060-8972-4C4E-1724-08DE238C96A9"),
          Nome = "Fonte Santa",
          ConcelhoId = Guid.Parse("84A1380D-28DA-4130-06CB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:52:18.8234797", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("AAEE2A91-AEB7-4751-1725-08DE238C96A9"),
          Nome = "Murteira",
          ConcelhoId = Guid.Parse("84A1380D-28DA-4130-06CB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:53:21.7313164", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("48840B95-8B49-4BDC-1726-08DE238C96A9"),
          Nome = "Badalinho",
          ConcelhoId = Guid.Parse("EB4F2F7C-B6C5-4AF7-06C9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:53:46.1001278", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("7AD42551-ED83-48FB-1727-08DE238C96A9"),
          Nome = "Casal da Coxa",
          ConcelhoId = Guid.Parse("EB4F2F7C-B6C5-4AF7-06C9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:53:55.2058434", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("55C4B177-1B8F-472B-1728-08DE238C96A9"),
          Nome = "Povos",
          ConcelhoId = Guid.Parse("EB4F2F7C-B6C5-4AF7-06C9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:54:08.7725787", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E1FE8D43-D86E-4186-1729-08DE238C96A9"),
          Nome = "Marinhas",
          ConcelhoId = Guid.Parse("182CE137-8C88-4F40-C6FF-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:54:53.1674809", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("58F09CD3-3010-4BDD-172A-08DE238C96A9"),
          Nome = "Apúlia",
          ConcelhoId = Guid.Parse("182CE137-8C88-4F40-C6FF-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:55:04.6209317", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("3E5F6833-B0DE-4765-172B-08DE238C96A9"),
          Nome = "Gandra",
          ConcelhoId = Guid.Parse("182CE137-8C88-4F40-C6FF-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:55:52.0011837", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("97F2832F-9B76-4AAF-172C-08DE238C96A9"),
          Nome = "Oleiros",
          ConcelhoId = Guid.Parse("5236DFFC-043E-4272-C6FE-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:56:01.0692596", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("919569AD-A6D0-416A-172D-08DE238C96A9"),
          Nome = "Turiz",
          ConcelhoId = Guid.Parse("5236DFFC-043E-4272-C6FE-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:56:18.6641694", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("AE27BC1B-8EDF-4929-172E-08DE238C96A9"),
          Nome = "Rendufe",
          ConcelhoId = Guid.Parse("5236DFFC-043E-4272-C6FE-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:56:37.2111249", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("66FFE3FD-A077-421B-172F-08DE238C96A9"),
          Nome = "Adaúfe",
          ConcelhoId = Guid.Parse("526E84AB-214C-4FCC-C6FD-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:58:15.5219192", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("9142726A-356C-4BF2-1730-08DE238C96A9"),
          Nome = "Sequeira",
          ConcelhoId = Guid.Parse("526E84AB-214C-4FCC-C6FD-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:58:23.7970884", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4B5CB779-05E9-400C-1731-08DE238C96A9"),
          Nome = "Pedralva",
          ConcelhoId = Guid.Parse("526E84AB-214C-4FCC-C6FD-08DE16FE6DFC"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:58:40.3564660", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4EE941B2-44F2-4D51-1732-08DE238C96A9"),
          Nome = "Capellans",
          ConcelhoId = Guid.Parse("830690E1-56BE-412A-06A9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:59:26.4764191", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("FDD4A124-C621-4624-1733-08DE238C96A9"),
          Nome = "Vilafortuny",
          ConcelhoId = Guid.Parse("830690E1-56BE-412A-06A9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T14:59:44.5573749", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E77A9FB0-FC65-4974-1734-08DE238C96A9"),
          Nome = "Platja de la Pineda",
          ConcelhoId = Guid.Parse("830690E1-56BE-412A-06A9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:00:19.1931051", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("68ED2AA4-1094-48E2-1735-08DE238C96A9"),
          Nome = "Sol-I-Vista",
          ConcelhoId = Guid.Parse("A082C57A-2660-401B-06AA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:00:48.1243448", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("199092DF-E7DA-4511-1736-08DE238C96A9"),
          Nome = "Barri Gaudí",
          ConcelhoId = Guid.Parse("A082C57A-2660-401B-06AA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:01:09.0636480", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("096FCBFE-AD8B-4AAE-1737-08DE238C96A9"),
          Nome = "La Plana",
          ConcelhoId = Guid.Parse("A082C57A-2660-401B-06AA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:01:36.1275241", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("EB33417F-7DD1-44C5-1738-08DE238C96A9"),
          Nome = "Salt",
          ConcelhoId = Guid.Parse("69710515-1830-4639-06A8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:01:59.1898480", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("3CB57A75-7B4E-497E-1739-08DE238C96A9"),
          Nome = "Domeny",
          ConcelhoId = Guid.Parse("69710515-1830-4639-06A8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:02:14.1220287", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("DDD11C66-231B-467B-173A-08DE238C96A9"),
          Nome = "Quart",
          ConcelhoId = Guid.Parse("69710515-1830-4639-06A8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:02:31.3354879", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4EE927A2-FAED-4E94-173B-08DE238C96A9"),
          Nome = "San Gabriel",
          ConcelhoId = Guid.Parse("A5F6C141-F2AD-4668-06AC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:03:05.0853633", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("A95BDC1B-2D63-4A57-173C-08DE238C96A9"),
          Nome = "Garbinet",
          ConcelhoId = Guid.Parse("A5F6C141-F2AD-4668-06AC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:03:17.1990369", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("2A2B8996-2D00-40D0-173D-08DE238C96A9"),
          Nome = "Orgegia",
          ConcelhoId = Guid.Parse("A5F6C141-F2AD-4668-06AC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:03:29.5955384", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("2C551FEF-BC6E-47C6-173E-08DE238C96A9"),
          Nome = "Imaginalia",
          ConcelhoId = Guid.Parse("DC979C3D-3BD1-4376-06AD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:04:00.5290561", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E973C119-C797-4CD5-173F-08DE238C96A9"),
          Nome = "Franciscanos",
          ConcelhoId = Guid.Parse("DC979C3D-3BD1-4376-06AD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:04:12.4686239", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("9A710219-337D-4B91-1740-08DE238C96A9"),
          Nome = "Campollano",
          ConcelhoId = Guid.Parse("DC979C3D-3BD1-4376-06AD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:04:28.7723283", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("1FB30C08-47D8-4238-1741-08DE238C96A9"),
          Nome = "Cala",
          ConcelhoId = Guid.Parse("884E79B8-9FAC-4F59-06AB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:05:16.7956678", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("974018BE-F33C-4A1B-1742-08DE238C96A9"),
          Nome = "Poniente",
          ConcelhoId = Guid.Parse("884E79B8-9FAC-4F59-06AB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:05:34.0836987", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("6EFBDE11-103A-41A2-1743-08DE238C96A9"),
          Nome = "La Creu",
          ConcelhoId = Guid.Parse("884E79B8-9FAC-4F59-06AB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:05:48.3481399", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("D93FE713-4C18-4357-1744-08DE238C96A9"),
          Nome = "El Bercial",
          ConcelhoId = Guid.Parse("9A8D0C25-05B0-4EC7-06AE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:06:16.8671934", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E3E1F21D-DCA5-4C20-1745-08DE238C96A9"),
          Nome = "La Alhóndiga",
          ConcelhoId = Guid.Parse("9A8D0C25-05B0-4EC7-06AE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:06:32.6216101", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("9F44C7A9-52B3-403B-1746-08DE238C96A9"),
          Nome = "San Cristóbal",
          ConcelhoId = Guid.Parse("9A8D0C25-05B0-4EC7-06AE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:07:15.8305974", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("135385C1-FDF0-4B50-1747-08DE238C96A9"),
          Nome = "Santa Barbara",
          ConcelhoId = Guid.Parse("98C55707-7DCE-4C24-06B0-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:07:52.6192928", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("EBF58829-F190-4201-1748-08DE238C96A9"),
          Nome = "Azucaica",
          ConcelhoId = Guid.Parse("98C55707-7DCE-4C24-06B0-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:08:09.3873119", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("2ACE64A6-61B4-4FAC-1749-08DE238C96A9"),
          Nome = "Las Nieves",
          ConcelhoId = Guid.Parse("98C55707-7DCE-4C24-06B0-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:08:19.7182498", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("8CD52F21-C1DC-45DD-174A-08DE238C96A9"),
          Nome = "Iriépal",
          ConcelhoId = Guid.Parse("57C86220-56B4-4688-06AF-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:08:52.7649461", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("62535F12-DF71-4768-174B-08DE238C96A9"),
          Nome = "El Alamín",
          ConcelhoId = Guid.Parse("57C86220-56B4-4688-06AF-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:09:11.4664831", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("1839120E-0DE8-4A6D-174C-08DE238C96A9"),
          Nome = "Cacharrerías",
          ConcelhoId = Guid.Parse("57C86220-56B4-4688-06AF-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:09:29.1653438", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4F9A2AC1-458D-4720-174D-08DE238C96A9"),
          Nome = "Les Goulvents",
          ConcelhoId = Guid.Parse("BC030406-B610-42E6-59C1-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:11:42.6437426", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("51A73064-4D60-4E48-174E-08DE238C96A9"),
          Nome = "Les Gibets",
          ConcelhoId = Guid.Parse("BC030406-B610-42E6-59C1-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:11:56.4289014", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("7EFEA4D2-6D92-44DA-174F-08DE238C96A9"),
          Nome = "Les Luaps",
          ConcelhoId = Guid.Parse("BC030406-B610-42E6-59C1-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:12:04.9471960", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("EF6C8944-0032-4E0D-1750-08DE238C96A9"),
          Nome = "Mont-Mesly",
          ConcelhoId = Guid.Parse("0E671A71-1A7D-4D98-59C2-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:12:23.6929574", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("A6F02D44-DAC0-420C-1751-08DE238C96A9"),
          Nome = "Stains",
          ConcelhoId = Guid.Parse("299B68B1-1C2E-4E5F-59C3-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:12:55.2345296", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("FF02EF4B-51B0-4B92-1752-08DE238C96A9"),
          Nome = "Villetaneuse",
          ConcelhoId = Guid.Parse("299B68B1-1C2E-4E5F-59C3-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:13:09.5073586", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("D42F16D5-6923-4B62-1753-08DE238C96A9"),
          Nome = "Aubervilliers",
          ConcelhoId = Guid.Parse("299B68B1-1C2E-4E5F-59C3-08DE17AB6051"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:13:24.4197409", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("D28F8EC9-E4E2-459C-1754-08DE238C96A9"),
          Nome = "La Grande Bastide",
          ConcelhoId = Guid.Parse("81A94386-7FFA-4E38-06C8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:14:30.6986744", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("BF734913-61B6-4D27-1755-08DE238C96A9"),
          Nome = "Les Cuettes",
          ConcelhoId = Guid.Parse("81A94386-7FFA-4E38-06C8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:14:40.0209194", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("5483C439-B61E-4E44-1756-08DE238C96A9"),
          Nome = "Le Plan de la Gare",
          ConcelhoId = Guid.Parse("81A94386-7FFA-4E38-06C8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:14:49.6917420", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("0B82C452-3F3D-405E-1757-08DE238C96A9"),
          Nome = "Les Aubagnens",
          ConcelhoId = Guid.Parse("978D17B0-137C-4287-06C7-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:15:16.4538629", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("B214C853-5BAF-466D-1758-08DE238C96A9"),
          Nome = "Plan-de-Cuques",
          ConcelhoId = Guid.Parse("978D17B0-137C-4287-06C7-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:15:32.4754917", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("843D4A9E-EE71-42A7-1759-08DE238C96A9"),
          Nome = "Les Tourres",
          ConcelhoId = Guid.Parse("978D17B0-137C-4287-06C7-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:15:52.1800474", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("F815600F-FD6D-4744-175A-08DE238C96A9"),
          Nome = "La Madrague",
          ConcelhoId = Guid.Parse("013A03B1-809D-47A5-06C6-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:16:21.6442188", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("8595E749-B949-4C4B-175B-08DE238C96A9"),
          Nome = "Les Goudes",
          ConcelhoId = Guid.Parse("013A03B1-809D-47A5-06C6-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:16:37.9549884", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("392F483A-83AD-45C2-175C-08DE238C96A9"),
          Nome = "Montredon",
          ConcelhoId = Guid.Parse("013A03B1-809D-47A5-06C6-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:16:48.5421333", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("E59234C0-2AD8-4899-175D-08DE238C96A9"),
          Nome = "Centre-Est",
          ConcelhoId = Guid.Parse("5DFBC274-2D7F-4CB1-06C5-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:17:12.2980700", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("CDBE5B85-6292-4A0B-175E-08DE238C96A9"),
          Nome = "L'Arsenal",
          ConcelhoId = Guid.Parse("5DFBC274-2D7F-4CB1-06C5-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:17:24.7179583", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("DC201A8F-EB11-43F9-175F-08DE238C96A9"),
          Nome = "Ctre - Chassagnon",
          ConcelhoId = Guid.Parse("5DFBC274-2D7F-4CB1-06C5-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:17:37.9803364", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("C6E07683-69D7-40B9-1760-08DE238C96A9"),
          Nome = "Villeneuve",
          ConcelhoId = Guid.Parse("C84B0936-1089-4F4B-06C4-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:18:00.9296652", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("550A3B44-6BD9-43D0-1761-08DE238C96A9"),
          Nome = "Champvert",
          ConcelhoId = Guid.Parse("C84B0936-1089-4F4B-06C4-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:18:15.8433656", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("2208798B-1F7B-4403-1762-08DE238C96A9"),
          Nome = "Tassin-la-Demi-Lune",
          ConcelhoId = Guid.Parse("C84B0936-1089-4F4B-06C4-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:18:29.1254294", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("427473C1-AB58-47B6-1763-08DE238C96A9"),
          Nome = "Cuire le Haut",
          ConcelhoId = Guid.Parse("06B3A645-E94C-470F-06C3-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:18:58.0268860", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("1CAF4CD2-E355-4C36-1764-08DE238C96A9"),
          Nome = "Margnolles - Rhône",
          ConcelhoId = Guid.Parse("06B3A645-E94C-470F-06C3-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:19:15.3145226", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("346C7556-044F-4D03-1765-08DE238C96A9"),
          Nome = "Bissardon",
          ConcelhoId = Guid.Parse("06B3A645-E94C-470F-06C3-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:19:25.2646390", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("0DE1DEA7-7C62-45A8-1766-08DE238C96A9"),
          Nome = "Segnanino",
          ConcelhoId = Guid.Parse("DD20CD99-2FFB-4354-06C2-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:19:52.0766544", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("30C17C6C-E736-4285-1767-08DE238C96A9"),
          Nome = "Segnano",
          ConcelhoId = Guid.Parse("DD20CD99-2FFB-4354-06C2-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:20:07.0660317", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("2DB1AC9B-8E83-4684-1768-08DE238C96A9"),
          Nome = "Prato Centenaro",
          ConcelhoId = Guid.Parse("DD20CD99-2FFB-4354-06C2-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:20:17.9621439", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("26B5B607-94B6-4624-1769-08DE238C96A9"),
          Nome = "Garegnano",
          ConcelhoId = Guid.Parse("1B80452E-EFA9-4E5A-06C1-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:21:55.6921235", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("904C2FB3-E267-4DE5-176A-08DE238C96A9"),
          Nome = "Gallaratese",
          ConcelhoId = Guid.Parse("1B80452E-EFA9-4E5A-06C1-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:22:06.1400516", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("01209B37-696A-4C04-176B-08DE238C96A9"),
          Nome = "Trenno",
          ConcelhoId = Guid.Parse("1B80452E-EFA9-4E5A-06C1-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:22:17.9481424", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("BE263FCB-B18C-4509-176C-08DE238C96A9"),
          Nome = "Porta Genova",
          ConcelhoId = Guid.Parse("D5CA07F0-810A-405F-06C0-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:22:57.6359954", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("229EC8B9-8F96-47E9-176D-08DE238C96A9"),
          Nome = "Zona Solari",
          ConcelhoId = Guid.Parse("D5CA07F0-810A-405F-06C0-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:23:17.6188110", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("34915DBF-941B-475E-176E-08DE238C96A9"),
          Nome = "San Cristoforo Sul Naviglio",
          ConcelhoId = Guid.Parse("D5CA07F0-810A-405F-06C0-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:23:41.9871333", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("AA82C409-19F5-4EF9-176F-08DE238C96A9"),
          Nome = "Villaggio Olimpico",
          ConcelhoId = Guid.Parse("13573EF8-BFBE-4E86-06BF-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:24:09.4703163", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("4E791A35-A2A6-472B-1770-08DE238C96A9"),
          Nome = "Trieste",
          ConcelhoId = Guid.Parse("13573EF8-BFBE-4E86-06BF-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:24:39.3625770", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("057EBE91-02D5-48E8-1771-08DE238C96A9"),
          Nome = "Nomentano",
          ConcelhoId = Guid.Parse("13573EF8-BFBE-4E86-06BF-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:24:52.7389768", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("6A0224F6-24B3-4615-1772-08DE238C96A9"),
          Nome = "Ostiense",
          ConcelhoId = Guid.Parse("AC97CEBA-BE51-4A68-06BE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:25:17.4880710", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("8618C779-D6B3-4F0C-1773-08DE238C96A9"),
          Nome = "Valco San Paolo",
          ConcelhoId = Guid.Parse("AC97CEBA-BE51-4A68-06BE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:25:30.8431823", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("8BE7E232-10B2-403F-1774-08DE238C96A9"),
          Nome = "Porta Metronia",
          ConcelhoId = Guid.Parse("AC97CEBA-BE51-4A68-06BE-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:25:52.7634772", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("CA1E839A-2BCF-44E9-1775-08DE238C96A9"),
          Nome = "Mostacciano",
          ConcelhoId = Guid.Parse("B202D26F-680D-46E0-06BD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:26:20.9655487", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("D4D806EA-2DF4-4311-1776-08DE238C96A9"),
          Nome = "Mezzocammino",
          ConcelhoId = Guid.Parse("B202D26F-680D-46E0-06BD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:26:30.1559490", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("7999D7F3-B6FD-4743-1777-08DE238C96A9"),
          Nome = "Poggio del Torrino",
          ConcelhoId = Guid.Parse("B202D26F-680D-46E0-06BD-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:26:42.7465529", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("543446AD-3D62-4051-1778-08DE238C96A9"),
          Nome = "Soccavo",
          ConcelhoId = Guid.Parse("4885A0A7-6D49-4818-06BC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:27:25.5629529", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("6FACEACC-F86D-43FF-1779-08DE238C96A9"),
          Nome = "Astroni",
          ConcelhoId = Guid.Parse("4885A0A7-6D49-4818-06BC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:27:36.4597454", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("49DF13D4-B291-4EE7-177A-08DE238C96A9"),
          Nome = "Pianura",
          ConcelhoId = Guid.Parse("4885A0A7-6D49-4818-06BC-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:27:46.3234552", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("8410369F-02B8-4E29-177B-08DE238C96A9"),
          Nome = "Antiniana",
          ConcelhoId = Guid.Parse("E65E5E58-DBAF-4EAA-06BB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:28:33.8499415", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("6F34F2E5-E726-4653-177C-08DE238C96A9"),
          Nome = "Bagnoli",
          ConcelhoId = Guid.Parse("E65E5E58-DBAF-4EAA-06BB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:28:48.6958033", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("85C76F21-AE1D-4AAF-177D-08DE238C96A9"),
          Nome = "Cofanara",
          ConcelhoId = Guid.Parse("E65E5E58-DBAF-4EAA-06BB-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:29:23.2764659", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("7A52A0BC-1017-46B6-177E-08DE238C96A9"),
          Nome = "Marianella",
          ConcelhoId = Guid.Parse("729006EA-14BB-4110-06BA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:29:47.9624927", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("2A50AA35-54ED-48A4-177F-08DE238C96A9"),
          Nome = "Rione Villa",
          ConcelhoId = Guid.Parse("729006EA-14BB-4110-06BA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:29:56.9869393", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("9E77CF76-5C15-4C13-1780-08DE238C96A9"),
          Nome = "Santa Maria del Pozzo",
          ConcelhoId = Guid.Parse("729006EA-14BB-4110-06BA-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:30:08.2437355", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("3ED29B2B-7D0F-4A75-1781-08DE238C96A9"),
          Nome = "Großhesselohe",
          ConcelhoId = Guid.Parse("9EE35E85-52B5-453D-06B9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:31:16.3946368", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("8C081238-D503-4CA0-1782-08DE238C96A9"),
          Nome = "Großhesselohe Isartalbahnhof",
          ConcelhoId = Guid.Parse("9EE35E85-52B5-453D-06B9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:31:26.7418670", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("27F6A01E-4FB4-4B9D-1783-08DE238C96A9"),
          Nome = "Forstenried-Fürstenried",
          ConcelhoId = Guid.Parse("9EE35E85-52B5-453D-06B9-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:32:03.2932748", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("BC967F77-1CEC-416E-1784-08DE238C96A9"),
          Nome = "Trudering-Riem",
          ConcelhoId = Guid.Parse("ADCF872D-F3ED-4B66-06B8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:32:41.7906513", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("1647799F-B597-4FE5-1785-08DE238C96A9"),
          Nome = "Dornach",
          ConcelhoId = Guid.Parse("ADCF872D-F3ED-4B66-06B8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:33:06.7393199", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("A85570DE-FF88-4C5A-1786-08DE238C96A9"),
          Nome = "Salmdorf",
          ConcelhoId = Guid.Parse("ADCF872D-F3ED-4B66-06B8-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:33:24.1633049", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("F3FC8D53-3775-4271-1787-08DE238C96A9"),
          Nome = "Kleinhadern",
          ConcelhoId = Guid.Parse("5042659E-EC5E-40DA-06B7-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:33:55.8290113", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("49A0C761-E8ED-47A3-1788-08DE238C96A9"),
          Nome = "Friedenheim",
          ConcelhoId = Guid.Parse("5042659E-EC5E-40DA-06B7-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:34:18.7400162", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("9C68E566-0391-44AA-1789-08DE238C96A9"),
          Nome = "Agnes-Bernauer",
          ConcelhoId = Guid.Parse("5042659E-EC5E-40DA-06B7-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:34:42.8037425", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("43466C1D-70FC-415B-178A-08DE238C96A9"),
          Nome = "Mörsenbroich",
          ConcelhoId = Guid.Parse("BA4CB3DC-2C2B-4F02-06B6-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:35:27.2586574", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("15875C66-074A-48CA-178B-08DE238C96A9"),
          Nome = "Knittkuhl",
          ConcelhoId = Guid.Parse("BA4CB3DC-2C2B-4F02-06B6-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:35:46.7496439", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("28F8FC5A-44DC-4442-178C-08DE238C96A9"),
          Nome = "Eckamp",
          ConcelhoId = Guid.Parse("BA4CB3DC-2C2B-4F02-06B6-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:35:59.9871730", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("5A53E7D0-5719-4205-178D-08DE238C96A9"),
          Nome = "Wersten",
          ConcelhoId = Guid.Parse("192F8BCB-8998-4E22-06B5-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:37:19.8514863", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("7F55C815-621F-46D6-178E-08DE238C96A9"),
          Nome = "Lierenfeld",
          ConcelhoId = Guid.Parse("192F8BCB-8998-4E22-06B5-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:37:41.8712906", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("A82EC77E-6B42-4266-178F-08DE238C96A9"),
          Nome = "Vennhausen",
          ConcelhoId = Guid.Parse("192F8BCB-8998-4E22-06B5-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:37:54.0271642", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("CE9EFBB2-65E3-4225-1790-08DE238C96A9"),
          Nome = "Seestern",
          ConcelhoId = Guid.Parse("B183D7FC-EC4D-4702-06B4-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:38:20.0430361", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("77F8BF82-444A-441B-1791-08DE238C96A9"),
          Nome = "Rheinallee",
          ConcelhoId = Guid.Parse("B183D7FC-EC4D-4702-06B4-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:39:04.0683507", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("3AE29B72-7B0F-4643-1792-08DE238C96A9"),
          Nome = "Werftstraße",
          ConcelhoId = Guid.Parse("B183D7FC-EC4D-4702-06B4-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:39:30.2305139", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("FE46E404-B08F-480D-1793-08DE238C96A9"),
          Nome = "Johannisthal",
          ConcelhoId = Guid.Parse("56C72180-CB28-4FE0-06B3-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:40:11.6391056", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("B56A462F-45E0-43CE-1794-08DE238C96A9"),
          Nome = "Gropiusstadt",
          ConcelhoId = Guid.Parse("56C72180-CB28-4FE0-06B3-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:40:25.6816602", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("55D35183-77A5-4397-1795-08DE238C96A9"),
          Nome = "Thiekesiedlung",
          ConcelhoId = Guid.Parse("56C72180-CB28-4FE0-06B3-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:40:39.0837246", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("59E5DE93-F8C7-42DA-1796-08DE238C96A9"),
          Nome = "Siedlung Habichtswald",
          ConcelhoId = Guid.Parse("4F25AA79-B803-4A2A-06B2-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:41:09.1951705", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("B36E1D85-F784-444C-1797-08DE238C96A9"),
          Nome = "Hohengatow",
          ConcelhoId = Guid.Parse("4F25AA79-B803-4A2A-06B2-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:41:25.4988767", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("77CC5F3C-EE7E-4A99-1798-08DE238C96A9"),
          Nome = "Landstadt Gatow",
          ConcelhoId = Guid.Parse("4F25AA79-B803-4A2A-06B2-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:41:42.8591012", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("3C9EC702-62A8-4C76-1799-08DE238C96A9"),
          Nome = "Brüsseler Kiez",
          ConcelhoId = Guid.Parse("1385AAF1-4688-4F1B-06B1-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:42:10.1234915", CultureInfo.InvariantCulture),
          LastModifiedBy = seedUserId,
          LastModifiedOn = DateTime.Parse(
            "2025-11-14T15:42:49.9307885",
            CultureInfo.InvariantCulture
          ),
        },
        new()
        {
          Id = Guid.Parse("C18F5733-9F2C-4F30-179A-08DE238C96A9"),
          Nome = "Moabit",
          ConcelhoId = Guid.Parse("1385AAF1-4688-4F1B-06B1-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:42:24.2678937", CultureInfo.InvariantCulture),
        },
        new()
        {
          Id = Guid.Parse("32A37F05-3DF5-458A-179B-08DE238C96A9"),
          Nome = "Sprengelkiez",
          ConcelhoId = Guid.Parse("1385AAF1-4688-4F1B-06B1-08DE2387E9B4"),
          CreatedBy = seedUserId,
          CreatedOn = DateTime.Parse("2025-11-14T15:42:35.5466505", CultureInfo.InvariantCulture),
        },
      ];

      HashSet<Guid> existingParishIds = context.Freguesias.Select(f => f.Id).ToHashSet();
      HashSet<Guid> existingConcelhoIds = context.Concelhos.Select(c => c.Id).ToHashSet();

      List<Freguesia> parishesToInsert = parishesToSeed
        .Where(
          parish =>
            existingConcelhoIds.Contains(parish.ConcelhoId) && !existingParishIds.Contains(parish.Id)
        )
        .ToList();

      if (parishesToInsert.Count == 0)
      {
        return;
      }

      context.Freguesias.AddRange(parishesToInsert);
      context.SaveChanges();
    }
  }
}
