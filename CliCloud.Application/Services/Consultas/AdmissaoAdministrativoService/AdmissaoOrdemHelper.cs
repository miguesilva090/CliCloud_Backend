using CliCloud.Application.Common;
using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

/// <summary>
/// Ordem de chegada no dia (legado ADMISS.ObterNumeroChegadaDia).
/// </summary>
internal static class AdmissaoOrdemHelper
{
  public static async Task<int> ObterProximaOrdemDiaAsync(
    Admissao admissao,
    IRepositoryAsync repository
  )
  {
    if (!admissao.Data.HasValue)
    {
      return 1;
    }

    DateTime dia = admissao.Data.Value.Date;
    List<Admissao> doDia = (await repository.GetListAsync<Admissao, Guid>()).ToList();

    int max = doDia
      .Where(a =>
        a.Id != admissao.Id
        && a.DeletedOn == null
        && a.Data.HasValue
        && a.Data.Value.Date == dia
        && a.StatusConsulta != StatusConsulta.Desmarcada
        && a.Ordem.HasValue
      )
      .Select(a => a.Ordem!.Value)
      .DefaultIfEmpty(0)
      .Max();

    return max + 1;
  }
}
