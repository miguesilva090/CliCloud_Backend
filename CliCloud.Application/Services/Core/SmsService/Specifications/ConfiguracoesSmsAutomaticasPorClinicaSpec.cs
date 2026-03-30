using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Sms;

namespace CliCloud.Application.Services.Core.SmsService.Specifications
{
    public class ConfiguracoesSmsAutomaticasPorClinicaSpec : Specification<ConfiguracaoSmsAutomatica>
    {
        public ConfiguracoesSmsAutomaticasPorClinicaSpec(Guid clinicaId)
        {
            _ = Query.Where(x => x.ClinicaId == clinicaId)
                .OrderBy(x => x.Codigo);
        }
    }
}