using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Sinistros.SinistradoService.Specifications
{
    public class ServicosConsultaByConsultaIdSpec : Specification<ServicoConsulta>
    {
        public ServicosConsultaByConsultaIdSpec(Guid consultaId)
        {
            _ = Query
                .Where(x => x.ConsultaId == consultaId)
                .Include(x => x.Servico);
        }
    }
}
