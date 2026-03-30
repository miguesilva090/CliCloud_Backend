using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Utentes.UtenteService.Specifications
{
  public class UtenteMatchNumeroUtente : Specification<Utente>
  {
    public UtenteMatchNumeroUtente(string? numeroUtente)
    {
      _ = Query.Include(x => x.Rua)
        .ThenInclude(x => x.Freguesia)
        .ThenInclude(x => x.Concelho)
        .ThenInclude(x => x.Distrito);

      _ = Query.Include(x => x.Rua)
        .ThenInclude(x => x.CodigoPostal);

      _ = Query.Include(x => x.EntidadeContactos);

      if(!string.IsNullOrWhiteSpace(numeroUtente))
      {
        _ = Query.Where(x => x.NumeroUtente == numeroUtente);
      }
    }
  }
}
