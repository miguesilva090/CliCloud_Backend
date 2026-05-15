using Ardalis.Specification;
using CliCloud.Domain.Entities.Organismos;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.Specifications;

/// <summary>Organismos cujo código ULS coincide com <see cref="CliCloud.Domain.Entities.Credenciais.LoteDirect.CodigoOrganismo"/>.</summary>
public sealed class OrganismosByCodigoULSNovaSpec : Specification<Organismo>
{
    public OrganismosByCodigoULSNovaSpec(IReadOnlyCollection<int> codigosUls)
    {
        if (codigosUls is not { Count: > 0 })
        {
            Query.Where(_ => false);
            return;
        }

        // Materializar e usar EF.Constant para gerar `IN (...)` em vez de OPENJSON(... WITH ...),
        // que no SQL Server pode falhar (erro 156) com split queries / certos batches.
        List<int> valores = codigosUls.Distinct().ToList();
        Query.Where(o => o.CodigoULSNova.HasValue && EF.Constant(valores).Contains(o.CodigoULSNova.Value));
    }
}
