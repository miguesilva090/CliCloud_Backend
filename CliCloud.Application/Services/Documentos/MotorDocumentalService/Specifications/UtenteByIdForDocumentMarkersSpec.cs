using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Documentos.MotorDocumentalService.Specifications;

public sealed class UtenteByIdForDocumentMarkersSpec : Specification<Utente>
{
    public UtenteByIdForDocumentMarkersSpec()
    {
        _ = Query
            .Include(x => x.Rua)
            .ThenInclude(x => x.CodigoPostal)
            .Include(x => x.CodigoPostal)
            .Include(x => x.EntidadeContactos);
    }
}
