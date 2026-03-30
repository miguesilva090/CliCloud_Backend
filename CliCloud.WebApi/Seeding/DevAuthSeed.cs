using CliCloud.Domain.Entities.Core;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Utility;
using CliCloud.Domain.Entities.Utentes;
using CliCloud.Domain.Enums;
using CliCloud.Infrastructure.Identity;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.WebApi.Seeding
{
  /// <summary>
  /// Seed mínimo (DEV) para testar autenticação multi-client:
  /// - 1 Clinica (tenant)
  /// - 1 ClinicaApiKey (X-API-Key)
  /// - 1 role "client"
  /// - 1 user com password e role "client"
  /// - 1 Utente e 1 Medico para testes (atestados, etc.)
  /// </summary>
  public static class DevAuthSeed
  {
    public const string SeedApiKey = "abcdefghijklmnopqrstuvwxyz0123456789ABCDE"; // >= 25 chars
    public const string SeedUserEmail = "cliente@demo.local";
    public const string SeedUserPassword = "Password123!";
    public const string SeedRole = "client";

    /// <summary>Nome do utente de teste criado pelo seed.</summary>
    public const string SeedUtenteNome = "Utente Teste";

    /// <summary>Nome do médico de teste criado pelo seed.</summary>
    public const string SeedMedicoNome = "Médico Teste";

    public static async Task SeedAsync(IServiceProvider services)
    {
      await SeedClinicaAndApiKeyAsync(services);
      await SeedIdentityAsync(services);
      await SeedUtilityLookupsAsync(services);
      await SeedUtenteEMedicoAsync(services);
    }

    private static async Task SeedClinicaAndApiKeyAsync(IServiceProvider services)
    {
      using IServiceScope scope = services.CreateScope();
      ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      // Clinica
      Clinica? clinica = await db.Clinicas.FirstOrDefaultAsync(c => c.Nome == "Clínica Demo");
      if (clinica == null)
      {
        clinica = new Clinica
        {
          Id = Guid.NewGuid(),
          Nome = "Clínica Demo",
          TipoEntidade = EntidadeTipo.Clinica,
          NomeComercial = "Clínica Demo",
          Abreviatura = "DEMO",
        };
        _ = db.Clinicas.Add(clinica);
        await db.SaveChangesAsync();
      }

      // API Key (multi-client)
      bool apiKeyExists = await db.ClinicasApiKeys.AnyAsync(k => k.ApiKey == SeedApiKey);
      if (!apiKeyExists)
      {
        var apiKey = new ClinicaApiKey
        {
          Id = Guid.NewGuid(),
          ClinicaId = clinica.Id,
          ApiKey = SeedApiKey,
          Ativo = true,
        };
        _ = db.ClinicasApiKeys.Add(apiKey);
        await db.SaveChangesAsync();
      }
    }

    private static async Task SeedIdentityAsync(IServiceProvider services)
    {
      using IServiceScope scope = services.CreateScope();
      RoleManager<IdentityRole> roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      UserManager<ApplicationUser> userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

      // Role
      if (!await roleManager.RoleExistsAsync(SeedRole))
      {
        IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole(SeedRole));
        if (!roleResult.Succeeded)
        {
          string error = string.Join("; ", roleResult.Errors.Select(e => e.Description));
          throw new InvalidOperationException($"Falha a criar role '{SeedRole}': {error}");
        }
      }

      // User
      ApplicationUser? user = await userManager.FindByEmailAsync(SeedUserEmail);
      if (user == null)
      {
        user = new ApplicationUser
        {
          // Importante: manter GUID em string para compatibilidade com o audit (Guid.Parse no CurrentUserId)
          Id = Guid.NewGuid().ToString(),
          UserName = SeedUserEmail,
          Email = SeedUserEmail,
          EmailConfirmed = true,
          IsActive = true,
        };

        IdentityResult userResult = await userManager.CreateAsync(user, SeedUserPassword);
        if (!userResult.Succeeded)
        {
          string error = string.Join("; ", userResult.Errors.Select(e => e.Description));
          throw new InvalidOperationException($"Falha a criar user '{SeedUserEmail}': {error}");
        }
      }

      // Role assignment
      if (!await userManager.IsInRoleAsync(user, SeedRole))
      {
        IdentityResult addRoleResult = await userManager.AddToRoleAsync(user, SeedRole);
        if (!addRoleResult.Succeeded)
        {
          string error = string.Join("; ", addRoleResult.Errors.Select(e => e.Description));
          throw new InvalidOperationException(
            $"Falha a atribuir role '{SeedRole}' ao user '{SeedUserEmail}': {error}"
          );
        }
      }
    }

    /// <summary>
    /// Seed mínimo (DEV) de dados Utility necessários para criar Utentes:
    /// - 1 CodigoPostal
    /// - 1 Rua (referenciando uma Freguesia existente e o CodigoPostal criado)
    /// </summary>
    private static async Task SeedUtilityLookupsAsync(IServiceProvider services)
    {
      using IServiceScope scope = services.CreateScope();
      ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      // Precisa existir pelo menos 1 freguesia (seed base do sistema)
      var freguesia = await db.Freguesias.AsNoTracking().FirstOrDefaultAsync();
      if (freguesia == null)
      {
        return;
      }

      // CodigoPostal
      var codigoPostal = await db.CodigosPostais.FirstOrDefaultAsync(cp => cp.Codigo == "1000-001");
      if (codigoPostal == null)
      {
        codigoPostal = new CodigoPostal
        {
          Id = Guid.NewGuid(),
          Codigo = "1000-001",
          Localidade = "Lisboa",
        };
        _ = db.CodigosPostais.Add(codigoPostal);
        await db.SaveChangesAsync();
      }

      // Rua
      bool ruaExists = await db.Ruas.AnyAsync(r => r.Nome == "Rua de Teste" && r.CodigoPostalId == codigoPostal.Id);
      if (!ruaExists)
      {
        var rua = new Rua
        {
          Id = Guid.NewGuid(),
          Nome = "Rua de Teste",
          FreguesiaId = freguesia.Id,
          // Para evitar warnings de nullability, carregamos as refs mínimas
          Freguesia = await db.Freguesias.FirstAsync(f => f.Id == freguesia.Id),
          CodigoPostalId = codigoPostal.Id,
          CodigoPostal = codigoPostal,
        };
        _ = db.Ruas.Add(rua);
        await db.SaveChangesAsync();
      }
    }

    /// <summary>
    /// Seed (DEV): 1 Utente e 1 Medico para testes (ex.: atestados carta condução).
    /// Depende de SeedUtilityLookupsAsync (Freguesia, CodigoPostal, Rua) e do user de seed para CreatedBy.
    /// </summary>
    private static async Task SeedUtenteEMedicoAsync(IServiceProvider services)
    {
      using IServiceScope scope = services.CreateScope();
      ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      UserManager<ApplicationUser> userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

      ApplicationUser? seedUser = await userManager.FindByEmailAsync(SeedUserEmail);
      Guid createdBy = Guid.TryParse(seedUser?.Id, out var g) ? g : Guid.Empty;
      if (createdBy == Guid.Empty)
      {
        return;
      }

      Freguesia? freguesia = await db.Freguesias
        .AsNoTracking()
        .Include(f => f.Concelho)
        .ThenInclude(c => c!.Distrito)
        .ThenInclude(d => d!.Pais)
        .FirstOrDefaultAsync();
      if (freguesia?.Concelho?.Distrito?.Pais == null)
      {
        return;
      }

      CodigoPostal? codigoPostal = await db.CodigosPostais.FirstOrDefaultAsync(cp => cp.Codigo == "1000-001");
      Rua? rua = codigoPostal != null
        ? await db.Ruas.FirstOrDefaultAsync(r => r.Nome == "Rua de Teste" && r.CodigoPostalId == codigoPostal.Id)
        : null;
      if (codigoPostal == null || rua == null)
      {
        return;
      }

      DateTime now = DateTime.UtcNow;
      Guid paisId = freguesia.Concelho.Distrito.Pais.Id;
      Guid distritoId = freguesia.Concelho.Distrito.Id;
      Guid concelhoId = freguesia.Concelho.Id;

      // Utente de teste
      bool utenteExiste = await db.Utentes.AnyAsync(u => u.Nome == SeedUtenteNome);
      if (!utenteExiste)
      {
        var utenteId = Guid.NewGuid();
        var utente = new Utente
        {
          Id = utenteId,
          Nome = SeedUtenteNome,
          TipoEntidade = EntidadeTipo.Utente,
          Email = "utente.teste@demo.local",
          NumeroContribuinte = "123456789",
          RuaId = rua.Id,
          CodigoPostalId = codigoPostal.Id,
          FreguesiaId = freguesia.Id,
          ConcelhoId = concelhoId,
          DistritoId = distritoId,
          PaisId = paisId,
          NumeroPorta = "1",
          AndarRua = "",
          Observacoes = "Utente de teste (seed DEV)",
          Status = CliCloud.Domain.Enums.Status.Ativo,
          NumeroUtente = "123456789",
          Desistencia = false,
          Cronico = false,
          TipoConsulta = TipoConsulta.Normal,
          Migrante = false,
          MarkConsentimento = 0,
          RgpdConsentimento = 0,
          MarkTratamentoDados = false,
          CreatedBy = createdBy,
          CreatedOn = now,
          EntidadeContactos =
          [
            new EntidadeContacto
            {
              Id = Guid.NewGuid(),
              EntidadeContactoTipoId = 1,
              EntidadeId = utenteId,
              Valor = "912345678",
              Principal = true,
              CreatedBy = createdBy,
              CreatedOn = now,
            },
          ],
        };
        _ = db.Utentes.Add(utente);
        await db.SaveChangesAsync();
      }

      // Médico de teste
      bool medicoExiste = await db.Medicos.AnyAsync(m => m.Nome == SeedMedicoNome);
      if (!medicoExiste)
      {
        var medico = new Medico
        {
          Id = Guid.NewGuid(),
          Nome = SeedMedicoNome,
          TipoEntidade = EntidadeTipo.Medico,
          Email = "medico.teste@demo.local",
          NumeroContribuinte = "987654321",
          RuaId = rua.Id,
          CodigoPostalId = codigoPostal.Id,
          FreguesiaId = freguesia.Id,
          ConcelhoId = concelhoId,
          DistritoId = distritoId,
          PaisId = paisId,
          NumeroPorta = "2",
          AndarRua = "",
          Observacoes = "Médico de teste (seed DEV)",
          Status = CliCloud.Domain.Enums.Status.Ativo,
          Director = false,
          ComunicacaoNif = false,
          CartaoCidadaoMedico = 0,
          Globalbooking = false,
          CreatedBy = createdBy,
          CreatedOn = now,
        };
        _ = db.Medicos.Add(medico);
        await db.SaveChangesAsync();
      }
    }
  }
}

