using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Specifications;

public class SeparadorPersonalizadoByClinicaAndIdSpec : Specification<SeparadorPersonalizado>
{
    public SeparadorPersonalizadoByClinicaAndIdSpec(Guid clinicaId, Guid utilizadorId, Guid id)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId && x.UtilizadorId == utilizadorId && x.Id == id);
    }
}
