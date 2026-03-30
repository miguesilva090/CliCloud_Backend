using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoService.Specifications
{
  /// <summary>
  /// Obtém a última evolução de tratamento (por CreatedOn desc) para um determinado TratamentoId.
  /// Usado, por exemplo, para sincronizar o estado de Alta quando o tratamento é dado como alta.
  /// </summary>
  public class EvolucaoTratamentoByTratamentoIdUltima : Specification<EvolucaoTratamento>
  {
    public EvolucaoTratamentoByTratamentoIdUltima(Guid tratamentoId)
    {
      Query.Where(x => x.TratamentoId == tratamentoId);
      Query.OrderByDescending(x => x.CreatedOn);
      Query.Take(1);
    }
  }
}

