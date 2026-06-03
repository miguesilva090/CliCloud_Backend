using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;
using Microsoft.EntityFrameworkCore;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

/// <summary>
/// Resolve serviço por GUID completo ou por prefixo (linhas sinistro truncam CodigoServico a 30 chars).
/// </summary>
public sealed class ServicoComTaxaIvaByCodigoSpec : Specification<Servico>
{
    public ServicoComTaxaIvaByCodigoSpec(string codigo)
    {
        var trimmed = codigo.Trim();
        Query.Include(x => x.TaxaIva);

        if (Guid.TryParse(trimmed, out Guid servicoId))
        {
            Query.Where(x => x.Id == servicoId);
            return;
        }

        Query.Where(x => EF.Functions.Like(x.Id.ToString(), trimmed + "%"));
    }
}
