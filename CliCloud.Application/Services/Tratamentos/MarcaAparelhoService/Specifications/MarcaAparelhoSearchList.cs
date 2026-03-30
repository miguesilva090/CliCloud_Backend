using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.Specifications
{
    public class MarcaAparelhoSearchList : Specification<MarcaAparelho>
    {
        public MarcaAparelhoSearchList(string? keyword = "")
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
