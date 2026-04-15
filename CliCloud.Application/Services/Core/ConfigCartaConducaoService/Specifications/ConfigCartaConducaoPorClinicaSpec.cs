using Ardalis.Specification;
using CliCloud.Domain.Entities.Common.Configurations;

namespace CliCloud.Application.Services.Core.ConfigCartaConducaoService.Specifications
{
    public class ConfigCartaConducaoPorClinicaSpec : Specification<ConfigCartaConducao>
    {
        public ConfigCartaConducaoPorClinicaSpec(Guid clinicaId)
        {
            _ = Query.Where(x => x.ClinicaId == clinicaId);
        }
    }
}