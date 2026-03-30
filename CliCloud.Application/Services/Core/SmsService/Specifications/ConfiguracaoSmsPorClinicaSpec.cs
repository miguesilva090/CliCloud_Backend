using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Sms;

namespace CliCloud.Application.Services.Core.SmsService.Specifications
{
    public class ConfiguracaoSmsPorClinicaSpec : Specification<ConfiguracaoSms>
    {
        public ConfiguracaoSmsPorClinicaSpec(Guid clinicaId)
        {
            _ = Query.Where(x => x.ClinicaId == clinicaId);
        }
    }
}