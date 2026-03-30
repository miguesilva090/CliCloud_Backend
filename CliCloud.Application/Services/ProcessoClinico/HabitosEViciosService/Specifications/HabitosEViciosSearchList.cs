using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico;

namespace CliCloud.Application.Services.ProcessoClinico.HabitosEViciosService.Specifications
{
    public class HabitosEViciosSearchList : Specification<HabitosEVicios>
    {
        public HabitosEViciosSearchList(string? keyword = "")
        {
            // Neste momento não existe um campo "Name" em HabitosEVicios;
            // se quisermos filtrar por utente ou outro campo, podemos ajustar aqui.
            _ = Query.OrderByDescending(x => x.CreatedOn);
        }
    }
}
