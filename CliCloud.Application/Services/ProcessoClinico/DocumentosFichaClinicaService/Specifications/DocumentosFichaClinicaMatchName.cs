using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Documentos;

namespace CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.Specifications
{
    public class DocumentosFichaClinicaMatchName : Specification<DocumentosFichaClinica>
    {
        public DocumentosFichaClinicaMatchName(Guid? utenteId, string? categoria, string? tipo)
        {
            if (utenteId.HasValue)
            {
                _ = Query.Where(h => h.UtenteId == utenteId.Value);
            }
            if (!string.IsNullOrWhiteSpace(categoria) &&
                Enum.TryParse<DocumentoFichaClinicaCategoria>(categoria, true, out var catEnum))
            {
                _ = Query.Where(h => h.Categoria == catEnum);
            }

            if (!string.IsNullOrWhiteSpace(tipo) &&
                Enum.TryParse<DocumentoFichaClinicaTipo>(tipo, true, out var tipoEnum))
            {
                _ = Query.Where(h => h.Tipo == tipoEnum);
            }
            _ = Query.OrderBy(h => h.CreatedOn);
        }
    }
}
