using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.TipoDocumentoService.Specifications
{
    public class TipoDocumentoByIdClinicaSpec : Specification<TipoDocumento>
    {
        public TipoDocumentoByIdClinicaSpec(Guid id, Guid clinicaId = default)
        {
            _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId);
        }
    }
}
