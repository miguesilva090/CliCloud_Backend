using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.Specifications
{
    // Mantido apenas para compatibilidade com o template; atualmente não é usado
    // porque HabitosEVicios não possui campo "Name".
    public class HabitosEViciosMatchName : Specification<HabitosEVicios>
    {
        public HabitosEViciosMatchName(string? name)
        {
            _ = Query.OrderBy(h => h.CreatedOn);
        }
    }
}
