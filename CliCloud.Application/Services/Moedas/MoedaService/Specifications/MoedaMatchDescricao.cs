using Ardalis.Specification;
using CliCloud.Domain.Entities.Moedas;

namespace CliCloud.Application.Services.Moedas.MoedaService.Specifications
{
    public class MoedaMatchDescricao : Specification<Moeda>
    {
        public MoedaMatchDescricao(string descricao)
        {
            _ = Query.Where(x => x.Descricao == descricao);
        }
    }
}
