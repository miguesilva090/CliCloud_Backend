using Ardalis.Specification;
using CliCloud.Application.Services.Sinistros.SinistradoService.DTOs;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class ConsultasBaseByUtenteSpec : Specification<Consulta, ConsultaServicoBaseRowDTO>
    {
        public ConsultasBaseByUtenteSpec(Guid utenteId)
        {
            _ = Query.Where(x => x.UtenteId == utenteId);
            _ = Query.OrderByDescending(x => x.Data ?? x.CreatedOn);
            _ = Query.Select(x => new ConsultaServicoBaseRowDTO
            {
                Id = x.Id,
                Data = x.Data,
                AdmissaoId = x.ConsultaMarcacaoId,
                TipoConsultaDesignacao = x.TipoConsultaItem != null ? x.TipoConsultaItem.Designacao : null
            });
        }
    }
}
