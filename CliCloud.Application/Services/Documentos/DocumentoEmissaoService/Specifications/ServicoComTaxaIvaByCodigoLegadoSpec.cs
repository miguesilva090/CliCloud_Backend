using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class ServicoComTaxaIvaByCodigoLegadoSpec : Specification<Servico>
{
    public ServicoComTaxaIvaByCodigoLegadoSpec(string codigoLegado)
    {
        var c = codigoLegado.Trim();
        _ = Query
            .Include(x => x.TaxaIva)
            .Where(x => !x.Inativo && (x.Designacao == c || x.EAN == c));
    }
}