using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.Odontologia;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.Specifications
{
    public class TiposTratamentoDentarioMatchCodigo : Specification<TipoTratamentoDentario>
    {
        public TiposTratamentoDentarioMatchCodigo(string codigo)
        {
            _ = Query.Where(h => h.Codigo == codigo);
        }
    }
}
