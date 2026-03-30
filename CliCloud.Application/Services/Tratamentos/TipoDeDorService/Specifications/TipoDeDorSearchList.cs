using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.TipoDeDorService.Specifications
{
    public class TipoDeDorSearchList : Specification<TipoDeDor>
    {
        public TipoDeDorSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao);

        }
    }
}
