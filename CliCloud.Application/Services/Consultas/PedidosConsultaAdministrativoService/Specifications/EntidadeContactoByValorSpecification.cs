using Ardalis.Specification;
using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Specifications;

public sealed class EntidadeContactoByValorSpecification : Specification<EntidadeContacto>
{
  public EntidadeContactoByValorSpecification(string valor)
  {
    string trimmed = valor.Trim();
    _ = Query.Where(x => x.Valor != null && x.Valor.Contains(trimmed));
  }
}
