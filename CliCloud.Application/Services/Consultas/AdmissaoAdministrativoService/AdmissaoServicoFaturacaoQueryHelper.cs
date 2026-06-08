using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.Specifications;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

internal static class AdmissaoServicoFaturacaoQueryHelper
{
  public static async Task<HashSet<Guid>> ObterAdmissaoServicoIdsJaFaturadosAsync(
    IRepositoryAsync repository,
    IEnumerable<Guid> admissaoServicoIds,
    CancellationToken ct = default
  )
  {
    List<Guid> ids = admissaoServicoIds.Where(x => x != Guid.Empty).Distinct().ToList();
    if (ids.Count == 0)
    {
      return [];
    }

    List<DocumentoLinha> linhas = (
      await repository.GetListAsync<DocumentoLinha, Guid>(
        new DocumentoLinhaByAdmissaoServicoIdsSpec(ids),
        ct
      )
    ).ToList();

    if (linhas.Count == 0)
    {
      return [];
    }

    List<Guid> documentoIds = linhas.Select(l => l.DocumentoId).Distinct().ToList();
    List<Documento> documentos = (
      await repository.GetListAsync<Documento, Guid>(
        new DocumentoByIdsNaoAnuladosComTipoSpec(documentoIds),
        ct
      )
    ).ToList();

    // Legado: só fatura global ao organismo consome linhas (CodigoFaturaOrganismo / faturado na admissão).
    // Recibos (FR) e FA avulsa desde admissão não bloqueiam a importação na fatura global.
    HashSet<Guid> documentosValidos = documentos
      .Where(d => d.FaturaGlobalDataInicio.HasValue)
      .Select(d => d.Id)
      .ToHashSet();

    return linhas
      .Where(l => l.AdmissaoServicoId.HasValue && documentosValidos.Contains(l.DocumentoId))
      .Select(l => l.AdmissaoServicoId!.Value)
      .ToHashSet();
  }
}
