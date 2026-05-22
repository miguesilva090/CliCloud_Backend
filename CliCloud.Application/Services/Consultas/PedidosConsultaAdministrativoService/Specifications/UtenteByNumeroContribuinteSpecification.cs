using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Specifications;

public sealed class UtenteByNumeroContribuinteSpecification : Specification<Utente>
{
  public UtenteByNumeroContribuinteSpecification(string nif)
  {
    string trimmed = nif.Trim();
    _ = Query.Where(x => x.NumeroContribuinte == trimmed);
  }
}
