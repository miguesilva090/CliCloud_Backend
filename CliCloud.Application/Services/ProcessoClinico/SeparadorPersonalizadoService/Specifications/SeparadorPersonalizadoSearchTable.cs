using Ardalis.Specification;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Specifications;

public class SeparadorPersonalizadoSearchTable : Specification<SeparadorPersonalizado>
{
    public SeparadorPersonalizadoSearchTable(
        Guid clinicaId,
        Guid utilizadorId,
        string? keyword = "",
        string? dynamicOrder = ""
    )
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId && x.UtilizadorId == utilizadorId);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            _ = Query.Where(x => x.NomeSeparador.Contains(keyword));
        }

        if (string.IsNullOrEmpty(dynamicOrder))
        {
            _ = Query.OrderBy(x => x.Ordem).ThenBy(x => x.NomeSeparador);
        }
        else
        {
            _ = Query.OrderBy(dynamicOrder);
        }
    }
}
