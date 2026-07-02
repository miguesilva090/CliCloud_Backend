using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Specifications;

public class AdsePreFaturaCoPagamentosSpec : Specification<AdseCoPagamento>
{
    public AdsePreFaturaCoPagamentosSpec(Guid clinicaId, string tipoPreFatura, int numOrdem) =>
        Query.Where(x =>
            x.ClinicaId == clinicaId
            && x.TipoPreFatura == tipoPreFatura
            && x.NumOrdemPreFatura == numOrdem
            && x.DeletedOn == null);
}
