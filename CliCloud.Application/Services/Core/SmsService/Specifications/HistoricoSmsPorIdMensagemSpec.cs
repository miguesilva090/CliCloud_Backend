using Ardalis.Specification;
using CliCloud.Domain.Entities.Core.Sms;

namespace CliCloud.Application.Services.Core.SmsService.Specifications
{
    public class HistoricoSmsPorIdMensagemSpec : Specification<HistoricoSms>
    {
        public HistoricoSmsPorIdMensagemSpec(Guid idMensagem)
        {
            _ = Query.Where(x => x.IdMensagem == idMensagem);
        }
    }
}