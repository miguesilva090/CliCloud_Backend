using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Email;

namespace CliCloud.Application.Services.Core.EmailService.Specifications;

public class ConfiguracaoEmailAutomaticaPorClinicaSpec : Specification<ConfiguracaoEmailAutomatica>
{
    public ConfiguracaoEmailAutomaticaPorClinicaSpec( Guid clinicaId, string codigo)
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId && x.Codigo == codigo);
    }
}