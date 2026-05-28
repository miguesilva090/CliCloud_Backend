using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public class DocumentoByIdClinicaSpec : Specification<Documento>
    {
        public DocumentoByIdClinicaSpec(Guid id, Guid clinicaId)
        {
            _ = Query.Where(x => x.Id == id && x.ClinicaId == clinicaId);
        }
    }
}
