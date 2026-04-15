using Ardalis.Specification;
using CliCloud.Domain.Entities.Atestados;

namespace CliCloud.Application.Services.Atestados.AtestadoService.Specifications;

public class AtestadoByIdForSmpsSpec : Specification<Atestado>
{
    public AtestadoByIdForSmpsSpec(Guid id)
    {
        _ = Query
            .Where(x => x.Id == id)
            .Include(x => x.Utente)
                .ThenInclude(u => u!.Sexo)
            .Include(x => x.Medico)
            .Include(x => x.Clinica)
            .Include(x => x.Categorias)
                .ThenInclude(c => c.CartaConducao)
            .Include(x => x.Restricoes)
                .ThenInclude(r => r.CartaConducaoRestricao)
            .Include(x => x.RestricoesAnteriores)
                .ThenInclude(r => r.CartaConducaoRestricao);
    }
}