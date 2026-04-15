using Ardalis.Specification;
using CliCloud.Domain.Entities.ProcessoClinico.SeparadoresPersonalizados;

namespace CliCloud.Application.Services.ProcessoClinico.SeparadorPersonalizadoService.Specifications;

public class SeparadorPersonalizadoMatchNome : Specification<SeparadorPersonalizado>
{
    public SeparadorPersonalizadoMatchNome(
        Guid clinicaId,
        Guid utilizadorId,
        string nomeSeparador,
        Guid? excludeId = null
    )
    {
        if (!string.IsNullOrWhiteSpace(nomeSeparador))
        {
            if (excludeId.HasValue)
            {
                _ = Query.Where(x =>
                    x.ClinicaId == clinicaId
                    && x.UtilizadorId == utilizadorId
                    && x.NomeSeparador == nomeSeparador
                    && x.Id != excludeId.Value
                );
            }
            else
            {
                _ = Query.Where(x =>
                    x.ClinicaId == clinicaId
                    && x.UtilizadorId == utilizadorId
                    && x.NomeSeparador == nomeSeparador
                );
            }
        }
    }
}
