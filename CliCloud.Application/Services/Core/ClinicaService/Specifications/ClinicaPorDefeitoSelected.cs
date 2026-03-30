using Ardalis.Specification;
using CliCloud.Domain.Entities.Core;
using System.Linq;

namespace CliCloud.Application.Services.Core.ClinicaService.Specifications
{
  /// <summary>
  /// Seleciona a clínica "por defeito" (legado: EMPRESAS.pordefeito).
  /// </summary>
  public class ClinicaPorDefeitoSelected : Specification<Clinica>
  {
    public ClinicaPorDefeitoSelected()
    {
      // Por defeito é esperado ser único; ainda assim garantimos que apenas levamos 1.
      Query.Where(c => c.PorDefeito);
      Query.OrderByDescending(c => c.CreatedOn);
      Query.Take(1);
    }
  }
}

