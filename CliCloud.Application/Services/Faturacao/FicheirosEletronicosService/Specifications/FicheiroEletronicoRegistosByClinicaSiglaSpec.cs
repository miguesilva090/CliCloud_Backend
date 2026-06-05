using Ardalis.Specification;
using CliCloud.Domain.Entities.Faturacao;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.Specifications;

public sealed class FicheiroEletronicoRegistosByClinicaSiglaSpec : Specification<FicheiroEletronicoRegisto>
{
    public FicheiroEletronicoRegistosByClinicaSiglaSpec(Guid clinicaId, FicheiroEletronicoSigla sigla)
    {
        _ = Query
            .Where(x => x.ClinicaId == clinicaId && x.Sigla == sigla && x.DeletedOn == null)
            .OrderByDescending(x => x.DataGeracao);
    }
}