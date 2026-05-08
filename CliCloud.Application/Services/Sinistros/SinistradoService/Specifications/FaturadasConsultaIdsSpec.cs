using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class FaturadasConsultaIdsSpec : Specification<ConsultaFaturacao>
    {
        public FaturadasConsultaIdsSpec(List<Guid> consultaIds)
        {
            _ = Query
                .Where(x => x.ConsultaId.HasValue && consultaIds.Contains(x.ConsultaId.Value) && x.Faturado);
        }
    }
}
