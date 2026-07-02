using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Specifications;

public class AdsePreFaturaPorIdClinicaSpec : Specification<AdsePreFatura>
{
    public AdsePreFaturaPorIdClinicaSpec(Guid clinicaId, Guid preFaturaId) =>
        Query.Where(x =>
            x.ClinicaId == clinicaId
            && x.Id == preFaturaId
            && x.DeletedOn == null);
}
