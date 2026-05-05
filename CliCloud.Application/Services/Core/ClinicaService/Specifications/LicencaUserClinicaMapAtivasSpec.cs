using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
    public class LicencaUserClinicaMapAtivasSpec : Specification<LicencaUserClinicaMap>
    {
      public LicencaUserClinicaMapAtivasSpec(Guid clientIdLicencas, Guid userIdLicencas)
      {
        Query.Where(x =>
          x.ClienteIdLicencas == clientIdLicencas &&
          x.UserIdLicencas == userIdLicencas &&
          x.Ativo &&
          x.DeletedOn == null
        );

        Query.OrderByDescending(x => x.IsDefault).ThenByDescending(x => x.CreatedOn);
      }
    }
}