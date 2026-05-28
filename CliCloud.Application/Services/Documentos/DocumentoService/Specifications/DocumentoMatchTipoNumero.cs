using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public class DocumentoMatchTipoNumero : Specification<Documento>
    {
        public DocumentoMatchTipoNumero(Guid tipoDocumentoId, int numeroDocumento, Guid clinicaId)
        {
            _ = Query.Where(x =>
                x.ClinicaId == clinicaId &&
                x.TipoDocumentoId == tipoDocumentoId &&
                x.NumeroDocumento == numeroDocumento);
        }
    }
}
