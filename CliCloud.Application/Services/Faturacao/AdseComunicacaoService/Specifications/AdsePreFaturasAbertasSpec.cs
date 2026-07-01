using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Specifications;

public class AdsePreFaturasAbertasSpec : Specification<AdsePreFatura>
{
    public AdsePreFaturasAbertasSpec(Guid clinicaId, string tipoPreFatura) =>
        Query.Where(x =>
            x.ClinicaId == clinicaId
            && x.TipoPreFatura == tipoPreFatura
            && x.DeletedOn == null
            && x.Estado < AdseEstados.PreFaturaFechada)
            .OrderBy(x => x.NumOrdem);
}
