using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.Specifications
{
  public class TratamentoByListaEsperaTratamentoIdSpec : Specification<Tratamento>
  {
    public TratamentoByListaEsperaTratamentoIdSpec(Guid listaEsperaTratamentoId)
    {
      _ = Query.Where(x =>
        x.ListaEsperaTratamentoId == listaEsperaTratamentoId &&
        x.DeletedOn == null);
    }
  }
}
