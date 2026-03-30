using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Specifications
{
    public class EvolucaoTratamentoSearchList : Specification<EvolucaoTratamento>
    {
        public EvolucaoTratamentoSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x =>
                    x.ObservacaoClinica != null &&
                    x.ObservacaoClinica.Contains(keyword));
            }

            _ = Query.OrderByDescending(x => x.CreatedOn); // default sort order

        }
    }
}
