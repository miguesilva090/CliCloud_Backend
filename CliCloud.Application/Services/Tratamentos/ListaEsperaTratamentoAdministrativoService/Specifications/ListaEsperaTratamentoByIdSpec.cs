using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.Specifications;

public sealed class ListaEsperaTratamentoByIdSpec : Specification<ListaEsperaTratamento>
{
    public ListaEsperaTratamentoByIdSpec(Guid id)
    {
        _ = Query
            .Include(x => x.Utente)
            .Include(x => x.Medico)
            .Include(x => x.Organismo)
            .Include(x => x.Prioridade)
            .Include(x => x.EstadoListaEspera)
            .Include(x => x.LocalTratamento)
            .Include(x => x.Patologia)
            .Include(x => x.Servicos.Where(s => s.DeletedOn == null))
            .ThenInclude(s => s.Servico)
            .Where(x => x.Id == id && x.DeletedOn == null);
    }
}
