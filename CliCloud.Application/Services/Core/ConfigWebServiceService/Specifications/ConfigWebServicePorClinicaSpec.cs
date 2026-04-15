using Ardalis.Specification;
using CliCloud.Domain.Entities.Common.Configurations;

namespace CliCloud.Application.Services.Core.ConfigWebServiceService.Specifications;

public class ConfigWebServicePorClinicaSpec : Specification<ConfigWebService>
{
    public ConfigWebServicePorClinicaSpec(Guid clinicaId)
    {
        Query.Where(x => x.ClinicaId == clinicaId);
    }
}