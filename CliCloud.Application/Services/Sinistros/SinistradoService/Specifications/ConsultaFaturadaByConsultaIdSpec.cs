using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class ConsultaFaturadaByConsultaIdSpec : Specification<ConsultaFaturacao>
    {
        public ConsultaFaturadaByConsultaIdSpec(Guid consultaId)
        {
            _ = Query.Where(x => x.ConsultaId == consultaId && x.Faturado);
        }
    }
}
