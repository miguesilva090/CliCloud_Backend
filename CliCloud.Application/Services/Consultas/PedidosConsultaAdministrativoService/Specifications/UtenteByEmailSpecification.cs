using Ardalis.Specification;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Specifications;

public sealed class UtenteByEmailSpecification : Specification<Utente>
{
  public UtenteByEmailSpecification(string email)
  {
    string trimmed = email.Trim();
    _ = Query.Where(x => x.Email != null && x.Email == trimmed);
  }
}
