using Ardalis.Specification;
using CliCloud.Domain.Entities.Atestados;

namespace CliCloud.Application.Services.Atestados.AtestadoService.Specifications;

public class AtestadoPendentesForSpmsSpec : Specification<Atestado>
{
  public AtestadoPendentesForSpmsSpec(Guid clinicaId)
  {
    _ = Query
      .Where(x => x.ClinicaId == clinicaId)
      .Where(x => x.EstadoEnvio == 0)
      .Include(x => x.CodigoPostal)
      .Include(x => x.Utente)
        .ThenInclude(u => u!.Sexo)
      .Include(x => x.Utente)
        .ThenInclude(u => u!.Pais)
      .Include(x => x.Utente)
        .ThenInclude(u => u!.CodigoPostal)
      .Include(x => x.Utente)
        .ThenInclude(u => u!.Rua)
          .ThenInclude(r => r!.CodigoPostal)
      .Include(x => x.Utente)
        .ThenInclude(u => u!.Freguesia)
          .ThenInclude(f => f!.Concelho)
            .ThenInclude(c => c!.Distrito)
      .Include(x => x.Utente)
        .ThenInclude(u => u!.Concelho)
          .ThenInclude(c => c!.Distrito)
      .Include(x => x.Utente)
        .ThenInclude(u => u!.Distrito)
      .Include(x => x.Medico)
      .Include(x => x.Clinica)
      .Include(x => x.Categorias)
        .ThenInclude(c => c.CartaConducao)
      .Include(x => x.Restricoes)
        .ThenInclude(r => r.CartaConducaoRestricao)
      .Include(x => x.RestricoesAnteriores)
        .ThenInclude(r => r.CartaConducaoRestricao)
      .OrderBy(x => x.CreatedOn);
  }
}
