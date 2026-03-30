using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.Specifications
{
    public class EstadoListaEsperaSearchList : Specification<EstadoListaEspera>
    {
        public EstadoListaEsperaSearchList(string keyword = "")
        {
            if (!string.IsNullOrWhiteSpace(keyword))
                _ = Query.Where(x => x.Descricao.Contains(keyword));
            _ = Query.OrderBy(x => x.Descricao);
        }
    }
}
