using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.ModeloAparelhoService.Specifications
{
    public class ModeloAparelhoSearchList : Specification<ModeloAparelho>
    {
        public ModeloAparelhoSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Designacao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Designacao); // default sort order

        }
    }
}
