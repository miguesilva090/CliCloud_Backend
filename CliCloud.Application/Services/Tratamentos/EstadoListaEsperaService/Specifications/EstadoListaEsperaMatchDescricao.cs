using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.EstadoListaEsperaService.Specifications
{
    public class EstadoListaEsperaMatchDescricao : Specification<EstadoListaEspera>
    {
        public EstadoListaEsperaMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
