using Ardalis.Specification;
using CliCloud.Application.Services.Sinistros.SinistradoService.DTOs;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class TratamentosBaseByUtenteSpec : Specification<Tratamento, TratamentoServicoBaseRowDTO>
    {
        public TratamentosBaseByUtenteSpec(Guid utenteId)
        {
            _ = Query.Where(x => x.UtenteId == utenteId && (x.Faturado ?? 0) == 0);
            _ = Query.OrderByDescending(x => x.DataInic ?? x.Data ?? x.CreatedOn);
            _ = Query.Select(x => new TratamentoServicoBaseRowDTO
            {
                Id = x.Id,
                DataServico = x.DataInic ?? x.Data,
                Designacao = x.Designacao ?? x.NomePatologia
            });
        }
    }
}
