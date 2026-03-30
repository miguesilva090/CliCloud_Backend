using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Sms;

namespace CliCloud.Application.Services.Core.SmsService.Specifications
{
    public class ConfiguracaoSmsAutomaticaPorClinicaSpec : Specification<ConfiguracaoSmsAutomatica>
    {
        public ConfiguracaoSmsAutomaticaPorClinicaSpec(Guid clinicaId, string codigo)
        {
            _  = Query.Where(x => x.ClinicaId == clinicaId && x.Codigo == codigo);
        }
    }
}