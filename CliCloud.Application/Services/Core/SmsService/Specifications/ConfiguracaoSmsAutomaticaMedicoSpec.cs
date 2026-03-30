using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Sms;

namespace CliCloud.Application.Services.Core.SmsService.Specifications
{
    public class ConfiguracaoSmsAutomaticaMedicoSpec : Specification<ConfiguracaoSmsAutomaticaMedico>
    {
        public ConfiguracaoSmsAutomaticaMedicoSpec(Guid clinicaId, string codigoConfiguracao)
        {
            _ = Query.Where( x => x.ClinicaId == clinicaId && x.CodigoConfiguracao == codigoConfiguracao)
                .OrderBy(x => x.CodigoMedico);
        }
    }
}