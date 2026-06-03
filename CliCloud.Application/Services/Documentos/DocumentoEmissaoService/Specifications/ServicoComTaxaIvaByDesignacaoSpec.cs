using Ardalis.Specification;
using CliCloud.Domain.Entities.Servicos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService.Specifications;

public sealed class ServicoComTaxaIvaByDesignacaoSpec : Specification<Servico>
{
    public ServicoComTaxaIvaByDesignacaoSpec(string designacao)
    {
        Query.Where(x => x.Designacao == designacao)
            .Include(x => x.TaxaIva);
    }
}
