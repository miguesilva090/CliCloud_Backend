using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService.Specifications
{
    public class PrioridadeMatchDescricao : Specification<Prioridade>
    {
        public PrioridadeMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
