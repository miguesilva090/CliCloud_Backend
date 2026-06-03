using Ardalis.Specification;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class OrganismoComMoradaByIdSpec : Specification<Organismo>
{
    public OrganismoComMoradaByIdSpec(Guid organismoId)
    {
        _ = Query
            .Where(x => x.Id == organismoId)
            .Include(x => x.CodigoPostal)
            .Include(x => x.Rua!)
                .ThenInclude(r => r.CodigoPostal);
    }

}