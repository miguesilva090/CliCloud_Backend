using Ardalis.Specification;
using CliCloud.Domain.Entities.Common.Configurations;

namespace CliCloud.Application.Services.Core.ConfigExamesSemPapelService.Specifications;

public class ConfigExamesSemPapelPorClinicaSpec : Specification<ConfigExamesSemPapel>
{
    public ConfigExamesSemPapelPorClinicaSpec(Guid clinicaId)
    {
        Query.Where(x => x.ClinicaId == clinicaId);
    }
}