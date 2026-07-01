using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Specifications;

public class AdsePreFaturasPorEstadoSpec : Specification<AdsePreFatura>
{
    public AdsePreFaturasPorEstadoSpec(Guid clinicaId, string tipoPreFatura, int estado) =>
        Query.Where(x =>
            x.ClinicaId == clinicaId
            && x.TipoPreFatura == tipoPreFatura
            && x.Estado == estado
            && x.DeletedOn == null)
            .OrderBy(x => x.NumOrdem);
}
