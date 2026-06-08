using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.Specifications;

public sealed class DocumentoLinhasReciboUtentePorAdmissaoSpec : Specification<DocumentoLinha>
{
    public DocumentoLinhasReciboUtentePorAdmissaoSpec(Guid admissaoId)
    {
        _ = Query
            .Where(l =>
                l.DeletedOn == null
                && l.AdmissaoServicoId != null
                && l.AdmissaoServico!.AdmissaoId == admissaoId
                && l.Documento != null
                && l.Documento.DeletedOn == null
                && !l.Documento.Anulado
                && l.Documento.EstadoDocumento == EstadoDocumento.Emitido)
            .Include(l => l.Documento!)
                .ThenInclude(d => d.TipoDocumento)
            .Include(l => l.AdmissaoServico);
    }
}
