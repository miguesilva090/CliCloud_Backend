using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Documentos;

namespace CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.Specifications
{
    public class DocumentoFichaClinicaByUtenteSpec : Specification<DocumentosFichaClinica>
    {
        public DocumentoFichaClinicaByUtenteSpec(Guid utenteId, string? categoria = "Clinico")
        {
            Query.Where(d => d.UtenteId == utenteId);

            if (!string.IsNullOrWhiteSpace(categoria) &&
                Enum.TryParse<DocumentoFichaClinicaCategoria>(categoria, ignoreCase: true, out var cat))
            {
                Query.Where(d => d.Categoria == cat);
            }

            Query.OrderByDescending(d => d.CreatedOn);
        }
    }
}

