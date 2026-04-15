using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Specifications;

public class SeparadorPersonalizadoSearchList : Specification<SeparadorPersonalizado>
{
    public SeparadorPersonalizadoSearchList(Guid clinicaId, Guid utilizadorId, string? keyword = "")
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId && x.UtilizadorId == utilizadorId);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            _ = Query.Where(x => x.NomeSeparador.Contains(keyword));
        }

        _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.NomeSeparador);
    }
}
