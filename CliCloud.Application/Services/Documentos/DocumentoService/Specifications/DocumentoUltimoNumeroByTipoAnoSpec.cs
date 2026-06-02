using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public sealed class DocumentoUltimoNumeroByTipoAnoSpec : Specification<Documento>
    {
        public DocumentoUltimoNumeroByTipoAnoSpec(Guid clinicaId, Guid tipoDocumentoId, int anoFiscal)
        {
            _ = Query
                .Where(x =>
                    x.ClinicaId == clinicaId &&
                    x.TipoDocumentoId == tipoDocumentoId &&
                    x.AnoFiscal == anoFiscal
                )
                .OrderByDescending(x => x.NumeroDocumento)
                .Take(1);
        }
    }
}
