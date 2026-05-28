using Ardalis.Specification;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoService.Specifications
{
    public class DocumentoSearchList : Specification<Documento>
    {
        public DocumentoSearchList(string? keyword = "", Guid? clinicaId = null)
        {
            if (clinicaId.HasValue)
            {
                _ = Query.Where(x => x.ClinicaId == clinicaId.Value);
            }

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.NomeCliente != null && x.NomeCliente.Contains(keyword) || 
                                   x.NumeroContribuinteCliente != null && x.NumeroContribuinteCliente.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order
        }
    }
}
