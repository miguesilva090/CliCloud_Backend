using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class ServicosConsultaByConsultaIdsSpec : Specification<ServicoConsulta>
    {
        public ServicosConsultaByConsultaIdsSpec(List<Guid> consultaIds)
        {
            _ = Query
                .Where(x => consultaIds.Contains(x.ConsultaId))
                .Include(x => x.Servico);
        }
    }
}
