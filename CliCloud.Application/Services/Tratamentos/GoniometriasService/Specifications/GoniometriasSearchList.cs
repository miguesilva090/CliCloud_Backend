using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.GoniometriasService.Specifications
{
    public class GoniometriasSearchList : Specification<Goniometrias>
    {
        public GoniometriasSearchList(string? keyword = "")
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
