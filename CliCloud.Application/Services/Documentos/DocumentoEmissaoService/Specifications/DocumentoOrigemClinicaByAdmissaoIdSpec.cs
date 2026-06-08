using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class DocumentoOrigemClinicaByAdmissaoIdSpec : Specification<DocumentoOrigemClinica>
{
    public DocumentoOrigemClinicaByAdmissaoIdSpec(Guid admissaoId)
    {
        _ = Query
            .Where(x => x.AdmissaoId == admissaoId && x.DeletedOn == null)
            .Include(x => x.Documento!)
                .ThenInclude(d => d.TipoDocumento);
    }
}
