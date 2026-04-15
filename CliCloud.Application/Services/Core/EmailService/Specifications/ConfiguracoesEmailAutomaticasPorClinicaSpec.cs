using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Email;

namespace CliCloud.Application.Services.Core.EmailService.Specifications;

public class ConfiguracoesEmailAutomaticasPorClinicaSpec : Specification<ConfiguracaoEmailAutomatica>
{
    public ConfiguracoesEmailAutomaticasPorClinicaSpec( Guid clinicaId ) 
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId).OrderBy(x => x.Codigo);
    }
}