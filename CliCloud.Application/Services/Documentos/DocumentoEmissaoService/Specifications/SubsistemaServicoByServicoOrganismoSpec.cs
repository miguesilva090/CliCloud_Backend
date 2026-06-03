using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class SubsistemaServicoByServicoOrganismoSpec : Specification<SubsistemaServico>
{
    public SubsistemaServicoByServicoOrganismoSpec(Guid servicoId, Guid organismoId)
    {
        Query.Where(x =>
            x.ServicoId == servicoId &&
            x.OrganismoId == organismoId &&
            !x.Inativo);
    }
}
