using Microsoft.EntityFrameworkCore;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;

namespace CliCloud.Infrastructure.Persistence.Extensions
{
  public static class StaticDataSeederExtensions
  {
    public static void SeedStaticData(this ModelBuilder builder) // create methods here for model seed data (static data) -- this data will be managed by EF migrations
    {

      builder.Entity<MapaBodyChart>().HasData(
        new MapaBodyChart
        {
          Id = new Guid("11111111-1111-1111-1111-111111111111"),
          Nome = "Mapa Ósseo",
          CaminhoImagem = "/UserFiles/MapasBodyChart/sistema-osseo.jpg",
        },
        new MapaBodyChart
        {
          Id = new Guid("22222222-2222-2222-2222-222222222222"),
          Nome = "Body Chart",
          CaminhoImagem = "/UserFiles/MapasBodyChart/body-chart2.jpg",
        },
        new MapaBodyChart
        {
          Id = new Guid("33333333-3333-3333-3333-333333333333"),
          Nome = "Mapa Muscular",
          CaminhoImagem = "/UserFiles/MapasBodyChart/sistema-muscular.jpg",
        }
      );

      // Marcadores específicos por mapa
      builder.Entity<MarcadorBodyChart>().HasData(
        // Body Chart (superficial)
        new MarcadorBodyChart
        {
          Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
          MapaBodyChartId = new Guid("22222222-2222-2222-2222-222222222222"),
          Titulo = "Bloqueio/disfunção",
          CorHex = "#f59e0b", // amarelo
        },
        new MarcadorBodyChart
        {
          Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaab"),
          MapaBodyChartId = new Guid("22222222-2222-2222-2222-222222222222"),
          Titulo = "Hipertonicidade",
          CorHex = "#ef4444", // vermelho
        },
        new MarcadorBodyChart
        {
          Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaac"),
          MapaBodyChartId = new Guid("22222222-2222-2222-2222-222222222222"),
          Titulo = "Hipotonicidade",
          CorHex = "#3b82f6", // azul
        },
        new MarcadorBodyChart
        {
          Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaad"),
          MapaBodyChartId = new Guid("22222222-2222-2222-2222-222222222222"),
          Titulo = "Irradiação da Dor",
          CorHex = "#a855f7", // roxo
        },

        // Mapa Ósseo
        new MarcadorBodyChart
        {
          Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
          MapaBodyChartId = new Guid("11111111-1111-1111-1111-111111111111"),
          Titulo = "Fratura",
          CorHex = "#ef4444",
        },
        new MarcadorBodyChart
        {
          Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbba"),
          MapaBodyChartId = new Guid("11111111-1111-1111-1111-111111111111"),
          Titulo = "Contusão",
          CorHex = "#f97316",
        },
        new MarcadorBodyChart
        {
          Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbc"),
          MapaBodyChartId = new Guid("11111111-1111-1111-1111-111111111111"),
          Titulo = "Lesão Lítica",
          CorHex = "#fde047",
        },
        new MarcadorBodyChart
        {
          Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
          MapaBodyChartId = new Guid("11111111-1111-1111-1111-111111111111"),
          Titulo = "Lesão benigna",
          CorHex = "#22c55e",
        },
        new MarcadorBodyChart
        {
          Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbe"),
          MapaBodyChartId = new Guid("11111111-1111-1111-1111-111111111111"),
          Titulo = "Doença Metabólica",
          CorHex = "#0ea5e9",
        },

        // Mapa Muscular
        new MarcadorBodyChart
        {
          Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
          MapaBodyChartId = new Guid("33333333-3333-3333-3333-333333333333"),
          Titulo = "Estiramento",
          CorHex = "#22c55e",
        },
        new MarcadorBodyChart
        {
          Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccd"),
          MapaBodyChartId = new Guid("33333333-3333-3333-3333-333333333333"),
          Titulo = "Contusão",
          CorHex = "#f97316",
        },
        new MarcadorBodyChart
        {
          Id = new Guid("cccccccc-cccc-cccc-cccc-ccccccccccce"),
          MapaBodyChartId = new Guid("33333333-3333-3333-3333-333333333333"),
          Titulo = "Contratura",
          CorHex = "#e11d48",
        },
        new MarcadorBodyChart
        {
          Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccf"),
          MapaBodyChartId = new Guid("33333333-3333-3333-3333-333333333333"),
          Titulo = "Ruptura",
          CorHex = "#7c3aed",
        },
        new MarcadorBodyChart
        {
          Id = new Guid("cccccccc-cccc-cccc-cccc-ccccccccccd0"),
          MapaBodyChartId = new Guid("33333333-3333-3333-3333-333333333333"),
          Titulo = "Dor Muscular Tardia",
          CorHex = "#14b8a6",
        }
      );
    }
  }
}
