using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications;

public sealed class ConsultaByIdWithServicosSpec : Specification<Consulta>
{
    public ConsultaByIdWithServicosSpec(Guid consultaId)
    {
        _ = Query
            .Where(x => x.Id == consultaId && x.DeletedOn == null)
            .Include(x => x.Utente)
            .Include(x => x.Organismo)
            .Include(x => x.Servicos)
                .ThenInclude(x => x.Servico)
                .ThenInclude(x => x!.TaxaIva);
    }
}