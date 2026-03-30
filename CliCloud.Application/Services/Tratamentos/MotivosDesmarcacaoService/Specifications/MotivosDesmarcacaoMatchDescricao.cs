using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.MotivosDesmarcacaoService.Specifications
{
    public class MotivosDesmarcacaoMatchDescricao : Specification<MotivosDesmarcacao>
    {
        public MotivosDesmarcacaoMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }
            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
