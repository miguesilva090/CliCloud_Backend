using Microsoft.EntityFrameworkCore;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;

namespace CliCloud.Infrastructure.Persistence.Extensions
{
  public static class StaticDataSeederExtensions
  {
    public static void SeedStaticData(this ModelBuilder builder) // create methods here for model seed data (static data) -- this data will be managed by EF migrations
    {
      builder.Entity<ConfiguracaoChamadaVozOpcao>().HasData(
        // Language options (paridade com legado)
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000001"), Tipo = "Language", Codigo = "pt", Descricao = "Português", Ordem = 1, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000002"), Tipo = "Language", Codigo = "en", Descricao = "Inglês", Ordem = 2, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000003"), Tipo = "Language", Codigo = "fr", Descricao = "Francês", Ordem = 3, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000004"), Tipo = "Language", Codigo = "es", Descricao = "Espanhol", Ordem = 4, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000005"), Tipo = "Language", Codigo = "zh-CN", Descricao = "Mandarim (China)", Ordem = 5, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000006"), Tipo = "Language", Codigo = "zh-TW", Descricao = "Mandarim (Taiwan)", Ordem = 6, Ativo = true },

        // TLD / variation options (paridade com legado)
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000101"), Tipo = "Tld", Codigo = "pt", Descricao = "Português (PT)", Ordem = 1, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000102"), Tipo = "Tld", Codigo = "com.br", Descricao = "Português (BR)", Ordem = 2, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000103"), Tipo = "Tld", Codigo = "com.au", Descricao = "Inglês (AU)", Ordem = 3, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000104"), Tipo = "Tld", Codigo = "co.uk", Descricao = "Inglês (UK)", Ordem = 4, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000105"), Tipo = "Tld", Codigo = "com", Descricao = "Inglês (US)", Ordem = 5, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000106"), Tipo = "Tld", Codigo = "ca", Descricao = "Inglês/Francês (CA)", Ordem = 6, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000107"), Tipo = "Tld", Codigo = "co.in", Descricao = "Inglês (IN)", Ordem = 7, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000108"), Tipo = "Tld", Codigo = "ie", Descricao = "Inglês (IE)", Ordem = 8, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000109"), Tipo = "Tld", Codigo = "co.za", Descricao = "Inglês (ZA)", Ordem = 9, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000110"), Tipo = "Tld", Codigo = "fr", Descricao = "Francês (FR)", Ordem = 10, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000111"), Tipo = "Tld", Codigo = "es", Descricao = "Espanhol (ES)", Ordem = 11, Ativo = true },
        new ConfiguracaoChamadaVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000112"), Tipo = "Tld", Codigo = "com.mx", Descricao = "Espanhol (MX)", Ordem = 12, Ativo = true }
      );


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
