using Ardalis.Specification;
using CliCloud.Domain.Entities.Prescricao;

namespace CliCloud.Application.Services.Prescricao.ReceitaMedicaService.Specifications
{
  public class ReceitaMedicaByIdWithLinhas : Specification<ReceitaMedica>
  {
    public ReceitaMedicaByIdWithLinhas(Guid id)
    {
      _ = Query.Where(x => x.Id == id)
        .Include(x => x.Linhas)
        .Include(x => x.Utente).ThenInclude(u => u!.Sexo)
        .Include(x => x.Medico).ThenInclude(m => m!.Especialidade)
        .Include(x => x.Clinica);
    }
  }
}
