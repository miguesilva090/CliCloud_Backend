using Microsoft.EntityFrameworkCore;
using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.ProcessoClinico.BodyChart;

namespace CliCloud.Infrastructure.Persistence.Extensions
{
  public static class StaticDataSeederExtensions
  {
    public static void SeedStaticData(this ModelBuilder builder) // create methods here for model seed data (static data) -- this data will be managed by EF migrations
    {
      builder.Entity<ConfiguracaoVozOpcao>().HasData(
        // Language options (paridade com legado)
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000001"), Tipo = "Language", Codigo = "pt-PT", Descricao = "Português (PT)", Ordem = 1, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000002"), Tipo = "Language", Codigo = "pt-BR", Descricao = "Português (BR)", Ordem = 2, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000003"), Tipo = "Language", Codigo = "en-US", Descricao = "Inglês (US)", Ordem = 3, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000004"), Tipo = "Language", Codigo = "es-ES", Descricao = "Espanhol (ES)", Ordem = 4, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000005"), Tipo = "Language", Codigo = "zh-CN", Descricao = "Mandarim", Ordem = 5, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000007"), Tipo = "Language", Codigo = "fr-FR", Descricao = "Francês (FR)", Ordem = 7, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000008"), Tipo = "Language", Codigo = "it-IT", Descricao = "Italiano (IT)", Ordem = 8, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000009"), Tipo = "Language", Codigo = "de-DE", Descricao = "Alemão (DE)", Ordem = 9, Ativo = true },

        // Voice options (seleção de voz TTS)
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000101"), Tipo = "Voice", Codigo = "pt-PT-Female", Descricao = "Português (PT) - Feminina", Ordem = 1, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000102"), Tipo = "Voice", Codigo = "pt-PT-Male", Descricao = "Português (PT) - Masculina", Ordem = 2, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000103"), Tipo = "Voice", Codigo = "pt-BR-Female", Descricao = "Português (BR) - Feminina", Ordem = 3, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000104"), Tipo = "Voice", Codigo = "pt-BR-Male", Descricao = "Português (BR) - Masculina", Ordem = 4, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000105"), Tipo = "Voice", Codigo = "en-US-Female", Descricao = "Inglês (US) - Feminina", Ordem = 5, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000106"), Tipo = "Voice", Codigo = "en-US-Male", Descricao = "Inglês (US) - Masculina", Ordem = 6, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000107"), Tipo = "Voice", Codigo = "es-ES-Female", Descricao = "Espanhol (ES) - Feminina", Ordem = 7, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000108"), Tipo = "Voice", Codigo = "es-ES-Male", Descricao = "Espanhol (ES) - Masculina", Ordem = 8, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000109"), Tipo = "Voice", Codigo = "fr-FR-Female", Descricao = "Francês (FR) - Feminina", Ordem = 9, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000110"), Tipo = "Voice", Codigo = "fr-FR-Male", Descricao = "Francês (FR) - Masculina", Ordem = 10, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000111"), Tipo = "Voice", Codigo = "zh-CN-Female", Descricao = "Mandarim (CN) - Feminina", Ordem = 11, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000112"), Tipo = "Voice", Codigo = "zh-CN-Male", Descricao = "Mandarim (CN) - Masculina", Ordem = 12, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000113"), Tipo = "Voice", Codigo = "it-IT-Female", Descricao = "Italiano (IT) - Feminina", Ordem = 13, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000114"), Tipo = "Voice", Codigo = "it-IT-Male", Descricao = "Italiano (IT) - Masculina", Ordem = 14, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000115"), Tipo = "Voice", Codigo = "de-DE-Female", Descricao = "Alemão (DE) - Feminina", Ordem = 15, Ativo = true },
        new ConfiguracaoVozOpcao { Id = new Guid("f1a10000-0000-0000-0000-000000000116"), Tipo = "Voice", Codigo = "de-DE-Male", Descricao = "Alemão (DE) - Masculina", Ordem = 16, Ativo = true }
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
