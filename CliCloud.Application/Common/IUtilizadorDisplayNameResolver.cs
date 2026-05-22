using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Common;

/// <summary>
/// Nome de exibição do utilizador autenticado (ex.: carimbo em observações de admissão).
/// </summary>
public interface IUtilizadorDisplayNameResolver : IScopedService
{
  Task<string> ResolveAsync(CancellationToken cancellationToken = default);

  /// <summary>Nomes de exibição por Id de utilizador (AspNetUsers), ex. coluna Utilizador na ordem de entrada.</summary>
  Task<IReadOnlyDictionary<Guid, string>> ResolveManyByIdsAsync(
    IEnumerable<Guid> userIds,
    CancellationToken cancellationToken = default
  );
}
