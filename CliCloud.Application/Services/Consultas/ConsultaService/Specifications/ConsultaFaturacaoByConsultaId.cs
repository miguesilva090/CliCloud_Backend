using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.ConsultaService.Specifications
{
  public class ConsultaFaturacaoByConsultaId : Specification<ConsultaFaturacao>
  {
    public ConsultaFaturacaoByConsultaId(Guid consultaId)
    {
      Query.Where(x => x.ConsultaId == consultaId);
    }
  }
}

