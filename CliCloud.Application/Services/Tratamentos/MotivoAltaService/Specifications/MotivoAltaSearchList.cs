using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;


namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService.Specifications
{
    public class MotivoAltaSearchList : Specification<MotivoAlta>
    {
        public MotivoAltaSearchList(string? keyword = "")
        {

            // filters
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            }

            _ = Query.OrderBy(x => x.Descricao); // default sort order

        }
    }
}
