using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Email;

namespace CliCloud.Application.Services.Core.EmailService.Specifications;

public class ConfiguracaoEmailPorClinicaSpec : Specification<ConfiguracaoEmail>
{
    public ConfiguracaoEmailPorClinicaSpec( Guid clinicaId )
    {
        _ = Query.Where(x => x.ClinicaId == clinicaId);
    }
}