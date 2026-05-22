using CliCloud.Application.Common;
using CliCloud.Application.Services.Consultas.ConsultaService.Specifications;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

/// <summary>
/// Sincroniza flags de faturação da admissão para <see cref="ConsultaFaturacao"/> no fecho/promoção.
/// </summary>
internal static class AdmissaoFaturacaoPromocaoHelper
{
  public static async Task SincronizarDesdeAdmissaoAsync(
    IRepositoryAsync repository,
    Admissao admissao,
    Guid consultaId,
    CancellationToken cancellationToken = default
  )
  {
    List<ConsultaFaturacao> existentes = (
      await repository.GetListAsync<ConsultaFaturacao, Guid>(
        new ConsultaFaturacaoByConsultaId(consultaId),
        cancellationToken
      )
    ).ToList();

    ConsultaFaturacao? faturacao = existentes.FirstOrDefault();
    bool pago = admissao.Pago == true;
    bool faturado = admissao.Faturado == true;

    if (faturacao == null)
    {
      faturacao = new ConsultaFaturacao
      {
        ConsultaId = consultaId,
        Pago = pago,
        Faturado = faturado,
      };
      if (faturacao.Id == Guid.Empty)
      {
        faturacao.Id = Guid.NewGuid();
      }

      _ = await repository.CreateAsync<ConsultaFaturacao, Guid>(faturacao);
      return;
    }

    if (admissao.Pago.HasValue)
    {
      faturacao.Pago = pago;
    }

    if (admissao.Faturado.HasValue)
    {
      faturacao.Faturado = faturado;
    }

    _ = await repository.UpdateAsync<ConsultaFaturacao, Guid>(faturacao);
  }
}
