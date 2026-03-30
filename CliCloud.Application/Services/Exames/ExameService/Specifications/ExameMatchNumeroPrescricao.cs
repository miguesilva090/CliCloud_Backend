using Ardalis.Specification;
using CliCloud.Domain.Entities.Exames;

namespace CliCloud.Application.Services.Exames.ExameService.Specifications
{
  /// <summary>
  /// Encontra prescrição de exame por número de prescrição (para validação de duplicados).
  /// </summary>
  public class ExameMatchNumeroPrescricao : Specification<Exame>
  {
    public ExameMatchNumeroPrescricao(string? numeroPrescricao, Guid? excludeId = null)
    {
      if (string.IsNullOrWhiteSpace(numeroPrescricao))
        _ = Query.Where(_ => false);
      else
      {
        _ = Query.Where(x => x.NumeroPrescricao == numeroPrescricao);
        if (excludeId.HasValue)
          _ = Query.Where(x => x.Id != excludeId.Value);
      }
    }
  }
}
