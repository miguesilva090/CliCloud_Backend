using Ardalis.Specification;
using CliCloud.Domain.Entities.Consultas;

namespace CliCloud.Application.Services.Consultas.MotivoConsultaService.Specifications
{
  public class MotivoConsultaMatchDesignacao : Specification<MotivoConsulta>
  {
    public MotivoConsultaMatchDesignacao(string designacao)
    {
      _ = Query.Where(x => x.Designacao == designacao);
    }
  }
}
