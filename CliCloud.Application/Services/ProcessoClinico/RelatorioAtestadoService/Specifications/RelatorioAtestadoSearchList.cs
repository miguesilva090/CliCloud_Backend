using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.RelatorioAtestado;


namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.Specifications
{
    public class RelatorioAtestadoSearchList : Specification<RelatorioAtestado>
    {
        public RelatorioAtestadoSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Titulo.Contains(keyword));
            }

            _ = Query.Include(x => x.Medico);
            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
