using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.MotivoAltaService.Specifications
{
    public class MotivoAltaMatchDescricao : Specification<MotivoAlta>
    {
        public MotivoAltaMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }
            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}
