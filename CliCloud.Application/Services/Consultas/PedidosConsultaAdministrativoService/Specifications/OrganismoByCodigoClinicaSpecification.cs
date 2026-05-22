using Ardalis.Specification;
using CliCloud.Domain.Entities.Organismos;

namespace CliCloud.Application.Services.Consultas.PedidosConsultaAdministrativoService.Specifications;

public sealed class OrganismoByCodigoClinicaSpecification : Specification<Organismo>
{
  public OrganismoByCodigoClinicaSpecification(string codigoClinica)
  {
    string key = codigoClinica.Trim();
    _ = Query.Where(x => x.CodigoClinica != null && x.CodigoClinica == key);
  }
}
