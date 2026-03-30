using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.Specifications
{
    public class EstadosDentariosMatchCodigo : Specification<EstadosDentarios>
    {
        public EstadosDentariosMatchCodigo(string codigo)
        {
            Query.Where(h => h.Codigo == codigo);
        }
    }
}
