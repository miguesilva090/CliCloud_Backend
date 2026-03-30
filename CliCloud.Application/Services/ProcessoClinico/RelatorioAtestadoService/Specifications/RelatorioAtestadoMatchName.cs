using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Specifications
{
    public class RelatorioAtestadoMatchName : Specification<RelatorioAtestado>
    {
        public RelatorioAtestadoMatchName(string? titulo)
        {
            if (!string.IsNullOrWhiteSpace(titulo))
            {
                _ = Query.Where(h => h.Titulo == titulo);
            }
            _ = Query.OrderBy(h => h.Titulo);
        }
    }
}
