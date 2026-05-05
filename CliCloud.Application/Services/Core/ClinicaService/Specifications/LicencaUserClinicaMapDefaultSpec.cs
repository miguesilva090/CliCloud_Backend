using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
    public class LicencaUserClinicaMapDefaultSpec : Specification<LicencaUserClinicaMap>
    {
      public LicencaUserClinicaMapDefaultSpec(Guid clientIdLicencas, Guid userIdLicencas)
      {
        Query.Where(x =>
          x.ClienteIdLicencas == clientIdLicencas &&
          x.UserIdLicencas == userIdLicencas &&
          x.Ativo &&
          x.IsDefault &&
          x.DeletedOn == null);

        Query.Take(1);
      }
    }
}