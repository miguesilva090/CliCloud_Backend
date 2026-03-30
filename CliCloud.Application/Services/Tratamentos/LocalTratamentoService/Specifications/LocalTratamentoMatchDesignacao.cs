using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.LocalTratamentoService.Specifications
{
    public class LocalTratamentoMatchDesignacao : Specification<LocalTratamento>
    {
        public LocalTratamentoMatchDesignacao(string designacao)
        {
            _ = Query.Where(x => x.Designacao == designacao);
        }
    }
}
