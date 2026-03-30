using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Utility.CodigoPostalService.Specifications
{
    public class CodigoPostalMatchCodigo : Specification<CodigoPostal>
    {
        public CodigoPostalMatchCodigo(string? codigo)
        {
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                _ = Query.Where(h => h.Codigo.Contains(codigo));
            }
            _ = Query.OrderBy(h => h.Codigo);
        }
    }
}
