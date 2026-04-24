using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
  /// <summary>
  /// Uma clínica quando não há PorDefeito=true: prioriza flag PorDefeito e depois a mais recente.
  /// </summary>
  public class ClinicaFallbackParaContextoAtualSpec : Specification<Clinica>
  {
    public ClinicaFallbackParaContextoAtualSpec()
    {
      _ = Query.OrderByDescending(c => c.PorDefeito).ThenByDescending(c => c.CreatedOn);
      Query.Take(1);
    }
  }
}
