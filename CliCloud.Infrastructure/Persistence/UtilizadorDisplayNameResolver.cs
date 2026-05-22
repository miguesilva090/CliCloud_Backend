using System.Data;
using System.Security.Claims;
using CliCloud.Application.Common;
using CliCloud.Application.Services.Funcionarios.FuncionarioService.Specifications;
using CliCloud.Application.Services.Medicos.MedicoService.Specifications;
using CliCloud.Application.Services.Tecnicos.TecnicoService.Specifications;
using CliCloud.Domain.Entities.Funcionarios;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Infrastructure.Persistence;

/// <summary>
/// Resolve o nome legível do utilizador a partir do email do JWT (e entidades clínica associadas).
/// Espelho legado: GetUtilizadorByGuid — aqui prioriza email + AspNetUsers.UserName.
/// </summary>
public sealed class UtilizadorDisplayNameResolver(
  IHttpContextAccessor httpContextAccessor,
  IRepositoryAsync repository,
  ApplicationDbContext dbContext
) : IUtilizadorDisplayNameResolver
{
  private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
  private readonly IRepositoryAsync _repository = repository;
  private readonly ApplicationDbContext _dbContext = dbContext;

  public async Task<string> ResolveAsync(CancellationToken cancellationToken = default)
  {
    ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
    string? email = user?.FindFirst("email")?.Value?.Trim();

    if (string.IsNullOrWhiteSpace(email))
    {
      return user?.Identity?.Name ?? "Utilizador";
    }

    string? nomeEntidade = await TryNomePorEmailEntidadeAsync(email, cancellationToken);
    if (!string.IsNullOrWhiteSpace(nomeEntidade))
    {
      return nomeEntidade;
    }

    string? nomeAspNet = await TryNomeAspNetUsersPorEmailAsync(email, cancellationToken);
    if (!string.IsNullOrWhiteSpace(nomeAspNet))
    {
      return nomeAspNet;
    }

    return email;
  }

  public async Task<IReadOnlyDictionary<Guid, string>> ResolveManyByIdsAsync(
    IEnumerable<Guid> userIds,
    CancellationToken cancellationToken = default
  )
  {
    List<Guid> ids = userIds.Where(x => x != Guid.Empty).Distinct().ToList();
    if (ids.Count == 0)
    {
      return new Dictionary<Guid, string>();
    }

    var connection = _dbContext.Database.GetDbConnection();
    bool shouldClose = connection.State != ConnectionState.Open;
    if (shouldClose)
    {
      await connection.OpenAsync(cancellationToken);
    }

    var result = new Dictionary<Guid, string>();
    try
    {
      foreach (Guid[] chunk in ids.Chunk(50))
      {
        string inList = string.Join(",", chunk.Select((_, i) => $"@id{i}"));
        await using var command = connection.CreateCommand();
        command.CommandText = $"""
          SELECT [Id],
            COALESCE(NULLIF(LTRIM(RTRIM([UserName])), ''), NULLIF(LTRIM(RTRIM([Email])), ''), CAST([Id] AS nvarchar(36))) AS Nome
          FROM [AspNetUsers]
          WHERE [Id] IN ({inList})
          """;

        for (int i = 0; i < chunk.Length; i++)
        {
          var param = command.CreateParameter();
          param.ParameterName = $"@id{i}";
          param.Value = chunk[i];
          command.Parameters.Add(param);
        }

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
          Guid id = reader.GetGuid(0);
          string nome = reader.GetString(1);
          result[id] = nome;
        }
      }
    }
    catch
    {
      /* fallback vazio — coluna mostra — */
    }
    finally
    {
      if (shouldClose)
      {
        await connection.CloseAsync();
      }
    }

    return result;
  }

  private async Task<string?> TryNomePorEmailEntidadeAsync(
    string email,
    CancellationToken cancellationToken
  )
  {
    List<Medico> medicos = (
      await _repository.GetListAsync<Medico, Guid>(new MedicoByEmailSpec(email), cancellationToken)
    ).ToList();
    string? nome = medicos.FirstOrDefault()?.Nome?.Trim();
    if (!string.IsNullOrWhiteSpace(nome))
    {
      return nome;
    }

    List<Funcionario> funcionarios = (
      await _repository.GetListAsync<Funcionario, Guid>(
        new FuncionarioByEmailSpec(email),
        cancellationToken
      )
    ).ToList();
    nome = funcionarios.FirstOrDefault()?.Nome?.Trim();
    if (!string.IsNullOrWhiteSpace(nome))
    {
      return nome;
    }

    List<Tecnico> tecnicos = (
      await _repository.GetListAsync<Tecnico, Guid>(new TecnicoByEmailSpec(email), cancellationToken)
    ).ToList();
    return tecnicos.FirstOrDefault()?.Nome?.Trim();
  }

  private async Task<string?> TryNomeAspNetUsersPorEmailAsync(
    string email,
    CancellationToken cancellationToken
  )
  {
    string normalized = email.Trim().ToLowerInvariant();

    var connection = _dbContext.Database.GetDbConnection();
    bool shouldClose = connection.State != ConnectionState.Open;
    if (shouldClose)
    {
      await connection.OpenAsync(cancellationToken);
    }

    try
    {
      await using var command = connection.CreateCommand();
      command.CommandText = """
        SELECT TOP (1)
          COALESCE(NULLIF(LTRIM(RTRIM([UserName])), ''), [Email]) AS Nome
        FROM [AspNetUsers]
        WHERE [IsActive] = 1
          AND (
            LOWER(LTRIM(RTRIM([Email]))) = @email
            OR LOWER(LTRIM(RTRIM([UserName]))) = @email
            OR LOWER(LTRIM(RTRIM([NormalizedEmail]))) = @email
          )
        """;

      var param = command.CreateParameter();
      param.ParameterName = "@email";
      param.Value = normalized;
      command.Parameters.Add(param);

      object? scalar = await command.ExecuteScalarAsync(cancellationToken);
      string? result = scalar?.ToString()?.Trim();
      return string.IsNullOrWhiteSpace(result) ? null : result;
    }
    catch
    {
      return null;
    }
    finally
    {
      if (shouldClose)
      {
        await connection.CloseAsync();
      }
    }
  }
}
