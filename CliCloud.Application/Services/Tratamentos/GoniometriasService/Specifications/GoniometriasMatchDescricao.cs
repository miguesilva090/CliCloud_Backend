using Ardalis.Specification;
using CliCloud.Domain.Entities.Tratamentos;

namespace CliCloud.Application.Services.Tratamentos.GoniometriasService.Specifications
{
    public class GoniometriasMatchDescricao : Specification<Goniometrias>
    {
        public GoniometriasMatchDescricao(string? descricao)
        {
            if (!string.IsNullOrWhiteSpace(descricao))
            {
                _ = Query.Where(h => h.Descricao == descricao);
            }

            _ = Query.OrderBy(h => h.Descricao);
        }
    }
}

