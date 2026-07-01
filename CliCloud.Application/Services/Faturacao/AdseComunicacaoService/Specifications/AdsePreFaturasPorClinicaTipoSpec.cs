using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Specifications;

public class AdsePreFaturasPorClinicaTipoSpec : Specification<AdsePreFatura>
{
    public AdsePreFaturasPorClinicaTipoSpec(Guid clinicaId, string tipoPreFatura) =>
        Query.Where(x =>
            x.ClinicaId == clinicaId
            && x.TipoPreFatura == tipoPreFatura
            && x.DeletedOn == null);
}
