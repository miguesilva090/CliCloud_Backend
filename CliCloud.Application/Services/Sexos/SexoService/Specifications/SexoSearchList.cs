using Ardalis.Specification;
using CliCloud.Domain.Entities.Sexos;

namespace CliCloud.Application.Services.Sexos.SexoService.Specifications
{
    public class SexoSearchList : Specification<Sexo>
    {
        public SexoSearchList(string? keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));

            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}

